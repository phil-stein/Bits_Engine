using static BitsCore.OpenGL.GL;
using BitsCore.DataManagement;
using BitsCore.Rendering;
using BitsCore.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Collections;

namespace BitsCore.Debugging
{
    public static class BBug
    {

        //methods flagged with [Conditional("DEBUG")] only get called in debug-mode
        //the additional compiler-preprocessor if-statements (#if Debug ... #endif),
        //are there to not even compile the contents of the functions for non-debug builds, as they never get called

        private class TimerVars
        {
            public Stopwatch timerStopwatch;
            public string timerName;
            public bool timerSecs;

            public TimerVars(Stopwatch stopwatch, string name, bool timerSecs)
            {
                this.timerStopwatch = stopwatch;
                this.timerName = name;
                this.timerSecs = timerSecs;
            }
        }
        private static List<TimerVars> timerVars = new List<TimerVars>();
        private static bool timerAct = true;
        private static float timerTrigger = 0.0f;
        private static bool printLog = false;
        private static List<string> logList = new List<string>();
        private static string printLogHeader = "";

        /// <summary> 
        /// Sets all <see cref="StopTimer"/> method to show '!!!' infront of the result if it was greater than or qual to the given triggerMS value. 
        /// <para> Given value is in milliseconds. </para>
        /// </summary>
        /// <param name="triggerMS"> Min amount of milliseconds to have been stoppped before triggering.</param>
        public static void SetTimerTrigger(float triggerMS)
        {
            //Log("  |Before: timerTrigger: " + timerTrigger + ", triggerMS: " + triggerMS);
            timerTrigger = triggerMS;
            //Log("  |After: timerTrigger: " + timerTrigger + ", triggerMS: " + triggerMS);
        }

        /// <summary> 
        /// <see cref="SetTimersAct(bool)"/>[false] suppresses all following calls to <see cref="StartTimer(string, bool)"/> and <see cref="StopTimer"/> 
        /// <para> until <see cref="SetTimersAct(bool)"/>[true]. </para>
        /// </summary>
        public static void SetTimersAct(bool act)
        {
            timerAct = act;
        }

        /// <summary> 
        /// Stops the <see cref="StopTimer"/> method from displaying warnings. 
        /// <para> Only has an effect if called after also having called <see cref="SetTimerTrigger(float)"/>. </para>
        /// </summary>
        public static void ResetTimerTrigger()
        {
            timerTrigger = 0.0f;
        }

        /// <summary> 
        /// Measures time until <see cref="StopTimer"/> is called. 
        /// <para> Output format: ['time'] - Elapsed during 'name' </para>
        /// </summary>
        /// <param name="name"> Name for the area between <see cref="StartTimer(string, bool)"/> and <see cref="StopTimer"/> to be measured. </param>
        /// <param name="timerInSecs"> If set to true time is displayed in seconds instead of miliseconds. </param>
        public static void StartTimer(string name, bool timerInSecs = false)
        {
            timerVars.Add(new TimerVars(
                    new Stopwatch(),
                    name,
                    timerInSecs
                ));

            timerVars.Last().timerStopwatch.Start();
        }

        /// <summary> 
        /// Stops timer started with <see cref="StopTimer"/> and outputs the elapsed-time in to the console. 
        /// <para> Output format: ['time'] - Elapsed during 'name' </para>
        /// </summary>
        public static void StopTimer()
        {
            if(timerVars.Last().timerStopwatch == null || timerVars.Count < 1) { Log("StartTimer() needs to be called before TimerStop()."); throw new Exception("No timers started. StartTimer() needs to be called before TimerStop()."); }
            timerVars.Last().timerStopwatch.Stop();

            string[] elapsedTotal = timerVars.Last().timerStopwatch.Elapsed.ToString().Split(":"); //gets the last elem after a :, because its formatted 00:00:00.000
            string elapsed = elapsedTotal[0] == "00" ? "" : elapsedTotal[0];
            elapsed += elapsedTotal[1] == "00" ? "" : elapsedTotal[1];
            elapsed += elapsedTotal[2];
            if (timerVars.Last().timerSecs) { Log("[" + elapsed + " sec] - Elapsed during: " + timerVars.Last().timerName); }
            else 
            {
                //gets the seconds elapsed and converts them to ms, because Stopwatch.ElapsedMiliseconds is rounded
                float ms = VarUtils.StringToFloat(elapsed) * 1000f;
                string triggerWarn = timerTrigger > 0.0f ? (ms >= timerTrigger ? "!!! " : "") : "";
                //Log("TriggerWarn: " + triggerWarn + ", ms: " + ms + ", TimerTrigger: " + timerTrigger);
                Log(triggerWarn + "[" + ms + " ms] - Elapsed during: " + timerVars.Last().timerName); 
            }

            timerVars.RemoveAt(timerVars.Count -1);
        }

        /// <summary> Writes a message to <see cref="Debug.WriteLine(string?)"/>, <see cref="Console.WriteLine"/> and the in-editor console in the future. </summary>
        /// <param name="message"> The message to be logged. </param>
        public static void Log(string message = "")
        {
            message = message == null ? "null-value" : message;
            #if DEBUG
            Debug.WriteLine(message);
            Console.WriteLine(message);
            #endif

            //in-editor console ...
            //hack:
            if (Renderer.activeApp.mainLayerUI.texts.ContainsKey("DEBUG_ONE"))
            {
                Renderer.activeApp.mainLayerUI.SetText("DEBUG_ONE", "Debug: " + message);
            }

            if (printLog)
            {
                logList.Add(message);
            }
        }
        /// <summary> Writes a message to <see cref="Debug.WriteLine(string?)"/>, <see cref="Console.WriteLine"/> and the in-editor console in the future. </summary>
        /// <param name="message"> The message to be logged. </param>
        public static void Log(object param) 
        {
            Log(param.ToString());
        }

        /// <summary> 
        /// Determines whether the Logged mesages get saved to a .txt file using <see cref="PrintLogToFile"/>. 
        /// <para> Text-File gets saved to <see cref="DataManager.rootPath"/>, the same folder as the BitsCore-dll is in. </para>
        /// </summary>
        /// <param name="act"> The new value of <see cref="printLog"/> </param>
        public static void SetPrintLog(bool act)
        {
            printLog = act;

            //should add cpu info
            printLogHeader = 
                "------------------------------------------------------------------------\n" +
                "| Text-Version of the BBug.Log() messages. \n" +
                "|\n" +
                "| Versions: \n" + 
                "|   Active BitsCore version " + BitsCoreContext.BitsCoreVersion + " \n" +
                "|   Active OpenGL version " + BitsCoreContext.openGLVersion + " \n" +
                "|   Active GLSL version " + BitsCoreContext.glslVersion + " \n" +
                "|\n" +
                "| Hardware: \n" + 
                "|   Active GPU Vendor " + BitsCoreContext.gpuVendor + ". \n" + //i.e. Nvidia / AMD
                "|   Active GPU " + BitsCoreContext.gpuName + ". \n" + //i.e. GTX 1050 / etc.
                "|   Active CPU " + BitsCoreContext.cpuName + ". \n" + //i.e. Intel i3 / etc.
                "|   Total Ram " + BitsCoreContext.totalRamKB.ToString("00,000") + " Gigabytes. \n" + //i.e. 8 Gigabytes / 16 Gigabytes
                "|\n" +
                "| Time & Date: \n" + 
                "|   BitsCore Aplication started at " + DateTime.Now + ". \n"; //current date + time of day //add .ToString("h:mm:ss tt") to only have 
        }

        /// <summary> 
        /// Saves the messages given to <see cref="Log(string)"/> to a .txt file in the folder with the BitsCore-dll. 
        /// <para> Only does so if <see cref=""/> was called first. </para>
        /// </summary>
        public static void PrintLogToFile()
        {
            if (!printLog) { return; }

            printLogHeader +=
                "|   BitsCore Application ran for " + GameTime.TotalElapsedSeconds + " seconds. \n" +
                "|   BitsCore Aplication ended at " + DateTime.Now + ". \n" + //current date + time of day //add .ToString("h:mm:ss tt") to only have time
                "------------------------------------------------------------------------\n";

            logList.Insert(0, printLogHeader);
            // Create a file to write to or write over an existing file
            File.WriteAllLines(DataManager.rootPath + @"\Log.txt", logList, Encoding.UTF8);
        }

    }
}
