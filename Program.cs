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

            SkyboxShader skybox = new SkyboxShader("skybox.vert", "skybox.frag");
            String skyboxShader = EngineCore.AddShader(skybox, "Skybox");

            ShadowShader shadow = new ShadowShader("shadow.vert", "shadow.frag");
            String shadowShader = EngineCore.AddShader(shadow, "Shadow");

            TerrainShader terrain = new TerrainShader("terrain.vert", "terrain.frag");
            String terrainShader = EngineCore.AddShader(terrain, "Terrain");


            String cubeModel = EngineCore.AddModel("cubeything.obj", "cube");
            String terrainModel = EngineCore.AddModel("terrain.obj", "terrain");
            String quad = EngineCore.AddModel("quad.obj", "Quad");
            String cubeObj = EngineCore.AddModel("cube.obj", "Cube");
            EngineCore.AddModel("tree.obj", "tree");
            EngineCore.AddModel("rock.obj", "rock");





            Light l = new Light(new Vector3(0, 3, 0), System.Drawing.Color.White);
            ds.lights.Add(l);
            terrain.lights.Add(l);

            Light l2 = new Light(new Vector3(0, 10, 0), System.Drawing.Color.White);
            water.lights.Add(l2);

            EngineCore.AddImage("tree.png", "treeImg");
            EngineCore.AddImage("rock.png", "rockImg");

            String boatObj = EngineCore.AddModel("boat.obj", "Boat");
            EngineCore.AddImage("boards.jpg", "Boards");
            Boat boat = new Boat(new Vector3(2, 0.2f, 2), new Vector3(0, 34, 0), new Vector3(0.25f, 0.25f, 0.25f), "Boards");
            EngineCore.AddObject("Boat", boat);

            String ground = EngineCore.AddImage("grass.jpg", "Ground");
            Terrain terrainObj = new Terrain("New Terrain", "Ground", "heightMap.png");
            EngineCore.AddObject("Terrain", terrainObj);

            EngineCore.AddImage("dudv.png", "DuDvMap");
            EngineCore.AddImage("normal.png", "NormalMap");
            Water waterObj = new Water(new Vector3(0, 0.25f, 0), System.Drawing.Color.Blue, new Vector2(1920, 1080), "DuDvMap", new Vector3(10, 10, 10));
            waterObj.AttachNormalMap("NormalMap");
            EngineCore.AddObject("water", waterObj);

            EngineCore.AddSkybox(new string[]{ "Skybox/xpos.png",
                "Skybox/xneg.png",
                "Skybox/ypos.png",
                "Skybox/yneg.png",
                "Skybox/zpos.png",
                "Skybox/zneg.png"}, "Skybox", 500);

            w.Run();
        }
    }
}
