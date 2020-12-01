using BitsCore.ObjectData.Components;
using BitsCore.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Text;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BitsCore.DataManagement
{
    /// <summary> Handles the importing of model-files. </summary>
    public static class ModelImporter
    {
        /// <summary>
        /// Import an .obj file as a Mesh-Component.
        /// <para> The 'useAssetsModelsDir' bool specifies whether the file-path chosen should be the 'assets\Models\fileName' or just the fileName. </para>
        /// <para> Use 'ImportSetting.png' in 'assets\BlendFiles' to see what blender export setings need to be set. </para>
        /// <para> Center the object(alt+g) and apply all transforms(ctrl+a) before export for optimal result. </para>
        /// </summary>
        /// <param name="mat"> Material of the Mesh. </param>
        /// <param name="fileName"> Name of the .obj file. </param>
        /// <param name="useAssetsModelsDir"> Specifies whether the file-path chosen should be the 'assets\Models\fileName' or just the fileName. </param>
        /// <returns></returns>
        public static Mesh Import(string fileName, bool useAssetsModelsDir = true)
        {
            bool fileExt = fileName.Contains(".obj"); //checks whether the given name has the file-extension in it, to pass it extra or not
            string path = useAssetsModelsDir ? DataManager.assetsPath + @"\Models\" + fileName + (fileExt ? "" : ".obj") : fileName;
            if (!File.Exists(path)) { throw new Exception("!!! File '" + path + "' doesn't exist !!!"); }
            
            //arrays for the Imported-Data
            List<Vector3> pos = new List<Vector3>();
            List<Vector3> normals = new List<Vector3>();
            List<Vector2> uvs = new List<Vector2>();
            List<int> ids = new List<int>();

            int posNum = 0;
            int triNum = 0;
            int uvNum = 0;
            foreach (string line in File.ReadLines(path))
            {
                string[] entries = line.Split(" ");
                if(entries[0] == "v")
                {
                    //vertex-data

                    //Debug.WriteLine("Vertex: ");
                    //Debug.WriteLine(entries[0] + " " + entries[1] + " " + entries[2] + " " + entries[3]);
                    //culture-info to read the '.'-dot as the divider between integral and decimal numbers (as some local threads are set to use a  ','-comma)

                    //Debug.WriteLine(x.ToString() + " " + y.ToString() + " " + z.ToString() + "\n");
                    pos.Add(new Vector3(
                        float.Parse(entries[1], CultureInfo.InvariantCulture.NumberFormat),
                        float.Parse(entries[2], CultureInfo.InvariantCulture.NumberFormat),
                        float.Parse(entries[3], CultureInfo.InvariantCulture.NumberFormat)
                        ));

                    posNum++;
                }
                else if(entries[0] == "vt")
                {
                    //uv-data
                    float u = float.Parse(entries[1], CultureInfo.InvariantCulture.NumberFormat);
                    float v = 1f - float.Parse(entries[2], CultureInfo.InvariantCulture.NumberFormat); //1f - v, because the v coordinate is flipped otherwise
                    uvs.Add(new Vector2(u, v));

                    uvNum++;
                }
                else if (entries[0] == "vn")
                {
                    //uv-data
                    normals.Add(new Vector3(
                        float.Parse(entries[1], CultureInfo.InvariantCulture.NumberFormat),
                        float.Parse(entries[2], CultureInfo.InvariantCulture.NumberFormat),
                        float.Parse(entries[3], CultureInfo.InvariantCulture.NumberFormat)
                        ));

                }
                else if(entries[0] == "f")
                {
                    //Debug.WriteLine("Face: ");
                    //Debug.WriteLine(entries[0] + " " + entries[1] + " " + entries[2] + " " + entries[3]);
                    for(int n = 0; n < 3; n++)
                    {
                        //gets the num TRI-ID out of the data-format TRI-ID/UV/NORM
                        string[] strs = entries[n + 1].Split("/");
                        int vertID = int.Parse(strs[0], CultureInfo.InvariantCulture.NumberFormat) -1; //-1, because .obj triangle indexes start at 1 not 0

                        int uvID = int.Parse(strs[1], CultureInfo.InvariantCulture.NumberFormat) -1; //-1, because .obj triangle indexes start at 1 not 0

                        int normID = int.Parse(strs[2], CultureInfo.InvariantCulture.NumberFormat) -1; //-1, because .obj triangle indexes start at 1 not 0

                        ids.AddRange(new int[] { vertID, uvID, normID });

                        //Debug.Write(tris[triNum].ToString() + " ");

                        triNum++;
                    }
                    //Debug.Write("\n");
                }
            }

            float[] verts = new float[(ids.Count/3) *8];

            int vertCount = 0;
            for(int i = 0; i < ids.Count; i += 3)
            {
                //pos
                verts[vertCount * 8 + 0] = pos[ids[i +0]].X;
                verts[vertCount * 8 + 1] = pos[ids[i +0]].Y;
                verts[vertCount * 8 + 2] = pos[ids[i +0]].Z;

                //normal
                verts[vertCount * 8 + 3] = normals[ids[i +2]].X;
                verts[vertCount * 8 + 4] = normals[ids[i +2]].Y;
                verts[vertCount * 8 + 5] = normals[ids[i +2]].Z;

                //uv
                verts[vertCount * 8 + 6] = uvs[ids[i +1]].X;
                verts[vertCount * 8 + 7] = uvs[ids[i +1]].Y;

                vertCount++;
            }

            Mesh m = new Mesh(verts, new uint[0], true);
            //m.CalcNormalsSmooth();
            return m;
        }

        /// <summary>
        /// Import an .obj file as a Mesh-Component.
        /// <para> The 'useAssetsModelsDir' bool specifies whether the file-path chosen should be the 'assets\Models\fileName' or just the fileName. </para>
        /// <para> Use 'ImportSetting.png' in 'assets\BlendFiles' to see what blender export setings need to be set. </para>
        /// <para> Center the object(alt+g) and apply all transforms(ctrl+a) before export for optimal result. </para>
        /// </summary>
        /// <param name="mat"> Material of the Mesh. </param>
        /// <param name="fileName"> Name of the .obj file. </param>
        /// <param name="useAssetsModelsDir"> Specifies whether the file-path chosen should be the 'assets\Models\fileName' or just the fileName. </param>
        /// <returns></returns>
        public static async Task<Mesh> ImportAsync(string fileName, bool useAssetsModelsDir = true)
        {
            bool fileExt = fileName.Contains(".obj"); //checks whether the given name has the file-extension in it, to pass it extra or not
            string path = useAssetsModelsDir ? DataManager.assetsPath + @"\Models\" + fileName + (fileExt ? "" : ".obj") : fileName;
            if (!File.Exists(path)) { System.Diagnostics.Debug.WriteLine("!!! File doesn't exist !!!"); return null; }

            //Debug.WriteLine(fileName + "------------------------------");

            //arrays for the Imported-Data
            List<Vector3> pos = new List<Vector3>();
            Vector3[] normals;
            Vector2[] uvs;

            List<uint> tris = new List<uint>();

            int posNum = 0;
            int triNum = 0;
            foreach (string line in File.ReadLines(path))
            {
                string[] entries = line.Split(" ");
                if (entries[0] == "v")
                {
                    //Debug.WriteLine("Vertex: ");
                    //Debug.WriteLine(entries[0] + " " + entries[1] + " " + entries[2] + " " + entries[3]);
                    //culture-info to read the '.'-dot as the divider between integral and decimal numbers (as some local threads are set to use a  ','-comma)
                    float x = VarUtils.StringToFloat(entries[1]);
                    float y = VarUtils.StringToFloat(entries[2]);
                    float z = VarUtils.StringToFloat(entries[3]);
                    //Debug.WriteLine(x.ToString() + " " + y.ToString() + " " + z.ToString() + "\n");
                    pos.Add(new Vector3(x, y, z));



                    posNum++;
                }
                else if (entries[0] == "f")
                {
                    //Debug.WriteLine("Face: ");
                    //Debug.WriteLine(entries[0] + " " + entries[1] + " " + entries[2] + " " + entries[3]);
                    for (int n = 0; n < 3; n++)
                    {
                        //gets the num TRI-ID out of the data-format TRI-ID/UV/NORM
                        tris.Add(uint.Parse(entries[n + 1].Split("/")[0], CultureInfo.InvariantCulture.NumberFormat) - 1); //-1, because .obj triangle indexes start at 1 not 0
                        //Debug.Write(tris[triNum].ToString() + " ");
                        triNum++;
                    }
                    //Debug.Write("\n");
                }
            }

            //arrays for the GameObject
            float[] verts = new float[pos.Count * 8];

            //fill the vert array
            for (int i = 0; i < pos.Count; i++)
            {
                verts[i * 8 + 0] = pos[i].X;
                verts[i * 8 + 1] = pos[i].Y;
                verts[i * 8 + 2] = pos[i].Z;
            }

            Mesh m = new Mesh(verts, tris.ToArray());
            await Task.Run(new Action(m.CalcNormalsSmooth));
            return m;
        }

    }
}
