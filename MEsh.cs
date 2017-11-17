using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace WaterSimulation
{
    public class Mesh
    {
        public List<Vector3> vertices;
        public List<Vector2> textureMap;
        public List<Vector3> normals;
        public List<int> indices;

        //Vertex Array Objects used to store information on each individual mesh
        public int vaoID;
        public string[] vao = new string[15];

        public Mesh(List<Vector3> vertices, List<Vector2> textureMap, List<Vector3> normals, List<int> indices)
        {
            this.vertices = vertices;
            this.textureMap = textureMap;
            this.normals = normals;
            this.indices = indices;

            int vao;
            GL.GenVertexArrays(1, out vao);
            GL.BindVertexArray(vao);

            vaoID = vao;



            WriteToAttribList(0, "Vertices", vertices, 3, Vector3.SizeInBytes);
            WriteToAttribList(1, "UV Map", textureMap, 2, Vector2.SizeInBytes);
            WriteToAttribList(2, "Normals", normals, 3, Vector3.SizeInBytes);

            //Index Buffer for more efficient rendering
            int index = 0;
            GL.GenBuffers(1, out index);

            //Add the indices to the buffer
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, index);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(indices.Count * sizeof(int)), indices.ToArray(), BufferUsageHint.StaticDraw);

            GL.BindVertexArray(0);
        }

        public void WriteToAttribList<T>(int attribNumber, string name, List<T> data, int dimensions, int size) where T : struct
        {
            int buffer = 0;
            GL.GenBuffers(1, out buffer);
            GL.BindBuffer(BufferTarget.ArrayBuffer, buffer);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(data.Count * size), data.ToArray(), BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(attribNumber, dimensions, VertexAttribPointerType.Float, false, 0, 0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            vao[attribNumber] = name;
        }
    }
}
