using BitsCore.Rendering.Display;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace BitsCore.Rendering.Cameras
{
    public class Camera2D
    {

        public Vector2 FocusPosition { get; set; }
        public float Zoom { get; set; }
        public Camera2D(Vector2 focusPosition, float zoom)
        {
            FocusPosition = focusPosition;
            Zoom = zoom;
        }

        public Matrix4x4 GetProjectionMatrix()
        {
            float left = FocusPosition.X - DisplayManager.WindowSize.X / 2f;
            float right = FocusPosition.X + DisplayManager.WindowSize.X / 2f;
            float top = FocusPosition.Y - DisplayManager.WindowSize.Y / 2f;
            float bottom = FocusPosition.Y + DisplayManager.WindowSize.Y / 2f;

            //last to args are the near-field-camera-clipping and the distance-culling for our camera (doen't matter because it's 2D)
            Matrix4x4 orthoMatrix = Matrix4x4.CreateOrthographicOffCenter(left, right, bottom, top, 0.01f, 100f);
            Matrix4x4 zoomMatrix = Matrix4x4.CreateScale(Zoom);

            return orthoMatrix * zoomMatrix;
        }

    }
}
