import matplotlib.pyplot as plt
from PIL import Image

def task2():
    image = Image.open('pic2.jpg')
    w, h = image.size
    red_image = Image.new("RGB", (w, h))
    green_image = Image.new("RGB", (w, h))
    blue_image = Image.new("RGB", (w, h))

    for x in range(w):
        for y in range(h):
            r, g, b = image.getpixel((x, y))
            red_image.putpixel((x, y), (r, 0, 0))
    red_image.show()

    red_hist = red_image.histogram()
    fig, ax = plt.subplots(figsize=(12, 6))
    ax.stairs(red_hist[:256], color='red', label='Red Color')
    ax.set_xlabel('Pixel Value')
    ax.set_ylabel('Frequency')
    ax.set_title('Red Color Histogram')
    ax.legend()
    plt.show()

    for x in range(w):
        for y in range(h):
            r, g, b = image.getpixel((x, y))
            green_image.putpixel((x, y), (0, g, 0))
    green_image.show()

    green_hist = green_image.histogram()
    fig, ax = plt.subplots(figsize=(12, 6))
    ax.stairs(green_hist[256:512], color='green', label='Green Color')
    ax.set_xlabel('Pixel Value')
    ax.set_ylabel('Frequency')
    ax.set_title('Green Color Histogram')
    ax.legend()
    plt.show()

    for x in range(w):
        for y in range(h):
            r, g, b = image.getpixel((x, y))
            blue_image.putpixel((x, y), (0, 0, b))
    blue_image.show()

    blue_hist = blue_image.histogram()
    fig, ax = plt.subplots(figsize=(12, 6))
    ax.stairs(blue_hist[512:768], color='blue', label='Blue Color')
    ax.set_xlabel('Pixel Value')
    ax.set_ylabel('Frequency')
    ax.set_title('Blue Color Histogram')
    ax.legend()
    plt.show()
