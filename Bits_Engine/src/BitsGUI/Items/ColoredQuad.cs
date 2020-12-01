using BitsCore.DataManagement;
using BitsCore.Debugging;
using BitsCore.ObjectData;
using SharpFont;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;

namespace BitsCore.BitsGUI
{
    public class ColoredQuadSettings
    {
        public Texture texture;
        public Vector3 color;
        public ColoredQuadSettings(Vector3 color)
        {
            this.color = color;
            this.texture = AssetManager.GetTexture("blank");
        }

    }

    public class ColoredQuad : Item
    {

        public ColoredQuad(ColoredQuadSettings settings) : base()
        {
            this.color = settings.color;
            this.texture = settings.texture;
        }
    }
}
