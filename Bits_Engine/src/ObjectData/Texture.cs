using BitsCore.Debugging;
using BitsCore.OpenGL;
using DevILSharp;
using Microsoft.FSharp.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Numerics;
using System.Text;
using static BitsCore.OpenGL.GL;

namespace BitsCore.ObjectData
{
    public enum PixelFormat { RGB, RGBA, BGR, SingleChannel }

    public class Texture : IDisposable
    {
        public PixelFormat pixelFormat { get; private set; }
        public byte[] data { get; private set; }
        public int width { get; private set; }
        public int height { get; private set; }
        public int size { get; private set; }
        public string filePath { get; private set; }

        uint texID;
        public uint textureID { get; private set; }

        public Texture(PixelFormat _pixelFormat, byte[] _data, int _width, int _height, string _filePath, int _size = 0)
        {
            this.pixelFormat = _pixelFormat;
            this.data = _data;
            this.width = _width;
            this.height = _height;
            this.size = _size;
            this.filePath = _filePath;

            if(this.size <= 0)
            {
                //the image has 3/4 bytes per pixel, depending on the amount of channels
                this.size = width * height * (this.pixelFormat == PixelFormat.RGBA ? 4 : 3);
            }

            Load();
        }

        public void Load()
        {
            unsafe
            {
                fixed(uint* ptr = &texID)
                {
                    glGenTextures(1, ptr);
                }

                // "Bind" the newly created texture : all future texture functions will modify this texture
                glBindTexture(GL_TEXTURE_2D, texID);

                // Give the image to OpenGL
                fixed (byte* ptr = &data[0])
                {
                    //GL_BRG -> the pixel data is stored blue, red, green not red, green, blue
                    int pixFormat = pixelFormat == PixelFormat.RGB ? GL_RGB : (pixelFormat == PixelFormat.RGBA ? GL_RGBA : (pixelFormat == PixelFormat.BGR ? GL_BGR : GL_RGB));
                    glTexImage2D(GL_TEXTURE_2D, 0, pixFormat, width, height, 0, pixFormat, GL_UNSIGNED_BYTE, ptr);
                }

                //glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_NEAREST);
                //glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_NEAREST);
                //set how the images should be wrapped, if uv(st in opengl) coordinates are outside of the 0-1 range
                //glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_MIRRORED_REPEAT);
                //glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_MIRRORED_REPEAT);
                //set how the texture should be minimized/magnified, linear blurs image on upscale and nearest makes it pixelated
                //glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
                //glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);

                // set the texture wrapping parameters
                glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_REPEAT);   // set texture wrapping to GL_REPEAT (default wrapping method)
                glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_REPEAT);
                // set texture filtering parameters
                glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
                glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);
            }

            textureID = texID;
        }
        public void Use(int glID = 0)
        {

            // "Bind" the newly created texture : all future texture functions will modify this texture
            int GL_Tex = glID == 0 ? GL_TEXTURE0 :
                            glID == 1 ? GL_TEXTURE1 :
                            glID == 2 ? GL_TEXTURE2 :
                            glID == 3 ? GL_TEXTURE3 :
                            glID == 4 ? GL_TEXTURE4 :
                            glID == 5 ? GL_TEXTURE5 :
                            glID == 6 ? GL_TEXTURE6 :
                            glID == 7 ? GL_TEXTURE7 :
                            glID == 8 ? GL_TEXTURE8 :
                            glID == 9 ? GL_TEXTURE9 :
                            glID == 10 ? GL_TEXTURE10 :
                            glID == 11 ? GL_TEXTURE11 :
                            glID == 12 ? GL_TEXTURE12 :
                            glID == 13 ? GL_TEXTURE13 :
                            glID == 14 ? GL_TEXTURE14 :
                            glID == 15 ? GL_TEXTURE15 :
                            glID == 16 ? GL_TEXTURE16 :
                            glID == 17 ? GL_TEXTURE17 :
                            glID == 18 ? GL_TEXTURE18 :
                            glID == 19 ? GL_TEXTURE19 :
                            glID == 20 ? GL_TEXTURE20 :
                            glID == 21 ? GL_TEXTURE21 :
                            glID == 22 ? GL_TEXTURE22 :
                            glID == 23 ? GL_TEXTURE23 :
                            glID == 24 ? GL_TEXTURE24 :
                            glID == 25 ? GL_TEXTURE25 :
                            glID == 26 ? GL_TEXTURE26 :
                            glID == 27 ? GL_TEXTURE27 :
                            glID == 28 ? GL_TEXTURE28 :
                            glID == 29 ? GL_TEXTURE29 :
                            glID == 30 ? GL_TEXTURE30 :
                            glID == 31 ? GL_TEXTURE31 : 0;

            glActiveTexture(GL_Tex);
            glBindTexture(GL_TEXTURE_2D, texID);

        }

        public ChannelFormat PixelFormatToILFormat(PixelFormat format)
        {
            switch (format)
            {
                case PixelFormat.RGB:
                    return ChannelFormat.RGB;
                case PixelFormat.RGBA:
                    return ChannelFormat.RGBA;
                case PixelFormat.BGR:
                    return ChannelFormat.BGR;
                case PixelFormat.SingleChannel:
                    return 0;

                default:
                    return 0;
            }
        }
        public int ChannelsAmount(PixelFormat format)
        {
            switch (format)
            {
                case PixelFormat.RGB:
                    return 3;
                case PixelFormat.RGBA:
                    return 4;
                case PixelFormat.BGR:
                    return 3;
                case PixelFormat.SingleChannel:
                    return 1;

                default:
                    return 0;
            }
        }

        /// <summary>
        /// Returns a Vector3[ Red, Green, Blue ] for each pixel in the specified block of pixles
        /// <para> <paramref name="xStart"/> and <paramref name="yStart"/> specify the start of the block of pixels to be returned. </para>
        /// <para> <paramref name="xSpan"/> and <paramref name="ySpan"/> spcify the width and height of the block of pixles to be returned. </para>
        /// <para> Format = 0.0f - 1.0f </para>
        /// </summary>
        /// <param name="xStart"> X-coord of the start of the block of pixels to be returned. </param>
        /// <param name="yStart"> Y-coord of the start of the block of pixels to be returned. </param>
        /// <param name="xSpan"> Width of the block of pixels to be returned. </param>
        /// <param name="ySpan"> Height of the block of pixels to be returned. </param>
        public Vector3[] GetPixels(int xStart, int yStart, int xSpan = 1, int ySpan = 1)
        {

            ////bind image to have it be the one currently being modified
            //IL.BindImage(ILimgID);
            //
            ////get data using devIL
            //float[] returnData = new float[xSpan * ySpan * ChannelsAmount(pixelFormat)];
            //unsafe
            //{
            //    fixed(float* ptr = &returnData[0])
            //    {
            //        BBug.Log("Copy-Pixels return-value: " + IL.CopyPixels(xStart, yStart, 0, xSpan, ySpan, 0, PixelFormatToILFormat(pixelFormat), ChannelType.Float, (IntPtr)ptr));
            //    }
            //}
            //
            ////convert to vec3
            //Vector3[] result = new Vector3[xSpan * ySpan];
            //int id = 0;
            //for(int i = xStart; i < (xSpan * ySpan * 3)-3; i+=ChannelsAmount(pixelFormat))
            //{
            //    result[id] = new Vector3(returnData[i +0], returnData[i + 1], returnData[i + 2]);
            //    id++;
            //}
            //
            ////unbind and delete image to free memory
            //IL.BindImage(0);

            //should get the data out of the 'data' byte-array 
            throw new NotImplementedException();

            return null;
        }

        //public Vector3 GetPixel(int xCoord, yCoord)
        //{
        //
        //}

        #region IDISPOSABLE_SUPPORT

        /// <param name="disposing"> Defines whether to dispose managed ressources. </param>

        /// <summary> Dispose Texture, to free memory. </summary>
        public void Dispose()
        {
            //implemented in the Dispose method directly because theres no unmanaged ressources to free

            //dispose managed state (managed objects).
            if (data != null)
            {
                Array.Clear(data, 0, data.Length);
                data = null;
                glDeleteTexture(texID);
            }
        }
        #endregion
    }
}
