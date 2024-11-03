using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3D_Labs.Graphic
{
    public enum Axis
    {
        X = 1, Y, Z
    };
    public class Camera
    {
        public Objects.Point position;
        public Objects.Point target;
        public Objects.Point up;

        public Camera(Objects.Point position, Objects.Point target, Objects.Point up)
        {
            this.position = position;
            this.target = target;
            this.up = up;
        }

        public Camera()
        {
            this.position = new Objects.Point(0, 0, 0);
            this.target = new Objects.Point(0, 0, 0);
            this.up = new Objects.Point(0, 1, 0);
        }

        public void MoveAlongAxis(Axis direction, float length)
        {
            switch (direction)
            {
                case Axis.X:
                    position.X += length;
                    target.X += length;
                    break;

                case Axis.Y:
                    position.Y += length;
                    target.Y += length;
                    break;

                case Axis.Z:
                    position.Z += length;
                    target.Z += length;
                    break;

            }
        }
    }
}
