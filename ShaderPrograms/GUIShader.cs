using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace WaterSimulation.ShaderPrograms
{
    public class GUIShader : Shader
    {
        public GUIShader(string vertex, string fragment) : base(vertex, fragment) { }

        public override void FillShader()
        {
            AddAttribute("Position", 1);
            AddAttribute("UV Map", 0);

            AddUniform("Model", GetUniform("model"), UniformDataType.Matrix4);
            AddUniform("View", GetUniform("view"), UniformDataType.Matrix4);
            AddUniform("Projection", GetUniform("projection"), UniformDataType.Matrix4);
        }

        public override void Update()
        {
            LoadUniform("Model", currentObject.ModelMatrix());
            LoadUniform("View", EngineCore.gameCamera.ViewMatrix());
            LoadUniform("Projection", EngineCore.gameCamera.ProjectionMatrix());
        }
    }
}
