using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using _3D_Labs.Graphic;
using _3D_Labs.Graphic.Objects;
using System.Diagnostics;
using System.Net.Mail;

namespace _3D_Labs
{
    public partial class FormLab6 : Form
    {
        List<Polyhedron> polyhedrons;
        System.Windows.Forms.Timer timer;
        Scene scene;
        static Bitmap bitmap;
        Axis cameraMoveAxis;
        float cameraMoveLength;
        bool isMoving;
        List<Func<float, float, float>> funcs;

        public FormLab6()
        {
            InitializeComponent();
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 1; 
            timer.Tick += TimerTickCameraMove;

            cameraMoveAxis = Axis.X;
            isMoving = false;
            funcs = new List<Func<float, float, float>>();

            bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            this.scene = new Scene();

            Polyhedron tetrahedron = SimplePolyhedrons.Tetrahedron(200);

            AffineTransform.Offset(tetrahedron.points, 0, 0, 1500);
            scene.polyhedrons.Add(tetrahedron);

            for (int i = 0; i < scene.polyhedrons.Count; i++)
            {
                comboBox1.Items.Add(i);
            }

            funcs.Add((x, y) => x * x - y * y);
            funcs.Add((x, y) => 2 * x + 3 * y + 5);
            funcs.Add((x, y) => x * x + y * y);
            funcs.Add((x, y) => -(x * x) - y * y);
            funcs.Add((x, y) => 1 / (5 * (x * x + y * y) + 1));
            funcs.Add((x, y) => 10* (MathF.Sin(x) + MathF.Sin(y)));
            funcs.Add((x, y) => MathF.Sqrt(x*x + y*y));
            funcs.Add((x, y) => 6*MathF.Exp(-(x*x+y*y)/2*16));

            comboBox2.Items.Add("Седло");
            comboBox2.Items.Add("Плоскость");
            comboBox2.Items.Add("Эллиптический параболоид вверх");
            comboBox2.Items.Add("Эллиптический параболоид вниз");
            comboBox2.Items.Add("Гиперболическая поверхность");
            comboBox2.Items.Add("Волновая поверхность (синусоидальная)");
            comboBox2.Items.Add("Конус");
            comboBox2.Items.Add(" Гауссова поверхность");


            scene.polyhedrons.Add(Surface.Create(-1, -1, 1, 1, 15, 15, funcs[0]));
            AffineTransform.ScallingRelativeCenter(scene.polyhedrons[scene.polyhedrons.Count - 1].points, 100, 100, 100);
            AffineTransform.Offset(scene.polyhedrons[scene.polyhedrons.Count - 1].points, 300, 0, 1500);
            comboBox1.Items.Add(scene.polyhedrons.Count - 1);

            scene.polyhedrons.Add(Surface.Create(-100, -100, 100, 100, 10, 10, funcs[1]));
            AffineTransform.Offset(scene.polyhedrons[scene.polyhedrons.Count - 1].points, 600, 0, 1500);
            comboBox1.Items.Add(scene.polyhedrons.Count - 1);

            scene.polyhedrons.Add(Surface.Create(-1, -1, 1, 1, 15, 15, funcs[2]));
            AffineTransform.ScallingRelativeCenter(scene.polyhedrons[scene.polyhedrons.Count - 1].points, 100, 100, 100);
            AffineTransform.Offset(scene.polyhedrons[scene.polyhedrons.Count - 1].points, 900, 0, 1500);
            comboBox1.Items.Add(scene.polyhedrons.Count - 1);

            scene.polyhedrons.Add(Surface.Create(-1, -1, 1, 1, 15, 15, funcs[3]));
            AffineTransform.ScallingRelativeCenter(scene.polyhedrons[scene.polyhedrons.Count - 1].points, 100, 100, 100);
            AffineTransform.Offset(scene.polyhedrons[scene.polyhedrons.Count - 1].points, 1200, 0, 1500);
            comboBox1.Items.Add(scene.polyhedrons.Count - 1);

            scene.polyhedrons.Add(Surface.Create(-1, -1, 1, 1, 15, 15, funcs[4]));
            AffineTransform.RotationAroundX(scene.polyhedrons[scene.polyhedrons.Count - 1].points, 270);
            AffineTransform.ScallingRelativeCenter(scene.polyhedrons[scene.polyhedrons.Count - 1].points, 100, 100, 100);
            AffineTransform.Offset(scene.polyhedrons[scene.polyhedrons.Count - 1].points, 1500, 0, 1500);
            comboBox1.Items.Add(scene.polyhedrons.Count - 1);

            scene.polyhedrons.Add(Surface.Create(-100, -100, 100, 100, 30, 30, funcs[5]));
            AffineTransform.RotationAroundX(scene.polyhedrons[scene.polyhedrons.Count - 1].points, 90);
            AffineTransform.Offset(scene.polyhedrons[scene.polyhedrons.Count - 1].points, 1800, 0, 1500);
            comboBox1.Items.Add(scene.polyhedrons.Count - 1);

            scene.polyhedrons.Add(Surface.Create(-100, -100, 100, 100, 10, 10, funcs[6]));
            AffineTransform.RotationAroundX(scene.polyhedrons[scene.polyhedrons.Count - 1].points, 90);
            AffineTransform.Offset(scene.polyhedrons[scene.polyhedrons.Count - 1].points, 2100, 0, 1500);
            comboBox1.Items.Add(scene.polyhedrons.Count - 1);

            scene.polyhedrons.Add(Surface.Create(-1, -1, 1, 1, 20, 20, funcs[7]));
            AffineTransform.RotationAroundX(scene.polyhedrons[scene.polyhedrons.Count - 1].points, 270);
            AffineTransform.ScallingRelativeCenter(scene.polyhedrons[scene.polyhedrons.Count - 1].points, 100, 100, 100);
            AffineTransform.Offset(scene.polyhedrons[scene.polyhedrons.Count - 1].points, 2400, -100, 1500);
            comboBox1.Items.Add(scene.polyhedrons.Count - 1);

            scene.camera.position.X = 1100;
            scene.camera.position.Y = 0;
            scene.camera.position.Z = -800;

            scene.camera.target.X = 1100;
            scene.camera.target.Y = 0;
            scene.camera.target.Z = 9200;

            DrawScene();
        }

        public void DrawScene()
        {
            bitmap = null;
            bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics g = Graphics.FromImage(bitmap);
            List<Graphic.Objects.Edge> edges = scene.GetProjection(bitmap.Width, bitmap.Height);

            for (int j = 0; j < edges.Count; j++)
            {
                
                var x = new PointF(edges[j].FirstPoint.X, edges[j].FirstPoint.Y);
                var xx = new PointF(edges[j].SecondPoint.X, edges[j].SecondPoint.Y);
                if ((edges[j].FirstPoint.Z > 0 && edges[j].SecondPoint.Z > 0 && scene.projectionNumber == 0) || scene.projectionNumber == 1)
                    g.DrawLine(Pens.Black, x, xx);
                
                //richTextBox1 += $"{x.X} {x.Y} | {xx.X} {xx.Y}\r\n";
            }
            pictureBox1.Image = bitmap;

            CameraPosText.Text = $"X: {scene.camera.position.X}  Y: {scene.camera.position.Y}  Z: {scene.camera.position.Z}";
            CameraTargetText.Text = $"X: {scene.camera.target.X}  Y: {scene.camera.target.Y}  Z: {scene.camera.target.Z}";
        }

        private void TimerTickCameraMove(object sender, EventArgs e)
        {
            if (isMoving)
            {
                scene.camera.MoveAlongAxis(cameraMoveAxis, cameraMoveLength);
                DrawScene();
            }
        }

        private void CameraMove(object sender, MouseEventArgs e)
        {
            Button clickedButton = sender as Button;
            if (e.Button == MouseButtons.Left)
            {
                isMoving = true;
                float speed = 10;
                if (textBox1.Text.Length > 0)
                    float.TryParse(textBox1.Text, out speed);
                switch(clickedButton.Name)
                {
                    case "Up":
                        cameraMoveLength = speed;
                        cameraMoveAxis = Axis.Y;
                        break;
                    case "Down":
                        cameraMoveLength = -speed;
                        cameraMoveAxis = Axis.Y;
                        break;
                    case "Right":
                        cameraMoveLength = speed;
                        cameraMoveAxis = Axis.X;
                        break;
                    case "Left":
                        cameraMoveLength = -speed;
                        cameraMoveAxis = Axis.X;
                        break;
                    case "Forward":
                        cameraMoveLength = speed;
                        cameraMoveAxis = Axis.Z;
                        break;
                    case "Backward":
                        cameraMoveLength = -speed;
                        cameraMoveAxis = Axis.Z;
                        break;
                }
                
                timer.Start(); 
            }
        }

        private void CameraMoveStop(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isMoving = false;
                timer.Stop(); 
            }
        }

        private void ObjectMove(object sender, EventArgs e)
        {
            float dx = 0;
            float dy = 0;
            float dz = 0;
            float.TryParse(DxText.Text, out dx);
            float.TryParse(DyText.Text, out dy);
            float.TryParse(DzText.Text, out dz);
            int number = -1;
            int.TryParse(comboBox1.Text, out number);
            if (number != -1)
            {
                AffineTransform.Offset(scene.polyhedrons[number].points, dx, dy, dz);
                //AffineTransform.Offset(tetrahedron., dx, dy, dz);
                DrawScene();
            }
        }

        private void DyText_TextChanged(object sender, EventArgs e)
        {

        }

        private void ScallingRelativeObjectCenter(object sender, EventArgs e)
        {
            float a = 1;
            float b = 1;
            float c = 1;
            if (aText.Text.Length > 0)
                float.TryParse(aText.Text, out a);
            if (bText.Text.Length > 0)
                float.TryParse(bText.Text, out b);
            if (cText.Text.Length > 0)
                float.TryParse(cText.Text, out c);
            int number = -1;
            int.TryParse(comboBox1.Text, out number);
            if (number != -1)
            {
                Graphic.Objects.Point center = scene.polyhedrons[number].Center();
                //richTextBox1.Text += $"{center.X} {center.Y} {center.Z}";
                AffineTransform.ScallingRelativePoint(scene.polyhedrons[number].points, a, b, c, center.X, center.Y, center.Z);
                DrawScene();
            }
        }

        private void ReflectionCoordinatePlane(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            int number = -1;
            int.TryParse(comboBox1.Text, out number);
            if (number != -1)
            {
                switch (clickedButton.Name)
                {
                    case "XY":
                        AffineTransform.reflectionXY(scene.polyhedrons[number].points);
                        break;
                    case "XZ":
                        AffineTransform.reflectionXZ(scene.polyhedrons[number].points);
                        break;
                    case "YZ":
                        AffineTransform.reflectionYZ(scene.polyhedrons[number].points);
                        break;
                }
                DrawScene();
            }
        }

        private void RotationAroundLineParallelAxisPassingObjectCenter(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            int number = -1;
            float fi = 0;
            float.TryParse(TextFi.Text, out fi);
            int.TryParse(comboBox1.Text, out number);
            if (number != -1)
            {
                Graphic.Objects.Point center = scene.polyhedrons[number].Center();
                
                switch (clickedButton.Name)
                {
                    case "Axis0X":
                        AffineTransform.RotationAroundLineParallelXPassingPoint(scene.polyhedrons[number].points, fi, center.X, center.Y, center.Z);
                        break;
                    case "Axis0Y":
                        AffineTransform.RotationAroundLineParallelYPassingPoint(scene.polyhedrons[number].points, fi, center.X, center.Y, center.Z);
                        break;
                    case "Axis0Z":
                        AffineTransform.RotationAroundLineParallelZPassingPoint(scene.polyhedrons[number].points, fi, center.X, center.Y, center.Z);
                        break;
                }
                
                DrawScene();
            }
        }

        private void RotationAroundStraight(object sender, EventArgs e)
        {
            int number = -1;
            float fi = 0;
            float.TryParse(TextFi.Text, out fi);
            int.TryParse(comboBox1.Text, out number);

            float x1 = 0;
            float y1 = 0;
            float z1 = 0;

            float x2 = 0;
            float y2 = 0;
            float z2 = 0;

            float.TryParse(x1Text.Text, out x1);
            float.TryParse(y1Text.Text, out y1);
            float.TryParse(z1Text.Text, out z1);

            float.TryParse(x2Text.Text, out x2);
            float.TryParse(y2Text.Text, out y2);
            float.TryParse(z2Text.Text, out z2);

            if (number != -1)
            {
                AffineTransform.RotationAroundStraight(scene.polyhedrons[number].points, fi, x1, y1, z1, x2, y2, z2);
                DrawScene();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            scene.projectionNumber = 0;
            DrawScene();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            scene.projectionNumber = 1;
            DrawScene();
        }

        private void CreateSurface(object sender, EventArgs e)
        {
            float x1 = 0;
            float y1 = 0;
            float x2 = 0;
            float y2 = 0;

            if (SurfaceX1.Text.Length > 0)
                float.TryParse(SurfaceX1.Text, out x1);

            if (SurfaceY1.Text.Length > 0)
                float.TryParse(SurfaceY1.Text, out y1);

            if (SurfaceX2.Text.Length > 0)
                float.TryParse(SurfaceX2.Text, out x2);

            if (SurfaceY2.Text.Length > 0)
                float.TryParse(SurfaceY2.Text, out y2);

            int partitionsCountX = 1;
            int partitionsCountY = 1;

            if (SurfaceCountX.Text.Length > 0)
                int.TryParse(SurfaceCountX.Text, out partitionsCountX);

            if (SurfaceCountY.Text.Length > 0)
                int.TryParse(SurfaceCountY.Text, out partitionsCountY);

            if (partitionsCountX < 1)
                partitionsCountX = 1;

            if (partitionsCountY < 1)
                partitionsCountY = 1;

            int funcNumber = comboBox2.SelectedIndex;

            if (funcNumber != -1)
            {
                scene.polyhedrons.Add(Surface.Create(x1, y1, x2, y2, partitionsCountX, partitionsCountY, funcs[funcNumber]));
                comboBox1.Items.Add(scene.polyhedrons.Count-1);
                if (funcNumber == 0 || funcNumber == 2 || funcNumber == 3)
                {
                    AffineTransform.ScallingRelativeCenter(scene.polyhedrons[scene.polyhedrons.Count - 1].points, 10, 10, 10);
                    AffineTransform.Offset(scene.polyhedrons[scene.polyhedrons.Count - 1].points, scene.camera.position.X, scene.camera.position.Y, scene.camera.position.Z + 100);
                }
                else if (funcNumber == 1)
                {
                    AffineTransform.Offset(scene.polyhedrons[scene.polyhedrons.Count - 1].points, scene.camera.position.X, scene.camera.position.Y, scene.camera.position.Z + 500);
                }
                else if (funcNumber == 4)
                {
                    AffineTransform.ScallingRelativeCenter(scene.polyhedrons[scene.polyhedrons.Count - 1].points, 10, 10, 10);
                    AffineTransform.RotationAroundX(scene.polyhedrons[scene.polyhedrons.Count - 1].points, 270);
                    AffineTransform.Offset(scene.polyhedrons[scene.polyhedrons.Count - 1].points, scene.camera.position.X, scene.camera.position.Y-5, scene.camera.position.Z + 40);
                }
                else if (funcNumber == 6)
                {
                    AffineTransform.RotationAroundX(scene.polyhedrons[scene.polyhedrons.Count - 1].points, 90);
                    AffineTransform.Offset(scene.polyhedrons[scene.polyhedrons.Count - 1].points, scene.camera.position.X, scene.camera.position.Y, scene.camera.position.Z + 100);
                }
                else if (funcNumber == 7)
                {
                    AffineTransform.ScallingRelativeCenter(scene.polyhedrons[scene.polyhedrons.Count - 1].points, 10, 10, 10);
                    AffineTransform.RotationAroundX(scene.polyhedrons[scene.polyhedrons.Count - 1].points, 270);
                    AffineTransform.Offset(scene.polyhedrons[scene.polyhedrons.Count - 1].points, scene.camera.position.X, scene.camera.position.Y- 25, scene.camera.position.Z + 100);

                }
                else
                {
                    AffineTransform.Offset(scene.polyhedrons[scene.polyhedrons.Count - 1].points, scene.camera.position.X, scene.camera.position.Y, scene.camera.position.Z + 100);
                }
                DrawScene();
            }
        }

        
    }
}
