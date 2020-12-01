using BitsCore.Debugging;
using BitsCore.ObjectData;
using BitsCore.ObjectData.Components;
using BitsCore.ObjectData.Materials;
using System.Runtime.Serialization;

namespace BitsCore.DataManagement.Serialization
{
    internal class ModelSerializationSurrogate : ISerializationSurrogate
    {

        //method called to serialize a Mesh object
        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {

            Model m = (Model)obj;
            bool isAsset = AssetManager.HasAsset(m.meshFileName);
            info.AddValue("isAsset", isAsset);
            if (isAsset)
            {
                info.AddValue("name", m.meshFileName);
                info.AddValue("mat", m.material);
            }
            else
            {
                info.AddValue("mesh", m.mesh);
                info.AddValue("name", m.meshFileName);
                info.AddValue("mat", m.material);
            }

        }

        //method called to deserialize a Mesh object
        public object SetObjectData(object obj, SerializationInfo info,
                                           StreamingContext context, ISurrogateSelector selector)
        {
            bool isAsset = (bool)info.GetValue("isAsset", typeof(bool));
            Model model = new Model(null, null, "");
            if (isAsset)
            {
                string fileName = (string)info.GetValue("name", typeof(string));
                Material mat = (Material)info.GetValue("mat", typeof(Material));
                Mesh m = AssetManager.GetMesh(fileName);
                model = new Model(m, mat, fileName);
            }
            else
            {
                Mesh m = (Mesh)info.GetValue("mesh", typeof(Mesh));
                string fileName = (string)info.GetValue("name", typeof(string));
                Material mat = (Material)info.GetValue("mat", typeof(Material));
                model = new Model(m ,mat, fileName);
            }

            return model;
        }
    }
}
