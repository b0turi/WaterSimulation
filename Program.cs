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

            EngineCore.AddImage("default.png", "default");

            DefaultShader ds = new DefaultShader("default.vert", "default.frag");
            String defaultShader = EngineCore.AddShader(ds, "Default");

            GUIShader gui = new GUIShader("gui.vert", "gui.frag");
            String guiShader = EngineCore.AddShader(gui, "GUI");

            WaterShader water = new WaterShader("water.vert", "water.frag");
            String waterShader = EngineCore.AddShader(water, "Water");

            String cubeModel = EngineCore.AddModel("cubeything.obj", "cube");
            String quad = EngineCore.AddModel("quad.obj", "Quad");

            Light l = new Light(new Vector3(0, 0, 4), System.Drawing.Color.Bisque);
            ds.lights.Add(l);

            RenderedEntity cube = new RenderedEntity(new Vector3(0,0,0), Vector3.Zero, Vector3.One);
            if (cube.AttachShader(waterShader) && cube.AttachMesh(cubeModel))
                EngineCore.AddObject("cube", cube);
            w.Run();
        }
    }
}
