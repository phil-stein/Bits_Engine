using BitsCore.ObjectData.Components;
using BitsCore.Rendering.Display;
using System;
using System.Diagnostics;
using System.Numerics;
using BitsCore.Debugging;
using BitsCore.Utils;
using BitsCore.InputSystem;

namespace BitsCore.Rendering.Cameras
{
    /// <summary> A Camera working in all 3-Dimensions </summary>
    [System.Serializable]
    public class Camera3D
    {
        public static Camera3D inst { get; private set; }
        public enum CameraMode { Fly, RotateAroundCenter, FirstPerson}
        public CameraMode cameraMode { get; private set; }
        public Transform transform;

        public float sensitivity = 1f;

        //for the 'FollowMouse()' function
        float lastX;
        float lastY;
        bool firstMouse = true;
        float yaw;
        float pitch;

        //jump
        bool jump = false;
        float maxJumpT = 1f;
        float curJumpT = 0f;
        float originalY;
        float groundY;
        float curGroundYDist = 1000f;
        int groundYindex;

        /// <summary> Generates a Camera. </summary>
        /// <param name="_position"> Position of the Camera. </param>
        /// /// <param name="_rotation"> Rotation of the Camera. </param>
        /// <param name="_cameraMode"> The mode the Camera gets set to. </param>
        public Camera3D(Vector3 _position, Vector3 _rotation, CameraMode _cameraMode)
        {
            if(inst == null) { inst = this; }
            else { return; }
            this.transform = new Transform(_position, _rotation, Vector3.One);
            this.cameraMode = _cameraMode;
            this.originalY = _position.Y;
        }

        /// <summary> Returns the Projection-Matrix of the Camera. </summary>
        public Matrix4x4 GetProjectionMatrix()
        {
            Matrix4x4 perspMatrix = Matrix4x4.CreatePerspectiveFieldOfView(0.45f, (float)(DisplayManager.WindowSize.X / DisplayManager.WindowSize.Y), 0.01f, 1000f);
            return perspMatrix;
        }
        /// <summary> Returns the Orthografic-Projection-Matrix of the Camera. </summary>
        public Matrix4x4 GetOrthoProjectionMatrix()
        {
            float left = transform.position.X - DisplayManager.WindowSize.X / 2f;
            float right = transform.position.X + DisplayManager.WindowSize.X / 2f;
            float top = transform.position.Y - DisplayManager.WindowSize.Y / 2f;
            float bottom = transform.position.Y + DisplayManager.WindowSize.Y / 2f;

            //last to args are the near-field-camera-clipping and the distance-culling for our camera (doen't matter because it's 2D)
            Matrix4x4 orthoMatrix = Matrix4x4.CreateOrthographicOffCenter(left, right, bottom, top, 0.01f, 100f);
            //Matrix4x4 zoomMatrix = Matrix4x4.CreateScale(); //transform.position.Z

            return orthoMatrix; //* zoomMatrix;
        }

        /// <summary> Returns the View-Matrix of the Camera. </summary>
        public Matrix4x4 GetViewMatrix()
        {
            switch (cameraMode) 
            {
                case CameraMode.RotateAroundCenter:

                    //rotates camera around the center of the screen at a 10f radius

                    transform.position = new Vector3(MathF.Sin(GameTime.TotalElapsedSeconds) * 10f, transform.position.Y, MathF.Cos(GameTime.TotalElapsedSeconds) * 10f);

                    Vector3 target = Vector3.Zero;
                    Vector3 direction = Vector3.Normalize(transform.position - target); //reverse is intentional
                    Vector3 localRight = Vector3.Normalize(Vector3.Cross(new Vector3(0f, 1f, 0f), direction));
                    Vector3 localUp = Vector3.Cross(direction, localRight);

                    Matrix4x4 view = Matrix4x4.CreateLookAt(transform.position, new Vector3(0f, transform.rotation.Y, 0f), new Vector3(0f, 1f, 0f));

                    return view;

                case CameraMode.Fly:

                    localRight = Vector3.Normalize(Vector3.Cross(new Vector3(0f, 1f, 0f), transform.GetForwardDir()));
                    localUp = Vector3.Cross(transform.GetForwardDir(), localRight);
                    view = Matrix4x4.CreateLookAt(transform.position, transform.position + transform.GetForwardDir(), localUp);

                    return view; //rot * pos;

                case CameraMode.FirstPerson:
                    localRight = Vector3.Normalize(Vector3.Cross(new Vector3(0f, 1f, 0f), transform.GetForwardDir()));
                    localUp = Vector3.Cross(transform.GetForwardDir(), localRight);
                    view = Matrix4x4.CreateLookAt(transform.position, transform.position + transform.GetForwardDir(), localUp);

                    return view; //rot * pos;

                default:
                    return Matrix4x4.Identity;
                
            }

        }

        /// <summary> Moves the Camera based on Input and Mode the Camera is set to. </summary>
        /// <param name="_position"> Position of the Camera. </param>
        public void MoveByInput()
        {
            //doesn't make the player go up or down
            if (cameraMode == CameraMode.FirstPerson)
            {
                /*
                Vector4 pos = System.Numerics.Vector4.Transform(transform.position, transform.GetModelMatrix());
                //Vector3 pos = new Vector3(pos4.X, pos4.Y, pos4.Z);
                float[] groundVerts = Program.app.mainLayer3D.RenderObjects[Program.app.mainLayer3D.RenderObjects.Count - 1].GetComp<Mesh>().GetVerticesWorldPosition();
                for(int i = 0; i < groundVerts.Length -3; i += 3)
                {
                    float dist = MathF.Abs(groundVerts[groundYindex + 1] - pos.Y);
                    float distNew = MathF.Abs(groundVerts[i + 1] - pos.Y);
                    if (distNew < dist) 
                    {
                        groundYindex = i;
                        groundY = groundVerts[i + 1];
                        curGroundYDist = dist;
                        Debug.WriteLine("Ground-Dist: " + curGroundYDist.ToString());
                    }
                }

                //moves the player up by a sin-wave !!! (for Terrain Collision) doesn't take reaching the floor before the sin-wave is over into consideration !!!
                if (jump)
                {
                    curJumpT += GameTime.DeltaTime;

                    float strenght = 2f;
                    transform.position = new Vector3(transform.position.X, originalY + ((MathF.Sin((curJumpT * 2) + .575f) - 0.525f) * strenght), transform.position.Z); //GeoGebra: f(x)=(sin(x+0.575)-0.525)*2

                    if (curJumpT >= maxJumpT) { jump = false; curJumpT = 0f; }
                }
                else if (transform.position.Y > groundY +1f)
                {
                    transform.Move(new Vector3(0f, -5f*GameTime.DeltaTime, 0f));
                }
                else if(transform.position.Y < groundY +1f)
                {
                    transform.position = new Vector3(transform.position.X, groundY +1f, transform.position.Z);
                }

                float speed = 1f;
                if (Input.IsDown(KeyCode.LeftShift))
                {
                    speed = 2f;
                }
                else { speed = 1f; }
                //'w' or 'up' moves cam forward
                if (Input.IsDown(KeyCode.W) || Input.IsDown(KeyCode.UpArrow))
                {
                    transform.Move(Vector3.Cross(new Vector3(0f, 1f, 0f), transform.GetRight()) * (5f * speed) * GameTime.DeltaTime);
                }
                //'s' or 'down' moves cam backward
                if (Input.IsDown(KeyCode.S) || Input.IsDown(KeyCode.DownArrow))
                {
                    transform.Move(Vector3.Cross(new Vector3(0f, 1f, 0f), transform.GetRight()) * (-5f * speed) * GameTime.DeltaTime);
                }
                //'a' or 'left' moves cam left
                if (Input.IsDown(KeyCode.A) || Input.IsDown(KeyCode.LeftArrow))
                {
                    transform.Move(transform.GetRight() * -5f * GameTime.DeltaTime);
                }
                //'d' or 'right' moves cam right
                if (Input.IsDown(KeyCode.D) || Input.IsDown(KeyCode.RightArrow))
                {
                    transform.Move(transform.GetRight() * 5f * GameTime.DeltaTime);
                }
                //'spacebar' triggers jump
                if (Input.IsDown(KeyCode.Space) && !jump)
                {
                    jump = true;
                }
                */
            }
            //move forward/backward/left/right and rotate on local x,y-axis
            if (cameraMode == CameraMode.Fly)
            {
                //'w' or 'up' moves cam forward
                if (Input.IsDown(KeyCode.W)) //|| Input.IsDown(KeyCode.UpArrow)
                {
                    transform.Move(transform.GetForwardDir() * 5f * GameTime.DeltaTime);
                }
                //'s' or 'down' moves cam backward
                if (Input.IsDown(KeyCode.S)) //|| Input.IsDown(KeyCode.DownArrow)
                {
                    transform.Move(transform.GetForwardDir() * -5f * GameTime.DeltaTime);
                }
                //'a' or 'left' moves cam left
                if (Input.IsDown(KeyCode.A)) //|| Input.IsDown(KeyCode.LeftArrow)
                {
                    transform.Move(transform.GetRight() * -5f * GameTime.DeltaTime);
                }
                //'d' or 'right' moves cam right
                if (Input.IsDown(KeyCode.D)) //|| Input.IsDown(KeyCode.RightArrow)
                {
                    transform.Move(transform.GetRight() * 5f * GameTime.DeltaTime);
                }

                //cam-rotation
                //'delete' rotates cam clock-wise around World.Up Axis
                if (Input.IsDown(KeyCode.Delete))
                {
                    transform.Rotate(new Vector3(0f, 5f * GameTime.DeltaTime, 0f));
                }
                //'pagedown' rotates cam counter-clock-wise around World.y aka World.Up Axis
                if (Input.IsDown(KeyCode.PageDown))
                {
                    transform.Rotate(new Vector3(0f, -5f * GameTime.DeltaTime, 0f));
                }
                //'home'('pos1') rotates cam clock-wise around World.x Axis
                if (Input.IsDown(KeyCode.Home))
                {
                    transform.Rotate(new Vector3(5f * GameTime.DeltaTime, 0f, 0f));
                }
                //'end'('ende') rotates cam counter-clock-wise around World.x Axis
                if (Input.IsDown(KeyCode.End))
                {
                    transform.Rotate(new Vector3(5f * GameTime.DeltaTime, 0f, 0f));
                }
            }
            //rotate around local y-axis
            else if (cameraMode == CameraMode.RotateAroundCenter)
            {
                //cam-rotation
                //'delete' rotates cam clock-wise around World.Up Axis
                if (Input.IsDown(KeyCode.Delete) || Input.IsDown(KeyCode.W))
                {
                    transform.Rotate(new Vector3(0f, 5f * GameTime.DeltaTime, 0f));
                }
                //'pagedown' rotates cam counter-clock-wise around World.y aka World.Up Axis
                if (Input.IsDown(KeyCode.PageDown) || Input.IsDown(KeyCode.S))
                {
                    transform.Rotate(new Vector3(0f, -5f * GameTime.DeltaTime, 0f));
                }
            }
            if (cameraMode == CameraMode.RotateAroundCenter || cameraMode == CameraMode.Fly)
            {
                //'e' or 'spacebar' moves cam up
                if (Input.IsDown(KeyCode.E) || Input.IsDown(KeyCode.Space))
                {
                    transform.Move(transform.GetUp() * 5f * GameTime.DeltaTime);
                }
                //'q' or 'leftshift' moves cam down
                if (Input.IsDown(KeyCode.Q) || Input.IsDown(KeyCode.LeftSuper))
                {
                    transform.Move(transform.GetUp() * -5f * GameTime.DeltaTime);
                }
            }

        }

        /// <summary> Rotates the Camera by the Mouses X-, Y-Movement. </summary>
        /// <param name="xpos"> X-Position of the Mouse. </param>
        /// <param name="ypos"> Y-Position of the Mouse. </param>
        public void FollowMouse(float xpos, float ypos)
        {
            //first call to the function
            if (firstMouse)
            {
                lastX = xpos;
                lastY = ypos;
                firstMouse = false;
            }

            float xoffset = xpos - lastX;
            float yoffset = lastY - ypos;
            lastX = xpos;
            lastY = ypos;

            xoffset *= sensitivity * GameTime.DeltaTime * 0.01f * sensitivity;
            yoffset *= sensitivity * GameTime.DeltaTime * 0.01f * sensitivity;

            yaw += xoffset;
            pitch += yoffset;

            if (pitch > 0.025f) { pitch = 0.025f; } //89.0f didn't work
            if (pitch < -0.025f) { pitch = -0.025f; } //89.0f didn't work
            
            transform.RotateEuler(new Vector3(yaw, pitch, 0f));
        }
    }
}
