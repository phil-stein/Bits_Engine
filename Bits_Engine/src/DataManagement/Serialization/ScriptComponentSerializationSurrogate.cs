using BitsCore.Debugging;
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
    internal class ScriptComponentSerializationSurrogate : ISerializationSurrogate
    {

        //method called to serialize an object derived from script-component
        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {

            ScriptComponent sc = (ScriptComponent)obj;
            //BindingFlags bindingFlags = BindingFlags.Public;
            //FieldInfo[] vars = ReflectionUtils.GetVariablesInClassInfo(sc, bindingFlags).ToArray();
            //info.AddValue("len", vars.Length);
            //int i = 0;
            //
            //foreach (FieldInfo field in vars)
            //{
            //    info.AddValue("info", field);
            //    info.AddValue("val" + i, field.GetValue(sc));
            //    i++;
            //}
            info.AddValue("type_name", obj.GetType().FullName);
        }

        //method called to deserialize an object derived from script-component
        public object SetObjectData(object obj, SerializationInfo info,
                                           StreamingContext context, ISurrogateSelector selector)
        {
            //ScriptComponent sc = (ScriptComponent)obj;
            //int length = (int)info.GetValue("len", typeof(int));
            //
            //for (int i = 0; i < length; i++)
            //{
            //    FieldInfo field = (FieldInfo)info.GetValue("info", typeof(FieldInfo));
            //    Type t = field.FieldType;
            //    object var = info.GetValue("val" + i, field.FieldType);
            //    field.SetValue(sc, var);
            //}

            string tname = (string)info.GetValue("type_name", typeof(string));

            Type t = null;
            foreach(Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                t = asm.GetType(tname);
                if(t != null) { break; }
            }

            return Activator.CreateInstance(t);
        }
    }
}
