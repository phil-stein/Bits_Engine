using System.Numerics;

namespace BitsCore.ObjectData.Components
{
    [System.Serializable]
    public class SpotLight : LightSource
    {
        //position vector is the gameobjects transform.position
        //direction vector is the gameobjects transform.rotation
        internal float cutOff;
        internal float outerCutOff;

        internal float constant;
        internal float linear;
        internal float quadratic;

        public SpotLight(Vector3 _ambientColor, Vector3 _diffuseColor, Vector3 _specularColor, float _strength, float _cutOff, float _outerCutOff, float _constant, float _linear, float _quadratic) : base(_ambientColor, _diffuseColor, _specularColor, _strength)
        {
            this.cutOff = _cutOff;
            this.outerCutOff = _outerCutOff;

            this.constant = _constant;
            this.linear = _linear;
            this.quadratic = _quadratic;
        }

        internal override void OnAdd()
        {
        }

        internal override void OnRemove()
        {
        }
    }
}
