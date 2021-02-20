using BitsCore;
using BitsCore.Debugging;
using BitsCore.InputSystem;
using BitsCore.ObjectData.Components;
using BitsCore.Rendering;
using BitsCore.Utils;
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
        const float camFollowSpeed = 0.5f;

        // @TODO: use const floats/ints here
        Vector3 playerRot_Forward   = new Vector3(0f, 90f, 0f);
        Vector3 playerRot_Backward  = new Vector3(0f, -90f, 0f);
        Vector3 playerRot_Left      = new Vector3(0f, 180f, 0f);
        Vector3 playerRot_Right     = new Vector3(0f, 0f, 0f);

        PlayerOrientation playerOrientation = PlayerOrientation.Forward;
        internal int curSelectedTile = 0;
        private int xPos; // private as the are only correct directly after a call to UpdateCurTilePos()
        private int zPos; // private as the are only correct directly after a call to UpdateCurTilePos()

        public override void OnStart()
        {
            app = ((BeSafeApp)Program.app);
        }

        public override void OnUpdate()
        {

            #region INPUT
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

            if (curSelectedTile != pre_curSelectedTile) // only move when the char has actually moved
            {
                UpdateCurTilePos();
            }
            #endregion

            #region CAMERA_FOLLOW
            // camera follow
            Vector3 newCamPos = Renderer.mainCam.transform.position;
            if (Renderer.mainCam.transform.position.X < xPos + camOffset.X) { newCamPos.X += camFollowSpeed; }
            if (Renderer.mainCam.transform.position.X > xPos + camOffset.X) { newCamPos.X -= camFollowSpeed; }
            if (Renderer.mainCam.transform.position.Y < 0f + camOffset.Y)   { newCamPos.Y += camFollowSpeed; }
            if (Renderer.mainCam.transform.position.Y > 0f + camOffset.Y)   { newCamPos.Y -= camFollowSpeed; }
            if (Renderer.mainCam.transform.position.Z < zPos + camOffset.Z) { newCamPos.Z += camFollowSpeed; }
            if (Renderer.mainCam.transform.position.Z > zPos + camOffset.Z) { newCamPos.Z -= camFollowSpeed; }

            Renderer.mainCam.transform.position = newCamPos;

            #endregion
        }

        // update the placement of the player-char
        public void UpdateCurTilePos()
        {
            xPos = ((curSelectedTile / EnvController.tileRows) % EnvController.tileColumns) * EnvController.tileDist;
            zPos = (curSelectedTile % EnvController.tileRows) * EnvController.tileDist;

            gameObject.transform.position = new Vector3(xPos, 0f, zPos);

            // set player rotation
            gameObject.transform.rotation = playerOrientation == PlayerOrientation.Forward ? playerRot_Forward :
                                            playerOrientation == PlayerOrientation.Backward ? playerRot_Backward :
                                            playerOrientation == PlayerOrientation.Left ? playerRot_Left :
                                            playerOrientation == PlayerOrientation.Right ? playerRot_Right : Vector3.Zero;
        }

        int CheckMove(int newPos)
        {
            // check if the new position is out of bounds or a unwalkable tile
        }

    }
}
