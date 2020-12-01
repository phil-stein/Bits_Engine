using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.Serialization;
using System.Text;

namespace BitsCore.DataManagement.Serialization
{
    internal class Vector2SerializationSurrogate : ISerializationSurrogate
    {

        //method called to serialize a Vector2 object
        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {

            Vector2 v2 = (Vector2)obj;
            info.AddValue("X", v2.X);
            info.AddValue("Y", v2.Y);
        }

        //method called to deserialize a Vector2 object
        public object SetObjectData(object obj, SerializationInfo info,
                                           StreamingContext context, ISurrogateSelector selector)
        {

            Vector2 v2 = (Vector2)obj;
            v2.X = (float)info.GetValue("X", typeof(float));
            v2.Y = (float)info.GetValue("Y", typeof(float));
            obj = v2;
            return obj;
        }
    }
}
