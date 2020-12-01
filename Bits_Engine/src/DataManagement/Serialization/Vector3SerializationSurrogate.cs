using System.Runtime.Serialization;
using System.Collections;
using System.Numerics;

namespace BitsCore.DataManagement.Serialization
{
    internal class Vector3SerializationSurrogate : ISerializationSurrogate
    {

        //method called to serialize a Vector3 object
        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {

            Vector3 v3 = (Vector3)obj;
            info.AddValue("X", v3.X);
            info.AddValue("Y", v3.Y);
            info.AddValue("Z", v3.Z);
        }

        //method called to deserialize a Vector3 object
        public object SetObjectData(object obj, SerializationInfo info,
                                           StreamingContext context, ISurrogateSelector selector)
        {

            Vector3 v3 = (Vector3)obj;
            v3.X = (float)info.GetValue("X", typeof(float));
            v3.Y = (float)info.GetValue("Y", typeof(float));
            v3.Z = (float)info.GetValue("Z", typeof(float));
            obj = v3;
            return obj;
        }
    }
}
