using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace WaterSimulation
{
    public class Boat : RenderedEntity
    {
        float timer = 0;
        public Boat(Vector3 position, Vector3 rotation, Vector3 scale, string texture) : base(position, rotation, scale, "Boat", "Default", texture, null)
        {

        }
        public override void Render()
        {
            timer++;
            if (timer > 720)
                timer = 0;
            rotation.X = 2 * ((float)Math.Cos(timer * Math.PI/180) * 2 - 1);
            rotation.Z = 3 * (float)Math.Sin(timer / 2 * Math.PI / 180);
            base.Render();
        }
    }
}
