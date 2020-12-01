using System;
using System.Diagnostics;
using System.IO;

namespace BitsCore.DataManagement
{
    /// <summary> Handles data and hold references to specific file-paths. </summary>
    public static class DataManager
    {
        /// <summary> 
        /// Path to the currect project-folder. 
        /// <para> Based on the build folder. </para>
        /// <para> Only works as long as the .exe is in the build folder. </para>
        /// </summary>
        public static string projectFilePath { get; private set; }

        /// <summary> 
        /// Path to the currect project-folders asset-folder. 
        /// <para> Based on the build folder. </para>
        /// <para> Only works as long as the .exe is in the build folder. </para>
        /// </summary>
        public static string projectAssetsFilePath { get; private set; }

        /// <summary> The file-path to the assets folder. </summary>
        public static string assetsPath { get; private set; }

        /// <summary> 
        /// The file-path to the executables root folder. 
        /// <para> After building this is usually '\bin\Release\netcoreapp[version-num]\' or '\bin\Debug\netcoreapp[version-num]\'. </para>
        /// </summary>
        public static string rootPath { get; private set; }

        /// <summary> Clears any remaining data from previous builds and copies the projects assets folder into the build-directory. </summary>
        public static void Init()
        {
            rootPath = AppDomain.CurrentDomain.BaseDirectory;
            assetsPath = AppDomain.CurrentDomain.BaseDirectory + @"assets";

            //project path is baiscally ..\..\..\..\build-folder, i.e. Project\bin\Debug\netcoreapp\
            int dirsToWalkBack = 4;
            for(int i = rootPath.Length -1; i > 0; i--)
            {
                if (rootPath[i] == '\\') // '\' needs to be escaped => '\\'
                {
                    projectFilePath = rootPath.Remove(i);
                    dirsToWalkBack--;
                    if(dirsToWalkBack <= 0) { projectFilePath += @"\"; break; }
                }
            }
            projectAssetsFilePath = projectFilePath + @"assets";

            //can't use BBug here because it gets initialized after the DataManager
            Debug.WriteLine("RootPath: " + rootPath);
            Console.WriteLine("RootPath: " + rootPath);
            
            Debug.WriteLine("FilePath: " + assetsPath);
            Console.WriteLine("FilePath: " + assetsPath);
            
            Debug.WriteLine("ProjectPath: " + projectFilePath);
            Console.WriteLine("ProjectPath: " + projectFilePath);

            Debug.WriteLine("ProjectPath: " + projectAssetsFilePath);
            Console.WriteLine("ProjectPath: " + projectAssetsFilePath);

            Debug.WriteLine("DataManager Done.");
            Console.WriteLine("DataManager Done.");
        }

        /// <summary> Copy a file from one location to another. </summary>
        /// <param name="sourcePath"> The file-path of the file to be copied. </param>
        /// <param name="destPath"> The destination path for the copy action. </param>
        /// <param name="overrideDestFile"> Override if file already exists. </param>
        public static void CopyFile(string sourcePath, string destPath, bool overrideDestFile = false)
        {
            // Get the files in the directory and copy them to the new location.
            FileInfo file = new FileInfo(sourcePath);
            file.CopyTo(destPath, overrideDestFile);
        }

        /// <summary> Copies one directory to another location. </summary>
        /// <param name="sourceDirName"> Location of the directory to be copied. </param>
        /// <param name="destDirName"> Location of the directory to be copied to. </param>
        /// <param name="copySubDirs"> Exclude/include the sub-directories. </param>
        private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs = true)
        {
            //taken from: https://docs.microsoft.com/en-us/dotnet/standard/io/how-to-copy-directories

            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();

            // If the destination directory doesn't exist, create it.       
            Directory.CreateDirectory(destDirName);

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string tempPath = Path.Combine(destDirName, file.Name);
                file.CopyTo(tempPath, false);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string tempPath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, tempPath, copySubDirs);
                }
            }
        }

        /// <summary> Clears all the files and folders under the given location. </summary>
        /// <param name="dirPath"> Location of the directory to be cleared. </param>
        /// <param name="excludeFile"> Exclude a file, leave empty to not exclude any files. </param>
        private static void DirectoryClear(string dirPath, string excludeFile = "")
        {
            System.IO.DirectoryInfo di = new DirectoryInfo(dirPath);
            if(!di.Exists) { return; }

            foreach (FileInfo file in di.GetFiles())
            {
                if (excludeFile != "" && excludeFile == file.Name) { continue; }
                Debug.WriteLine("Deteted File during DirectoryClear(): " + file.Name);
                file.Delete();
            }
            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                dir.Delete(true);
            }
        }
    }
}

