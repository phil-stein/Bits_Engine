using BitsCore;
using BitsCore.Debugging;
using BitsCore.ObjectData;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace BeSafe.Scripts
{
    public static class EnvController
    {
        public static int tileColumns    = 8;
        public static int tileRows       = 5;
        public static int tileDist       = 5;
        public static int positionIndex  = 0;

        public static List<GameObject> GenerateWorld()
        {
            List<GameObject> gameObjects = new List<GameObject>();

            // gameObjects.Add(GameObject.CreateFromFile(new Vector3(0f, 1f, -12f), Vector3.Zero, Vector3.One, AssetManager.GetMaterial("Mat_CelShading"), "sphere_poles")); //sphere
            // gameObjects.Add(GameObject.CreateFromFile(new Vector3(0f, 1f, -8f), Vector3.Zero, Vector3.One, AssetManager.GetMaterial("Mat_CelShading"), "Cel_Crate01"));

            gameObjects.Add(GameObject.CreateFromFile(new Vector3(0f, 1f, -8f), Vector3.Zero, Vector3.One * 0.75f, AssetManager.GetMaterial("Mat_Default_Grey"), "hero_defense_char"));
            gameObjects[gameObjects.Count - 1].AddComp(new PlayerController()); //add script-comp
            positionIndex = gameObjects.Count - 1;

            for (int column = tileColumns; column > 0; column--)
            {
                for (int row = tileRows; row > 0; row--)
                {
                    gameObjects.Add(GameObject.CreateFromFile(new Vector3((column - 1) * tileDist, 0f, (row - 1) * tileDist), Vector3.Zero, Vector3.One, AssetManager.GetMaterial("Mat_Tile"), "tile01")); // Mat_Cel_Tile
                }
            }

            BBug.StartTimer("Lights creation");
            gameObjects.Add(GameObject.CreateDirectionalLight(new Vector3(0.5f, 4f, 0f), new Vector3(20f, -30f, 0f), Vector3.One, AssetManager.GetMaterial("Mat_DefaultLight"), new Vector3(1.0f, 1.0f, 1.0f), 0.75f)); //0.5f
            BBug.StopTimer();

            return gameObjects;
        }
    }
}
