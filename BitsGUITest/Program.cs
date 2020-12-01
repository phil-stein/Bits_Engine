using BitsCore;
using System;

namespace BitsGUITest
{
    class Program
    {
        public static Application app;
        public static void Main(string[] args)
        {
            app = new BitsGUITestEnvironment(1280, 720, "BitsCore-Application", false);
            app.Run();

        }
    }
}
