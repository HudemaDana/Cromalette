import cv2
import numpy as np
from sklearn.cluster import KMeans

def rgb_to_hex(rgb):
    return '#{:02x}{:02x}{:02x}'.format(int(rgb[2]), int(rgb[1]), int(rgb[0]))

def extract_colors(image_path, num_colors=9):
    image = cv2.imread(image_path)

    if is_mostly_one_color(image):
        dominant_color = np.mean(image, axis=(0, 1))
        return [rgb_to_hex(dominant_color)] * num_colors

    resized_image = cv2.resize(image, (500, 500), interpolation=cv2.INTER_AREA)

    pixels = resized_image.reshape((-1, 3))

    kmeans = KMeans(n_clusters=num_colors)
    kmeans.fit(pixels)

    colors = kmeans.cluster_centers_

    labels = kmeans.labels_
    label_counts = np.bincount(labels)

    sorted_indices = np.argsort(label_counts)[::-1]
    sorted_colors = colors[sorted_indices]

    hex_colors = [rgb_to_hex(color) for color in sorted_colors]

    return hex_colors[:num_colors]

def extract_colors_to_plot(image_path, num_colors=9):
    image = cv2.imread(image_path)

    if is_mostly_one_color(image):
        dominant_color = np.mean(image, axis=(0, 1))
        return [rgb_to_hex(dominant_color)] * num_colors

    resized_image = cv2.resize(image, (500, 500), interpolation=cv2.INTER_AREA)

    pixels = resized_image.reshape((-1, 3))

    kmeans = KMeans(n_clusters=num_colors)
    kmeans.fit(pixels)

    colors = kmeans.cluster_centers_

    labels = kmeans.labels_
    label_counts = np.bincount(labels)

    sorted_indices = np.argsort(label_counts)[::-1]
    sorted_colors = colors[sorted_indices]

    hex_colors = [rgb_to_hex(color) for color in sorted_colors]
    frequencies = label_counts[sorted_indices] / sum(label_counts)

    return sorted_colors, hex_colors[:num_colors], frequencies

def is_mostly_one_color(image, threshold=0.95):
    flattened_image = image.reshape(-1, 3)
    color_diffs = np.max(flattened_image, axis=0) - np.min(flattened_image, axis=0)
    return np.mean(color_diffs) < threshold * 255
