using BitsCore.ObjectData.Components;
using BitsCore.ObjectData.Materials;
using BitsCore.ObjectData;
using BitsCore.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Numerics;
using System.Reflection;
using System.Text;
using BitsCore.Debugging;
using BitsCore.SceneManagement;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using BitsCore.Utils.Reflection;
using BitsCore.ObjectData.Shaders;

namespace BitsCore.DataManagement.Serialization
{
    public static class Serializer
    {
        /// <summary> 
        /// Serializes a <see cref="Scene"/> and all it's <see cref="GameObject"/>'s. 
        /// <para> Saves the serialized data to <paramref name="path"/>. </para>
        /// </summary>
        /// <param name="scene"> The <see cref="Scene"/> to be serialized. </param>
        /// <param name="path"> The path the serialized scene will be saved to. </param>
        public static void SerializeScene(Scene scene, string path)
        {
            IFormatter formatter = new BinaryFormatter(); //should prob make one formatter for the entire class and use that one for all operations, ut prob only better with a lot of scene-switching

            //add the surrogate selector
            SurrogateSelector surrogateSelector = MakeSurrogateSelector();
            formatter.SurrogateSelector = surrogateSelector;

            Stream stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, scene);
            stream.Close();
        }
        /// <summary> 
        /// Deerializes a <see cref="Scene"/> and all it's <see cref="GameObject"/>'s. 
        /// <para> Returns the deserialized data. </para>
        /// </summary>
        /// <param name="path"> The path the serialized scene will be deserialized from. </param>
        public static Scene DeserializeScene(string path)
        {
            IFormatter formatter = new BinaryFormatter(); //should prob make one formatter for the entire class and use that one for all operations, ut prob only better with a lot of scene-switching

            //add the surrogate selector
            SurrogateSelector surrogateSelector = MakeSurrogateSelector();
            formatter.SurrogateSelector = surrogateSelector;

            Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            Scene scene = (Scene)formatter.Deserialize(stream);
            stream.Close();
            return scene;
        }

        private static SurrogateSelector MakeSurrogateSelector()
        {
            SurrogateSelector surrogateSelector = new SurrogateSelector();
            Vector3SerializationSurrogate vector3SS = new Vector3SerializationSurrogate();
            Vector2SerializationSurrogate vector2SS = new Vector2SerializationSurrogate();

            GameObjectSerializationSurrogate goSS = new GameObjectSerializationSurrogate();
            MeshSerializationSurrogate meshSS = new MeshSerializationSurrogate();
            ModelSerializationSurrogate modelSS = new ModelSerializationSurrogate();

            TextureSerializationSurrogate textureSS = new TextureSerializationSurrogate();
            ShaderSerializationSurrogate shaderSS = new ShaderSerializationSurrogate();

            ScriptComponentSerializationSurrogate sCompSS = new ScriptComponentSerializationSurrogate();

            surrogateSelector.AddSurrogate(typeof(Vector3), new StreamingContext(StreamingContextStates.All), vector3SS);
            surrogateSelector.AddSurrogate(typeof(Vector2), new StreamingContext(StreamingContextStates.All), vector2SS);

            surrogateSelector.AddSurrogate(typeof(GameObject), new StreamingContext(StreamingContextStates.All), goSS);
            surrogateSelector.AddSurrogate(typeof(Mesh), new StreamingContext(StreamingContextStates.All), meshSS);
            surrogateSelector.AddSurrogate(typeof(Model), new StreamingContext(StreamingContextStates.All), modelSS);

            surrogateSelector.AddSurrogate(typeof(Texture), new StreamingContext(StreamingContextStates.All), textureSS);
            surrogateSelector.AddSurrogate(typeof(Shader), new StreamingContext(StreamingContextStates.All), shaderSS);

            //script component surrogate
            foreach (Type t in ReflectionUtils.GetInheritedOfType<ScriptComponent>())
            {
                surrogateSelector.AddSurrogate(t, new StreamingContext(StreamingContextStates.All), sCompSS);
            }

            return surrogateSelector;
        }

        #region OLD_READ_&_WRITE
        /*

        //I/O-GameObjects
        public static void WriteGameObjectToFile(string path, GameObject go)
        {
            //has async func
            //Debug.WriteLine("\n|----------------------------------- \nStarted WriteGameObjectToFile(). \n");
            bool ext = path.Contains(".gameobject"); //check if the path-string contains the file-extension
            File.WriteAllBytes(path + @"\" + go.tag + (ext ? "" : ".gameobject"), Serializer.SerializeGameObject(go));
            //Debug.WriteLine("\nWrote to: " + path + @"\" + go.tag + ".gameObject \n|-----------------------------------");
        }
        public static GameObject ReadGameObjectFromFile(string path, string tag)
        {
            //has async func
            //Debug.WriteLine("\n|----------------------------------- \nStarted ReadGameObjectFromFile().\n");
            //Debug.WriteLine(" |Opened File: " + path + @"\" + tag + ".gameObject");
            bool ext = path.Contains(".gameobject"); //check if the path-string contains the file-extension
            return DeserializeGameObject(File.ReadAllBytes(path + @"\" + tag + (ext ? "" : ".gameobject")));
        }
        public static void WriteComponentToFile(string path, Component comp)
        {
            //has async func
            //Debug.WriteLine("Wrote to: " + path + @"\" + comp.ToString() + ".component");
            bool ext = path.Contains(".component"); //check if the path-string contains the file-extension
            File.WriteAllBytes(path + @"\" + comp.GetType().Name + (ext ? "" : ".component"), Serializer.SerializeComponent(comp));
        }
        public static Component ReadComponentFromFile(string path, string name)
        {
            //has async func
            //Debug.WriteLine("Opened File: " + path + @"\" + name + ".component");
            bool ext = path.Contains(".component"); //check if the path-string contains the file-extension
            return DeserializeComponent(File.ReadAllBytes(path + @"\" + name + (ext ? "" : ".component")));
        }
        public static void WriteMaterialToFile(string path, Material mat)
        {
            //has async func
            //Debug.WriteLine("Wrote to: " + path + @"\" + mat.ToString() + ".material");
            bool ext = path.Contains(".material"); //check if the path-string contains the file-extension
            File.WriteAllBytes(path + @"\" + mat.GetType().Name + (ext ? "" : ".material"), Serializer.SerializeMaterial(mat));
        }
        public static Material ReadMaterialFromFile(string path, string name)
        {
            //has async func
            //Debug.WriteLine("Opened File: " + path + @"\" + name + ".material");
            bool ext = path.Contains(".material"); //check if the path-string contains the file-extension
            return DeserializeMaterial(File.ReadAllBytes(path + @"\" + name + (ext ? "" : ".material")));
        }

        */
        #endregion

        #region OLD_SERIALIE_&DESERIALIZE
        /*
         
        /// <summary> Converts the GameObjects Components and variables into bytes and returns them. </summary>
        /// <param name="go"> The GameObject to be serialized. </param>
        public static byte[] SerializeGameObject(GameObject go)
        {
            //structure: [guid...]->[tag-len(int32)_4bytes]->[tag(string)_given]->[trans(Transform/float*9 +1)_37bytes]->
            //structure-comps: ->[comp-len(int32)_4bytes]->[comp(Component)_given]->repeat

            List<byte> bytes = new List<byte>();

            //guid should go here
            //...

            //tag
            //Debug.WriteLine("  |Tag: " + go.tag);

            byte[] tagBytes = VarUtils.VarToBytes(go.tag);
            bytes.AddRange(VarUtils.VarToBytes(tagBytes.Length)); //tag length
            bytes.AddRange(tagBytes); //tag
            //Debug.WriteLine("  |Serialized Tag, Tag-Len: " + tagBytes.Length.ToString() + ", readPos: " + bytes.Count.ToString());

            //transform, this is outside the loop, as every GameObject has a Transform, but varying components
            //nor length int because every transform is 37 bytes long 9*float(4bytes each) + 1byte for the id
            byte[] transBytes = SerializeComponent(go.transform);
            bytes.AddRange(transBytes); //add the serialized data from the component
            //Debug.WriteLine("  |Serialized Transform, Length: " + transBytes.Length.ToString() + ", readPos: " + bytes.Count.ToString());

            //Debug.WriteLine("  |Components-Len: " + go.components.Length.ToString());
            //components
            if (go.components.Length != 0 && go.components != null)
            {
                foreach (Component comp in go.components)
                {
                    byte[] compBytes = SerializeComponent(comp);
                    
                    bytes.AddRange(VarUtils.VarToBytes(compBytes.Length)); //add the length of the comp to the front of the comp

                    bytes.AddRange(compBytes); //add the serialized data from the component

                    //Debug.WriteLine("  |Serialized Length-Total: " + bytes.Count.ToString() + ", Length-Comp: " + compBytes.Length.ToString());
                }
            }

            return bytes.ToArray();
        }
        /// <summary> Converts the given bytes into a GameObject with Components and variables and returns it. </summary>
        /// <param name="bytes"> The bytes storing the GameObject </param>
        public static GameObject DeserializeGameObject(byte[] bytes)
        {
            //structure: [guid...]->[tag-len(int32)_4bytes]->[tag(string)_given]->[trans-len(int32)_4bytes]->[trans(Transform)_given]->
            //structure-comps: ->[comp-len(int32)_4bytes]->[comp(Component)_given]->repeat
            //Debug.WriteLine(" |Serializer.DeserializeComp()");

            int readPos = 0;

            //tag
            int tagLen = VarUtils.BytesToInt(ArrayUtils.GetBetweenXAndYExcl(bytes, readPos, readPos + sizeof(int))); //tag-len
            readPos += sizeof(int);
            string tag = VarUtils.BytesToString(ArrayUtils.GetBetweenXAndYExcl(bytes, readPos, readPos + tagLen)); //tag
            readPos += tagLen;
            //Debug.WriteLine("  |Deserialized Tag: " + tag + ", readPos: " + readPos.ToString());

            //transform
            //int transLen = VarUtils.BytesToInt(ArrayUtils.GetBetweenXAndYExcl(bytes, readPos, readPos + sizeof(int))); //trans-len
            //readPos += sizeof(int);
            byte[] transBytes = ArrayUtils.GetBetweenXAndYExcl(bytes, readPos, readPos + 37); //transform is 37 bytes long 9*float(4bytes each) + 1byte for the id
            Transform trans = (Transform)DeserializeComponent(transBytes); //trans
            readPos += transBytes.Length;
            //Debug.WriteLine("  |Trasnform-Len: " + transBytes.Length.ToString() + ", readPos: " + readPos.ToString());
            //Debug.WriteLine("  |Deserialized Transform: " + trans.ToString());

            GameObject go = new GameObject(trans, tag);

            //go through the rest of the Components and add them to the GameObject
            while (readPos < bytes.Length -1)
            {
                //component
                int compLen = VarUtils.BytesToInt(ArrayUtils.GetBetweenXAndYExcl(bytes, readPos, readPos + sizeof(int))); //comp-len
                readPos += sizeof(int);

                byte[] compBytes = ArrayUtils.GetBetweenXAndYExcl(bytes, readPos, readPos + compLen);
                Component comp = DeserializeComponent(compBytes);

                go.AddComp(comp); //comp
                
                readPos += compLen;
                //Debug.WriteLine("  |Deserialized Comp: " + comp.GetType().ToString());
            }
            //Debug.WriteLine(" |Deserialized GameObject: " + go.tag);
            return go;
        }

        /// <summary> Converts the Components type and variables into bytes and returns them. </summary>
        /// <param name="component"> The Component to be serialized. </param>
        public static byte[] SerializeComponent(Component component)
        {
            //Debug.WriteLine(" |Serializer.SerializeComp(): " + component.GetType().Name);

            if (component.GetType() == typeof(Transform))
            {
                //structure: [Comp-ID(byte)_1byte]->[Position(3*float)_12bytes]->[Rotation(3*float)_12bytes]->[Scale(3*float)_12bytes]->[parent-guid ...]

                Transform comp = (Transform)component;
                //global because transform is set before parent is set
                Vector3 pos = comp.GetGlobalPos();
                Vector3 rot = comp.GetGlobalRot();
                Vector3 sca = comp.GetGlobalScale();

                List<byte> bytes = new List<byte>();

                bytes.Add(comp.ID);

                bytes.AddRange(VarUtils.VarToBytes(pos));
                //Debug.WriteLine("  |Serialized Position");

                bytes.AddRange(VarUtils.VarToBytes(rot));
                //Debug.WriteLine("  |Serialized Rotation");

                bytes.AddRange(VarUtils.VarToBytes(sca));
                //Debug.WriteLine("  |Serialized Scale");

                //parent needs guid
                //...

                return bytes.ToArray();

            }
            else if(component.GetType() == typeof(Mesh))
            {
                //structure: [Comp-ID(byte)_1byte]->[Vert-Array-Len(int32)_4bytes]->[Vert-Array(float[])_given]->[Tris-Array-Len(int32)_4bytes]->[Tris-Array(uint[])_given]->[Mat-Len(int32)_4bytes]->[Mat(Material)_given]

                //cast to right type
                Mesh comp = (Mesh)component;

                List<byte> bytes = new List<byte>();

                //component-id
                bytes.Add(comp.ID);

                //serialize verts---------------
                bytes.AddRange(VarUtils.VarToBytes(sizeof(float) * comp.vertices.Length)); //vert array-length indicator
                bytes.AddRange(ArrayUtils.FloatToByte(comp.vertices)); //vert-array
                //Debug.WriteLine("  |Serialized Verts - Len: " + ArrayUtils.FloatToByte(comp.vertices).Length.ToString());

                //serialize tris----------------
                bytes.AddRange(VarUtils.VarToBytes(sizeof(uint) * comp.triangles.Length)); //tris array-length indicator
                bytes.AddRange(ArrayUtils.UintToByte(comp.triangles)); //tris-array
                //Debug.WriteLine("  |Serialized Tris - Len: " + ArrayUtils.UintToByte(comp.triangles).Length.ToString());

                //serialize uv-coords-----------
                //...

                //serialize material------------
                byte[] matBytes = SerializeMaterial(comp.material);
                bytes.AddRange(VarUtils.VarToBytes(matBytes.Length)); //mat-len
                bytes.AddRange(matBytes); //mat
                //Debug.WriteLine("  |Serialized Material - Len: " + matBytes.Length.ToString());

                return bytes.ToArray();
            }
            else if (component.GetType() == typeof(Billboard))
            {
                //structure: [Comp-ID(byte)_1byte]->[Size(Vector2 / 2*float)_8bytes]

                //cast to right type
                Billboard comp = (Billboard)component;

                List<byte> bytes = new List<byte>();

                //component-id
                bytes.Add(comp.ID);

                //serialize size---------------
                bytes.AddRange(VarUtils.VarToBytes(comp.size)); //size-vec2

                return bytes.ToArray();
            }
            else if (component.GetType() == typeof(RandomHeight))
            {
                //structure: [Comp-ID(byte)_1byte]->[xSize(int)_4bytes]->[ySize(int)_4bytes]->[strength(float)_4bytes]->[frequency(float)_4bytes]->[lacunarity(float)_4bytes]->[gain(float)_4bytes]->[seed(int32)_4bytes]

                //cast to right type
                RandomHeight comp = (RandomHeight)component;

                List<byte> bytes = new List<byte>();

                //component-id
                bytes.Add(comp.ID);

                //serialize vars---------------
                bytes.AddRange(VarUtils.VarToBytes(comp.xSize));
                bytes.AddRange(VarUtils.VarToBytes(comp.ySize));
                bytes.AddRange(VarUtils.VarToBytes(comp.strength));
                bytes.AddRange(VarUtils.VarToBytes(comp.frequency));
                bytes.AddRange(VarUtils.VarToBytes(comp.lacunarity));
                bytes.AddRange(VarUtils.VarToBytes(comp.gain));
                bytes.AddRange(VarUtils.VarToBytes(comp.seed));

                return bytes.ToArray();
            }
            else
            {
                //Debug.WriteLine("!!! Serialization fro this Component-Type is not supported !!!");

                //insert the ID to be abled to know the type of component to be saved
                byte[] bytes = new byte[] { component.ID };
                return bytes;
            }
        }
        /// <summary> Converts the given bytes to a Component and variables. </summary>
        /// <param name="bytes"> The bytes containing the serialized Component. </param>
        public static Component DeserializeComponent(byte[] bytes)
        {
            //Type t = Component.GetCompByID(bytes[sizeof(int) +1]);
            //Debug.WriteLine("Serializer.DeserializeComp(): " + t.Name);

            int readPos = 0; //indicates how far along through the byte-array we have deserialized

            Type t = Component.GetCompByID(bytes[readPos]);
            readPos += 1; //1 because the comp-id is 1 byte long

            //Debug.WriteLine("  |Deserializing CompType: " + t.ToString() + ", CompID: " + bytes[readPos + 1].ToString() + ", ReadPos: " + readPos.ToString());

            if (t == typeof(Transform))
            {
                //structure: [Comp-ID(byte)_1byte]->[Position(3*float)_12bytes]->[Rotation(3*float)_12bytes]->[Scale(3*float)_12bytes]->[parent-guid ...]

                if (bytes[0] != Component.GetCompID(typeof(Transform))) 
                { 
                    //Debug.WriteLine("!!! Deserialization failed !!!"); 
                    return null; 
                }

                //position
                byte[] posBytes = ArrayUtils.GetBetweenXAndYExcl(bytes, readPos, readPos + sizeof(float)*3);
                Vector3 pos = VarUtils.BytesToVector3(posBytes);
                readPos += sizeof(float) * 3;

                //rotation
                byte[] rotBytes = ArrayUtils.GetBetweenXAndYExcl(bytes, readPos, readPos + sizeof(float) * 3);
                Vector3 rot = VarUtils.BytesToVector3(rotBytes);
                readPos += sizeof(float) * 3;

                //scale
                byte[] scaBytes = ArrayUtils.GetBetweenXAndYExcl(bytes, readPos, readPos + sizeof(float) * 3);
                Vector3 sca = VarUtils.BytesToVector3(scaBytes);
                readPos += sizeof(float) * 3;

                //parent
                //...

                return new Transform(pos, rot, sca);
            }
            else if (t == typeof(Mesh))
            {
                //structure: [Comp-ID(byte)_1byte]->[Vert-Array-Len(int32)_4bytes]->[Vert-Array(float[])_given]->[Tris-Array-Len(int32)_4bytes]->[Tris-Array(uint[])_given]->[Mat-Len(int32)_4bytes]->[Mat(Material)_given]

                if (bytes[0] != Component.GetCompID(typeof(Mesh))) 
                { 
                    //Debug.WriteLine("!!! Deserialization failed !!!"); 
                    return null; 
                }

                //deserialize verts---------------
                //get the bytes after the comp-id that specify the length of the given vertices,
                byte[] vertLenByte = ArrayUtils.GetBetweenXAndYExcl(bytes, readPos, sizeof(int) + readPos);
                int vertLen = VarUtils.BytesToInt(vertLenByte);
                readPos += sizeof(int); //we've read the length of the float array, given as an int

                byte[] vertsBytes = ArrayUtils.GetBetweenXAndYExcl(bytes, readPos, readPos + vertLen); //get the bytes after the vert-length indicator and get as many as the vert-len indicator defined
                float[] verts = ArrayUtils.BytesToFloat(vertsBytes);
                readPos += vertLen;

                //deserialize tris----------------
                byte[] trisLenByte = ArrayUtils.GetBetweenXAndYExcl(bytes, readPos, sizeof(int) + readPos);
                int trisLen = VarUtils.BytesToInt(trisLenByte);
                readPos += sizeof(int); //we've read the length of the uint array, given as an int

                byte[] trisBytes = ArrayUtils.GetBetweenXAndYExcl(bytes, readPos, readPos + trisLen); //get the bytes after the vert-length indicator and get as many as the vert-len indicator defined
                uint[] tris = ArrayUtils.BytesToUint(trisBytes);
                readPos += trisLen;

                //deserialize uv-coords-----------
                //...

                //deserialize material------------
                byte[] matLenByte = ArrayUtils.GetBetweenXAndYExcl(bytes, readPos, sizeof(int) + readPos);
                int matLen = VarUtils.BytesToInt(matLenByte);
                readPos += sizeof(int); //we've read the length of the uint array, given as an int

                Material mat = DeserializeMaterial(ArrayUtils.GetBetweenXAndYExcl(bytes, readPos, readPos + matLen));
                mat.Load(); //material needs to be loaded before the mesh can use it

                return new Mesh(verts, tris, mat);
            }
            else if (t == typeof(Billboard))
            {
                //structure: [Comp-Len(int)_4bytes]->[Comp-ID(byte)_1byte]->[Size(Vector2 / 2*float)_8bytes]

                if (bytes[0] != Component.GetCompID(typeof(Billboard)))
                {
                    //Debug.WriteLine("!!! Deserialization failed !!!"); 
                    return null;
                }

                //size
                byte[] sizeBytes = ArrayUtils.GetBetweenXAndYExcl(bytes, readPos, readPos + sizeof(float) * 2);
                Vector2 size = VarUtils.BytesToVector2(sizeBytes);

                System.Diagnostics.Debug.WriteLine("  |Billboard Size: " + size.ToString());

                return new Billboard(size);
            }
            else if (t == typeof(RandomHeight))
            {
                //structure: [Comp-ID(byte)_1byte]->[xSize(int)_4bytes]->[ySize(int)_4bytes]->[strength(float)_4bytes]->[frequency(float)_4bytes]->[lacunarity(float)_4bytes]->[gain(float)_4bytes]->[seed(int32)_4bytes]

                if (bytes[0] != Component.GetCompID(typeof(RandomHeight)))
                {
                    System.Diagnostics.Debug.WriteLine("!!! Deserialization failed !!!"); 
                    return null;
                }

                System.Diagnostics.Debug.WriteLine("\n |Deserialize RandHeight-Comp: bytes-len: " + bytes.Length.ToString() + ", comp-len: " + (sizeof(int)*3 + sizeof(float)*4 +1).ToString());
                //vars
                int xSize = VarUtils.BytesToInt(ArrayUtils.GetBetweenXAndYExcl(bytes, readPos, readPos + sizeof(int)));
                readPos += sizeof(int);

                int ySize = VarUtils.BytesToInt(ArrayUtils.GetBetweenXAndYExcl(bytes, readPos, readPos + sizeof(int)));
                readPos += sizeof(int);

                float strength = VarUtils.BytesToFloat(ArrayUtils.GetBetweenXAndYExcl(bytes, readPos, readPos + sizeof(float)));
                readPos += sizeof(float);

                float frequency = VarUtils.BytesToFloat(ArrayUtils.GetBetweenXAndYExcl(bytes, readPos, readPos + sizeof(float)));
                readPos += sizeof(float);

                float lacunarity = VarUtils.BytesToFloat(ArrayUtils.GetBetweenXAndYExcl(bytes, readPos, readPos + sizeof(float)));
                readPos += sizeof(float);

                float gain = VarUtils.BytesToFloat(ArrayUtils.GetBetweenXAndYExcl(bytes, readPos, readPos + sizeof(float)));
                readPos += sizeof(float);

                int seed = VarUtils.BytesToInt(ArrayUtils.GetBetweenXAndYExcl(bytes, readPos, readPos + sizeof(int)));
                readPos += sizeof(int);

                return new RandomHeight(xSize, ySize, strength, frequency, lacunarity, gain, seed);
            }
            else
            {
                Type T = Component.GetCompByID(bytes[0]);
                //Debug.WriteLine("!!! Component (" + T.FullName + ") not Deserializable !!!");
                return null;
            }
        }

        /// <summary> Serializes the Material into an array of bytes. </summary>
        /// <param name="material"></param>
        public static byte[] SerializeMaterial(Material material)
        {
            List<byte> bytes = new List<byte>(); //the list of bytes that gets returned after serialization
            bytes.Add(Material.GetMatID(material.GetType())); //material-id

            if (material.GetType() == typeof(BasicPhongMaterial))
            {
                //structure: [Mat-ID(byte)_1byte]->[VertCode-Len(int32)_4bytes]->[VertCode(string)_ given]->[FragCode-Len(int32)_4bytes]->[FragCode(string)_ given]
                //structure-vars: [^shadercode]->[ambient(3*float)_12bytes]->[diffuse(3*float)_12bytes]->[specular(3*float)_12bytes]->[shininess(float)_4bytes]

                BasicPhongMaterial mat = (BasicPhongMaterial)material;

                //shaders need to be serialized for every material, so it's seperated into its own method
                bytes.AddRange(SerializeShaderPaths(mat));

                //variables
                bytes.AddRange(VarUtils.VarToBytes(mat.ambient));
                bytes.AddRange(VarUtils.VarToBytes(mat.diffuse));
                bytes.AddRange(VarUtils.VarToBytes(mat.specular));
                bytes.AddRange(VarUtils.VarToBytes(mat.shininess));

                return bytes.ToArray();
            }
            else if (material.GetType() == typeof(UnlitMaterial))
            {
                //structure: [Mat-ID(byte)_1byte]->[VertCode-Len(int32)_4bytes]->[VertCode(string)_ given]->[FragCode-Len(int32)_4bytes]->[FragCode(string)_ given]

                //shaders need to be serialized for every material, so it's seperated into its own method
                bytes.AddRange(SerializeShaderPaths(material));

                //vars
                UnlitMaterial mat = (UnlitMaterial)material;
                bytes.AddRange(VarUtils.VarToBytes(mat.objColor));

                return bytes.ToArray();
            }
            else if (material.GetType() == typeof(LightMaterial))
            {
                //structure: [Mat-ID(byte)_1byte]->[VertCode-Len(int32)_4bytes]->[VertCode(string)_ given]->[FragCode-Len(int32)_4bytes]->[FragCode(string)_ given]

                //shaders need to be serialized for every material, so it's seperated into its own method
                bytes.AddRange(SerializeShaderPaths(material));

                return bytes.ToArray();
            }
            else 
            { 
                //Debug.WriteLine("!!! Serialization fro this Material-Type is not supported !!!"); 
                return null; 
            }

        }
        /// <summary> Deserializes the Material from an array of bytes. </summary>
        /// <param name="bytes"></param>
        public static Material DeserializeMaterial(byte[] bytes)
        {
            Type t = Material.GetMatByID(bytes[0]);
            //Debug.WriteLine("\n" + "Material-Type: " + t.Name + ", Material-ID: " + bytes[0].ToString());

            int readPos = 0; //indicates how far along through the byte-array we have deserialized
            readPos = 1; //1 because it's the comp-id, which is 1 byte long

            if (t == typeof(BasicPhongMaterial))
            {
                //structure-shader: [Mat-ID(byte)_1byte]->[VertCode-Len(int32)_4bytes]->[VertCode(string)_ given]->[FragCode-Len(int32)_4bytes]->[FragCode(string)_ given]
                //structure-vars: [^shadercode]->[ambient(3*float)_12bytes]->[diffuse(3*float)_12bytes]->[specular(3*float)_12bytes]->[shininess(float)_4bytes]

                if (bytes[0] != Material.GetMatID(typeof(BasicPhongMaterial))) 
                { 
                    ////Debug.WriteLine("!!! Deserialization failed !!!"); 
                    return null; }

                //shaders need to be deserialized for every material, so it's seperated into its own method
                DeserializeShaderPaths(bytes, readPos, out string vertPath, out string fragPath, out int newReadPos);
                readPos = newReadPos;

                //ambient-color
                byte[] ambientBytes = ArrayUtils.GetBetweenXAndYExcl(bytes, readPos, readPos + sizeof(float)*3);
                Vector3 ambient = VarUtils.BytesToVector3(ambientBytes);
                readPos += sizeof(float) * 3;

                //diffuse-color
                byte[] diffuseBytes = ArrayUtils.GetBetweenXAndYExcl(bytes, readPos, readPos + sizeof(float) * 3);
                Vector3 diffuse = VarUtils.BytesToVector3(diffuseBytes);
                readPos += sizeof(float) * 3;

                //specular-color
                byte[] specularBytes = ArrayUtils.GetBetweenXAndYExcl(bytes, readPos, readPos + sizeof(float) * 3);
                Vector3 specular = VarUtils.BytesToVector3(specularBytes);
                readPos += sizeof(float) * 3;

                //shininess
                byte[] shininessBytes = ArrayUtils.GetBetweenXAndYExcl(bytes, readPos, readPos + sizeof(float));
                float shininess = VarUtils.BytesToFloat(shininessBytes);
                readPos += sizeof(float);

                //Debug.WriteLine("  |VertShader-Path-Full: '" + DataManager.assetsFilePath + vertPath + "'");
                //Debug.WriteLine("  |FragShader-Path-Full: '" + DataManager.assetsFilePath + fragPath + "'" + "\n");

                return new BasicPhongMaterial(DataManager.assetsPath + vertPath, DataManager.assetsPath + fragPath, ambient, diffuse, specular, shininess);
            }
            else if (t == typeof(UnlitMaterial))
            {
                //structure-shader: [Mat-ID(byte)_1byte]->[VertCode-Len(int32)_4bytes]->[VertCode(string)_ given]->[FragCode-Len(int32)_4bytes]->[FragCode(string)_ given]

                if (bytes[0] != Material.GetMatID(typeof(UnlitMaterial)))
                {
                    //Debug.WriteLine("!!! Deserialization failed !!!"); 
                    return null;
                }

                //shaders need to be deserialized for every material, so it's seperated into its own method
                DeserializeShaderPaths(bytes, readPos, out string vertPath, out string fragPath, out int newReadPos);
                readPos = newReadPos;

                //Debug.WriteLine("  |VertShader-Path-Full: '" + DataManager.assetsFilePath + vertPath + "'");
                //Debug.WriteLine("  |FragShader-Path-Full: '" + DataManager.assetsFilePath + fragPath + "'" + "\n");

                //object-color
                byte[] colorBytes = ArrayUtils.GetBetweenXAndYExcl(bytes, readPos, readPos + sizeof(float) * 3);
                Vector3 color = VarUtils.BytesToVector3(colorBytes);
                readPos += sizeof(float) * 3;

                return new UnlitMaterial(DataManager.assetsPath + vertPath, DataManager.assetsPath + fragPath, color);
            }
            else if(t == typeof(LightMaterial))
            {
                //structure-shader: [Mat-ID(byte)_1byte]->[VertCode-Len(int32)_4bytes]->[VertCode(string)_ given]->[FragCode-Len(int32)_4bytes]->[FragCode(string)_ given]

                if (bytes[0] != Material.GetMatID(typeof(LightMaterial))) 
                { 
                    //Debug.WriteLine("!!! Deserialization failed !!!"); 
                    return null; 
                }

                //shaders need to be deserialized for every material, so it's seperated into its own method
                DeserializeShaderPaths(bytes, readPos, out string vertPath, out string fragPath, out int newReadPos);
                readPos = newReadPos;

                //Debug.WriteLine("  |VertShader-Path-Full: '" + DataManager.assetsFilePath + vertPath + "'");
                //Debug.WriteLine("  |FragShader-Path-Full: '" + DataManager.assetsFilePath + fragPath + "'" + "\n");


                return new LightMaterial(DataManager.assetsPath + vertPath, DataManager.assetsPath + fragPath);
            }
            else 
            {
                Type T = Material.GetMatByID(bytes[0]);
                //Debug.WriteLine("!!! Matrial (" + T.FullName + ") not Deserializable !!!");
                return null;
            }


        }

        /// <summary> Serializes the Shader-Code into an array of bytes. </summary>
        /// <param name="mat"></param>
        private static byte[] SerializeShaderPaths(Material mat)
        {
            //structure: [Mat-ID(byte)_1byte]->[VertCode-Len(int32)_4bytes]->[VertCode(string)_ given]->[FragCode-Len(int32)_4bytes]->[FragCode(string)_ given]

            //Debug.WriteLine(" |Serializer.SerializeShaderPaths ");

            List<byte> bytes = new List<byte>();

            //vert-shader
            byte[] vertBytes = VarUtils.VarToBytes(VarUtils.GetStringAfterKey(mat.vertShaderFilePath, @"assets"));
            //Debug.WriteLine("  |VertShader FilePath: '" + VarUtils.GetStringAfterKey(mat.vertShaderFilePath, @"assets") + "'");
            bytes.AddRange(VarUtils.VarToBytes(vertBytes.Length)); //vertCode-len
            bytes.AddRange(vertBytes); //the shaders vertex-code

            //frag-shader
            byte[] fragBytes = VarUtils.VarToBytes(VarUtils.GetStringAfterKey(mat.fragShaderFilePath, @"assets"));
            //Debug.WriteLine("  |FragShader FilePath: '" + VarUtils.GetStringAfterKey(mat.fragShaderFilePath, @"assets") + "'"); 
            bytes.AddRange(VarUtils.VarToBytes(fragBytes.Length)); //fragCode-len
            bytes.AddRange(fragBytes); //the shaders fragment-code

            return bytes.ToArray();
        }
        /// <summary> Desrializes the Shader-Code from an array of bytes. </summary>
        /// <param name="bytes"></param>
        /// <param name="readPos"></param>
        /// <param name="vertPath"></param>
        /// <param name="fragPath"></param>
        /// <param name="newReadPos"></param>
        private static void DeserializeShaderPaths(byte[] bytes, int readPos, out string vertPath, out string fragPath, out int newReadPos)
        {
            //structure: [Mat-ID(byte)_1byte]->[VertPath-Len(int32)_4bytes]->[VertPath(string)_ given]->[FragPath-Len(int32)_4bytes]->[FragPath(string)_ given]

            //Debug.WriteLine("\n |Deserialize ShaderPaths: \n  |readPos: " + readPos.ToString());
            //deserialize vert-shader
            //get the bytes after the mat-id that specify the length of the given vert-shader
            byte[] vertLenByte = ArrayUtils.GetBetweenXAndYExcl(bytes, readPos, readPos + sizeof(int));
            int vertLen = VarUtils.BytesToInt(vertLenByte);
            readPos += sizeof(int); //we've read the length of the string, given as an int
            //Debug.WriteLine("  |VertShader-Len(" + vertLen.ToString() + ")");

            byte[] vertBytes = ArrayUtils.GetBetweenXAndYExcl(bytes, readPos, readPos + vertLen); //get the bytes after the vert-length indicator and get as many as the vert-len indicator defined
            vertPath = VarUtils.BytesToString(vertBytes);
            readPos += vertLen;
            //Debug.WriteLine("  |VertShader-Path: '" + vertPath + "'");

            //deserialize frag-shader
            //get the bytes after the mat-id that specify the length of the given frag-shader
            byte[] fragLenByte = ArrayUtils.GetBetweenXAndYExcl(bytes, readPos, readPos + sizeof(int));
            int fragLen = VarUtils.BytesToInt(fragLenByte);
            readPos += sizeof(int); //we've read the length of the string, given as an int
            //Debug.WriteLine("  |FragShader-Len(" + fragLen.ToString() + ")");
            
            byte[] fragBytes = ArrayUtils.GetBetweenXAndYExcl(bytes, readPos, readPos + fragLen); //get the bytes after the vert-length indicator and get as many as the vert-len indicator defined
            fragPath = VarUtils.BytesToString(fragBytes);
            readPos += fragLen;
            //Debug.WriteLine("  |FragShader-Path: '" + fragPath + "'" + "\n");

            newReadPos = readPos;
        }

        */
        #endregion
    }
}
