using BitsCore.ObjectData;
using System.Collections.Generic;

namespace BitsCore.Rendering.Layers
{
    [System.Serializable]
    public class Layer3D : Layer
    {
        public List<GameObject> gameObjects;

        public Layer3D() : base()
        {
            gameObjects = new List<GameObject>();
        }

        public Layer3D(List<GameObject> _gameObjects) : base()
        {
            gameObjects = _gameObjects;
        }

        public void SetRenderObjects(List<GameObject> objs)
        {
            gameObjects = objs;
        }
    }
}
