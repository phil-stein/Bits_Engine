using BitsCore.DataManagement;
using BitsCore.DataManagement.Serialization;
using BitsCore.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitsCore.SceneManagement
{
    public static class SceneManager
    {
        private static string entrySceneKey;

        /// <summary> The <see cref="Scene"/> that gets loaded on start-up. </summary>
        internal static Scene entryScene;
        public static void Init(Scene _entryScene) //should be internal
        {
            //entryScene = Serializer.DeserializeScene(AssetManager.GetScenePath(entrySceneKey));

            Renderer.LayerStack = _entryScene.layers.ToList();
            Renderer.lightSources = _entryScene.lightSources;
            Renderer.dirLights = _entryScene.dirLights;
            Renderer.pointLights = _entryScene.pointLights;
            Renderer.spotLights = _entryScene.spotLights;
        }

        public static Scene LoadScene(string sceneName)
        {
            string path = AssetManager.GetScenePath(sceneName);
            return Serializer.DeserializeScene(path);
        }
    }
}
