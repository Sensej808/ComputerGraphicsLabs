import tkinter as tk
from tkinter import filedialog
from tkinter import messagebox
import matplotlib.pyplot as plt
import numpy as np
from PIL import Image
from matplotlib.backends.backend_tkagg import FigureCanvasTkAgg

def split_channels(image_path):
    """Выделяет каналы R, G, B из изображения."""
    try:
        img = Image.open(image_path)
        img_array = np.array(img)

        r_channel = img_array[:, :, 0]
        g_channel = img_array[:, :, 1]
        b_channel = img_array[:, :, 2]

        return r_channel, g_channel, b_channel, img
    except FileNotFoundError:
        messagebox.showerror("Ошибка", "Файл изображения не найден.")
        return None, None, None, None

def plot_histograms(r_values, g_values, b_values):
    """Строит гистограммы по цветам (R, G, B)."""
    fig, axes = plt.subplots(1, 3, figsize=(15, 5))

    axes[0].hist(r_values, bins=256, color='red', edgecolor='red')
    axes[0].set_title("Красный канал")

    axes[1].hist(g_values, bins=256, color='green', edgecolor='green')
    axes[1].set_title("Зеленый канал")

    axes[2].hist(b_values, bins=256, color='blue', edgecolor='blue')
    axes[2].set_title("Синий канал")

    # Добавляем plt.show() для отображения гистограмм
    return fig

def display_results(r_channel, g_channel, b_channel, img):
    # Отображение оригинального изображения
    fig_original = plt.figure()
    canvas_original = FigureCanvasTkAgg(fig_original, master=root)
    canvas_original.draw()
    canvas_original.get_tk_widget().pack(side=tk.TOP, padx=5, pady=5)

    plt.imshow(img)
    plt.title("Оригинальное изображение")
    plt.axis('off')

    # Отображение гистограмм
    r_values = r_channel.flatten()
    g_values = g_channel.flatten()
    b_values = b_channel.flatten()
    fig_histograms = plot_histograms(r_values, g_values, b_values)

    canvas_histograms = FigureCanvasTkAgg(fig_histograms, master=root)
    canvas_histograms.draw()
    canvas_histograms.get_tk_widget().pack(side=tk.TOP, padx=5, pady=5)

plt.show()

def load_image():
    """Открывает диалоговое окно для выбора изображения."""
    filepath = filedialog.askopenfilename(
        initialdir="/",
        title="Выберите изображение",
        filetypes=(("Изображения", "*.jpg *.jpeg *.png *.bmp"), ("Все файлы", "*.*"))
    )
    if filepath:
        r_channel, g_channel, b_channel, img = split_channels(filepath)
        if r_channel is not None:
            display_results(r_channel, g_channel, b_channel, img)

# Создание главного окна
root = tk.Tk()
root.title("Анализ изображения")

# Кнопка загрузки изображения
load_button = tk.Button(root, text="Загрузить изображение", command=load_image)
load_button.pack(pady=20)

root.mainloop()
