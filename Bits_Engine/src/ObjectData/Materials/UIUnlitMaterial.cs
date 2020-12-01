using BitsCore.ObjectData.Materials;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using BitsCore.Rendering;
using BitsCore.ObjectData.Shaders;

namespace BitsCore.ObjectData.Materials
{
    [System.Serializable]
    class UIUnlitMaterial : Material
    {
        public Vector3 objColor;

        /// <summary> Generates a UI-Material without lighting. </summary>
        /// <param name="_vertShaderFilePath"> The File-Path to the Vert-Shader for the Materials Shader-Program. </param>
        /// <param name="_fragShaderFilePath"> The File-Path to the Frag-Shader for the Materials Shader-Program. </param>
        /// <param name="_objColor"> The Object-Color of the Material. </param>
        public UIUnlitMaterial(string _vertShaderFilePath, string _fragShaderFilePath, Vector3 _objColor) : base(_vertShaderFilePath, _fragShaderFilePath)
        {
            this.objColor = _objColor;

            Load();
        }
        public UIUnlitMaterial(Shader _shader, Vector3 _objColor) : base(_shader)
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
