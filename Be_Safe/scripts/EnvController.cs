using BitsCore;
using BitsCore.Debugging;
using BitsCore.ObjectData;
using BitsCore.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Numerics;
using System.Text;

namespace BeSafe.Scripts
{
    public static class EnvController
    {
        struct MapObject
        {
            public GameObject gameObject;
            public TileObjectType type;
            public int tileIndex;

            public MapObject(GameObject _gameObject, TileObjectType _type, int _tileIndex)
            {
                this.gameObject = _gameObject;
                this.type = _type;
                this.tileIndex = _tileIndex;
            }
        }

        public static int tileRows       = 0;
        public static int tileColumns    = 0;
        public static int tileDist       = 4;
        public static int positionIndex  = 0;

        public static int playerStartPos = 0;

        #region MAP
        // 'G': Grass; 'W': Water; '|': Wall-Straight; '-': Wall-Sideways; 'C': Corner; 'P': PressurePlate; 'D': Door
        public const string mapString =
            "C-G-----------C" +
            "|GGGGGGGGGGGGG|" +
            "|GGGGGGGGGGWWG|" +
            "|GGGGGGGGGWWWG|" +
            "|GGGGGGGGGWWWG|" +
            "CGGGGGGGGGGGGG|" +
            "GGGGGGGGGGGGGG|" +
            "GGC-G---CGGGGG|" +
            "GG|GGGGG|GGGGG|" +
            "GG|GGGGG|GGGGGC" +
            "GG|GGGGG|GGGGGG" +
            "GG|GGGPG|GGGGGG" +
            "GG|GGGGG|GGGGGG" +
            "GGC-D---CGGGGGG" +
            "GGGGGGGGGGGGGGG" +
            "GGGGGGGGGGGGGGG" +
            "GGGGGGGGGGGGGGG" +
            "GGGWWGGGGGGGGGG" +
            "GGGWWGGGGGGGGGG" +
            "GGGGGGGGGGGGGGG";
        // 'O': Ball; 'C': Crate; 'P': Plant
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
        #endregion

        static TileData[] mapData;

        static GameObject doorObj; // @TEMP: only untile pressureplate - door linking is proper

        static Dictionary<int, MapObject> tileObjects = new Dictionary<int, MapObject>();

        public static List<GameObject> GenerateWorld()
        {
            List<GameObject> gameObjects = new List<GameObject>();

            // gameObjects.Add(GameObject.CreateFromFile(new Vector3(0f, 1f, -12f), Vector3.Zero, Vector3.One, AssetManager.GetMaterial("Mat_CelShading"), "sphere_poles")); //sphere
            // gameObjects.Add(GameObject.CreateFromFile(new Vector3(0f, 1f, -8f), Vector3.Zero, Vector3.One, AssetManager.GetMaterial("Mat_CelShading"), "Cel_Crate01"));

            gameObjects.Add(GameObject.CreateFromFile(new Vector3(0f, 0f, -8f), Vector3.Zero, Vector3.One * 0.75f, AssetManager.GetMaterial("Mat_Default_Light_Grey"), "hero_defense_char"));
            gameObjects[gameObjects.Count -1].AddComp(new PlayerController(playerStartPos)); //add script-comp
            gameObjects[gameObjects.Count - 1].GetComp<PlayerController>().UpdatePlayerTilePos(); // on beeing spawned this sets the right location
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
                    else if (mapString[tileChar] == 'D')
                    {
                        gameObjects.Add(GameObject.CreateFromFile(new Vector3((column - 1) * tileDist, 0f, (row - 1) * tileDist), Vector3.Zero, Vector3.One, AssetManager.GetMaterial("Mat_UV-Checkered"), "tile_wall_straight"));
                        doorObj = gameObjects[gameObjects.Count - 1];
                    }
                    else if (mapString[tileChar] == 'P')
                    {
                        if (doorObj == null) { continue; } // @TEMP: only untile pressureplate - door linking is proper
                        gameObjects.Add(GameObject.CreateFromFile(new Vector3((column - 1) * tileDist, 0f, (row - 1) * tileDist), Vector3.Zero, Vector3.One, AssetManager.GetMaterial("Mat_UV-Checkered"), "tile_pressure_plate"));
                        gameObjects[gameObjects.Count - 1].AddComp(new PressurePlateObject(tileChar, PressurePlateObject.PressurePlateType.Door, doorObj));
                        tileObjects.Add(tileChar, new MapObject(gameObjects[gameObjects.Count - 1], TileObjectType.PressurePlate, tileChar)); // add to the tracked tile-objects
                    }
                    #endregion

                    #region PROPS
                    if (propsString[tileChar] == 'O')
                    {
                        gameObjects.Add(GameObject.CreateFromFile(new Vector3((column - 1) * tileDist, 1f, (row - 1) * tileDist), Vector3.Zero, Vector3.One * 1.25f, AssetManager.GetMaterial("Mat_UV-Checkered"), "sphere_subdiv02"));
                        gameObjects[gameObjects.Count - 1].AddComp(new PushableObject(tileChar));
                        tileObjects.Add(tileChar, new MapObject(gameObjects[gameObjects.Count -1], TileObjectType.Pushable, tileChar)); // add to the tracked pushable-object
                    }
                    else if (propsString[tileChar] == 'C')
                    {
                        gameObjects.Add(GameObject.CreateFromFile(new Vector3((column - 1) * tileDist, 1f, (row - 1) * tileDist), Vector3.Zero, Vector3.One, AssetManager.GetMaterial("Mat_Crate01"), "crate01"));
                        gameObjects[gameObjects.Count - 1].AddComp(new PushableObject(tileChar));
                        tileObjects.Add(tileChar, new MapObject(gameObjects[gameObjects.Count - 1], TileObjectType.Pushable, tileChar)); // add to the tracked pushable-object
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

        public static List<GameObject> GenerateWorldTextFile(string fileName)
        {
            // load the map data
            mapData = MapLoader.LoadMap(fileName, true, out int columns, out int rows, out int playerStart);

            tileColumns    = columns;
            tileRows       = rows;
            playerStartPos = playerStart;
            BBug.Log("Read PlayerStart: '" + playerStart + "'");

            List<GameObject> gameObjects = new List<GameObject>();

            gameObjects.Add(GameObject.CreateFromFile(new Vector3(0f, 0f, -8f), Vector3.Zero, Vector3.One * 0.75f, AssetManager.GetMaterial("Mat_Default_Light_Grey"), "hero_defense_char"));
            gameObjects[gameObjects.Count - 1].AddComp(new PlayerController(playerStartPos)); //add script-comp
            gameObjects[gameObjects.Count - 1].GetComp<PlayerController>().UpdatePlayerTilePos(); // on beeing spawned this sets the right location
            positionIndex = gameObjects.Count - 1;

            int mapDataPos = mapData.Length -1; // walks through the mapdata backwards
            for (int column = tileRows; column > 0 && mapDataPos >= 0; column--)
            { 
                for (int row = tileColumns; row > 0 && mapDataPos >= 0; row--)
                {
                    #region GROUND
                    if (mapData[mapDataPos].groundType == TileData.GroundType.Grass)
                    {
                        // water to the right
                        if (mapDataPos + 1 < mapData.Length && mapData[mapDataPos + 1].groundType == TileData.GroundType.Water)
                        { gameObjects.Add(GameObject.CreateFromFile(new Vector3((column - 1) * tileDist, 0f, (row - 1) * tileDist), Vector3.Zero, Vector3.One, AssetManager.GetMaterial("Mat_Grass01"), "tile_extruded")); }
                        // water to the left
                        else if (mapDataPos - 1 >= 0 && mapData[mapDataPos - 1].groundType == TileData.GroundType.Water)
                        { gameObjects.Add(GameObject.CreateFromFile(new Vector3((column - 1) * tileDist, 0f, (row - 1) * tileDist), Vector3.Zero, Vector3.One, AssetManager.GetMaterial("Mat_Grass01"), "tile_extruded")); }
                        // water above
                        else if (mapDataPos + tileColumns < mapData.Length && mapData[mapDataPos + tileColumns].groundType == TileData.GroundType.Water)
                        { gameObjects.Add(GameObject.CreateFromFile(new Vector3((column - 1) * tileDist, 0f, (row - 1) * tileDist), Vector3.Zero, Vector3.One, AssetManager.GetMaterial("Mat_Grass01"), "tile_extruded")); }
                        // water below
                        else if (mapDataPos - tileColumns >= 0 && mapData[mapDataPos - tileColumns].groundType == TileData.GroundType.Water)
                        { gameObjects.Add(GameObject.CreateFromFile(new Vector3((column - 1) * tileDist, 0f, (row - 1) * tileDist), Vector3.Zero, Vector3.One, AssetManager.GetMaterial("Mat_Grass01"), "tile_extruded")); }
                        else
                        { gameObjects.Add(GameObject.CreateFromFile(new Vector3((column - 1) * tileDist, 0f, (row - 1) * tileDist), Vector3.Zero, Vector3.One, AssetManager.GetMaterial("Mat_Grass01"), "tile_flat")); }
                        
                        gameObjects.Add(GameObject.CreateFromFile(new Vector3((column - 1) * tileDist, 0f, (row - 1) * tileDist), Vector3.Zero, Vector3.One, AssetManager.GetMaterial("Mat_Grass01"), "tile_flat"));
                    }
                    else if (mapData[mapDataPos].groundType == TileData.GroundType.Water)
                    {
                        gameObjects.Add(GameObject.CreateFromFile(new Vector3((column - 1) * tileDist, 0f - waterHeightDif, (row - 1) * tileDist), Vector3.Zero, Vector3.One, AssetManager.GetMaterial("Mat_Default"), "tile_flat"));
                    }
                    #endregion

                    #region STRUCTURES
                    if (mapData[mapDataPos].structureType == TileData.StructureType.Wall_Corner)
                    {
                        gameObjects.Add(GameObject.CreateFromFile(new Vector3((column - 1) * tileDist, 0f, (row - 1) * tileDist), Vector3.Zero, Vector3.One, AssetManager.GetMaterial("Mat_UV-Checkered"), "tile_wall_corner_round"));
                    }
                    else if (mapData[mapDataPos].structureType == TileData.StructureType.Wall_Straight)
                    {
                        gameObjects.Add(GameObject.CreateFromFile(new Vector3((column - 1) * tileDist, 0f, (row - 1) * tileDist), Vector3.Zero, Vector3.One, AssetManager.GetMaterial("Mat_Wall01"), "tile_wall_straight"));
                    }
                    else if (mapData[mapDataPos].structureType == TileData.StructureType.Wall_Sideways)
                    {
                        gameObjects.Add(GameObject.CreateFromFile(new Vector3((column - 1) * tileDist, 0f, (row - 1) * tileDist), new Vector3(0f, 90f, 0f), Vector3.One, AssetManager.GetMaterial("Mat_Wall01"), "tile_wall_straight")); // the straight wall just rotated
                    }
                    else if (mapData[mapDataPos].structureType == TileData.StructureType.PressurePlate)
                    {
                        // if (doorObj == null) { continue; } // @TEMP: only untile pressureplate - door linking is proper
                        gameObjects.Add(GameObject.CreateFromFile(new Vector3((column - 1) * tileDist, 0f, (row - 1) * tileDist), Vector3.Zero, Vector3.One, AssetManager.GetMaterial("Mat_UV-Checkered"), "tile_pressure_plate"));
                        //gameObjects[gameObjects.Count - 1].AddComp(new PressurePlateObject(tileChar, PressurePlateObject.PressurePlateType.Door, doorObj));
                        //tileObjects.Add(tileChar, new MapObject(gameObjects[gameObjects.Count - 1], TileObjectType.PressurePlate, tileChar)); // add to the tracked tile-objects
                    }
                    #endregion

                    #region OBJECTS
                    foreach (TileData.ObjectType obj in mapData[mapDataPos].objectTypes)
                    if (obj == TileData.ObjectType.Crate)
                    {
                        gameObjects.Add(GameObject.CreateFromFile(new Vector3((column - 1) * tileDist, 1f, (row - 1) * tileDist), Vector3.Zero, Vector3.One, AssetManager.GetMaterial("Mat_Crate01"), "crate01"));
                        gameObjects[gameObjects.Count - 1].AddComp(new PushableObject(mapDataPos));
                        tileObjects.Add(mapDataPos, new MapObject(gameObjects[gameObjects.Count - 1], TileObjectType.Pushable, mapDataPos)); // add to the tracked pushable-object
                    }
                    else if (obj == TileData.ObjectType.Plant)
                    {
                            gameObjects.Add(GameObject.CreateFromFile(new Vector3((column - 1) * tileDist, 0f, (row - 1) * tileDist), new Vector3(0f, 180f, 0f), Vector3.One, AssetManager.GetMaterial("Mat_Plants01"), "plants01_group_wall_straight01"));
                    }
                    #endregion

                    mapDataPos--;
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

            // @UNSURE: move this to PushableObject class ???
            // use recursion to check the tile the pushable would be pushed
            if(tileObjects.ContainsKey(newTileIndex))
            {
                if(tileObjects[newTileIndex].type == TileObjectType.Pushable)
                {
                    // @TODO: the pushables wrap around the sides just like the player did,
                    //        prob. fix this by using a Move() func in the base-class
                    //        PS. Move() did fix it but now the objects get stuck
                    // pushing to the left
                    if (oldTileIndex - newTileIndex == 1)
                    {
                        if (!IsWalkableTile(newTileIndex, newTileIndex - 1)) // recursively check to make pushables push pushables
                        { return false; }
                        tileObjects[newTileIndex].gameObject.GetComp<PushableObject>().Move(Direction.Left);

                        // @CLEANUP: is there a way to assign/change a key ?
                        // create a new entry with the updated index as the key and remove the old one
                        tileObjects.Add(newTileIndex - 1, tileObjects[newTileIndex]);
                        tileObjects.Remove(newTileIndex);
                    }
                    // pushing to the right
                    if (newTileIndex - oldTileIndex == 1)
                    {
                        if (!IsWalkableTile(newTileIndex, newTileIndex + 1)) // recursively check to make pushables push pushables
                        { return false; }
                        tileObjects[newTileIndex].gameObject.GetComp<PushableObject>().Move(Direction.Right);

                        // @CLEANUP: is there a way to assign/change a key ?
                        // create a new entry with the updated index as the key and remove the old one
                        tileObjects.Add(newTileIndex + 1, tileObjects[newTileIndex]);
                        tileObjects.Remove(newTileIndex);
                    }
                    // pushing up
                    if (newTileIndex == (oldTileIndex + tileColumns))
                    {
                        if (!IsWalkableTile(newTileIndex, newTileIndex + tileColumns)) // recursively check to make pushables push pushables
                        { return false; }
                        tileObjects[newTileIndex].gameObject.GetComp<PushableObject>().Move(Direction.Up);

                        // @CLEANUP: is there a way to assign/change a key ?
                        // create a new entry with the updated index as the key and remove the old one
                        tileObjects.Add(newTileIndex + tileColumns, tileObjects[newTileIndex]);
                        tileObjects.Remove(newTileIndex);
                    }
                    // pushing down
                    if (newTileIndex == (oldTileIndex - tileColumns))
                    {
                        if (!IsWalkableTile(newTileIndex, newTileIndex - tileColumns)) // recursively check to make pushables push pushables
                        { return false; }
                        tileObjects[newTileIndex].gameObject.GetComp<PushableObject>().Move(Direction.Down);

                        // @CLEANUP: is there a way to assign/change a key ?
                        // create a new entry with the updated index as the key and remove the old one
                        tileObjects.Add(newTileIndex - tileColumns, tileObjects[newTileIndex]);
                        tileObjects.Remove(newTileIndex);
                    }
                }

                else if(tileObjects[newTileIndex].type == TileObjectType.PressurePlate)
                {
                    // @CLEANUP: direction is irrelevant here

                    // pushing to the left
                    if (oldTileIndex - newTileIndex == 1)
                    {
                        tileObjects[newTileIndex].gameObject.GetComp<PressurePlateObject>().Interact(Direction.Left);
                    }
                    // pushing to the right
                    if (newTileIndex - oldTileIndex == 1)
                    {
                        tileObjects[newTileIndex].gameObject.GetComp<PressurePlateObject>().Interact(Direction.Right);
                    }
                    // pushing up
                    if (newTileIndex == (oldTileIndex + tileColumns))
                    {
                        tileObjects[newTileIndex].gameObject.GetComp<PressurePlateObject>().Interact(Direction.Up);
                    }
                    // pushing down
                    if (newTileIndex == (oldTileIndex - tileColumns))
                    {
                        tileObjects[newTileIndex].gameObject.GetComp<PressurePlateObject>().Interact(Direction.Down);
                    }
                }
            }

            // BBug.Log("Attempt to walk on tile: '" + mapString[tileIndex] + "'");
            return mapData[newTileIndex].groundType != TileData.GroundType.Water && mapData[newTileIndex].structureType != TileData.StructureType.Wall_Corner && mapData[newTileIndex].structureType != TileData.StructureType.Wall_Straight && mapData[newTileIndex].structureType != TileData.StructureType.Wall_Sideways;
        }

        public static void TileInfo(int tileIndex, out TileData tileData, out bool isObjOnTile, out TileObjectType objOnTileType, out GameObject tileObj)
        {
            tileData = mapData[tileIndex];

            if(tileObjects.ContainsKey(tileIndex))
            {
                isObjOnTile = true;
                objOnTileType = tileObjects[tileIndex].type;
                tileObj = tileObjects[tileIndex].gameObject;
            }
            else
            {
                // no obj in that tile
                isObjOnTile = false;
                objOnTileType = TileObjectType.Pushable;
                tileObj = null;
            }
        }

    }
}
