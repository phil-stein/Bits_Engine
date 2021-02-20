using BitsCore;
using BitsCore.Debugging;
using BitsCore.InputSystem;
using BitsCore.ObjectData.Components;
using BitsCore.Rendering;
using System.Numerics;

namespace BeSafe.Scripts
{
    public class PlayerController : ScriptComponent
    {
        // @TODO: somehow put this in the scriptcomponent class
        BeSafeApp app;

        enum PlayerOrientation { Forward, Backward, Left, Right };

        // standard values
        Vector3 camOffset = new Vector3(-25f, 17.5f, 0f);

        Vector3 playerRot_Forward   = new Vector3(0f, 90f, 0f);
        Vector3 playerRot_Backward  = new Vector3(0f, -90f, 0f);
        Vector3 playerRot_Left      = new Vector3(0f, 180f, 0f);
        Vector3 playerRot_Right     = new Vector3(0f, 0f, 0f);

        PlayerOrientation playerOrientation = PlayerOrientation.Forward;
        internal int curSelectedTile = 0;

        public override void OnStart()
        {
            app = ((BeSafeApp)Program.app);
        }

        public override void OnUpdate()
        {

            //move cur selected tile ring
            int pre_curSelectedTile = curSelectedTile;
            if (Input.IsPressed(KeyCode.LeftArrow))
            {
                curSelectedTile--;
                curSelectedTile = curSelectedTile < 0 ? 0 : curSelectedTile;
                playerOrientation = PlayerOrientation.Left;
                BBug.Log("Cur. Selected Tile: " + curSelectedTile);
            }
            if (Input.IsPressed(KeyCode.RightArrow))
            {
                curSelectedTile++;
                curSelectedTile = curSelectedTile > EnvController.tileRows * EnvController.tileColumns - 1 ? EnvController.tileRows * EnvController.tileColumns - 1 : curSelectedTile;
                playerOrientation = PlayerOrientation.Right;
                BBug.Log("Cur. Selected Tile: " + curSelectedTile);
            }
            if (Input.IsPressed(KeyCode.UpArrow))
            {
                curSelectedTile += EnvController.tileRows;
                curSelectedTile = curSelectedTile > EnvController.tileRows * EnvController.tileColumns - 1 ? curSelectedTile - EnvController.tileRows : curSelectedTile;
                playerOrientation = PlayerOrientation.Forward;
                BBug.Log("Cur. Selected Tile: " + curSelectedTile);
            }
            if (Input.IsPressed(KeyCode.DownArrow))
            {
                curSelectedTile -= EnvController.tileRows;
                curSelectedTile = curSelectedTile < 0 ? curSelectedTile + EnvController.tileRows : curSelectedTile;
                playerOrientation = PlayerOrientation.Backward;
                BBug.Log("Cur. Selected Tile: " + curSelectedTile);
            }

            if(curSelectedTile != pre_curSelectedTile) // only move when the char has actually moved
            {
                UpdateCurTilePos();
            }
            
        }

        public void UpdateCurTilePos()
        {
            int xPos = ((curSelectedTile / EnvController.tileRows) % EnvController.tileColumns) * EnvController.tileDist;
            int zPos = (curSelectedTile % EnvController.tileRows) * EnvController.tileDist;

            gameObject.transform.position = new Vector3(xPos, 0f, zPos);
            Renderer.mainCam.transform.position = new Vector3(xPos + camOffset.X, 0f + camOffset.Y, zPos + camOffset.Z);

            // set player rotation
            gameObject.transform.rotation = playerOrientation == PlayerOrientation.Forward ? playerRot_Forward :
                                            playerOrientation == PlayerOrientation.Backward ? playerRot_Backward :
                                            playerOrientation == PlayerOrientation.Left ? playerRot_Left :
                                            playerOrientation == PlayerOrientation.Right ? playerRot_Right : Vector3.Zero;
        }

    }
}
