using BitsCore.Rendering;
using BitsCore.ObjectData.Components;
using BitsCore.ObjectData.Materials;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Text;
using BitsCore.Events;

namespace BitsCore.ObjectData.Components
{
    [System.Serializable]
    public class Billboard : Component
    {
        /// <summary> The size of the billboard. </summary>
        public Vector2 size { get; private set; }

        /// <summary>
        /// Generates a Billboard-Component, that always rotates the Plane towards the Camera.
        /// <para> Only add this to a GameObject.Primitives.Plane, or custom quad with the same vertices-data structure. </para>
        /// <para> Not definig size will set it to zero, which will set the size to the 'gameObjects.transform.scale' x-coord and y-coord. </para>
        /// </summary>
        public Billboard()
        {
            //subscribe the function to the update-event
            EventManager.calledUpdate += alignToCamera;
            this.size = Vector2.Zero;
        }

        /// <summary>
        /// Generates a Billboard-Component, that always rotates the Plane towards the Camera.
        /// <para> Only add this to a GameObject.Primitives.Plane, or custom quad with the same vertices-data structure. </para>
        /// <para> Not definig size will set it to zero, which will set the size to the 'gameObjects.transform.scale' x-coord and y-coord. </para>
        /// </summary>
        /// <param name="_size"> The size of the billboard. </param>
        public Billboard(Vector2 _size)
        {
            //subscribe the function to the update-event
            EventManager.calledUpdate += alignToCamera;
            this.size = _size *.5f; //times .5f because the size if calculated for a 2*2 quad
        }

        /// <summary>
        /// Generates a Billboard-Component, that always rotates the Plane towards the Camera.
        /// <para> Only add this to a GameObject.Primitives.Plane, or custom quad with the same vertices-data structure. </para>
        /// <para> Not definig size will set it to zero, which will set the size to the 'gameObjects.transform.scale' x-coord and y-coord. </para>
        /// </summary>
        /// <param name="xSize"> The size of the billboard in x-direction. </param>
        /// <param name="ySize"> The size of the billboard in y-direction. </param>
        public Billboard(float xSize, float ySize)
        {
            //subscribe the function to the update-event
            EventManager.calledUpdate += alignToCamera;
            this.size = new Vector2(xSize *.5f, ySize *.5f); //times .5f because the size if calculated for a 2*2 quad
        }

        internal override void OnAdd()
        {
            //on of the constructors sets the size to zero which means it should be set to the gameObjects scale, which is only accesible after being added
            if (size == Vector2.Zero || size == null)
            {
                size = new Vector2(gameObject.transform.scale.X * .5f, gameObject.transform.scale.Y * .5f); //times .5f because the size if calculated for a 2*2 quad
            }

            if (!gameObject.HasComp<Model>())
            {
                gameObject.AddComp(new Model(new Mesh(new float[0], new uint[0]), AssetManager.GetMaterial("Mat_Default"), "Billboard_Mesh"));
            }
        }

        internal override void OnRemove()
        {
        }

        private void alignToCamera()
        {
            //Vector3 rot = Program.game.mainCam.transform.position - gameObject.transform.position;
            //gameObject.transform.rotation = rot;

            //Tutorial: https://www.youtube.com/watch?v=puOTwCrEm7Q

            //calc the 'local coordinate-system' of the quad, basically a parralel plane to the camera
            if (!gameObject.HasComp<Mesh>()) { System.Diagnostics.Debug.WriteLine("!!! Billboards 'gameObject' doesn't also have a Mesh-Component."); return; }
            //Debug.WriteLine("Billboards gameObject position: " + gameObject.transform.position.ToString());

            Vector3 forward = Vector3.Normalize(gameObject.transform.position - Renderer.mainCam.transform.position);
            Vector3 right = Vector3.Normalize(Vector3.Cross(new Vector3(0f, 1f, 0f), forward)) * size.X;
            Vector3 up = Vector3.Normalize(Vector3.Cross(forward, right)) * size.Y;

            //Debug.WriteLine("Billboards forward: " + forward.ToString());

            //set the meshes verts based on the new 'local coordinate-system'
            float[] verts = gameObject.GetComp<Mesh>().vertices;

            //first vertex, top-left
            Vector3 pos = up + right;
            verts[0 + 0] = pos.X; //x-coord of the vertex
            verts[0 + 1] = pos.Y; //y-coord of the vertex
            verts[0 + 2] = pos.Z; //z-coord of the vertex

            //second vertex, bottom-left
            pos = right - up;
            verts[8 + 0] = pos.X; //x-coord of the vertex
            verts[8 + 1] = pos.Y; //y-coord of the vertex
            verts[8 + 2] = pos.Z; //z-coord of the vertex

            //third vertex, bottom-right
            pos = -up - right;
            verts[16 + 0] = pos.X; //x-coord of the vertex
            verts[16 + 1] = pos.Y; //y-coord of the vertex
            verts[16 + 2] = pos.Z; //z-coord of the vertex

            //forth vertex, top-right
            pos = up - right;
            verts[24 + 0] = pos.X; //x-coord of the vertex
            verts[24 + 1] = pos.Y; //y-coord of the vertex
            verts[24 + 2] = pos.Z; //z-coord of the vertex

            gameObject.GetComp<Mesh>().SetVertices(verts);
        }

        //billboard matrix, doesn't work 
        /*
        public Matrix4x4 GetBillboardModelMatrix()
        {
            if (gameObject.HasComp<Mesh>()) { return Matrix4x4.Identity; }
            //https://stackoverflow.com/questions/5467007/inverting-rotation-in-3d-to-make-an-object-always-face-the-camera
            Matrix4x4 model = gameObject.transform.GetModelMatrix(true);
            
            float d = MathF.Sqrt(MathF.Pow(model.M11, 2) + MathF.Pow(model.M22, 2) + MathF.Pow(model.M33, 2));
            float transX = model.M14; float transY = model.M24; float transZ = model.M34;
            
            model = Matrix4x4.Identity;
            model.M11 = d; model.M22 = d; model.M33 = d;
            model.M14 = transX; model.M24 = transY; model.M34 = transZ; model.M44 = 1f;
            return gameObject.transform.GetModelMatrix(true); //model;
        }
        */

    }
}
