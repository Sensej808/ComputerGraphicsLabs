using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace _3D_Labs.Graphic
{
    public enum Axis
    {
        X = 1, Y, Z
    };

    public enum RotationDirection
    {
        LeftRight = 1, UpDown
    };
    public class Camera
    {
        public Objects.Point position;
        public Objects.Point target;
        public Objects.Point up;
        public float leftRightAngle;
        public Objects.Point direction;
        public float upDownAngle;

        public Camera(Objects.Point position, Objects.Point target, Objects.Point up)
        {
            this.position = position;
            this.target = target;
            this.up = up;
            this.leftRightAngle = 0;
            this.direction = this.target - this.position;
            List<Objects.Point> cameraDirectionList = new List<Objects.Point> { this.direction };
            AffineTransform.RotationAroundY(cameraDirectionList, leftRightAngle);
            AffineTransform.RotationAroundX(cameraDirectionList, upDownAngle);
            Mathematics.Vector.Normalization(this.direction.CoordinateVector);

        }

        public Camera()
        {
            this.position = new Objects.Point(0, 0, 0);
            this.target = new Objects.Point(0, 0, 0);
            this.up = new Objects.Point(0, 1, 0);
            this.leftRightAngle = 0;
            this.direction = this.target - this.position;
            List<Objects.Point> cameraDirectionList = new List<Objects.Point> { this.direction };
            AffineTransform.RotationAroundY(cameraDirectionList, leftRightAngle);
            AffineTransform.RotationAroundX(cameraDirectionList, upDownAngle);
            Mathematics.Vector.Normalization(this.direction.CoordinateVector);
        }

        public void MoveAlongAxis(Axis direction, float length)
        {
            switch (direction)
            {
                case Axis.X:
                    if ((this.direction.X > 0 && this.direction.Z > 0) || (this.direction.X > 0 && this.direction.Z < 0))
                        length = -length;
                    if (this.direction.X != 0)
                    {

                        float perpendicularX = -1 * (0* this.direction.Y * 1 + this.direction.Z * 1) / this.direction.X;

                        float perpendicularLength = MathF.Sqrt(perpendicularX * perpendicularX + 0 + 1);

                        position.X += perpendicularX * length / perpendicularLength;
                        position.Z += length / perpendicularLength;

                        target.X += perpendicularX * length / perpendicularLength;
                        target.Z += length / perpendicularLength;

                        break;
                    }

                    else if (this.direction.Z != 0)
                    {
                        float perpendicularZ = -1 * (this.direction.X * 1 + 0 * this.direction.Y * 1) / this.direction.Z;

                        float perpendicularLength = MathF.Sqrt(perpendicularZ * perpendicularZ + 0 + 1);

                        position.X += length / perpendicularLength;
                        position.Z += perpendicularZ * length / perpendicularLength;

                        target.X += length / perpendicularLength;
                        target.Z += perpendicularZ * length / perpendicularLength;

                        break;
                    }
                    break;

                case Axis.Y:
                    position.Y += length;
                    target.Y += length;
                    break;

                case Axis.Z:
                    position.X += this.direction.X * length;
                    position.Y += this.direction.Y * length;
                    position.Z += this.direction.Z * length;

                    target.X += this.direction.X * length;
                    target.Y += this.direction.Y * length;
                    target.Z += this.direction.Z * length;
                    break;
            }
            CameraUpdateDirection();
        }
        
        
        public void CameraRotation(RotationDirection direction, float fi, RichTextBox richText)
        {
            
            if (direction == RotationDirection.LeftRight)
            {
                this.leftRightAngle += fi;
                richText.Text += this.leftRightAngle.ToString() + " ";
                if (this.leftRightAngle >= 360)
                    this.leftRightAngle -= 360;
                else if (this.leftRightAngle <= -360)
                    this.leftRightAngle += 360;
                CameraUpdateDirection();
            }
            else if (direction == RotationDirection.UpDown)
            {
                this.upDownAngle += fi;
                richText.Text += fi.ToString() + " ";
                if (this.upDownAngle > 90)
                    this.upDownAngle = 90;
                else if (this.upDownAngle < -90)
                    this.upDownAngle = -90;
                CameraUpdateDirection();
                richText.Text += this.upDownAngle + " ";
            }
            
        }

        public void CameraUpdateDirection()
        {
            this.direction = this.target - this.position;
            List<Objects.Point> cameraDirectionList = new List<Objects.Point> { this.direction };

            AffineTransform.RotationAroundY(cameraDirectionList, leftRightAngle);
            float currentX = direction.X;
            float currentZ = direction.Z;
            AffineTransform.RotationAroundY(cameraDirectionList, -leftRightAngle);

            AffineTransform.RotationAroundX(cameraDirectionList, upDownAngle);
            float currentY = direction.Y;
            AffineTransform.RotationAroundX(cameraDirectionList, -upDownAngle);

            this.direction.X = currentX;
            this.direction.Y = currentY;
            this.direction.Z = currentZ;

            Mathematics.Vector.Normalization(this.direction.CoordinateVector);
        }
        

    }
}
