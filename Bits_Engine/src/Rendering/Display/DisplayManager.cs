using GLFW;
using BitsCore.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Text;
using System.Xml.Serialization;
using static BitsCore.OpenGL.GL;
using System.Diagnostics;
using BitsCore.Events_Input;

namespace BitsCore.Rendering.Display
{
    public static class DisplayManager
    {
        /// <summary> Lists the possible libraries using which the DisplayManager can create a window. </summary>
        public enum WindowType { Glfw };

        /// <summary> The library that was used to create the current window. </summary>
        public static WindowType windowType { get; private set; }

        /// <summary> The window that gets rendered to. </summary>
        public static Window Window { get; private set; }

        /// <summary> The size of the current window, x = width, y = height. </summary>
        public static Vector2 WindowSize { get; private set; }

        #region WINDOW_ATTRIBS
        //currently only works for glfw
        public static bool isMouseOver => Glfw.GetWindowAttribute(Window, WindowAttribute.MouseHover);
        public static bool isFloating
        {
            get => Glfw.GetWindowAttribute(Window, WindowAttribute.Floating);
            set => Glfw.SetWindowAttribute(Window, WindowAttribute.Floating, value);
        }
        public static bool isFocused
        {
            get => Glfw.GetWindowAttribute(Window, WindowAttribute.Focused);
            set => Glfw.SetWindowAttribute(Window, WindowAttribute.Focused, value);
        }
        public static bool isMaximized
        {
            get => Glfw.GetWindowAttribute(Window, WindowAttribute.Maximized);
            set
            {
                if (value) { Glfw.MaximizeWindow(Window); }
                else { throw new System.Exception("Cannot set Window 'Minimized' using isMaximized = false."); }
            }
        }
        public static bool isResizeable
        {
            get => Glfw.GetWindowAttribute(Window, WindowAttribute.Resizable);
            set => Glfw.SetWindowAttribute(Window, WindowAttribute.Resizable, value);
        }
        public static bool isVisible
        {
            get => Glfw.GetWindowAttribute(Window, WindowAttribute.Visible);
            set
            {
                if (value) { Glfw.ShowWindow(Window); }
                else { Glfw.HideWindow(Window); }
            }
        }
        #endregion

        /// <summary> 
        /// The title of the current window. 
        /// <para>Use SetWindowTitle() to set a new title. </para>
        /// </summary>
        public static string WindowTitle { get; private set; }

        /// <summary> Creates a new window and initializes Glfw. </summary>
        /// <param name="width"> The height of the window. </param>
        /// <param name="height"> The width of the window. </param>
        /// <param name="title"> The title of the window. </param>
        public static void CreateWindow(int width, int height, string title, bool maximize = false)
        {
            //currently using Glfw, because we don't any other libraries yet
            if (windowType == WindowType.Glfw)
            {
                if (Window != Window.None) { System.Diagnostics.Debug.WriteLine("!!! Can't create a new window, one already exists !!!"); return; }
                //Debug.WriteLine("Variable: " + Window.ToString() + ", Type: " + typeof(Window).ToString());

                windowType = WindowType.Glfw; //don't support other libraries yet

                WindowTitle = title;
                WindowSize = new Vector2(width, height);

                Glfw.Init();

                //opengl 3.3 core profile
                Glfw.WindowHint(Hint.ContextVersionMajor, 3);
                Glfw.WindowHint(Hint.ContextVersionMinor, 3);
                Glfw.WindowHint(Hint.OpenglProfile, Profile.Core);

                //window is focused and not resizable
                Glfw.WindowHint(Hint.Focused, true);
                Glfw.WindowHint(Hint.Resizable, true);

                //create the window, 'Monitor.None': windowed-mode, 'Window.None': not sharing opengl-context with other windows
                Window = Glfw.CreateWindow(width, height, title, Monitor.None, Window.None);

                if (Window == Window.None)
                {
                    //window wasn't created
                    return;
                }

                //sets window position to the middle of the screen
                Rectangle screen = Glfw.PrimaryMonitor.WorkArea; //rectangle represents the current entire screen
                int x = (screen.Width - width) / 2;
                int y = (screen.Height - height) / 2;
                Glfw.SetWindowPosition(Window, x, y);

                Glfw.MakeContextCurrent(Window);
                Import(Glfw.GetProcAddress);

                //maximizes the window
                if (maximize)
                {
                    isMaximized = maximize;
                    Glfw.GetWindowSize(Window, out int _width, out int _height);
                    width = _width;
                    height = _height;
                }


                glViewport(0, 0, width, height);
                Glfw.SwapInterval(1); //0: VSync off, 1: VSync on


                //subsrice to the event
                EventManager.Window_SizeChanged += OnResize;

                return;
            }

            System.Diagnostics.Debug.WriteLine("\n!!! Window-Library not supported !!!\n");
        }

        public static void OnResize(int width, int height)
        {
            WindowSize = new Vector2(width, height);
            glViewport(0, 0, width, height);
        }

        /// <summary> Change the windows title. </summary>
        /// <param name="newTitle"> The string the windows title is going to be set to. </param>
        public static void SetWindowTitle(string newTitle)
        {
            //currently using Glfw, because we don't any other libraries yet
            if (windowType == WindowType.Glfw)
            {
                Glfw.SetWindowTitle(Window, newTitle);
                WindowTitle = newTitle;
            }
        }


        public static void SetCursor(CursorType type)
        {
            //currently using Glfw, because we don't any other libraries yet
            if (windowType == WindowType.Glfw)
            {
                //can convert directly, because our CursorType is copied from GLFW.CursorType
                Cursor cursor = Glfw.CreateStandardCursor((GLFW.CursorType)type);
                Glfw.SetCursor(Window, cursor);
            }
        }

        public static void SetCursorPos(double xPos, double yPos)
        {
            //currently using Glfw, because we don't any other libraries yet
            if (windowType == WindowType.Glfw)
            {
                Glfw.SetCursorPosition(Window, xPos, yPos);
            }
        }

        public static void GetCursorPos(out double xPos, out double yPos)
        {
            //currently using Glfw, because we don't any other libraries yet
            if (windowType == WindowType.Glfw)
            {
                Glfw.GetCursorPosition(DisplayManager.Window, out double xpos, out double ypos);
                xPos = xpos; yPos = ypos;
                return;
            }
            xPos = 0; yPos = 0;
        }

        public static void SetCursorVisible(bool visible)
        {
            if(windowType == WindowType.Glfw)
            {
                Glfw.SetInputMode(Window, InputMode.Cursor, visible == true ? (int)CursorMode.Normal : (int)CursorMode.Hidden);
            }
        }

        /// <summary> Terminates the current window. </summary>
        public static void TerminateWindow()
        {
            //currently using Glfw, because we don't any other libraries yet
            if (windowType == WindowType.Glfw)
            {
                Glfw.Terminate();
            }
        }

        /// <summary> Closes the current window. </summary>
        public static void CloseWindow()
        {
            //currently using Glfw, because we don't any other libraries yet
            if (windowType == WindowType.Glfw)
            {
                Glfw.SetWindowShouldClose(Window, true);
            }
        }

        /// <summary> Checks whether the current window is set for closing. </summary>
        public static bool ShouldClose()
        {
            //currently using Glfw, because we don't any other libraries yet
            if (windowType == WindowType.Glfw)
            {
                return Glfw.WindowShouldClose(Window);
            }

            return true;
        }

        /// <summary> Gets the time elapsed since the Window was created. </summary>
        public static double GetTime()
        {
            //currently using Glfw, because we don't any other libraries yet
            if (windowType == WindowType.Glfw) { return Glfw.Time; }

            return 0;
        }
    }
}
