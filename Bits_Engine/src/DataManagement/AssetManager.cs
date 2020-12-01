using BitsCore.DataManagement;
using BitsCore.Debugging;
using BitsCore.ObjectData;
using BitsCore.ObjectData.Components;
using BitsCore.ObjectData.Materials;
using BitsCore.ObjectData.Shaders;
using BitsCore.SceneManagement;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;

namespace BitsCore
{
    public static class AssetManager
    {
        class Asset
        {
            public Type type;
            public bool loaded;
            public object asset;
            public string path;

            public Asset(Type _type)
            {
                this.type = _type;
                this.loaded = false;
                this.asset = null;
            }
            public Asset(Type type, bool loaded, object asset, string path)
            {
                this.type = type;
                this.loaded = loaded;
                this.asset = asset;
                this.path = path;
            }
        }

        static Dictionary<string, Asset> meshDict = new Dictionary<string, Asset>();
        static Dictionary<string, Asset> materialDict = new Dictionary<string, Asset>();
        static Dictionary<string, Asset> textureDict = new Dictionary<string, Asset>();
        static Dictionary<string, Asset> shaderDict = new Dictionary<string, Asset>();
        static Dictionary<string, Asset> shaderFilesDict = new Dictionary<string, Asset>();
        static Dictionary<string, Asset> sceneFilesDict = new Dictionary<string, Asset>();

        /// <summary> Initialize the <see cref="AssetManager"/> </summary>
        internal static void Init()
        {
            GatherAssetReferences();
        }

        /// <summary> Gets all references to the asset-files in the <see cref="DataManager.assetsPath"/> 'assets' folder. </summary>
        private static void GatherAssetReferences()
        {
            string[] fileNames = Directory.GetFiles(DataManager.assetsPath, "*.*", SearchOption.AllDirectories);
            foreach (string path in fileNames)
            {
                string fname = Path.GetFileName(path);
                string ext = fname.Substring(fname.IndexOf(".")); //extension after the period-symbol
                string key = fname.Substring(0, fname.IndexOf(".")); //remove extension


                if (ext == ".obj")
                {
                    meshDict.Add(key, new Asset(typeof(Mesh), false, null, path));
                }
                else if (ext == ".png" || ext == ".jpg" || ext == ".bmp")
                {
                    if (textureDict.ContainsKey(key)) { throw new Exception("Filename Duplicates, can't add multiple files of the same name to the AssetManager. Rename files."); }
                    textureDict.Add(key, new Asset(typeof(Texture), false, null, path));
                }
                else if(ext == ".vert" || ext == ".frag")
                {
                    if (shaderFilesDict.ContainsKey(key)) { throw new Exception("Filename Duplicates, can't add multiple files of the same name to the AssetManager. Rename files."); }
                    shaderFilesDict.Add(fname, new Asset(typeof(string), false, null, path));
                }
                else if(ext == ".scene")
                {
                    if (sceneFilesDict.ContainsKey(key)) { throw new Exception("Filename Duplicates, can't add multiple files of the same name to the AssetManager. Rename files."); }
                    sceneFilesDict.Add(key, new Asset(typeof(byte), false, null, path));
                }
            }
        }

        public static bool HasAsset(string key)
        {
            return meshDict.ContainsKey(key) || materialDict.ContainsKey(key) || textureDict.ContainsKey(key) || shaderDict.ContainsKey(key) || shaderFilesDict.ContainsKey(key) || sceneFilesDict.ContainsKey(key);
        }

        #region GET_METHODS
        /// <summary> Get the path to the scene with the specified name. </summary>
        /// <param name="name"> Name of the asset. </param>
        internal static string GetScenePath(string name)
        {
            //asset not gathered yet
            if (!sceneFilesDict.ContainsKey(name)) { BBug.Log("!!! No Scene of name: '" + name + "' in AssetManager !!!"); return null; }

            return sceneFilesDict[name].path;
        }

        /// <summary> Get the mesh with the specified name. </summary>
        /// <param name="name"> Name of the asset. </param>
        public static Mesh GetMesh(string name)
        {
            //asset not gathered yet
            if (!meshDict.ContainsKey(name)) { BBug.Log("!!! No Mesh of specified name in AssetManager !!!"); return null; }

            //asset already loaded
            if (meshDict[name].loaded) { return (Mesh)meshDict[name].asset; }

            //asset not loaded yet
            Mesh m = ModelImporter.Import(meshDict[name].path, false);
            if (m == null) { throw new Exception("!!! GameObject.CreateFromFile -> ModelImporter.Import returned null !!!"); }


            return m;
        }

        /// <summary> Get the material with the specified name. </summary>
        /// <param name="name"> Name of the asset. </param>
        public static Material GetMaterial(string name)
        {
            //asset not gathered yet
            if (!materialDict.ContainsKey(name)) { BBug.Log("!!! No Material of name: '" + name + "' in AssetManager !!!"); return null; }

            //asset already loaded
            if (materialDict[name].loaded) { return (Material)materialDict[name].asset; }
            else
            {
                throw new NotImplementedException();

                //Texture t = ImageImporter.LoadTexture(materialDict[name].path);
                //if (t == null) { throw new Exception("!!! AssetManager.GetTexture() -> ImageImporter.LoadTexture() returned null !!!"); }
                //materialDict[name].asset = t;
                //materialDict[name].loaded = true;
                //
                //return t;
            }
        }

        /// <summary> Get the Texture with the specified name. </summary>
        /// <param name="name"> Name of the asset. </param>
        public static Texture GetTexture(string name)
        {
            //asset not gathered yet
            if (!textureDict.ContainsKey(name)) { BBug.Log("!!! No Texture of name: '" + name + "' in AssetManager !!!"); return null; }

            //asset already loaded
            if (textureDict[name].loaded) { return (Texture)textureDict[name].asset; }
            else
            {
                Texture t = ImageImporter.LoadTexture(textureDict[name].path);
                if (t == null) { throw new Exception("!!! AssetManager.GetTexture() -> ImageImporter.LoadTexture() returned null !!!"); }
                textureDict[name].asset = t;
                textureDict[name].loaded = true;

                return t;
            }
        }

        /// <summary> Get the shader with the specified name. </summary>
        /// <param name="name"> Name of the asset. </param>
        public static Shader GetShader(string name)
        {
            //asset not gathered yet
            if (!shaderDict.ContainsKey(name)) { BBug.Log("!!! No Shader of name: '" + name + "' in AssetManager !!!"); return null; }

            //asset already loaded
            if (shaderDict[name].loaded) { return (Shader)shaderDict[name].asset; }
            else
            {
                throw new Exception("Shader doesn't exist in the AssetManager");
            }
        }
        #endregion

        #region ADD_METHODS
        /// <summary> Add the asset of the specified type with the specified name. </summary>
        /// <param name="name"> Name of the asset. </param>
        /// <typeparam name="T"> Type of the asset. </typeparam>
        public static bool Add<T>(string name, string path)
        {
            if (typeof(T) == typeof(Mesh))
            {
                throw new NotImplementedException();
            }
            else if (typeof(T) == typeof(Material))
            {
                return AddMaterial(name, path);
            }
            else if (typeof(T) == typeof(Texture))
            {
                return AddTexture(name, path);
            }
            else if (typeof(T) == typeof(Shader))
            {
                throw new NotImplementedException();
            }

            //if none of the if-statements triggered
            BBug.Log("!!! Couldn't add Asset in AssetManager!!!");
            return false;
        }
        /// <summary> Add the Mesh with the specified name. </summary>
        /// <param name="name"> Name of the asset. </param>
        /// <param name="path"> Path to the asset. </param>
        public static bool AddMesh(string name, string path)
        {
            if (meshDict.ContainsKey(name)) { BBug.Log("!!! AssetManager already contains this Asset !!!"); return true; }

            //asset not loaded yet
            meshDict.Add(name, new Asset(typeof(Mesh), false, null, path));

            return true;
        }
        /// <summary> Add the Material with the specified name. </summary>
        /// <param name="name"> Name of the asset. </param>
        /// <param name="path"> Path to the asset. </param>
        public static bool AddMaterial(string name, string path)
        {
            throw new NotImplementedException();

            if (materialDict.ContainsKey(name)) { BBug.Log("!!! AssetManager already contains this Asset !!!"); return true; }

            //asset not loaded yet
            materialDict.Add(name, new Asset(typeof(Material), false, null, DataManager.assetsPath + path));

            return true;
        }
        public static bool AddMaterial(string name, Material mat)
        {
            if (materialDict.ContainsKey(name)) { BBug.Log("!!! AssetManager already contains this Asset !!!"); return true; }

            //add to material-dicitonary
            materialDict.Add(name, new Asset(typeof(Material), true, mat, "doesnt have a file"));

            return true;

        }

        /// <summary> Add the Texture with the specified name. </summary>
        /// <param name="name"> Name of the asset. </param>
        /// <param name="path"> Path to the asset. </param>
        public static bool AddTexture(string name, string path)
        {
            if(textureDict.ContainsKey(name)) { BBug.Log("!!! AssetManager already contains this Asset !!!"); return true; }

            //asset not loaded yet
            textureDict.Add(name, new Asset(typeof(Texture), false, null, DataManager.assetsPath + path));

            return true;
        }
        /// <summary> Add the Shader with the specified name. </summary>
        /// <param name="name"> Name of the asset. </param>
        /// <param name="path"> Path to the asset. </param>
        public static bool AddShader(string name, string nameVert, string nameFrag)
        {
            if (shaderDict.ContainsKey(name)) { BBug.Log("!!! AssetManager already contains this Asset !!!"); return true; }
            if (!shaderFilesDict.ContainsKey(nameVert)) { throw new Exception("!!! AssetManager doesn't contain this Asset !!!");; }
            if (!shaderFilesDict.ContainsKey(nameFrag)) { throw new Exception("!!! AssetManager doesn't contain this Asset !!!");; }

            //create shader

            Shader shader = new Shader(shaderFilesDict[nameVert].path, shaderFilesDict[nameFrag].path);
            shader.shaderAssetKey = name;
            shader.vertAssetKey = nameVert;
            shader.fragAssetKey = nameFrag;

            shaderDict.Add(name, new Asset(typeof(Shader), true, shader, "no path for shaders"));

            return true;
        }
        //get font
        /*
        /// <summary> Get the font with the specified name. </summary>
        /// <param name="name"></param>
        public static Font GetFont(string name)
        {
            return null;
        }
        */
        #endregion

        #region Reload
        public static void ReloadShader(string name)
        {
            if (!shaderDict.ContainsKey(name)) { BBug.Log("!!! AssetManager doesn't contain this Asset, couldn't reload !!!"); return; }

            //reloads shader
            string vertName = ((Shader)shaderDict[name].asset).vertexFilePath; vertName = Path.GetFileName(vertName);
            string fragName = ((Shader)shaderDict[name].asset).fragmentFilePath; fragName = Path.GetFileName(fragName);
            DataManager.CopyFile(DataManager.projectFilePath + @"assets\Shaders\Vertex_Shaders\" + vertName, DataManager.assetsPath + @"\Shaders\Vertex_Shaders\" + vertName, true);
            DataManager.CopyFile(DataManager.projectFilePath + @"assets\Shaders\Fragment_Shaders\" + fragName, DataManager.assetsPath + @"\Shaders\Fragment_Shaders\" + fragName, true);
            ((Shader)shaderDict[name].asset).Load();
        }
        public static void ReloadTexture(string name)
        {
            if(!textureDict.ContainsKey(name)) { BBug.Log("!!! AssetManager doesn't contain this Asset, couldn't reload !!!"); return; }

            //reload
            string projectPath = textureDict[name].path.Substring(textureDict[name].path.IndexOf(@"assets\"));
            BBug.Log("Texture Path: " + DataManager.projectFilePath + projectPath);
            BBug.Log("Texture Dest: " + textureDict[name].path);
            DataManager.CopyFile(DataManager.projectFilePath + projectPath, textureDict[name].path, true);
            textureDict[name].asset = ImageImporter.LoadTexture(textureDict[name].path);
            textureDict[name].loaded = true;

            throw new NotImplementedException();
        }
        public static void ReloadMaterial(string name)
        {
            if (!materialDict.ContainsKey(name)) { BBug.Log("!!! AssetManager doesn't contain this Asset, couldn't reload !!!"); return; }
            //else if(!materialDict[name].loaded == false) { BBug.Log("!!! Material not loaded, couldn't reload !!!"); return; }

            //reload shader
            //reloads shader
            string vertName = ((Material)materialDict[name].asset).shader.vertexFilePath; vertName = Path.GetFileName(vertName);
            string fragName = ((Material)materialDict[name].asset).shader.fragmentFilePath; fragName = Path.GetFileName(fragName);
            DataManager.CopyFile(DataManager.projectFilePath + @"assets\Shaders\Vertex_Shaders\" + vertName, DataManager.assetsPath + @"\Shaders\Vertex_Shaders\" + vertName, true);
            DataManager.CopyFile(DataManager.projectFilePath + @"assets\Shaders\Fragment_Shaders\" + fragName, DataManager.assetsPath + @"\Shaders\Fragment_Shaders\" + fragName, true);
            ((Material)materialDict[name].asset).shader.Load();

            //set binding-flags to capture all vars (instance: int x;(outside any function) nonpublic: private int x; public: public int x;)
            BindingFlags bindingFlags =     BindingFlags.Instance |
                                            BindingFlags.NonPublic |
                                            BindingFlags.Public;
            //get all vars from the material
            FieldInfo[] vars = materialDict[name].asset.GetType().GetFields(bindingFlags);
            
            //check if the material has textures and if so reload them
            //foreach(FieldInfo field in vars)
            //{
            //    if (field.FieldType == typeof(Texture))
            //    {
            //        Texture texture = (Texture)field.GetValue(materialDict[name].asset);
            //        ReloadTexture(Path.GetFileNameWithoutExtension(texture.filePath)); //reload by using the textures own filePath var
            //    }
            //}
        }
        #endregion
    }
}
