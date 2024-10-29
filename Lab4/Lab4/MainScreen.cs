using System.Drawing.Drawing2D;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using static Lab4.MainScreen;

namespace Lab4
{
    public partial class MainScreen : Form
    {
        /* 
         каждый раз перерисовываю все полигоны, ибо если  этого не делать, то будет стираться часть точек при стирании точек, при афинных преобразованиях
        */

        List<Point> start_polygon_points; //список для точек из которых будет создан полигон
        Point pointAffineTransform; //точка относительно которой будут делаться афинные преобразования
        List<Polygon> polygons; //список полигонов
        static Bitmap bitmap; //карта пикселей
        States state = States.None;
        //состояния для нажатия мышки по экрану

        //Точки начала и конца отрезка(для пересечения отрезков)
        private Point? dynamicStartPoint;
        private Point? dynamicEndPoint;
        enum States
        {
            None,
            DrawLine,
            CreatePolygon, //точки кладутся в список для создания полигонов
            AffineTransform //относительно этой точки будут делаться некоторые афинные преобразования
        }
        public MainScreen()
        {
            InitializeComponent();
            start_polygon_points = new List<Point>();
            bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            polygons = new List<Polygon>();
            pointAffineTransform = new Point(0, 0);
        }
        //переходим в состояние для создания полигона
        public void button1_Click(object sender, EventArgs e)
        {
            start_polygon_points.Clear();
            state = States.CreatePolygon;
        }

        //рисуем полигон из полученных точек
        public void button2_Click(object sender, EventArgs e)
        {
            Graphics g = Graphics.FromImage(bitmap);
            //убираем точки для создания полигона с экрана
            for (int i = 0; i < start_polygon_points.Count; i++)
            {
                g.FillEllipse(new SolidBrush(pictureBox1.BackColor), start_polygon_points[i].X - 5, start_polygon_points[i].Y - 5, 5 * 2, 5 * 2);

            }
            //убираем точку для афинных преобразований
            //g.FillEllipse(new SolidBrush(pictureBox1.BackColor), pointAffineTransform.X - 5, pointAffineTransform.Y - 5, 5 * 2, 5 * 2);
            pictureBox1.Image = bitmap;
            Polygon new_polygon = new Polygon(start_polygon_points, GetPolygonPoints(ref start_polygon_points));
            //DrawList(ref new_polygon.polygon_points, ref bitmap, Color.Black);
            polygons.Add(new_polygon);
            DrawAll(ref bitmap, Color.Black);
            comboBox1.Items.Add(polygons.Count - 1);
            pictureBox1.Image = bitmap;
            start_polygon_points.Clear();
            state = States.None;

        }
        //очищаем экран
        public void button3_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = null;
            bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            //очищаем все списки
            polygons.Clear();
            comboBox1.Items.Clear();
            state = States.None;
        }

        //функция которая работает при нажатии мыги
        public void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            Graphics g = Graphics.FromImage(bitmap);
            //если состояние создания полигона то кладём точки сюда
            if (state == States.CreatePolygon)
            {
                start_polygon_points.Add(new Point(e.X, e.Y));
                g.FillEllipse(new SolidBrush(Color.Green), e.X - 5, e.Y - 5, 5 * 2, 5 * 2);
            }
            //если состояние для выбора точки для афинных преобразований то кладём точку сюда
            else if (state == States.AffineTransform)
            {
                //стираем предыдущую точку афинных преобразований
                g.FillEllipse(new SolidBrush(pictureBox1.BackColor), pointAffineTransform.X - 5, pointAffineTransform.Y - 5, 5 * 2, 5 * 2);
                pointAffineTransform = new Point(e.X, e.Y);
                g.FillEllipse(new SolidBrush(Color.Green), e.X - 5, e.Y - 5, 5 * 2, 5 * 2);

            }
            else if (state == States.DrawLine)
            {
                if (!dynamicStartPoint.HasValue)
                {
                    dynamicStartPoint = e.Location; // Устанавливаем первую точку
                }
                else
                {
                    dynamicEndPoint = e.Location; // Устанавливаем вторую точку
                }
                if (dynamicStartPoint.HasValue && dynamicEndPoint.HasValue)
                {
                    g.DrawLine(Pens.Red, dynamicStartPoint.Value, dynamicEndPoint.Value);
                    Point x1 = polygons[Int32.Parse(comboBox1.Text)].polygon_vertices[Int32.Parse(comboBox2.Text)];
                    Point x2 = polygons[Int32.Parse(comboBox1.Text)].polygon_vertices[(Int32.Parse(comboBox2.Text) + 1) % polygons[Int32.Parse(comboBox1.Text)].polygon_vertices.Count];

                    Point? intersection = GetIntersection(dynamicStartPoint.Value, dynamicEndPoint.Value, x1, x2);
                    if (intersection.HasValue)
                    {
                        g.FillEllipse(Brushes.Blue, intersection.Value.X - 5, intersection.Value.Y - 5, 10, 10);
                    }
                }
            }
            pictureBox1.Image = bitmap;
        }

        //кнопка для перемещения полигона 
        public void button4_Click(object sender, EventArgs e)
        {
            //получаем нужные данные
            label1.Text = textBox1.Text + " " + textBox2.Text + " " + comboBox1.Text;
            int index = int.Parse(comboBox1.Text);
            int dx = int.Parse(textBox1.Text);
            int dy = int.Parse(textBox2.Text);

            //убираем нарисованный полигон
            DrawList(ref polygons[index].polygon_points, ref bitmap, pictureBox1.BackColor);
            pictureBox1.Image = bitmap;

            //делаем сдвиг
            OffsetDxDy(ref polygons[index].polygon_vertices, dx, dy);

            //меняем остальные точки
            polygons[index].polygon_points = GetPolygonPoints(ref polygons[index].polygon_vertices);

            //рисуем все полигоны
            //DrawList(ref polygons[index].polygon_points, ref bitmap, Color.Black);
            DrawAll(ref bitmap, Color.Black);
            pictureBox1.Image = bitmap;

        }
        //кнопка для поворота полигона 
        private void button5_Click(object sender, EventArgs e)
        {
            //получаем нужные данные
            int index = int.Parse(comboBox1.Text);
            double fi = double.Parse(textBox3.Text);

            //убираем нарисованный полигон
            DrawList(ref polygons[index].polygon_points, ref bitmap, pictureBox1.BackColor);
            pictureBox1.Image = bitmap;

            //делаем поворот
            RotationAroundPointFi(ref polygons[index].polygon_vertices, fi, pointAffineTransform.X, pointAffineTransform.Y);
            Graphics g = Graphics.FromImage(bitmap);
            g.FillEllipse(new SolidBrush(Color.Green), pointAffineTransform.X - 5, pointAffineTransform.Y - 5, 5 * 2, 5 * 2);
            pictureBox1.Image = bitmap;

            //меняем остальные точки
            polygons[index].polygon_points = GetPolygonPoints(ref polygons[index].polygon_vertices);

            //рисуем новый полигон
            //DrawList(ref polygons[index].polygon_points, ref bitmap, Color.Black);
            DrawAll(ref bitmap, Color.Black);
            pictureBox1.Image = bitmap;
        }

        //устанавлиаем состояние для установки точки для афинных преобразований
        private void button6_Click(object sender, EventArgs e)
        {
            state = States.AffineTransform;
        }

        //кнопка для поворота полигона относительно центра
        private void button7_Click(object sender, EventArgs e)
        {
            //получаем нужные данные
            int index = int.Parse(comboBox1.Text);
            double fi = double.Parse(textBox3.Text);

            //убираем нарисованный полигон
            DrawList(ref polygons[index].polygon_points, ref bitmap, pictureBox1.BackColor);
            pictureBox1.Image = bitmap;

            //делаем поворот
            Point center = GetCenterPolygon(ref polygons[index].polygon_vertices);
            RotationAroundPointFi(ref polygons[index].polygon_vertices, fi, center.X, center.Y);

            //меняем остальные точки
            polygons[index].polygon_points = GetPolygonPoints(ref polygons[index].polygon_vertices);

            //рисуем новый полигон
            //DrawList(ref polygons[index].polygon_points, ref bitmap, Color.Black);
            DrawAll(ref bitmap, Color.Black);
            pictureBox1.Image = bitmap;
        }

        //кнопка для масштабирования полигона относительно точки
        private void button8_Click(object sender, EventArgs e)
        {
            //получаем нужные данные
            int index = int.Parse(comboBox1.Text);
            double a = double.Parse(textBox4.Text);
            double b = double.Parse(textBox5.Text);

            //убираем нарисованный полигон
            DrawList(ref polygons[index].polygon_points, ref bitmap, pictureBox1.BackColor);
            pictureBox1.Image = bitmap;

            //масштабируем
            ScallingRelativePoint(ref polygons[index].polygon_vertices, a, b, pointAffineTransform.X, pointAffineTransform.Y);
            Graphics g = Graphics.FromImage(bitmap);
            g.FillEllipse(new SolidBrush(Color.Green), pointAffineTransform.X - 5, pointAffineTransform.Y - 5, 5 * 2, 5 * 2);
            pictureBox1.Image = bitmap;

            //меняем остальные точки
            polygons[index].polygon_points = GetPolygonPoints(ref polygons[index].polygon_vertices);

            //рисуем новый полигон
            //DrawList(ref polygons[index].polygon_points, ref bitmap, Color.Black);
            DrawAll(ref bitmap, Color.Black);
            pictureBox1.Image = bitmap;
        }

        //масштабирование относительно центра
        private void button9_Click(object sender, EventArgs e)
        {
            //получаем нужные данные
            int index = int.Parse(comboBox1.Text);
            double a = double.Parse(textBox4.Text);
            double b = double.Parse(textBox5.Text);

            //убираем нарисованный полигон
            DrawList(ref polygons[index].polygon_points, ref bitmap, pictureBox1.BackColor);
            pictureBox1.Image = bitmap;

            //масштабируем
            Point center = GetCenterPolygon(ref polygons[index].polygon_vertices);
            ScallingRelativePoint(ref polygons[index].polygon_vertices, a, b, center.X, center.Y);
            //ScallingRelativeCenter(ref polygons[index].polygon_vertices, a, b);

            //меняем остальные точки
            polygons[index].polygon_points = GetPolygonPoints(ref polygons[index].polygon_vertices);

            //рисуем новый полигон
            //DrawList(ref polygons[index].polygon_points, ref bitmap, Color.Black);
            DrawAll(ref bitmap, Color.Black);
            pictureBox1.Image = bitmap;
        }


        //функции для отрисовки которые работают с уже вычисленными значениями

        //функция для отрисовки списка точек
        public static void DrawList(ref List<Point> l, ref Bitmap pixelMap, Color draw_color)
        {
            for (int i = 0; i < l.Count; i++)
            {
                pixelMap.SetPixel(l[i].X, l[i].Y, draw_color);
            }
            //рисуем центр
            //Graphics g = Graphics.FromImage(pixelMap);
            //g.FillEllipse(new SolidBrush(Color.Green), l[l.Count - 1].X - 5, l[l.Count - 1].Y - 5, 5 * 2, 5 * 2);
        }

        //
        public void DrawAll(ref Bitmap pixelMap, Color draw_color)
        {
            for (int i = 0; i < this.polygons.Count; i++)
            {
                for (int j = 0; j < this.polygons[i].polygon_points.Count; j++)
                {
                    pixelMap.SetPixel(this.polygons[i].polygon_points[j].X, this.polygons[i].polygon_points[j].Y, draw_color);
                }
                //label1.Text = $"{this.polygons.Count}";
                //рисуем центр
                //Graphics g = Graphics.FromImage(pixelMap);
                //g.FillEllipse(new SolidBrush(Color.Green), this.polygons[i].polygon_points[this.polygons[i].polygon_points.Count - 1].X - 5, this.polygons[i].polygon_points[this.polygons[i].polygon_points.Count - 1].Y - 5, 5 * 2, 5 * 2);
            }
        }

        //Вычислительные функции не связанные с отрисовкой

        //класс полигона который хранит вершины и все точкиb
        //возможно всю логику стоит занести в него но хз, пока она разбита по функциям
        public class Polygon
        {
            public List<Point> polygon_vertices;
            public List<Point> polygon_points;

            public Polygon(List<Point> polygon_v, List<Point> polygon_p)
            {
                this.polygon_vertices = new List<Point>(polygon_v);
                this.polygon_points = new List<Point>(polygon_p);
            }
        }

        //функция которая возвращает список точек полигона по заданному списку вершин
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
            //label1.Text = $"{cnt} {polygonPoints[polygonPoints.Count-1].X} {polygonPoints.Count}";
            if (verices.Count > 2)
            {
                var last_line = getLineCoordsByBresenham(verices[0], verices[verices.Count - 1]).ToArray();
                for (int j = 0; j < last_line.Length; j++)
                {
                    polygonPoints.Add(last_line[j]);
                }
            }
            //polygonPoints.Add(GetCenterPolygon(ref verices));
            return polygonPoints;
        }

        //умножение матриц
        public double[,] MatrixMultiplication(ref double[,] m1, ref double[,] m2)
        {
            int row1 = m1.GetLength(0);
            int col1 = m1.GetLength(1);
            int row2 = m2.GetLength(0);
            int col2 = m2.GetLength(1);
            double[,] res = new double[row1, col2];
            for (int i = 0; i < row1; i++)
            {
                for (int j = 0; j < col2; j++)
                {
                    double cell = 0;
                    for (int k = 0; k < col1; k++)
                    {
                        cell += m1[i, k] * m2[k, j];
                    }
                    res[i, j] = cell;
                }
            }
            return res;
        }

        //функция для афинных преобразований точки на плоскости
        public Point AffineTransformation(Point p, ref double[,] TransMatrix)
        {
            double[,] matrix_p = new double[1, 3] { { p.X, p.Y, 1 } };
            double[,] matrix_res = MatrixMultiplication(ref matrix_p, ref TransMatrix);
            Point res = new Point((int)Math.Round(matrix_res[0, 0]), (int)Math.Round(matrix_res[0, 1]));
            return res;
        }

        //функция для  афинных преобразования списка точек на плоскости
        public void AffineTransformationList(ref List<Point> l, ref double[,] TransMatrix)
        {
            for (int i = 0; i < l.Count; i++)
            {
                l[i] = AffineTransformation(l[i], ref TransMatrix);
            }
        }

        //Сдвиг списка точек плоскости на dx и dy
        public void OffsetDxDy(ref List<Point> l, double dx, double dy)
        {
            double[,] transMatrix = new double[3, 3] {
                                                { 1, 0, 0 },
                                                { 0, 1, 0 },
                                                { dx, dy, 1 }
                                              };
            AffineTransformationList(ref l, ref transMatrix);
        }

        //Поворот вокруг центра плоскости на угол fi
        public void RotationAroundCenterFi(ref List<Point> l, double fi)
        {
            fi = fi * Math.PI / 180;
            double[,] transMatrix = new double[3, 3] {
                                                { Math.Cos(fi), Math.Sin(fi), 0 },
                                                { -Math.Sin(fi), Math.Cos(fi), 0 },
                                                { 0, 0, 1},
                                               };
            AffineTransformationList(ref l, ref transMatrix);
        }

        //Поворот вокруг точки на угол fi
        public void RotationAroundPointFi(ref List<Point> l, double fi, double x, double y)
        {
            OffsetDxDy(ref l, -x, -y);
            RotationAroundCenterFi(ref l, fi);
            OffsetDxDy(ref l, x, y);
        }

        //функция для подсчёта центра списка точек
        static public Point GetCenterPolygon(ref List<Point> l)
        {

            double A = 0;  // Подписанная площадь
            double Cx = 0;
            double Cy = 0;

            int n = l.Count;

            // Формула для площади и центроида
            for (int i = 0; i < n; i++)
            {
                int j = (i + 1) % n; // Следующий индекс (замкнутый полигон)

                double xi = l[i].X;
                double yi = l[i].Y;
                double xi1 = l[j].X;
                double yi1 = l[j].Y;

                double commonTerm = (xi * yi1 - xi1 * yi);

                A += commonTerm;
                Cx += (xi + xi1) * commonTerm;
                Cy += (yi + yi1) * commonTerm;
            }

            A *= 0.5;
            Cx = (Cx) / (6 * A);
            Cy = (Cy) / (6 * A);

            return new Point((int)Math.Round(Cx), (int)Math.Round(Cy));

        }

        //Масштабирование относительно центра плоскости на коэффицента a, b
        public void ScallingRelativeCenter(ref List<Point> l, double a, double b)
        {
            double[,] transMatrix = new double[3, 3] {
                                                { a, 0, 0 },
                                                { 0, b, 0 },
                                                { 0, 0, 1},
                                               };
            AffineTransformationList(ref l, ref transMatrix);
        }

        //Масштабирование относительно точки на коэффиценты a, b
        public void ScallingRelativePoint(ref List<Point> l, double a, double b, double x, double y)
        {
            OffsetDxDy(ref l, -x, -y);
            ScallingRelativeCenter(ref l, a, b);
            OffsetDxDy(ref l, x, y);
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

        public bool IsPointInPolygon(Point point, Polygon p)
        {
            int crossings = 0;
            int n = p.polygon_vertices.Count;

            for (int i = 0; i < n; i++)
            {
                Point p1 = p.polygon_vertices[i];
                Point p2 = p.polygon_vertices[(i + 1) % n];

                // Проверяем, пересекает ли горизонтальная линия, проведенная через точку, отрезок
                if ((p1.Y > point.Y) != (p2.Y > point.Y))
                {
                    // Вычисляем x-координату пересечения
                    float intersectionX = (float)(p2.X - p1.X) * (point.Y - p1.Y) / (p2.Y - p1.Y) + p1.X;
                    if (point.X < intersectionX)
                    {
                        crossings++;
                    }
                }
            }

            // Если число пересечений нечётное, точка внутри полигона
            return (crossings % 2) == 1;
        }

        //Кнопка проверки на принадлежность точки к выбранному полигону
        private void button11_Click(object sender, EventArgs e)
        {
            if (state == States.AffineTransform)
            {
                //строка чтобы людей пугать
                MessageBox.Show(IsPointInPolygon(pointAffineTransform, polygons[Int32.Parse(comboBox1.Text)]) ? "Точка внутри полигона" : "Точка вне полигона");
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.Items.Clear();
            for (int i = 0; i < polygons[Int32.Parse(comboBox1.Text)].polygon_vertices.Count; i++)
            {
                comboBox2.Items.Add(i);
            }
        }

        private Point? GetIntersection(Point p1, Point p2, Point p3, Point p4)
        {
            float denominator = (p4.Y - p3.Y) * (p2.X - p1.X) - (p4.X - p3.X) * (p2.Y - p1.Y);
            if (denominator == 0)
            {
                return null; // Параллельные линии
            }

            float ua = ((p4.X - p3.X) * (p1.Y - p3.Y) - (p4.Y - p3.Y) * (p1.X - p3.X)) / denominator;
            float ub = ((p2.X - p1.X) * (p1.Y - p3.Y) - (p2.Y - p1.Y) * (p1.X - p3.X)) / denominator;

            if (ua >= 0 && ua <= 1 && ub >= 0 && ub <= 1)
            {
                // Вычисляем координаты точки пересечения
                float intersectionX = p1.X + ua * (p2.X - p1.X);
                float intersectionY = p1.Y + ua * (p2.Y - p1.Y);
                return new Point((int)intersectionX, (int)intersectionY);
            }

            return null; // Нет пересечения
        }

        private void button12_Click(object sender, EventArgs e)
        {
            state = States.DrawLine;
        }
    }
}