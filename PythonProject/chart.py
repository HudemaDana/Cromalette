import matplotlib.pyplot as plt
import numpy as np
import matplotlib.colors as mcolors
import cv2
from sklearn.cluster import KMeans
from matplotlib.patches import PathPatch
import plotly.graph_objs as go
import plotly.io as pio
import io

def plot_colors(colors, frequencies):
    fig = go.Figure(data=[go.Scatter3d(
        x=colors[:, 0],
        y=colors[:, 1],
        z=colors[:, 2],
        mode='markers',
        marker=dict(
            size=frequencies * 100,
            color=['rgb({},{},{})'.format(int(c[2]), int(c[1]), int(c[0])) for c in colors],
            opacity=0.8,
        ),
        text=['RGB: ({}, {}, {})'.format(int(c[2]), int(c[1]), int(c[0])) for c in colors],
        hoverinfo='text'
    )])

    fig.update_layout(scene=dict(
        xaxis=dict(title='Red', showticklabels=False),
        yaxis=dict(title='Green', showticklabels=False),
        zaxis=dict(title='Blue', showticklabels=False)
    ), title="Color Clusters in RGB Space")

    return pio.to_html(fig, full_html=False)

def add_gradient_fill(ax, angles, counts, colors):
    from matplotlib.path import Path
    from matplotlib.transforms import IdentityTransform
    
    vertices = np.column_stack([angles, counts])
    path = Path(vertices)
    
    patch = PathPatch(path, facecolor='none', edgecolor='none')
    ax.add_patch(patch)
    
    for i in range(len(colors) - 1):
        ax.imshow(np.array([[colors[i], colors[i + 1]]]), extent=[angles[i], angles[i + 1], 0, 1], transform=IdentityTransform(), aspect='auto')

def create_radar_chart(image_path, num_colors=5):
    # Load the image
    image = cv2.imread(image_path)
    image = cv2.cvtColor(image, cv2.COLOR_BGR2RGB)

    # Reshape the image to be a list of pixels
    pixels = image.reshape((-1, 3))

    # Apply KMeans clustering to find the most common colors
    kmeans = KMeans(n_clusters=num_colors)
    kmeans.fit(pixels)

    # Get the cluster centers (the most common colors)
    colors = kmeans.cluster_centers_
    labels = kmeans.labels_

    # Count the number of pixels assigned to each cluster
    counts = np.bincount(labels)

    # Normalize counts to percentages
    counts = counts / counts.sum()

    # Prepare data for the radar chart
    angles = np.linspace(0, 2 * np.pi, num_colors, endpoint=False).tolist()
    colors = colors.astype(int)
    color_rgb = [color/255 for color in colors]  # Convert to 0-1 range

    # Create a color wheel background
    fig, ax = plt.subplots(figsize=(10, 10), subplot_kw=dict(polar=True))

    # Create a color wheel
    n = 360
    theta = np.linspace(0, 2 * np.pi, n)
    r = np.linspace(0, 1, 2)
    rg, tg = np.meshgrid(r, theta)
    c = tg

    ax.pcolormesh(tg, rg, c, cmap='hsv', shading='auto')
    ax.set_yticklabels([])
    ax.set_xticklabels([])

    # Plot the radar chart on top of the color wheel
    angles += angles[:1]
    counts = np.append(counts, counts[0])

    # Plot data with gradient fill and shadow effect
    add_gradient_fill(ax, angles, counts, color_rgb)
    ax.plot(angles, counts, color='white', linewidth=2, linestyle='solid', marker='o', markersize=8, markerfacecolor='white')

    # Set colorwheel colors for ticks
    color_labels = [mcolors.rgb2hex(mcolors.hsv_to_rgb([h, 1, 1])) for h in np.linspace(0, 1, num_colors, endpoint=False)]
    ax.set_xticks(angles[:-1])
    ax.set_xticklabels(color_labels, fontsize=14, fontweight='bold', color='white')

    # Add title and labels
    plt.title('Color Distribution Radar Chart', size=24, color='white', weight='bold', pad=20)
    ax.grid(color='white', linestyle='--', linewidth=0.5)
    ax.set_facecolor('#2E2E2E')
    fig.patch.set_facecolor('#2E2E2E')

    # Save the figure to a BytesIO object
    img_bytes = io.BytesIO()
    plt.savefig(img_bytes, format='png')
    img_bytes.seek(0)
    plt.close(fig)
    
    return img_bytes
