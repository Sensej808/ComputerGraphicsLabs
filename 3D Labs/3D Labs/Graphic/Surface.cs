using _3D_Labs.Graphic.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3D_Labs.Graphic
{
    public class Surface
    {
        public static Polyhedron Create(float x1, float y1, float x2, float y2, int partitionsCountX, int partitionsCountY, Func<float, float, float> func)
        {
            float startX = Math.Min(x1, x2);
            float finishX = Math.Max(x1, x2);

            float startY = Math.Min(y1, y2);
            float finishY = Math.Max(y1, y2);

            float stepX = (finishX - startX) / partitionsCountX;
            float stepY = (finishY - startY) / partitionsCountY;

            float currentX = startX;
            float currentY = startY;

            List<Objects.Point> points = new List<Objects.Point>();


            for (int i = 0; i <= partitionsCountY; i++)
            {
                for (int j = 0; j <= partitionsCountX; j++)
                {
                    points.Add(new Objects.Point(currentX, currentY, func(currentX, currentY)));
                    currentX += stepX;
                }
                currentX = startX;
                currentY += stepY;
            }

            List<Face> faces = new List<Face>();

            for (int i = 0; i < partitionsCountY; i++)
            {
                for (int j = 0; j < partitionsCountX; j++)
                {
                    faces.Add(new Face(new List<Objects.Point> { points[i * (partitionsCountX+1)+j],
                                                                 points[(i + 1) * (partitionsCountX+1)+j],
                                                                 points[i * (partitionsCountX+1) + j + 1 ] }));

                    faces.Add(new Face(new List<Objects.Point> { points[(i + 1) * (partitionsCountX+1) + j],
                                                                 points[(i + 1) * (partitionsCountX+1) + j + 1],
                                                                 points[i * (partitionsCountX+1) + j + 1 ] }));
                }
            }

            return new Polyhedron(faces);

        }
    }
}
