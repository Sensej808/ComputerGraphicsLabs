using _3D_Labs.Graphic.Objects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using static System.Formats.Asn1.AsnWriter;


namespace _3D_Labs.Graphic
{
    public class Scene
    {
        public List<Graphic.Objects.Polyhedron> polyhedrons;
        public Camera camera;
        public int projectionNumber;

        public Scene(List<Polyhedron> polyhedrons, Camera camera)
        {
            this.polyhedrons = polyhedrons;
            this.camera = camera;
            this.projectionNumber = 0;
        }

        public Scene()
        {
            this.polyhedrons = new List<Polyhedron>();
            this.camera = new Camera();
            this.projectionNumber = 0;
        }

        public List<Objects.Edge> GetProjection(int width, int height)
        {
            List<Objects.Point> oldPoints = new List<Objects.Point>();
            for (int i = 0; i < polyhedrons.Count; i++)
            {
                for (int j = 0; j < polyhedrons[i].points.Count; j++)
                {
                    oldPoints.Add(new Objects.Point(polyhedrons[i].points[j].X, polyhedrons[i].points[j].Y, polyhedrons[i].points[j].Z));
                }
            }

            Objects.Point cameraDirection = camera.target - camera.position;
            Mathematics.Vector.Normalization(cameraDirection.CoordinateVector);
            Objects.Point cameraRight = new Graphic.Objects.Point(0, 0, 0);
            cameraRight.CoordinateVector = Mathematics.Vector.Multiplication(camera.up.CoordinateVector, cameraDirection.CoordinateVector);
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
                { -camera.position.X, -camera.position.Y, -camera.position.Z, 1}
            };

            float[,] viewMatrix = Mathematics.Matrix.Multiplication(viewMatrixPart1, viewMatrixPart2);

           
            float[,] projectionMatrix = new float[4, 4];
            if (this.projectionNumber == 0)
            {
                float fovy = 60f;
                fovy = fovy / 180f * MathF.PI;
                float aspect = (float)width / (float)height;
                float n = 10f;
                float f = 1000f;
                projectionMatrix = new float[4, 4]
                {
                { (1f/MathF.Tan(fovy/2))/aspect, 0, 0, 0 },
                { 0, (1f/MathF.Tan(fovy/2)), 0, 0 },
                { 0, 0, (f+n)/(f-n), 1},
                { 0, 0, (-2*f*n)/(f-n), 0}
                };
            }
            else
            {
                float angleX = 45f * (MathF.PI / 180f);
                float angleY = 35.264f * (MathF.PI / 180f); 
                float cosX = MathF.Cos(angleX);
                float sinX = MathF.Sin(angleX);
                float cosY = MathF.Cos(angleY);
                float sinY = MathF.Sin(angleY);

                projectionMatrix = new float[4, 4]
                {
                { cosY, sinX * sinY, 0, 0 },
                { 0, cosX, 0, 0 },
                { sinY, -sinX * cosY, 0, 0 },
                { 0, 0, 0, 1 }
                };
            }

            for (int i = 0; i < polyhedrons.Count; i++)
            {
                AffineTransform.AffineTransformationList(polyhedrons[i].points, viewMatrix);
                AffineTransform.AffineTransformationList(polyhedrons[i].points, projectionMatrix);

                for (int j = 0; j < polyhedrons[i].points.Count; j++)
                {
                    if (projectionNumber == 0 && polyhedrons[i].points[j].Z > 0)
                    {
                        float w = polyhedrons[i].points[j].CoordinateVector[3];
                        if (w != 0)
                        {
                            polyhedrons[i].points[j].X /= w;
                            polyhedrons[i].points[j].Y /= w;
                            polyhedrons[i].points[j].Z /= w;
                        }
                        polyhedrons[i].points[j].X = ((polyhedrons[i].points[j].X + 1) / 2) * width;
                        polyhedrons[i].points[j].Y = ((1 - polyhedrons[i].points[j].Y) / 2) * height;
                    }
                    else
                    {
                        polyhedrons[i].points[j].X = ((polyhedrons[i].points[j].X + 1) / 2);
                        polyhedrons[i].points[j].Y = ((1 - polyhedrons[i].points[j].Y) / 2);
                    }
                }
            }
            //stopwatch.Stop();

            //richTextBox1.Text += $"Конец: {stopwatch.ElapsedMilliseconds}";
            List<Edge> res = new List<Edge>();
            for (int i = 0; i < polyhedrons.Count; i++)
            {
                for (int j = 0; j < polyhedrons[i].edges.Count; j++)
                {
                    res.Add(new Edge(new Objects.Point(polyhedrons[i].edges[j].FirstPoint.X, polyhedrons[i].edges[j].FirstPoint.Y, polyhedrons[i].edges[j].FirstPoint.Z),
                                     new Objects.Point(polyhedrons[i].edges[j].SecondPoint.X, polyhedrons[i].edges[j].SecondPoint.Y, polyhedrons[i].edges[j].SecondPoint.Z)));
                }
            }
            int points_number = 0;
            for (int i = 0; i < polyhedrons.Count; i++)
            {
                for (int j = 0; j < polyhedrons[i].points.Count; j++)
                {
                    polyhedrons[i].points[j].X = oldPoints[points_number].X;
                    polyhedrons[i].points[j].Y = oldPoints[points_number].Y;
                    polyhedrons[i].points[j].Z = oldPoints[points_number].Z;
                    points_number++;
                }
            }

            return res;
        }

    }
}
