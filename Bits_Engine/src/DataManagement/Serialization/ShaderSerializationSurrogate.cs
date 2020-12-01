using BitsCore.ObjectData;
using BitsCore.ObjectData.Shaders;
using System.IO;
using System.Runtime.Serialization;

namespace BitsCore.DataManagement.Serialization
{
    internal class ShaderSerializationSurrogate : ISerializationSurrogate
    {

        //method called to serialize a shader object
        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {

            Shader val = (Shader)obj;
            info.AddValue("shaderAssetKey", val.shaderAssetKey);
            info.AddValue("vertAssetKey", val.vertAssetKey);
            info.AddValue("fragAssetKey", val.fragAssetKey);
        }

        //method called to deserialize a shader object
        public object SetObjectData(object obj, SerializationInfo info,
                                           StreamingContext context, ISurrogateSelector selector)
        {

            Shader val = (Shader)obj;

            string shaderAssetKey = (string)info.GetValue("shaderAssetKey", typeof(string));
            string vertAssetKey = (string)info.GetValue("vertAssetKey", typeof(string));
            string fragAssetKey = (string)info.GetValue("fragAssetKey", typeof(string));

            AssetManager.AddShader(shaderAssetKey, vertAssetKey, fragAssetKey);
            val = AssetManager.GetShader(shaderAssetKey);

            obj = val;
            return obj;
        }
    }
}
