using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;

namespace WaterSimulation
{
    public class Terrain : RenderedEntity
    {
        private static float SIZE = 10;
        private static float VERTEX_COUNT = 50;
        private static float MAX_HEIGHT = 3;
        private string heightMap;

        private float x;
        private float z;

        private Bitmap bmp;

        public Terrain(string name, string texture, string heightMap) : base(Vector3.Zero, Vector3.Zero, Vector3.One, null, "Default")
        {
            this.texture = texture;
            this.heightMap = heightMap;

            bmp = new Bitmap("Assets/Images/" + heightMap);

            generateTerrain(name);
            mesh = name;
        }

        public override void Draw()
        {
            GL.BindVertexArray(GetMesh().vaoID);
            GetShader().currentObject = this;
            GetShader().Start();
            
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, GetTexture().texID);

            GL.DrawElements(PrimitiveType.Triangles, GetMesh().indices.Count, DrawElementsType.UnsignedInt, 0);

            GetShader().Stop();
            GL.BindVertexArray(0);
        }

        private float getHeight(int i, int j)
        {
            if (i < 0 || i >= bmp.Width-1 || j < 0 || j >= bmp.Height-1)
                return 0;
            return (bmp.GetPixel((int)(i / VERTEX_COUNT * (bmp.Width - 1)), (int)(j / VERTEX_COUNT * (bmp.Height - 1))).R / 255f) * MAX_HEIGHT * 2 - MAX_HEIGHT;
        }

        private void generateTerrain(string name)
        {
            float count = VERTEX_COUNT * VERTEX_COUNT;
            List<Vector3> vertices = new List<Vector3>();
            List<Vector3> normals = new List<Vector3>();
            List<Vector2> textureCoords = new List<Vector2>();
            List<int> indices = new List<int>();
            int vertexPointer = 0;

            for (int i = 0; i < VERTEX_COUNT; i++)
            {
                for (int j = 0; j < VERTEX_COUNT; j++)
                {
                    vertices.Add(new Vector3((float)j / ((float)VERTEX_COUNT - 1) * SIZE * 2 - SIZE, getHeight(i, j), (float)i / ((float)VERTEX_COUNT - 1) * SIZE * 2 - SIZE));

                    //Calculate normal
                    float heightL = getHeight(i - 1, j);
                    float heightR = getHeight(i + 1, j);
                    float heightD = getHeight(i, j - 1);
                    float heightU = getHeight(i, j + 1);

                    Vector3 normal = new Vector3(heightL - heightR, 2, heightD - heightU);
                    normal.Normalize();
                    normals.Add(normal);


                    textureCoords.Add(new Vector2((float)j / ((float)VERTEX_COUNT - 1), (float)i / ((float)VERTEX_COUNT - 1)));
                    vertexPointer++;
                }
            }
            for (int gz = 0; gz < VERTEX_COUNT - 1; gz++)
            {
                for (int gx = 0; gx < VERTEX_COUNT - 1; gx++)
                {
                    int topLeft = (gz * (int)VERTEX_COUNT) + gx;
                    int topRight = topLeft + 1;
                    int bottomLeft = ((gz + 1) * (int)VERTEX_COUNT) + gx;
                    int bottomRight = bottomLeft + 1;
                    indices.Add(topLeft);
                    indices.Add(bottomLeft);
                    indices.Add(topRight);
                    indices.Add(topRight);
                    indices.Add(bottomLeft);
                    indices.Add(bottomRight);
                }
            }
            EngineCore.meshes.Add(name, new Mesh(vertices, textureCoords, normals, indices));
        }
    }
}
