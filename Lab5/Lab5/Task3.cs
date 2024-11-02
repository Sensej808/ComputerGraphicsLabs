using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab5
{
    public partial class Task3 : Form
    {
        class ControlPoint
        {
            public PointF coord;
            public bool is_fictive;

            public ControlPoint(PointF coord, bool is_fictive)
            {
                this.coord = coord;
                this.is_fictive = is_fictive;
            }

        }
        private List<ControlPoint> points;

        private List<PointF> controlPoints = new List<PointF>();
        private const float PointSize = 8f;
        private int selectedPointIndex = -1;
        bool is_move = false;
        public Task3()
        {
            this.DoubleBuffered = true;
            this.Width = 800;
            this.Height = 600;

            points = new List<ControlPoint>();

            this.MouseDown += OnMouseDown;
            this.MouseMove += OnMouseMove;
            this.MouseUp += OnMouseUp;
            this.Paint += OnPaint;
        }

        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                for (int i = 0; i < controlPoints.Count; i++)
                {
                    if (IsPointClose(controlPoints[i], e.Location))
                    {
                        is_move = true;
                        selectedPointIndex = i;
                        return;
                    }
                }
                points.Add(new ControlPoint(e.Location, false));
                
                    
                    
                controlPoints.Add(e.Location);
                
                Invalidate();
            }
            else if (e.Button == MouseButtons.Right)
            {
                selectedPointIndex = -1;
                Invalidate();
            }
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (selectedPointIndex >= 0 && is_move && !points[selectedPointIndex].is_fictive)
            {
                points[selectedPointIndex].coord = e.Location;
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

        private void AddFictive(int begin_ind)
        {
            points.Insert(begin_ind+1, new ControlPoint(new PointF((points[begin_ind].coord.X + points[begin_ind+1].coord.X) / 2, (points[begin_ind].coord.Y + points[begin_ind + 1].coord.Y) / 2), true));
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;

            // Рисуем кривую Безье
            for (int i = 0; i < controlPoints.Count - 3; i++)
            {
                DrawBezierCurve(g, points[i].coord, points[i + 1].coord, points[i + 2].coord, points[i + 3].coord);
            }

            // Рисуем опорные точки
            Brush color;
            foreach (var point in points)
            {
                color = point.is_fictive ? Brushes.Violet : Brushes.Red;
                g.FillEllipse(color, point.coord.X - PointSize / 2, point.coord.Y - PointSize / 2, PointSize, PointSize);
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
