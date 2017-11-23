using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace WaterSimulation
{
    public abstract class Entity
    {
        public Vector3 position;
        public Vector3 rotation;
        public Vector3 scale;

        public Entity(Vector3 position, Vector3 rotation = new Vector3(), Vector3 scale = new Vector3())
        {
            this.position = position;
            this.rotation = rotation;
            this.scale = scale;
        }

        public abstract void Update();
        public abstract void Render();

        public Matrix4 ModelMatrix()
        {
            Matrix4 Model = Matrix4.CreateScale(scale);
            Model *= Matrix4.CreateRotationX(rotation.X * (float)(Math.PI / 180));
            Model *= Matrix4.CreateRotationY(rotation.Y * (float)(Math.PI / 180));
            Model *= Matrix4.CreateRotationZ(rotation.Z * (float)(Math.PI / 180));
            Model *= Matrix4.CreateTranslation(position);

            return Model;
        }
    }
}
