using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using OpenTK;

namespace WaterSimulation
{
    public class Light : Entity
    {
        public Color color;
        public Light(Vector3 position, Color color, Vector3 rotation = default(Vector3), Vector3 scale = default(Vector3)) : base(position, rotation, scale)
        {
            this.color = color;
        }

        public override void Render()
        {
        }

        public override void Update()
        {

        }
    }
}
