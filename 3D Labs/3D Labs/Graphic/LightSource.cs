using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3D_Labs.Graphic
{
    public class LightSource
    {
        public Graphic.Objects.Point position;
        public float intensity;
        public LightSource(Objects.Point position, float intensity)
        {
            this.position = position;
            this.intensity = intensity;
        }
    }
}
