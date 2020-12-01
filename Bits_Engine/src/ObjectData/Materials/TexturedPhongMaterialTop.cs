using BitsCore.ObjectData.Shaders;
using BitsCore.Rendering;
using System.Numerics;
using static BitsCore.OpenGL.GL;

namespace BitsCore.ObjectData.Materials
{
    [System.Serializable]
    public class TexturedPhongMaterialTop : Material
    {
        PhongMaterialSettings materialOne;
        PhongMaterialSettings materialTwo;

        /// <summary> Generates a Material with Phong-Lighting. </summary>
        /// <param name="_vertShaderFilePath"> The File-Path to the Vert-Shader for the Materials Shader-Program. </param>
        /// <param name="_fragShaderFilePath"> The File-Path to the Frag-Shader for the Materials Shader-Program. </param>

        public TexturedPhongMaterialTop(string _vertShaderFilePath, string _fragShaderFilePath, PhongMaterialSettings _materialOne, PhongMaterialSettings _materialTwo) : base(_vertShaderFilePath, _fragShaderFilePath)
        {
            this.materialOne = _materialOne;
            this.materialTwo = _materialTwo;

            Load();
        }

        /// <summary> Generates a Material with Phong-Lighting. </summary>
        /// <param name="_shader"> The Shader-Program used by the Material. </param>
        /// <param name="_diffuseTexture"> The Diffuse-Color of the Material. </param>
        /// <param name="_specularTexture"> The Specular-Color of the Material. </param>
        /// <param name="_shininess"> The Shininess of the Material. </param>
        public TexturedPhongMaterialTop(Shader _shader, PhongMaterialSettings _materialOne, PhongMaterialSettings _materialTwo) : base(_shader)
        {
            this.materialOne = _materialOne;
            this.materialTwo = _materialTwo;

            Load();
        }

        public override void Load()
        {
            shader.Load();
        }

        public override void Use()
        {
            shader.Use();

            materialOne.diffuseTexture.Use(0);
            shader.SetInt("materialOne.diffuse", 0);
            materialOne.specularTexture.Use(1);
            shader.SetInt("materialOne.specular", 1);
            shader.SetFloat("materialOne.shininess", materialOne.shininess);
            shader.SetVector2("materialOne.tile", materialOne.tile.X, materialOne.tile.Y);

            materialTwo.diffuseTexture.Use(2);
            shader.SetInt("materialTwo.diffuse", 2);
            materialTwo.specularTexture.Use(3);
            shader.SetInt("materialTwo.specular", 3);
            shader.SetFloat("materialTwo.shininess", materialTwo.shininess);
            shader.SetVector2("materialTwo.tile", materialTwo.tile.X, materialTwo.tile.Y);
        }

    }
}
