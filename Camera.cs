using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace WaterSimulation
{
    public class Camera : Entity
    {
        private int fov = 60;
        private int zNear = 1;
        private int zFar = 5000;
        private Vector2 window = new Vector2();


        public Camera(Vector2 windowSize, Vector3 position, Vector3 rotation = default(Vector3), Vector3 scale = default(Vector3)) : base(position, rotation, scale)
        {
            this.window = windowSize;
        }

        public override void Update()
        {
        }

        /// <summary>
        /// Create a view matrix based off of the camera's current position and rotation
        /// </summary>
        /// <returns>The view matrix</returns>
        public Matrix4 ViewMatrix()
        {
            Matrix4 View = Matrix4.CreateTranslation(new Vector3(-position.X, -position.Y, -position.Z));
            View *= Matrix4.CreateRotationX(rotation.X * (float)(Math.PI / 180));
            View *= Matrix4.CreateRotationY(rotation.Y * (float)(Math.PI / 180));
            View *= Matrix4.CreateRotationZ(rotation.Z * (float)(Math.PI / 180));
            return View;
        }

        /// <summary>
        /// Create a projection matrix based off of the provided values for the Field of View and Near/Far planes
        /// </summary>
        /// <returns>The projection matrix</returns>
        public Matrix4 ProjectionMatrix()
        {
            Matrix4 Projection = new Matrix4();

            float aspect = window.X / window.Y;
            float yScale = (1f / (float)Math.Tan((Math.PI / 180) * (fov / 2f))) * aspect;
            float xScale = yScale / aspect;
            float frustum = zFar - zNear;

            Projection.M11 = xScale;
            Projection.M22 = yScale;
            Projection.M33 = -((zFar + zNear) / frustum);
            Projection.M34 = -1;
            Projection.M43 = -((2 * zNear * zFar) / frustum);
            Projection.M44 = 0;
            return Projection;
        }

        public override void Render()
        {
        }
    }
}
