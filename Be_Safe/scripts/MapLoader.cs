using BitsCore.DataManagement;
using BitsCore.Debugging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace BeSafe.Scripts
{
    public static class MapLoader
    {
        public static MapData[] LoadMap(string fileName, bool useAssetsMapsDir, out int columns, out int rows)
        {
            bool fileExt = fileName.Contains(".obj"); //checks whether the given name has the file-extension in it, to pass it extra or not
            string path = useAssetsMapsDir ? DataManager.assetsPath + @"\Maps\" + fileName + (fileExt ? "" : ".obj") : fileName;
            if (!File.Exists(path)) { throw new Exception("!!! File '" + path + "' doesn't exist !!!"); }

            List<MapData> mapData = new List<MapData>();

            columns = 0;
            rows    = 0;
            bool readX      = false;
            bool readY      = false;
            bool readAuthor = false; 
            bool readDate   = false;

            // read the entire file, then get the individual tiles segmented by semicolons
            foreach (string text in File.ReadAllText(path).Split(";"))
            {
                #region HEADER
                if (!readX)
                {
                    columns = int.Parse(text, CultureInfo.InvariantCulture.NumberFormat);
                    readX = true;
                    continue;
                }
                else if (!readY)
                {
                    rows = int.Parse(text, CultureInfo.InvariantCulture.NumberFormat);
                    readY = true;
                    continue;
                }
                else if (!readAuthor)
                {
                    // @TODO: author = text;
                    readAuthor = true;
                    continue;
                }
                else if (!readDate)
                {
                    // @TODO: date = text;
                    readDate = true;
                    continue;
                }
                #endregion

                MapData.GroundType groundType;
                MapData.StructureType structureType;
                List<MapData.ObjectType> objectTypes = new List<MapData.ObjectType>();

                string[] words = text.Split(" ");
    
                if(words.Length < 2) { BBug.Log("!!! Too little words in the maps save-files tile-entry.");  }

                groundType = ReadGroundType(words[0]);
                structureType = ReadStructureType(words[1]);

                for(int i = 2; i < words.Length; i++)
                {
                    objectTypes.Add(ReadObjectType(words[i]));
                }

                mapData.Add(new MapData(groundType, structureType, objectTypes.ToArray()));
            }

            return mapData.ToArray();
        }

        static MapData.GroundType ReadGroundType(string word)
        {
            if(word == "grass") { return MapData.GroundType.Grass; } 
            else if(word == "water") { return MapData.GroundType.Water; }

            BBug.Log("!!! MapLoader: GroundType not recognized: '" + word + "'");
            return MapData.GroundType.Grass;
        }
        static MapData.StructureType ReadStructureType(string word)
        {
            if(word == "N") { return MapData.StructureType.None; } 
            else if(word == "wall_corner") { return MapData.StructureType.Wall_Corner; } 
            else if(word == "wall_straight") { return MapData.StructureType.Wall_Straight; } 
            else if(word == "wall_sideways") { return MapData.StructureType.Wall_Sideways; } 

            BBug.Log("!!! MapLoader: StructureType not recognized: '" + word + "'");
            return MapData.StructureType.None;
        }
        static MapData.ObjectType ReadObjectType(string word)
        {
            if(word == "crate") { return MapData.ObjectType.Crate; } 

            BBug.Log("!!! MapLoader: ObjectType not recognized: '" + word + "'");
            return MapData.ObjectType.Crate;
        }

    }
}
