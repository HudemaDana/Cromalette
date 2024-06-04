import cv2
import numpy as np
from sklearn.cluster import KMeans

def rgb_to_hex(rgb):
    return '#{:02x}{:02x}{:02x}'.format(int(rgb[2]), int(rgb[1]), int(rgb[0]))

def extract_colors(image_path, num_colors=9):
    image = cv2.imread(image_path)

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

def extract_colors_to_plot(image_path, num_colors=9):
    image = cv2.imread(image_path)

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
    frequencies = label_counts[sorted_indices] / sum(label_counts)  # Normalized frequencies

    return sorted_colors, hex_colors[:num_colors], frequencies

def is_mostly_one_color(image, threshold=0.95):
    # Flatten the image array and check if more than 95% of the pixels are within a small range of each other
    flattened_image = image.reshape(-1, 3)
    color_diffs = np.max(flattened_image, axis=0) - np.min(flattened_image, axis=0)
    return np.mean(color_diffs) < threshold * 255
