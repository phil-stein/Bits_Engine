using BitsCore.ObjectData.Materials;
using BitsCore.ObjectData.Shaders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BitsCore.ObjectData.Materials
{
    [System.Serializable]
    class LightMaterial : Material
    {

        /// <summary> Generates a Material without Lighting, therefore looks bright like a Light. </summary>
        /// <param name="_vertShaderFilePath"> The File-Path to the Vert-Shader for the Materials Shader-Program. </param>
        /// <param name="_fragShaderFilePath"> The File-Path to the Frag-Shader for the Materials Shader-Program. </param>
        public LightMaterial(string _vertShaderFilePath, string _fragShaderFilePath) : base(_vertShaderFilePath, _fragShaderFilePath)
        {
            Load();
        }
        public LightMaterial(Shader _shader) : base(_shader)
        {
            Load();
        }

        /// <summary> Loads the Materials Shader. </summary>
        public override void Load()
        {
            shader.Load();
        }

        /// <summary> Sets the Materials Shader to be the currently used. </summary>
        public override void Use()
        {
            shader.Use();
            shader.SetVector3("lightColor", 1.0f, 1.0f, 1.0f);
        }
    }
}
