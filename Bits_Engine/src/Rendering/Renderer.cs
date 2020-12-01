using static BitsCore.OpenGL.GL;
using BitsCore.ObjectData;
using BitsCore.ObjectData.Components;
using BitsCore.ObjectData.Materials;
using BitsCore.Rendering.Cameras;
using BitsCore.Rendering.Display;
using GLFW;
using System.Collections.Generic;
using System.Numerics;
using BitsCore.Rendering.Layers;
using BitsCore.BitsGUI;
using BitsCore.Debugging;
using BitsCore.Utils;
using System;

namespace BitsCore.Rendering
{
    public static class Renderer
    {
        #region VARIABLES

        //Misc-------------------------------------------
        public static Application activeApp;

        /// <summary> Renders all objects as a Wireframe when true. </summary>
        public static bool wireFrameMode = false;
        /// <summary> Renders all objects as a color representing the surfaces normal-direction when true. </summary>
        public static bool normalMode = false;
        /// <summary> Renders all objects as a color representing the surfaces uv-coordinats when true. </summary>
        public static bool uvMode = false;

        //Layers-----------------------------------------
        /// <summary> 
        /// The stack of layers, that get drawn ontop of each other. 
        /// <para> f.e. Can be used for always showing UI or FPS hands infront. </para>
        /// </summary>
        internal static List<Layer> LayerStack = new List<Layer>();
        
        /// <summary> 
        /// The index of the Layer in the <see cref="LayerStack"/>,
        /// <para> that will be submitted to by <see cref="Submit(GameObject)"/> and <see cref="Submit(IEnumerable{GameObject})"/>. </para> 
        /// </summary>
        private static int activeLayer;

        /// <summary> 
        /// Gets the first <see cref="Layer"/> of type <see cref="Layer3D"/> in the <see cref="LayerStack"/>. 
        /// <para> Returns new <see cref="Layer3D"/> if no <see cref="Layer"/> of that type is contained in the <see cref="LayerStack"/>. </para>
        /// </summary>
        public static Layer3D BaseLayer3D => (Layer3D)BaseLayer<Layer3D>() == null ? new Layer3D() : (Layer3D)BaseLayer<Layer3D>();
        /// <summary> 
        /// Gets the first <see cref="Layer"/> of type <see cref="LayerUI"/> in the <see cref="LayerStack"/>. 
        /// <para> Returns new <see cref="LayerUI"/> if no <see cref="Layer"/> of that type is contained in the <see cref="LayerStack"/>. </para>
        /// </summary>
        public static LayerUI BaseLayerUI => (LayerUI)BaseLayer<LayerUI>() == null ? new LayerUI() : (LayerUI)BaseLayer<LayerUI>();


        //3d----------------------------------------------
        /// <summary> The main camera of the game. </summary>
        public static Camera3D mainCam;

        /// <summary> The camera for Cutscenes, etc. </summary>
        public static Camera3D secondaryCam;

        /// <summary> The Background-Color of the game. </summary>
        public static Vector3 bgCol;

        /// <summary> The LightSource-Components used to light all <see cref="Layer3D"/>. </summary>
        public static LightSource[] lightSources = new LightSource[0];
        internal static DirectionalLight[] dirLights = new DirectionalLight[0];
        internal static PointLight[] pointLights = new PointLight[0];
        internal static SpotLight[] spotLights = new SpotLight[0];

        //2d----------------------------------------------
        /// <summary> 
        /// The Vertex Array Object of the Mesh 
        /// <para> Used to pass the vertex data the VBO. </para>
        /// </summary>
        public static uint quadVAO { get; private set; } //unsigned-int storing the pointer to the Vertex-Array-Object
        /// <summary> 
        /// The Vertex Buffer Object of the Mesh 
        /// <para> Used to store the vertex-data. </para>
        /// </summary>
        public static uint quadVBO { get; private set; } //unsigned-int storing the pointer to the Vertex-Buffer-Object
        
        #endregion

        /// <summary> Initializes the Renderer. </summary>
        /// <param name="app"> The currently active instance of the Application-Class. </param>
        public static void Init(Application app)
        {
            wireFrameMode = false;
            normalMode = false;

            activeApp = app;

            //generates a depth-buffer using which opengl determines which face should be infront/behind
            glEnable(GL_DEPTH_TEST);

            Gen2DQuad();
        }

        #region Rendering

        /// <summary> Generates the opengl-data(vao, vbo) for the quad used to render all 2d objects. </summary>
        private static void Gen2DQuad()
        {
            #region OPENGL
            //Generates the opengl-data(vao, vbo)

            #region VERTICES
            float[] verts = new float[]
            {
                //pos       //uv
                0.0f, 1.0f, 0.0f, 1.0f,
                1.0f, 0.0f, 1.0f, 0.0f,
                0.0f, 0.0f, 0.0f, 0.0f,

                0.0f, 1.0f, 0.0f, 1.0f,
                1.0f, 1.0f, 1.0f, 1.0f,
                1.0f, 0.0f, 1.0f, 0.0f,
            };
            #endregion

            unsafe
            {

                //create VAO and VBO
                quadVAO = glGenVertexArray();
                quadVBO = glGenBuffer();

                glBindVertexArray(quadVAO);

                glBindBuffer(GL_ARRAY_BUFFER, quadVBO);

                fixed (float* ptr = &verts[0])
                {
                    glBufferData(GL_ARRAY_BUFFER, verts.Length * sizeof(float), ptr, GL_STATIC_DRAW);
                }

                //tell vao how to interpret the data in vbo
                //arg01: vbo-index - for using mutiple vbo/ pointing to the same one multiple times
                //arg02: size-per-vertex - how many vars per vertex
                //arg03: type of data - e.g. int/float/etc.
                //arg04: normalize - normalizes the data (changes it)
                //arg05: amount of bytes inbetween each pair of relevant data - 5 * float-size, because there are 5 floats for each set of data out of which we on use the first two as specified in arg02
                //arg06: offset between start of the data-set as a whole and the first set of relevant data
                //arg06&(void*): pointer of type void, (void*)0 is a pointer to "first position"
                glVertexAttribPointer(0, 4, GL_FLOAT, false, 4 * sizeof(float), (void*)0);
                glEnableVertexAttribArray(0);

                glBindBuffer(GL_ARRAY_BUFFER, 0);
                glBindVertexArray(0);
            }
            #endregion
        }

        /// <summary> Renders the entire scene, aka all layers, to the current window. </summary>
        public static void RenderScene()
        {
            //activates/deactivates wireframe-mode  , do here to affect all layers
            if (wireFrameMode)
            {
                glPolygonMode(GL_FRONT_AND_BACK, GL_LINE);
            }
            else
            {
                glPolygonMode(GL_FRONT_AND_BACK, GL_FILL);
            }

            //sets the clear-color and fills the screen with it
            if (!wireFrameMode && !normalMode && !uvMode) { glClearColor(bgCol.X, bgCol.Y, bgCol.Z, 1f); } else { glClearColor(0.1f, 0.1f, 0.1f, 1f); }
            glClear(GL_COLOR_BUFFER_BIT); //clears the color of the last frame

            Matrix4x4 projection;
            Matrix4x4 view;

            int vertsInScene = 0;
            foreach (Layer layer in LayerStack)
            {
                glClear(GL_DEPTH_BUFFER_BIT); //clears the depth information of the last frame, to render the layers actually ontop of one another

                if(layer.GetType() == typeof(Layer3D))
                {
                    //do these calc outside the foreach-loop because its the same each layer
                    projection = mainCam.GetProjectionMatrix();
                    view = mainCam.GetViewMatrix();

                    Layer3D l = (Layer3D)layer;
                    vertsInScene += RenderLayer3D(l.gameObjects, projection, view);
                }
                else if(layer.GetType() == typeof(LayerUI))
                {
                    //do these calc outside the foreach-loop because its the same each layer
                    projection = mainCam.GetOrthoProjectionMatrix();

                    RenderLayerUI((LayerUI)layer);
                }
            }

            activeApp.mainLayerUI.SetText("VERTS", "Verts in Scene: " + vertsInScene);

            //swaps the buffers, opengl has two buffers one for being displayed and one for inputing the data being rendered in the 'Render()' loop
            Glfw.SwapBuffers(DisplayManager.Window);
        }

        /// <summary> Renders the given GameObjects to the screen. </summary>
        /// <param name="RenderObjects"> The GameObjects containing a Mesh-Component. </param>
        /// <param name="LightObjects"> The GameObjects containing a LightSource-Component. These GameObjects are sometimes also in the 'RenderObjects' array, should they also have a Mesh-Component. </param>
        /// <param name="projection"> The Projection-Matrix used. </param>
        /// <param name="view"> The View-Matrix used. </param>
        public static int RenderLayer3D(List<GameObject> RenderObjects, Matrix4x4 projection, Matrix4x4 view)
        {
            //setup -------------------------------------------------------------------------------------------------------------------
            glEnable(GL_BLEND); //enable blending
            glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA); //1 - source_alpha, e.g. 0.6(60%) transparency -> 1 - 0.6 = 0.4(40%)
            glBlendFuncSeparate(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA, GL_ONE, GL_ZERO);


            //Object-Render -----------------------------------------------------------------------------------------------------------
            int vertsInLayerCount = 0;
            //go through all GameObjects in 'RenderObjects', which are all the objects with Mesh-Components and draw them to the screen
            foreach (GameObject go in RenderObjects)
            {
                //get the mesh-component in the GameObject and error-check
                if (!go.HasComp<Model>()) { continue; }
                Model m = go.GetComp<Model>();
                if (m == null || m.GetType() != typeof(Model)) { throw new System.Exception("!!!No Mesh on GameObject in RenderObjects-List!!!"); }

                vertsInLayerCount += m.mesh.vertices.Length / 8;

                //unlit-materials properties have to be set before material.Use()
                if (m.material.GetType() == typeof(UnlitMaterial))
                {
                    if (m.gameObject.HasComp<DirectionalLight>())
                    {
                        UnlitMaterial material = (UnlitMaterial)m.material;
                        material.objColor = m.gameObject.GetComp<DirectionalLight>().diffuseColor;
                    }
                    else if (m.gameObject.HasComp<PointLight>())
                    {
                        UnlitMaterial material = (UnlitMaterial)m.material;
                        material.objColor = m.gameObject.GetComp<PointLight>().diffuseColor;
                    }
                    else if (m.gameObject.HasComp<SpotLight>())
                    {
                        UnlitMaterial material = (UnlitMaterial)m.material;
                        material.objColor = m.gameObject.GetComp<SpotLight>().diffuseColor;
                    }
                }

                //activates this materials shader
                m.material.Use();

                if ((m.material.GetType() == typeof(TexturedPhongMaterial) || m.material.GetType() == typeof(TexturedPhongMaterialTop) || m.material.GetType() == typeof(TexturedCelShadingMaterial)) && lightSources.Length > 0)
                {
                    m.material.shader.SetVector3("viewPos", mainCam.transform.position);

                    if(dirLights.Length > 0)
                    {
                        m.material.shader.SetInt("Num_DirLights", dirLights.Length);

                        for (int i = 0; i < dirLights.Length; i++)
                        {
                            Vector3 dir = dirLights[i].gameObject.transform.rotation;
                            //dir.Y = MathUtils.RadiansToDegree(dir.Y);
                            //dir.X = MathUtils.RadiansToDegree(dir.X);
                            //dir.Z = MathUtils.RadiansToDegree(dir.Z);

                            dir.Y = MathUtils.DegreeToRadians(dir.Y);
                            dir.X = MathUtils.DegreeToRadians(dir.X);
                            dir.Z = MathUtils.DegreeToRadians(dir.Z);

                            m.material.shader.SetVector3("dirLights[" + i + "].ambient", dirLights[i].GetAmbient());
                            m.material.shader.SetVector3("dirLights[" + i + "].diffuse", dirLights[i].GetDiffuse());
                            m.material.shader.SetVector3("dirLights[" + i + "].specular", dirLights[i].GetSpecular());

                            m.material.shader.SetVector3("dirLights[" + i + "].direction", dir.X, dir.Y, dir.Z);
                        }
                    }
                    if(pointLights.Length > 0)
                    {
                        m.material.shader.SetInt("Num_PointLights", pointLights.Length);

                        for(int i = 0; i < pointLights.Length; i++)
                        {
                            m.material.shader.SetVector3("pointLights[" + i + "].position", pointLights[i].gameObject.transform.position);

                            m.material.shader.SetVector3("pointLights[" + i + "].ambient", pointLights[i].GetAmbient());
                            m.material.shader.SetVector3("pointLights[" + i + "].diffuse", pointLights[i].GetDiffuse());
                            m.material.shader.SetVector3("pointLights[" + i + "].specular", pointLights[i].GetSpecular());

                            m.material.shader.SetFloat("pointLights[" + i + "].constant", pointLights[i].constant);
                            m.material.shader.SetFloat("pointLights[" + i + "].linear", pointLights[i].linear);
                            m.material.shader.SetFloat("pointLights[" + i + "].quadratic", pointLights[i].quadratic);
                        }
                    }
                    if(spotLights.Length > 0)
                    {
                        m.material.shader.SetInt("Num_SpotLights", spotLights.Length);

                        for(int i = 0; i < spotLights.Length; i++)
                        {
                            //need to convert this from cartesian to euler to get the forward vec
                            Vector3 dir = spotLights[i].gameObject.transform.rotation;
                            
                            m.material.shader.SetVector3("spotLights[" + i + "].direction", spotLights[i].gameObject.transform.GetForwardDir()); //dir
                            m.material.shader.SetVector3("spotLights[" + i + "].position", spotLights[i].gameObject.transform.position);
                    
                            m.material.shader.SetVector3("spotLights[" + i + "].ambient", spotLights[i].GetAmbient());
                            m.material.shader.SetVector3("spotLights[" + i + "].diffuse", spotLights[i].GetDiffuse());
                            m.material.shader.SetVector3("spotLights[" + i + "].specular", spotLights[i].GetSpecular());

                            m.material.shader.SetFloat("spotLights[" + i + "].cutOff", MathF.Cos(MathUtils.DegreeToRadians(spotLights[i].cutOff)));
                            m.material.shader.SetFloat("spotLights[" + i + "].outerCutOff", MathF.Cos(MathUtils.DegreeToRadians(spotLights[i].outerCutOff)));

                            m.material.shader.SetFloat("spotLights[" + i + "].constant", spotLights[i].constant);
                            m.material.shader.SetFloat("spotLights[" + i + "].linear", spotLights[i].linear);
                            m.material.shader.SetFloat("spotLights[" + i + "].quadratic", spotLights[i].quadratic);
                        }
                    }
                }
                else
                {
                    //set ambient light
                    m.material.shader.SetVector3("light.ambient", new Vector3(.1f, .1f, .1f));
                }


                //set the projection-matrix
                m.material.shader.SetMatrix4x4("projection", projection);
                //set the shaders view-matrix
                m.material.shader.SetMatrix4x4("view", view);
                //set the shaders translation-matrix ('model')
                m.material.shader.SetMatrix4x4("model", go.transform.GetModelMatrix());

                #region SPECIAL_MODES
                //changes the material to be used for a objs to be a basic white, for better readability
                if (wireFrameMode)
                {
                    AssetManager.GetMaterial("Mat_Wireframe").Use();

                    //set the projection-matrix
                    AssetManager.GetMaterial("Mat_Wireframe").shader.SetMatrix4x4("projection", projection);
                    //set the shaders translation-matrix ('model')
                    AssetManager.GetMaterial("Mat_Wireframe").shader.SetMatrix4x4("model", go.transform.GetModelMatrix());
                    //set the shaders view-matrix
                    AssetManager.GetMaterial("Mat_Wireframe").shader.SetMatrix4x4("view", view);
                }
                else if (normalMode)
                {
                    AssetManager.GetMaterial("Mat_Normal").Use();

                    //set the projection-matrix
                    AssetManager.GetMaterial("Mat_Normal").shader.SetMatrix4x4("projection", projection);
                    //set the shaders translation-matrix ('model')
                    AssetManager.GetMaterial("Mat_Normal").shader.SetMatrix4x4("model", go.transform.GetModelMatrix());
                    //set the shaders view-matrix
                    AssetManager.GetMaterial("Mat_Normal").shader.SetMatrix4x4("view", view);
                }
                else if (uvMode)
                {
                    AssetManager.GetMaterial("Mat_UV").Use();

                    //set the projection-matrix
                    AssetManager.GetMaterial("Mat_UV").shader.SetMatrix4x4("projection", projection);
                    //set the shaders translation-matrix ('model')
                    AssetManager.GetMaterial("Mat_UV").shader.SetMatrix4x4("model", go.transform.GetModelMatrix());
                    //set the shaders view-matrix
                    AssetManager.GetMaterial("Mat_UV").shader.SetMatrix4x4("view", view);
                }
                #endregion

                //bind vertex array and do the actual draw-call
                glBindVertexArray(m.mesh.vao);

                if (m.mesh.notIndexed)
                {
                    //draws the meshes that aren't indexed, 
                    //to do this it assumes each e consecutive vertices in the meshes vertices-array for a triangle
                    glDrawArrays(GL_TRIANGLES, 0, m.mesh.vertices.Length);
                }
                else
                {
                    unsafe
                    {
                        //the fixed framework makes sure all mentioned pointers('ptr') are consistent and stay the same (c# uses dynamic memory-allocation stuff like vectors in c++ I think (I don't know what im doing so you know))
                        fixed (uint* ptr = &m.mesh.triangles[0])
                        {
                            //draw elements draws the meshes with triangle indices
                            glDrawElements(GL_TRIANGLES, m.mesh.triangles.Length, GL_UNSIGNED_INT, ptr);
                        }
                    }
                }
                //unbind the vertex-array to re-gain the memory occupied by the prev. bound vertex-array
                glBindVertexArray(0);
            }

            return vertsInLayerCount;
        }
        
        /// <summary> Renders the Items in a Container's Elements, for each given Container. </summary>
        /// <param name="containers"> The given Containers. </param>
        /// <param name="projection"> The Projection-Matrix used. </param>
        /// <param name="view"> The View-Matrix used. </param>
        public static void RenderLayerUI(LayerUI l)
        {
            //all objects use the same shader, vertex-array, projection-matrix as they are all quads
            l.shader.Use();
            glBindVertexArray(quadVAO);
            l.shader.SetMatrix4x4("projection", l.projection);

            List<Item> itemsTotal = new List<Item>();

            //add all the items in the different master containers and their respective subcontainers together and calc their position
            foreach(Container cont in l.container)
            {
                itemsTotal.AddRange(BitsGUIRenderer.FormatContainerTree(cont));
            }

            //BBug.Log(l.container.Count + " Container(s) with " + itemsTotal.Count + " item(s).");

            foreach (Item item in itemsTotal)
            {
                //Matrix4x4 pos = Matrix4x4.Identity;
                //pos *= Matrix4x4.CreateTranslation(new Vector3(item.screen_posX, item.screen_posY, 0.0f));
                //pos *= Matrix4x4.CreateTranslation(new Vector3(.5f * item.screen_width, .5f * item.screen_height, 0.0f));

                Matrix4x4 trans = Matrix4x4.CreateTranslation(item.screen_posX, item.screen_posY, 0);

                //Matrix4x4 rot = Matrix4x4.CreateRotationZ(item.rot);

                Matrix4x4 sca = Matrix4x4.CreateScale(item.screen_width, item.screen_height, 1.0f);

                //Matrix4x4 model = sca * rot * pos;
                //Matrix4x4 model = pos * rot * sca;
                Matrix4x4 model = sca * trans; //sca * rot * trans

                l.shader.SetMatrix4x4("model", model);
                l.shader.SetVector3("color", item.color);

                //all items have a texture, for the single color items its just a 1x1px white texture
                item.texture.Use();

                glDrawArrays(GL_TRIANGLES, 0, 6);

            }
            glBindVertexArray(0);

            foreach(RenderText txt in l.texts.Values)
            {
                TextRenderer.RenderText(l, txt.text, txt.xPos, txt.yPos, txt.scale, txt.color);
            }
        }

        /// <summary> Goes through all Layer3D in the <see cref="LayerStack"/> and calls <see cref="Model.GenOpenGLData()"/>. </summary>
        public static void SetupAssets(Layer newLayer = null)
        {

            if(newLayer == null)
            {
                //takes care of vao, vbo, ebo for all GO's
                foreach (Layer layer in LayerStack)
                {
                    if (layer.GetType() == typeof(Layer3D))
                    {
                        Layer3D l = (Layer3D)layer;

                        foreach (GameObject go in l.gameObjects)
                        {
                            if (go == null) { BBug.Log("Emty-GameObject in  Scene."); continue; }

                            if (go.HasComp<Model>())
                            {
                                go.GetComp<Model>().GenOpenGLData();
                            }
                        }
                    }
                }
            }
            else
            {
                if (newLayer.GetType() == typeof(Layer3D))
                {
                    Layer3D l = (Layer3D)newLayer;

                    foreach (GameObject go in l.gameObjects)
                    {
                        if (go == null) { BBug.Log("Emty-GameObject in  Scene."); continue; }

                        if (go.HasComp<Model>())
                        {
                            go.GetComp<Model>().GenOpenGLData();
                        }
                    }
                }
            }

        }
        #endregion

        #region LAYER_METHODS
        /// <summary> 
        /// Setting a layer active means that all subsequent calls to <see cref="Submit(GameObject)"/> or <see cref="Submit(IEnumerable{GameObject})"/> submit to the chosen/activated layer. 
        /// <para> Returns false if the provided layer isn't contained in <see cref="LayerStack"/>. </para>
        /// </summary>
        /// <param name="layer"> The layer to be set active. </param>
        public static bool SetActiveLayer(Layer layer)
        {
            if (LayerStack.Contains(layer))
            {
                activeLayer = LayerStack.IndexOf(layer);
                return true;
            }
            return false;
        }
        /// <summary> 
        /// Setting a layer active means that all subsequent calls to <see cref="Submit(GameObject)"/> or <see cref="Submit(IEnumerable{GameObject})"/> submit to the chosen/activated layer. 
        /// <para> Returns false if the provided index did not have an associated layer in <see cref="LayerStack"/>. </para>
        /// </summary>
        /// <param name="index"> The index of the layer in <see cref="LayerStack"/>. </param>
        public static bool SetActiveLayer(int index)
        {
            if (LayerStack[index] == null) { return false; }
            else
            {
                activeLayer = index;
                return true;
            }
            
        }

        /// <summary>
        /// Add a Layer to the <see cref="LayerStack"/>
        /// <para> Leaving the index to the default value(-1) will simply add the given Layer to the end of <see cref="LayerStack"/>. </para>
        /// <para> If the given layer is the first one added to the <see cref="LayerStack"/> then it's index is the new <see cref="activeLayer"/>. </para>
        /// </summary>
        /// <param name="layer"> The Layer to add. </param>
        /// <param name="index"> The index in <see cref="LayerStack"/> the given layer should be added. </param>
        public static void AddLayer(Layer layer, int index = -1)
        {
            if(layer == null) { throw new System.ArgumentNullException("layer"); }

            if(index == -1)
            {
                LayerStack.Add(layer);
                SetupAssets(layer);
            }
            else
            {
                LayerStack.Insert(index, layer);
                SetupAssets(layer);
            }

            //set the new layer as the currently active one if it was the last one added
            if(LayerStack.Count == 1) { activeLayer = index == -1 ? 0 : index; }
        }

        /// <summary>
        /// Add a Layer to the <see cref="LayerStack"/> and set it to be the new <see cref="activeLayer"/>.
        /// <para> Leaving the index to the default value(-1) will simply add the given Layer to the end of <see cref="LayerStack"/>. </para>
        /// </summary>
        /// <param name="layer"> The Layer to add. </param>
        /// <param name="index"> The index in <see cref="LayerStack"/> the given layer should be added. </param>
        public static void AddLayerSetActive(Layer layer, int index = -1)
        {
            if (layer == null) { throw new System.ArgumentNullException("layer"); }

            AddLayer(layer, index);
            activeLayer = LayerStack.IndexOf(layer);
        }

        /// <summary> Gets the currently active Layer. </summary>
        public static Layer ActiveLayer()
        {
            if(LayerStack[activeLayer] == null) { throw new System.Exception("Active Layer is null. Only call this function after assigning a Layer."); }
            return LayerStack[activeLayer];
        }

        //base layer methods----------------------------------------------------------------
        /// <summary> 
        /// Gets the first <see cref="Layer"/> of type 'T' in the <see cref="LayerStack"/>. 
        /// <para> Returns null if no <see cref="Layer"/> of that type is contained in the <see cref="LayerStack"/>. </para>
        /// </summary>
        public static Layer BaseLayer<T>()
        {
            if(!typeof(T).IsSubclassOf(typeof(Layer))) { throw new System.Exception("Type passed to Renderer.BaseLayer() isn't a subclass of Layer."); }
            foreach (Layer layer in LayerStack)
            {
                if (layer.GetType() == typeof(T)) { return layer; }
            }

            return null;
        }

        /// <summary> 
        /// Submit/Add a <see cref="GameObject"/> to the Layer in <see cref="LayerStack"/> with index <see cref="activeLayer"/>. 
        /// <para> This makes the GameObject get rendered, take affect, etc. </para>
        /// <para> Returns false if the GameObject could not be added, because the currently active layer is not a <see cref="Layer3D"/>. </para>
        /// </summary>
        /// <param name="go"> The GameObject to be submitted. </param>
        public static bool Submit(GameObject go)
        {
            if(LayerStack[activeLayer].GetType() != typeof(Layer3D)) { BBug.Log("!!! Layer that a GameObject was Submitted to wasn't of Type Layer3D !!!"); return false;  }

            Layer3D layer = (Layer3D)LayerStack[activeLayer];
            bool wasIntegrated = false;

            if (go.HasComp<Model>())
            {
                layer.gameObjects.Add(go);
                wasIntegrated = true;
            }
            if (go.HasComp<DirectionalLight>()) //need to add spotlight
            {
                if(dirLights.Length >= 4) { throw new System.Exception("Can't have more than 4 Directional-Lights at once."); }

                //add lightsource to the end of the array
                int len = lightSources.Length;
                Array.Resize(ref lightSources, len + 1);
                if(lightSources.Length <= len) { throw new System.Exception("Could't resize the 'lightSources' array."); }
                lightSources[lightSources.Length - 1] = go.GetComp<DirectionalLight>();

                //add to the directionalLights array
                len = dirLights.Length;
                Array.Resize(ref dirLights, len + 1);
                if (dirLights.Length <= len) { throw new System.Exception("Could't resize the 'dirLights' array."); }
                dirLights[dirLights.Length - 1] = (DirectionalLight)lightSources[lightSources.Length -1];

                wasIntegrated = true;
            }
            if (go.HasComp<PointLight>())
            {
                if (pointLights.Length >= 32) { throw new System.Exception("Can't have more than 32 Point-Lights at once."); }

                //add lightsource to the end of the array
                int len = lightSources.Length;
                Array.Resize(ref lightSources, len + 1);
                if (lightSources.Length <= len) { throw new System.Exception("Could't resize the 'lightSources' array."); }
                lightSources[lightSources.Length - 1] = go.GetComp<PointLight>();

                //add to the pointLights array
                len = pointLights.Length;
                Array.Resize(ref pointLights, len + 1);
                if (pointLights.Length <= len) { throw new System.Exception("Could't resize the 'pointLights' array."); }
                pointLights[pointLights.Length - 1] = (PointLight)lightSources[lightSources.Length - 1];

                wasIntegrated = true;
            }
            if (go.HasComp<SpotLight>())
            {
                if (spotLights.Length >= 16) { throw new System.Exception("Can't have more than 16 Spot-Lights at once."); }

                //add lightsource to the end of the array
                int len = lightSources.Length;
                Array.Resize(ref lightSources, len + 1);
                if (lightSources.Length <= len) { throw new System.Exception("Could't resize the 'lightSources' array."); }
                lightSources[lightSources.Length - 1] = go.GetComp<SpotLight>();

                //add to the pointLights array
                len = spotLights.Length;
                Array.Resize(ref spotLights, len + 1);
                if (spotLights.Length <= len) { throw new System.Exception("Could't resize the 'spotLights' array."); }
                spotLights[spotLights.Length - 1] = (SpotLight)lightSources[lightSources.Length - 1];

                wasIntegrated = true;
            }
            return wasIntegrated;
        }
        /// <summary> 
        /// Submit/Add an <see cref="IEnumerable{}"/> of type <see cref="GameObject"/> to the Layer in <see cref="LayerStack"/> with index <see cref="activeLayer"/>. 
        /// <para> This makes the GameObjects get rendered, take affect, etc. </para>
        /// <para> Returns false if the GameObjects could not be added, because the currently active layer is not a <see cref="Layer3D"/>. </para>
        /// </summary>
        /// <param name="objs"> The GameObjects to be submitted. </param>
        public static bool Submit(IEnumerable<GameObject> objs)
        {
            if (LayerStack[activeLayer].GetType() != typeof(Layer3D)) { throw new System.Exception("!!! Layer that the GameObjects was Submitted to wasn't of Type Layer3D !!!"); return false; }

            foreach (GameObject obj in objs)
            {
                if(obj == null) { continue; }
                Submit(obj);
            }

            return true;
        }
        #endregion

    }
}
