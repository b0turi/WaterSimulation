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
        FrameBuffer fb;
        RenderedEntity dispBoy;

        public Window(int wid, int hei) : base (wid, hei, GraphicsMode.Default, "WaterSimulation", GameWindowFlags.Fullscreen, DisplayDevice.Default)
        {
            EngineCore.Dimensions = new Vector2(Width, Height);
            EngineCore.gameCamera = new Camera(EngineCore.Dimensions, new Vector3(0, 0, 15));
        }

        protected override void OnLoad(EventArgs e)
        {
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.ClipDistance0);
            fb = new FrameBuffer(new Vector2(Size.Width, Size.Height), "whole scene");
            EngineCore.images.Add("fb", new Texture(fb.textureAttachment, Size.Width, Size.Height));
            dispBoy = new RenderedEntity(Vector3.Zero, Vector3.Zero, new Vector3(4,4,4), "Quad", "GUI", "fb");
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
            fb.Bind();
            foreach (Entity obj in EngineCore.gameObjects.Values)
            {
                if (obj.GetType() == new RenderedEntity(new Vector3()).GetType())
                {
                    ((RenderedEntity)obj).Render();
                }
            }
            fb.UnBind();

            dispBoy.Render();
            GL.Flush();
            SwapBuffers();
        }
    }
}
