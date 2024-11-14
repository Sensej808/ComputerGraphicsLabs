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
using System.Windows.Forms.VisualStyles;
using System.Runtime.CompilerServices;

namespace _3D_Labs.Graphic
{
    public class Scene
    {
        public List<Graphic.Objects.Polyhedron> polyhedrons;
        public Camera camera;
        public int projectionNumber;
        public Graphics g;
        public Pen pen;
        public Bitmap bitmap;
        public System.Windows.Forms.PictureBox picture;
        //Для более удобного дебага
        public System.Windows.Forms.RichTextBox richTextBox;

        public Scene(List<Polyhedron> polyhedrons, Camera camera, Graphics g, Pen pen, Bitmap bitmap, PictureBox picture, RichTextBox richTextBox)
        {
            this.polyhedrons = polyhedrons;
            this.camera = camera;
            this.projectionNumber = 0;
            this.bitmap = bitmap;
            this.pen = pen;
            this.g = g;
            this.picture = picture;
            this.richTextBox = richTextBox;
        }
        public void GetProjection(int width, int height)
        {
            List<Objects.Point> oldPoints = new List<Objects.Point>();
            for (int i = 0; i < polyhedrons.Count; i++)
            {
                for (int j = 0; j < polyhedrons[i].points.Count; j++)
                {
                    oldPoints.Add(new Objects.Point(polyhedrons[i].points[j].X, polyhedrons[i].points[j].Y, polyhedrons[i].points[j].Z));
                }
            }

            float deltaX = camera.position.X;
            float deltaY = camera.position.Y;
            float deltaZ = camera.position.Z;

            foreach (var polyhedron in polyhedrons)
            {
                foreach (var point in polyhedron.points)
                {
                    point.X -= deltaX;
                    point.Y -= deltaY;
                    point.Z -= deltaZ;
                }
            }

            Objects.Point cameraRight = new Graphic.Objects.Point(0, 0, 0);
            cameraRight.CoordinateVector = Mathematics.Vector.Multiplication(camera.up.CoordinateVector, camera.direction.CoordinateVector);
            Mathematics.Vector.Normalization(cameraRight.CoordinateVector);

            Graphic.Objects.Point cameraUp = new Graphic.Objects.Point(0, 0, 0);
            cameraUp.CoordinateVector = Mathematics.Vector.Multiplication(camera.direction.CoordinateVector, cameraRight.CoordinateVector);
            Mathematics.Vector.Normalization(cameraUp.CoordinateVector);



            float[,] viewMatrixPart1 = new float[4, 4]
            {
                { cameraRight.X, cameraUp.X, camera.direction.X, 0  },
                { cameraRight.Y, cameraUp.Y, camera.direction.Y, 0 },
                { cameraRight.Z, cameraUp.Z, camera.direction.Z, 0 },
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
                float f = 20f;
                
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
                { cosY, -sinX * sinY, 0, 0 },
                { 0, cosX, 0, 0 },
                { sinY, sinX * cosY, 0, 0 },
                { 0, 0, 0, 1 }
                };
            }

            float x1;
            float y1;
            float z1;
            float x2;
            float y2;
            float z2;

            HashSet<Edge> DrawEdges = new HashSet<Edge>();
            int addedEdgesCnt = 0;
            for (int i = 0; i < polyhedrons.Count; i++)
            {
                AffineTransform.AffineTransformationList(polyhedrons[i].points, viewMatrixPart1);
                foreach (var point in polyhedrons[i].points)
                {
                    point.X += deltaX;
                    point.Y += deltaY;
                    point.Z += deltaZ;
                }
                AffineTransform.AffineTransformationList(polyhedrons[i].points, viewMatrixPart2);

                AffineTransform.AffineTransformationList(polyhedrons[i].points, projectionMatrix);

                for (int j = 0; j < polyhedrons[i].faces.Count; j++)
                {
                    float cosAlpha = Mathematics.Vector.ScalarProduct((polyhedrons[i].faces[j].center - camera.position).CoordinateVector, polyhedrons[i].faces[j].normalVector.CoordinateVector);

                    if (cosAlpha < 0 || !polyhedrons[i].closed)
                    {
                        
                        for (int k = 0; k < polyhedrons[i].faces[j].edges.Count; k++)
                        {
                            if (!DrawEdges.Contains(polyhedrons[i].faces[j].edges[k]))
                            {
                                x1 = polyhedrons[i].faces[j].edges[k].FirstPoint.X;
                                y1 = polyhedrons[i].faces[j].edges[k].FirstPoint.Y;
                                z1 = polyhedrons[i].faces[j].edges[k].FirstPoint.Z;
                                x2 = polyhedrons[i].faces[j].edges[k].SecondPoint.X;
                                y2 = polyhedrons[i].faces[j].edges[k].SecondPoint.Y;
                                z2 = polyhedrons[i].faces[j].edges[k].SecondPoint.Z;

                                float w1 = polyhedrons[i].faces[j].edges[k].FirstPoint.CoordinateVector[3];
                                if (w1 != 0)
                                {
                                    x1 /= w1;
                                    y1 /= w1;
                                    z1 /= w1;
                                }
                                x1 = ((x1 + 1) / 2) * width;
                                y1 = ((1 - y1) / 2) * height;

                                float w2 = polyhedrons[i].faces[j].edges[k].SecondPoint.CoordinateVector[3];
                                if (w2 != 0)
                                {
                                    x2 /= w2;
                                    y2 /= w2;
                                    z2 /= w2;
                                }
                                x2 = ((x2 + 1) / 2) * width;
                                y2 = ((1 - y2) / 2) * height;

                                var x = new PointF(x1, y1);
                                var xx = new PointF(x2, y2);
                                if (((polyhedrons[i].faces[j].edges[k].FirstPoint.Z > 0 && polyhedrons[i].faces[j].edges[k].SecondPoint.Z > 0) && projectionNumber == 0) || projectionNumber == 1)
                                {
                                    g.DrawLine(pen, x, xx);
                                    DrawEdges.Add(polyhedrons[i].faces[j].edges[k]);
                                }
                            }
                            else
                            {
                                addedEdgesCnt++;
                            }
                        }
                        
                    }
                }
            }

            picture.Image = bitmap;

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
        }
    }
}
