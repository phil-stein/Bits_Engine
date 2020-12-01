using BitsCore.ObjectData;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace BitsCore.BitsGUI
{
    public class TexturedQuadSettings
    {
        public Vector3 color;
        public Texture texture;
        public TexturedQuadSettings(Vector3 color, Texture texture)
        {
            this.color = color;
            this.texture = texture;
        }
    }
    public class TexturedQuad : Item
    {

        public TexturedQuad(TexturedQuadSettings settings) : base()
        {
            this.color = settings.color;
            this.texture = settings.texture;
        }
    }
}
