using System.Numerics;

namespace BitsCore.ObjectData.Materials
{
    [System.Serializable]
    public struct PhongMaterialSettings
    {
        public Texture diffuseTexture { get; private set; }
        public Texture specularTexture { get; private set; }
        public float shininess { get; private set; }
        public Vector2 tile { get; private set; }

        public PhongMaterialSettings(Texture _diffuseTexture, Texture _specularTexture, float tileX = 1f, float tileY = 1f, float _shininess = 1f)
        {
            this.diffuseTexture = _diffuseTexture;
            this.specularTexture = _specularTexture;
            this.shininess = _shininess;
            this.tile = new Vector2(tileX, tileY);
        }
    }

    [System.Serializable]
    public struct LightLevelSettings
    {
        public float min;
        public float max;

        public float lighness;

        public Vector3 tint;

        public LightLevelSettings(float _min, float _max, float _lighness, Vector3? _tint = null)
        {
            this.min = _min;
            this.max = _max;
            this.lighness = _lighness;
            this.tint = _tint == null ? Vector3.One : (Vector3)_tint;
        }
    }
}
