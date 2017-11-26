using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace WaterSimulation
{
    public class Skybox : RenderedEntity
    {
        public int cubeScale;
        int texID;
        float[] verts = new float[72]{
            1, -1, 1, 
            1, 1, 1,
            1, 1, -1,
            1, -1, -1,

            -1, -1, -1,
            -1, 1, -1,
            -1, 1, 1,
            -1, -1, 1,

            -1, 1, -1,
            1, 1, -1,
            1, 1, 1,
            -1, 1, 1,

            -1, -1, 1,
            1, -1, 1,
            1, -1, -1,
            -1, -1, -1,

            1, -1, 1,
            -1, -1, 1,
            -1, 1, 1,
            1, 1, 1,

            -1, -1, -1,
            1, -1, -1,
            1, 1, -1,
            -1, 1, -1

        };
        int vaoID;
        int vbo;

        public Skybox(int scale, int texID) : base(Vector3.Zero, Vector3.Zero, Vector3.Zero, null, "Skybox", null, null)
        {
            this.cubeScale = scale;
            this.texID = texID;
            GL.GenVertexArrays(1, out vaoID);
            GL.BindVertexArray(vaoID);

            GL.GenBuffers(1, out vbo);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(verts.Length * sizeof(float)), verts, BufferUsageHint.StaticDraw);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, sizeof(float) * 3, 0);
            GL.EnableVertexAttribArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
        }

        public override void Draw()
        {
            GL.Disable(EnableCap.CullFace);
            GL.BindVertexArray(vaoID);
            GetShader().currentObject = this;
            GetShader().Start();

            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.TextureCubeMap, texID);

            GL.DrawArrays(PrimitiveType.Quads, 0, 24);

            GetShader().Stop();
            GL.BindVertexArray(0);
            GL.Enable(EnableCap.CullFace);
        }
    }
}
