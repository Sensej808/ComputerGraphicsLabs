using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Lab5
{
    public partial class Task3 : Form
    {
        private const float PointSize = 8f;
        private List<PointF> controlPoints = new List<PointF>();
        private int selectedPointIndex = -1;
        private bool is_move = false;

        public Task3()
        {
            this.DoubleBuffered = true;
            this.Width = 800;
            this.Height = 600;

            this.MouseDown += OnMouseDown;
            this.MouseMove += OnMouseMove;
            this.MouseUp += OnMouseUp;
            this.Paint += OnPaint;
        }

        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                // Проверяем, находится ли курсор над опорной точкой
                for (int i = 0; i < controlPoints.Count; i++)
                {
                    if (IsPointClose(controlPoints[i], e.Location))
                    {
                        is_move = true;
                        selectedPointIndex = i;
                        return;
                    }
                }

                // Если точка не найдена, добавляем новую точку
                controlPoints.Add(e.Location);
                Invalidate();
            }
            else if (e.Button == MouseButtons.Right)
            {
                // Проверяем, находится ли курсор над опорной точкой для удаления
                for (int i = 0; i < controlPoints.Count; i++)
                {
                    if (IsPointClose(controlPoints[i], e.Location))
                    {
                        controlPoints.RemoveAt(i);
                        Invalidate();
                        return;
                    }
                }
                selectedPointIndex = -1;
                Invalidate();
            }
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (selectedPointIndex >= 0 && is_move)
            {
                controlPoints[selectedPointIndex] = e.Location;
                Invalidate();
            }
        }

        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            is_move = false;
        }

        private bool IsPointClose(PointF point, PointF mousePos)
        {
            return Math.Abs(point.X - mousePos.X) < PointSize && Math.Abs(point.Y - mousePos.Y) < PointSize;
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;

            // Рисуем кривую Безье
            for (int i = 1; i < controlPoints.Count - 1; i += 2)
            {
                PointF p0, p1, p2, p3;

                int prev = i - 1; // p0
                int next2 = i + 2; // p3 

                if (prev == 0)
                    p0 = controlPoints[prev];
                else
                    p0 = new PointF((controlPoints[i].X + controlPoints[prev].X) / 2, (controlPoints[i].Y + controlPoints[prev].Y) / 2);

                if (i == controlPoints.Count - 2)
                {
                    p1 = controlPoints[i];
                    p3 = controlPoints[i + 1];
                    p2 = new PointF((p3.X + p1.X) / 2, (p3.Y + p1.Y) / 2);
                }
                else
                {
                    p1 = controlPoints[i];
                    p2 = controlPoints[i + 1];
                    if (next2 == controlPoints.Count - 1)
                        p3 = controlPoints[next2];
                    else
                        p3 = new PointF((controlPoints[next2].X + p2.X) / 2, (controlPoints[next2].Y + p2.Y) / 2);
                }

                DrawBezierCurve(g, p0, p1, p2, p3);
            }

            // Рисуем опорные точки
            Brush color;
            foreach (var point in controlPoints)
            {
                color = Brushes.Red;
                g.FillEllipse(color, point.X - PointSize / 2, point.Y - PointSize / 2, PointSize, PointSize);
            }
        }

        private void DrawBezierCurve(Graphics g, PointF p0, PointF p1, PointF p2, PointF p3)
        {
            var points = new List<PointF>();
            for (float t = 0; t <= 1; t += 0.01f)
            {
                float x = (float)(Math.Pow(1 - t, 3) * p0.X +
                                  3 * Math.Pow(1 - t, 2) * t * p1.X +
                                  3 * (1 - t) * Math.Pow(t, 2) * p2.X +
                                  Math.Pow(t, 3) * p3.X);
                float y = (float)(Math.Pow(1 - t, 3) * p0.Y +
                                  3 * Math.Pow(1 - t, 2) * t * p1.Y +
                                  3 * (1 - t) * Math.Pow(t, 2) * p2.Y +
                                  Math.Pow(t, 3) * p3.Y);
                points.Add(new PointF(x, y));
            }

            g.DrawLines(Pens.Blue, points.ToArray());
        }
    }
}
