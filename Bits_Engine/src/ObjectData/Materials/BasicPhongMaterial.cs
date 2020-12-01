using BitsCore.Rendering;
using BitsCore.ObjectData.Materials;
using BitsCore.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Text;
using BitsCore.Rendering.Layers;
using BitsCore.ObjectData.Shaders;

namespace BitsCore.ObjectData.Materials
{
    [System.Serializable]
    public class BasicPhongMaterial : Material
    {
        public Vector3 ambient;
        public Vector3 diffuse;
        public Vector3 specular;
        public float shininess;

        /// <summary> Generates a Material with Phong-Lighting. </summary>
        /// <param name="_vertShaderFilePath"> The File-Path to the Vert-Shader for the Materials Shader-Program. </param>
        /// <param name="_fragShaderFilePath"> The File-Path to the Frag-Shader for the Materials Shader-Program. </param>
        /// <param name="_ambient"> The Ambient-Color of the Material. </param>
        /// <param name="_diffuse"> The Diffuse-Color of the Material. </param>
        /// <param name="_specular"> The Specular-Color of the Material. </param>
        /// <param name="_shininess"> The Shininess of the Material. </param>
        public BasicPhongMaterial(string _vertShaderFilePath, string _fragShaderFilePath, Vector3 _ambient, Vector3 _diffuse, Vector3 _specular, float _shininess) : base(_vertShaderFilePath, _fragShaderFilePath)
        {
            this.ambient = _ambient;
            this.diffuse = _diffuse;
            this.specular = _specular;
            this.shininess = _shininess;

            Load();
        }

        /// <summary> Generates a Material with Phong-Lighting. </summary>
        /// <param name="_shader"> The Shader-Program used by the Material. </param>
        /// <param name="_ambient"> The Ambient-Color of the Material. </param>
        /// <param name="_diffuse"> The Diffuse-Color of the Material. </param>
        /// <param name="_specular"> The Specular-Color of the Material. </param>
        /// <param name="_shininess"> The Shininess of the Material. </param>
        public BasicPhongMaterial(Shader _shader, Vector3 _ambient, Vector3 _diffuse, Vector3 _specular, float _shininess) : base(_shader)
        {
            this.ambient = _ambient;
            this.diffuse = _diffuse;
            this.specular = _specular;
            this.shininess = _shininess;

            Load();
        }

        public override void Load()
        {
            shader.Load();
        }

        public override void Use()
        {
            shader.Use();
            shader.SetVector3("material.ambient", ambient);
            shader.SetVector3("material.diffuse", diffuse);
            shader.SetVector3("material.specular", specular);
            shader.SetFloat("material.shininess", shininess);

            shader.SetVector3("light.ambient", 0.2f, 0.2f, 0.2f);
            shader.SetVector3("light.diffuse", 0.5f, 0.5f, 0.5f);
            shader.SetVector3("light.specular", 1.0f, 1.0f, 1.0f);
            shader.SetVector3("light.position", Renderer.lightSources.Length > 0 ? Renderer.lightSources[0].gameObject.transform.position : Vector3.Zero);

            shader.SetVector3("viewPos", Renderer.mainCam.transform.position);
        }

    }
}
