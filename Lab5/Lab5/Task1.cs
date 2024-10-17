using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.AxHost;

namespace Lab5
{
    public partial class Task1 : Form
    {
        Bitmap bitmap;
        public Task1()
        {
            InitializeComponent();
            bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
        }

        //кнопка для отрисовки фрактала по заданным данным
        private void button1_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = null;
            bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Dictionary<char, string> map = new Dictionary<char, string>();

            //сначала просто получаем информацию из формы
            string lText = richTextBox2.Text;
            var strArr = lText.Split(' ', '\n', '\r');
            try
            {
                string axiom = strArr[0];
                int rotationAngle = int.Parse(strArr[1]);
                int startAngle = int.Parse(strArr[2]);
                for (int i = 3; i < strArr.Length; i += 2)
                {
                    map.Add(strArr[i][0], strArr[i + 1]);
                }
                bool isRandom = checkBox1.Checked;
                int generations = int.Parse(textBox1.Text);

                //генерируем строку L системы
                string s = LSystem.GeneratedString(axiom, ref map, generations);
                label2.Text = s.Count().ToString();
                //получаем список пар точек(линий) фрактала
                List<(PointF, PointF)> lines = LSystem.GetFractalVertices(s, new Point(0, 0), 1f, rotationAngle, startAngle, isRandom, 0);
                //преобразуем список пар точек в список точек для масштабирования
                List<PointF> pair_points = new List<PointF>();
                foreach (var line in lines)
                {
                    pair_points.Add(line.Item1);
                    pair_points.Add(line.Item2);
                }
                var minx = pair_points.Min(p => p.X);
                var maxx = pair_points.Max(p => p.X);
                label2.Text = (maxx-minx).ToString();
                //масштабируем весь фрактал на все поле и переносим его в центр
                Helper.scallingFullWindow(ref pair_points, 0, 0, pictureBox1.Width, pictureBox1.Height);
                //возвращаемся обратно к парам
                for (int i = 0; i < lines.Count; i++)
                    lines[i] = (pair_points[i*2], pair_points[i*2+1]);
                //получаем список точек для отрисовки из пар точек
                List<Point> points = LineGenerator.GetPointsForLines(lines.Select(p => (new Point((int)p.Item1.X, (int)p.Item1.Y),
                                                                                        new Point((int)p.Item2.X, (int)p.Item2.Y))).ToList());
                //рисуем точки
                foreach (Point p in points)
                {
                    bitmap.SetPixel(p.X, p.Y, Color.Black);
                }
                pictureBox1.Image = bitmap;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка в чтении данных: {ex.Message}");
            }
        }

        //кнопка для отрисовки дерева
        private void button2_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = null;
            bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Dictionary<char, string> map = new Dictionary<char, string>();
            //получаем L систему для отрисовки дерева с равными ветвями
            string axiom = "X";
            map.Add('X', "F[[-X]+X]");
            //получаем количество поколений
            int iter = int.Parse(textBox1.Text);
            //генерируем конечную строку L системы
            string s = LSystem.GeneratedString(axiom, ref map, iter);
            //получаем пары точек(линии) полученного фрактала
            List<(PointF, PointF)> lines = LSystem.GetFractalVertices(s, new Point(0, 0), 1f, 45, 270, checkBox1.Checked, 0.9f);
            //преобразуем список пар точек в список точек
            List<PointF> pair_points = new List<PointF>();
            foreach (var line in lines)
            {
                pair_points.Add(line.Item1);
                pair_points.Add(line.Item2);
            }
            //масштабируем весь фрактал на все поле и переносим его в центр
            Helper.scallingFullWindow(ref pair_points, 0, 0, pictureBox1.Width, pictureBox1.Height, 0.85f);
            //возвращаемся обратно к парам
            for (int i = 0; i < lines.Count; i++)
                lines[i] = (pair_points[i * 2], pair_points[i * 2 + 1]);
            //получаем для каждого поколения веток свой цвет и свою толщину
            //мы проходимся по финальной строке и в зависимости от расположение [ ] создаем список ширины и цвета для каждого поколения
            List<float> widthList = new List<float>();
            Stack<float> widtStack = new Stack<float>();
            List<Color> colorList = new List<Color>();
            Stack<Color> colorStack = new Stack<Color>();
            //получаем ширину из количества поколений
            float width = iter*1.25f;
            //начальный цвет
            Color color = Color.Black;
            foreach (char c in s)
            {
                if (c == 'F')
                {
                    widthList.Add(width);
                    colorList.Add(color);
                }
                if (c == '[')
                {
                    if (width >= 1)
                        width *= 0.9f;
                    widtStack.Push(width);
                    color = Color.FromArgb(Math.Min(color.R + 8, 255), Math.Min(color.G + 8, 255), Math.Min(color.B + 8, 255));
                    colorStack.Push(color);
                }
               if (c == ']')
               {
                   width = widtStack.Pop();
                   color = colorStack.Pop();
               }
              
            }
            
            List<Point> points = new List<Point>();
            List<Color> colors = new List<Color>();
            //получаем список точек из пар точек, но уже для толстых линий
            for (int i = 0; i < lines.Count; i++)
            {
                //получаем часть списка точек и свою ширину для него
                var l = LineGenerator.getWideLineCoordsByBresenham(new Point((int)lines[i].Item1.X, (int)lines[i].Item1.Y),
                                                                   new Point((int)lines[i].Item2.X, (int)lines[i].Item2.Y), (int)widthList[i]);
                //для каждо точки из этоц части получаем свой ццвет
                for (int j = 0; j < l.Count; j++)
                {
                    colors.Add(colorList[i]);
                }
                points.AddRange(l.ToArray());
            }
            //отрисовка
            for (int i = 0; i < points.Count; i++)
            {
                bitmap.SetPixel(points[i].X, points[i].Y, colors[i]);
            }
            widthList = widthList.Distinct().ToList();
            pictureBox1.Image = bitmap;

        }

        //функция для получения файла через диалоговое окно
        private void button3_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Multiselect = true; 
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    richTextBox1.Text =  openFileDialog.FileName;
                }
            }
        }

        //срабатывает, если мы с зажатым мышкой файлом перемещаемся к кнопке 
        private void button4_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
        }

        //получаем путь перемещаемого файла
        private void button4_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            richTextBox1.Text = files[0];
        }

        //получаем текст из файла
        private void button5_Click(object sender, EventArgs e)
        {
            string file = richTextBox1.Text;
            if (File.Exists(file))
            {
                try
                {
                    string fileContent = File.ReadAllText(file);
                    richTextBox2.Text = fileContent;
                }
                catch (Exception ex) {
                    MessageBox.Show($"Ошибка при чтении файла: {ex.Message}"); }
            }
            else 
                MessageBox.Show("Файл не найден. Убедитесь, что путь к файлу правильный.");
        }
    }


    //класс для работы L системы
    public class LSystem
    {
        //функция для получения конечной строки L системы
        public static string GeneratedString(string axiom, ref Dictionary<char, string> grammar, int generations)
        {
            List<char> result = axiom.ToList();
            for (int i = 0; i < generations; i++)
            {
                List<char> newResult = new List<char>();
                foreach (char c in result)
                {
                    if (grammar.ContainsKey(c))
                    {
                        foreach (char c1 in grammar[c])
                        {
                            newResult.Add(c1);
                        }
                    }
                    else
                    {
                        newResult.Add(c);
                    }
                }
                result = newResult;
            }
            return new string(result.ToArray());
        }

        //получение списка пар точек(первая точка в паре - начало отрезка, вторая - конец) фрактала
        public static List<(PointF, PointF)> GetFractalVertices(string s, PointF begin, float size,
                                                     int rotationAngle, int startAngle, bool isRandom, float sizeM)
        {
            List<(PointF, PointF)> result = new List<(PointF, PointF)>();
            int angle = startAngle;
            Stack<(int, PointF)> stack = new Stack<(int, PointF)>();
            Random random = new Random();
            PointF prevPoint = begin;
            float currSize = size;
            Stack<float> stackSize = new Stack<float>();
            foreach (char c in s)
            {
                if (c == 'F')
                {
                    PointF nextPoint = GetNextPoint(prevPoint, angle, currSize);
                    result.Add((prevPoint, nextPoint));
                    prevPoint = nextPoint;
                }

                if (c == 'f')
                {
                    prevPoint = GetNextPoint(prevPoint, angle, currSize);
                }

                if (c == '+')
                {
                    if (isRandom)
                    {
                        angle += random.Next(0, rotationAngle + 1);
                    }
                    else
                        angle += rotationAngle;
                }
                if (c == '-')
                {
                    if (isRandom)
                    {
                        angle -= random.Next(0, rotationAngle + 1);
                    }
                    else
                        angle -= rotationAngle;
                }
                if (c == '[')
                {
                    if (sizeM != 0)
                    {
                        currSize = currSize * sizeM;
                        stackSize.Push(currSize);
                    }
                    stack.Push((angle, prevPoint));
                }
                if (c == ']')
                {
                    (int savedAngle, PointF lastPoint) = stack.Pop();
                    prevPoint = lastPoint;
                    angle = savedAngle;
                    if (sizeM != 0)
                    {
                        currSize = stackSize.Pop();
                    }
                }
            }
            return result;
        }

        //получаем следующую точку исходя из предыдущей, длины линии и угла
        private static PointF GetNextPoint(PointF point, int angle, float size)
        {
            PointF result = new PointF((float)(Math.Cos(angle * Math.PI / 180) * size + point.X), (float)(Math.Sin(angle * Math.PI / 180) * size + point.Y));
            return result;
        }



    }

    //класс для генерации линий
    public class LineGenerator
    {
        //вспомогательная функция для алгоритма Брезенхейма
        static private Point[] getLineHigh(int x0, int y0, int x1, int y1)
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

        //вспомогательная функция для алгоритма Брезенхейма
        static private Point[] getLineLow(int x0, int y0, int x1, int y1)
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

        //возвращает список точек для прямой с помощью алгоритма Брезенхейма
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

        //получает список точек для широкой прямой с помощью алгоритма Брезенхейма и дорисовки квадратов вокруг каждой точки
        static public List<Point> getWideLineCoordsByBresenham(Point p0, Point p1, int width)
        {
            List<Point> points = new List<Point>();

            Point[] linePoints = getLineCoordsByBresenham(p0, p1);

            int halfThickness = width / 2;
            int halfThicknessPlus = halfThickness;
            if (width % 2 == 0)
            {
                halfThicknessPlus--;
            }

            foreach (var point in linePoints)
            {
                for (int dx = -halfThickness; dx <= halfThicknessPlus; dx++)
                {
                    for (int dy = -halfThickness; dy <= halfThicknessPlus; dy++)
                    {
                        points.Add(new Point(point.X + dx, point.Y + dy));
                    }
                }
            }
            return points.Distinct().ToList();
        }

        //получаем список точек из списка вершин путем соединения каждой вершины со следующей в порядке данного
        static public List<Point> GetPointsForVerices(List<Point> verices)
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
            return polygonPoints;
        }

        //получаем список точек для списка пар точек(где 1 точка - начало, 2 - конец отрезка)
        static public List<Point> GetPointsForLines(List<(Point, Point)> lines)
        {
            List<Point> points = new List<Point>();
            for (int i = 0; i < lines.Count; i++)
            {
                points.AddRange(getLineCoordsByBresenham(lines[i].Item1, lines[i].Item2).ToArray());
            }
            return points;
        }
    }

    //вспомогательный класс хз как назвать
    public class Helper
    {
        //масштабирует точки на весь экран с возможным расстягиванием по 1 из осей, но при этой на весь экран
        public static void scallingFullWindowWithDeformation(ref List<PointF> points, float minX, float minY, float maxX, float maxY, float percent)
        {
            //получаем значения X и Y на границах списка точек
            float pointsMinX = points.Min(p => p.X);
            float pointsMinY = points.Min(p => p.Y);
            float pointsMaxX = points.Max(p => p.X);
            float pointsMaxY = points.Max(p => p.Y);
            //находим ширину и длину которую они занимают
            float pointsWidth = pointsMaxX - pointsMinX;
            float pointsHeight = pointsMaxY - pointsMinY;
            //находим ширину и длину места куда нам надо перенести список точек
            float width = maxX - minX;
            float height = maxY - minY;
            //получаем центр места списка точек
            PointF pointsCenter = new PointF((pointsMaxX + pointsMinX)/2.0f, (pointsMaxY + pointsMinY) / 2.0f);
            //получаем центр того места куда нам надо перенести
            PointF center = new PointF((maxX + minX) / 2.0f, (maxY + minY) / 2.0f);
            //вычисляем на сколько нам надо перенести
            float dx = center.X - pointsCenter.X;
            float dy = center.Y - pointsCenter.Y;
            //масштабируем список точек с помощью афинных преобразований
            AffineTransformer.ScallingRelativePoint(ref points, width* percent / pointsWidth, height* percent / pointsHeight*(height/width), center.X, center.Y);
            //далее обновляем данные после машстабирования и теперь перемещаем список точек к центру того, куда мы хотели его перенести
            pointsMinX = points.Min(p => p.X);
            pointsMinY = points.Min(p => p.Y);
            pointsMaxX = points.Max(p => p.X);
            pointsMaxY = points.Max(p => p.Y);
            pointsCenter = new PointF((pointsMaxX + pointsMinX) / 2.0f, (pointsMaxY + pointsMinY) / 2.0f);
            dx = center.X - pointsCenter.X;
            dy = center.Y - pointsCenter.Y;
            AffineTransformer.OffsetDxDy(ref points, dx, dy);
        }

        //масштабирует точки на весь экран но по 1 из осей экран будет не заполнен но сохранится масштаб
        public static void scallingFullWindow(ref List<PointF> points, float minX, float minY, float maxX, float maxY, float percent=0.99f)
        {
            float pointsMinX = points.Min(p => p.X);
            float pointsMinY = points.Min(p => p.Y);
            float pointsMaxX = points.Max(p => p.X);
            float pointsMaxY = points.Max(p => p.Y);
            float pointsWidth = pointsMaxX - pointsMinX;
            float pointsHeight = pointsMaxY - pointsMinY;
            float width = maxX - minX;
            float height = maxY - minY;
            PointF pointsCenter = new PointF((pointsMaxX + pointsMinX) / 2.0f, (pointsMaxY + pointsMinY) / 2.0f);
            PointF center = new PointF((maxX + minX) / 2.0f, (maxY + minY) / 2.0f);
            float dx = center.X - pointsCenter.X;
            float dy = center.Y - pointsCenter.Y;
            //вычисляем длину на которую и будем масштабироваться
            float scaleX = width * percent / pointsWidth;
            float scaleY = height * percent / pointsHeight;
            float scale = Math.Min(scaleX, scaleY);
            AffineTransformer.ScallingRelativePoint(ref points, scale, scale, center.X, center.Y);
            pointsMinX = points.Min(p => p.X);
            pointsMinY = points.Min(p => p.Y);
            pointsMaxX = points.Max(p => p.X);
            pointsMaxY = points.Max(p => p.Y);
            pointsCenter = new PointF((pointsMaxX + pointsMinX) / 2.0f, (pointsMaxY + pointsMinY) / 2.0f);
            dx = center.X - pointsCenter.X;
            dy = center.Y - pointsCenter.Y;
            AffineTransformer.OffsetDxDy(ref points, dx, dy);
        }
    }
    
    //класс для афинных преобразований
    public class AffineTransformer
    {
        //умножение матриц
        public static float[,] MatrixMultiplication(ref float[,] m1, ref float[,] m2)
        {
            int row1 = m1.GetLength(0);
            int col1 = m1.GetLength(1);
            int row2 = m2.GetLength(0);
            int col2 = m2.GetLength(1);
            float[,] res = new float[row1, col2];
            for (int i = 0; i < row1; i++)
            {
                for (int j = 0; j < col2; j++)
                {
                    float cell = 0;
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
        public static PointF AffineTransformation(PointF p, ref float[,] TransMatrix)
        {
            float[,] matrix_p = new float[1, 3] { { p.X, p.Y, 1 } };
            float[,] matrix_res = MatrixMultiplication(ref matrix_p, ref TransMatrix);
            PointF res = new PointF(matrix_res[0, 0], matrix_res[0, 1]);
            return res;
        }

        //функция для  афинных преобразования списка точек на плоскости
        public static void AffineTransformationList(ref List<PointF> l, ref float[,] TransMatrix)
        {
            for (int i = 0; i < l.Count; i++)
            {
                l[i] = AffineTransformation(l[i], ref TransMatrix);
            }
        }

        //Сдвиг списка точек плоскости на dx и dy
        public static void OffsetDxDy(ref List<PointF> l, float dx, float dy)
        {
            float[,] transMatrix = new float[3, 3] {
                                                { 1, 0, 0 },
                                                { 0, 1, 0 },
                                                { dx, dy, 1 }
                                              };
            AffineTransformationList(ref l, ref transMatrix);
        }

        //Поворот вокруг центра плоскости на угол fi
        public static void RotationAroundCenterFi(ref List<PointF> l, float fi)
        {
            fi = fi * (float)Math.PI / 180;
            float[,] transMatrix = new float[3, 3] {
                                                { (float)Math.Cos(fi), (float)Math.Sin(fi), 0 },
                                                { (float)-Math.Sin(fi), (float)Math.Cos(fi), 0 },
                                                { 0, 0, 1},
                                               };
            AffineTransformationList(ref l, ref transMatrix);
        }

        //Поворот вокруг точки на угол fi
        public static void RotationAroundPointFi(ref List<PointF> l, float fi, float x, float y)
        {
            OffsetDxDy(ref l, -x, -y);
            RotationAroundCenterFi(ref l, fi);
            OffsetDxDy(ref l, x, y);
        }

        //Масштабирование относительно центра плоскости на коэффицента a, b
        public static void ScallingRelativeCenter(ref List<PointF> l, float a, float b)
        {
            float[,] transMatrix = new float[3, 3] {
                                                { a, 0, 0 },
                                                { 0, b, 0 },
                                                { 0, 0, 1},
                                               };
            AffineTransformationList(ref l, ref transMatrix);
        }

        //Масштабирование относительно точки на коэффиценты a, b
        public static void ScallingRelativePoint(ref List<PointF> l, float a, float b, float x, float y)
        {
            OffsetDxDy(ref l, -x, -y);
            ScallingRelativeCenter(ref l, a, b);
            OffsetDxDy(ref l, x, y);
        }
    }

}
