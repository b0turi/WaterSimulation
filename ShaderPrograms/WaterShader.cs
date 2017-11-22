using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace WaterSimulation.ShaderPrograms
{
    class WaterShader : Shader
    {
        public List<Light> lights = new List<Light>();

        public WaterShader(string vertex, string fragment) : base(vertex, fragment) { }

        public override void FillShader()
        {
            AddAttribute("Position", 0);
            AddAttribute("UV Map", 1);
            AddAttribute("Normals", 2);

            AddUniform("model", GetUniform("model"), UniformDataType.Matrix4);
            AddUniform("view", GetUniform("view"), UniformDataType.Matrix4);
            AddUniform("projection", GetUniform("projection"), UniformDataType.Matrix4);
        }

        public override void Update()
        {
            LoadUniform("model", currentObject.ModelMatrix());
            LoadUniform("view", EngineCore.gameCamera.ViewMatrix());
            LoadUniform("projection", EngineCore.gameCamera.ProjectionMatrix());

            for (int i = 0; i < lights.Count; i++)
            {
                if (!Uniforms.Keys.ToList().Contains("lightPos" + i))
                {
                    AddUniform("lightPos" + i, GetUniform("lightPos" + i), UniformDataType.Vector3);
                    AddUniform("lightColor" + i, GetUniform("lightColor" + i), UniformDataType.Vector3);
                }

                LoadUniform("lightPos" + i, lights[i].position);
                LoadUniform("lightColor" + i, new Vector3(lights[i].color.R / 255f, lights[i].color.G / 255f, lights[i].color.B / 255f));
            }
        }
    }
}
