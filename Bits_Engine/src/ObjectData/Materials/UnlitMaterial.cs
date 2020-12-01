﻿using BitsCore.ObjectData.Shaders;
using BitsCore.Rendering;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace BitsCore.ObjectData.Materials
{
    [System.Serializable]
    public class UnlitMaterial : Material
    {
        public Vector3 objColor;

        /// <summary> Generates a Material with Phong-Lighting. </summary>
        /// <param name="_vertShaderFilePath"> The File-Path to the Vert-Shader for the Materials Shader-Program. </param>
        /// <param name="_fragShaderFilePath"> The File-Path to the Frag-Shader for the Materials Shader-Program. </param>
        /// <param name="_objColor"> The Object-Color of the Material. </param>
        public UnlitMaterial(string _vertShaderFilePath, string _fragShaderFilePath, Vector3 _objColor) : base(_vertShaderFilePath, _fragShaderFilePath)
        {
            this.objColor = _objColor;
            
            Load();
        }
        public UnlitMaterial(Shader _shader, Vector3 _objColor) : base(_shader)
        {
            this.objColor = _objColor;
            
            Load();
        }

        public override void Load()
        {
            shader.Load();
        }

        public override void Use()
        {
            shader.Use();
            shader.SetVector3("objectColor", objColor);

            shader.SetVector3("viewPos", Renderer.mainCam.transform.position);
        }
    }
}
