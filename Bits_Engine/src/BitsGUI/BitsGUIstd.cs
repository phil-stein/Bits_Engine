using BitsCore.DataManagement;
using BitsCore.ObjectData;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Text;

namespace BitsCore.BitsGUI
{
    /// <summary> The standard settings for the different item-types. </summary>
    public static class BitsGUIstd
    {
        //colors
        public static Vector3 std_col_bg = new Vector3(.35f, .35f, .35f);
        public static Vector3 std_col_header = new Vector3(0f, .1679f, .5f);
        public static Vector3 std_col_dark = new Vector3(.25f, .25f, .25f);

        //textures

        //colored-quads
        public static ColoredQuad std_bg = new ColoredQuad(new ColoredQuadSettings(std_col_bg));
        public static ColoredQuad std_header = new ColoredQuad(new ColoredQuadSettings(std_col_header));
        public static ColoredQuad std_dark = new ColoredQuad(new ColoredQuadSettings(std_col_dark));
        

        //colored-quads-methods
        public static ColoredQuad ColQuad(Vector3 color)
        {
            ColoredQuadSettings settings = new ColoredQuadSettings(color);
            return new ColoredQuad(settings);
        }

        //textured-quads-methods
        public static TexturedQuad TexQuad(Vector3 color, Texture texture)
        {
            TexturedQuadSettings settings = new TexturedQuadSettings(color, texture);
            return new TexturedQuad(settings);
        }
    }
}
