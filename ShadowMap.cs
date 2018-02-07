using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace WaterSimulation
{
    public class ShadowMap
    {
        int IMG_SIZE = 2048;
        Vector2 dimensions;
        FrameBuffer depths;
        Matrix4 offset = new Matrix4();
        public ShadowMap()
        {
            dimensions = new Vector2(IMG_SIZE, IMG_SIZE);
            depths = new FrameBuffer(dimensions, "Shadows", true, false);
            offset *= Matrix4.CreateTranslation(new Vector3(0.5f, 0.5f, 0.5f));
            offset *= Matrix4.CreateScale(0.5f);
        }

        public void Update()
        {
            depths.Bind();
            depths.UnBind();
        }
    }
}
