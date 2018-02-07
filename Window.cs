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

        int num = 0;
        bool num1Down = false;
        bool num2Down = false;
        bool selecting = false;
        Random rotationGenerator = new Random();
        Vector2 lastMousePosition = new Vector2(0,0);
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            EngineCore.gameCamera.Update();
            if (Keyboard[Key.Escape])
                Exit();
            EngineCore.gameCamera.rotation.Y += 0.08f * (Mouse.GetState().X - lastMousePosition.X);
            EngineCore.gameCamera.rotation.X += 0.08f * (Mouse.GetState().Y - lastMousePosition.Y);
            if (!selecting)
            {
                if (Keyboard[Key.W])
                    EngineCore.gameCamera.position -= EngineCore.gameCamera.forward * 0.15f;
                if (Keyboard[Key.S])
                    EngineCore.gameCamera.position += EngineCore.gameCamera.forward * 0.15f;
                if (Keyboard[Key.D])
                    EngineCore.gameCamera.position += EngineCore.gameCamera.right * 0.15f;
                if (Keyboard[Key.A])
                    EngineCore.gameCamera.position -= EngineCore.gameCamera.right * 0.15f;
            } else
            {
                if (Keyboard[Key.W])
                    EngineCore.selected.position -= new Vector3(EngineCore.gameCamera.forward.X, 0, EngineCore.gameCamera.forward.Z) * 0.15f;
                if (Keyboard[Key.S])
                    EngineCore.selected.position += new Vector3(EngineCore.gameCamera.forward.X, 0, EngineCore.gameCamera.forward.Z) * 0.15f;
                if (Keyboard[Key.D])
                    EngineCore.selected.position += new Vector3(EngineCore.gameCamera.right.X, 0, EngineCore.gameCamera.right.Z) * 0.15f;
                if (Keyboard[Key.A])
                    EngineCore.selected.position -= new Vector3(EngineCore.gameCamera.right.X, 0, EngineCore.gameCamera.right.Z) * 0.15f;
                if (Keyboard[Key.Q])
                    EngineCore.selected.position.Y -= 0.025f;
                if (Keyboard[Key.E])
                    EngineCore.selected.position.Y += 0.025f;

                EngineCore.gameCamera.position += EngineCore.gameCamera.forward * Mouse.GetState().WheelPrecise * 0.001f;
            }

            if (Keyboard[Key.Number1] && !num1Down)
            {
                RenderedEntity tree = new RenderedEntity(Vector3.Zero, new Vector3(0, rotationGenerator.Next(360), 0), Vector3.One * ((float)rotationGenerator.NextDouble() * 0.5f + 0.5f), "tree", "Default", "treeImg");
                EngineCore.AddObject("tree"+num, tree);
                num++;
                num1Down = true;
                EngineCore.selected = tree;
                selecting = true;
            }
            if(!Keyboard[Key.Number1])
                num1Down = false;

            if (Keyboard[Key.Number2] && !num2Down)
            {
                RenderedEntity rock = new RenderedEntity(Vector3.Zero, new Vector3(0, rotationGenerator.Next(360), 0), Vector3.One * ((float)rotationGenerator.NextDouble() * 0.2f + 0.2f), "rock", "Default", "rockImg");
                EngineCore.AddObject("rock" + num, rock);
                num++;
                num2Down = true;
                EngineCore.selected = rock;
                selecting = true;
            }
            if (!Keyboard[Key.Number2])
                num2Down = false;

            if (Keyboard[Key.Enter])
                selecting = false;

            if (Keyboard[Key.Up])
            {
                ((Water)EngineCore.gameObjects["water"]).position.Y += 0.05f;
                EngineCore.gameObjects["Boat"].position.Y += 0.05f;
            }
            if (Keyboard[Key.Down])
            {
                ((Water)EngineCore.gameObjects["water"]).position.Y -= 0.05f;
                EngineCore.gameObjects["Boat"].position.Y -= 0.05f;
            }
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
