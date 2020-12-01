using BitsCore.ObjectData;
using BitsCore.Utils;
using DevILSharp;
using System;
using System.Diagnostics;
using System.IO;

namespace BitsCore.DataManagement
{
    public static class ImageImporter
    {

        public static Texture LoadTexture(string filePath)
        {
            //docs: http://openil.sourceforge.net/docs/DevIL%20Manual.pdf
            //also useful: http://openil.sourceforge.net/docs/DevIL%20Reference%20Guide.pdf
            //tut: https://www.geeks3d.com/20090105/tutorial-how-to-load-and-display-an-image-with-devil-and-opengl/

            //Debug.WriteLine("DevIL-Version: " + IL.GetInteger(IntName.VersionNumber).ToString());

            //init the image-library
            IL.Init(); //low-level library for loading/saving images
            //ILU.Init(); //mid-level library for manipulating images, not always necessary

            //generate image-name, to reference image later on
            int ImgId = IL.GenImage();

            //bind image to have it be the one currently being modified
            IL.BindImage(ImgId);

            //returns false if the image was read correctly
            bool load = IL.LoadImage(filePath);
            //Debug.WriteLine("IL.LoadImage result: " + load.ToString());
            if (load == false)
            {
                DevILSharp.ErrorCode error = IL.GetError();
                throw new System.ArgumentException("DevIL could not load the Image. ErrorCode: " + error.ToString() + ", FilePath: '" + filePath + "' ", "filePath");
            }

            //get all info about the image
            int width = IL.GetInteger(IntName.ImageWidth);
            int height = IL.GetInteger(IntName.ImageHeight);
            ChannelFormat channelFormat = IL.GetFormat();
            ChannelType channelType = IL.GetDataType();

            //Debug.WriteLine("\n |DevIL-ImageLoad: ChannelFormat: " + channelFormat.ToString() + ", ChannelType: " + channelType.ToString() + ", Path: " + filePath +"\n");

            //the image has 3/4 bytes per pixel, depending on the amount of channels
            byte[] data = new byte[width * height * (channelFormat == ChannelFormat.RGBA ? 4 : 3)];
            //get the loaded images data
            unsafe
            {
                fixed (byte* ptr = &data[0])
                {
                    IL.CopyPixels(0, 0, 0, width, height, 1, channelFormat, channelType, (IntPtr)ptr);
                }
            }

            //unbind and delete image to free memory
            IL.BindImage(0);
            IL.DeleteImage(ImgId);

            PixelFormat pixelFormat = PixelFormat.RGB;

            //convert DevIL's format enum to our Texture-Classes format enum
            switch (channelFormat)
            {
                case ChannelFormat.RGB:
                    pixelFormat = PixelFormat.RGB;
                    break;
                case ChannelFormat.RGBA:
                    pixelFormat = PixelFormat.RGBA;
                    break;
                case ChannelFormat.BGR:
                    pixelFormat = PixelFormat.BGR;
                    break;

                default:
                    break;
            }

            return new Texture(pixelFormat, data, width, height, filePath, data.Length);

        }

    }
}
