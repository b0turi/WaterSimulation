using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace WaterSimulation
{
    public class FrameBuffer
    {
        int frameBuffer = 0;
        public int textureAttachment = 0;
        public int depthTextureAttachment = 0;
        int depthBuffer = 0;
        Vector2 imgScale;
        bool texture = true;

        public FrameBuffer(Vector2 imgScale, string name, bool depthTexture, bool texture = true)
        {
            this.imgScale = imgScale;
            this.texture = texture;

            GL.GenFramebuffers(1, out frameBuffer);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, frameBuffer);
            if (texture)
            {
                GL.DrawBuffer(DrawBufferMode.ColorAttachment0);

                textureAttachment = GL.GenTexture();
                GL.BindTexture(TextureTarget.Texture2D, textureAttachment);
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, (int)imgScale.X, (int)imgScale.Y, 0, PixelFormat.Rgb, PixelType.UnsignedByte, (IntPtr)null);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                GL.FramebufferTexture(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, textureAttachment, 0);
            }
            else
            {
                GL.DrawBuffer(DrawBufferMode.None);
                GL.ReadBuffer(ReadBufferMode.None);
            }

            

            if(depthTexture)
            {
                depthTextureAttachment = GL.GenTexture();
                GL.BindTexture(TextureTarget.Texture2D, depthTextureAttachment);
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.DepthComponent, (int)imgScale.X, (int)imgScale.Y, 0, PixelFormat.DepthComponent, PixelType.Float, (IntPtr)null);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
                GL.FramebufferTexture(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachmentExt, depthTextureAttachment, 0);
            }
            else
            {
                GL.GenRenderbuffers(1, out depthBuffer);
                GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, depthBuffer);
                GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.DepthComponent, (int)imgScale.X, (int)imgScale.Y);
                GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, RenderbufferTarget.Renderbuffer, depthBuffer);
            }
            

            UnBind();

            Texture fbo = new Texture(textureAttachment, (int)imgScale.X, (int)imgScale.Y);
            EngineCore.AddTexture(fbo, name);
        }

        public void Bind()
        {
            GL.BindTexture(TextureTarget.Texture2D, 0);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, frameBuffer);
            GL.Viewport(0, 0, (int)imgScale.X, (int)imgScale.Y);

            if (texture)
                GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.Clear(ClearBufferMask.DepthBufferBit);
            GL.ClearDepth(1);
        }

        public void UnBind()
        {
            if(texture)
                GL.BindTexture(TextureTarget.Texture2D, 0);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            GL.Viewport(0, 0, (int)EngineCore.Dimensions.X, (int)EngineCore.Dimensions.Y);
        }
    }
}
