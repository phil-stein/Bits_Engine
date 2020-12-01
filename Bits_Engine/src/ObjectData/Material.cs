using BitsCore.ObjectData.Shaders;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Numerics;
using System.Text;

namespace BitsCore.ObjectData.Materials
{
    [System.Serializable]
    public abstract class Material
    {
        public Shader shader { get; private set; }
        public static Material defaultMat { get; private set; }
        public string vertShaderFilePath { get; private set; }
        public string fragShaderFilePath { get; private set; }

        /// <summary> Generates a Material. </summary>
        /// <param name="_vertShaderFilePath"> The File-Path to the Vert-Shader for the Materials Shader-Program. </param>
        /// <param name="_fragShaderFilePath"> The File-Path to the Frag-Shader for the Materials Shader-Program. </param>
        public Material(string _vertShaderFilePath, string _fragShaderFilePath)
        {
            shader = new Shader(_vertShaderFilePath, _fragShaderFilePath);
            vertShaderFilePath = _vertShaderFilePath;
            fragShaderFilePath = _fragShaderFilePath;
        }

        /// <summary> Generates a Material. </summary>
        /// <param name="_shader"> The Shader-Program used by the Material. </param>
        public Material(Shader _shader)
        {
            shader = _shader;
        }

        /// <summary> Loads the Materials Shader-Program into the GPU. </summary>
        public abstract void Load();

        /// <summary> Sets the Materials Shader to be the currently used one. </summary>
        public abstract void Use();


        /// <summary> 
        /// Gets the Material-ID of the given Material-Type.
        /// <para>One static functions to declare the ID's for each Material-Type in so that they are set in one place. </para>
        /// </summary>
        /// <typeparam name="T"> The Type of the Material. </typeparam>
        public static byte GetMatID(Type T)
        {
            if (T == typeof(BasicPhongMaterial))
            {
                return 1;
            }
            else if (T == typeof(UnlitMaterial))
            {
                return 2;
            }
            else if (T == typeof(LightMaterial))
            {
                return 3;
            }
            else
            {
                return 0;
            }
        }

        /// <summary> Gets the Material-Type of the given Material-ID. </summary>
        /// <param name="id"> The ID of the desired Material-Type. </param>
        public static Type GetMatByID(byte id)
        {
            if (id == GetMatID(typeof(BasicPhongMaterial)))
            {
                return typeof(BasicPhongMaterial);
            }
            else if (id == GetMatID(typeof(LightMaterial)))
            {
                return typeof(LightMaterial);
            }
            else
            {
                return null;
            }
        }

    }
}
