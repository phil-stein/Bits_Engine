using BitsCore.Debugging;
using BitsCore.ObjectData;
using BitsCore.ObjectData.Components;
using BitsCore.Utils.Reflection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;

namespace BitsCore.DataManagement.Serialization
{
    internal class GameObjectSerializationSurrogate : ISerializationSurrogate
    {

        //method called to serialize a GameObject object
        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {

            GameObject go = (GameObject)obj;
            info.AddValue("tag", go.tag);
            info.AddValue("trans", go.transform);
            info.AddValue("len", go.components.Length);
            for(int i = 0; i < go.components.Length; i++)
            {
                info.AddValue(("comp" + i), go.components[i]);
            }
        }

        //method called to deserialize a GameObject object
        public object SetObjectData(object obj, SerializationInfo info,
                                           StreamingContext context, ISurrogateSelector selector)
        {
            string tag = (string)info.GetValue("tag", typeof(string));
            Transform trans = (Transform)info.GetValue("trans", typeof(Transform));
            
            int compsLen = (int)info.GetValue("len", typeof(int));

            Component[] comps = new Component[compsLen];
            GameObject go = new GameObject(trans, tag);

            for(int i = 0; i < compsLen; i++)
            {
                comps[i] = (Component)info.GetValue(("comp" +i), typeof(Component));
                go.AddComp(comps[i]);
            }

            return go;
        }
    }
}
