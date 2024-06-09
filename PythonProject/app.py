from flask import Flask, request, jsonify, render_template, send_file
from flask_cors import CORS
from flask_wtf import FlaskForm
from flask_wtf.file import FileField, FileRequired
from werkzeug.utils import secure_filename
import tempfile
import os

from utils import *
from chart import *

app = Flask(__name__)
CORS(app)
app.config['SECRET_KEY'] = 'your_secret_key'

class UploadForm(FlaskForm):
    file = FileField(validators=[FileRequired()])

@app.route('/')
def index():
    return render_template('index.html')

@app.route('/colors', methods=['POST'])
def get_colors():
    if 'file' not in request.files:
        return jsonify({'error': 'No file part'})

    file = request.files['file']
    if file.filename == '':
        return jsonify({'error': 'No selected file'})

    filename = secure_filename(file.filename)
    with tempfile.NamedTemporaryFile(delete=False, suffix='.png') as temp_file:
        file.save(temp_file.name)
        temp_file_path = temp_file.name

    colors = extract_colors(temp_file_path, num_colors=9)

    os.remove(temp_file_path)

    return jsonify(colors)

@app.route('/upload', methods=['POST'])
def upload():
    file = request.files['file']
    if file.filename == '':
        return jsonify({'error': 'No selected file'})

    if file:
        filename = secure_filename(file.filename)
        with tempfile.NamedTemporaryFile(delete=False, suffix='.png') as temp_file:
            file.save(temp_file.name)
            temp_file_path = temp_file.name

        colors, hex_colors, frequencies = extract_colors_to_plot(temp_file_path, num_colors=9)

        plot_html = plot_colors(colors, frequencies)

        os.remove(temp_file_path)

        return jsonify(plot_html)
    
if __name__ == "__main__":
    app.run(host='0.0.0.0', port=5000, debug=True)
