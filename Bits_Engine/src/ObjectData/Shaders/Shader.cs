using BitsCore.Debugging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Numerics;
using System.Text;
using static BitsCore.OpenGL.GL;

namespace BitsCore.ObjectData.Shaders
{
    /// <summary> The Shader. </summary>
    public class Shader
    {
        internal string vertexCode { get; private set; }
        internal string fragmentCode { get; private set; }

        internal string fragmentFilePath;
        internal string vertexFilePath;
        internal string shaderAssetKey;
        internal string vertAssetKey;
        internal string fragAssetKey;

        public uint ProgramID { get; private set; }

        /// <summary> Generates  </summary>
        /// <param name="_vertexFilePath"> File-Path to the 'FileName.vert' GLSL-Shader. </param>
        /// <param name="_fragmentFilePath"> File-Path to the 'FileName.frag' GLSL-Shader. </param>
        public Shader(string _vertexFilePath, string _fragmentFilePath)
        {
            vertexFilePath = _vertexFilePath;
            fragmentFilePath = _fragmentFilePath;
        }

        /// <summary> Reads the GLSL-Shader at the path. </summary>
        /// <param name="path"> File-Path to the Shader-File. </param>
        public string ReadShader(string path)
        {
            //if the file doesn't exist
            if (!File.Exists(path))
            {
                BBug.Log("!!! File under given Path doen't exist !!! Path: " + path);
                return null;
            }

            // Open the file to read from.
            string readText = @File.ReadAllText(path);
            return @readText;
        }

        /// <summary> Loads the Shader-Program to the GPU. </summary>
        public void Load()
        {
            uint vs, fs;

            //read the shaders from their given file
            //Debug.WriteLine(ReadShader(vertexFilePath));
            vertexCode = ReadShader(vertexFilePath);
            //Debug.WriteLine(ReadShader(fragmentFilePath));
            fragmentCode = ReadShader(fragmentFilePath);

            //assign local-var for the vertex shader
            vs = glCreateShader(GL_VERTEX_SHADER);
            glShaderSource(vs, vertexCode);
            glCompileShader(vs);

            //gets the status for the compiled vertex-shader and checks if there where any errors
            int[] status = glGetShaderiv(vs, GL_COMPILE_STATUS, 1);
            if (status[0] == 0)
            {
                //failed to compile
                string error = glGetShaderInfoLog(vs);
                BBug.Log("ERROR COMPILING VERTEX SHADER: \n" + error);
            }

            //assign local-var for the fragment shader
            fs = glCreateShader(GL_FRAGMENT_SHADER);
            glShaderSource(fs, fragmentCode);
            glCompileShader(fs);

            //gets the status for the compiled fragment-shader and checks if there where any errors
            status = glGetShaderiv(fs, GL_COMPILE_STATUS, 1);
            if (status[0] == 0)
            {
                //failed to compile
                string error = glGetShaderInfoLog(fs);
                BBug.Log("ERROR COMPILING FRAGMENT SHADER: \n" + error);
            }

            //assign the created shader to the program created at the 'shader' pointer location, program is what opengl calls the combination of shaders
            ProgramID = glCreateProgram();
            glAttachShader(ProgramID, vs);
            glAttachShader(ProgramID, fs);

            //link the created program
            glLinkProgram(ProgramID);

            //delete shaders, as they aren't used after being stored in the program
            glDetachShader(ProgramID, vs);
            glDetachShader(ProgramID, fs);
            glDeleteShader(vs);
            glDeleteShader(fs);
        }

        /// <summary> Sets this Shader-Program to be the active one. </summary>
        public void Use()
        {
            glUseProgram(ProgramID);
        }

        #region SET_UNIFORMS
        /// <summary> Sets a Matrix4x4 by Name. </summary>
        /// <param name="uniformName"> Name of the Variable to be set. </param>
        /// <param name="mat"> The Matrix4x4 that will be set in the shader. </param>
        public void SetMatrix4x4(string uniformName, Matrix4x4 mat)
        {
            int location = glGetUniformLocation(ProgramID, uniformName);
            glUniformMatrix4fv(location, 1, false, GetMatrix4x4Values(mat));
        }

        #region SET_VECTOR2(VECTOR2)_DOESNT_WORK
        /*
        /// <summary> Sets a Vector2 by Name. </summary>
        /// <param name="uniformName"> Name of the Variable to be set. </param>
        /// <param name="vec"> The Vector2 that will be set in the shader. </param>
        public void SetVector2(string uniformName, Vector2 vec)
        {
            int location = glGetUniformLocation(ProgramID, uniformName);
            glUniform2f(location, vec.X, vec.Y);
        }
        */
        #endregion
        /// <summary> Sets a Vector2 by Name. </summary>
        /// <param name="uniformName"> Name of the Variable to be set. </param>
        /// <param name="x"> The x-Position of the Vector2 that will be set in the shader. </param>
        /// <param name="y"> The y-Position of the Vector2 that will be set in the shader. </param>
        public void SetVector2(string uniformName, float x, float y)
        {
            int location = glGetUniformLocation(ProgramID, uniformName);
            glUniform2f(location, x, y);
        }

        /// <summary> Sets a Vector3 by Name. </summary>
        /// <param name="uniformName"> Name of the Variable to be set. </param>
        /// <param name="vec"> The Vector3 that will be set in the shader. </param>
        public void SetVector3(string uniformName, Vector3 vec)
        {
            int location = glGetUniformLocation(ProgramID, uniformName);
            glUniform3f(location, vec.X, vec.Y, vec.Z);
        }
        /// <summary> Sets a Vector3 by Name. </summary>
        /// <param name="uniformName"> Name of the Variable to be set. </param>
        /// <param name="x"> The x-Position of the Vector3 that will be set in the shader. </param>
        /// <param name="y"> The y-Position of the Vector3 that will be set in the shader. </param>
        /// <param name="z"> The z-Position of the Vector3 that will be set in the shader. </param>
        public void SetVector3(string uniformName, float x, float y, float z)
        {
            int location = glGetUniformLocation(ProgramID, uniformName);
            glUniform3f(location, x, y, z);
        }

        /// <summary> Sets a float by Name. </summary>
        /// <param name="uniformName"> Name of the Variable to be set. </param>
        /// <param name="val"> The float that will be set in the shader. </param>
        public void SetFloat(string uniformName, float val)
        {
            int location = glGetUniformLocation(ProgramID, uniformName);
            glUniform1f(location, val);
        }

        /// <summary> Sets a int by Name. </summary>
        /// <param name="uniformName"> Name of the Variable to be set. </param>
        /// <param name="val"> The int that will be set in the shader. </param>
        public void SetInt(string uniformName, int val)
        {
            int location = glGetUniformLocation(ProgramID, uniformName);
            glUniform1i(location, val);
        }
        #endregion

        private float[] GetMatrix4x4Values(Matrix4x4 m)
        {
            return new float[]
            {
                m.M11, m.M12, m.M13, m.M14,
                m.M21, m.M22, m.M23, m.M24,
                m.M31, m.M32, m.M33, m.M34,
                m.M41, m.M42, m.M43, m.M44
            };
        }


    }
}
