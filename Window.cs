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
        Vector2 lastMousePosition = new Vector2(0,0);
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            EngineCore.gameCamera.Update();
            if (Keyboard[Key.Escape])
                Exit();
            EngineCore.gameCamera.rotation.Y += 0.2f * (Mouse.GetState().X - lastMousePosition.X);
            EngineCore.gameCamera.rotation.X += 0.2f* (Mouse.GetState().Y - lastMousePosition.Y);
            if (Keyboard[Key.W])
                EngineCore.gameCamera.position -= EngineCore.gameCamera.forward * 0.5f;
            if (Keyboard[Key.S])
                EngineCore.gameCamera.position += EngineCore.gameCamera.forward * 0.5f;
            if (Keyboard[Key.D])
                EngineCore.gameCamera.position += EngineCore.gameCamera.right * 0.5f;
            if (Keyboard[Key.A])
                EngineCore.gameCamera.position -= EngineCore.gameCamera.right * 0.5f;

            EngineCore.gameCamera.CalculateMatrices(false);
            lastMousePosition = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
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
