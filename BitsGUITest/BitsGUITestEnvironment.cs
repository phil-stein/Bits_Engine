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

namespace BitsGUITest
{
    /// <summary> The TestGame, derived from the Game Class. Used for testing stuff. </summary>
    public class BitsGUITestEnvironment : Application
    {
        float totalX;
        float totalY;
        bool paused = false;
        bool lightXPositive = true;
        bool lightYPositive = true;
        bool lightZPositive = true;
        int curFps;
        float fpsT;
        int fpsTicksCounterUpdate = 0;

        /// <summary> Generates an Instance of the derived Application class. </summary>
        public BitsGUITestEnvironment(int initialWindowWidth, int initialWindowHeight, string initialWindowTitle, bool initialMaximizeWindow = false) : base(initialWindowWidth, initialWindowHeight, initialWindowTitle, initialMaximizeWindow)
        {
        }

        protected override void Start()
        {
            BBug.Log("\n--------Application--------\n");
            BBug.StartTimer("LoadContent");
            
            LoadMaterials();

            #region RENDERER_AND_LAYERS
            //Background-Color ---------------------------------------------------------------------------------------------------------------------
            //Renderer.bgCol = new Vector3((float)166 / 255, (float)222 / 255, (float)255 / 255); //light-blue
            //Renderer.bgCol = new Vector3((float)31 / 255, (float)54 / 255, (float)99 / 255); //dark-blue
            Renderer.bgCol = new Vector3((float)10 / 255, (float)16 / 255, (float)25 / 255); //darker-blue

            //instatiates mainCam setting position and rotation, set the CameraMode
            //CameraMode isn't eficient at all, but convienient for testing use singular camera-type / strategy-switching for actual game
            Renderer.mainCam = new Camera3D(new Vector3(-20f, 0f, 0f), new Vector3(0f, 0f, 0f), Camera3D.CameraMode.Fly);

            #region FRONT_LAYER_AXIS_ARROWS
            //front layer
            Layer3D frontLayer = new Layer3D();

            //adds a layer to the front of the layerstack which makes it get rendered ontop of mainLayer3D
            //Renderer.AddLayerSetActive(frontLayer);
            //Renderer.Submit(new GameObject[]
            //{
            //    GameObject.CreateFromFile(new Vector3(0f, 0f, 0f), Vector3.Zero, Vector3.One * .5f, AssetManager.GetMaterial("Mat_DefaultLight"), @"\Gizmos\arrow3_bevel03"),
            //    GameObject.CreateFromFile(new Vector3(0f, 0f, 0f), Vector3.Zero, Vector3.One * .5f, AssetManager.GetMaterial("Mat_DefaultLight"), @"\Gizmos\arrow3_texts")
            //});
            #endregion

            #endregion

            #region BITS_GUI
            mainLayerUI.Init();
            mainLayerUI.AddText("FPS", "Hello, World!", 25f, 40f, 0.0f, Vector3.One); //0.75f
            mainLayerUI.AddText("DISCL", "Hello, World!", 25f, 75f, 0.0f, Vector3.One); //0.5f
            mainLayerUI.AddText("VERTS", "Verts in Scene: ", 25f, 110f, 0.0f, Vector3.One); //0.5f

            mainLayerUI.AddText("DEBUG_ONE", "DEBUG: ", 25f, 145f, 0.0f, Vector3.One); //0.5f

            //header container
            ContainerSettings settings = new ContainerSettings();
            settings.items = new List<Item>() { BitsGUIstd.std_header };
            settings.containerAlignment = Alignment.TopLeft;
            settings.containerOrder = Order.Fill;

            settings.xPosition = 0f;
            settings.yPosition = 0f;

            settings.width = 1.0f;
            settings.height = 0.35f;
            Container header = new Container(settings);
            
            //body container
            settings = new ContainerSettings();
            settings.items = new List<Item>() { BitsGUIstd.std_dark };
            settings.containerAlignment = Alignment.TopLeft;
            settings.containerOrder = Order.Fill;

            settings.xPosition = 0f;
            settings.yPosition = 0f;

            settings.width = 1.0f;
            settings.height = 1f;
            Container body = new Container(settings);

            //main container
            settings = new ContainerSettings();
            //settings.items = new List<Item>() { BitsGUIstd.std_bg }; //add bg to the container
            settings.subSontainer = new List<Container>() { header, body };

            settings.containerAlignment = Alignment.TopLeft;
            settings.containerOrder = Order.VerticalDescending;
            
            settings.xPosition = 0.5f;
            settings.yPosition = 0.5f;
            
            settings.width = 0.25f;
            settings.height = 0.25f;
            
            Container container = new Container(settings);
            mainLayerUI.AddContainer(container);            

            Renderer.AddLayer(mainLayerUI);
            #endregion

            BBug.StopTimer();
            BBug.Log("\n---LoadContent()_Done---\n");
        }

        protected override void Update()
        {
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

            //switch between wireframe and solid with tab
            if (Input.IsPressed(KeyCode.Tab))
            {
                Renderer.wireFrameMode = !Renderer.wireFrameMode;
                Renderer.normalMode = false;
                Renderer.uvMode = false;
            }

            //switch to to normal-mode and regular with 'N'
            if (Input.IsPressed(KeyCode.N))
            {
                Renderer.wireFrameMode = false;
                Renderer.uvMode = false;

                Renderer.normalMode = !Renderer.normalMode;
            }

            //switch to to uv-mode and regular with 'U'
            if (Input.IsPressed(KeyCode.U))
            {
                Renderer.wireFrameMode = false;
                Renderer.normalMode = false;

                Renderer.uvMode = !Renderer.uvMode;
            }

            //mouse
            if (Input.IsMousePressed(MouseButton.Right)) { DisplayManager.SetCursorPos(DisplayManager.WindowSize.X / 2, DisplayManager.WindowSize.Y / 2); }
            if (!paused && Input.IsMouseDown(MouseButton.Right))
            {
                DisplayManager.GetCursorPos(out double xpos, out double ypos);
                DisplayManager.SetCursorPos(DisplayManager.WindowSize.X / 2, DisplayManager.WindowSize.Y / 2);
                totalX += (float)xpos - (DisplayManager.WindowSize.X / 2);
                totalY += (float)ypos - (DisplayManager.WindowSize.Y / 2);
                Renderer.mainCam.FollowMouse(totalX, totalY);

                DisplayManager.SetCursor(CursorType.Crosshair);
            }
            else
            {
                DisplayManager.SetCursor(CursorType.Arrow);
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
            #endregion

            #region BASIC_MATERIALS
            //declare materials-----------------------------------------------------------------------------------------------------------------------------------------
            //defaults
            AssetManager.AddMaterial("Mat_Wireframe", new UnlitMaterial(AssetManager.GetShader("Shader_BasicBasic"), new Vector3(1f, 1f, 1f))); //wireframe
            AssetManager.AddMaterial("Mat_Normal", new BasicPhongMaterial(AssetManager.GetShader("Shader_BasicNormal"), new Vector3(0.5f, 0.5f, 0.5f), new Vector3(0.5f, 0.5f, 0.5f), new Vector3(0.08f, 0.08f, 0.08f), 1.0f)); //normals
            AssetManager.AddMaterial("Mat_UV", new BasicPhongMaterial(AssetManager.GetShader("Shader_BasicUV"), new Vector3(0.5f, 0.5f, 0.5f), new Vector3(0.5f, 0.5f, 0.5f), new Vector3(0.08f, 0.08f, 0.08f), 1.0f)); //

            //basic materials
            AssetManager.AddMaterial("Mat_DefaultLight", new UnlitMaterial(AssetManager.GetShader("Shader_BasicBasic"), new Vector3(1.0f, 1.0f, 1.0f))); //basic light
            #endregion
        }
    }
}
