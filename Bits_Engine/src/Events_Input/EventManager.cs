using BitsCore.Rendering.Display;
using GLFW;
using System;
using System.Diagnostics;

namespace BitsCore.Events_Input
{
    public static class EventManager
    {

        #region VARS
        /// <summary> "false" before <see cref="Application.Init"/> gets called and "true" after </summary>
        public static bool initPassed;
        /// <summary> "false" before <see cref="Application.Start"/> gets called and "true" after </summary>
        public static bool startPassed;
        #endregion
        
        #region EVENTS
        //application events
        /// <summary> Event that is triggered after the Initialize() function is called. </summary>
        public static event Action calledInit;
        /// <summary> Event that is triggered after the LoadContent() function is called. </summary>
        public static event Action calledStart;
        /// <summary> Event that is triggered after the Update() function is called. </summary>
        public static event Action calledUpdate;
        /// <summary> Event that is triggered after the Render() function is called. </summary>
        public static event Action calledRender;

        //window events
        public static event EventHandler Window_Refreshed;
        public static event Action<int, int> Window_SizeChanged;
        public static event Action<double, double> Window_PositionChanged;
        public static event Action<bool> Window_FocusChanged;
        public static event Action Window_Closing;
        public static event Action<int, IntPtr> Window_FileDrop;
        public static event Action<int, int> Window_FramebufferSize;
        public static event Action<bool> Window_MaximizedChanged;
        public static event Action<float, float> Window_ContentScaleChanged;

        //input events
        public static event Action<KeyCode, InputState> Input_Key;
        public static event Action<double, double> Input_MouseMove;
        public static event Action<bool> Input_MouseEnter;
        public static event Action<MouseButton, InputState> Input_Mouse;
        public static event Action<double, double> Input_MouseScroll;
        public static event Action<uint> Input_CharInput;
        #endregion

        #region CALLBACKS
        //window callbacks
        private static PositionCallback windowPositionCallback;
        private static SizeCallback windowSizeCallback, framebufferSizeCallback;
        private static FocusCallback windowFocusCallback;
        private static WindowCallback closeCallback, windowRefreshCallback;
        private static FileDropCallback dropCallback;
        private static WindowMaximizedCallback windowMaximizeCallback;
        private static WindowContentsScaleCallback windowContentScaleCallback;
        
        //input callbacks
        private static CharModsCallback charModsCallback;
        private static MouseEnterCallback cursorEnterCallback;
        private static MouseButtonCallback mouseButtonCallback;
        private static KeyCallback keyCallback;
        private static MouseCallback cursorPositionCallback, scrollCallback;
        #endregion

        public static void Init()
        {
            BindWindowCallbacks();
            BindInputCallbacks();
        }

        private static void BindWindowCallbacks()
        {
            //taken from https://github.com/ForeverZer0/glfw-net/blob/master/GLFW.NET/NativeWindow.cs
            //forum https://github.com/ForeverZer0/glfw-net/issues/19
            windowPositionCallback = (_, x, y) => Window_OnPositionChanged(x, y);
            windowSizeCallback = (_, w, h) => Window_OnSizeChanged(w, h);
            windowFocusCallback = (_, focusing) => Window_OnFocusChanged(focusing);
            closeCallback = _ => Window_OnClosing();
            dropCallback = (_, count, arrayPtr) => Window_OnFileDrop(count, arrayPtr);
            framebufferSizeCallback = (_, w, h) => Window_OnFramebufferSizeChanged(w, h);
            windowRefreshCallback = _ => Window_Refreshed?.Invoke(DisplayManager.Window, EventArgs.Empty);
            windowMaximizeCallback = (_, maximized) => Window_OnMaximizeChanged(maximized);
            windowContentScaleCallback = (_, x, y) => Window_OnContentScaleChanged(x, y);

            Glfw.SetWindowPositionCallback(DisplayManager.Window, windowPositionCallback);
            Glfw.SetWindowSizeCallback(DisplayManager.Window, windowSizeCallback);
            Glfw.SetWindowFocusCallback(DisplayManager.Window, windowFocusCallback);
            Glfw.SetCloseCallback(DisplayManager.Window, closeCallback);
            Glfw.SetDropCallback(DisplayManager.Window, dropCallback);
            Glfw.SetFramebufferSizeCallback(DisplayManager.Window, framebufferSizeCallback);
            Glfw.SetWindowRefreshCallback(DisplayManager.Window, windowRefreshCallback);
            Glfw.SetWindowMaximizeCallback(DisplayManager.Window, windowMaximizeCallback);
            Glfw.SetWindowContentScaleCallback(DisplayManager.Window, windowContentScaleCallback);
        }
        private static void BindInputCallbacks()
        {
            keyCallback = (_, key, code, state, mods) => Input_OnKey(key, code, state, mods);
            cursorPositionCallback = (_, x, y) => Input_OnMouseMove(x, y);
            cursorEnterCallback = (_, entering) => Input_OnMouseEnter(entering);
            mouseButtonCallback = (_, button, state, mod) => Input_OnMouseButton(button, state, mod);
            scrollCallback = (_, x, y) => Input_OnMouseScroll(x, y);
            charModsCallback = (_, cp, mods) => Input_OnCharacterInput(cp, mods);

            Glfw.SetCursorPositionCallback(DisplayManager.Window, cursorPositionCallback);
            Glfw.SetCursorEnterCallback(DisplayManager.Window, cursorEnterCallback);
            Glfw.SetMouseButtonCallback(DisplayManager.Window, mouseButtonCallback);
            Glfw.SetScrollCallback(DisplayManager.Window, scrollCallback);
            Glfw.SetCharModsCallback(DisplayManager.Window, charModsCallback);
            Glfw.SetKeyCallback(DisplayManager.Window, keyCallback);
        }

        /// <summary> Gathers all Events that happened this frame, i.e. Input- or Window-Events. </summary>
        public static void GatherEvents()
        {
            //currently using Glfw, because we don't any other libraries yet
            if (DisplayManager.windowType == DisplayManager.WindowType.Glfw)
            {
                Glfw.PollEvents();
            }
        }

        #region APPLICATION_EVENTS
        /// <summary> Invokes <see cref="calledInit"/>. </summary>
        static internal void InvokeInitializeEvent()
        {
            initPassed = true;
            calledInit?.Invoke();
        }
        /// <summary> Invokes <see cref="calledStart"/>. </summary>
        static internal void InvokeLoadContentEvent()
        {
            startPassed = true;
            calledStart?.Invoke();
        }
        /// <summary> Invokes <see cref="calledUpdate"/>. </summary>
        static internal void InvokeUpdateEvent()
        {
            calledUpdate?.Invoke();
        }
        /// <summary> Invokes <see cref="calledRender"/>. </summary>
        static internal void InvokeRenderEvent()
        {
            calledRender?.Invoke();
        }
        #endregion

        #region WINDOW_CALLBACKS
        private static void Window_OnContentScaleChanged(float x, float y)
        {
            Window_ContentScaleChanged?.Invoke(x, y);
        }
        private static void Window_OnMaximizeChanged(bool maximized)
        {
            Window_MaximizedChanged?.Invoke(maximized);
        }
        private static void Window_OnFramebufferSizeChanged(int w, int h)
        {
            Window_FramebufferSize?.Invoke(w, h);
        }
        private static void Window_OnFileDrop(int count, IntPtr arrayPtr)
        {
            Window_FileDrop?.Invoke(count, arrayPtr);
        }
        private static void Window_OnClosing()
        {
            Window_Closing?.Invoke();
        }
        private static void Window_OnFocusChanged(bool focusing)
        {
            Window_FocusChanged?.Invoke(focusing);
        }
        private static void Window_OnSizeChanged(int w, int h)
        {
            Window_SizeChanged?.Invoke(w, h);
        }
        private static void Window_OnPositionChanged(double x, double y)
        {
            Window_PositionChanged?.Invoke(x, y);
        }
        #endregion

        #region INPUT_CALLBACKS
        private static void Input_OnKey(GLFW.Keys key, int code, GLFW.InputState state, GLFW.ModifierKeys mods)
        {
            Input_Key?.Invoke((KeyCode)key, (InputState)state);
        }
        private static void Input_OnCharacterInput(uint cp, GLFW.ModifierKeys mods)
        {
            Input_CharInput?.Invoke(cp);
        }
        private static void Input_OnMouseScroll(double x, double y)
        {
            Input_MouseScroll?.Invoke(x, y);
        }
        private static void Input_OnMouseButton(GLFW.MouseButton button, GLFW.InputState state, GLFW.ModifierKeys mod)
        {
            Input_Mouse?.Invoke((MouseButton)button, (InputState)state);
        }
        private static void Input_OnMouseEnter(bool entering)
        {
            Input_MouseEnter?.Invoke(entering);
        }
        private static void Input_OnMouseMove(double x, double y)
        {
            Input_MouseMove?.Invoke(x, y);
        }
        #endregion
    }
}
