using BitsCore;
using BitsCore.Debugging;
using BitsCore.ObjectData;
using BitsCore.Utils;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace BeSafe.Scripts
{
    public static class EnvController
    {

        public static int tileRows       = 20;
        public static int tileColumns    = 15;
        public static int tileDist       = 4;
        public static int positionIndex  = 0;

        public const int playerStartPos = 0;

        // 'G': Grass; 'W': Water; '|': Wall-Straight; '-': Wall-Sideways; 'C': Corner;
        public const string mapString =
            "GGGGGGGGGGGGGGG" +
            "GGGGGGGGGGGGGGG" +
            "GGGGGGGGGGGWWGG" +
            "GGGGGGGGGGWWWGG" +
            "GGGGGGGGGGWWWGG" +
            "GGGGGGGGGGGGGGG" +
            "GGGGGGGGGGGGGGG" +
            "GGC-G---CGGGGGG" +
            "GG|GGGGG|GGGGGG" +
            "GG|GGGGG|GGGGGG" +
            "GG|GGGGG|GGGGGG" +
            "GG|GGGGG|GGGGGG" +
            "GG|GGGGG|GGGGGG" +
            "GGC-G---CGGGGGG" +
            "GGGGGGGGGGGGGGG" +
            "GGGGGGGGGGGGGGG" +
            "GGGGGGGGGGGGGGG" +
            "GGGWWGGGGGGGGGG" +
            "GGGWWGGGGGGGGGG" +
            "GGGGGGGGGGGGGGG";
        // 'O': Ball
        public const string propsString =
            "XXXXXXXXXXXXXXX" +
            "XXXXXXXXXXXPPXX" +
            "XXXOXCXXXXPXXXX" +
            "XXXXXXXXXXXXXXX" +
            "XXXXXXXXXXXXXXX" +
            "XXXXXXXXXXXXXXX" +
            "XXXXXXXXXXXXXPX" +
            "XPXXXXXXXXXXXXX" +
            "XXXXXXXXXPXXXXX" +
            "XPXXXXXXXPXXXXX" +
            "XPXXXXXXXPXXXXX" +
            "XXXXXXXXXXXPXXX" +
            "XXXXXXXXXXXXXXX" +
            "XXXXXXXXXXXXXXX" +
            "XXXXXXXXXXXXXXX" +
            "XXXXXPXXXXXXXXX" +
            "XXXXXXXXXXXXXXX" +
            "XXXXXXXXXXXXPPXX" +
            "XXXXXXXXXXPPPXX" +
            "XXXXXXXXXXXPXXX";
        const float waterHeightDif = 0.25f;

        static Dictionary<int, GameObject> pushables = new Dictionary<int, GameObject>();

        public static List<GameObject> GenerateWorld()
        {
            List<GameObject> gameObjects = new List<GameObject>();

            // gameObjects.Add(GameObject.CreateFromFile(new Vector3(0f, 1f, -12f), Vector3.Zero, Vector3.One, AssetManager.GetMaterial("Mat_CelShading"), "sphere_poles")); //sphere
            // gameObjects.Add(GameObject.CreateFromFile(new Vector3(0f, 1f, -8f), Vector3.Zero, Vector3.One, AssetManager.GetMaterial("Mat_CelShading"), "Cel_Crate01"));

            gameObjects.Add(GameObject.CreateFromFile(new Vector3(0f, 1f, -8f), Vector3.Zero, Vector3.One * 0.75f, AssetManager.GetMaterial("Mat_Default_Light_Grey"), "hero_defense_char"));
            gameObjects[gameObjects.Count - 1].AddComp(new PlayerController(playerStartPos)); //add script-comp
            positionIndex = gameObjects.Count - 1;

            int tileChar = mapString.Length -1;
            for (int column = tileRows; column > 0; column--)
            {
                for (int row = tileColumns; row > 0; row--)
                {
                    #region TILES
                    if (mapString[tileChar] == 'G')
                    {
                        // water to the right
                        if(tileChar+1 < mapString.Length && mapString[tileChar+1] == 'W')
                        { gameObjects.Add(GameObject.CreateFromFile(new Vector3((column - 1) * tileDist, 0f, (row - 1) * tileDist), Vector3.Zero, Vector3.One, AssetManager.GetMaterial("Mat_Grass01"), "tile_extruded")); }
                        // water to the left
                        else if (tileChar - 1 >= 0 && mapString[tileChar - 1] == 'W')
                        { gameObjects.Add(GameObject.CreateFromFile(new Vector3((column - 1) * tileDist, 0f, (row - 1) * tileDist), Vector3.Zero, Vector3.One, AssetManager.GetMaterial("Mat_Grass01"), "tile_extruded")); }
                        // water above
                        else if (tileChar + tileColumns < mapString.Length && mapString[tileChar + tileColumns] == 'W')
                        { gameObjects.Add(GameObject.CreateFromFile(new Vector3((column - 1) * tileDist, 0f, (row - 1) * tileDist), Vector3.Zero, Vector3.One, AssetManager.GetMaterial("Mat_Grass01"), "tile_extruded")); }
                        // water below
                        else if (tileChar - tileColumns >= 0 && mapString[tileChar - tileColumns] == 'W')
                        { gameObjects.Add(GameObject.CreateFromFile(new Vector3((column - 1) * tileDist, 0f, (row - 1) * tileDist), Vector3.Zero, Vector3.One, AssetManager.GetMaterial("Mat_Grass01"), "tile_extruded")); }
                        else 
                        { gameObjects.Add(GameObject.CreateFromFile(new Vector3((column - 1) * tileDist, 0f, (row - 1) * tileDist), Vector3.Zero, Vector3.One, AssetManager.GetMaterial("Mat_Grass01"), "tile_flat")); }
                    }
                    else if (mapString[tileChar] == 'W')
                    {
                        gameObjects.Add(GameObject.CreateFromFile(new Vector3((column - 1) * tileDist, 0f - waterHeightDif, (row - 1) * tileDist), Vector3.Zero, Vector3.One, AssetManager.GetMaterial("Mat_Default"), "tile_flat"));
                    }
                    else if (mapString[tileChar] == '|')
                    {
                        gameObjects.Add(GameObject.CreateFromFile(new Vector3((column - 1) * tileDist, 0f, (row - 1) * tileDist), Vector3.Zero, Vector3.One, AssetManager.GetMaterial("Mat_UV-Checkered"), "tile_wall_sideways"));
                    }
                    else if (mapString[tileChar] == '-')
                    {
                        gameObjects.Add(GameObject.CreateFromFile(new Vector3((column - 1) * tileDist, 0f, (row - 1) * tileDist), Vector3.Zero, Vector3.One, AssetManager.GetMaterial("Mat_UV-Checkered"), "tile_wall_straight"));
                    }
                    else if (mapString[tileChar] == 'C')
                    {
                        gameObjects.Add(GameObject.CreateFromFile(new Vector3((column - 1) * tileDist, 0f, (row - 1) * tileDist), Vector3.Zero, Vector3.One, AssetManager.GetMaterial("Mat_UV-Checkered"), "tile_wall_corner"));
                    }
                    #endregion

                    #region PROPS
                    if(propsString[tileChar] == 'O')
                    {
                        gameObjects.Add(GameObject.CreateFromFile(new Vector3((column - 1) * tileDist, 1f, (row - 1) * tileDist), Vector3.Zero, Vector3.One * 1.25f, AssetManager.GetMaterial("Mat_UV-Checkered"), "sphere_subdiv02"));
                        gameObjects[gameObjects.Count - 1].AddComp(new PushableObject(tileChar));
                        pushables.Add(tileChar, gameObjects[gameObjects.Count -1]); // add to the tracked pushable-object
                    }
                    else if (propsString[tileChar] == 'C')
                    {
                        gameObjects.Add(GameObject.CreateFromFile(new Vector3((column - 1) * tileDist, 1f, (row - 1) * tileDist), Vector3.Zero, Vector3.One, AssetManager.GetMaterial("Mat_Crate01"), "crate01"));
                        gameObjects[gameObjects.Count - 1].AddComp(new PushableObject(tileChar));
                        pushables.Add(tileChar, gameObjects[gameObjects.Count - 1]); // add to the tracked pushable-object
                    }
                    else if (propsString[tileChar] == 'P')
                    {
                        gameObjects.Add(GameObject.CreateFromFile(new Vector3((column - 1) * tileDist, 0f, (row - 1) * tileDist), Vector3.Zero, Vector3.One, AssetManager.GetMaterial("Mat_Plant01"), "post_apocalyptic_plant02"));
                    }
                    #endregion

                    tileChar--;
                }
            }

            BBug.StartTimer("Lights creation");
            gameObjects.Add(GameObject.CreateDirectionalLight(new Vector3(0.5f, 4f, 0f), new Vector3(20f, -30f, 0f), Vector3.One, AssetManager.GetMaterial("Mat_DefaultLight"), new Vector3(1.0f, 1.0f, 1.0f), 1.0f)); //0.5f
            BBug.StopTimer();

            return gameObjects;
        }

        // @CLEANUP: remove pushable-check into different func as this one should only check walk-ability
        public static bool IsWalkableTile(int oldTileIndex, int newTileIndex)
        {
            if(newTileIndex < 0 || newTileIndex >= mapString.Length) { return false; } // out of bounds of the array

            // use recursion to check the tile the pushable would be pushed
            if(pushables.ContainsKey(newTileIndex))
            {
                // @TODO: the pushables wrap around the sides just like the player did,
                //        prob. fix this by using a Move() func in the base-class
                // pushing to the left
                if(oldTileIndex - newTileIndex == 1)
                {
                    if (!IsWalkableTile(newTileIndex, newTileIndex - 1)) // recursively check to make pushables push pushables
                    { return false; }
                    pushables[newTileIndex].GetComp<PushableObject>().Move(Direction.Left);

                    // @CLEANUP: is there a way to assign/change a key ?
                    // create a new entry with the updated index as the key and remove the old one
                    pushables.Add(newTileIndex - 1, pushables[newTileIndex]);
                    pushables.Remove(newTileIndex);
                }
                // pushing to the right
                if (newTileIndex - oldTileIndex == 1)
                {
                    if (!IsWalkableTile(newTileIndex, newTileIndex + 1)) // recursively check to make pushables push pushables
                    { return false; }
                    pushables[newTileIndex].GetComp<PushableObject>().Move(Direction.Right);

                    // @CLEANUP: is there a way to assign/change a key ?
                    // create a new entry with the updated index as the key and remove the old one
                    pushables.Add(newTileIndex + 1, pushables[newTileIndex]);
                    pushables.Remove(newTileIndex);
                }
                // pushing up
                if (newTileIndex == (oldTileIndex + tileColumns))
                {
                    if(!IsWalkableTile(newTileIndex, newTileIndex + tileColumns)) // recursively check to make pushables push pushables
                    { return false; }
                    pushables[newTileIndex].GetComp<PushableObject>().Move(Direction.Up);

                    // @CLEANUP: is there a way to assign/change a key ?
                    // create a new entry with the updated index as the key and remove the old one
                    pushables.Add(newTileIndex + tileColumns, pushables[newTileIndex]);
                    pushables.Remove(newTileIndex);
                }
                // pushing down
                if (newTileIndex == (oldTileIndex - tileColumns))
                {
                    if (!IsWalkableTile(newTileIndex, newTileIndex - tileColumns)) // recursively check to make pushables push pushables
                    { return false; }
                    pushables[newTileIndex].GetComp<PushableObject>().Move(Direction.Down);

                    // @CLEANUP: is there a way to assign/change a key ?
                    // create a new entry with the updated index as the key and remove the old one
                    pushables.Add(newTileIndex - tileColumns, pushables[newTileIndex]);
                    pushables.Remove(newTileIndex);
                }
            }

            // BBug.Log("Attempt to walk on tile: '" + mapString[tileIndex] + "'");
            return mapString[newTileIndex] != 'W' && mapString[newTileIndex] != 'C' && mapString[newTileIndex] != '|' && mapString[newTileIndex] != '-';
        }

        // @Cleanup, @Refactor: move this into the base-class for player, pushables, etc.
        static void TileToPos(int curPos, out int xPos, out int zPos)
        {
            xPos = ((curPos / EnvController.tileColumns) % EnvController.tileRows) * EnvController.tileDist;
            zPos = (curPos % EnvController.tileColumns) * EnvController.tileDist;
        }
    }
}
