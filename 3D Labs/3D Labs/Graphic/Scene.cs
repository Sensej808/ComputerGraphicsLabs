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
using System.Runtime.InteropServices;

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


        public float[,] zBuffer;
        public System.Drawing.Point[] vertices;
        public Color[] verticesColors;
        public float[] verticesW;
        public float[] verticeslightIntensity;
        public float[][] verticesNormal;
        public float[][] verticesPoint;
        public float[][] verticesTexture;

        public List<int[]> yLevelCoordinate;
        public List<float[]> yLevelW;
        public List<Color[]> yLevelColor;
        public List<float[]> yLevelLightIntensity;
        public List<float[][]> yLevelNormal;
        public List<float[][]> yLevelPoint;
        public List<float[][]> yLevelTexture;


        public List<LightSource> lightSources;

        public System.Windows.Forms.RichTextBox richTextBox;

        public Scene(List<Polyhedron> polyhedrons, Camera camera, Graphics g, Pen pen, Bitmap bitmap, PictureBox picture, RichTextBox richTextBox, List<LightSource> lightSources)
        {
            this.polyhedrons = polyhedrons;
            this.camera = camera;
            this.projectionNumber = 0;
            this.bitmap = bitmap;
            this.pen = pen;
            this.g = g;
            this.picture = picture;
            this.richTextBox = richTextBox;

            this.zBuffer = new float[bitmap.Width, bitmap.Height];

            this.vertices = new System.Drawing.Point[3];
            this.verticesColors = new Color[3];
            this.verticesW = new float[3];
            this.verticeslightIntensity = new float[3];
            this.verticesNormal = new float[3][];
            this.verticesPoint = new float[3][];
            this.verticesTexture = new float[3][];

            this.yLevelW = new List<float[]>();
            this.yLevelCoordinate = new List<int[]>();
            this.yLevelColor = new List<Color[]>();
            this.yLevelLightIntensity = new List<float[]>();
            this.yLevelNormal = new List<float[][]>();
            this.yLevelPoint = new List<float[][]>();
            this.yLevelTexture = new List<float[][]>();
            for (int i = 0; i < bitmap.Width; i++)
            {
                this.yLevelCoordinate.Add(new int[3] { int.MaxValue, int.MinValue, 0 });
                this.yLevelW.Add(new float[2] { float.MaxValue, float.MaxValue });
                this.yLevelColor.Add(new Color[2] { Color.White, Color.White });
                this.yLevelLightIntensity.Add(new float[2] { 1f, 1f });
                //this.yLevelNormal.Add(new Objects.Point[2] { new Objects.Point(0, 0, 0), new Objects.Point(0, 0, 0) });
                this.yLevelNormal.Add(new float[2][] { new float[3] { 0, 0, 0 }, new float[3] { 0, 0, 0 } });
                this.yLevelPoint.Add(new float[2][] { new float[3] { 0, 0, 0 }, new float[3] { 0, 0, 0 } });
                this.yLevelTexture.Add(new float[2][] { new float[2] { 0, 0 }, new float[2] { 0, 0 } });
            }
            zBufferInitialization();

            this.lightSources = lightSources;
        }

        private System.Drawing.Point[] getLineHigh(int x0, int y0, float w0, Color color0, float intensity0, float[] normal0, float[] point0, float[] texture0, int x1, int y1, float w1, Color color1, float intensity1, float[] normal1, float[] point1, float[] texture1)
        {
            //richTextBox.Text += $"Интерполяция ребра: {x0} {y0} {color0}\r\n {x1} {y1} {color1}\r\n\r\n";
            List<System.Drawing.Point> res = new List<System.Drawing.Point>();
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

            float step = (w1 - w0) / (y1 - y0);

            float stepR = (float)(color1.R - color0.R) / (float)(y1 - y0);
            float stepG = (float)(color1.G - color0.G) / (float)(y1 - y0);
            float stepB = (float)(color1.B - color0.B) / (float)(y1 - y0);

            float stepIntensity = (intensity1 - intensity0) / (float)(y1 - y0); 

            float stepX = (float)(normal1[0] - normal0[0]) / (float)(y1 - y0);
            float stepY = (float)(normal1[1] - normal0[1]) / (float)(y1 - y0);
            float stepZ = (float)(normal1[2] - normal0[2]) / (float)(y1 - y0);

            float stepXPoint = (float)(point1[0] - point0[0]) / (float)(y1 - y0);
            float stepYPoint = (float)(point1[1] - point0[1]) / (float)(y1 - y0);
            float stepZPoint = (float)(point1[2] - point0[2]) / (float)(y1 - y0);

            float stepXTexture = (float)(texture1[0] - texture0[0]) / (float)(y1 - y0);
            float stepYTexture = (float)(texture1[1] - texture0[1]) / (float)(y1 - y0);

            int stepCnt = 0;
            for (int y = y0; y <= y1; y++)
            {
                if (y >= 0 && y < bitmap.Height)
                {
                    if (yLevelCoordinate[y][0] > x)
                    {
                        yLevelCoordinate[y][0] = x;
                        yLevelCoordinate[y][2] = 1;
                        //if (w0 + step * stepCnt <= yLevelW[y][0])
                        {
                            yLevelW[y][0] = (w0 + step * stepCnt);
                            yLevelColor[y][0] = Color.FromArgb((byte)MathF.Round(color0.R + stepR * stepCnt), (byte)MathF.Round(color0.G + stepG * stepCnt), (byte)MathF.Round(color0.B + stepB * stepCnt));
                            yLevelLightIntensity[y][0] = intensity0 + stepIntensity * stepCnt;
                            yLevelNormal[y][0] = new float[3] { normal0[0] + stepX * stepCnt, normal0[1] + stepY * stepCnt, normal0[2] + stepZ * stepCnt };
                            yLevelPoint[y][0] = new float[3] { point0[0] + stepXPoint * stepCnt, point0[1] + stepYPoint * stepCnt, point0[2] + stepZPoint * stepCnt };
                            yLevelTexture[y][0] = new float[2] { texture0[0] + stepXTexture * stepCnt, texture0[1] + stepYTexture * stepCnt };
                        }
                    }
                    if (yLevelCoordinate[y][1] < x)
                    {
                        yLevelCoordinate[y][1] = x;
                        yLevelCoordinate[y][2] = 1;
                        //if (w0 + step * stepCnt <= yLevelW[y][1])
                        {
                            yLevelW[y][1] = (w0 + step * stepCnt);
                            yLevelColor[y][1] = Color.FromArgb((byte)MathF.Round(color0.R + stepR * stepCnt), (byte)MathF.Round(color0.G + stepG * stepCnt), (byte)MathF.Round(color0.B + stepB * stepCnt));
                            yLevelLightIntensity[y][1] = intensity0 + stepIntensity * stepCnt;
                            yLevelNormal[y][1] = new float[3] { normal0[0] + stepX * stepCnt, normal0[1] + stepY * stepCnt, normal0[2] + stepZ * stepCnt };
                            yLevelPoint[y][1] = new float[3] { point0[0] + stepXPoint * stepCnt, point0[1] + stepYPoint * stepCnt, point0[2] + stepZPoint * stepCnt };
                            yLevelTexture[y][1] = new float[2] { texture0[0] + stepXTexture * stepCnt, texture0[1] + stepYTexture * stepCnt };
                        }
                    }
                }

                if (D > 0)
                {
                    x += xi;
                    D += 2 * (dx - dy);
                }
                else
                    D += 2 * dx;
                stepCnt++;
            }

            return res.OrderBy(p => p.Y).ToArray();
        }

        private System.Drawing.Point[] getLineLow(int x0, int y0, float w0, Color color0, float intensity0, float[] normal0, float[] point0, float[] texture0, int x1, int y1, float w1, Color color1, float intensity1, float[] normal1, float[] point1, float[] texture1)
        {
            //richTextBox.Text += $"Интерполяция ребра: {x0} {y0} | {color0.R} {color0.G} {color0.B}\r\n {x1} {y1} | {color1.R} {color1.G} {color1.B}\r\n\r\n";
            List<System.Drawing.Point> res = new List<System.Drawing.Point>();
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

            float step = (w1 - w0) / (x1 - x0);

            float stepR = (float)(color1.R - color0.R) / (float)(x1 - x0);
            float stepG = (float)(color1.G - color0.G) / (float)(x1 - x0);
            float stepB = (float)(color1.B - color0.B) / (float)(x1 - x0);

            float stepIntensity = (intensity1 - intensity0) / (float)(x1 - x0);

            float stepX = (float)(normal1[0] - normal0[0]) / (float)(x1 - x0);
            float stepY = (float)(normal1[1] - normal0[1]) / (float)(x1 - x0);
            float stepZ = (float)(normal1[2] - normal0[2]) / (float)(x1 - x0);

            float stepXPoint = (float)(point1[0] - point0[0]) / (float)(x1 - x0);
            float stepYPoint = (float)(point1[1] - point0[1]) / (float)(x1 - x0);
            float stepZPoint = (float)(point1[2] - point0[2]) / (float)(x1 - x0);

            float stepXTexture = (float)(texture1[0] - texture0[0]) / (float)(x1 - x0);
            float stepYTexture = (float)(texture1[1] - texture0[1]) / (float)(x1 - x0);

            int stepCnt = 0;
            for (int x = x0; x <= x1; x++)
            {
                if (y >= 0 && y < bitmap.Height)
                {
                    if (yLevelCoordinate[y][0] > x)
                    {
                        yLevelCoordinate[y][0] = x;
                        yLevelCoordinate[y][2] = 1;
                        //if (w0 + step * stepCnt <= yLevelW[y][0])
                        {
                            yLevelW[y][0] = (w0 + step * stepCnt);
                            yLevelColor[y][0] = Color.FromArgb((byte)MathF.Round(color0.R + stepR * stepCnt), (byte)MathF.Round(color0.G + stepG * stepCnt), (byte)MathF.Round(color0.B + stepB * stepCnt));
                            yLevelLightIntensity[y][0] = intensity0 + stepIntensity * stepCnt;
                            yLevelNormal[y][0] = new float[3] { normal0[0] + stepX * stepCnt, normal0[1] + stepY * stepCnt, normal0[2] + stepZ * stepCnt };
                            yLevelPoint[y][0] = new float[3] { point0[0] + stepXPoint * stepCnt, point0[1] + stepYPoint * stepCnt, point0[2] + stepZPoint * stepCnt };
                            yLevelTexture[y][0] = new float[2] { texture0[0] + stepXTexture * stepCnt, texture0[1] + stepYTexture * stepCnt };
                        }
                    }
                    if (yLevelCoordinate[y][1] < x)
                    {
                        yLevelCoordinate[y][1] = x;
                        yLevelCoordinate[y][2] = 1;
                        //if (w0 + step * stepCnt <= yLevelW[y][1])
                        {
                            yLevelW[y][1] = (w0 + step * stepCnt);
                            yLevelColor[y][1] = Color.FromArgb((byte)MathF.Round(color0.R + stepR * stepCnt), (byte)MathF.Round(color0.G + stepG * stepCnt), (byte)MathF.Round(color0.B + stepB * stepCnt));
                            yLevelLightIntensity[y][1] = intensity0 + stepIntensity * stepCnt;
                            yLevelNormal[y][1] = new float[3] { normal0[0] + stepX * stepCnt, normal0[1] + stepY * stepCnt, normal0[2] + stepZ * stepCnt };
                            yLevelPoint[y][1] = new float[3] { point0[0] + stepXPoint * stepCnt, point0[1] + stepYPoint * stepCnt, point0[2] + stepZPoint * stepCnt };
                            yLevelTexture[y][1] = new float[2] { texture0[0] + stepXTexture * stepCnt, texture0[1] + stepYTexture * stepCnt };
                        }
                    }
                }

                if (D > 0)
                {
                    y += yi;
                    D += 2 * (dy - dx);
                }
                else
                    D += 2 * dy;
                stepCnt++;
            }

            return res.OrderBy(p => p.Y).ToArray();
        }

        public System.Drawing.Point[] getLineCoordsByBresenham(System.Drawing.Point p0, float w0, Color color0, float intensity0, float[] normal0, float[] point0, float[] texture0, System.Drawing.Point p1, float w1, Color color1, float intensity1, float[] normal1, float[] point1, float[] texture1)
        {
            if (Math.Abs(p1.Y - p0.Y) < Math.Abs(p1.X - p0.X))
            {
                if (p0.X > p1.X)
                    return getLineLow(p1.X, p1.Y, w1, color1, intensity1, normal1, point1, texture1, p0.X, p0.Y, w0, color0, intensity0, normal0, point0, texture0);

                return getLineLow(p0.X, p0.Y, w0, color0, intensity0, normal0, point0, texture0, p1.X, p1.Y, w1, color1, intensity1, normal1, point1, texture1);
            }

            if (p0.Y > p1.Y)
                return getLineHigh(p1.X, p1.Y, w1, color1, intensity1, normal1, point1, texture1, p0.X, p0.Y, w0, color0, intensity0, normal0, point0, texture0);

            return getLineHigh(p0.X, p0.Y, w0, color0, intensity0, normal0, point0, texture0, p1.X, p1.Y, w1, color1, intensity1, normal1, point1, texture1);
        }



        public void GetProjection(int width, int height)
        {
            richTextBox.Text = "";
            zBufferInitialization();
            List<Objects.Point> oldPoints = new List<Objects.Point>();
            int oldIndex = 0;
            for (int i = 0; i < polyhedrons.Count; i++)
            {
                for (int j = 0; j < polyhedrons[i].points.Count; j++)
                {
                    oldPoints.Add(new Objects.Point(polyhedrons[i].points[j].X, polyhedrons[i].points[j].Y, polyhedrons[i].points[j].Z));
                    polyhedrons[i].points[j].oldIndex = oldIndex;
                    oldIndex++;
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
            int printCnt = 0;
            //for (int i = 1; i < 2; i++)
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

                    if (cosAlpha <= 0 || !polyhedrons[i].closed)
                    {
                        for (int triangle = 0; triangle < polyhedrons[i].faces[j].points.Count-2; triangle++)
                        {
                            yLevelInitialization();
                            printCnt++;
                            Objects.Point v0 = polyhedrons[i].faces[j].points[0];
                            Objects.Point v1 = polyhedrons[i].faces[j].points[triangle + 1];
                            Objects.Point v2 = polyhedrons[i].faces[j].points[triangle + 2];
                            if (v0.Z > 0 && v1.Z > 0 && v2.Z > 0)
                            {
                                float x0 = v0.X;
                                float y0 = v0.Y;
                                float x1 = v1.X;
                                float y1 = v1.Y;
                                float x2 = v2.X;
                                float y2 = v2.Y;

                                float w0 = v0.CoordinateVector[3];
                                if (w0 != 0)
                                {
                                    x0 /= w0;
                                    y0 /= w0;
                                }
                                x0 = ((x0 + 1) / 2) * width;
                                y0 = ((1 - y0) / 2) * height;

                                float w1 = v1.CoordinateVector[3];
                                if (w1 != 0)
                                {
                                    x1 /= w1;
                                    y1 /= w1;
                                }
                                x1 = ((x1 + 1) / 2) * width;
                                y1 = ((1 - y1) / 2) * height;

                                float w2 = v2.CoordinateVector[3];
                                if (w2 != 0)
                                {
                                    x2 /= w2;
                                    y2 /= w2;
                                }
                                x2 = ((x2 + 1) / 2) * width;
                                y2 = ((1 - y2) / 2) * height;

                                vertices[0].X = (int)x0;
                                vertices[0].Y = (int)y0;
                                vertices[1].X = (int)x1;
                                vertices[1].Y = (int)y1;
                                vertices[2].X = (int)x2;
                                vertices[2].Y = (int)y2;

                                /*
                                verticesColors[0] = Color.FromArgb(255, 0, 0);
                                verticesColors[1] = Color.FromArgb(0, 255, 0);
                                verticesColors[2] = Color.FromArgb(0, 0, 255);
                                */
                                verticesColors[0] = v0.color;
                                verticesColors[1] = v1.color;
                                verticesColors[2] = v2.color;

                                verticesW[0] = w0;
                                verticesW[1] = w1;
                                verticesW[2] = w2;

                                float cos0 = Mathematics.Vector.ScalarProduct((lightSources[0].position - oldPoints[v0.oldIndex]).CoordinateVector, v0.NormalVector);
                                float cos1 = Mathematics.Vector.ScalarProduct((lightSources[0].position - oldPoints[v1.oldIndex]).CoordinateVector, v1.NormalVector);
                                float cos2 = Mathematics.Vector.ScalarProduct((lightSources[0].position - oldPoints[v2.oldIndex]).CoordinateVector, v2.NormalVector);

                                if (Mathematics.Vector.Length((lightSources[0].position - oldPoints[v0.oldIndex]).CoordinateVector) * Mathematics.Vector.Length(v0.NormalVector) != 0) 
                                    cos0 = cos0 / (Mathematics.Vector.Length((lightSources[0].position - oldPoints[v0.oldIndex]).CoordinateVector) * Mathematics.Vector.Length(v0.NormalVector));

                                if (Mathematics.Vector.Length((lightSources[0].position - oldPoints[v1.oldIndex]).CoordinateVector) * Mathematics.Vector.Length(v1.NormalVector) != 0)
                                    cos1 = cos1 / (Mathematics.Vector.Length((lightSources[0].position - oldPoints[v1.oldIndex]).CoordinateVector) * Mathematics.Vector.Length(v1.NormalVector));

                                if (Mathematics.Vector.Length((lightSources[0].position - oldPoints[v2.oldIndex]).CoordinateVector) * Mathematics.Vector.Length(v2.NormalVector) != 0)
                                    cos2 = cos2 / (Mathematics.Vector.Length((lightSources[0].position - oldPoints[v2.oldIndex]).CoordinateVector) * Mathematics.Vector.Length(v2.NormalVector));

                                verticeslightIntensity[0] = lightSources[0].intensity * polyhedrons[i].faces[j].reflectionCoefficient * Math.Max(0, cos0);
                                verticeslightIntensity[1] = lightSources[0].intensity * polyhedrons[i].faces[j].reflectionCoefficient * Math.Max(0, cos1);
                                verticeslightIntensity[2] = lightSources[0].intensity * polyhedrons[i].faces[j].reflectionCoefficient * Math.Max(0, cos2);

                                verticesNormal[0] = v0.NormalVector;
                                verticesNormal[1] = v1.NormalVector;
                                verticesNormal[2] = v2.NormalVector;

                                verticesPoint[0] = oldPoints[v0.oldIndex].CoordinateVector;
                                verticesPoint[1] = oldPoints[v1.oldIndex].CoordinateVector;
                                verticesPoint[2] = oldPoints[v2.oldIndex].CoordinateVector;

                                /*
                                verticesTexture[0] = v0.textureVectorList[j];
                                verticesTexture[1] = v1.textureVectorList[j];
                                verticesTexture[2] = v2.textureVectorList[j];
                                */
                                verticesTexture[0] = polyhedrons[i].faces[j].texturePoints[0];
                                verticesTexture[1] = polyhedrons[i].faces[j].texturePoints[triangle + 1];
                                verticesTexture[2] = polyhedrons[i].faces[j].texturePoints[triangle + 2];

                                //drawTriangle(polyhedrons[i].faces[j].color);
                                drawTriangle(i, Color.FromArgb(200, 200, 200), polyhedrons[i].faces[j].reflectionCoefficient, lightSources[0]);
                            }
                        }
                    }
                }
                //richTextBox.Text += $"{printCnt}\r\n";
                printCnt = 0;
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
                    polyhedrons[i].points[j].CoordinateVector[3] = 1;
                    points_number++;
                }
            }
        }

        public void zBufferInitialization()
        {
            for (int i = 0; i < zBuffer.GetLength(0); i++)
            {
                for (int j = 0; j < zBuffer.GetLength(1); j++)
                    zBuffer[i, j] = float.MaxValue;
            }
        }

        public void yLevelInitialization()
        {
            for (int i = 0; i < yLevelCoordinate.Count; i++)
            {
                yLevelCoordinate[i][0] = int.MaxValue;
                yLevelCoordinate[i][1] = int.MinValue;
                yLevelCoordinate[i][2] = 0;

                yLevelW[i][0] = float.MaxValue;
                yLevelW[i][1] = float.MaxValue;

                yLevelColor[i][0] = Color.White;
                yLevelColor[i][1] = Color.White;

                yLevelLightIntensity[i][0] = 1;
                yLevelLightIntensity[i][1] = 1;

                yLevelNormal[i][0][0] = 0;
                yLevelNormal[i][0][1] = 0;
                yLevelNormal[i][0][2] = 0;

                yLevelNormal[i][1][0] = 0;
                yLevelNormal[i][1][1] = 0;
                yLevelNormal[i][1][2] = 0;

                yLevelTexture[i][0][0] = 0;
                yLevelTexture[i][0][1] = 0;

                yLevelTexture[i][1][0] = 0;
                yLevelTexture[i][1][1] = 0;
            }
        }

        public void drawTriangle(int polyhedronIndex, Color faceColor, float reflectionCoefficient, LightSource lightSource)
        {
            getLineCoordsByBresenham(vertices[0], verticesW[0], verticesColors[0], verticeslightIntensity[0], verticesNormal[0], verticesPoint[0], verticesTexture[0], vertices[1], verticesW[1], verticesColors[1], verticeslightIntensity[1], verticesNormal[1], verticesPoint[1], verticesTexture[1]);
            getLineCoordsByBresenham(vertices[0], verticesW[0], verticesColors[0], verticeslightIntensity[0], verticesNormal[0], verticesPoint[0], verticesTexture[0], vertices[2], verticesW[2], verticesColors[2], verticeslightIntensity[2], verticesNormal[2], verticesPoint[2], verticesTexture[2]);
            getLineCoordsByBresenham(vertices[1], verticesW[1], verticesColors[1], verticeslightIntensity[1], verticesNormal[1], verticesPoint[1], verticesTexture[1], vertices[2], verticesW[2], verticesColors[2], verticeslightIntensity[2], verticesNormal[2], verticesPoint[2], verticesTexture[2]);
            for (int yIndex = 0; yIndex < yLevelCoordinate.Count; yIndex++)
            {
                if (yLevelCoordinate[yIndex][2] == 1 && yLevelCoordinate[yIndex][0] < bitmap.Width && yLevelCoordinate[yIndex][1] >= 0)
                {
                    int stepCount = yLevelCoordinate[yIndex][1] - yLevelCoordinate[yIndex][0];
                    if (stepCount == 0)
                        stepCount = 1;

                    float wStart = yLevelW[yIndex][0];
                    float wEnd = yLevelW[yIndex][1];

                    float wStep = (wEnd - wStart) / stepCount;

                    Color colorStart = yLevelColor[yIndex][0];
                    Color colorEnd = yLevelColor[yIndex][1];

                    float rStep = (float)(colorEnd.R - colorStart.R) / (float)stepCount;
                    float gStep = (float)(colorEnd.G - colorStart.G) / (float)stepCount;
                    float bStep = (float)(colorEnd.B - colorStart.B) / (float)stepCount;

                    float lightIntensityStart = yLevelLightIntensity[yIndex][0];
                    float lightIntensityEnd = yLevelLightIntensity[yIndex][1];

                    float lightIntensityStep = (lightIntensityEnd - lightIntensityStart) / (float)stepCount;

                    float[] normalStart = yLevelNormal[yIndex][0];
                    float[] normalEnd = yLevelNormal[yIndex][1];

                    float xStep = (normalEnd[0] - normalStart[0]) / (float)stepCount;
                    float yStep = (normalEnd[1] - normalStart[1]) / (float)stepCount;
                    float zStep = (normalEnd[2] - normalStart[2]) / (float)stepCount;

                    float[] pointStart = yLevelPoint[yIndex][0];
                    float[] pointEnd = yLevelPoint[yIndex][1];

                    float xStepPoint = (pointEnd[0] - pointStart[0]) / (float)stepCount;
                    float yStepPoint = (pointEnd[1] - pointStart[1]) / (float)stepCount;
                    float zStepPoint = (pointEnd[2] - pointStart[2]) / (float)stepCount;

                    float[] textureStart = yLevelTexture[yIndex][0];
                    float[] textureEnd = yLevelTexture[yIndex][1];

                    float xStepTexture = (textureEnd[0] - textureStart[0]) / (float)stepCount;
                    float yStepTexture = (textureEnd[1] - textureStart[1]) / (float)stepCount;

                    int currentStep = 0;

                    if (yLevelCoordinate[yIndex][0] < 0)
                    {
                        wStart += wStep * (-yLevelCoordinate[yIndex][0]);
                        colorStart = Color.FromArgb((int)Math.Round(colorStart.R + rStep * (-yLevelCoordinate[yIndex][0])),
                                                    (int)Math.Round(colorStart.G + gStep * (-yLevelCoordinate[yIndex][0])),
                                                    (int)Math.Round(colorStart.B + bStep * (-yLevelCoordinate[yIndex][0])));
                        lightIntensityStart += lightIntensityStep * (-yLevelCoordinate[yIndex][0]);
                        normalStart[0] += xStep * (-yLevelCoordinate[yIndex][0]);
                        normalStart[1] += yStep * (-yLevelCoordinate[yIndex][0]);
                        normalStart[2] += zStep * (-yLevelCoordinate[yIndex][0]);

                        pointStart[0] += xStepPoint * (-yLevelCoordinate[yIndex][0]);
                        pointStart[1] += yStepPoint * (-yLevelCoordinate[yIndex][0]);
                        pointStart[2] += zStepPoint * (-yLevelCoordinate[yIndex][0]);

                        textureStart[0] += xStepTexture * (-yLevelCoordinate[yIndex][0]);
                        textureStart[1] += yStepTexture * (-yLevelCoordinate[yIndex][0]);
                    }

                    for (int xIndex = Math.Max(yLevelCoordinate[yIndex][0], 0); xIndex < Math.Min(yLevelCoordinate[yIndex][1], bitmap.Width); xIndex++)
                    {
                        if (wStart + currentStep * wStep <= zBuffer[xIndex, yIndex])
                        {
                            zBuffer[xIndex, yIndex] = wStart + currentStep * wStep;

                            /*
                            bitmap.SetPixel(xIndex, yIndex, Color.FromArgb((int)Math.Round(colorStart.R + rStep * currentStep),
                                                                           (int)Math.Round(colorStart.G + gStep * currentStep),
                                                                           (int)Math.Round(colorStart.B + bStep * currentStep)));
                            */
                            /*
                            bitmap.SetPixel(xIndex, yIndex, Color.FromArgb((int)(faceColor.R * (lightIntensityStart + currentStep * lightIntensityStep)),
                                                                           (int)(faceColor.G * (lightIntensityStart + currentStep * lightIntensityStep)),
                                                                           (int)(faceColor.B * (lightIntensityStart + currentStep * lightIntensityStep))));
                            
                            */

                            float cos0 = Mathematics.Vector.ScalarProduct((lightSource.position - new Objects.Point(pointStart[0] + currentStep * xStepPoint,
                                                                                                                    pointStart[1] + currentStep * yStepPoint,
                                                                                                                    pointStart[2] + currentStep * zStepPoint)).CoordinateVector, 
                                                                            new float[3] { normalStart[0] + currentStep * xStep,
                                                                                           normalStart[1] + currentStep * yStep,
                                                                                           normalStart[2] + currentStep * zStep});

                            if (Mathematics.Vector.Length((lightSource.position - new Objects.Point(pointStart[0] + currentStep * xStepPoint,
                                                                                                                    pointStart[1] + currentStep * yStepPoint,
                                                                                                                    pointStart[2] + currentStep * zStepPoint)).CoordinateVector) * 
                                Mathematics.Vector.Length(new float[3] { normalStart[0] + currentStep * xStep,
                                                                         normalStart[1] + currentStep * yStep,
                                                                         normalStart[2] + currentStep * zStep}) != 0)

                                cos0 = cos0 / (Mathematics.Vector.Length((lightSource.position - new Objects.Point(pointStart[0] + currentStep * xStepPoint,
                                                                                                                   pointStart[1] + currentStep * yStepPoint,
                                                                                                                   pointStart[2] + currentStep * zStepPoint)).CoordinateVector) *
                                               Mathematics.Vector.Length(new float[3] { normalStart[0] + currentStep * xStep,
                                                                                        normalStart[1] + currentStep * yStep,
                                                                                        normalStart[2] + currentStep * zStep}));
                            var CurrentIntensivity = lightSource.intensity * reflectionCoefficient * Math.Max(0, cos0);
                            //bitmap.SetPixel(xIndex, yIndex, Color.FromArgb((int)(faceColor.R * CurrentIntensivity), (int)(faceColor.G * CurrentIntensivity), (int)(faceColor.B * CurrentIntensivity)));
                            //richTextBox.Text = $"{(int)(textureStart[1])}\r\n";

                            if (polyhedrons[polyhedronIndex].isTexture)
                            {
                                bitmap.SetPixel(xIndex, yIndex, Color.FromArgb((int)(polyhedrons[polyhedronIndex].texture.GetPixel((int)(textureStart[0] + currentStep * xStepTexture), (int)(textureStart[1] + currentStep * yStepTexture)).R * CurrentIntensivity),
                                                                               (int)(polyhedrons[polyhedronIndex].texture.GetPixel((int)(textureStart[0] + currentStep * xStepTexture), (int)(textureStart[1] + currentStep * yStepTexture)).G * CurrentIntensivity),
                                                                               (int)(polyhedrons[polyhedronIndex].texture.GetPixel((int)(textureStart[0] + currentStep * xStepTexture), (int)(textureStart[1] + currentStep * yStepTexture)).B * CurrentIntensivity)));
                            }
                            else
                                bitmap.SetPixel(xIndex, yIndex, Color.FromArgb((int)(faceColor.R * CurrentIntensivity), (int)(faceColor.G * CurrentIntensivity), (int)(faceColor.B * CurrentIntensivity)));

                        }
                        currentStep++;
                    }
                }
            }
        }
    }
}
