using _3D_Labs.Graphic.Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3D_Labs.Graphic
{
    public class OBJFormat
    {
        public static Polyhedron Read(string fileName, System.Windows.Forms.RichTextBox richTextBox)
        {
            List<Objects.Point> points = new List<Objects.Point>();
            List<Objects.Face> faces = new List<Face>();
            foreach (string line in File.ReadLines(fileName))
            {
                string currentLine = line.Replace('.', ',');
                string[] words = currentLine.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (words.Length > 3)
                {
                    if (words[0] == "v")
                    {
                        points.Add(new Objects.Point(float.Parse(words[1]), float.Parse(words[2]), float.Parse(words[3])));
                    }

                    if (words[0] == "f")
                    {
                        List<Objects.Point> facePoints = new List<Objects.Point>();
                        for (int i = 1; i < words.Length; i++)
                        {
                            string[] numbers = words[i].Split("/");
                            facePoints.Add(points[int.Parse(numbers[0]) - 1]);
                        }
                        faces.Add(new Face(facePoints));
                    }
                }
            }

            richTextBox.Text += faces.Count.ToString() + " " + points.Count.ToString();
            return new Polyhedron(faces);
        }

        public static void Save(string fileName, Polyhedron polyhedron)
        {
            fileName += ".obj";
            using (StreamWriter writer = new StreamWriter(fileName, append: true))
            {
                for (int i = 0; i < polyhedron.points.Count; i++)
                {
                    string s = $"v {polyhedron.points[i].X} {polyhedron.points[i].Y} {polyhedron.points[i].Z}";
                    s = s.Replace(',', '.');
                    writer.WriteLine(s);
                }
                for (int i = 0; i < polyhedron.faces.Count; i++)
                {
                    string s = "f";
                    for (int j = 0; j < polyhedron.faces[i].points.Count; j++)
                    {
                        s += " " + (polyhedron.points.FindIndex(p => p == polyhedron.faces[i].points[j]) + 1).ToString();
                    }
                    writer.WriteLine(s);
                }
            }
        }
    }
}
