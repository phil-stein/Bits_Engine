using BitsCore.ObjectData.Components;
using BitsCore.ObjectData.Materials;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;

namespace BitsCore.ObjectData.Components
{
    /// <summary> 
    /// Creates and updates quads between the LineRenderers positions. 
    /// <para> Creates/replaces a Mesh-Component when added to a GameObject and removes it when it gets removed from its GameObject. </para>
    /// </summary>
    [System.Serializable]
    public class LineRenderer : Component
    {
        /// <summary> The positions between which the line gets created. </summary>
        public List<Vector3> positions { get; private set; }

        /// <summary> When true the line creates a connection between the first and last position. </summary>
        public bool loopAround { get; private set; }

        /// <summary> The width of the line. </summary>
        public float width { get; private set; }

        /// <summary> The Material of the line. </summary>
        public Material material { get; private set; }

        /// <summary> 
        /// Create a LineRenderer.
        /// <para> Connects the given points with quads. </para>
        /// </summary>
        /// <param name="_positions"> The given positions. </param>
        /// <param name="_width"> The width of the drawn line. </param>
        /// <param name="_material"> The Material of the drawn line. </param>
        /// <param name="_loopAround"> When true the line creates a connection between the first and last position. </param>
        public LineRenderer(List<Vector3> _positions, float _width, Material _material, bool _loopAround = false)
        {
            this.positions = new List<Vector3>();
            this.loopAround = _loopAround;
            this.width = _width * .5f;
            this.material = _material;

            SetPositions(_positions);
        }
        /// <summary> 
        /// Create a LineRenderer.
        /// <para> Connects the given points with quads. </para>
        /// </summary>
        /// <param name="_positions"> The given positions. </param>
        /// <param name="_width"> The width of the drawn line. </param>
        /// <param name="_material"> The Material of the drawn line. </param>
        /// <param name="_loopAround"> When true the line creates a connection between the first and last position. </param>
        public LineRenderer(Vector3[] _positions, float _width, Material _material, bool _loopAround = false)
        {
            this.positions = new List<Vector3>();
            this.loopAround = _loopAround;
            this.width = _width * .5f;
            this.material = _material;

            SetPositions(_positions);
        }
        /// <summary> 
        /// Create a LineRenderer.
        /// <para> Connects the given points with quads. </para>
        /// </summary>
        /// <param name="posOne"> The first position in positions. </param>
        /// /// <param name="posTwo"> The second position in positions. </param>
        /// <param name="_width"> The width of the drawn line. </param>
        /// <param name="_material"> The Material of the drawn line. </param>
        /// <param name="_loopAround"> When true the line creates a connection between the first and last position. </param>
        public LineRenderer(Vector3 posOne, Vector3 posTwo, float _width, Material _material, bool _loopAround = false)
        {
            this.positions = new List<Vector3>();
            this.loopAround = _loopAround;
            this.positions.Add(posOne);
            this.positions.Add(posTwo);
            this.width = _width * .5f;
            this.material = _material;
        }



        internal override void OnAdd()
        {
            Init();
        }

        internal override void OnRemove()
        {
            //as this is the Mesh-Component being altered to make the line, it isn't needed anymore if the LineRenderer doesn't need it anymore
            gameObject.RemoveComp<Mesh>();
        }

        //Set--------------------------------------------------
        /// <summary> Replace the LineRenderers positions with the given list of Vector3s. </summary>
        /// <param name="newPositions"> The Vector3s that will be the new posisitions of the LineRenderer. </param>
        public void SetPositions(List<Vector3> newPositions)
        {
            positions = newPositions;

            if(gameObject == null) { return; } //can't calc mesh before having a gameObject with mesh-component
            UpdateMesh();
        }
        /// <summary> Replace the LineRenderers positions with the given array of Vector3s. </summary>
        /// <param name="newPositions"> The Vector3s that will be the new posisitions of the LineRenderer. </param>
        public void SetPositions(Vector3[] newPositions)
        {
            List<Vector3> newPos = new List<Vector3>();
            newPos.AddRange(newPositions);
            positions = newPos;

            if (gameObject == null) { return; } //can't calc mesh before having a gameObject with mesh-component
            UpdateMesh();
        }
        /// <summary> Replace the position at the given index, with the given position. </summary>
        /// <param name="position"> The Vector3 of the position to be set. </param>
        /// <param name="index"> The index of the position to be set. </param>
        public void SetPosition(Vector3 position, int index)
        {
            positions[index] = position;

            if (gameObject == null) { return; } //can't calc mesh before having a gameObject with mesh-component
            UpdateMesh();
        }
        /// <summary> 
        /// Insert the given position at the given index, without replacing the previous position at that index. 
        /// <para> The position that was previously at the given index and all following positions get shifted one to the right. </para>
        /// </summary>
        /// <param name="position"> The Vector3 of the position to be set. </param>
        /// <param name="index"> The index of the position to be set. </param>
        public void InserPosition(Vector3 position, int index)
        {
            positions.Insert(index, position);

            if (gameObject == null) { return; } //can't calc mesh before having a gameObject with mesh-component
            UpdateMesh();
        }
        /// <summary> Sets the Material of the LineRenderer Mesh.
        /// <para> Same as setting the Material directly on the GameObject's Mesh-Component, which got added by the LineRenderer. </para>
        /// </summary>
        /// <param name="mat">  </param>
        public void SetMaterial(Material mat)
        {
            if (gameObject == null) { material = mat; return; }
            gameObject.GetComp<Model>().SetMaterial(mat);
            material = mat;
        }
        /// <summary> Set the width of the line. </summary>
        /// <param name="_width"> The new width of the line. </param>
        public void SetWidth(float _width)
        {
            width = _width * .5f;

            if (gameObject == null) { return; } //can't calc mesh before having a gameObject with mesh-component
            UpdateMesh();
        }

        private void Init()
        {
            if (gameObject.HasComp<Mesh>()) { gameObject.RemoveComp<Mesh>(); } //could maybe add ReplaceComp(comp c) and ReplaceOrAddComp() to GameObject

            CalcMesh(out float[] verts, out uint[] tris);

            Mesh mesh = new Mesh(verts, tris);
            Model model = new Model(mesh, material, "LineRenderer_Mesh");
            //mesh.CalcNormalsSmooth();

            gameObject.AddComp(model);
        }
        private void UpdateMesh()
        {
            //calc the new values for the mesh, this is done after the positions have been altered
            CalcMesh(out float[] verts, out uint[] tris);

            gameObject.GetComp<Mesh>().SetVertices(verts);
            gameObject.GetComp<Mesh>().SetTriangleIndices(tris);
            //gameObject.GetComp<Mesh>().CalcNormalsSmooth();
        }

        private void CalcMesh(out float[] verts, out uint[] tris)
        {
            List<float> vertsLst = new List<float>();
            List<uint> trisLst = new List<uint>();

            int vertCount = 0;
            for (int i = 0; i < positions.Count; i++)
            {
                Vector3 dir = Vector3.Zero;
                if (i + 1 >= positions.Count)
                {
                    dir = Vector3.Normalize(positions[i - 1] - positions[i]); //normalized dir-vector pointing from position[0] to position[1]
                }
                else
                {
                    dir = Vector3.Normalize(positions[i] - positions[i + 1]); //normalized dir-vector pointing from position[0] to position[1]

                    //if there has been a position before the current position the direction of the current position is an average between the vector towards the next and previous positions
                    if (i > 0)
                    {
                        dir = (dir + Vector3.Normalize(positions[i - 1] - positions[i])) * .5f;
                    }
                }
                Vector3 right = Vector3.Normalize(Vector3.Cross(Vector3.UnitY, dir)); //normalized right-vector pointing to the right of dir
                //Vector3 up = Vector3.Normalize(Vector3.Cross(dir, right)); //normalized up-vector pointing up (perpendicular) from the plain between dir and right

                float dirLen = Vector3.Distance(Vector3.Zero, dir);
                float rightLen = Vector3.Distance(Vector3.Zero, right);
                //Debug.WriteLine("\n  |LineRenderer dir: " + dir.ToString() + ", right: " + right.ToString() + ", dir-len: " + dirLen.ToString() + ", right-len: " + rightLen.ToString());

                Vector3 vertOne = right * width + positions[i];
                Vector3 vertTwo = -right * width + positions[i];

                //Debug.WriteLine("  |LineRenderer vert-one: " + vertOne.ToString() + ", vert-two: " + vertTwo.ToString());

                //add the prev. calced vertices
                vertsLst.AddRange(new float[]
                {
                    //pos.x    pos.y      pos.z|normal.x .y .z |  u       v-coords
                    vertOne.X, vertOne.Y, vertOne.Z, 0f, 1f, 0f, 0f, vertCount*0.5f, //vert-data for the first vertex
                    vertTwo.X, vertTwo.Y, vertTwo.Z, 0f, 1f, 0f, 1f, vertCount*0.5f, //vert-data for the second vertex
                });
                vertCount += 2;

                //every time a new st of verts get added except for the first one as those do not have a second pair pf verts to make a quad(two tris)
                if (i != 0)
                {
                    trisLst.AddRange(new uint[]
                    {
                        //-1 because the array starts at 0, and the vertCount counts the actual amount of verts
                        (uint)vertCount-1, (uint)vertCount-2, (uint)vertCount-4, //first tri
                        (uint)vertCount-1, (uint)vertCount-4, (uint)vertCount-3, //second tri
                        //(uint)i, (uint)i+1, (uint)i+2, //first tri
                        //(uint)i, (uint)i+2, (uint)i-1, //second tri
                    });

                    //Debug.WriteLine("\n  |LineRenderer added tris, vert-len: " + vertsLst.Count.ToString() + ", vert-count: " + vertCount.ToString() + ", tris-len: " + trisLst.Count.ToString() + ", i: " + i.ToString());
                    //Debug.Write(" ");
                    int counter = 0;
                    foreach (uint tri in trisLst)
                    {
                        if (counter == 3) { /*Debug.Write("\n ");*/ counter = 0; }
                        //Debug.Write(" | " + tri.ToString());
                        counter++;
                    }
                    //Debug.Write("\n \n");
                }
            }

            verts = vertsLst.ToArray();
            tris = trisLst.ToArray();
        }
        private void CalcMeshLoopAroundDoesntWork(out float[] verts, out uint[] tris)
        {
            List<float> vertsLst = new List<float>();
            List<uint> trisLst = new List<uint>();

            int vertCount = 0;
            for (int i = 0; i < positions.Count + (loopAround ? 1 : 0); i++)
            {
                Vector3 dir = Vector3.Zero;

                //if the current position is the last ne
                if (i +1 >= positions.Count && !loopAround) 
                {
                    dir = Vector3.Normalize(positions[i -1] - positions[i]); //normalized dir-vector pointing from position[0] to position[1]
                }
                else
                {
                    if (i + 1 >= positions.Count && i > 0)
                    {
                        //takes average between previous and first position
                        dir = (Vector3.Normalize(positions[i - 2] - positions[i -1]) + Vector3.Normalize(positions[0] - positions[i -1])) * .5f;
                    }
                    else
                    {
                        dir = Vector3.Normalize(positions[i] - positions[i + 1]); //normalized dir-vector pointing from position[0] to position[1]

                        //if there has been a position before the current position the direction of the current position is an average between the vector towards the next and previous positions
                        if (i > 0)
                        {
                            dir = (dir + Vector3.Normalize(positions[i - 1] - positions[i])) * .5f;
                        }
                    }
                }
                //so that on the extra loop-cycle with loopAround it doesn't create any extra verts
                if (i + 1 < positions.Count || !loopAround)
                {
                    Vector3 right = Vector3.Normalize(Vector3.Cross(Vector3.UnitY, dir)); //normalized right-vector pointing to the right of dir
                                                                                          //Vector3 up = Vector3.Normalize(Vector3.Cross(dir, right)); //normalized up-vector pointing up (perpendicular) from the plain between dir and right

                    float dirLen = Vector3.Distance(Vector3.Zero, dir);
                    float rightLen = Vector3.Distance(Vector3.Zero, right);
                    //Debug.WriteLine("\n  |LineRenderer dir: " + dir.ToString() + ", right: " + right.ToString() + ", dir-len: " + dirLen.ToString() + ", right-len: " + rightLen.ToString());

                    Vector3 vertOne = right * width + positions[i];
                    Vector3 vertTwo = -right * width + positions[i];

                    //Debug.WriteLine("  |LineRenderer vert-one: " + vertOne.ToString() + ", vert-two: " + vertTwo.ToString());

                    //calc uv's with pos.index in positions (i in for-loop) %2 if uneven put top/bottom if even put on the other side
                    //...

                    //add the prev. calced vertices
                    vertsLst.AddRange(new float[]
                    {
                    //pos.x     pos.y       pos.z normal.x .y .z  u__v-coords
                    vertOne.X, vertOne.Y, vertOne.Z, 0f, 0f, 0f, 0f, 0f, //vert-data for the first vertex
                    vertTwo.X, vertTwo.Y, vertTwo.Z, 0f, 0f, 0f, 0f, 0f, //vert-data for the second vertex
                    });
                    vertCount += 2;

                    //every time a new st of verts get added except for the first one as those do not have a second pair pf verts to make a quad(two tris)
                    if (i != 0)
                    {
                        trisLst.AddRange(new uint[]
                        {
                        //-1 because the array starts at 0, and the vertCount counts the actual amount of verts
                        (uint)vertCount-1, (uint)vertCount-2, (uint)vertCount-4, //first tri
                        (uint)vertCount-1, (uint)vertCount-4, (uint)vertCount-3, //second tri
                        });

                        //Debug.WriteLine("\n  |LineRenderer added tris, vert-len: " + vertsLst.Count.ToString() + ", vert-count: " + vertCount.ToString() + ", tris-len: " + trisLst.Count.ToString() + ", i: " + i.ToString());
                        //Debug.Write(" ");
                        int counter = 0;
                        foreach (uint tri in trisLst)
                        {
                            if (counter == 3) { /*Debug.Write("\n ");*/ counter = 0; }
                            System.Diagnostics.Debug.Write(" | " + tri.ToString());
                            counter++;
                        }
                        //Debug.Write("\n \n");
                    }
                }
                else
                {
                    trisLst.AddRange(new uint[] 
                    {
                        (uint)vertCount-1, 0, (uint)vertCount-2,
                        0, 1, (uint)vertCount-1
                    });
                }
            }

            verts = vertsLst.ToArray();
            tris = trisLst.ToArray();
        }
    }
}
