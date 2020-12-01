using static BitsCore.OpenGL.GL;
using System;
using System.Collections.Generic;
using System.Numerics;
using SharpFont;
using BitsCore.Debugging;
using BitsCore.Rendering.Layers;
using BitsCore.DataManagement;
using BitsCore.ObjectData.Shaders;

namespace BitsCore.Rendering
{
    public static class TextRenderer
    {
        public struct Character
        {
            public uint textureID { get; set; }
            public Vector2 size { get; set; }
            public Vector2 bearing { get; set; }
            public int Advance { get; set; }

            public override string ToString()
            {
                return  "TexID: " + textureID + ", Size: " + size + ", Bearing: " + bearing + ", Advance: " + Advance;
            }
        }

        private static SortedDictionary<uint, Character> characters = new SortedDictionary<uint, Character>();
        /// <summary> 
        /// The Vertex Array Object of the Mesh 
        /// <para> Used to pass the vertex data the VBO. </para>
        /// </summary>
        private static uint quadVAO; //unsigned-int storing the pointer to the Vertex-Array-Object
        /// <summary> 
        /// The Vertex Buffer Object of the Mesh 
        /// <para> Used to store the vertex-data. </para>
        /// </summary>
        private static uint quadVBO; //unsigned-int storing the pointer to the Vertex-Buffer-Object
        private static Shader shader;

        public static void Init(uint fontRes = 48)
        {
            //tutorial: https://stackoverflow.com/questions/59800470/c-sharp-opentk-draw-string-on-window, LearnOpenGL page 450 (Chapter: Text Rendering)

            // initialize library
            Library lib = new Library();
            Face face = new Face(lib, DataManager.assetsPath + @"\Fonts\arial.ttf"); //arial //agency_r
            face.SetPixelSizes(0, fontRes); //setting width, height for chars in pixel (width == 0 means FreeFont calculates the width automatically, to not have monospaced text)
            
            glPixelStorei(GL_UNPACK_ALIGNMENT, 1); //set 1 byte pixel alignment (because each pixel is only represented by a single 8-bit number not 3)

            // Load first 128 characters of ASCII set
            for (uint c = 0; c < 128; c++)
            {
                try
                {
                    //load glyph
                    //face.LoadGlyph(c, LoadFlags.Render, LoadTarget.Normal);
                    face.LoadChar(c, LoadFlags.Render, LoadTarget.Normal);
                    GlyphSlot glyph = face.Glyph;
                    FTBitmap bitmap = glyph.Bitmap;

                    //create glyph texture
                    uint texObj = glGenTexture();
                    glBindTexture(GL_TEXTURE_2D, texObj);
                    glTexImage2D(
                        GL_TEXTURE_2D,
                        0,
                        GL_RED,
                        bitmap.Width,
                        bitmap.Rows,
                        0,
                        GL_RED,
                        GL_UNSIGNED_BYTE,
                        bitmap.Buffer
                        );

                    //set texture parameters
                    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_CLAMP_TO_EDGE);
                    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_CLAMP_TO_EDGE);
                    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR); //GL_CLAMP_TO_EDGE
                    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR); //GL_CLAMP_TO_EDGE

                    //add character
                    Character ch = new Character();
                    ch.textureID = texObj;
                    ch.size = new Vector2(bitmap.Width, bitmap.Rows);
                    ch.bearing = new Vector2(glyph.BitmapLeft, glyph.BitmapTop);
                    ch.Advance = glyph.Advance.X;
                    characters.Add(c, ch);
                }
                catch (Exception ex)
                {
                    BBug.Log(ex);
                    throw ex;
                }
            }

            //BBug.Log("\nCharacters: ");
            //foreach(KeyValuePair<uint, Character> valuePair in characters)
            //{
            //    BBug.Log("ASCII: " + (char)valuePair.Key + ", " + valuePair);
            //}
            //BBug.Log("\n");

            Gen2DQuad();

            shader = new Shader(DataManager.assetsPath + @"\Shaders\Vertex_Shaders\BitsGUIText.vert", DataManager.assetsPath + @"\Shaders\Fragment_Shaders\BitsGUIText.frag");
            shader.Load();
        }
        /// <summary> Generates the opengl-data(vao, vbo) for the quad used to render all 2d objects. </summary>
        private static void Gen2DQuad()
        {
            //Generates the opengl-data(vao, vbo)
            unsafe
            {

                //create VAO and VBO
                quadVAO = glGenVertexArray();
                quadVBO = glGenBuffer();

                glBindVertexArray(quadVAO);

                glBindBuffer(GL_ARRAY_BUFFER, quadVBO);

                glBufferData(GL_ARRAY_BUFFER, sizeof(float) * (6 * 4), NULL, GL_DYNAMIC_DRAW); //doesn't properly point to any data because we are using glBindBufferSubData()

                //tell vao how to interpret the data in vbo
                //arg01: vbo-index - for using mutiple vbo/ pointing to the same one multiple times
                //arg02: size-per-vertex - how many vars per vertex
                //arg03: type of data - e.g. int/float/etc.
                //arg04: normalize - normalizes the data (changes it)
                //arg05: amount of bytes inbetween each pair of relevant data - 5 * float-size, because there are 5 floats for each set of data out of which we on use the first two as specified in arg02
                //arg06: offset between start of the data-set as a whole and the first set of relevant data
                //arg06&(void*): pointer of type void, (void*)0 is a pointer to "first position"
                glVertexAttribPointer(0, 4, GL_FLOAT, false, 4 * sizeof(float), (void*)0);
                glEnableVertexAttribArray(0);

                glBindBuffer(GL_ARRAY_BUFFER, 0);
                glBindVertexArray(0);
            }
        }

        public static uint GetCharTex(uint charID)
        {
            return characters[charID].textureID;
        }
        public static void RenderText(LayerUI layer, string text, float x, float y, float scale, Vector3 color = default)
        {
            //tutorial: https://stackoverflow.com/questions/59800470/c-sharp-opentk-draw-string-on-window, LearnOpenGL page 450 (Chapter: Text Rendering)

            glEnable(GL_BLEND);
            glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);

            //activate the shader
            shader.Use();
            shader.SetMatrix4x4("model", Matrix4x4.Identity);
            shader.SetMatrix4x4("projection", layer.projection);
            shader.SetVector3("color", color);

            glActiveTexture(GL_TEXTURE0);
            glBindVertexArray(quadVAO);

            //iterate through all characters
            foreach (char c in text)
            {
                //BBug.Log("char: " + c + ", uint: " + (uint)c);

                if (!characters.ContainsKey(c))
                {
                    continue;
                }

                Character ch = characters[c];

                float xPos = x + ch.bearing.X * scale;
                float yPos = y + (ch.size.Y - ch.bearing.Y) * scale;

                float width = ch.size.X * scale;
                float height = ch.size.Y * -scale;
                float[] verts = new float[]
                {
                    xPos,          yPos + height,  0.0f, 0.0f,
                    xPos,          yPos,           0.0f, 1.0f,
                    xPos + width,  yPos,           1.0f, 1.0f,

                    xPos,          yPos + height,  0.0f, 0.0f,
                    xPos + width,  yPos,           1.0f, 1.0f,
                    xPos + width,  yPos + height,  1.0f, 0.0f,
                };

                //render glyph texture over quad
                glBindTexture(GL_TEXTURE_2D, ch.textureID);

                //update content of vbo memory
                glBindBuffer(GL_ARRAY_BUFFER, quadVBO); //bind buffer, sets it 'active'
                unsafe
                {
                    fixed (float* ptr = &verts[0])
                    {
                        glBufferSubData(GL_ARRAY_BUFFER, 0, sizeof(float) * verts.Length, ptr); //gives opengl a pointer to our new verts-data and its length in memory
                    }
                }
                glBindBuffer(GL_ARRAY_BUFFER, 0); //unbind buffer

                glDrawArrays(GL_TRIANGLES, 0, 6); //render quad

                x += (ch.Advance >> 6) * scale; //bitshift by 6 to get value in pixels (2^6 = 64 (divide amount of 1/64th pixels by 64 to get amount of pixels))
            }

            glBindVertexArray(0); //unbind vao
            glBindTexture(GL_TEXTURE_2D, 0); //unbind texture
            glDisable(GL_BLEND); //disable blending

        }
    }
}
