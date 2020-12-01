using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace BitsCore.ObjectData.Components
{
    [System.Serializable]
    public class DirectionalLight : LightSource
    {
        //direction vector is the gameobjects transform.rotation
        public DirectionalLight(Vector3 _ambientColor, Vector3 _diffuseColor, Vector3 _specularColor, float _strength) : base(_ambientColor, _diffuseColor, _specularColor, _strength)
        {
        }

        internal override void OnAdd()
        {
        }

        internal override void OnRemove()
        {
        }
    }
}
