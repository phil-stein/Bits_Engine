using BitsCore.ObjectData.Components;
using BitsCore.ObjectData.Materials;
using System;
using System.Numerics;
using System.Threading.Tasks;
using BitsCore.DataManagement;
using BitsCore.Utils;
using BitsCore.Debugging;

namespace BitsCore.ObjectData
{
    
    /// <summary> The entity-class, attach components to make a mesh get rendered, emit light, etc. </summary>
    public class GameObject
    {
        #region VARIABLES
        //pos, rot, scale of the GO
        public Transform transform { get; private set; }
        //all components attached to the GO
        public Component[] components { get; private set; }

        /// <summary> The tag attached to the GameObject. </summary>
        public string tag { get; private set; }
        #endregion

        #region CONSTRUCTOR
        /// <summary> Generates a GameObject. </summary>
        /// <param name="_transform">
        ///     The Transform attached to the GameObject, that determines the Position, Rotation and Scale of the GameObject.
        /// </param>
        public GameObject(Transform _transform, string _tag)
        {
            this.transform = _transform;
            transform.SetGameObj(this);

            this.components = new Component[0]; //just to init/assign the var

            tag = _tag == "" ? "empty" : _tag;
        }
        #endregion

        #region COMPONENTS
        //Components----------------------------------
        /// <summary> Add a Component to the GameObject. </summary>
        /// <param name="comp"> The Component to add. </param>
        public void AddComp(Component comp)
        {
            if(components.Length < 1) { components = new Component[1]; }
            if(components[components.Length -1] != null) 
            { 
                //resizes array, local val needed as ref's can't be global-vars
                Component[] comps = components; 
                Array.Resize(ref comps, components.Length +1); 
                components = comps; 
            }
            if(components[components.Length -1] != null) { BBug.Log("!!!Could not assign component, array-slot already assigned!!!"); return; }
            foreach(Component c in components) 
            { 
                if (c == null) { continue; } 
                if (comp.GetType() == c.GetType()) 
                {
                    BBug.Log("!!!Could not assign component, GO already contained component of the same type!!!"); return; 
                } 
            }
            components[components.Length -1] = comp;
            comp.OnAddEvent(this); //sets the 'gameObject' var in the component and calls the OnAdd() function
        }

        /// <summary> Remove the Component of the given type from the GameObject. </summary>
        /// <typeparam name="T"> Type of the Component to be removed. </typeparam>
        public void RemoveComp<T>() 
        {
            //Debug.WriteLine("RemoveComp() not implemented");

            if (!HasComp<T>()) { System.Diagnostics.Debug.WriteLine("!!! GameObject.RemoveComp() was called, but specified Comp wasn't attached to the GameObject !!!"); return; }

            int index = Array.IndexOf(components, GetComp<T>());

            //calls OnRemove()
            components[index].OnRemoveEvent();
            
            components = ArrayUtils.RemoveAtX(components, index);

            if (HasComp<T>()) { BBug.Log("!!! GameObject.RemoveComp<>() failed !!!"); }
        }
        /// <summary> Remove the Component at the given index from the GameObject. </summary>
        /// <param name="index"> Index of the Component to be removed. </param>
        public void RemoveComp(int index)
        {
            if(components.Length < index - 1) { System.Diagnostics.Debug.WriteLine("!!! GameObject.RemoveComp() index outside of GameObject.components[] range !!!"); return; }

            //calls OnRemove()
            components[index].OnRemoveEvent();

            components = ArrayUtils.RemoveAtX(components, index);
        }

        /// <summary> Get a Component attached to the GameObject by index. </summary>
        /// <param name="index"> The index in the GameObjects components Array of Components. </param>
        /// <param name="t"> The Type of Component to be returned. </param>
        public Component GetComp(int index, Type t)
        {
            if(components[index].GetType() != t) { System.Diagnostics.Debug.WriteLine("!!!Component Type mismatch, can't return requested Component!!!"); }
            return components[index];
        }

        /// <summary> Get the Component attached to the GameObject of Type T. </summary>
        /// <param name="T"> Type of the Component to be returned. </param>
        public T GetComp<T>()
        {
            if(!typeof(T).IsSubclassOf(typeof(Component))) { throw new Exception("!!!Requested Type doesn't derive from Component!!!");  }
            //if(!HasComp<T>()) { return null; }

            Component component = null;
            foreach (Component comp in components)
            {
                if (comp.GetType() == typeof(T)) { component = comp; break; }
            }
            if (component == null) { BBug.Log("!!!No Component of requested type in GameObject!!!");  return default(T); }
            return (T)Convert.ChangeType(component, typeof(T));
        }

        /// <summary> Check if GameObject has a Component of Type T. </summary>
        /// <param name="T"> Type of the Component to be checked. </param>
        public bool HasComp<T>()
        {
            bool result = false;
            foreach(Component c in components)
            {
                if(c.GetType() == typeof(T)) { result = true; break; }
            }
            return result;
        }
        #endregion

        #region METHODS
        //Methods-----------------------------------
        /// <summary> Generates a Primitive-Cube GameObject for each vertex in this GameObject. </summary>
        public GameObject[] VerticesCubes()
        {
            //generates a small cube for each vertex in the GO gameObjects[objID]
            GameObject[] vertObjs;
            float[] verts = GetComp<Model>().GetVerticesWorldPosition();
            for (int i = 0; i < verts.Length - 1; i += 8)
            {
                //Debug.WriteLine("X: " + verts[i+0] + ", Y: " + verts[i + 1] + ", Z: " + verts[i + 2]);
            }
            vertObjs = new GameObject[verts.Length / 8];
            for (int n = 0, i = 0; i < verts.Length; i += 8)
            {
                Vector3 pos = new Vector3(verts[i + 0], verts[i + 1], verts[i + 2]);
                vertObjs[n] = GameObject.CreatePrimitive(Primitve.Cube, pos, Vector3.Zero, Vector3.One * .05f, AssetManager.GetMaterial("Mat_Default"));
                //vertObjs[n].transform.Move(gameObjects[objID].transform.position);
                //vertObjs[n].transform.Rotate(gameObjects[objID].transform.rotation);
                //vertObjs[n].transform.Scale(gameObjects[objID].transform.scale);
                n++;
            }
            //takes care of vao, vbo, ebo for all GO's
            foreach (GameObject go in vertObjs)
            {
                go.GetComp<Mesh>().GenOpenGLData();
            }

            BBug.Log("\n |VerticesCubes() added " + vertObjs.Length.ToString() + " cubes.\n");
            return vertObjs;
        }

        /// <summary> Sets the transform of the GameObject to the given Transform. </summary>
        /// <param name="newTrans"> The Transform the GameObjects transform will be set to. </param>
        public void SetTransform(Transform newTrans)
        {
            transform = newTrans;
        }

        /// <summary> Sets the tag of the GameObject to the given string. </summary>
        /// <param name="newTag"> The string the GameObjects tag will be set to. </param>
        public void SetTag(string newTag)
        {
            tag = newTag;
        }
        #endregion

        #region CREATE_AND_LOAD
        //Create&Load-Methods-----------------------------------
        /// <summary>
        /// Generates a GameObject with a DirectionalLight and Mesh Component attached. 
        /// <para> Mesh Component is an arrow. </para>
        /// </summary>
        /// <param name="type"> Type of the LightSource. </param>
        /// <param name="pos"> Position of the LightSource. </param>
        /// <param name="rot"> Rotation of the LightSource. </param>
        /// <param name="scale"> Scale of the LightSource. </param>
        /// <param name="mat"> Material of the LightSource. <para> Remember to attach a Material meant for LightSources </para> </param>
        /// <param name="ambient"> Ambient-Color of the LightSource. </param>
        /// <param name="diffuse"> Diffuse-Color of the LightSource. </param>
        /// <param name="specular"> Specular-Color of the LightSource. </param>
        /// <param name="strength"> Strength of the LightSource. </param>
        public static GameObject CreateDirectionalLight(Vector3 pos, Vector3 rot, Vector3 scale, Material mat, Vector3 diffuse, float strength)
        {

            //GameObject obj = new GameObject(new Transform(pos, rot, scale), "LightSource_Light"); 
            //GameObject obj = Serializer.ReadGameObjectFromFile(DataManager.assetsPath, @"\SerializedData\Models\"); //Pyramids\pyramid_bevel01
            //obj.SetTransform(new Transform(pos, rot, scale));
            GameObject obj = CreateFromFile(pos, rot, scale, mat, @"arrow_down");
            obj.SetTag("LightSource_DirectionalLight");
            obj.GetComp<Model>().SetMaterial(mat);

            Vector3 ambient = diffuse * 0.3f;
            Vector3 specular = diffuse * 1.2f;
            DirectionalLight lightSource = new DirectionalLight(ambient, diffuse, specular, strength);
            obj.AddComp(lightSource);
            return obj;
        }

        /// <summary>
        /// Generates a GameObject with a PointLight and Mesh Component attached. 
        /// <para> Mesh Component is a sphere. </para>
        /// </summary>
        /// <param name="type"> Type of the LightSource. </param>
        /// <param name="pos"> Position of the LightSource. </param>
        /// <param name="rot"> Rotation of the LightSource. </param>
        /// <param name="scale"> Scale of the LightSource. </param>
        /// <param name="mat"> Material of the LightSource. <para> Remember to attach a Material meant for LightSources </para> </param>
        /// <param name="ambient"> Ambient-Color of the LightSource. </param>
        /// <param name="diffuse"> Diffuse-Color of the LightSource. </param>
        /// <param name="specular"> Specular-Color of the LightSource. </param>
        /// <param name="strength"> Strength of the LightSource. </param>
        public static GameObject CreatePointLight(Vector3 pos, Vector3 rot, Vector3 scale, Material mat, Vector3 diffuse, float strength, float constant = 1.0f, float linear = 0.09f, float quadratic = 0.032f)
        {
            GameObject obj = CreateFromFile(pos, rot, scale * .3f, mat, @"lightbulb");
            obj.SetTag("LightSource_PointLight");
            obj.GetComp<Model>().SetMaterial(mat);


            Vector3 ambient = diffuse * 0.3f;
            Vector3 specular = diffuse * 1.2f;
            PointLight lightSource = new PointLight(ambient, diffuse, specular, strength, constant, linear, quadratic);
            obj.AddComp(lightSource);
            return obj;
        }

        /// <summary>
        /// Generates a GameObject with a SpotLight and Mesh Component attached. 
        /// <para> Mesh Component is a sphere. </para>
        /// </summary>
        /// <param name="type"> Type of the LightSource. </param>
        /// <param name="pos"> Position of the LightSource. </param>
        /// <param name="rot"> Rotation of the LightSource. </param>
        /// <param name="scale"> Scale of the LightSource. </param>
        /// <param name="mat"> Material of the LightSource. <para> Remember to attach a Material meant for LightSources </para> </param>
        /// <param name="ambient"> Ambient-Color of the LightSource. </param>
        /// <param name="diffuse"> Diffuse-Color of the LightSource. </param>
        /// <param name="specular"> Specular-Color of the LightSource. </param>
        /// <param name="strength"> Strength of the LightSource. </param>
        public static GameObject CreateSpotLight(Vector3 pos, Vector3 rot, Vector3 scale, Material mat, Vector3 diffuse, float strength, float outerCutOff = 17.5f, float innerCutOff = 12.5f, float constant = 1.0f, float linear = 0.09f, float quadratic = 0.032f)
        {
            GameObject obj = CreateFromFile(pos, rot, scale * .5f, mat, @"flashlight");
            obj.SetTag("LightSource_SpotLight");
            obj.GetComp<Model>().SetMaterial(mat);


            Vector3 ambient = diffuse * 0.3f;
            Vector3 specular = diffuse * 1.2f;
            SpotLight lightSource = new SpotLight(ambient, diffuse, specular, strength, innerCutOff, outerCutOff, constant, linear, quadratic);
            obj.AddComp(lightSource);
            return obj;
        }

        /// <summary> Generates a GameObject with a Mesh Component attached. </summary>
        /// <param name="type"> 
        ///     Type of Primitive. 
        ///     <para> All GameObject.Primitive enums are accepted. </para>
        /// </param>
        /// <param name="pos"> Position of the GameObject. </param>
        /// <param name="rot"> Rotation of the GameObject. </param>
        /// <param name="scale"> Scale of the GameObject. </param>
        /// <param name="mat"> Material of the GameObject. </param>
        public static GameObject CreatePrimitive(Primitve type, Vector3 pos, Vector3 rot, Vector3 scale, Material mat)
        {
            float col = 1f; 
            if (type == Primitve.Plane)
            {
                /*
                verticesPlane = new float[]
                {
                    //XYZ Coordinates, Normal, UV Coordinates
                    0.5f, 0.5f, 0f, 0f, 0f, 0f, 1f, 1f, //top right
                    0.5f, -0.5f, 0f, 0f, 0f, 0f, 1f, 0f, //bottom right
                    -0.5f, -0.5f, 0f, 0f, 0f, 0f, 0f, 0f, //bottom left
                    -0.5f, 0.5f, 0f, 0f, 0f, 0f, 0f, 1f, //top left
                };
                */

                float[] verticesPlane = new float[]
                {
                    //XYZ Coordinates, Normal, UV Coordinates
                    0.5f, 0f, 0.5f, 0f, 0f, 0f, 1f, 1f, //top right
                    0.5f, 0f, -0.5f, 0f, 0f, 0f, 1f, 0f, //bottom right
                    -0.5f, 0f, -0.5f, 0f, 0f, 0f, 0f, 0f, //bottom left
                    -0.5f, 0f, 0.5f, 0f, 0f, 0f, 0f, 1f, //top left
                };


                uint[] indicesPlane = new uint[]
                {
                    0, 1, 3,
                    1, 2, 3,
                };
                GameObject obj = new GameObject(new Transform(pos, rot, scale), "Plane");
                Mesh mesh = new Mesh(verticesPlane, indicesPlane);
                Model model = new Model(mesh, mat, "Plane_Mesh");
                obj.AddComp(model);
                obj.GetComp<Model>().CalcNormals(true);
                return obj;
            }
            else if (type == Primitve.Cube)
            {
                //the array of vertices
                float[] verticesCube = new float[]
                {
                    //XYZ Coordinates, RGB Color, UV Coordinates
                    //front
                    0.5f, 0.5f, 0.5f, 1f, 1f, 0f, 99f, 99f, //bottom right
                    0.5f, -0.5f, 0.5f, 1f, -1f, 0f, 99f, 99f, //top right
                    -0.5f, -0.5f, 0.5f, -1f, -1f, 0f, 99f, 99f, //top left
                    -0.5f, 0.5f, 0.5f, -1f, 1f, 0f, 99f, 99f, //bottom left
                
                    //back
                    0.5f, 0.5f, -0.5f, 0f, 1f, 0f, 99f, 99f, //bottom right
                    0.5f, -0.5f, -0.5f, 0f, -1f, 0f, 99f, 99f, //top right
                    -0.5f, -0.5f, -0.5f, 0f, -1f, 0f, 99f, 99f, //top left
                    -0.5f, 0.5f, -0.5f, 0f, 1f, 0f, 99f, 99f //bottom left
                };
                
                float[] verts = new float[]
                {
                    -0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,
                     0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,
                     0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,
                     0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,
                    -0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,
                    -0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,

                    -0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f,
                     0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f,
                     0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f,
                     0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f,
                    -0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f,
                    -0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f,

                    -0.5f,  0.5f,  0.5f, -1.0f,  0.0f,  0.0f,
                    -0.5f,  0.5f, -0.5f, -1.0f,  0.0f,  0.0f,
                    -0.5f, -0.5f, -0.5f, -1.0f,  0.0f,  0.0f,
                    -0.5f, -0.5f, -0.5f, -1.0f,  0.0f,  0.0f,
                    -0.5f, -0.5f,  0.5f, -1.0f,  0.0f,  0.0f,
                    -0.5f,  0.5f,  0.5f, -1.0f,  0.0f,  0.0f,

                     0.5f,  0.5f,  0.5f,  1.0f,  0.0f,  0.0f,
                     0.5f,  0.5f, -0.5f,  1.0f,  0.0f,  0.0f,
                     0.5f, -0.5f, -0.5f,  1.0f,  0.0f,  0.0f,
                     0.5f, -0.5f, -0.5f,  1.0f,  0.0f,  0.0f,
                     0.5f, -0.5f,  0.5f,  1.0f,  0.0f,  0.0f,
                     0.5f,  0.5f,  0.5f,  1.0f,  0.0f,  0.0f,

                    -0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,
                     0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,
                     0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,
                     0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,
                    -0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,
                    -0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,

                    -0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f,
                     0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f,
                     0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,
                     0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,
                    -0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,
                    -0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f,
                };
                
                //the array of indices telling us which vertices in the 'vertices' array form triangles
                uint[] indicesCube = new uint[]
                {
                    //front-face
                    0, 1, 3,
                    1, 2, 3,

                    //back-face
                    7, 4, 5,
                    5, 6, 7,

                    //top-face
                    7, 4, 0,
                    0, 3, 7,

                    //bottom-face
                    1, 5, 6,
                    6, 2, 1,

                    //left-face
                    3, 2, 6,
                    6, 7, 3,

                    //right-face
                    0, 4, 5,
                    5, 1, 0
                };

                //GameObject obj =  new GameObject(verticesCube, indicesCube, new Transform(pos, rot, scale), mat, false, true, false);
                //obj.CalcNormalsInclusive();
                GameObject obj = new GameObject(new Transform(pos, rot, scale), "Cube");
                Mesh mesh = new Mesh(verts, indicesCube, true);
                Model model = new Model(mesh, mat, "Cube_Mesh");
                obj.AddComp(model);
                //obj.GetComp<Mesh>().CalcNormalsSmooth();
                return obj;
            }
            else if (type == Primitve.Pyramid)
            {
                //verts and triangle-indices for a pyramid shape
                float[] verticesPyramid = new float[]
                {
                    //XYZ Coordinates, RGB Color, UV Coordinates
                    //base-quad
                    0.5f, 0.5f, 0f, .5f*col, .5f*col, .5f*col, 99f, 99f, //top right
                    0.5f, -0.5f, 0f, .5f*col, .5f*col, .5f*col, 99f, 99f, //bottom right
                    -0.5f, -0.5f, 0f, .5f*col, .5f*col, .5f*col, 99f, 99f, //bottom left
                    -0.5f, 0.5f, 0f, .5f*col, .5f*col, .5f*col, 99f, 99f, //top left

                    0f, 0f, 1f, 1f*col, 1f*col, 1f*col,  99f, 99f, //the top 
                };
                uint[] indicesPyramid = new uint[]
                {
                    //base quad
                    3, 2, 1,
                    1, 0, 3,
            
                    //the tri-sides
                    4, 3, 0,
                    4, 0, 1,
                    4, 2, 1,
                    4, 2, 3
                };

                GameObject obj = new GameObject(new Transform(pos, rot, scale), "Pyramid");
                Mesh mesh = new Mesh(verticesPyramid, indicesPyramid);
                Model model = new Model(mesh, mat, "Pyramid_Mesh");
                obj.AddComp(model);
                obj.GetComp<Model>().CalcNormals(true);
                return obj;
            }
            else if (type == Primitve.Billboard)
            {
                float[] verticesPlane = new float[]
                {
                    //XYZ Coordinates, RGB Color, UV Coordinates
                    0.5f, 0f, 0.5f, 0f, 0f, 0f, 1f, 1f, //top right
                    0.5f, 0f, -0.5f, 0f, 0f, 0f, 1f, 0f, //bottom right
                    -0.5f, 0f, -0.5f, 0f, 0f, 0f, 0f, 0f, //bottom left
                    -0.5f, 0f, 0.5f, 0f, 0f, 0f, 0f, 1f, //top left
                };

                uint[] indicesPlane = new uint[]
                {
                    0, 1, 3,
                    1, 2, 3,
                };
                GameObject obj = new GameObject(new Transform(pos, rot, scale), "Billboard");
                Mesh mesh = new Mesh(verticesPlane, indicesPlane);
                Model model = new Model(mesh, mat, "Billboard_Mesh");
                obj.AddComp(model);
                obj.GetComp<Model>().CalcNormals(true);
                Billboard billboard = new Billboard();
                obj.AddComp(billboard);
                return obj;
            }
            else if(type == Primitve.LineRenderer)
            {
                GameObject obj = new GameObject(new Transform(pos, rot, scale), "LineRenderer");
                LineRenderer lineRenderer = new LineRenderer(new Vector3(-0.5f, 0.0f, 0.0f), new Vector3(0.5f, 0.0f, 0.0f), 0.5f, mat);
                obj.AddComp(lineRenderer);
                return obj;
            }
            throw new Exception("ERROR: Primitive has no yet been implemented.");
        }

        /// <summary> 
        /// Generates a GameObject with a mesh created from an .obj file. 
        /// <para> Assumes .obj file is in 'assets\Models\fileName'. </para>
        /// <para> 'fileName' doesn't have to include the .obj extension. </para>
        /// </summary>
        /// <param name="pos"> Position of the GameObject. </param>
        /// <param name="rot"> Rotation of the GameObject. </param>
        /// <param name="scale"> Scale of the GameObject. </param>
        /// <param name="mat"> Material of the GameObject. </param>
        /// <param name="fileName"> The name of the .obj file in 'assets\Models\fileName', doesn't have to include the .obj extension.. </param>
        public static async Task<GameObject> CreateFromFileAsync(Vector3 pos, Vector3 rot, Vector3 scale, Material mat, string fileName)
        {
            Task<Mesh> mesh = Task.Run(() => ModelImporter.Import(fileName));
            await mesh;
            Mesh m = mesh.Result;
            if(m == null) { return null; }

            //get the actual name from the 'fileName' string, excludes all directories
            string[] s = fileName.Split(@"\");
            fileName = s[s.Length - 1];

            GameObject obj = new GameObject(new Transform(pos, rot, scale), fileName);
            Model model = new Model(m, mat, fileName);
            await Task.Run(() => obj.AddComp(model));
            return obj;
        }

        /// <summary> 
        /// Generates a GameObject with a mesh created from an .obj file. 
        /// <para> Assumes .obj file is in 'assets\Models\fileName'. </para>
        /// <para> 'fileName' doesn't have to include the .obj extension. </para>
        /// </summary>
        /// <param name="pos"> Position of the GameObject. </param>
        /// <param name="rot"> Rotation of the GameObject. </param>
        /// <param name="scale"> Scale of the GameObject. </param>
        /// <param name="mat"> Material of the GameObject. </param>
        /// <param name="fileName"> The name of the .obj file in 'assets\Models\fileName', doesn't have to include the .obj extension. </param>
        public static GameObject CreateFromFile(Vector3 pos, Vector3 rot, Vector3 scale, Material mat, string fileName)
        {
            //Mesh m = ModelImporter.Import(fileName);
            //if (m == null) { BBug.Log("!!! GameObject.CreateFromFile -> ModelImporter.Import returned null !!!"); return null; }

            Mesh m = AssetManager.GetMesh(fileName);

            if (fileName.Contains(@"\")) 
            {
                //get the actual name from the 'fileName' string, excludes all directories
                string[] s = fileName.Split(@"\");
                fileName = s[s.Length - 1];
            }

            GameObject obj = new GameObject(new Transform(pos, rot, scale), "import_" + fileName);
            Model model = new Model(m, mat, fileName);
            obj.AddComp(model);
            return obj;
        }

        /// <summary>
        ///     Generates a Grid with 'xVerts' in x-Direction and 'zVerts' in z-Direction, as a  GameObject with a Mesh Component attached.
        /// </summary>
        /// <param name="xVerts"> The amount of Vertices in the x-Direction. </param>
        /// <param name="zVerts"> The amount of Vertices in the z-Direction. </param>
        /// <param name="pos"> Position of the GameObject. </param>
        /// <param name="rot"> Rotation of the GameObject. </param>
        /// <param name="scale"> Scale of the GameObject. </param>
        /// <param name="mat"> Material of the GameObject. </param>
        /// <param name="randHeight"> Moves the Grids Vertices Y-Coordinates up/down based on some noise. </param>
        /// <param name="strength"> Strength of the Grids Random-Heigh-Gen. </param>
        /// <param name="frequency"> Frequency of the Grids Random-Heigh-Gens Noise </param>
        /// <param name="lacunarity"> Lacunarity of the Grids Random-Heigh-Gens Noise </param>
        /// <param name="gain"> Gain of the Grids Random-Heigh-Gens Noise </param>
        /// <param name="seed"> Seed of the Grids Random-Heigh-Gens Noise </param>
        public static GameObject CreateGrid(int xVerts, int zVerts, Vector3 pos, Vector3 rot, Vector3 scale, Material mat, bool randHeight = false, float strength = 1f, float frequency = .005f, float lacunarity = 1.2f, float gain = .4f, int seed = 1339)
        {
            //make sure both xVerts and zVerts are at least 2
            xVerts = xVerts < 2 ? 2 : xVerts; zVerts = zVerts < 2 ? 2 : zVerts;

            //generates the vertices
            float[] verts = new float[zVerts * xVerts * 8];
            for (int z = 0; z < zVerts; z++)
            {
                //Debug.WriteLine("---Row0" + z.ToString() + "---");
                for (int x = 0; x < xVerts * 8;)
                {
                    //xyz coords              amount of verts in row +1 * step size per vert to fit in -1 to 1 space | sizeX / 2 and sizeX&sizeZ are always 1
                    verts[(z * (xVerts * 8)) + x + 0] = (((x / 8) + 1) * (2f / xVerts)) - 1f;//(float)Math.Clamp((double) , -1, 1);
                    verts[(z * (xVerts * 8)) + x + 1] = 0f;
                    verts[(z * (xVerts * 8)) + x + 2] = z * (2f / zVerts) - 1f; //(float)Math.Clamp((double) , -1, 1);
                    //Debug.WriteLine("Vert0" + (x / 8).ToString() + ": X: " + ((((x / 8) + 1) * (2f / xVerts)) - 1f).ToString() + ", Z: " + (z * (2f / zVerts) - 1f).ToString());

                    //usually normals for lighting, here rgb colors
                    verts[(z * (xVerts * 8)) + x + 3] = 0f;
                    verts[(z * (xVerts * 8)) + x + 4] = 1f;
                    verts[(z * (xVerts * 8)) + x + 5] = 0f;

                    //uv
                    verts[(z * (xVerts * 8)) + x + 6] = (float)x/(xVerts *8);
                    verts[(z * (xVerts * 8)) + x + 7] = (float)z/zVerts;
                    //Debug.WriteLine(" |U: " + verts[(z * (xVerts * 8)) + x + 6].ToString() + ", V: " + verts[(z * (xVerts * 8)) + x + 7].ToString() + ", x: " + x.ToString() + ", xVerts: " + xVerts.ToString() + ", z: " + z.ToString() + ", zVerts: " + zVerts.ToString());
                    //Debug.WriteLine("Vert: " + (z * (xVerts * 8) + x).ToString() + ", x: " + x.ToString() + ", Coords: " + ((((x / 8) + 1) * (2f / xVerts)) - 1f).ToString() + "f, 0f ," + (z * (2f / zVerts) - 1f).ToString() + "f");
                    x += 8;
                }
            }

            //generates the triangles
            uint[] tris = new uint[xVerts * zVerts * 6];
            //Debug.WriteLine("xVerts: " + xVerts.ToString() + "; zVerts: " + zVerts.ToString() + "; Verts-Length: " + verts.Length.ToString() + "; Tris-Length: " + tris.Length.ToString() + "\n");
            uint quad = 0; int tri = 0; bool firstW = true;
            for (int w = 0, z = 0; z < zVerts - 1; z++)
            {
                if (firstW) { w = 1; }
                //Debug.WriteLine("---Row0" + z.ToString() + "---");
                for (int x = 0; x < xVerts - 1; x++)
                {
                    tris[tri + 0] = quad + (uint)(xVerts * (z / w)) + 1;
                    tris[tri + 1] = quad + (uint)(xVerts * (z / w)) + 0;
                    tris[tri + 2] = quad + (uint)(xVerts * (z / w) + xVerts);
                    tris[tri + 3] = quad + (uint)(xVerts * (z / w) + xVerts);
                    tris[tri + 4] = quad + (uint)(xVerts * (z / w) + xVerts + 1);
                    tris[tri + 5] = quad + (uint)(xVerts * (z / w)) + 1;

                    //Debug.WriteLine("Quad: " + quad.ToString() + ", Indices: " + tris[tri + 0].ToString() + ", " + tris[tri + 1].ToString() + ", " + tris[tri + 2].ToString() + ", " + tris[tri + 3].ToString() + ", " + tris[tri + 4].ToString() + ", " + tris[tri + 5].ToString());

                    quad += 1;
                    tri += 6;
                }
                if (firstW) { w = 0; firstW = false; }
                quad++;
                w += 2;
            }

            GameObject grid = new GameObject(new Transform(pos, rot, scale), "Grid");
            Mesh mesh = new Mesh(verts, tris);
            Model model = new Model(mesh, mat, "Grid_Mesh");
            grid.AddComp(model);
            if (randHeight)
            {
                BBug.StartTimer("Terrain RandHeight-Total");
                RandomHeight rndHeight = new RandomHeight(xVerts, zVerts, strength, frequency, lacunarity, gain, seed);
                grid.AddComp(rndHeight);
                BBug.StopTimer();
            }
            else
            {
                grid.GetComp<Mesh>().CalcNormalsFlat();
            }

            return grid;
        }
        #endregion
    }
}
