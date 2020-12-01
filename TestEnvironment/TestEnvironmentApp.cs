using System;
using System.Collections.Generic;
using BitsCore.Rendering.Display;
using BitsCore.Rendering.Cameras;
using BitsCore.ObjectData;
using System.Numerics;
using BitsCore.ObjectData.Materials;
using BitsCore.ObjectData.Components;
using System.Threading.Tasks;
using BitsCore.DataManagement;
using BitsCore.ObjectData;
using BitsCore.Rendering;
using BitsCore.Utils;
using BitsCore.Rendering.Layers;
using BitsCore.BitsGUI;
using BitsCore.Debugging;
using BitsCore;
using BitsCore.InputSystem;
using BitsCore.SceneManagement;
using TestEnvironment.Scripts;
using BitsCore.DataManagement.Serialization;

namespace TestEnvironment
{
    /// <summary> The TestGame, derived from the Game Class. Used for testing stuff. </summary>
    public class TestEnvironmentApp : Application
    {

        bool paused = false;
        bool lightXPositive = true;
        bool lightYPositive = true;
        bool lightZPositive = true;
        bool mouseMoveLastFrame = false;
        int curFps;
        float fpsT;
        int fpsTicksCounterUpdate = 0;

        /// <summary> Generates an Instance of the derived Application class. </summary>
        public TestEnvironmentApp(int initialWindowWidth, int initialWindowHeight, string initialWindowTitle, bool initialMaximizeWindow = false) : base(initialWindowWidth, initialWindowHeight, initialWindowTitle, initialMaximizeWindow)
        {
        }

        protected override void Start()
        {
            BBug.SetPrintLog(true);
            BBug.Log("\n--------Application--------\n");
            BBug.StartTimer("Start");

            LoadMaterials(); //loads textures & materials into the assetmanager

            #region 3D_SCENE
            /*

            BBug.StartTimer("GameObject creation");
            //asynchrounsly fills the 'gameObjects' array
            //MakeGameObjects().Wait();
            //gameObjects = new GameObject[] { GameObject.CreateGrid(200, 200, new Vector3(-2f, -2f, 2.75f), Vector3.Zero, Vector3.One * 20f, MaterialLibrary.terrain, true, .5f, 0.02f) };
            List<GameObject> gameObjects = new List<GameObject>();

            BBug.Log("\nCalcNormalsSmooth-------------------------------");
            
            BBug.SetTimerTrigger(100.0f);
            gameObjects.Add(GameObject.CreateGrid(100, 100, new Vector3(0f, -3f, 0f), Vector3.Zero, Vector3.One * 20f, AssetManager.GetMaterial("Mat_Terrain01"), true, 0.75f, 0.025f));
            BBug.ResetTimerTrigger();

            BBug.Log("End---------------------------------------------\n");

            //sphere
            gameObjects.Add(GameObject.CreateFromFile(new Vector3(3f, 0f, 0f), Vector3.Zero, Vector3.One, AssetManager.GetMaterial("Mat_CelShading"), "sphere_poles"));
            
            gameObjects.Add(GameObject.CreateFromFile(new Vector3(6f, 0f, 0f), Vector3.Zero, Vector3.One, AssetManager.GetMaterial("Mat_UV-Checkered"), "cube"));

            gameObjects.Add(GameObject.CreateFromFile(new Vector3(0f, 0f, 0f), Vector3.Zero, Vector3.One, AssetManager.GetMaterial("Mat_CelShading"), "Cel_Crate01"));
            gameObjects.Add(GameObject.CreateFromFile(new Vector3(0f, 0f, 4f), Vector3.Zero, Vector3.One, AssetManager.GetMaterial("Mat_UV-Checkered"), "Cel_Crate01"));

            gameObjects.Add(GameObject.CreateFromFile(new Vector3(-3f, 0f, 0f), Vector3.Zero, Vector3.One, AssetManager.GetMaterial("Mat_CelShading"), "Cel_Crate01_Bevel01"));

            gameObjects.Add(GameObject.CreateFromFile(new Vector3(-6f, 0f, 0f), Vector3.Zero, Vector3.One, AssetManager.GetMaterial("Mat_CelShading"), "robot01_LD"));
            
            //add script-comp
            gameObjects[gameObjects.Count - 5].AddComp(new MoveByArrowKeys());
            gameObjects[gameObjects.Count - 4].AddComp(new MoveByArrowKeys());
            gameObjects[gameObjects.Count - 3].AddComp(new MoveByArrowKeys());
            gameObjects[gameObjects.Count - 2].AddComp(new MoveByArrowKeys());
            gameObjects[gameObjects.Count - 1].AddComp(new MoveByArrowKeys());

            //material-cubes
            gameObjects.Add(GameObject.CreateFromFile(new Vector3(0.0f, 0f, 10f), Vector3.Zero, Vector3.One, AssetManager.GetMaterial("Mat_Brick01"), "cube"));
            gameObjects.Add(GameObject.CreateFromFile(new Vector3(2.5f, 0f, 10f), Vector3.Zero, Vector3.One, AssetManager.GetMaterial("Mat_Grass01"), "cube"));
            gameObjects.Add(GameObject.CreateFromFile(new Vector3(5.0f, 0f, 10f), Vector3.Zero, Vector3.One, AssetManager.GetMaterial("Mat_StainedGlass01"), "cube"));
            gameObjects.Add(GameObject.CreateFromFile(new Vector3(7.5f, 0f, 10f), Vector3.Zero, Vector3.One, AssetManager.GetMaterial("Mat_Cliff01"), "cube"));
            gameObjects.Add(GameObject.CreateFromFile(new Vector3(10.0f, 0f, 10f), Vector3.Zero, Vector3.One, AssetManager.GetMaterial("Mat_Shotgun01"), "cube"));

            //walls
            gameObjects.Add(GameObject.CreateFromFile(new Vector3(18.0f, -3.5f, 6f), new Vector3(0f, -90f, 0f), Vector3.One * 0.5f, AssetManager.GetMaterial("Mat_Wall01"), "post_apocalyptic_wall"));
            gameObjects.Add(GameObject.CreateFromFile(new Vector3(18.0f, -3.5f, 0f), new Vector3(0f, -90f, 0f), new Vector3(-0.5f, 0.5f, 0.5f), AssetManager.GetMaterial("Mat_Wall01"), "post_apocalyptic_wall"));
            gameObjects.Add(GameObject.CreateFromFile(new Vector3(18.0f, -3.5f, -6f), new Vector3(0f, -90f, 0f), Vector3.One * 0.5f, AssetManager.GetMaterial("Mat_Wall01"), "post_apocalyptic_wall"));

            //barrels
            gameObjects.Add(GameObject.CreateFromFile(new Vector3(4.0f, -0.15f, -2.0f), new Vector3(0f, 0f, 0f), Vector3.One * 0.5f, AssetManager.GetMaterial("Mat_Barrel01"), "post_apocalyptic_barrel"));
            gameObjects.Add(GameObject.CreateFromFile(new Vector3(3.0f, -0.15f, -2.2f), new Vector3(0f, -90f, 0f), Vector3.One * 0.5f, AssetManager.GetMaterial("Mat_Barrel01"), "post_apocalyptic_barrel"));
            gameObjects.Add(GameObject.CreateFromFile(new Vector3(3.75f, -0.3f, -1.0f), new Vector3(92.5f, 60f, 0f), Vector3.One * 0.5f, AssetManager.GetMaterial("Mat_Barrel01"), "post_apocalyptic_barrel"));

            //crates
            gameObjects.Add(GameObject.CreateFromFile(new Vector3(-1f, -0.5f, -3.5f), Vector3.Zero, Vector3.One * 0.75f, AssetManager.GetMaterial("Mat_Crate01"), "crate01"));
            gameObjects.Add(GameObject.CreateFromFile(new Vector3(-1f, 1.0f, -3.5f), new Vector3(0f, 12f, 0f), Vector3.One * 0.75f, AssetManager.GetMaterial("Mat_Crate01"), "crate01"));
            gameObjects.Add(GameObject.CreateFromFile(new Vector3(-1f, 2.5f, -3.5f), new Vector3(0f, 6f, 0f), Vector3.One * 0.75f, AssetManager.GetMaterial("Mat_Crate01"), "crate01"));
            BBug.StopTimer();

            BBug.StartTimer("Lights creation");
            gameObjects.Add(GameObject.CreateDirectionalLight(new Vector3(0.5f, 4f, 0f), new Vector3(20f, -30f, 0f), Vector3.One, AssetManager.GetMaterial("Mat_DefaultLight"), new Vector3(1.0f, 0.65f, 0.5f), 0.5f)); //0.5f
            gameObjects.Add(GameObject.CreateDirectionalLight(new Vector3(-0.5f, 4f, 0f), new Vector3(-20f, 30f, 0f), Vector3.One, AssetManager.GetMaterial("Mat_DefaultLight"), new Vector3(0.1f, 0.0f, 0.5f), 0.1f)); //0.2f
            //gameObjects.Add(GameObject.CreatePointLight(new Vector3(0f, 2f, 0f), new Vector3(0f, 0f, 0f), Vector3.One, AssetManager.GetMaterial("Mat_DefaultLight"), new Vector3(0.0f, 0.0f, 1.0f), 2.5f));
            gameObjects.Add(GameObject.CreatePointLight(new Vector3(-5f, 6f, 0f), new Vector3(0f, 0f, 0f), Vector3.One, AssetManager.GetMaterial("Mat_DefaultLight"), new Vector3(1.0f, 1.0f, 1.0f), 1.5f)); //1.75f
            gameObjects.Add(GameObject.CreatePointLight(new Vector3(2f, 2f, 12f), new Vector3(0f, 0f, 0f), Vector3.One, AssetManager.GetMaterial("Mat_DefaultLight"), new Vector3(0.75f, 0.0f, 1.0f), 1.0f)); //1.5f
            //gameObjects.Add(GameObject.CreatePointLight(new Vector3(6f, 1f, -4f), new Vector3(0f, 0f, 0f), Vector3.One, AssetManager.GetMaterial("Mat_DefaultLight"), new Vector3(0.2f, 1.0f, 0.0f), 0.75f)); //1.0f

            gameObjects.Add(GameObject.CreateSpotLight(new Vector3(-2f, 2f, 2f), new Vector3(0f, 0f, 0f), Vector3.One, AssetManager.GetMaterial("Mat_DefaultLight"), new Vector3(1.0f, 1.0f, 1.0f), 1.0f));
            gameObjects[gameObjects.Count - 1].transform.RotateEuler(new Vector3(0f, MathUtils.DegreeToRadians(-33f), 0f)); //weird value
            BBug.StopTimer();

            BBug.StartTimer("Vertices-Cubes");
            //generates a small cube for each vertex in the GO gameObjects[objID]
            //gameObjects.AddRange(gameObjects[0].VerticesCubes());
            BBug.StopTimer();
            
            BBug.StartTimer("Plants");
            float[] pos = gameObjects[0].GetComp<Model>().GetVerticesWorldPosition();
            for(int i = 0; i < pos.Length -1;i += 8)
            {
                if(RNG.ZeroToOne() < 0.035f && pos[i +4] > 0.8f) //pos[i + 1] < -3f && 
                {
                    gameObjects.Add(GameObject.CreateFromFile(new Vector3(pos[i], pos[i +1] - 0.05f, pos[i +2]), new Vector3(0f, RNG.ZeroToMax(360f), 0f), Vector3.One * RNG.MinToMax(0.5f, 1.5f), AssetManager.GetMaterial("Mat_Plant01"), "post_apocalyptic_plant02"));
                }
            }
            BBug.StopTimer();
            
            */
            #endregion

            #region RENDERER_AND_LAYERS
            //Background-Color ---------------------------------------------------------------------------------------------------------------------
            //Renderer.bgCol = new Vector3((float)166 / 255, (float)222 / 255, (float)255 / 255); //light-blue
            //Renderer.bgCol = new Vector3((float)31 / 255, (float)54 / 255, (float)99 / 255); //dark-blue
            Renderer.bgCol = new Vector3((float)10 / 255, (float)16 / 255, (float)25 / 255); //darker-blue

            //instatiates mainCam setting position and rotation, set the CameraMode
            //CameraMode isn't eficient at all, but convienient for testing use singular camera-type / strategy-switching for actual game
            Renderer.mainCam = new Camera3D(new Vector3(-20f, 0f, 0f), new Vector3(0f, 0f, 0f), Camera3D.CameraMode.Fly);

            //adds a layer for 3d-objects and submits the created objs in 'gameObjects' to that layer
            //Renderer.AddLayerSetActive(mainLayer3D);
            //Renderer.Submit(gameObjects);

            BBug.Log();
            //mainLayer3D.SetRenderObjects(gameObjects);
            //Scene scene = new Scene(new Layer[] { mainLayer3D });
            //Serializer.SerializeScene(scene, DataManager.projectAssetsFilePath + @"\Scenes\Scene.scene");
            BBug.StartTimer("Deserialize");
            Scene scene = SceneManager.LoadScene("Scene");
            BBug.StopTimer();
            SceneManager.Init(scene);
            BBug.Log();

            #region FRONT_LAYER_AXIS_ARROWS
            //front layer
            //Layer3D frontLayer = new Layer3D();

            //adds a layer to the front of the layerstack which makes it get rendered ontop of mainLayer3D
            //Renderer.AddLayerSetActive(frontLayer);
            //Renderer.Submit(new GameObject[]
            //{
            //    GameObject.CreateFromFile(new Vector3(0f, 0f, 0f), Vector3.Zero, Vector3.One * .5f, AssetManager.GetMaterial("Mat_Default"), @"\Gizmos\arrow3_bevel03"),
            //    GameObject.CreateFromFile(new Vector3(0f, 0f, 0f), Vector3.Zero, Vector3.One * .5f, AssetManager.GetMaterial("Mat_Default"), @"\Gizmos\arrow3_texts")
            //});
            #endregion
            
            #endregion

            #region BITS_GUI
            //Renderer.bgCol = new Vector3(1.0f, 0.1f, 0.525f);
            mainLayerUI.Init();
            mainLayerUI.AddText("FPS", "Hello, World!", 25f, 40f, 0.75f, Vector3.One);
            mainLayerUI.AddText("DISCL", "Hello, World!", 25f, 75f, 0.5f, Vector3.One);
            mainLayerUI.AddText("VERTS", "Verts in Scene: ", 25f, 110f, 0.5f, Vector3.One);     
            
            mainLayerUI.AddText("DEBUG_ONE", "DEBUG: ", 25f, 145f, 0.5f, Vector3.One);     
            
            Renderer.AddLayer(mainLayerUI);
            #endregion

            BBug.Log();
            BBug.StopTimer();
            BBug.Log("\n---Start()_Done---\n");
        }

        protected override void Update()
        {
            if (Input.IsPressed(KeyCode.R))
            {
                AssetManager.ReloadMaterial("Mat_CelShading");
                BBug.Log("\nReloaded Cel Shading Material");
            }

            if (Input.IsPressed(KeyCode.C))
            { 
                BBug.Log("\nGameObject Components: ");
                foreach(Component comp in mainLayer3D.gameObjects[5].components)
                {
                    BBug.Log("Component: " + comp);
                }
            }

            #region LIGHT

            //light moves with cam
            if (Renderer.lightSources != null && Renderer.lightSources.Length > 0)
            {
                float speed = GameTime.DeltaTime * 50f;

                #region DIR_LIGHT
                int dirLightID = 0;
                if (Input.IsDown(KeyCode.Numpad5))
                {
                    Renderer.lightSources[dirLightID].gameObject.transform.Rotate( Vector3.UnitZ * speed);
                }
                else if (Input.IsDown(KeyCode.Numpad2))
                {
                    Renderer.lightSources[dirLightID].gameObject.transform.Rotate(-Vector3.UnitZ * speed);
                }
                else if (Input.IsDown(KeyCode.Numpad1))
                {
                    Renderer.lightSources[dirLightID].gameObject.transform.Rotate(Vector3.UnitX * speed);
                }
                else if (Input.IsDown(KeyCode.Numpad3))
                {
                    Renderer.lightSources[dirLightID].gameObject.transform.Rotate(-Vector3.UnitX * speed);
                }
                #endregion

                #region POINT_LIGHT
                int pointlightID = 2;
                if (Input.IsDown(KeyCode.NumpadDivide))
                {
                    Renderer.lightSources[pointlightID].gameObject.transform.Move(Vector3.UnitX * speed * 0.2f);
                }
                else if (Input.IsDown(KeyCode.Numpad8))
                {
                    Renderer.lightSources[pointlightID].gameObject.transform.Move(-Vector3.UnitX * speed * 0.2f);
                }
                else if (Input.IsDown(KeyCode.Numpad9))
                {
                    Renderer.lightSources[pointlightID].gameObject.transform.Move(Vector3.UnitZ * speed * 0.2f);
                }
                else if (Input.IsDown(KeyCode.Numpad7))
                {
                    Renderer.lightSources[pointlightID].gameObject.transform.Move(-Vector3.UnitZ * speed * 0.2f);
                }
                else if (Input.IsDown(KeyCode.NumLock))
                {
                    Renderer.lightSources[pointlightID].gameObject.transform.Move(Vector3.UnitY * speed * 0.2f);
                }
                else if (Input.IsDown(KeyCode.NumpadMultiply))
                {
                    Renderer.lightSources[pointlightID].gameObject.transform.Move(-Vector3.UnitY * speed * 0.2f);
                }
                #endregion

                #region SPOT_LIGHT
                int spotlightID = 4;
                if (Input.IsDown(KeyCode.Home))
                {
                    Renderer.lightSources[spotlightID].gameObject.transform.Move(Vector3.UnitX * speed * 0.2f);
                }
                else if (Input.IsDown(KeyCode.End))
                {
                    Renderer.lightSources[spotlightID].gameObject.transform.Move(-Vector3.UnitX * speed * 0.2f);
                }
                else if (Input.IsDown(KeyCode.PageDown))
                {
                    Renderer.lightSources[spotlightID].gameObject.transform.Move(Vector3.UnitZ * speed * 0.2f);
                }
                else if (Input.IsDown(KeyCode.Delete))
                {
                    Renderer.lightSources[spotlightID].gameObject.transform.Move(-Vector3.UnitZ * speed * 0.2f);
                }
                else if (Input.IsDown(KeyCode.Insert))
                {
                    Renderer.lightSources[spotlightID].gameObject.transform.Move(Vector3.UnitY * speed * 0.2f);
                }
                else if (Input.IsDown(KeyCode.PageUp))
                {
                    Renderer.lightSources[spotlightID].gameObject.transform.Move(-Vector3.UnitY * speed * 0.2f);
                }
                #endregion
            }
            //{ Renderer.BaseLayer3D.LightObjects[0].transform.Rotate(Vector3.UnitZ * MathF.Sin(GameTime.TotalElapsedSeconds)); } 
            //{ Renderer.BaseLayer3D.LightObjects[0].transform.position = Renderer.mainCam.transform.position; }
            #endregion

            #region INPUT
            //Input------------------------------------------------------------------------------------------------------------------------------------            
            //cam movement
            if (!paused) { Renderer.mainCam.MoveByInput(); }

            //'enter' or 'F5' closes window
            if (Input.IsPressed(KeyCode.Enter) || Input.IsPressed(KeyCode.F5))
            {
                DisplayManager.CloseWindow();
            }

            //pause / un-pause the game
            if (Input.IsPressed(KeyCode.Escape))
            {
                paused = !paused;


                if (!paused)
                {
                    GameTime.TimeScale = 1.0f;
                }
                else
                {
                    GameTime.TimeScale = 0.0f;
                }
            }

            //switch between wireframe and regular with tab
            if (Input.IsPressed(KeyCode.Tab))
            {
                Renderer.wireFrameMode = !Renderer.wireFrameMode;
                Renderer.normalMode = false;
                Renderer.uvMode = false;
            }

            //switch between normal-mode and regular with 'N'
            if (Input.IsPressed(KeyCode.N))
            {
                Renderer.wireFrameMode = false;
                Renderer.uvMode = false;

                Renderer.normalMode = !Renderer.normalMode;
            }

            //switch between c uv-mode and regular with 'U'
            if (Input.IsPressed(KeyCode.U))
            {
                Renderer.wireFrameMode = false;
                Renderer.normalMode = false;

                Renderer.uvMode = !Renderer.uvMode;
            }

            //mouse
            if (Input.IsMousePressed(MouseButton.Right)) { DisplayManager.SetCursorPos(DisplayManager.WindowSize.X * 0.5f, DisplayManager.WindowSize.Y * 0.5f); }
            else if (!paused && Input.IsMouseDown(MouseButton.Right))
            {
                if(!mouseMoveLastFrame)
                {
                    mouseMoveLastFrame = true;
                }
                else
                {
                    Renderer.mainCam.FollowMouse(Input.mouseDelta.X, Input.mouseDelta.Y);

                    DisplayManager.SetCursorVisible(false);
                }
            }
            else if(!paused)
            { 
                DisplayManager.SetCursorVisible(true);
                mouseMoveLastFrame = false;
                //DisplayManager.SetCursorPos(DisplayManager.WindowSize.X * 0.5f, DisplayManager.WindowSize.Y * 0.5f); 
            }

            //BBug.Log("Pos: " + mainCam.transform.position.ToString() + ", Dir: " + mainCam.transform.GetForwardDir().ToString());

            #endregion

            #region WINDOW_TITLE
            //fps in window-title
            fpsT += GameTime.DeltaTime;
            fpsTicksCounterUpdate++;
            if (fpsT > 1f)
            {
                curFps = (int)MathF.Floor(fpsTicksCounterUpdate / fpsT);
                fpsTicksCounterUpdate = 0;
                fpsT = 0f;
            }
            //add fps-counter and title disclaimer to the window title
            string titleDiscalimer = paused ? " - PAUSED" : (Renderer.wireFrameMode ? " - WIREFRAME" : (Renderer.normalMode ? " - NORMALS" : (Renderer.uvMode ? " - UV_COORDS" : "")));
            DisplayManager.SetWindowTitle("TestEnvironment - " + curFps.ToString() + " fps" + titleDiscalimer);
            mainLayerUI.SetText("FPS", "FPS: " + curFps);
            mainLayerUI.SetText("DISCL", "Mode: " + (titleDiscalimer == "" ? " - Default" : titleDiscalimer));
            #endregion
        }

        private void LoadMaterials()
        {
            //http://devernay.free.fr/cours/opengl/materials.html good phong-materials values

            #region SHADERS
            //Basic
            AssetManager.AddShader("Shader_BasicBasic", "Basic.vert", "Basic.frag");
            AssetManager.AddShader("Shader_BasicNormal", "Basic.vert", "Normal.frag");
            AssetManager.AddShader("Shader_BasicUV", "Basic.vert", "UV.frag");

            //Basic_Phong
            AssetManager.AddShader("Shader_BasicPhong", "Basic.vert", "Lighting.frag");
            AssetManager.AddShader("Shader_BasicPhongTop", "Basic.vert", "LightingTop.frag");
            AssetManager.AddShader("Shader_BasicPhongTerrain", "Basic.vert", "LightingTerrain.frag");

            //Textured
            AssetManager.AddShader("Shader_Textured", "Basic.vert", "PhongTextured.frag");
            AssetManager.AddShader("Shader_TexturedTop", "Basic.vert", "PhongTexturedTop.frag");
            AssetManager.AddShader("Shader_TexturedCel", "Basic.vert", "CelShadingTextured.frag");
            #endregion

            #region BASIC
            //declare materials-----------------------------------------------------------------------------------------------------------------------------------------
            //defaults
            AssetManager.AddMaterial("Mat_Wireframe", new UnlitMaterial(AssetManager.GetShader("Shader_BasicBasic"), new Vector3(1f, 1f, 1f))); //wireframe
            AssetManager.AddMaterial("Mat_Normal", new BasicPhongMaterial(AssetManager.GetShader("Shader_BasicNormal"), new Vector3(0.5f, 0.5f, 0.5f), new Vector3(0.5f, 0.5f, 0.5f), new Vector3(0.08f, 0.08f, 0.08f), 1.0f)); //normals
            AssetManager.AddMaterial("Mat_UV", new BasicPhongMaterial(AssetManager.GetShader("Shader_BasicUV"), new Vector3(0.5f, 0.5f, 0.5f), new Vector3(0.5f, 0.5f, 0.5f), new Vector3(0.08f, 0.08f, 0.08f), 1.0f)); //

            //basic materials
            AssetManager.AddMaterial("Mat_DefaultLight", new UnlitMaterial(AssetManager.GetShader("Shader_BasicBasic"), new Vector3(1.0f, 1.0f, 1.0f))); //basic light

            AssetManager.AddMaterial("Mat_Default", new BasicPhongMaterial(
                AssetManager.GetShader("Shader_BasicPhong"),
                new Vector3(0.3f, 0.3f, 0.3f), new Vector3(0.5f, 0.5f, 0.5f), new Vector3(0.074597f, 0.074597f, 0.074597f), 0.15f)); //base-mat
            
            AssetManager.AddMaterial("Mat_Chrome", new BasicPhongMaterial(
                AssetManager.GetShader("Shader_BasicPhong"), 
                new Vector3(0.25f, 0.25f, 0.25f), new Vector3(0.4f, 0.4f, 0.4f), new Vector3(0.774597f, 0.774597f, 0.774597f), 0.6f)); //chrome
            
            AssetManager.AddMaterial("Mat_GreenRubber", new BasicPhongMaterial(
                AssetManager.GetShader("Shader_BasicPhong"), 
                new Vector3(0f, 0.05f, 0.0f), new Vector3(0.4f, 0.5f, 0.4f), new Vector3(0.04f, 0.7f, 0.04f), 0.078125f)); //green-rubber

            AssetManager.AddMaterial("Mat_TopColored", new BasicPhongMaterial(
                AssetManager.GetShader("Shader_BasicPhongTop"),
                new Vector3(0.3f, 0.3f, 0.3f), new Vector3(0.5f, 0.5f, 0.5f), new Vector3(0.074597f, 0.074597f, 0.074597f), 0.15f)); //top colored green
            
            AssetManager.AddMaterial("Mat_TerrainBasic", new BasicPhongMaterial(
                AssetManager.GetShader("Shader_BasicPhongTerrain"), 
                new Vector3(0.3f, 0.4f, 0.05f), new Vector3(0.6f, 0.8f, 0.1f), new Vector3(0.08f, 0.08f, 0.01f), 0.015f)); //terrain
            #endregion

            #region TEXTURED
            //textured materials
            AssetManager.AddMaterial("Mat_Brick01", new TexturedPhongMaterial(
                AssetManager.GetShader("Shader_Textured"),
                AssetManager.GetTexture("brick01_dif"), AssetManager.GetTexture("brick01_spec"), 1f, .5f, .5f));

            AssetManager.AddMaterial("Mat_Crate01", new TexturedPhongMaterial(
                AssetManager.GetShader("Shader_Textured"),
                AssetManager.GetTexture("crate01_dif"), AssetManager.GetTexture("crate01_spec"), 1f));

            AssetManager.AddMaterial("Mat_Cliff01", new TexturedPhongMaterial(
                AssetManager.GetShader("Shader_Textured"),
                AssetManager.GetTexture("cliff01_dif"), AssetManager.GetTexture("cliff01_spec"), 1f, 6f, 6f));

            AssetManager.AddMaterial("Mat_Grass01", new TexturedPhongMaterial(
                AssetManager.GetShader("Shader_Textured"),
                AssetManager.GetTexture("grass01_dif"), AssetManager.GetTexture("grass01_spec"), 1f, 6f, 6f));

            AssetManager.AddMaterial("Mat_GrassBillboard01", new TexturedPhongMaterial(
                AssetManager.GetShader("Shader_Textured"),
                AssetManager.GetTexture("grass_billboard01_dif"), AssetManager.GetTexture("grass_billboard01_spec"), 1f));

            AssetManager.AddMaterial("Mat_StainedGlass01", new TexturedPhongMaterial(
                AssetManager.GetShader("Shader_Textured"),
                AssetManager.GetTexture("stained_glass01_dif"), AssetManager.GetTexture("stained_glass01_spec"), 1f));

            AssetManager.AddMaterial("Mat_Shotgun01", new TexturedPhongMaterial(
                AssetManager.GetShader("Shader_Textured"),
                AssetManager.GetTexture("shotgun01_dif_ao"), AssetManager.GetTexture("shotgun01_spec"), 1f));

            AssetManager.AddMaterial("Mat_Wall01", new TexturedPhongMaterial(
                AssetManager.GetShader("Shader_Textured"),
                AssetManager.GetTexture("wall01_dif"), AssetManager.GetTexture("wall01_spec"), 1f));

            AssetManager.AddMaterial("Mat_Barrel01", new TexturedPhongMaterial(
                AssetManager.GetShader("Shader_Textured"),
                AssetManager.GetTexture("barrel01_dif"), AssetManager.GetTexture("barrel01_spec"), 1f));

            AssetManager.AddMaterial("Mat_Plant01", new TexturedPhongMaterial(
                AssetManager.GetShader("Shader_Textured"),
                AssetManager.GetTexture("plant01_dif"), AssetManager.GetTexture("plant01_dif"), 1f));

            AssetManager.AddMaterial("Mat_Terrain01", new TexturedPhongMaterialTop(
                AssetManager.GetShader("Shader_TexturedTop"),
                new PhongMaterialSettings(
                    AssetManager.GetTexture("grass01_dif"),
                    AssetManager.GetTexture("grass01_spec"),
                    5f, 5f),
                new PhongMaterialSettings(
                    AssetManager.GetTexture("cliff01_dif"),
                    AssetManager.GetTexture("cliff01_spec"),
                    6f, 6f)));
            #endregion

            AssetManager.AddMaterial("Mat_UV-Checkered", new TexturedPhongMaterial(
                AssetManager.GetShader("Shader_Textured"),
                AssetManager.GetTexture("cel_crate01_dif"), AssetManager.GetTexture("blank"), 1f)); //UV_Checkered

            //cell shading / style experimentation
            AssetManager.AddMaterial("Mat_CelShading", new TexturedCelShadingMaterial(
                AssetManager.GetShader("Shader_TexturedCel"),
                AssetManager.GetTexture("cel_crate01_dif"), AssetManager.GetTexture("blank"),
                new LightLevelSettings[]
                {
                    new LightLevelSettings(0.0f, 0.15f, 0.1f, new Vector3(1.2f, 1.0f, 3.0f)),
                    new LightLevelSettings(0.15f, 0.5f, 0.5f, new Vector3(1.2f, 1.0f, 1.5f)),
                    new LightLevelSettings(0.5f, 2.0f, 0.8f, new Vector3(1.2f, 1.0f, 1.2f)),
                    //new LightLevelSettings(0.0f, 2.0f, 1.0f),
                },
                1f));
        }
    }

}
