from flask import Flask, request, jsonify
from flask_cors import CORS
from flask_wtf import FlaskForm
from flask_wtf.file import FileField, FileRequired
from PIL import Image
from sklearn.cluster import KMeans

import cv2
import numpy as np
import matplotlib.pyplot as plt
import tempfile
import os


app = Flask(__name__)
CORS(app)
app.config['SECRET_KEY'] = 'your_secret_key'

class UploadForm(FlaskForm):
    file = FileField(validators=[FileRequired()])

def rgb_to_hex(rgb):
    return '#{:02x}{:02x}{:02x}'.format(int(rgb[2]), int(rgb[1]), int(rgb[0]))

# def extract_colors(image_path, num_colors=10):

#     pixels = image_np.reshape((-1, 3))
#     kmeans = KMeans(n_clusters=num_colors, random_state=0).fit(pixels)
#     colors = kmeans.cluster_centers_
#     counts = np.bincount(kmeans.labels_)

#     color_counts = sorted(zip(colors, counts), key=lambda x: x[1], reverse=True)
#     hex_colors = [(rgb_to_hex(color), count) for color, count in color_counts]

#     return hex_colors


def extract_colors(image, num_colors=9):
    # Check if the image is mostly of one color
    if is_mostly_one_color(image):
        # Return the dominant color repeated to match the requested number of colors
        dominant_color = np.mean(image, axis=(0, 1))
        return [rgb_to_hex(dominant_color)] * num_colors

    # Resize image for faster processing
    resized_image = cv2.resize(image, (500, 500), interpolation=cv2.INTER_AREA)

    # Convert image to 2D array of pixels
    pixels = resized_image.reshape((-1, 3))

    # Apply KMeans clustering
    kmeans = KMeans(n_clusters=num_colors)
    kmeans.fit(pixels)
    
    # Get the cluster centers (the dominant colors)
    colors = kmeans.cluster_centers_

    # Count the number of pixels in each cluster
    labels = kmeans.labels_
    label_counts = np.bincount(labels)

    # Sort colors by count in descending order
    sorted_indices = np.argsort(label_counts)[::-1]
    sorted_colors = colors[sorted_indices]

    # Convert RGB to HEX
    hex_colors = [rgb_to_hex(color) for color in sorted_colors]

    return hex_colors[:num_colors]

def extract_colors1(image, num_colors=9):
    # Check if the image is mostly of one color
    if is_mostly_one_color(image):
        # Return the dominant color repeated to match the requested number of colors
        dominant_color = np.mean(image, axis=(0, 1))
        return [(rgb_to_hex(dominant_color), 1.0)] * num_colors

    # Resize image for faster processing
    resized_image = cv2.resize(image, (500, 500), interpolation=cv2.INTER_AREA)

    # Convert image to 2D array of pixels
    pixels = resized_image.reshape((-1, 3))

    # Apply KMeans clustering
    kmeans = KMeans(n_clusters=num_colors)
    kmeans.fit(pixels)
    
    # Get the cluster centers (the dominant colors)
    colors = kmeans.cluster_centers_

    # Count the number of pixels in each cluster
    labels = kmeans.labels_
    label_counts = np.bincount(labels)

    # Calculate the proportion of each color
    total_pixels = len(pixels)
    color_proportions = label_counts / total_pixels

    # Sort colors by proportion in descending order
    sorted_indices = np.argsort(color_proportions)[::-1]
    sorted_colors = colors[sorted_indices]
    sorted_proportions = color_proportions[sorted_indices]

    # Convert RGB to HEX and pair with proportions
    hex_colors_with_proportions = [(rgb_to_hex(color), proportion) for color, proportion in zip(sorted_colors, sorted_proportions)]

    return hex_colors_with_proportions[:num_colors]



def is_mostly_one_color(image, threshold=0.95):
    # Flatten the image array and check if more than 95% of the pixels are within a small range of each other
    flattened_image = image.reshape(-1, 3)
    color_diffs = np.max(flattened_image, axis=0) - np.min(flattened_image, axis=0)
    return np.mean(color_diffs) < threshold * 255

def plot_colors(colors, sizes):
    plt.figure(figsize=(8, 8))
    for color, size in zip(colors, sizes):
        plt.scatter([0], [0], c=[color], s=size, alpha=0.7)
    plt.axis('off')
    plt.show()
    # Save the plot to a file and return the file path
    plot_path = tempfile.mktemp(suffix='.png')
    plt.savefig(plot_path, bbox_inches='tight')
    plt.close()
    return plot_path



@app.route('/colors', methods=['POST'])
def get_colors():
    if 'file' not in request.files:
        return jsonify({'error': 'No file part'})

    file = request.files['file']
    if file.filename == '':
        return jsonify({'error': 'No selected file'})

    # Save the uploaded file to a temporary location
    with tempfile.NamedTemporaryFile(delete=False, suffix='.png') as temp_file:
        file.save(temp_file.name)
        temp_file_path = temp_file.name

    # Load the image
    image = cv2.imread(temp_file_path)
    colors = extract_colors(image, num_colors=9)

    # Clean up the temporary file
    os.remove(temp_file_path)

    return jsonify(colors)

@app.route('/plot', methods=['POST'])
def create_plot():
    if 'file' not in request.files:
        return jsonify({'error': 'No file part'})

    file = request.files['file']
    if file.filename == '':
        return jsonify({'error': 'No selected file'})

    # Save the uploaded file to a temporary location
    with tempfile.NamedTemporaryFile(delete=False, suffix='.png') as temp_file:
        file.save(temp_file.name)
        temp_file_path = temp_file.name

    image = cv2.imread(temp_file_path)
    colors_with_proportions = extract_colors1(image, num_colors=10)
    colors = [color for color, _ in colors_with_proportions]
    proportions = [proportion for _, proportion in colors_with_proportions]
    
    plot_path = plot_colors(colors, proportions)

    # Clean up the temporary file
    os.remove(temp_file_path)

    return jsonify(plot_path)

if __name__ == "__main__":
    app.run(host='0.0.0.0', port=5000, debug=True)
