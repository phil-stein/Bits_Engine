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

        // vars ------------------------------------------
        Vector3 camOffset = new Vector3(-25f, 17.5f, 0f);
        const float camFollowSpeed = 0.5f;

        // @TODO: use const floats/ints here
        Vector3 playerRot_Forward   = new Vector3(0f, 90f, 0f);
        Vector3 playerRot_Backward  = new Vector3(0f, -90f, 0f);
        Vector3 playerRot_Left      = new Vector3(0f, 180f, 0f);
        Vector3 playerRot_Right     = new Vector3(0f, 0f, 0f);
        PlayerOrientation playerOrientation = PlayerOrientation.Forward;
        
        int curPosition = 0;
        private int xPos; // private as the are only correct directly after a call to UpdateCurTilePos()
        private int zPos; // private as the are only correct directly after a call to UpdateCurTilePos()

        bool editorCamMode = false;

        public override void OnStart()
        {
            app = ((BeSafeApp)Program.app);
        }

        public override void OnUpdate()
        {
            #region CHAR_MOVEMENT
            if(Input.IsPressed(KeyCode.Enter))
            {
                editorCamMode = !editorCamMode;
                if(!editorCamMode)
                {
                    Renderer.mainCam.transform.position = new Vector3(xPos + camOffset.X, 0f + camOffset.Y, zPos + camOffset.Z);
                }
            }

            //move cur selected tile ring
            int pre_curSelectedTile = curPosition;
            if (Input.IsPressed(KeyCode.LeftArrow))
            {
                curPosition = CheckMove(curPosition -1);
                curPosition = curPosition < 0 ? 0 : curPosition;
                playerOrientation = PlayerOrientation.Left;
                BBug.Log("Cur. Selected Tile: " + curPosition);
            }
            if (Input.IsPressed(KeyCode.RightArrow))
            {
                curPosition = CheckMove(curPosition +1);
                curPosition = curPosition > EnvController.tileColumns * EnvController.tileRows - 1 ? EnvController.tileColumns * EnvController.tileRows - 1 : curPosition;
                playerOrientation = PlayerOrientation.Right;
                BBug.Log("Cur. Selected Tile: " + curPosition);
            }
            if (Input.IsPressed(KeyCode.UpArrow))
            {
                curPosition = CheckMove(curPosition + EnvController.tileColumns);
                curPosition = curPosition > EnvController.tileColumns * EnvController.tileRows - 1 ? curPosition - EnvController.tileColumns : curPosition;
                playerOrientation = PlayerOrientation.Forward;
                BBug.Log("Cur. Selected Tile: " + curPosition);
            }
            if (Input.IsPressed(KeyCode.DownArrow))
            {
                curPosition = CheckMove(curPosition - EnvController.tileColumns);
                curPosition = curPosition < 0 ? curPosition + EnvController.tileColumns : curPosition;
                playerOrientation = PlayerOrientation.Backward;
                BBug.Log("Cur. Selected Tile: " + curPosition);
            }

            if (curPosition != pre_curSelectedTile) // only move when the char has actually moved
            {
                UpdateCurTilePos();
            }
            #endregion

            #region CAMERA_FOLLOW

            // camera follow
            if (!editorCamMode)
            {
                Vector3 newCamPos = Renderer.mainCam.transform.position;
                if (Renderer.mainCam.transform.position.X < xPos + camOffset.X) { newCamPos.X += camFollowSpeed; }
                if (Renderer.mainCam.transform.position.X > xPos + camOffset.X) { newCamPos.X -= camFollowSpeed; }
                if (Renderer.mainCam.transform.position.Y < 0f + camOffset.Y) { newCamPos.Y += camFollowSpeed; }
                if (Renderer.mainCam.transform.position.Y > 0f + camOffset.Y) { newCamPos.Y -= camFollowSpeed; }
                if (Renderer.mainCam.transform.position.Z < zPos + camOffset.Z) { newCamPos.Z += camFollowSpeed; }
                if (Renderer.mainCam.transform.position.Z > zPos + camOffset.Z) { newCamPos.Z -= camFollowSpeed; }

                Renderer.mainCam.transform.position = newCamPos;
            }
            #endregion

            #region EDITOR_CAMERA

            if(editorCamMode)
            {
                //move cam
                float camSpeed = 15f * GameTime.DeltaTime;
                if (Input.IsDown(KeyCode.W))
                {
                    Renderer.mainCam.transform.Move(camSpeed, 0f, 0f);
                }
                if (Input.IsDown(KeyCode.S))
                {
                    Renderer.mainCam.transform.Move(-camSpeed, 0f, 0f);
                }
                if (Input.IsDown(KeyCode.A))
                {
                    Renderer.mainCam.transform.Move(0f, 0f, -camSpeed);
                }
                if (Input.IsDown(KeyCode.D))
                {
                    Renderer.mainCam.transform.Move(0f, 0f, camSpeed);
                }
                if (Input.IsDown(KeyCode.Q))
                {
                    Renderer.mainCam.transform.Move(0f, -camSpeed * 0.33f, 0f);
                }
                if (Input.IsDown(KeyCode.E))
                {
                    Renderer.mainCam.transform.Move(0f, camSpeed * 0.33f, 0f);
                }
            }
            #endregion
        }

        /// <summary> Update the placement of the player-char. </summary>
        public void UpdateCurTilePos()
        {
            xPos = ((curPosition / EnvController.tileColumns) % EnvController.tileRows) * EnvController.tileDist;
            zPos = (curPosition % EnvController.tileColumns) * EnvController.tileDist;

            gameObject.transform.position = new Vector3(xPos, 0f, zPos);

            // set player rotation
            gameObject.transform.rotation = playerOrientation == PlayerOrientation.Forward ? playerRot_Forward :
                                            playerOrientation == PlayerOrientation.Backward ? playerRot_Backward :
                                            playerOrientation == PlayerOrientation.Left ? playerRot_Left :
                                            playerOrientation == PlayerOrientation.Right ? playerRot_Right : Vector3.Zero;
        }

        /// <summary> Check if the new position is out of bounds or a un-walkable tile. </summary>
        /// <param name="newPos"> The position be checked for walk-ability. </param>
        int CheckMove(int newPos)
        {
            // out of bounds ----------------------------------
            // trying to move to the left
            if (curPosition > newPos && (curPosition - newPos) == 1 && curPosition % EnvController.tileColumns == 0) 
            {
                return curPosition;  
            }
            // trying to move to the right
            if (curPosition < newPos && (newPos - curPosition) == 1 && (curPosition +1) % EnvController.tileColumns == 0) 
            {
                return curPosition;
            }

            if(!EnvController.IsWalkableTile(curPosition, newPos))
            {
                return curPosition;
            }
            return newPos;
        }

    }
}
