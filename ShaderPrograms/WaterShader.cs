using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace WaterSimulation.ShaderPrograms
{
    public class WaterShader : Shader
    {
        public List<Light> lights = new List<Light>();

        private float waveSpeed = 0.03f;
        private float moveFactor = 0;
        public WaterShader(string vertex, string fragment, Vector4 clippingPlane = default(Vector4)) : base(vertex, fragment) { }

        public override void FillShader()
        {
            AddAttribute("Position", 0);
            AddAttribute("UV Map", 1);
            AddAttribute("Normals", 2);

            AddUniform("model", GetUniform("model"), UniformDataType.Matrix4);
            AddUniform("view", GetUniform("view"), UniformDataType.Matrix4);
            AddUniform("projection", GetUniform("projection"), UniformDataType.Matrix4);

            AddUniform("reflection", GetUniform("reflection"), UniformDataType.Float);
            AddUniform("refraction", GetUniform("refraction"), UniformDataType.Float);
            AddUniform("dudv", GetUniform("dudv"), UniformDataType.Float);
            AddUniform("normal", GetUniform("normalMap"), UniformDataType.Float);
            AddUniform("depthMap", GetUniform("depthMap"), UniformDataType.Float);


            AddUniform("lightColor", GetUniform("lightColor"), UniformDataType.Vector3);
            AddUniform("lightPosition", GetUniform("lightPosition"), UniformDataType.Vector3);

            AddUniform("moveFactor", GetUniform("moveFactor"), UniformDataType.Float);
            AddUniform("cameraPos", GetUniform("cameraPos"), UniformDataType.Vector3);
        }

        public override void Update()
        {
            GL.Enable(EnableCap.ClipDistance0);

            LoadUniform("model", currentObject.ModelMatrix());
            LoadUniform("view", EngineCore.gameCamera.View);
            LoadUniform("projection", EngineCore.gameCamera.Projection);

            LoadUniform("reflection", 0);
            LoadUniform("refraction", 1);
            LoadUniform("dudv", 2);
            LoadUniform("normal", 3);
            LoadUniform("depthMap", 4);

            LoadUniform("cameraPos", EngineCore.gameCamera.position);
            if(lights.Count > 0)
            {
                LoadUniform("lightPosition", lights[0].position);
                LoadUniform("lightColor", new Vector3(lights[0].color.R, lights[0].color.G, lights[0].color.B));
            }

            moveFactor += waveSpeed / EngineCore.FPS;
            moveFactor %= 1;

            LoadUniform("moveFactor", moveFactor);

        }
    }
}
