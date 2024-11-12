using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace LAB5G
{
    public partial class Form1 : Form
    {
        private int width = 800;
        private int height = 600;
        private int iterations;
        private float roughness;
        private List<PointF> points;
        private int currentStep;
        private Timer timer; // Таймер для автоматического воспроизведения

        public Form1()
        {
            InitializeComponent();
            this.DoubleBuffered = true; // Для предотвращения мерцаний
            timer = new Timer();
            timer.Interval = 1000; // Время между шагами в миллисекундах (1 секунда)
            timer.Tick += (sender, e) => StepMidpointDisplacement(); // Автоматический шаг при каждом тикере таймера
            InitializeParameters();
        }
        private void InitializeParameters()
        {
            GenerateInitialPoints();
            iterations = trackBar1.Value;
            roughness = trackBar2.Value;
            currentStep = 0; // Сбросить текущий шаг
        }

        private void GenerateInitialPoints()
        {
            points = new List<PointF>
            {
                new PointF(0, height / 4),
                new PointF(width, height / 4)
            };
        }

        private void StepMidpointDisplacement()
        {
            if (currentStep < iterations)
            {
                Random random = new Random();
                List<PointF> newPoints = new List<PointF>();
                float displacement = roughness / (1 << currentStep); // Размещение уменьшается с каждой итерацией

                for (int j = 0; j < points.Count - 1; j++)
                {
                    PointF p1 = points[j];
                    PointF p2 = points[j + 1];
                    newPoints.Add(p1);

                    // Находим середину
                    PointF midPoint = new PointF((p1.X + p2.X) / 2, (p1.Y + p2.Y) / 2);
                    midPoint.Y += (float)(random.NextDouble() * 2 * displacement - displacement); // Случайное смещение
                    newPoints.Add(midPoint);
                }

                newPoints.Add(points[points.Count - 1]); // Добавляем последнюю точку
                points = newPoints;
                currentStep++;
                this.Invalidate(); // Перерисовываем форму после каждого шага
            }
            else
            {
                timer.Stop(); // Останавливаем таймер, если достигнуто максимальное количество итераций
                button1.Text = "Старт";
                MessageBox.Show("Максимальное количество итераций достигнуто.");
            }
        }

        private void ToggleAutoPlay()
        {
            if (timer.Enabled)
            {
                timer.Stop(); // Остановить таймер
                button1.Text = "Старт"; // Изменить текст кнопки
            }
            else
            {
                currentStep = 0; // Сбросить номера шагов
                InitializeParameters(); // Сброс и повторная инициализация параметров
                timer.Start(); // Запустить таймер
                button1.Text = "Стоп"; // Изменить текст кнопки
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (points != null && points.Count > 0)
            {
                using (Pen pen = new Pen(Color.Blue, 2))
                {
                    e.Graphics.DrawLines(pen, points.ToArray());
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ToggleAutoPlay();
        }
    }
}
