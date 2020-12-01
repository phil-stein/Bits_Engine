using System;
using System.Numerics;

namespace BitsCore.ObjectData.Components
{
    [System.Serializable]
    public class PointLight : LightSource
    {
        //position vector is the gameobjects transform.position

        internal float constant;
        internal float linear;
        internal float quadratic;

        public PointLight(Vector3 _ambientColor, Vector3 _diffuseColor, Vector3 _specularColor, float _strength, float _constant, float _linear, float _quadratic) : base(_ambientColor, _diffuseColor, _specularColor, _strength)
        {
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
