using BitsCore.Debugging;
using BitsCore.ObjectData;
using BitsCore.ObjectData.Components;
using System.Runtime.Serialization;

namespace BitsCore.DataManagement.Serialization
{
    internal class MeshSerializationSurrogate : ISerializationSurrogate
    {

        //method called to serialize a Mesh object
        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {

            Mesh m = (Mesh)obj;
            info.AddValue("vlen", m.vertices.Length);
            for(int i = 0; i < m.vertices.Length; i++)
            {
                info.AddValue("v" + i, m.vertices[i]);
            }
            info.AddValue("tlen", m.triangles.Length);
            for (int i = 0; i < m.triangles.Length; i++)
            {
                info.AddValue("t" + i, m.triangles[i]);
            }
            info.AddValue("index", m.notIndexed);
        }

        //method called to deserialize a Mesh object
        public object SetObjectData(object obj, SerializationInfo info,
                                           StreamingContext context, ISurrogateSelector selector)
        {

            int vertLen = (int)info.GetValue("vlen", typeof(int));
            float[] verts = new float[vertLen];
            for(int i = 0; i < vertLen; i++)
            {
                verts[i] = (float)info.GetValue("v"+i, typeof(float));

            }
            int triLen = (int)info.GetValue("tlen", typeof(int));
            uint[] tris = new uint[triLen];
            for (int i = 0; i < triLen; i++)
            {
                tris[i] = (uint)info.GetValue("t" + i, typeof(uint));

            }
            bool notIndexed = (bool)info.GetValue("index", typeof(bool));

            return new Mesh(verts, tris, notIndexed);
        }
    }
}
