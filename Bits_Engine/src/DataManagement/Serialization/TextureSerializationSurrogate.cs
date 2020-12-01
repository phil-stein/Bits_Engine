using BitsCore.ObjectData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Runtime.Serialization;
using System.Text;

namespace BitsCore.DataManagement.Serialization
{
    internal class TextureSerializationSurrogate : ISerializationSurrogate
    {

        //method called to serialize a texture object
        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {

            Texture val = (Texture)obj;
            info.AddValue("path", val.filePath);
        }

        //method called to deserialize a texture object
        public object SetObjectData(object obj, SerializationInfo info,
                                           StreamingContext context, ISurrogateSelector selector)
        {

            Texture val = (Texture)obj;
            string path = (string)info.GetValue("path", typeof(string));

            val = AssetManager.GetTexture(Path.GetFileNameWithoutExtension(path));

            obj = val;
            return obj;
        }
    }
}
