using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaterSimulation.ShaderPrograms;
using OpenTK;

namespace WaterSimulation
{
    class Program
    {
        static void Main(string[] args)
        {
            Window w = new Window(1920, 1080);

            DefaultShader ds = new DefaultShader("default.vert", "default.frag");
            String defaultShader = EngineCore.AddShader(ds, "Default");
            String dragonModel = EngineCore.AddModel("dragon.obj", "Dragon");

            Light l = new Light(new Vector3(0, 5, 0), System.Drawing.Color.Firebrick);
            ds.lights.Add(l);

            RenderedEntity sample = new RenderedEntity(Vector3.Zero, Vector3.Zero, new Vector3(0.25f, 0.25f, 0.25f));
            sample.AttachShader(defaultShader);
            sample.AttachMesh("Quad");
            EngineCore.AddObject("sample", sample);

            w.Run();
        }
    }
}
