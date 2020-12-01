using BitsCore.ObjectData;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace BitsCore.ObjectData.Components
{
    /// <summary> LightSource-Component containing the Type and Colors. </summary>
    [System.Serializable]
    public abstract class LightSource : Component
    {
        public Vector3 ambientColor { get; private set; }
        public Vector3 diffuseColor { get; private set; }
        public Vector3 specularColor { get; private set; }
        public float strength { get; private set; }

        public LightSource(Vector3 _ambientColor, Vector3 _diffuseColor, Vector3 _specularColor, float _strength)
        {
            this.ambientColor = _ambientColor;
            this.diffuseColor = _diffuseColor;
            this.specularColor = _specularColor;
            this.strength = _strength;
        }

        /// <summary> Returns the LightSources ambient-color multiplied with it's strength. </summary>
        public Vector3 GetAmbient()
        {
            return ambientColor * strength;
        }
        /// <summary> Returns the LightSources diffuse-color multiplied with it's strength. </summary>
        public Vector3 GetDiffuse()
        {
            return diffuseColor * strength;
        }
        /// <summary> Returns the LightSources specular-color multiplied with it's strength. </summary>
        public Vector3 GetSpecular()
        {
            return specularColor * strength;
        }
    }
}
