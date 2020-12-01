using BitsCore.Rendering.Display;
using System;
using BitsCore.DataManagement;
using BitsCore.ObjectData.Materials;
using BitsCore.Rendering;
using BitsCore.Events_Input;
using BitsCore.Rendering.Layers;
using BitsCore.Debugging;
using BitsCore.InputSystem;

namespace BitsCore
{
    /// <summary> The abstract Class different Games can be derived from. </summary>
    public abstract class Application
    {
        #region VARS
        /// <summary> 
        /// The Platform the application gets set to.
        /// <para> All platform-specific code relies on this value/the symbols that get set because of it. </para>
        /// <para> Not implemented yet !!! </para>
        /// </summary>
        public Platform platform;

        /// <summary> The width of the window on startup. </summary>
        protected int InitialWindowWidth { get; private set; }
        /// <summary> The height of the window on startup. </summary>
        protected int InitialWindowHeight { get; private set; }
        /// <summary> The title of the window on startup. </summary>
        protected string InitialWindowTitle { get; private set; }
        protected bool InitialMaximizeWindow { get; private set; }

        public Layer3D mainLayer3D;
        public LayerUI mainLayerUI;
        #endregion

        /// <summary> Generates a Game Class. This class calls Initialize(), LoadContent(), Update() and Render() functions.</summary>
        /// <param name="initialWindowWidth"> The width of the window on startup. </param>
        /// <param name="initialWindowHeight"> The height of the window on startup. </param>
        /// <param name="initialWindowTitle"> The title of the window on startup. </param>
        public Application(int initialWindowWidth, int initialWindowHeight, string initialWindowTitle, bool initialMaximizeWindow = false)
        {
            this.InitialWindowWidth = initialWindowWidth;
            this.InitialWindowHeight = initialWindowHeight;
            this.InitialWindowTitle = initialWindowTitle;
            this.InitialMaximizeWindow = initialMaximizeWindow;

            mainLayer3D = new Layer3D();
            mainLayerUI = new LayerUI();

            GameTime.TimeScale = 1.0f;
        }

        /// <summary> Starts the game, calls the Initialize() and LoadContent() function and then enters the Update()-Render() loop. </summary>
        public void Run()
        {
            DataManager.Init(); //assets-path and root-path
            Init(); //init the application
            EventManager.InvokeInitializeEvent(); //same as if(calledInitialize != null) { calledInitialize(); }, checks if event is assigned anywhere and if so invokes it

            //the order shouldn't be changed
            DisplayManager.CreateWindow(InitialWindowWidth, InitialWindowHeight, InitialWindowTitle, InitialMaximizeWindow);
            Renderer.Init(this); //sets up the renderer
            EventManager.Init(); //sets up the EventManager, creates the callbacks
            InputManager.Init(); //sets up the InputManager
            BitsCoreContext.Init(); //sets the system-info vars, such as gpu-name

            //the order shouldn't be changed
            AssetManager.Init(); //gathers references to all the assets in the 'assets' folder
            TextRenderer.Init(48); //loads font, shader, vao, vbo, etc.

            Start();
            EventManager.InvokeLoadContentEvent(); //same as if(calledLoadContent != null) { calledLoadContent(); }, checks if event is assigned anywhere and if so invokes it

            Renderer.SetupAssets();

            while (!DisplayManager.ShouldClose())
            {
                GameTime.UnscaledDeltaTime = ((float)DisplayManager.GetTime() - GameTime.TotalElapsedSeconds);
                GameTime.DeltaTime = ((float)DisplayManager.GetTime() - GameTime.TotalElapsedSeconds) * GameTime.TimeScale;
                GameTime.TotalElapsedSeconds = (float)DisplayManager.GetTime();

                Update();
                EventManager.InvokeUpdateEvent(); //same as if(calledUpdate != null) { calledUpdate(); }, checks if event is assigned anywhere and if so invokes it

                InputManager.ResetLastFrameKeyButtonStates();

                EventManager.GatherEvents(); //tells Windows the program is still responding and hasn't crashed, also checks for input/other events

                OnRender();
                EventManager.InvokeRenderEvent(); //same as if(calledRender != null) { calledRender(); }, checks if event is assigned anywhere and if so invokes it
            }

            BBug.PrintLogToFile(); //generates a .txt file with the debug log, if it was activated 

            DisplayManager.TerminateWindow();
        }

        /// <summary> Function to initialize the Application. </summary>
        protected virtual void Init() { }
        /// <summary> Function to load and set up Content, Variables, etc. for the Application. </summary>
        protected virtual void Start() { }

        /// <summary> Function update the Application-State, gets called each Frame. </summary>
        protected virtual void Update() { }

        //OnPhysics should also happen here/in Update()

        /// <summary> 
        /// Function renders the active scene , gets called each Frame. 
        /// <para> Only override if you want to change the way the scene gets rendered. </para>
        /// </summary>
        protected virtual void OnRender()
        {
            Renderer.RenderScene();
        }

    }

}
