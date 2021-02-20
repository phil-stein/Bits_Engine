using BitsCore;
using System;

namespace BeSafe
{
    /// <summary> The Main() Starts the Game, also sets the Games Window-Title. </summary>
    class Program
    {
        public static Application app;
        public static void Main(string[] args)
        {
            app = new BeSafeApp(1920, 1080, "BitsCore - Application", true); // 1280, 720 , ... , false
            app.Run();
        }
    }
}
