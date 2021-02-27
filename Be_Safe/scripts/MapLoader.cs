using BitsCore.DataManagement;
using BitsCore.Debugging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace BeSafe.Scripts
{
    public static class MapLoader
    {

        // @HINT: newline-chars could be a problem
        public static TileData[] LoadMap(string fileName, bool useAssetsMapsDir, out int columns, out int rows, out int playerStart)
        {
            bool fileExt = fileName.Contains(".map"); //checks whether the given name has the file-extension in it, to pass it extra or not
            string path = useAssetsMapsDir ? DataManager.assetsPath + @"\Maps\" + fileName + (fileExt ? "" : ".map") : fileName;
            if (!File.Exists(path)) { throw new Exception("!!! File '" + path + "' doesn't exist !!!"); }

            List<TileData> mapData = new List<TileData>();

            columns     = 0;
            rows        = 0;
            playerStart = 0;
            
            bool readX           = false;
            bool readY           = false;
            bool readPlayerStart = false;
            bool readName        = false;
            bool readAuthor      = false; 
            bool readDate        = false;

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
                else if (!readPlayerStart)
                {
                    playerStart = int.Parse(text, CultureInfo.InvariantCulture.NumberFormat);
                    readPlayerStart = true;
                    continue;
                }
                else if (!readName)
                {
                    // @TODO: name = text;
                    readName = true;
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

                TileData.GroundType groundType;
                TileData.StructureType structureType;
                List<TileData.ObjectType> objectTypes = new List<TileData.ObjectType>();

                // remove empty strings
                List<string> wordsSplit = text.Split(" ").ToList();
                List<string> words = wordsSplit.SkipWhile(ele => string.IsNullOrEmpty(ele)).ToList();

                // BBug.Log("\n|--No Empty -------------------------|");
                // foreach (string str in words)
                // {
                //     BBug.Log("  -> '" + str + "'");
                // }

                // remove white-space chars
                for (int i = 0; i < words.Count; i++)
                {
                    // taken from https://stackoverflow.com/questions/46364237/how-to-remove-all-whitespace-characters-from-a-string
                    words[i] = string.Concat(words[i].Where(c => !char.IsWhiteSpace(c)));
                }

                // BBug.Log("\n|--No WhiteSpace --------------------|");
                // foreach (string str in words)
                // {
                //     BBug.Log("  -> '" + str + "'");
                // }
                // BBug.Log("--------------------------------------------------------");


                if (words.Count < 2) { BBug.Log("!!! Too little words in the maps save-files tile-entry."); continue; /*@CLEANUP: continue to skip the last entry as that always is an empty entry because of the foreach loop*/ }

                groundType    = ReadGroundType(words[0]);
                structureType = ReadStructureType(words[1]);

                if(words.Count > 2)
                {
                    for(int i = 2; i < words.Count; i++)
                    {
                        objectTypes.Add(ReadObjectType(words[i]));
                    }
                }

                mapData.Add(new TileData(groundType, structureType, objectTypes.ToArray()));
            }

            return mapData.ToArray();
        }

        static TileData.GroundType ReadGroundType(string word)
        {
            if(word == "grass") { return TileData.GroundType.Grass; } 
            else if(word == "water") { return TileData.GroundType.Water; }

            BBug.Log("!!! MapLoader: GroundType not recognized: '" + word + "'");
            return TileData.GroundType.Grass;
        }
        static TileData.StructureType ReadStructureType(string word)
        {
            if(word == "N") { return TileData.StructureType.None; } 
            else if(word == "wall_corner") { return TileData.StructureType.Wall_Corner; } 
            else if(word == "wall_straight") { return TileData.StructureType.Wall_Straight; } 
            else if(word == "wall_side") { return TileData.StructureType.Wall_Sideways; }
            else if(word == "pressure_plate") { return TileData.StructureType.PressurePlate; }

            BBug.Log("!!! MapLoader: StructureType not recognized: '" + word + "'");
            return TileData.StructureType.None;
        }
        static TileData.ObjectType ReadObjectType(string word)
        {
            if(word == "crate") { return TileData.ObjectType.Crate; } 
            else if(word == "plant") { return TileData.ObjectType.Plant; } 

            BBug.Log("!!! MapLoader: ObjectType not recognized: '" + word + "'");
            return TileData.ObjectType.Crate;
        }

    }
}
