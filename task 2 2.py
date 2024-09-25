import tkinter as tk
from tkinter import filedialog
from PIL import Image, ImageTk, ImageOps, ImageChops
import numpy as np
import matplotlib.pyplot as plt


def to_grayscale_formula1(image):
    r, g, b = image.split()

    # Применяем коэффициенты к каждому каналу и создаём новое изображение
    grayscale = Image.eval(r, lambda i: i * 0.2999)
    grayscale = Image.eval(g, lambda i: grayscale.getpixel((0, 0)) + i * 0.5870)
    grayscale = Image.eval(b, lambda i: grayscale.getpixel((0, 0)) + i * 0.1140)

    return grayscale.convert("L")

def to_grayscale_formula2(image):
    r, g, b = image.split()
    r = r.point(lambda i: i * 0.2126)
    g = g.point(lambda i: i * 0.7152)
    b = b.point(lambda i: i * 0.0722)
    return Image.merge("RGB", (r, g, b)).convert("L")

def show_histogram(image, ax, title):
    hist_data = np.array(image).flatten()
    ax.clear()
    ax.hist(hist_data, bins=256, range=(0, 255), color='gray')
    ax.set_title(title)

def load_image():
    file_path = filedialog.askopenfilename()
    if file_path:
        original_image = Image.open(file_path)

        gray_image1 = to_grayscale_formula1(original_image)
        gray_image2 = to_grayscale_formula2(original_image)

        diff_image = ImageOps.invert(ImageChops.difference(gray_image1, gray_image2))

        update_image_display(original_image, gray_image1, gray_image2, diff_image)

        fig, axs = plt.subplots(1, 2, figsize=(10, 4))
        show_histogram(gray_image1, axs[0], "Formula 1")
        show_histogram(gray_image2, axs[1], "Formula 2")
        plt.show()

def update_image_display(original, gray1, gray2, diff):
    original_photo = ImageTk.PhotoImage(original)
    gray1_photo = ImageTk.PhotoImage(gray1)
    gray2_photo = ImageTk.PhotoImage(gray2)
    diff_photo = ImageTk.PhotoImage(diff)

    update_canvas_image(original_canvas, original_photo)
    update_canvas_image(gray1_canvas, gray1_photo)
    update_canvas_image(gray2_canvas, gray2_photo)
    update_canvas_image(diff_canvas, diff_photo)

def update_canvas_image(canvas, image_photo):
    canvas.config(width=image_photo.width(), height=image_photo.height())
    canvas.create_image(0, 0, anchor=tk.NW, image=image_photo)
    canvas.image = image_photo

root = tk.Tk()
root.title("Grayscale Conversion")

frame = tk.Frame(root)
frame.pack(fill=tk.BOTH, expand=True, padx=10, pady=10)

load_button = tk.Button(frame, text="Load Image", command=load_image)
load_button.grid(row=0, column=0, columnspan=2, sticky="ew")

original_canvas = tk.Canvas(frame, bg="white")
original_canvas.grid(row=1, column=0, padx=5, pady=5, sticky="nsew")

gray1_canvas = tk.Canvas(frame, bg="white")
gray1_canvas.grid(row=1, column=1, padx=5, pady=5, sticky="nsew")

gray2_canvas = tk.Canvas(frame, bg="white")
gray2_canvas.grid(row=2, column=0, padx=5, pady=5, sticky="nsew")

diff_canvas = tk.Canvas(frame, bg="white")
diff_canvas.grid(row=2, column=1, padx=5, pady=5, sticky="nsew")

# Настройка пропорционального изменения размеров
frame.columnconfigure(0, weight=1)
frame.columnconfigure(1, weight=1)
frame.rowconfigure(1, weight=1)
frame.rowconfigure(2, weight=1)

root.mainloop()
