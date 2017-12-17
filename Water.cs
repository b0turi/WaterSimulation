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
        string normalMap;
        public Water(Vector3 position, Color color, Vector2 imgSize, string dudv, Vector3 scale = default(Vector3)) : base(position, new Vector3(-90,0,0), scale, "Quad", "Water", dudv, null)
        {
            reflection = new FrameBuffer(imgSize, "reflection", false);
            refraction = new FrameBuffer(imgSize, "refraction", true);
        }

        public override void Draw()
        {
            GL.Enable(EnableCap.ClipDistance0);

            float dist = 2 * (EngineCore.gameCamera.position.Y - position.Y);
            EngineCore.gameCamera.position.Y -= dist;
            EngineCore.gameCamera.rotation.X *= -1;
            EngineCore.gameCamera.CalculateMatrices(false);

            EngineCore.shaders["Default"].SetClippingPlane(new Vector4(0, 1, 0, -position.Y+0.1f));
            reflection.Bind();
            EngineCore.RenderWithout(this);
            reflection.UnBind();

            EngineCore.gameCamera.position.Y += dist;
            EngineCore.gameCamera.rotation.X *= -1;
            EngineCore.gameCamera.CalculateMatrices(false);


            EngineCore.shaders["Default"].SetClippingPlane(new Vector4(0, -1, 0, position.Y+0.1f));
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

            GL.ActiveTexture(TextureUnit.Texture3);
            GL.BindTexture(TextureTarget.Texture2D, GetNormalMap().texID);

            GL.ActiveTexture(TextureUnit.Texture4);
            GL.BindTexture(TextureTarget.Texture2D, refraction.depthTextureAttachment);

            GL.DrawElements(PrimitiveType.Triangles, GetMesh().indices.Count, DrawElementsType.UnsignedInt, 0);

            GetShader().Stop();
            GL.BindVertexArray(0);
        }

        public bool AttachNormalMap(string normal)
        {
            if (EngineCore.images.ContainsKey(normal))
                normalMap = normal;
            else
                return false;
            return true;
        }

        //Getters
        public Texture GetNormalMap() { return EngineCore.images[normalMap]; }
    }
}
