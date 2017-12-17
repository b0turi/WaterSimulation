using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace WaterSimulation.ShaderPrograms
{
    public class SkyboxShader : Shader
    {
        public SkyboxShader(string vertex, string fragment) : base(vertex, fragment) { }

        public override void FillShader()
        {
            AddAttribute("position", 0);
            AddUniform("projection", GetUniform("projection"), UniformDataType.Matrix4);
            AddUniform("view", GetUniform("view"), UniformDataType.Matrix4);
            AddUniform("scale", GetUniform("scale"), UniformDataType.Float);
            AddUniform("cubeMap", GetUniform("cubeMap"), UniformDataType.Float);
        }

        public override void Update()
        {
            LoadUniform("projection", EngineCore.gameCamera.Projection);
            Matrix4 newView = EngineCore.gameCamera.View;
            newView.M41 = 0;
            newView.M42 = 0;
            newView.M43 = 0;
            LoadUniform("view", newView);
            LoadUniform("scale", (float)(((Skybox)currentObject).cubeScale));
            LoadUniform("cubeMap", 0);
        }
    }
}
