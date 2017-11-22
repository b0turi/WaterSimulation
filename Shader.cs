using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.IO;

namespace WaterSimulation
{
    public enum UniformDataType
    {
        Boolean = 0,
        Float = 1,
        Vector2 = 2,
        Vector3 = 3,
        Vector4 = 4,
        Matrix4 = 5
    }

    public abstract class Shader
    {
        public int ProgramID;
        protected int VShaderID;
        protected int FShaderID;

        protected int AttributeCount;
        protected int UniformCount;
        public Entity currentObject;


        protected Dictionary<string, int> Attributes { get; set; }
        protected Dictionary<string, Uniform> Uniforms { get; set; }

        private Dictionary<string, bool> UniformsFilled { get; set; }

        public Shader(string vertex, string fragment)
        {
            Attributes = new Dictionary<string, int>();
            Uniforms = new Dictionary<string, Uniform>();
            UniformsFilled = new Dictionary<string, bool>();

            ProgramID = GL.CreateProgram();

            LoadShaderFromFile(vertex, ShaderType.VertexShader);
            LoadShaderFromFile(fragment, ShaderType.FragmentShader);

            GL.LinkProgram(ProgramID);
            GL.ValidateProgram(ProgramID);

            FillShader();

            BindAttributes();
        }

        public void LoadShaderFromFile(String filename, ShaderType type)
        {
            string fullFilename = "Assets/Shaders/" + filename;

            EngineCore.CheckFilepath(fullFilename);
            string shaderBody = File.ReadAllText(fullFilename);
            if (type == ShaderType.VertexShader)
                VShaderID = LoadShader(shaderBody, type);
            else if (type == ShaderType.FragmentShader)
                FShaderID = LoadShader(shaderBody, type);
        }

        private int LoadShader(String code, ShaderType type)
        {
            int address = GL.CreateShader(type);
            int status;

            GL.ShaderSource(address, code);
            GL.CompileShader(address);

            //See if the shader compiled correctly and throw an error if not
            GL.GetShader(address, ShaderParameter.CompileStatus, out status);
            if (status == 0)
                throw new GraphicsException(String.Format("Error compiling {0} shader: {1}", type.ToString(), GL.GetShaderInfoLog(address)));

            GL.AttachShader(ProgramID, address);
            return address;
        }

        /// <summary>
        /// Add the necessary attributes and uniforms for the shader to function to their respective dictionaries
        /// </summary>
        public abstract void FillShader();


        /// <summary>
        /// Load Uniforms and perform other operations that are necessary every frame
        /// </summary>
        public abstract void Update();

        public void AddAttribute(string name, int location)
        {
            EngineCore.CheckDuplicateName(Attributes.Keys, name);
            Attributes.Add(name, location);
        }

        public void AddUniform(string name, int location, UniformDataType type)
        {
            EngineCore.CheckDuplicateName(Uniforms.Keys, name);
            Uniforms.Add(name, new Uniform(location, type));
            UniformsFilled.Add(name, false);
        }

        public int GetUniform(string name)
        {
            return GL.GetUniformLocation(ProgramID, name);
        }

        public void LoadUniform(string name, bool value)
        {
            float tempValue = 0;
            if (value)
                tempValue = 1;
            if (Uniforms[name].type == UniformDataType.Boolean)
            {
                GL.Uniform1(Uniforms[name].location, tempValue);
                UniformsFilled[name] = true;
            }
            else
                throw new ArgumentException("The given Uniform is not a Boolean, it is a " + Uniforms[name].type);
        }

        public void LoadUniform(string name, float value)
        {
            if (Uniforms[name].type == UniformDataType.Float)
            {
                GL.Uniform1(Uniforms[name].location, value);
                UniformsFilled[name] = true;
            }
            else
                throw new ArgumentException("The given Uniform is not a Float, it is a " + Uniforms[name].type);
        }

        public void LoadUniform(string name, Vector2 value)
        {
            if (Uniforms[name].type == UniformDataType.Vector2)
            {
                GL.Uniform2(Uniforms[name].location, value);
                UniformsFilled[name] = true;
            }
            else
                throw new ArgumentException("The given Uniform is not a Vector2, it is a " + Uniforms[name].type);
        }

        public void LoadUniform(string name, Vector3 value)
        {
            if (Uniforms[name].type == UniformDataType.Vector3)
            {
                GL.Uniform3(Uniforms[name].location, value);
                UniformsFilled[name] = true;
            }
            else
                throw new ArgumentException("The given Uniform is not a Vector3, it is a " + Uniforms[name].type);
        }

        public void LoadUniform(string name, Vector4 value)
        {
            if (Uniforms[name].type == UniformDataType.Vector4)
            {
                GL.Uniform4(Uniforms[name].location, value);
                UniformsFilled[name] = true;
            }
            else
                throw new ArgumentException("The given Uniform is not a Vector4, it is a " + Uniforms[name].type);
        }

        public void LoadUniform(string name, Matrix4 value)
        {
            if (Uniforms[name].type == UniformDataType.Matrix4)
            {
                GL.UniformMatrix4(Uniforms[name].location, false, ref value);
                UniformsFilled[name] = true;
            }
            else
                throw new ArgumentException("The given Uniform is not a Matrix4, it is a " + Uniforms[name].type);
        }

        public virtual void Start()
        {
            Update();

            if (UniformsFilled.Values.Contains(false))
                throw new MissingShaderUniformException("There are uniforms that have not been added to the shader and will cause errors");

            foreach (KeyValuePair<string, int> attrib in Attributes)
                GL.EnableVertexAttribArray(attrib.Value);
        }

        public virtual void Stop()
        {

            foreach (KeyValuePair<string, int> attrib in Attributes)
                GL.DisableVertexAttribArray(attrib.Value);
        }

        public void BindAttributes()
        {
            foreach (KeyValuePair<string, int> attrib in Attributes)
                GL.BindAttribLocation(ProgramID, attrib.Value, attrib.Key);
        }
    }

    public struct Uniform
    {
        public int location { get; }
        public UniformDataType type { get; }

        public Uniform(int location, UniformDataType type)
        {
            this.location = location;
            this.type = type;
        }
    }
}
