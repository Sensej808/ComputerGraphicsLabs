using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;

namespace _3D_Labs.Graphic.Objects
{
    //класс точки
    public class Point
    {
        public float[] CoordinateVector;
        public float X
        {
            get => CoordinateVector[0];
            set => CoordinateVector[0] = value;
        }

        public float Y
        {
            get => CoordinateVector[1];
            set => CoordinateVector[1] = value;
        }

        public float Z
        {
            get => CoordinateVector[2];
            set => CoordinateVector[2] = value;
        }

        public Point(float x, float y, float z)
        {
            CoordinateVector = new float[4];
            CoordinateVector[0] = x;
            CoordinateVector[1] = y;
            CoordinateVector[2] = z;
            CoordinateVector[3] = 1;
        }

        public static Point operator -(Point p1, Point p2)
        {
            return new Point(p1.X - p2.X, p1.Y - p2.Y, p1.Z - p2.Z);
        }

        public static Point operator -(Point p)
        {
            return new Point(-p.X, -p.Y, -p.Z);
        }

        public static bool operator ==(Point p1, Point p2)
        {
            if (ReferenceEquals(p1, p2)) return true;
            if (ReferenceEquals(p1, null) || ReferenceEquals(p2, null)) return false;

            return p1.X == p2.X && p1.Y == p2.Y && p1.Z == p2.Z;
        }

        public static bool operator !=(Point p1, Point p2)
        {
            return !(p1 == p2);
        }

        //проверка на то, что точки лежат в одной плоскости
        public static bool BelongingOnePlace(List<Point> points, float accuracy)
        {
            Point v1 = points[1] - points[0];
            Point v2 = points[2] - points[0];
            for (int i = 3; i < points.Count; i++)
            {
                Point v3 = points[i] - points[0];
                /*
                 v1.X v1.Y v1.Z
                 v2.X v2.Y v2.Z
                 v3.X v3.Y v3.Z
                */
                float det = v1.X * v2.Y * v3.Z + v1.Y * v2.Z * v3.X + v2.X * v3.Y * v1.Z -
                             v3.X * v2.Y * v1.Z - v2.X * v1.Y * v3.Z - v3.Y * v2.Z * v1.X;
                if (det > accuracy || det < -accuracy)
                    return false;
            }
            return true;
        }
    }

    //класс ребра
    public class Edge
    {
        public Point[] CoordinateVector;
        public Point FirstPoint
        {
            get => CoordinateVector[0];
            set => CoordinateVector[0] = value;
        }

        public Point SecondPoint
        {
            get => CoordinateVector[1];
            set => CoordinateVector[1] = value;
        }

        public Edge(Point firstPoint, Point secondPoint)
        {
            CoordinateVector = new Point[2];
            CoordinateVector[0] = firstPoint;
            CoordinateVector[1] = secondPoint;
        }

        public static bool operator ==(Edge edge1, Edge edge2)
        {
            if (ReferenceEquals(edge1, edge2)) return true;
            if (ReferenceEquals(edge1, null) || ReferenceEquals(edge2, null)) return false;

            return (edge1.FirstPoint == edge2.FirstPoint && edge1.SecondPoint == edge2.SecondPoint) || (edge1.FirstPoint == edge2.SecondPoint && edge1.SecondPoint == edge2.FirstPoint);
        }

        public static bool operator !=(Edge p1, Edge p2)
        {
            return !(p1 == p2);
        }
    }


    //класс грани
    public class Face
    {
        public List<Point> points;
        public List<Edge> edges;
        public Point normalVector;
        public Point center;
        public Face(List<Point> points, float accuracy = 0.001f)
        {
            this.points = points;

            this.edges = new List<Edge>();
            for (int i = 0; i < points.Count - 1; i++)
                this.edges.Add(new Edge(points[i], points[i + 1]));
            edges.Add(new Edge(points[0], points[points.Count - 1]));

            normalVector = new Point(0, 0, 0);
            
            Point v1 = points[points.Count - 1] - points[0];
            Point v2 = points[1] - points[0];
            normalVector.CoordinateVector = Mathematics.Vector.Multiplication(v1.CoordinateVector, v2.CoordinateVector);
            Mathematics.Vector.Normalization(normalVector.CoordinateVector);
            
            center = new Point(0, 0, 0);
            Center();
            NormalVector();
        }

        public void NormalVector()
        {
            Point v2 = points[points.Count - 1] - points[0];
            Point v1 = points[1] - points[0];
            normalVector.CoordinateVector = Mathematics.Vector.Multiplication(v1.CoordinateVector, v2.CoordinateVector);
            Mathematics.Vector.Normalization(normalVector.CoordinateVector);
        }

        public void Center()
        {
            float maxX = float.MinValue;
            float minX = float.MaxValue;
            float maxY = float.MinValue;
            float minY = float.MaxValue;
            float maxZ = float.MinValue;
            float minZ = float.MaxValue;
            for (int i = 0; i < points.Count; i++)
            {
                if (points[i].X < minX)
                {
                    minX = points[i].X;
                }
                if (points[i].X > maxX)
                {
                    maxX = points[i].X;
                }

                if (points[i].Y < minY)
                {
                    minY = points[i].Y;
                }
                if (points[i].Y > maxY)
                {
                    maxY = points[i].Y;
                }

                if (points[i].Z < minZ)
                {
                    minZ = points[i].Z;
                }
                if (points[i].Z > maxZ)
                {
                    maxZ = points[i].Z;
                }
            }

            center.X = (maxX + minX) / 2;
            center.Y = (maxY + minY) / 2;
            center.Z = (maxZ + minZ) / 2;
        }



        //проверка что у каждого ребра только 2 грани
        public static bool Closed(List<Face> faces)
        {
            List<Edge> allEdges = new List<Edge>();

            for (int i = 0; i < faces.Count; i++)
            {
                for (int j = 0; j < faces[i].edges.Count; j++)
                {
                    allEdges.Add(faces[i].edges[j]);
                }
            }
            for (int i = 0; i < allEdges.Count; i++)
            {
                if (allEdges.Count(x => x == allEdges[i]) != 2)
                    return false;
            }
            return true;
        }

        //проверка на связность
        public static bool Connectivity(List<Face> faces)
        {
            List<Face> visitedFaces = new List<Face>();
            visitedFaces.Add(faces[0]);
            for (int i = 1; i < faces.Count; i++)
            {
                bool find = false;
                for (int j = 0; j < visitedFaces.Count; j++)
                {
                    for (int k = 0; k < visitedFaces[j].edges.Count; k++)
                    {
                        for (int e = 0; e < faces[i].edges.Count; e++)
                        {
                            if (faces[i].edges[e] == visitedFaces[j].edges[k])
                            {
                                visitedFaces.Add(faces[i]);
                                find = true;
                                break;
                            }
                        }
                        if (find)
                        {
                            break;
                        }
                    }
                    if (find)
                    {
                        break;
                    }
                }
                find = false;
            }
            return visitedFaces.Count == faces.Count;
        }

        public class FaceException : Exception
        {
            public FaceException() : base("Error in Face class")
            {
            }

            public FaceException(string message) : base(message)
            {
            }

            public int ErrorCode { get; set; }
        }
    }

    //класс многогранника
    public class Polyhedron
    {
        public List<Face> faces;
        public List<Edge> edges;
        public List<Point> points;
        public bool closed;

        public Polyhedron(List<Face> faces)
        {
            this.faces = faces;
            var edgeSet = new HashSet<Edge>();
            var pointSet = new HashSet<Point>();

            this.edges = new List<Edge>();
            this.points = new List<Point>();
            this.closed = false;

            foreach (var face in faces)
            {
                foreach (var edge in face.edges)
                {
                    if (edgeSet.Add(edge))  
                    {
                        this.edges.Add(edge);

                        if (pointSet.TryGetValue(edge.FirstPoint, out var existingFirstPoint))
                        {
                            edge.FirstPoint = existingFirstPoint;
                        }
                        else
                        {
                            pointSet.Add(edge.FirstPoint);
                            this.points.Add(edge.FirstPoint);
                        }

                        if (pointSet.TryGetValue(edge.SecondPoint, out var existingSecondPoint))
                        {
                            edge.SecondPoint = existingSecondPoint;
                        }
                        else
                        {
                            pointSet.Add(edge.SecondPoint);
                            this.points.Add(edge.SecondPoint);
                        }
                    }
                }

                foreach (var point in face.points)
                {
                    if (pointSet.Add(point))
                    {
                        this.points.Add(point);
                    }
                }
            }
        }

        public void Update()
        {
            for (int i = 0; i < faces.Count; i++)
            {
                faces[i].Center();
                faces[i].NormalVector();
            }
        }

        public Polyhedron DeepCopy()
        {
            List<Face> copyFaces = new List<Face>();
            for (int i = 0; i < this.faces.Count; i++)
            {
                List<Point> copyPoints = new List<Point>();
                for (int j = 0; j < this.faces[i].points.Count; j++)
                {
                    copyPoints.Add(new Point(this.faces[i].points[j].X, this.faces[i].points[j].Y, this.faces[i].points[j].Z));
                }
                copyFaces.Add(new Face(copyPoints, 0));
            }
            return new Polyhedron(copyFaces);
        }

        public Objects.Point Center()
        {
            float maxX = float.MinValue;
            float minX = float.MaxValue;
            float maxY = float.MinValue;
            float minY = float.MaxValue;
            float maxZ = float.MinValue;
            float minZ = float.MaxValue;
            for (int i = 0; i < points.Count; i++)
            {
                if (points[i].X < minX)
                {
                    minX = points[i].X;
                }
                if (points[i].X > maxX)
                {
                    maxX = points[i].X;
                }

                if (points[i].Y < minY)
                {
                    minY = points[i].Y;
                }
                if (points[i].Y > maxY)
                {
                    maxY = points[i].Y;
                }

                if (points[i].Z < minZ)
                {
                    minZ = points[i].Z;
                }
                if (points[i].Z > maxZ)
                {
                    maxZ = points[i].Z;
                }
            }
            return new Objects.Point((maxX + minX) / 2, (maxY + minY) / 2, (maxZ + minZ) / 2);
        }

        public class PolyHedronException : Exception
        {
            public PolyHedronException() : base("Error in PolyHedron class")
            {

            }

            public PolyHedronException(string message) : base(message)
            {
            }

            public int ErrorCode { get; set; }
        }

    }
}
