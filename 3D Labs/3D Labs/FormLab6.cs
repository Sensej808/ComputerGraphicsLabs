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
        System.Windows.Forms.Timer timer;
        Scene scene;
        Bitmap bitmap;
        Axis cameraMoveAxis;
        float cameraMoveLength;
        bool isMoving;
        List<Func<float, float, float>> funcs;
        Graphics g;
        Pen pen;
        bool isRotate;
        float cameraRotateAngle;
        RotationDirection rotationDirection;

        public FormLab6()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            funcs = new List<Func<float, float, float>>();
            funcs.Add((x, y) => x * x - y * y);
            funcs.Add((x, y) => 2 * x + 3 * y + 5);
            funcs.Add((x, y) => x * x + y * y);
            funcs.Add((x, y) => -(x * x) - y * y);
            funcs.Add((x, y) => 1 / (5 * (x * x + y * y) + 1));
            funcs.Add((x, y) => 10 * (MathF.Sin(x) + MathF.Sin(y)));
            funcs.Add((x, y) => MathF.Sqrt(x * x + y * y));
            funcs.Add((x, y) => 6 * MathF.Exp(-(x * x + y * y) / 2 * 16));

            comboBox2.Items.Add("Седло");
            comboBox2.Items.Add("Плоскость");
            comboBox2.Items.Add("Эллиптический параболоид вверх");
            comboBox2.Items.Add("Эллиптический параболоид вниз");
            comboBox2.Items.Add("Гиперболическая поверхность");
            comboBox2.Items.Add("Волновая поверхность (синусоидальная)");
            comboBox2.Items.Add("Конус");
            comboBox2.Items.Add(" Гауссова поверхность");

            timer = new System.Windows.Forms.Timer();
            timer.Interval = 1;
            timer.Tick += TimerTickCameraMove;

            cameraMoveAxis = Axis.X;
            isMoving = false;
            isRotate = false;
            cameraRotateAngle = 0;
            rotationDirection = RotationDirection.LeftRight;

            bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(bitmap);
            pen = Pens.Black;


            this.scene = new Scene(new List<Polyhedron>(), new Camera(), g, pen, bitmap, pictureBox1, richTextBox1, new List<LightSource>());
            /*
            scene.bitmap = bitmap;
            scene.g = g;
            scene.pen = pen;
            scene.picture = pictureBox1;
            */

            StartStateScene();

            for (int i = 0; i < scene.polyhedrons.Count; i++)
                comboBox1.Items.Add(i);

            DrawScene();
        }

        public void DrawScene()
        {
            g.Clear(pictureBox1.BackColor);
            scene.GetProjection(bitmap.Width, bitmap.Height);
            CameraPosText.Text = $"X: {scene.camera.position.X}  Y: {scene.camera.position.Y}  Z: {scene.camera.position.Z}";
            CameraTargetText.Text = $"X: {scene.camera.direction.X}  Y: {scene.camera.direction.Y}  Z: {scene.camera.direction.Z}";
            /*
            for (int i = 0; i < scene.polyhedrons[0].points.Count; i++)
            {
                richTextBox1.Text += $"{scene.polyhedrons[0].points[i].X} {scene.polyhedrons[0].points[i].Y} {scene.polyhedrons[0].points[i].Z}\r\n";
            }
            richTextBox1.Text += "\r\n";
            */
        }

        private void TimerTickCameraMove(object sender, EventArgs e)
        {
            if (isMoving)
            {
                scene.camera.MoveAlongAxis(cameraMoveAxis, cameraMoveLength);
                DrawScene();
            }
            if (isRotate)
            {
                scene.camera.CameraRotation(rotationDirection, cameraRotateAngle, richTextBox1);
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
                AffineTransform.ScallingRelativePoint(scene.polyhedrons[number].points, a, b, c, center.X, center.Y, center.Z);
                DrawScene();
                scene.polyhedrons[number].Update();
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
                scene.polyhedrons[number].Update();
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
                scene.polyhedrons[number].Update();
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
                scene.polyhedrons[number].Update();
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

        private void SaveEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
        }

        private void SaveDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            FIleNameText.Text = files[0];
        }

        private void CreateOBJ(object sender, EventArgs e)
        {
            Polyhedron polyhedron = OBJFormat.Read(FIleNameText.Text, richTextBox1);
            AffineTransform.Offset(polyhedron.points, scene.camera.position.X, scene.camera.position.Y, scene.camera.position.Z + 500);
            scene.polyhedrons.Add(polyhedron);
            comboBox1.Items.Add(scene.polyhedrons.Count - 1);
            scene.polyhedrons[scene.polyhedrons.Count - 1].Update();
            scene.polyhedrons[scene.polyhedrons.Count - 1].closed = true;
            DrawScene();
        }

        private void Save(object sender, EventArgs e)
        {
            int n = comboBox1.SelectedIndex;

            if (n != -1)
            {
                Graphic.Objects.Point center = scene.polyhedrons[n].Center();
                AffineTransform.Offset(scene.polyhedrons[n].points, -center.X, -center.Y, -center.Z);
                OBJFormat.Save(SaveFileNameText.Text, scene.polyhedrons[n]);
                AffineTransform.Offset(scene.polyhedrons[n].points, center.X, center.Y, center.Z);
            }
        }

        private void RotationCreate(object sender, EventArgs e)
        {
            string[] stringPoints = richTextBox1.Text.Split(";");
            List<Graphic.Objects.Point> rotationPoints = new List<Graphic.Objects.Point>();
            for (int i = 0; i < stringPoints.Length; i++)
            {
                string[] coordinates = stringPoints[i].Split(" ", StringSplitOptions.RemoveEmptyEntries);
                if (coordinates.Length > 2)
                {
                    rotationPoints.Add(new Graphic.Objects.Point(float.Parse(coordinates[0]), float.Parse(coordinates[1]), float.Parse(coordinates[2])));
                }
            }
            int rotationAxisNumber = 0;
            int.TryParse(RotationAxisNumberText.Text, out rotationAxisNumber);

            if (rotationAxisNumber >= 0 && rotationAxisNumber <= 2)
            {
                int partsCnt = 0;
                int.TryParse(partsNumberText.Text, out partsCnt);
                if (partsCnt > 0)
                {
                    Polyhedron rotatePolyhedron = RotationFigure.Create(rotationPoints, rotationAxisNumber, partsCnt);
                    AffineTransform.Offset(rotatePolyhedron.points, scene.camera.position.X, scene.camera.position.Y, scene.camera.position.Z + 500);
                    scene.polyhedrons.Add(rotatePolyhedron);
                    comboBox1.Items.Add(scene.polyhedrons.Count - 1);
                    DrawScene();
                }
            }

        }

        private void CameraRotate(object sender, MouseEventArgs e)
        {  
            Button clickedButton = sender as Button;
            if (e.Button == MouseButtons.Left)
            {
                isRotate = true;
                float fi = 3f;
                switch (clickedButton.Name)
                {
                    case "RotateUp":
                        cameraRotateAngle = -fi;
                        rotationDirection = RotationDirection.UpDown;
                        break;
                    case "RotateDown":
                        cameraRotateAngle = fi;
                        rotationDirection = RotationDirection.UpDown;
                        break;
                    case "RotateLeft":
                        cameraRotateAngle = -fi;
                        rotationDirection = RotationDirection.LeftRight;
                        break;
                    case "RotateRight":
                        cameraRotateAngle = fi;
                        rotationDirection = RotationDirection.LeftRight;
                        break;
                }
                timer.Start();
            }
        }

        private void CameraRotateStop(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isRotate = false;
                timer.Stop();
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            scene.camera.CameraRotation(RotationDirection.LeftRight, 90f, richTextBox1);
            DrawScene();
        }

        public void StartStateScene()
        {
            scene.camera.position.X = 0;
            scene.camera.position.Y = 800;
            scene.camera.position.Z = 0;

            scene.camera.target.X = 0;
            scene.camera.target.Y = 800;
            scene.camera.target.Z = 2000;
            scene.camera.CameraUpdateDirection();
            scene.lightSources.Add(new LightSource(new Graphic.Objects.Point(500, 1200, 800), 1));

            /*
            Polyhedron floor = Surface.Create(-6000, -6000, 6000, 6000, 10, 10, (x, y) => 0);
            AffineTransform.RotationAroundX(floor.points, 90);
            scene.polyhedrons.Add(floor);
            */

            
            Polyhedron octahedron = SimplePolyhedrons.Octahedron(150);
            AffineTransform.Offset(octahedron.points, -600, 801, 1500);
            scene.polyhedrons.Add(octahedron);
            
            Polyhedron tetrahedron = SimplePolyhedrons.Tetrahedron(200);
            AffineTransform.Offset(tetrahedron.points, -300, 800, 1500);
            scene.polyhedrons.Add(tetrahedron);

            Bitmap textureBitmapTetrahedron = new Bitmap("Tetrahedron.jpg");
            tetrahedron.texture = textureBitmapTetrahedron;
            tetrahedron.isTexture = true;

            for (int i = 0; i < tetrahedron.faces.Count; i++)
            {
                tetrahedron.faces[i].texturePoints = new List<float[]> { new float[2] { 0, 0 }, new float[2] { 767, 0 }, new float[2] { 388, 388 } };
            }


            Polyhedron cube = SimplePolyhedrons.Cube(200);
            Bitmap textureBitmap = new Bitmap("Minecraft.jpg");
            cube.texture = textureBitmap;
            cube.isTexture = true;

            cube.faces[0].texturePoints = new List<float[]> { new float[2] { 420, 410 }, new float[2] { 420, 50 }, new float[2] { 770, 50 }, new float[2] { 770, 410 } };
            cube.faces[1].texturePoints = new List<float[]> { new float[2] { 420, 410 }, new float[2] { 770, 410 }, new float[2] { 770, 760 }, new float[2] { 420, 760 } };
            cube.faces[2].texturePoints = new List<float[]> { new float[2] { 420, 410 }, new float[2] { 420, 50 }, new float[2] { 770, 50 }, new float[2] { 770, 410 } };
            cube.faces[3].texturePoints = new List<float[]> { new float[2] { 1130, 410 }, new float[2] { 1490, 410 }, new float[2] { 1490, 760 }, new float[2] { 1130, 760 } };
            cube.faces[4].texturePoints = new List<float[]> { new float[2] { 420, 50 }, new float[2] { 770, 50 }, new float[2] { 770, 410 }, new float[2] { 420, 410 } };
            cube.faces[5].texturePoints = new List<float[]> { new float[2] { 770, 50 }, new float[2] { 770, 410 }, new float[2] { 420, 410 }, new float[2] { 420, 50 } };

            //pictureBox1.Image = textureBitmap;
            //richTextBox1.Text = "";


            //AffineTransform.RotationAroundY(cube.points, 180);
            AffineTransform.Offset(cube.points, 0, 800, 1500);
            scene.polyhedrons.Add(cube);
                      
            
            scene.polyhedrons.Add(Surface.Create(-1, -1, 1, 1, 15, 15, funcs[0]));
            AffineTransform.RotationAroundY(scene.polyhedrons[scene.polyhedrons.Count - 1].points, 180);
            AffineTransform.ScallingRelativeCenter(scene.polyhedrons[scene.polyhedrons.Count - 1].points, 100, 100, 100);
            AffineTransform.Offset(scene.polyhedrons[scene.polyhedrons.Count - 1].points, 300, 800, 1500);

            List<Graphic.Objects.Point> rotationPoints = new List<Graphic.Objects.Point>();
            int x = -100;

            for (int i = 0; i <= 20; i++)
            {
                x += 100 / 10 * i;
                rotationPoints.Add(new Graphic.Objects.Point(x, MathF.Sqrt(10000 - x * x), 0));
                x -= 100 / 10 * i;
            }

            Polyhedron ball = RotationFigure.Create(rotationPoints, 0, 20);
            Bitmap textureBitmapSphere = new Bitmap("Sphere2.jpg");
            ball.texture = textureBitmapSphere;
            ball.isTexture = true;

            for (int i = 0; i < ball.faces.Count; i++)
            {
                ball.faces[i].texturePoints = new List<float[]> { new float[2] { 732, 431 }, new float[2] { 850, 197 }, new float[2] { 975, 431 } };
            }

            AffineTransform.Offset(ball.points, 600, 800, 1500);
            scene.polyhedrons.Add(ball);
            scene.polyhedrons[scene.polyhedrons.Count - 1].closed = true;

            /*


            
            scene.polyhedrons.Add(Surface.Create(-100, -100, 100, 100, 10, 10, funcs[1]));
            AffineTransform.Offset(scene.polyhedrons[scene.polyhedrons.Count - 1].points, 600, 800, 1500);

            scene.polyhedrons.Add(Surface.Create(-1, -1, 1, 1, 15, 15, funcs[2]));
            AffineTransform.ScallingRelativeCenter(scene.polyhedrons[scene.polyhedrons.Count - 1].points, 100, 100, 100);
            AffineTransform.Offset(scene.polyhedrons[scene.polyhedrons.Count - 1].points, 900, 800, 1500);

            scene.polyhedrons.Add(Surface.Create(-1, -1, 1, 1, 15, 15, funcs[3]));
            AffineTransform.ScallingRelativeCenter(scene.polyhedrons[scene.polyhedrons.Count - 1].points, 100, 100, 100);
            AffineTransform.Offset(scene.polyhedrons[scene.polyhedrons.Count - 1].points, 1200, 800, 1500);

            scene.polyhedrons.Add(Surface.Create(-1, -1, 1, 1, 15, 15, funcs[4]));
            AffineTransform.RotationAroundX(scene.polyhedrons[scene.polyhedrons.Count - 1].points, 270);
            AffineTransform.ScallingRelativeCenter(scene.polyhedrons[scene.polyhedrons.Count - 1].points, 100, 100, 100);
            AffineTransform.Offset(scene.polyhedrons[scene.polyhedrons.Count - 1].points, 1500, 800, 1500);

            scene.polyhedrons.Add(Surface.Create(-100, -100, 100, 100, 30, 30, funcs[5]));
            AffineTransform.RotationAroundX(scene.polyhedrons[scene.polyhedrons.Count - 1].points, 90);
            AffineTransform.Offset(scene.polyhedrons[scene.polyhedrons.Count - 1].points, 1800, 800, 1500);

            scene.polyhedrons.Add(Surface.Create(-100, -100, 100, 100, 10, 10, funcs[6]));
            AffineTransform.RotationAroundX(scene.polyhedrons[scene.polyhedrons.Count - 1].points, 90);
            AffineTransform.Offset(scene.polyhedrons[scene.polyhedrons.Count - 1].points, 2100, 800, 1500);

            scene.polyhedrons.Add(Surface.Create(-1, -1, 1, 1, 20, 20, funcs[7]));
            AffineTransform.RotationAroundX(scene.polyhedrons[scene.polyhedrons.Count - 1].points, 270);
            AffineTransform.ScallingRelativeCenter(scene.polyhedrons[scene.polyhedrons.Count - 1].points, 100, 100, 100);
            AffineTransform.Offset(scene.polyhedrons[scene.polyhedrons.Count - 1].points, 2400, 800, 1500);
            
            List<Graphic.Objects.Point> rotationPoints = new List<Graphic.Objects.Point>();
            int x = -100;

            for (int i = 0; i <= 20; i++)
            {
                x += 100 / 10 * i;
                rotationPoints.Add(new Graphic.Objects.Point(x, MathF.Sqrt(10000 - x * x), 0));
                x -= 100 / 10 * i;
            }

            Polyhedron ball = RotationFigure.Create(rotationPoints, 0, 20);
            AffineTransform.Offset(ball.points, 2700, 800, 1500);
            scene.polyhedrons.Add(ball);
            scene.polyhedrons[scene.polyhedrons.Count - 1].closed = true;

            List<Graphic.Objects.Point> circlePoints = new List<Graphic.Objects.Point>();
            for (int i = 0; i < rotationPoints.Count; i++)
                circlePoints.Add(new Graphic.Objects.Point(rotationPoints[i].X, rotationPoints[i].Y, rotationPoints[i].Z));

            for (int i = rotationPoints.Count - 1; i >= 0; i--)
                circlePoints.Add(new Graphic.Objects.Point(rotationPoints[i].X, -rotationPoints[i].Y, rotationPoints[i].Z));

            AffineTransform.Offset(circlePoints, -500, 0, 0);
            Polyhedron bagel = RotationFigure.Create(circlePoints, 1, 20);
            AffineTransform.RotationAroundX(bagel.points, 90);
            AffineTransform.Offset(bagel.points, 3500, 800, 1500);
            scene.polyhedrons.Add(bagel);
            scene.polyhedrons[scene.polyhedrons.Count - 1].closed = true;

            List<Graphic.Objects.Point> parabola = new List<Graphic.Objects.Point>();
            x = -100;
            for (int i = -0; i <= 20; i++)
            {
                x += 100 / 10 * i;
                parabola.Add(new Graphic.Objects.Point(x, x * x / 225 + 10, 0));
                x -= 100 / 10 * i;
            }
            Polyhedron hyperboloid = RotationFigure.Create(parabola, 0, 20);
            AffineTransform.ScallingRelativeCenter(hyperboloid.points, 5, 5, 5);
            AffineTransform.RotationAroundZ(hyperboloid.points, 90);
            AffineTransform.Offset(hyperboloid.points, 4500, 800, 1500);
            scene.polyhedrons.Add(hyperboloid);
            */

            //richTextBox1.Text += $"{scene.polyhedrons[0].faces[5].normalVector.X} {scene.polyhedrons[0].faces[5].normalVector.Y} {scene.polyhedrons[0].faces[5].normalVector.Z}\r\n";
            for (int i = 0; i < scene.polyhedrons.Count; i++)
            {
                scene.polyhedrons[i].Update();
                //richTextBox1.Text += $"{scene.polyhedrons[i].points[0].NormalVector[0]} {scene.polyhedrons[i].points[0].NormalVector[1]} {scene.polyhedrons[i].points[0].NormalVector[2]}\r\n";
                //richTextBox1.Text += $"{scene.polyhedrons[0].faces[i].normalVector.X} {scene.polyhedrons[0].faces[i].normalVector.Y} {scene.polyhedrons[0].faces[i].normalVector.Z}\r\n"; 
            }
            //richTextBox1.Text += $"{scene.polyhedrons[0].faces[5].normalVector.X} {scene.polyhedrons[0].faces[5].normalVector.Y} {scene.polyhedrons[0].faces[5].normalVector.Z}\r\n";
            
            /*
            for (int i = 0; i < scene.polyhedrons.Count; i++)
            {
                scene.polyhedrons[i].Update();
            }
            */
        }
    }
}
