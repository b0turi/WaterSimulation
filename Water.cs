using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using System.Drawing;
using OpenTK.Graphics.OpenGL;
using WaterSimulation.ShaderPrograms;

namespace WaterSimulation
{
    public class Water : RenderedEntity
    {
        private FrameBuffer reflection;
        private FrameBuffer refraction;
        public Water(Vector3 position, Color color, Vector2 imgSize, string dudv, Vector3 scale = default(Vector3)) : base(position, new Vector3(-90,0,0), scale, "Quad", "Water", dudv, null)
        {
            reflection = new FrameBuffer(imgSize, "reflection");
            refraction = new FrameBuffer(imgSize, "refraction");
        }

        public override void Draw()
        {

            //Render to the frame buffers

            GL.Enable(EnableCap.ClipDistance0);

            float dist = 2 * (EngineCore.gameCamera.position.Y - position.Y);
            EngineCore.gameCamera.position.Y -= dist;
            EngineCore.gameCamera.rotation.X *= -1;

            EngineCore.shaders["Default"].SetClippingPlane(new Vector4(0, 1, 0, -position.Y));
            reflection.Bind();
            EngineCore.RenderWithout(this);
            reflection.UnBind();

            EngineCore.gameCamera.position.Y += dist;
            EngineCore.gameCamera.rotation.X *= -1;


            EngineCore.shaders["Default"].SetClippingPlane(new Vector4(0, -1, 0, position.Y));
            refraction.Bind();
            EngineCore.RenderWithout(this);
            refraction.UnBind();

            EngineCore.shaders["Default"].SetClippingPlane(new Vector4(0, 0, 0, 0));

            GL.Disable(EnableCap.ClipDistance0);

            GL.UseProgram(GetShader().ProgramID);

            GL.BindVertexArray(GetMesh().vaoID);
            GetShader().currentObject = this;
            GetShader().Start();

            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, reflection.textureAttachment);

            GL.ActiveTexture(TextureUnit.Texture1);
            GL.BindTexture(TextureTarget.Texture2D, refraction.textureAttachment);

            GL.ActiveTexture(TextureUnit.Texture2);
            GL.BindTexture(TextureTarget.Texture2D, GetTexture().texID); //DuDv Map


            GL.DrawElements(PrimitiveType.Triangles, GetMesh().indices.Count, DrawElementsType.UnsignedInt, 0);

            GetShader().Stop();
            GL.BindVertexArray(0);
        }


    }
}
