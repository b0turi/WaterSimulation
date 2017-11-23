using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace WaterSimulation
{
    public class RenderedEntity : Entity
    {
        protected string texture = null;
        private string mesh = null;
        private string material = null;
        private string shader = null;

        public RenderedEntity(Vector3 position, Vector3 rotation = default(Vector3), Vector3 scale = default(Vector3), string mesh = null, string shader = null, string texture = "default", string material = null) : base(position, rotation, scale)
        {
            this.texture = texture;
            this.mesh = mesh;
            this.material = material;
            this.shader = shader;
        }

        public override void Update()
        {
        }

        public override void Render()
        {
            GL.UseProgram(GetShader().ProgramID);
            Draw();
            GL.UseProgram(0);
        }

        public virtual void Draw()
        {
            GL.BindVertexArray(GetMesh().vaoID);
            GetShader().currentObject = this;
            GetShader().Start();

            //Bind the current model's texture
            if (texture != null)
            {
                GL.ActiveTexture(TextureUnit.Texture0);
                GL.BindTexture(TextureTarget.Texture2D, GetTexture().texID);
            }

            GL.DrawElements(PrimitiveType.Triangles, GetMesh().indices.Count, DrawElementsType.UnsignedInt, 0);

            GetShader().Stop();
            GL.BindVertexArray(0);
        }


        //Setters
        public bool AttachMesh(string meshName)
        {
            if (EngineCore.meshes.ContainsKey(meshName))
                mesh = meshName;
            else
                return false;
            return true;
        }
        public bool AttachMaterial(string materialName)
        {
            if (EngineCore.materials.ContainsKey(materialName))
                material = materialName;
            else
                return false;
            return true;
        }
        public bool AttachShader(string shaderName)
        {
            if (EngineCore.shaders.ContainsKey(shaderName))
                shader = shaderName;
            else
                return false;
            return true;
        }
        public bool AttachTexture(string textureName)
        {
            if (EngineCore.images.ContainsKey(textureName))
                texture = textureName;
            else
                return false;
            return true;
        }

        //Getters
        public Mesh GetMesh() { return EngineCore.meshes[mesh]; }
        public Texture GetTexture() { return EngineCore.images[texture]; }
        public Material GetMaterial() { return EngineCore.materials[material]; }
        public Shader GetShader() { return EngineCore.shaders[shader]; }

    }
}
