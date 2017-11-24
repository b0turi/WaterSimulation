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

        public Window(int wid, int hei) : base (wid, hei, new GraphicsMode(32, 24,0,4), "WaterSimulation", GameWindowFlags.Fullscreen, DisplayDevice.Default)
        {
            EngineCore.Dimensions = new Vector2(Width, Height);
            EngineCore.gameCamera = new Camera(EngineCore.Dimensions, new Vector3(0, 10, 30), new Vector3(15,0,0));
        }

        protected override void OnLoad(EventArgs e)
        {
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.Enable(EnableCap.PolygonSmooth);
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);
        }
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            if (Keyboard[Key.Escape])
                Exit();
            if (Keyboard[Key.W])
                EngineCore.gameCamera.position.Y++;
            if (Keyboard[Key.S])
                EngineCore.gameCamera.position.Y--;
            if (Keyboard[Key.A])
                EngineCore.gameObjects["water"].position.Y -= 0.1f;
            if (Keyboard[Key.D])
                EngineCore.gameObjects["water"].position.Y += 0.1f;
            if (Keyboard[Key.Q])
                EngineCore.gameCamera.rotation.X++;
            if (Keyboard[Key.E])
                EngineCore.gameCamera.rotation.X--;
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
            EngineCore.Render();
            GL.Flush();
            SwapBuffers();
        }
    }
}
