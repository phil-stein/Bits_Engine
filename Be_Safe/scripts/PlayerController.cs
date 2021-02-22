using BitsCore;
using BitsCore.Debugging;
using BitsCore.InputSystem;
using BitsCore.ObjectData.Components;
using BitsCore.Rendering;
using BitsCore.Utils;
using System.Numerics;

namespace BeSafe.Scripts
{
    public class PlayerController : TileObject
    {
        // @TODO: somehow put this in the scriptcomponent class
        BeSafeApp app;

        public enum PlayerOrientation { Forward, Backward, Left, Right };

        // vars ------------------------------------------
        Vector3 camOffset = new Vector3(-25f, 17.5f, 0f);
        const float camFollowSpeed = 0.5f;

        // @TODO: use const floats/ints here
        Vector3 playerRot_Forward   = new Vector3(0f, 90f, 0f);
        Vector3 playerRot_Backward  = new Vector3(0f, -90f, 0f);
        Vector3 playerRot_Left      = new Vector3(0f, 180f, 0f);
        Vector3 playerRot_Right     = new Vector3(0f, 0f, 0f);
        PlayerOrientation playerOrientation = PlayerOrientation.Forward;
        
        bool editorCamMode = false;

        public PlayerController(int _curPosition, PlayerOrientation initialOrientation = PlayerOrientation.Forward) : base(_curPosition)
        {
            type = TileObjectType.Player;
            playerOrientation = initialOrientation;
        }

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
                playerOrientation = PlayerOrientation.Left;
                Move(Direction.Left);
                app.mainLayerUI.SetText("PLAYER_TILE", "Cur. Tile: " + curPosition.ToString("00"));
            }
            if (Input.IsPressed(KeyCode.RightArrow))
            {
                playerOrientation = PlayerOrientation.Right;
                Move(Direction.Right);
                app.mainLayerUI.SetText("PLAYER_TILE", "Cur. Tile: " + curPosition.ToString("00"));
            }
            if (Input.IsPressed(KeyCode.UpArrow))
            {
                playerOrientation = PlayerOrientation.Forward;
                Move(Direction.Up);
                app.mainLayerUI.SetText("PLAYER_TILE", "Cur. Tile: " + curPosition.ToString("00"));
            }
            if (Input.IsPressed(KeyCode.DownArrow))
            {
                playerOrientation = PlayerOrientation.Backward;
                Move(Direction.Down);
                app.mainLayerUI.SetText("PLAYER_TILE", "Cur. Tile: " + curPosition.ToString("00"));
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

            if (Input.IsDown(KeyCode.G))
            {
                TweenUtils.TweenPos(gameObject, new Vector3(gameObject.transform.position.X, gameObject.transform.position.Y + 1f, gameObject.transform.position.Z), 0.2f, true);
            }
        }

        /// <summary> Update the placement of the player-char. </summary>
        public void UpdatePlayerTilePos()
        {
            UpdateCurTilePos(); // sets the position

            // set player rotation
            gameObject.transform.rotation = playerOrientation == PlayerOrientation.Forward ? playerRot_Forward :
                                            playerOrientation == PlayerOrientation.Backward ? playerRot_Backward :
                                            playerOrientation == PlayerOrientation.Left ? playerRot_Left :
                                            playerOrientation == PlayerOrientation.Right ? playerRot_Right : Vector3.Zero;
        }

        public override void Move(Direction dir)
        {
            // override as the player also need to rotate in the direction hes moving
            SetTileIndex(dir);
            UpdatePlayerTilePos(); // would be UpdateCurTilePos();

            app.mainLayerUI.SetText("PLAYER_POS", "Player X: " + gameObject.transform.position.X.ToString("00") + ", Z: " + gameObject.transform.position.Z.ToString("00"));
        }
    }
}
