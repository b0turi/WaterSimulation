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
            String terrainModel = EngineCore.AddModel("terrain.obj", "terrain");
            String quad = EngineCore.AddModel("quad.obj", "Quad");

            Light l = new Light(new Vector3(0, 10, 0), System.Drawing.Color.Green);
            ds.lights.Add(l);
            water.lights.Add(l);

            RenderedEntity cube = new RenderedEntity(new Vector3(0,0,0), new Vector3(0,0,0), new Vector3(10,10,10));
            if (cube.AttachShader(defaultShader) && cube.AttachMesh(terrainModel))
                EngineCore.AddObject("cube", cube);

            RenderedEntity cubeyThing = new RenderedEntity(new Vector3(0, 2, 0), Vector3.Zero, Vector3.One, "cube", "Default");
            EngineCore.AddObject("cubeything", cubeyThing);

            EngineCore.AddImage("dudv.png", "DuDvMap");
            Water waterObj = new Water(new Vector3(0,0.25f,0), System.Drawing.Color.Blue, new Vector2(1920, 1080), "DuDvMap", new Vector3(10, 10, 10));
            EngineCore.AddObject("water", waterObj);
            


            w.Run();
        }
    }
}
