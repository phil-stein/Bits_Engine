using BitsCore.Debugging;
using System.Runtime.InteropServices;
using static BitsCore.OpenGL.GL;

namespace BitsCore
{
    public static class BitsCoreContext
    {
        public static string BitsCoreVersion { get; private set; }
        public static string openGLVersion { get; private set; }
        public static string glslVersion { get; private set; }
        public static string gpuVendor { get; private set; }
        public static string gpuName { get; private set; }
        public static string cpuName { get; private set; }
        public static ulong totalRamGB { get; private set; }
        public static ulong totalRamKB { get; private set; }

        /// <summary> Sets the variables such as <see cref="gpuName"/> and <see cref="openGLVersion"/>. </summary>
        public static void Init()
        {
            BitsCoreVersion = "0.1";

            int major = 0;
            int minor = 0;
            unsafe
            {
                glGetIntegerv(GL_MAJOR_VERSION, &major);
                glGetIntegerv(GL_MINOR_VERSION, &minor);
            }
            openGLVersion = major + "." + minor;
            glslVersion = glGetString(GL_SHADING_LANGUAGE_VERSION​);

            gpuVendor = glGetString​(GL_VENDOR);
            gpuName = glGetString​(GL_RENDERER);

            ulong installedMemory;
            MEMORYSTATUSEX memStatus = new MEMORYSTATUSEX();
            if (GlobalMemoryStatusEx(memStatus))
            {
                installedMemory = memStatus.ullTotalPhys;
                totalRamKB = (installedMemory / 1024) / 1024;
                totalRamGB = totalRamKB / 1024;
            }
            else { throw new System.Exception("Get total size of Memory failed. Uses 'kernel32.dll'.'"); }

            //not very useful use https://www.codeproject.com/Articles/26310/Using-WMI-to-retrieve-processor-information-in-C instead
            cpuName = System.Environment.GetEnvironmentVariable("PROCESSOR_IDENTIFIER");
        }

        //taken from stackoverflow https://stackoverflow.com/questions/105031/how-do-you-get-total-amount-of-ram-the-computer-has
        //wrapper for c++ function to retrieve total ram of the system using https://docs.microsoft.com/en-us/windows/win32/api/sysinfoapi/nf-sysinfoapi-globalmemorystatusex
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private class MEMORYSTATUSEX
        {
            public uint dwLength;
            public uint dwMemoryLoad;
            public ulong ullTotalPhys;
            public ulong ullAvailPhys;
            public ulong ullTotalPageFile;
            public ulong ullAvailPageFile;
            public ulong ullTotalVirtual;
            public ulong ullAvailVirtual;
            public ulong ullAvailExtendedVirtual;
            public MEMORYSTATUSEX()
            {
                this.dwLength = (uint)Marshal.SizeOf(this);
            }
        }


        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool GlobalMemoryStatusEx([In, Out] MEMORYSTATUSEX lpBuffer);
    }
}
