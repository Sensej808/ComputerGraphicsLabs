using System.Drawing;
using System.Net.Security;

namespace IndividualTask1
{
    public partial class Form1 : Form
    {
        enum States
        {
            None,
            Create
        }
        States state;
        List<Point> points;
        Bitmap bitmap;
        public Form1()
        {
            InitializeComponent();
            points = new List<Point>();
            state = States.None;
            bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            points.Clear();
            state = States.Create;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            List<Point> shellVertices = JarvisAlgorithm(points);
            shellpointsColoring(ref shellVertices, ref bitmap, Color.Red);
            pictureBox1.Image = bitmap;
            List<Point> shell = GetPolygonPoints(ref shellVertices);
            DrawList(ref shell, ref bitmap, Color.Black);
            pictureBox1.Image = bitmap;
            points.Clear();
            state = States.None;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            points.Clear();
            state = States.None;
            pictureBox1.Image = null;
            bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            Graphics g = Graphics.FromImage(bitmap);
            if (state == States.Create)
            {
                points.Add(new Point(e.X, e.Y));
                g.FillEllipse(new SolidBrush(Color.Green), e.X - 5, e.Y - 5, 5 * 2, 5 * 2);
                pictureBox1.Image = bitmap;
            }
        }

        public static void DrawList(ref List<Point> l, ref Bitmap pixelMap, Color draw_color)
        {
            for (int i = 0; i < l.Count; i++)
            {
                pixelMap.SetPixel(l[i].X, l[i].Y, draw_color);
            }
        }

        public void shellpointsColoring(ref List<Point> l, ref Bitmap pixelMap, Color draw_color)
        {
            Graphics g = Graphics.FromImage(bitmap);
            for (int i = 0; i < l.Count; i++)
            {
                points.Add(new Point(l[i].X, l[i].Y));
                g.FillEllipse(new SolidBrush(draw_color), l[i].X - 5, l[i].Y - 5, 5 * 2, 5 * 2);

            }
        }

        private List<Point> JarvisAlgorithm(List<Point> allPoints)
        {
            List<Point> shell = new List<Point>();
            int minY = int.MaxValue;
            int indexMinY = 0;
            for (int i = 0; i < allPoints.Count; i++)
            {
                if (allPoints[i].Y < minY)
                {
                    indexMinY = i;
                    minY = allPoints[i].Y;
                }
                if (allPoints[i].Y == minY)
                {
                    if (allPoints[i].X > allPoints[indexMinY].X)
                    {
                        indexMinY = i;
                    }
                }
            }
            shell.Add(allPoints[indexMinY]);
            double minFi = double.MaxValue;
            int minFiIndex = 0;
            for (int i = 0; i < allPoints.Count; i++)
            {
                if (indexMinY != i)
                {
                    int x = allPoints[i].X - shell[0].X;
                    int y = allPoints[i].Y - shell[0].Y;
                    double fi = Math.Atan2(y, x);
                    if (fi < minFi)
                    {
                        minFi = fi;
                        minFiIndex = i;
                    }
                }
            }
            shell.Add(allPoints[minFiIndex]);
            
            allPoints.RemoveAt(minFiIndex);
            Point p1 = shell[0];
            Point p2 = shell[1];
            while (true)
            {
                double minCos = double.MaxValue;
                int minCosIndex = 0;
                for (int i = 0; i < allPoints.Count; i++)
                {
                    Point v1 = new Point(p1.X-p2.X, p1.Y - p2.Y);
                    Point v2 = new Point(allPoints[i].X - p2.X, allPoints[i].Y - p2.Y);
                    double currCos = (v1.X * v2.X + v1.Y * v2.Y) / (Math.Sqrt(v1.X * v1.X + v1.Y * v1.Y) * Math.Sqrt(v2.X * v2.X + v2.Y * v2.Y));
                    if (currCos < minCos)
                    {
                        minCos = currCos;
                        minCosIndex = i;
                    }
                }
                if (allPoints[minCosIndex] == shell[0])
                {
                    break;
                }
                shell.Add(allPoints[minCosIndex]);
                allPoints.RemoveAt(minCosIndex);
                p1 = p2;
                p2 = shell[shell.Count - 1];
            }
            return shell;
        }

        public List<Point> GetPolygonPoints(ref List<Point> verices)
        {
            List<Point> polygonPoints = new List<Point>();
            int cnt = 0;
            for (int i = 0; i < verices.Count - 1; i++)
            {
                cnt++;
                var line = getLineCoordsByBresenham(verices[i], verices[i + 1]).ToArray();
                for (int j = 0; j < line.Length; j++)
                {
                    polygonPoints.Add(line[j]);
                }
            }
            if (verices.Count > 2)
            {
                var last_line = getLineCoordsByBresenham(verices[0], verices[verices.Count - 1]).ToArray();
                for (int j = 0; j < last_line.Length; j++)
                {
                    polygonPoints.Add(last_line[j]);
                }
            }
            return polygonPoints;
        }

        static public Point[] getLineHigh(int x0, int y0, int x1, int y1)
        {
            List<Point> res = new List<Point>();
            int dx = x1 - x0;
            int dy = y1 - y0;
            int xi = 1;
            if (dx < 0)
            {
                xi = -1;
                dx = -dx;
            }

            int D = (2 * dx) - dy;
            int x = x0;

            for (int y = y0; y <= y1; y++)
            {
                res.Add(new Point(x, y));
                if (D > 0)
                {
                    x += xi;
                    D += 2 * (dx - dy);
                }
                else
                {
                    D += 2 * dx;
                }
            }

            return res.OrderBy(p => p.Y).ToArray();
        }

        static public Point[] getLineLow(int x0, int y0, int x1, int y1)
        {
            List<Point> res = new List<Point>();
            int dx = x1 - x0;
            int dy = y1 - y0;
            int yi = 1;
            if (dy < 0)
            {
                yi = -1;
                dy = -dy;
            }

            int D = (2 * dy) - dx;
            int y = y0;

            for (int x = x0; x <= x1; x++)
            {
                res.Add(new Point(x, y));
                if (D > 0)
                {
                    y += yi;
                    D += 2 * (dy - dx);
                }
                else
                {
                    D += 2 * dy;
                }
            }

            return res.OrderBy(p => p.Y).ToArray();
        }

        static public Point[] getLineCoordsByBresenham(Point p0, Point p1)
        {
            if (Math.Abs(p1.Y - p0.Y) < Math.Abs(p1.X - p0.X))
            {
                if (p0.X > p1.X)
                {
                    return getLineLow(p1.X, p1.Y, p0.X, p0.Y);
                }
                return getLineLow(p0.X, p0.Y, p1.X, p1.Y);
            }

            if (p0.Y > p1.Y)
            {
                return getLineHigh(p1.X, p1.Y, p0.X, p0.Y);
            }

            return getLineHigh(p0.X, p0.Y, p1.X, p1.Y);
        }
    }
}