using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace WaterSimulation
{
    public class Window : GameWindow
    {
        public Window(int wid, int hei) : base (wid, hei, GraphicsMode.Default, "WaterSimulation", GameWindowFlags.Fullscreen, DisplayDevice.Default)
        {
            EngineCore.Dimensions = new Vector2(Width, Height);
            EngineCore.gameCamera = new Camera(EngineCore.Dimensions, new Vector3(0, 0, 15));
            EngineCore.meshes.Add("Quad", new Mesh(
                //Vertices
                new List<Vector3>(){
                    new Vector3(-1,1,0),
                    new Vector3(-1,-1,0),
                    new Vector3(1,-1,0),
                    new Vector3(1,1,0)
                },
                //UV Map
                new List<Vector2>()
                {
                    new Vector2(0,0),
                    new Vector2(0,1),
                    new Vector2(1,1),
                    new Vector2(1,0)
                },
                //Normals
                new List<Vector3>()
                {
                    new Vector3(0,0,1),
                    new Vector3(0,0,1),
                    new Vector3(0,0,1),
                    new Vector3(0,0,1)
                },

                //Faces
                new List<int>()
                {
                    0,1,3,3,1,2
                }));
        }

        protected override void OnLoad(EventArgs e)
        {
            GL.Enable(EnableCap.DepthTest);

        }
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            if (Keyboard[Key.Escape])
                Exit();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(new Point(), new Size(Width, Height));
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.ClearColor(Color.Blue);

            foreach (Entity obj in EngineCore.gameObjects.Values)
            {
                if (obj.GetType() == new RenderedEntity(new Vector3()).GetType())
                {
                    obj.rotation.Y++;
                    ((RenderedEntity)obj).Render();
                }
            }

            GL.Flush();
            SwapBuffers();
        }
    }
}
