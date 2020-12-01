using BitsCore.ObjectData.Shaders;
using BitsCore.Rendering;
using System.Numerics;
using static BitsCore.OpenGL.GL;

namespace BitsCore.ObjectData.Materials
{
    [System.Serializable]
    public class TexturedPhongMaterial : Material
    {
        public Texture diffuseTexture;
        public Texture specularTexture;
        public float shininess;
        public Vector2 tile;

        /// <summary> Generates a Material with Phong-Lighting. </summary>
        /// <param name="_vertShaderFilePath"> The File-Path to the Vert-Shader for the Materials Shader-Program. </param>
        /// <param name="_fragShaderFilePath"> The File-Path to the Frag-Shader for the Materials Shader-Program. </param>
        /// <param name="diffuseTexture"> The Diffuse-Color of the Material. </param>
        /// <param name="_specularTexture"> The Specular-Color of the Material. </param>
        /// <param name="_shininess"> The Shininess of the Material. </param>
        public TexturedPhongMaterial(string _vertShaderFilePath, string _fragShaderFilePath, Texture diffuseTexture, Texture _specularTexture, float _shininess, float tileX = 1f, float tileY = 1f) : base(_vertShaderFilePath, _fragShaderFilePath)
        {
            this.diffuseTexture = diffuseTexture;
            this.specularTexture = _specularTexture;
            this.shininess = _shininess;
            this.tile = new Vector2(tileX, tileY);

            Load();
        }

        /// <summary> Generates a Material with Phong-Lighting. </summary>
        /// <param name="_shader"> The Shader-Program used by the Material. </param>
        /// <param name="_diffuseTexture"> The Diffuse-Color of the Material. </param>
        /// <param name="_specularTexture"> The Specular-Color of the Material. </param>
        /// <param name="_shininess"> The Shininess of the Material. </param>
        public TexturedPhongMaterial(Shader _shader, Texture _diffuseTexture, Texture _specularTexture, float _shininess, float tileX = 1f, float tileY = 1f) : base(_shader)
        {
            this.diffuseTexture = _diffuseTexture;
            this.specularTexture = _specularTexture;
            this.shininess = _shininess;
            this.tile = new Vector2(tileX, tileY);
            
            Load();
        }

        public override void Load()
        {
            shader.Load();
        }

        public override void Use()
        {
            shader.Use();
            diffuseTexture.Use(0);
            shader.SetInt("material.diffuse", 0);
            specularTexture.Use(1);
            shader.SetInt("material.specular", 1);
            shader.SetFloat("material.shininess", shininess);
            shader.SetVector2("material.tile", tile.X, tile.Y);
        }

    }
}
