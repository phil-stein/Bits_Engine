using static BitsCore.OpenGL.GL;
using BitsCore.BitsGUI;
using BitsCore.Rendering.Display;
using System.Collections.Generic;
using System.Numerics;
using BitsCore.DataManagement;
using System.Drawing;
using BitsCore.ObjectData.Shaders;

namespace BitsCore.Rendering.Layers
{
    public struct RenderText
    {
        public string text;
        public float xPos;
        public float yPos;
        public float scale;
        public Vector3 color;
    }

    [System.Serializable]
    public class LayerUI : Layer
    {
        public List<Container> container;
        public Matrix4x4 projection;
        public Shader shader;
        internal SortedDictionary<string, RenderText> texts;

        public LayerUI() : base()
        {
            container = new List<Container>();
            texts = new SortedDictionary<string, RenderText>();
            projection = Matrix4x4.Identity;
        }

        public void AddContainer(Container cont)
        {
            container.Add(cont);
        }
        public void AddText(RenderText text, string id)
        {
            texts.Add(id, text);
        }
        public void AddText(string id, string text, float xPos, float yPos, float scale, Vector3 color)
        {
            RenderText renderText = new RenderText();
            renderText.text = text;
            renderText.xPos = xPos;
            renderText.yPos = yPos;
            renderText.scale = scale;
            renderText.color = color;
            texts.Add(id, renderText);
        }
        public void SetText(string id, string newText)
        {
            if (!texts.ContainsKey(id)) { throw new System.Exception("No Text with the given ID exists."); }
            RenderText renderText = texts[id];
            renderText.text = newText;
            texts[id] = renderText;
        }
        public string GetText(string id)
        {
            if (!texts.ContainsKey(id)) { throw new System.Exception("No Text with the given ID exists."); }
            return texts[id].text;
        }

        public void Init()
        {
            AssetManager.AddShader("BitsGUIShader", "BitsGUI.vert", "BitsGUI.frag");
            shader = AssetManager.GetShader("BitsGUIShader");
            shader.Load();
            UpdateProjectionMat();
        }
        private void UpdateProjectionMat()
        {
            //projection = Matrix4x4.CreateOrthographic(DisplayManager.WindowSize.X, DisplayManager.WindowSize.Y, -1f, 1f);

            float left = 0;
            float right = DisplayManager.WindowSize.X;
            float top = 0;
            float bottom = DisplayManager.WindowSize.Y;

            //last to args are the near-field-camera-clipping and the distance-culling for our camera (doen't matter because it's 2D)
            projection = Matrix4x4.CreateOrthographicOffCenter(left, right, bottom, top, -1f, 100f);
        }

    }
}
