using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace WaterSimulation
{
    public class EngineCore
    {
        public static Vector2 Dimensions;
        public static Camera gameCamera;
        public static int FPS = 60;

        public static int Skybox;

        public static Dictionary<String, FrameBuffer> frameBuffers = new Dictionary<string, FrameBuffer>();
        public static Dictionary<String, Entity> gameObjects = new Dictionary<string, Entity>();
        public static Dictionary<String, Mesh> meshes = new Dictionary<string, Mesh>();
        public static Dictionary<String, Texture> images = new Dictionary<string, Texture>();
        public static Dictionary<String, Material> materials = new Dictionary<string, Material>();
        public static Dictionary<String, Shader> shaders = new Dictionary<string, Shader>();

        public static string AddFrameBuffer(String name, FrameBuffer fb)
        {
            frameBuffers.Add(name, fb);
            return name;
        }

        public static string AddObject(String name, Entity obj)
        {
            gameObjects.Add(name, obj);
            Console.WriteLine("Added an object called " + name);
            return name;
        }

        public static string AddModel(string path, string name)
        {
            if (meshes.Keys.Contains(name))
                throw new InvalidDataException("A mesh named " + name + " already exists in the engine memory");

            CheckFilepath("Assets/Models/" + path);

            string[] lines = { };
            List<Vector3> vertices = new List<Vector3>();
            List<Vector2> textures = new List<Vector2>();
            List<Vector3> normals = new List<Vector3>();
            List<int> indices = new List<int>();

            //After parsing the data, re-sort the info so that all vertices have their data aligned
            Vector2[] newTextures = new Vector2[1];
            Vector3[] newNormals = new Vector3[1];

            //Read in the .obj file
            lines = File.ReadAllLines("Assets/Models/" + path);

            foreach (string line in lines)
            {
                string[] currentLine = line.Split(' ');
                if (line.StartsWith("v "))
                    vertices.Add(new Vector3(float.Parse(currentLine[1]), float.Parse(currentLine[2]), float.Parse(currentLine[3])));
                else if (line.StartsWith("vt "))
                    textures.Add(new Vector2(float.Parse(currentLine[1]), float.Parse(currentLine[2])));
                else if (line.StartsWith("vn "))
                    normals.Add(new Vector3(float.Parse(currentLine[1]), float.Parse(currentLine[2]), float.Parse(currentLine[3])));
                else if (line.StartsWith("f "))
                {
                    if (newTextures.Length == 1)
                    {
                        newTextures = new Vector2[vertices.Count];
                        newNormals = new Vector3[vertices.Count];
                    }

                    string[] vertex1 = currentLine[1].Split('/');
                    string[] vertex2 = currentLine[2].Split('/');
                    string[] vertex3 = currentLine[3].Split('/');

                    processVertex(vertex1, indices, textures, normals, newTextures, newNormals);
                    processVertex(vertex2, indices, textures, normals, newTextures, newNormals);
                    processVertex(vertex3, indices, textures, normals, newTextures, newNormals);
                }
            }



            meshes.Add(name, new Mesh(vertices, newTextures.ToList(), newNormals.ToList(), indices));
            return name;
        }

        private static void processVertex(string[] vertexData, List<int> indices, List<Vector2> textures, List<Vector3> normals, Vector2[] newTextures, Vector3[] newNormals)
        {
            int currentVertexPointer = int.Parse(vertexData[0]) - 1;
            indices.Add(currentVertexPointer);
            Vector2 currentTex = textures[int.Parse(vertexData[1]) - 1];
            newTextures[currentVertexPointer] = currentTex;
            Vector3 currentNorm = normals[int.Parse(vertexData[2]) - 1];
            newNormals[currentVertexPointer] = currentNorm;
        }

        public static void AddSkybox(string[] textures, string name)
        {
            int id = GL.GenTexture();
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.TextureCubeMap, id);
            for(int i = 0;i<6;i++)
            {
                AddImage(textures[i], name + "i", false, id);
                Texture tex = images[name + "i"];
                GL.TexImage2D(TextureTarget.TextureCubeMapPositiveX + i, 0, PixelInternalFormat.Rgba, tex.width, tex.height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Rgba, PixelType.UnsignedByte, tex.buffer);

                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            }
            Skybox = id;
        }

        public static string AddImage(string path, string name, bool unique = true, int parentID = -1)
        {
            CheckFilepath("Assets/Images/" + path);
            //Read in bitmap data and process with alpha channels
            Bitmap bmp = new Bitmap("Assets/Images/" + path);
            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            Texture tex;

            if (unique)
            {
                //Create a texture object in OpenGL and bind it to read in bitmap data
                int id = GL.GenTexture();
                GL.BindTexture(TextureTarget.Texture2D, id);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

                tex = new Texture(id, bmp.Width, bmp.Height);

                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bmpData.Width, bmpData.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bmpData.Scan0);
                bmp.UnlockBits(bmpData);

            }
            else
            {
                tex = new Texture(parentID, bmp.Width, bmp.Height);
            }

            ImageConverter convert = new ImageConverter();
            tex.buffer = ((byte[])convert.ConvertTo(bmp, typeof(byte[]))).ToList();

            images.Add(name, tex);
            return name;
        }

        public static void AddTexture(Texture tex, string name)
        {
            images.Add(name, tex);
        }

        public static string AddShader(Shader shader, string name)
        {
            shaders.Add(name, shader);
            return name;
        }
        
        public static void Render()
        {
            foreach (Entity obj in gameObjects.Values)
            {
                obj.rotation.Y += 0.03f;
                obj.Render();
            }
        }

        public static void RenderWithout(Entity excludedObj)
        {
            foreach (Entity obj in gameObjects.Values)
            {
                if(obj != excludedObj)
                    obj.Render();
            }
        }

        /// <summary>
        /// Make sure that no attempts at file IO include invalid file names
        /// </summary>
        /// <param name="path">The path to the file using Debug as the base directory</param>
        /// <exception cref="FileNotFoundException">Thrown when a file path does not exist</exception>
        public static void CheckFilepath(String path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException("File not found at '" + path + "'");
        }

        /// <summary>
        /// Check before a new item is added to a collection if that item was already in that collection
        /// </summary>
        /// <param name="data">The collection to check</param>
        /// <param name="obj">The new term</param>
        /// <exception cref="DuplicateNameException">Thrown when another item is added to a collection that already has that item</exception>
        public static void CheckDuplicateName(ICollection<string> data, string obj)
        {
            if (data.Contains(obj))
                throw new DuplicateNameException("Another item in this list is already named " + obj);
        }
    }

    #region Custom exceptions to be used in the engine

    public class EngineException : Exception
    {
        public EngineException(string message)
        {
            Console.Error.WriteLine(message);
            Console.Error.WriteLine(Source);
            Console.Error.WriteLine(StackTrace);
        }
    }

    public class DuplicateNameException : EngineException { public DuplicateNameException(string message) : base(message) { } }
    public class MissingShaderUniformException : EngineException { public MissingShaderUniformException(string message) : base(message) { } }

    #endregion
}
