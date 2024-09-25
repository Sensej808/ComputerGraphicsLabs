using System;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    partial class Task1B : Form
    {
        private Bitmap bitmap;
        private Color fillColor;
        private Bitmap originalImage;
        private Point[] polygonPoints;
        private bool isDrawing = false;
        private Button loadImageButton;

        public Task1B()
        {
            this.Text = "Flood Fill Example";
            this.Width = 800;
            this.Height = 600;

            fillColor = Color.Red; // Цвет заливки
            polygonPoints = new Point[0];

            loadImageButton = new Button();
            loadImageButton.Text = "Загрузить изображение";
            loadImageButton.Dock = DockStyle.Top;
            loadImageButton.Click += LoadImageButton_Click;

            this.Controls.Add(loadImageButton);
            this.Paint += new PaintEventHandler(DrawImage);
            this.MouseClick += new MouseEventHandler(OnMouseClick);
            this.MouseDown += new MouseEventHandler(OnMouseDown);
            this.MouseUp += new MouseEventHandler(OnMouseUp);
            this.MouseMove += new MouseEventHandler(OnMouseMove);
        }

        private void LoadImageButton_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.png;*.jpg;*.jpeg;*.bmp|All files|*.*";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    originalImage = new Bitmap(openFileDialog.FileName);
                    bitmap = new Bitmap(originalImage);
                    this.ClientSize = new Size(bitmap.Width, bitmap.Height);
                    polygonPoints = new Point[0]; // Сбросить точки многоугольника
                    this.Invalidate(); // Перерисовываем форму
                }
            }
        }

        private void DrawImage(object sender, PaintEventArgs e)
        {
            if (bitmap != null)
            {
                e.Graphics.DrawImage(bitmap, 0, 0);
            }
            if (polygonPoints.Length > 0)
            {
                e.Graphics.DrawPolygon(Pens.Blue, polygonPoints);
            }
        }

        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDrawing = true;
                Array.Resize(ref polygonPoints, polygonPoints.Length + 1);
                polygonPoints[polygonPoints.Length - 1] = e.Location;
            }
        }

        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            if (isDrawing)
            {
                isDrawing = false;
                // Замкнуть многоугольник
                Array.Resize(ref polygonPoints, polygonPoints.Length + 1);
                polygonPoints[polygonPoints.Length - 1] = polygonPoints[0]; // Вернуться к первой точке
                this.Invalidate(); // Перерисовываем форму
            }
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (isDrawing)
            {
                Array.Resize(ref polygonPoints, polygonPoints.Length + 1);
                polygonPoints[polygonPoints.Length - 1] = e.Location;
                this.Invalidate(); // Перерисовываем форму
            }
        }

        private void OnMouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                // Начинаем заливку с позиции курсора
                RecursiveFloodFill(e.X, e.Y, bitmap.GetPixel(e.X, e.Y), fillColor);
                this.Invalidate(); // Перерисовываем форму
            }
        }


        private void RecursiveFloodFill(int x, int y, Color targetColor, Color replacementColor)
        {
            if (x < 0 || x >= bitmap.Width || y < 0 || y >= bitmap.Height)
                return; // Выход за границы

            if (bitmap.GetPixel(x, y) != targetColor)
                return; // Цвет не совпадает

            bitmap.SetPixel(x, y, replacementColor); // Заменяем цвет

            // Рекурсивные вызовы для соседних пикселей
            RecursiveFloodFill(x + 1, y, targetColor, replacementColor); // Вправо
            RecursiveFloodFill(x - 1, y, targetColor, replacementColor); // Влево
            RecursiveFloodFill(x, y + 1, targetColor, replacementColor); // Вниз
            RecursiveFloodFill(x, y - 1, targetColor, replacementColor); // Вверх
        }
    }

}