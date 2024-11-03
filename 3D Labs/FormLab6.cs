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

namespace _3D_Labs
{
    public partial class FormLab6 : Form
    {
        List<Polyhedron> polyhedrons;

        Scene scene;
        static Bitmap bitmap;
        public FormLab6()
        {
            InitializeComponent();
            bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            this.scene = new Scene();

            Polyhedron tetrahedron = SimplePolyhedrons.Tetrahedron(200);
            AffineTransform.Offset(tetrahedron.points, 0, 0, 500);
            scene.polyhedrons.Add(tetrahedron);

            scene.camera.target.X = 0;
            scene.camera.target.Y = 0;
            scene.camera.target.Z = 10000;
        }

        private void CreateTetrahedron(object sender, EventArgs e)
        {
            
            List<Graphic.Objects.Polyhedron> projection = scene.GetProjection(bitmap.Width, bitmap.Height, richTextBox1);

            /*
            for (int i = 0; i < projection[0].points.Count; i++)
            {
                richTextBox1.Text += $"{projection[0].points[i].X} {projection[0].points[i].Y} {projection[0].points[i].Z}\r\n";
            }
            */
            //richTextBox1.Text += $"\r\n----------------------\r\n";
            Graphics g = Graphics.FromImage(bitmap);
            for (int i = 0; i < projection.Count; i++)
            {
                //richTextBox1.Text += $"{i} ";
                for (int j = 0; j < projection[i].edges.Count; j++)
                {
                    //richTextBox1.Text += $"{j} ";
                    var x = new PointF(projection[i].edges[j].FirstPoint.X, projection[i].edges[j].FirstPoint.Y);
                    var xx = new PointF(projection[i].edges[j].SecondPoint.X, projection[i].edges[j].SecondPoint.Y);
                    //richTextBox1.Text += $"{projection[i].edges[j].FirstPoint.X} {projection[i].edges[j].FirstPoint.Y} {projection[i].edges[j].SecondPoint.X} {projection[i].edges[j].SecondPoint.Y}\r\n";
                    g.DrawLine(Pens.Black, x,xx);
                    //richTextBox1.Text += "Fix";
                }
                //richTextBox1.Text += $"\r\n----------------------\r\n";
            }
            pictureBox1.Image = bitmap;
            
         
            //Polyhedron tetrahedron= SimplePolyhedrons.Tetrahedron(200);
            /*
            Polyhedron tetrahedron = scene.polyhedrons[0];
            //AffineTransform.RotationAroundY(tetrahedron.points, 45);
            //AffineTransform.RotationAroundX(tetrahedron.points, -15);
            //AffineTransform.RotationAroundZ(tetrahedron.points, 15);
            //AffineTransform.Offset(tetrahedron.points, 0, 0, 600);

            Graphic.Objects.Point cameraPosition = new Graphic.Objects.Point(0, 0, 0);
            Graphic.Objects.Point cameraTarget = new Graphic.Objects.Point(0, 0, 10000);
            Graphic.Objects.Point up = new Graphic.Objects.Point(0, 1, 0);

            //Graphic.Objects.Point cameraDirection = cameraPosition - cameraTarget;
            Graphic.Objects.Point cameraDirection = cameraTarget - cameraPosition;
            Mathematics.Vector.Normalization(cameraDirection.CoordinateVector);
            
            Graphic.Objects.Point cameraRight = new Graphic.Objects.Point(0, 0, 0);
            cameraRight.CoordinateVector = Mathematics.Vector.Multiplication(up.CoordinateVector, cameraDirection.CoordinateVector);
            Mathematics.Vector.Normalization(cameraRight.CoordinateVector);

            Graphic.Objects.Point cameraUp = new Graphic.Objects.Point(0, 0, 0);
            cameraUp.CoordinateVector = Mathematics.Vector.Multiplication(cameraDirection.CoordinateVector, cameraRight.CoordinateVector);
            
            float[,] viewMatrixPart1 = new float[4, 4]
            {
                { cameraRight.X, cameraUp.X, cameraDirection.X, 0  },
                { cameraRight.Y, cameraUp.Y, cameraDirection.Y, 0 },
                { cameraRight.Z, cameraUp.Z, cameraDirection.Z, 0 },
                { 0, 0, 0, 1}
            };
            float[,] viewMatrixPart2 = new float[4, 4]
            {
                { 1, 0, 0, 0},
                { 0, 1, 0, 0},
                { 0, 0, 1, 0},
                { -cameraPosition.X, -cameraPosition.Y, -cameraPosition.Z, 1}
            };
            
            float[,] viewMatrix = Mathematics.Matrix.Multiplication(viewMatrixPart1, viewMatrixPart2);
            float fovy = 60f;
            fovy = fovy / 180f * MathF.PI;
            float aspect = (float)bitmap.Width / (float)bitmap.Height;
            float n = 10f;
            float f = 1000f;
            
            float[,] projectionMatrix = new float[4, 4]
            {
                { (1f/MathF.Tan(fovy/2))/aspect, 0, 0, 0 },
                { 0, (1f/MathF.Tan(fovy/2)), 0, 0 },
                { 0, 0, (f+n)/(f-n), 1},
                { 0, 0, (-2*f*n)/(f-n), 0}
            };

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    richTextBox1.Text += $"{projectionMatrix[i, j]} ";
                }
                richTextBox1.Text += "\r\n";
            }



            AffineTransform.AffineTransformationList(tetrahedron.points, viewMatrix);
            
            //for (int i = 0; i < tetrahedron.points.Count; i++)
            //{
            //    richTextBox1.Text += $"{tetrahedron.points[i].X} {tetrahedron.points[i].Y} {tetrahedron.points[i].Z}\r\n";
            //}
            
            AffineTransform.AffineTransformationList(tetrahedron.points, projectionMatrix);
            
            for (int i = 0; i < tetrahedron.points.Count; i++)
            {
                richTextBox1.Text += $"{tetrahedron.points[i].X} {tetrahedron.points[i].Y} {tetrahedron.points[i].Z}\r\n";
            }
            
            richTextBox1.Text += ("----------------------------\r\n");

            for (int i = 0; i < tetrahedron.points.Count; i++)
            {
                float w = tetrahedron.points[i].CoordinateVector[3];
                if (w != 0)
                {
                    tetrahedron.points[i].X /= w;
                    tetrahedron.points[i].Y /= w;
                    tetrahedron.points[i].Z /= w;
                }
                //richTextBox1.Text += $"{tetrahedron.points[i].X} {tetrahedron.points[i].Y} {tetrahedron.points[i].Z}\r\n";
                tetrahedron.points[i].X = ((tetrahedron.points[i].X + 1) / 2)* bitmap.Width;
                tetrahedron.points[i].Y = ((1 - tetrahedron.points[i].Y) / 2)* bitmap.Height;

                //richTextBox1.Text += $"{tetrahedron.points[i].X} {tetrahedron.points[i].Y} {tetrahedron.points[i].Z}\r\n";
            }

            for (int i = 0; i < tetrahedron.points.Count; i++)
            {
                richTextBox1.Text += $"{tetrahedron.points[i].X} {tetrahedron.points[i].Y} {tetrahedron.points[i].Z}\r\n";
            }
            Graphics g = Graphics.FromImage(bitmap);
            for (int i = 0; i < tetrahedron.edges.Count; i++)
            {
                g.DrawLine(Pens.Black, new PointF(tetrahedron.edges[i].FirstPoint.X, tetrahedron.edges[i].FirstPoint.Y),
                                       new PointF(tetrahedron.edges[i].SecondPoint.X, tetrahedron.edges[i].SecondPoint.Y));
            }
            pictureBox1.Image = bitmap;
            */
        }
    }
}
