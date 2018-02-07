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

        public Vector3 forward;
        public Vector3 right;

        public Matrix4 View;
        public Matrix4 Projection;

        private int calcCount = 0;

        public Camera(Vector2 windowSize, Vector3 position, Vector3 rotation = default(Vector3), Vector3 scale = default(Vector3)) : base(position, rotation, scale)
        {
            this.window = windowSize;
            CalculateMatrices(true);
        }

        public override void Update()
        {
            CalculateMatrices(false);

            Matrix4 transformationMatrix = Matrix4.CreateTranslation(position);
            transformationMatrix *= Matrix4.CreateRotationY(rotation.Y * (float)(Math.PI / 180));
            transformationMatrix *= Matrix4.CreateRotationX(rotation.X * (float)(Math.PI / 180));
            forward = transformationMatrix.Column2.Xyz;
            right = transformationMatrix.Column0.Xyz;

            calcCount = 0;
        }

        public void CalculateMatrices(bool setProj)
        {
            View = ViewMatrix();
            if(setProj)
                Projection = ProjectionMatrix();
        }

        /// <summary>
        /// Create a view matrix based off of the camera's current position and rotation
        /// </summary>
        /// <returns>The view matrix</returns>
        private Matrix4 ViewMatrix()
        {
            Matrix4 View = Matrix4.CreateTranslation(new Vector3(-position.X, -position.Y, -position.Z));
            View *= Matrix4.CreateRotationY(rotation.Y * (float)(Math.PI / 180));
            View *= Matrix4.CreateRotationX(rotation.X * (float)(Math.PI / 180));
            View *= Matrix4.CreateRotationZ(rotation.Z * (float)(Math.PI / 180));
            calcCount++;
            return View;
        }

        /// <summary>
        /// Create a projection matrix based off of the provided values for the Field of View and Near/Far planes
        /// </summary>
        /// <returns>The projection matrix</returns>
        private Matrix4 ProjectionMatrix()
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

        public override void RenderWith(Shader shader)
        {
        }
    }
}
