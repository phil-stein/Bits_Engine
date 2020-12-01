using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Text;
using BitsCore.Utils;
using BitsCore.ObjectData.Components;
using GLFW.Game;
using BitsCore.ObjectData;
using BitsCore.Rendering;

namespace BitsCore.ObjectData.Components
{
    /// <summary> The Transform attached to each GameObject, contains Position, Rotation and Scale. </summary>
    [System.Serializable]
    public class Transform : Component
    {
        #region VARIABLES
        /// <summary> The position in 3D-Space, represented by X,Y,Z Coordinates. </summary>
        public Vector3 position { get; set; }
        /// <summary> The rotation in 3D-Space, represented by X,Y,Z Coordinates. </summary>
        public Vector3 rotation { get; set; }
        /// <summary> The scale in 3D-Space, represented by X,Y,Z Coordinates. </summary>
        public Vector3 scale { get; set; }

        public Transform parent { get; private set; }

        Vector3 forward;
        #endregion

        #region CONSTRUCTOR_AND_DERIVED
        /// <summary> Generates a Transform. </summary>
        /// <param name="_position"> The Position of the GameObject. </param>
        /// <param name="_rotation"> The Rotation of the GameObject. </param>
        /// <param name="_scale"> The Scale of the GameObject. </param>
        public Transform(Vector3 _position, Vector3 _rotation, Vector3 _scale)
        {
            this.position = _position;
            this.rotation = _rotation;
            this.scale = _scale;

            this.forward = Vector3.Normalize(Vector3.Zero - this.position); //reverse is intentional
        }
        /// <summary> 
        /// Generates a Transform. 
        /// <para> position = (0, 0, 0), rotation (0, 0, 0), scale = (1, 1, 1) </para>
        /// </summary>
        public Transform()
        {
            this.position = Vector3.Zero;
            this.rotation = Vector3.Zero;
            this.scale = Vector3.One;

            this.forward = Vector3.Normalize(Vector3.Zero - this.position); //reverse is intentional
        }

        internal override void OnAdd()
        {
        }

        internal override void OnRemove()
        {
        }
        #endregion

        #region MODEL_MATRIX
        /// <summary> The Model-Matrix of the Transform. Used for Object-Space to World-Space transformation. </summary>
        /// <param name="_position"> The Position of the GameObject. </param>
        public Matrix4x4 GetModelMatrix()
        {
            Vector3 objPos;
            Vector3 objRot;
            Vector3 objScale;
            Matrix4x4 parentModel = Matrix4x4.Identity;
            if(parent != null) 
            { 
                parentModel = parent.GetModelMatrix();
                objPos = position;
                objRot = rotation;
                objScale = scale;
            }
            else
            {
                objPos = GetGlobalPos();
                objRot = GetGlobalRot();
                objScale = GetGlobalScale();
            }

            Matrix4x4 pos = Matrix4x4.Identity;
            pos *= Matrix4x4.CreateTranslation(objPos);

            Matrix4x4 rotX = Matrix4x4.CreateRotationX(objRot.X * (MathF.PI / 180));
            Matrix4x4 rotY = Matrix4x4.CreateRotationY(objRot.Y * (MathF.PI / 180));
            Matrix4x4 rotZ = Matrix4x4.CreateRotationZ(objRot.Z * (MathF.PI / 180));
            Matrix4x4 rot = rotX * rotY * rotZ;

            Matrix4x4 sca = Matrix4x4.CreateScale(objScale.X, objScale.Y, objScale.Z);

            return parentModel * (sca * rot * pos);
        }
        #endregion

        #region MOVE_ROTATE_SCALE
        /// <summary> Moves the Transforms position by a Vector. </summary>
        /// <param name="deltaPos"> The direction the Transform is supposed to be Translated by. </param>
        /// <param name="worldSpace"> Determines whether to Translate in World-Space or Local-Space. </param>
        public void Move(Vector3 deltaPos, bool worldSpace = false)
        {
            if (!worldSpace)
            {
                //doesn't work
                Vector3 forward = Vector3.Normalize(position - Renderer.mainCam.transform.position);
                Vector3 right = Vector3.Normalize(Vector3.Cross(new Vector3(0f, 1f, 0f), forward));
                Vector3 up = Vector3.Normalize(Vector3.Cross(forward, right));

                Vector3 newDeltaPos = deltaPos.X * right + deltaPos.Y * up + deltaPos.Z * forward;
                //position += newDeltaPos;
                position += deltaPos;
            }
            else 
            {
                position += deltaPos;
            }
        }

        /// <summary> Rotates the Transform by a Vector. </summary>
        /// <param name="deltaRot"> The direction the Rotated is supposed to be Rotated by. </param>
        /// <param name="worldSpace"> Determines whether to Rotate in World-Space or Local-Space. </param>
        public void Rotate(Vector3 deltaRot, bool worldSpace = false)
        {
            if (!worldSpace)
            {
                //if(rotation.X + deltaRot.X >= 360f) { rotation.X = (rotation.X + deltaRot.X) % 360f; }
                //else { rotation.X += deltaRot.X; }
                //if(rotation.Y + deltaRot.Y >= 360f) { rotation.Y = (rotation.Y + deltaRot.Y) % 360f; }
                //else { rotation.Y += deltaRot.Y; }
                //if (rotation.Z + deltaRot.Z >= 360f) { rotation.Z = (rotation.Z + deltaRot.Z) % 360f; }
                //else { rotation.Z += deltaRot.Z; }

                rotation += deltaRot;
                //rotation.X = rotation.X >= 360f ? 0f : rotation.X <= -360f ? 0f : rotation.X;
                //rotation.Y = rotation.Y >= 360f ? 0f : rotation.Y <= -360f ? 0f : rotation.Y;
                //rotation.Z = rotation.Z >= 360f ? 0f : rotation.Z <= -360f ? 0f : rotation.Z;
                return;
            }
            else 
            {
                Vector4 newDelta = Vector4.Transform(deltaRot, GetModelMatrix());
                rotation += new Vector3(newDelta.X, newDelta.Y, newDelta.Z);
            }
        }

        public void RotateAround(Vector3 axis, float angle)
        {
            Matrix4x4 model = GetModelMatrix();
            Matrix4x4 rot = Matrix4x4.CreateFromAxisAngle(axis, MathUtils.DegreeToRadians(angle));

            rot = model * rot;

            rotation = new Vector3();
        }

        /// <summary> Scales the Transform by a Vector. </summary>
        /// <param name="deltaScale"> The Scale the Transform is supposed to be Translated by. </param>
        /// <param name="worldSpace"> Determines whether to Scale in World-Space or Local-Space. </param>
        public void Scale(Vector3 deltaScale)
        {
            scale += deltaScale;
        }

        /// <summary> Multiplies the scale by a float. </summary>
        /// <param name="multiplier"> The float the scale gets multiplied by. </param>
        /// <param name="worldSpace"> Determines whether to Scale in World-Space or Local-Space. </param>
        public void Scale(float multiplier)
        {
            scale *= multiplier;
        }

        /// <summary> Rotates the transform by a vector, using Euler-Angles, values given in degree. </summary>
        /// <param name="eulers"> The direction the transform is supposed to be rotated by, given in degree. </param>
        public void RotateEuler(Vector3 eulers)
        {
            Vector3 direction;
            direction.X = MathF.Cos(MathUtils.RadiansToDegree(eulers.X)) * MathF.Cos(MathUtils.RadiansToDegree(eulers.Y));
            direction.Y = MathF.Sin(MathUtils.RadiansToDegree(eulers.Y));
            direction.Z = MathF.Sin(MathUtils.RadiansToDegree(eulers.X)) * MathF.Cos(MathUtils.RadiansToDegree(eulers.Y));
            forward = Vector3.Normalize(direction);
            //rotation = direction;
        }
        #endregion

        #region GET_METHODS
        /// <summary> Get the position of the GameObject independent of the parent-transform. </summary>
        public Vector3 GetGlobalPos()
        {
            if(parent != null)
            {
                return position + parent.GetGlobalPos();
            }
            else { return position; }
        }

        /// <summary> Get the rotation of the GameObject independent of the parent-transform. </summary>
        public Vector3 GetGlobalRot()
        {
            if(parent != null)
            {
                return parent.GetGlobalRot() * rotation;
            }
            else { return rotation; }
        }

        /// <summary> Get the scale of the GameObject independent of the parent-transform. </summary>
        public Vector3 GetGlobalScale()
        {
            if(parent != null)
            {
                return scale * parent.GetGlobalScale();
            }
            else { return scale; }
        }

        /// <summary> The normalized forward direction of the transform. </summary>
        public Vector3 GetForwardDir()
        {
            //should "update" the rotation here
            return Vector3.Normalize(forward);
        }

        /// <summary> The normalized right direction of the transform. </summary>
        public Vector3 GetRight()
        {
            return -Vector3.Normalize(Vector3.Cross(new Vector3(0f, 1f, 0f), GetForwardDir()));
        }

        /// <summary> The normalized up direction of the transform </summary>
        public Vector3 GetUp()
        {
            Vector3 localRight = Vector3.Normalize(Vector3.Cross(new Vector3(0f, 1f, 0f), GetForwardDir()));
            return Vector3.Normalize(Vector3.Cross(GetForwardDir(), localRight));
        }
        #endregion

        #region SET_METHODS
        /// <summary> Set the 'gameObject' of the transform. </summary>
        /// <param name="obj"> The GameObject to be the new 'gameObject'. </param>
        public void SetGameObj(GameObject obj)
        {
            gameObject = obj;
        }
        #endregion

        #region PARENT
        /// <summary> Sets a new parent. </summary>
        /// <param name="newParent"> The new parent-transform. </param>
        public void SetParent(Transform newParent)
        {
            if(newParent.gameObject == null) { System.Diagnostics.Debug.WriteLine("!!! Can't set parent of Transfrom, 'newParent' does not have a 'gameObject' !!!"); return; }
            
            position = GetGlobalPos() - newParent.GetGlobalPos(); //otherwise the child can get moved by becoming a child
            rotation = GetGlobalRot() * newParent.GetGlobalRot(); //otherwise the child can get rotated by becoming a child
            scale = GetGlobalScale() * newParent.GetGlobalScale(); //otherwise the child can get scaled by becoming a child
            
            parent = newParent;
        }
        /// <summary> Clear the parent. Doesn't account for the changed tranform yet. </summary>
        public void ClearParent()
        {
            parent = null;
        }
        #endregion
    }
}
