using System;
using System.Collections.Generic;
using BitsCore.Rendering.Display;
using BitsCore.Rendering.Cameras;
using BitsCore.ObjectData;
using System.Numerics;
using BitsCore.ObjectData.Materials;
using BitsCore.ObjectData.Components;
using BitsCore.DataManagement;
using BitsCore.Rendering;
using BitsCore.Utils;
using BitsCore.Rendering.Layers;
using BitsCore.Debugging;
using BitsCore;
using BitsCore.InputSystem;
using BeSafe.Scripts;

namespace BeSafe
{
    /// <summary> The TestGame, derived from the Game Class. Used for testing stuff. </summary>
    public class BeSafeApp : Application
    {

        bool paused = false;
        bool mouseMoveLastFrame = false;
        int curFps;
        float fpsT;
        int fpsTicksCounterUpdate = 0;

        public List<RectCollider> environCollider = new List<RectCollider>(); //used by PlayerController

        /// <summary> Generates an Instance of the derived Application class. </summary>
        public BeSafeApp(int initialWindowWidth, int initialWindowHeight, string initialWindowTitle, bool initialMaximizeWindow = false) : base(initialWindowWidth, initialWindowHeight, initialWindowTitle, initialMaximizeWindow)
        {
        }

        protected override void Start()
        {
            BBug.SetPrintLog(true);
            BBug.Log("\n--------Application--------\n");
            BBug.StartTimer("Start");

            LoadMaterials(); //loads textures & materials into the assetmanager

            #region 3D_SCENE

            BBug.StartTimer("GameObject creation");
            //asynchrounsly fills the 'gameObjects' array
            //MakeGameObjects().Wait();
            //gameObjects = new GameObject[] { GameObject.CreateGrid(200, 200, new Vector3(-2f, -2f, 2.75f), Vector3.Zero, Vector3.One * 20f, MaterialLibrary.terrain, true, .5f, 0.02f) };
            List<GameObject> gameObjects = new List<GameObject>();

            gameObjects.Add(GameObject.CreateFromFile(new Vector3(0f, 1f, -12f), Vector3.Zero, Vector3.One, AssetManager.GetMaterial("Mat_CelShading"), "sphere_poles")); //sphere

            gameObjects.Add(GameObject.CreateFromFile(new Vector3(0f, 1f, -8f), Vector3.Zero, Vector3.One, AssetManager.GetMaterial("Mat_CelShading"), "Cel_Crate01"));

            gameObjects.Add(GameObject.CreateFromFile(new Vector3(1.5f, 0f, 0f), Vector3.UnitY * -90f, Vector3.One, AssetManager.GetMaterial("Mat_CelShading"), "Coffe_Store"));
            environCollider.Add(new RectCollider(-10, -10, 20, 10));
            gameObjects.Add(GameObject.CreateFromFile(new Vector3(1.5f, 0f, 0f), Vector3.UnitY * -90f, Vector3.One, AssetManager.GetMaterial("Mat_CelShading"), "Ground"));
            environCollider.Add(new RectCollider(2, 0, 8, 4));

            gameObjects.Add(GameObject.CreateFromFile(new Vector3(0f, 8f, -6f), Vector3.UnitY * 180f, Vector3.One, AssetManager.GetMaterial("Mat_CelShading"), "robot01_LD"));
            gameObjects[gameObjects.Count - 1].AddComp(new PlayerController(1, 2)); //add script-comp

            BBug.StartTimer("Lights creation");
            gameObjects.Add(GameObject.CreateDirectionalLight(new Vector3(0.5f, 4f, 0f), new Vector3(20f, -30f, 0f), Vector3.One, AssetManager.GetMaterial("Mat_DefaultLight"), new Vector3(1.0f, 1.0f, 1.0f), 0.5f)); //0.5f
            BBug.StopTimer();

            foreach(RectCollider rect in environCollider)
            {
                gameObjects.Add(GameObject.CreateFromFile(new Vector3(0f, rect.yPos + (rect.height * 0.5f), rect.xPos + (rect.width * 0.5f)), Vector3.Zero, new Vector3(0.1f, rect.height * 0.5f, rect.width * 0.5f), AssetManager.GetMaterial("Mat_DefaultLight"), "cube"));
                gameObjects.Add(GameObject.CreateFromFile(new Vector3(0f, rect.yPos, rect.xPos), Vector3.Zero, new Vector3(0.25f, 0.25f, 0.25f), AssetManager.GetMaterial("Mat_Default"), "cube"));
            }

            BBug.StopTimer();
            #endregion

            #region RENDERER_AND_LAYERS
            //Background-Color ---------------------------------------------------------------------------------------------------------------------
            Renderer.bgCol = new Vector3((float)166 / 255, (float)222 / 255, (float)255 / 255); //light-blue
            //Renderer.bgCol = new Vector3((float)10 / 255, (float)16 / 255, (float)25 / 255); //darker-blue

            //instatiates mainCam setting position and rotation, set the CameraMode
            Renderer.mainCam = new Camera3D(new Vector3(-30f, 18f, 0f), new Vector3(0f, 0f, 0f), Camera3D.CameraMode.Fly);


            //adds a layer for 3d-objects and submits the created objs in 'gameObjects' to that layer
            Renderer.AddLayerSetActive(mainLayer3D);
            Renderer.Submit(gameObjects);
            #endregion

            #region FRONT_LAYER_AXIS_ARROWS
            //front layer
            Layer3D frontLayer = new Layer3D();

            //adds a layer to the front of the layerstack which makes it get rendered ontop of mainLayer3D
            Renderer.AddLayerSetActive(frontLayer);
            Renderer.Submit(new GameObject[]
            {
                GameObject.CreateFromFile(new Vector3(0f, 0f, 0f), Vector3.Zero, Vector3.One * .5f, AssetManager.GetMaterial("Mat_DefaultLight"), @"arrow3_bevel03"),
                GameObject.CreateFromFile(new Vector3(0f, 0f, 0f), Vector3.Zero, Vector3.One * .5f, AssetManager.GetMaterial("Mat_DefaultLight"), @"arrow3_texts")
            });
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

            #region COLLISION_TESTING
            BBug.Log("---Line-Overlap-Test---\n");

            BBug.Log("startOne: 0, endOne: 100, startTwo: 20, endTwo: 40");
            BBug.Log("  -> " + RectCollider.OneD_LineOverlap(0, 100, 20, 40) + "\n");

            BBug.Log("startOne: 0, endOne: 10, startTwo: 20, endTwo: 30");
            BBug.Log("  -> " + RectCollider.OneD_LineOverlap(0, 10, 20, 30) + "\n");

            BBug.Log("startOne: 30, endOne: 100, startTwo: 20, endTwo: 40");
            BBug.Log("  -> " + RectCollider.OneD_LineOverlap(30, 100, 20, 40) + "\n");

            BBug.Log("---Line-Overlap-Test_Done---\n");


            BBug.Log("---Rect-Overlap-Test---\n");

            RectCollider rect_one = new RectCollider(0,     0,      10,     10);
            RectCollider rect_two = new RectCollider(-10f,  -10f,   20,     20);
            BBug.Log("Rect Collision: ");
            Collision collision = RectCollider.CheckCollision(rect_one, rect_two);
            BBug.Log("  -> X: " + collision.xOverlap + ", Y: " + collision.yOverlap);

            BBug.Log("\n---Rect-Overlap-Test_Done---\n");
            #endregion
        }

        protected override void Update()
        {
            //reload the shader
            if (Input.IsPressed(KeyCode.R))
            {
                AssetManager.ReloadMaterial("Mat_CelShading");
                BBug.Log("\nReloaded Cel Shading Material");
            }

            #region LIGHT

            //light moves with cam
            if (Renderer.lightSources != null && Renderer.lightSources.Length > 0)
            {
                float speed = GameTime.DeltaTime * 50f;

                int dirLightID = 0;
                if (Input.IsDown(KeyCode.Numpad5))
                {
                    Renderer.lightSources[dirLightID].gameObject.transform.Rotate(Vector3.UnitZ * speed);
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
                if (!mouseMoveLastFrame)
                {
                    mouseMoveLastFrame = true;
                }
                else
                {
                    Renderer.mainCam.FollowMouse(Input.mouseDelta.X, Input.mouseDelta.Y);

                    DisplayManager.SetCursorVisible(false);
                }
            }
            else if (!paused)
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
            #endregion

            AssetManager.AddMaterial("Mat_UV-Checkered", new TexturedPhongMaterial(
                AssetManager.GetShader("Shader_Textured"),
                AssetManager.GetTexture("UV_Checkered"), AssetManager.GetTexture("blank"), 1f)); //UV_Checkered

            //cell shading / style experimentation
            AssetManager.AddMaterial("Mat_CelShading", new TexturedCelShadingMaterial(
                AssetManager.GetShader("Shader_TexturedCel"),
                AssetManager.GetTexture("blank"), AssetManager.GetTexture("blank"), //cel_crate01_dif
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
