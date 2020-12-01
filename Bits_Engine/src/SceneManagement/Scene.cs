using BitsCore.ObjectData;
using BitsCore.ObjectData.Components;
using BitsCore.Rendering.Layers;
using System.Collections.Generic;

namespace BitsCore.SceneManagement
{
    [System.Serializable]
    public class Scene
    {
        public Layer[] layers;
        public LightSource[] lightSources;
        public DirectionalLight[] dirLights;
        public PointLight[] pointLights;
        public SpotLight[] spotLights;

        public Scene(Layer[] _layers)
        {
            this.layers = _layers;

            List<LightSource> lightSourcesLst = new List<LightSource>();
            List<DirectionalLight> dirLightsLst = new List<DirectionalLight>();
            List<PointLight> pointLightsLst = new List<PointLight>();
            List<SpotLight> spotLightsLst = new List<SpotLight>();
            foreach(Layer layer in layers)
            {
                if(layer.GetType() != typeof(Layer3D)) { continue; }

                Layer3D l = (Layer3D)layer;
                foreach (GameObject go in l.gameObjects)
                {
                    if (go.HasComp<DirectionalLight>())
                    {
                        lightSourcesLst.Add(go.GetComp<DirectionalLight>());
                        dirLightsLst.Add(go.GetComp<DirectionalLight>());
                    }
                    if (go.HasComp<PointLight>())
                    {
                        lightSourcesLst.Add(go.GetComp<PointLight>());
                        pointLightsLst.Add(go.GetComp<PointLight>());
                    }
                    if (go.HasComp<SpotLight>())
                    {
                        lightSourcesLst.Add(go.GetComp<SpotLight>());
                        spotLightsLst.Add(go.GetComp<SpotLight>());
                    }
                }
            }
            lightSources = lightSourcesLst.ToArray();
            dirLights = dirLightsLst.ToArray();
            pointLights = pointLightsLst.ToArray();
            spotLights = spotLightsLst.ToArray();
        }
    }
}
