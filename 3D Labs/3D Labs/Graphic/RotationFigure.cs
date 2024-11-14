using _3D_Labs.Graphic.Objects;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3D_Labs.Graphic
{
    public class RotationFigure
    {
        public static Polyhedron Create(List<Objects.Point> points, int axis, int partitionsCount)
        {
            List<Objects.Point> resPoints = new List<Objects.Point>();
            float angle = 360f / partitionsCount;
            for (int i = 0; i <= partitionsCount; i++)
            {
                List<Objects.Point> newPoints = new List<Objects.Point>();
                for (int j = 0; j < points.Count; j++)
                {
                    newPoints.Add(new Objects.Point(points[j].X, points[j].Y, points[j].Z));
                }
                if (axis == 0)
                {
                    AffineTransform.RotationAroundX(newPoints, angle*i);
                }
                if (axis == 1)
                {
                    AffineTransform.RotationAroundY(newPoints, angle*i);
                }
                if (axis == 2)
                {
                    AffineTransform.RotationAroundZ(newPoints, angle*i);
                }
                for (int j = 0; j < newPoints.Count; j++)
                {
                    resPoints.Add(new Objects.Point(newPoints[j].X, newPoints[j].Y, newPoints[j].Z));
                }
            }
            
            List<Objects.Face> faces = new List<Objects.Face>();

            for (int i = 0; i < partitionsCount; i++)
            {
                for (int j = 0; j < points.Count-1; j++)
                {
                    int leftBottom = i * (points.Count - 1 + 1) + j;
                    int leftTop = i * (points.Count - 1 + 1) + j + 1;
                    int rightTop = (i + 1) * (points.Count - 1 + 1) + j + 1;
                    int rightBottom = (i + 1) * (points.Count - 1 + 1 ) + j;

                    faces.Add(new Face(new List<Objects.Point> { resPoints[rightTop], resPoints[leftTop], resPoints[leftBottom] }));
                    faces.Add(new Face(new List<Objects.Point> { resPoints[rightBottom], resPoints[rightTop], resPoints[leftBottom] }));
                }
            }

            return new Polyhedron(faces);
        }
    }
}
