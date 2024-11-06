using _3D_Labs.Graphic.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace _3D_Labs.Graphic
{
    public class SimplePolyhedrons
    {
        public static Objects.Polyhedron Cube(float edgeLength)
        {
            Objects.Point point1 = new Objects.Point(edgeLength/2, edgeLength/2, edgeLength/2); //1
            Objects.Point point2 = new Objects.Point(edgeLength / 2, edgeLength / 2, -edgeLength / 2); //2
            Objects.Point point3 = new Objects.Point(edgeLength / 2, -edgeLength / 2, -edgeLength / 2); //3
            Objects.Point point4 = new Objects.Point(-edgeLength / 2, -edgeLength / 2, -edgeLength / 2); //4
            Objects.Point point5 = new Objects.Point(-edgeLength / 2, edgeLength / 2, -edgeLength / 2); //5
            Objects.Point point6 = new Objects.Point(-edgeLength / 2, edgeLength / 2, edgeLength / 2); //6
            Objects.Point point7 = new Objects.Point(-edgeLength / 2, -edgeLength / 2, edgeLength / 2); //7
            Objects.Point point8 = new Objects.Point(edgeLength / 2, -edgeLength / 2, edgeLength / 2); //8

            Objects.Face face1 = new Face(new List<Objects.Point>(new[] { point1, point6, point7, point8 }), 0);
            Objects.Face face2 = new Face(new List<Objects.Point>(new[] { point1, point6, point5, point2 }), 0);
            Objects.Face face3 = new Face(new List<Objects.Point>(new[] { point5, point2, point3, point4 }), 0);
            Objects.Face face4 = new Face(new List<Objects.Point>(new[] { point7, point8, point3, point4 }), 0);
            Objects.Face face5 = new Face(new List<Objects.Point>(new[] { point1, point8, point3, point2 }), 0);
            Objects.Face face6 = new Face(new List<Objects.Point>(new[] { point4, point5, point6, point7 }), 0);

            return new Objects.Polyhedron(new List<Objects.Face>(new[] { face1, face2, face3, face4, face5, face6 }));
        }

        public static Objects.Polyhedron Tetrahedron(float distanceCenterVertices)
        {
            Objects.Point point1 = new Objects.Point(-distanceCenterVertices / 2, distanceCenterVertices / 2, distanceCenterVertices / 2);
            Objects.Point point2 = new Objects.Point(-distanceCenterVertices / 2, -distanceCenterVertices / 2, -distanceCenterVertices / 2);
            Objects.Point point3 = new Objects.Point(-distanceCenterVertices / 2, -distanceCenterVertices / 2, distanceCenterVertices / 2);
            Objects.Point point4 = new Objects.Point(distanceCenterVertices / 2, distanceCenterVertices / 2, -distanceCenterVertices / 2);

            Objects.Face face1 = new Face(new List<Objects.Point>(new[] { point1, point2, point3 }), 0);
            Objects.Face face2 = new Face(new List<Objects.Point>(new[] { point2, point3, point4 }), 0);
            Objects.Face face3 = new Face(new List<Objects.Point>(new[] { point1, point3, point4 }), 0);

            return new Objects.Polyhedron(new List<Objects.Face>(new[] { face1, face2, face3 }));
        }

        public static Objects.Polyhedron Octahedron(float distanceCenterVertices)
        {
            Objects.Point point1 = new Objects.Point(0, 0, distanceCenterVertices);
            Objects.Point point2 = new Objects.Point(0, distanceCenterVertices, 0);
            Objects.Point point3 = new Objects.Point(distanceCenterVertices, 0, 0);
            Objects.Point point4 = new Objects.Point(0, -distanceCenterVertices, 0);
            Objects.Point point5 = new Objects.Point(-distanceCenterVertices, 0, 0);
            Objects.Point point6 = new Objects.Point(0, 0, -distanceCenterVertices);

            Objects.Face face1 = new Face(new List<Objects.Point>(new[] { point1, point2, point3 }), 0);
            Objects.Face face2 = new Face(new List<Objects.Point>(new[] { point1, point3, point4 }), 0);
            Objects.Face face3 = new Face(new List<Objects.Point>(new[] { point1, point4, point5 }), 0);
            Objects.Face face4 = new Face(new List<Objects.Point>(new[] { point1, point5, point2 }), 0);

            Objects.Face face5 = new Face(new List<Objects.Point>(new[] { point6, point5, point4 }), 0);
            Objects.Face face6 = new Face(new List<Objects.Point>(new[] { point6, point4, point3 }), 0);
            Objects.Face face7 = new Face(new List<Objects.Point>(new[] { point6, point3, point2 }), 0);
            Objects.Face face8 = new Face(new List<Objects.Point>(new[] { point6, point2, point5 }), 0);

            return new Objects.Polyhedron(new List<Objects.Face>(new[] { face1, face2, face3, face4, face5, face6, face7, face8 }));
        }
    }
}
