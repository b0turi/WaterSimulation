using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterSimulation.ShaderPrograms
{
    public class ShadowShader : Shader
    {
        public ShadowShader(string vertex, string fragment) : base(vertex, fragment) { }

        public override void FillShader()
        {
            AddAttribute("Position", 0);

            AddUniform("Model", GetUniform("model"), UniformDataType.Matrix4);
            AddUniform("View", GetUniform("view"), UniformDataType.Matrix4);
            AddUniform("Projection", GetUniform("projection"), UniformDataType.Matrix4);
        }

        public override void Update()
        {
            LoadUniform("Model", currentObject.ModelMatrix());
            LoadUniform("View", EngineCore.gameCamera.View);
            LoadUniform("Projection", EngineCore.gameCamera.Projection);
        }
    }
}
