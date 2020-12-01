using static BitsCore.OpenGL.GL;
using System;
using System.Diagnostics;
using System.Numerics;

namespace BitsCore.ObjectData.Components
{
    /// <summary> Mesh-Component containing info about vertices and triangles. </summary>
    public class Mesh
    {
        #region VARIABLES
        /// <summary> 
        /// Array of floats describing the position, normals and uv-coordinats of multiple vertices.
        /// <para> Positions in "Normalized Device Coordinates" (NDC) between -1.0 and 1.0. </para>
        /// <para> Structure: xCoord, yCoord, zCoord, xNormal, yNormal, zNormal, uCoord, vCoord </para>
        /// </summary>
        public float[] vertices { get; private set; }
        /// <summary> The indices of the vertices that form a triangles. </summary>
        public uint[] triangles { get; private set; }

        /// <summary> 
        /// The Vertex Array Object of the Mesh 
        /// <para> Used to pass the vertex data the VBO. </para>
        /// </summary>
        public uint vao { get; private set; } //unsigned-int storing the pointer to the Vertex-Array-Object
        /// <summary> 
        /// The Vertex Buffer Object of the Mesh 
        /// <para> Used to store the vertex-data. </para>
        /// </summary>
        public uint vbo { get; private set; } //unsigned-int storing the pointer to the Vertex-Buffer-Object
        /// <summary> 
        /// The Element Buffer Object of the Mesh 
        /// <para> Used to store the triangle indices of the mesh. </para>
        /// </summary>
        public uint ebo { get; private set; } //unsigned-int storing the pointer to the Element-Buffer-Object

        /// <summary> 
        /// Indicates whether the Mesh should be drawn using traiangle-indices, saved in 'triangles'.
        /// <para> Or assume the vertices in the 'vertices' array are in such an order that each consecutive three vertex from a triangle. </para>
        /// </summary>
        public bool notIndexed { get; set; }
        #endregion

        /// <summary> 
        /// Generates a Mesh-Component. 
        ///<para> Used to create/show 3D-Models. </para>
        /// </summary>
        /// <param name="_gameObject"> The GameObject the Mesh-Component will be attached to. </param>
        /// <param name="_vertices"> The Vertices of the Mesh. </param>
        /// <param name="_triangles"> The Triangles of the Mesh. </param>
        /// <param name="_material"> The Material of the Mesh. </param>
        /// <param name="_notIndexed"> Dictates the GenOpenGLData() function. </param>
        public Mesh(float[] _vertices, uint[] _triangles, bool _notIndexed = false)
        {
            this.vertices = _vertices;
            this.triangles = _triangles;

            this.notIndexed = _notIndexed;
        }


        #region METHODS
        //set&get-----------------
        /// <summary> Sets the vertices of the Mesh-Component to the passed float-array. </summary>
        /// <param name="verts"> Float-array with the new vertices-data. Values between -1.0 and 1.0. </param>
        public void SetVertices(float[] verts)
        {
            vertices = verts;

            CalcNormalsSmooth(); //used to be smooth but thats very slow

            GenOpenGLData();
        }
        /// <summary> Sets the triangle-indices of the Mesh-Component to the passed uint-array. </summary>
        /// <param name="verts"> Uint-array with the new triangle-indices. </param>
        public void SetTriangleIndices(uint[] tris)
        {
            triangles = tris;
            GenOpenGLData();
            CalcNormalsSmooth();
        }

        #endregion

        #region NORMALS
        //normals----------------
        /// <summary> Calculates the Normals based on the Average of the Vertexes surrounding Triangles Flat-Normals. </summary>
        public void CalcNormalsSmooth()
        {
            //2d array holding [the index of a vert, indices of all tris connected to that vert]
            //8 is the max amount of tris a vert can be connected to, ste to 9 because the 0th row counts as one row
            //Debug.WriteLine("\n" + "Vert.Len/8: " + ((int)(vertices.Length / 8)).ToString());
            int[,] trisOfVert = new int[(int)(vertices.Length / 8), 9];
            //Debug.WriteLine("TrisOfVert.Len: " + trisOfVert.Length.ToString());

            for (int i = 0; i < (vertices.Length / 8); i++)
            {
                trisOfVert[i, 0] = i;
            }

            //Print2DArrayInt(trisOfVert);

            float forLoopOne = 0.0f;
            float forLoopTwo = 0.0f;
            float forLoopThree = 0.0f;
            Stopwatch stopwatchOne = new Stopwatch();
            Stopwatch stopwatchTwo = new Stopwatch();
            Stopwatch stopwatchThree = new Stopwatch();
            for (int i = 0; i < triangles.Length - 1; i += 3)
            {
                stopwatchOne.Restart();
                for (int t = 0; t < 3; t++)
                {
                    stopwatchTwo.Restart();
                    //get the x-position of the vertex
                    int vertID = 0;
                    for (int n = 0; n < (int)(vertices.Length / 8); n++)
                    {
                        if (trisOfVert[n, 0] == triangles[i + t]) { vertID = n; break; }
                    }
                    stopwatchTwo.Stop();
                    forLoopTwo += stopwatchTwo.ElapsedMilliseconds;

                    stopwatchThree.Restart();
                    //use the first y-position under that x-position that is == 0, or skip if val already exists 
                    for (int n = 1; n <= 8; n++)
                    {
                        if (trisOfVert[vertID, n] == i + 1) { break; }
                        else if (trisOfVert[vertID, n] == 0) { trisOfVert[vertID, n] = i + 1; break; } //plus one to never have a 0 as an assigned index !!!need to subtract this when using it!!!
                    }
                    stopwatchThree.Stop();
                    forLoopThree += stopwatchThree.ElapsedMilliseconds;
                }
                stopwatchOne.Stop();
                forLoopOne += stopwatchOne.ElapsedMilliseconds;
            }
            
            //Print2DArrayInt(trisOfVert);

            int vertAmountAssigned = 0;
            //calc normal-vec for all adjacent tris of vert and combine them
            for (int i = 0; i < vertices.Length - 1; i += 8)
            {
                //the normals for each of the tris in 'trisOfVert[,]'
                Vector3[] normals = new Vector3[8];
                int addedNormals = 0; //the amount of normals added in case there were less than 8
                for (int n = 1; n <= 8; n++)
                {
                    //!!!subtract one from the index, as we added one before!!!
                    int id = trisOfVert[(i / 8), n] - 1;

                    //if the id is zero the index was not assigned, because there were less than 8 adjacent tris 
                    if (id < 0) { continue; }

                    //Debug.WriteLine("i: " + i.ToString() + ", (i/8) = " + (i / 8).ToString() + ", n: " + n.ToString() + ", id: " + id.ToString());

                    //Coords for the vertices of the face            vert-num     x                       vert-num     y                       vert-num     z
                    Vector3 vert01 = new Vector3(vertices[triangles[id + 0] * 8 + 0], vertices[triangles[id + 0] * 8 + 1], vertices[triangles[id + 0] * 8 + 2]);
                    Vector3 vert02 = new Vector3(vertices[triangles[id + 1] * 8 + 0], vertices[triangles[id + 1] * 8 + 1], vertices[triangles[id + 1] * 8 + 2]);
                    Vector3 vert03 = new Vector3(vertices[triangles[id + 2] * 8 + 0], vertices[triangles[id + 2] * 8 + 1], vertices[triangles[id + 2] * 8 + 2]);

                    //the normal, just one as the whole face has the same normal-dir
                    Vector3 a = vert02 - vert01;
                    Vector3 b = vert03 - vert01;
                    normals[n - 1] = Vector3.Cross(a, b);
                    addedNormals++;
                }
                //resize the array, leaves the inserted values intact, just cuts off the "unused" end of the array
                if (addedNormals == 0) { continue; }
                if (addedNormals != 8) { Array.Resize(ref normals, addedNormals); }
                //Debug.WriteLine("AddedNormals: " + addedNormals.ToString());

                //combine all the normal-dirs into one dir
                Vector3 normalFinal = Vector3.Zero;
                foreach (Vector3 norm in normals)
                {
                    normalFinal += norm;
                }

                //get the average of all the added normals
                normalFinal /= addedNormals;
                normalFinal = Vector3.Normalize(normalFinal);

                //Debug.WriteLine("Normal: " + normalFinal.ToString());

                //assigning normal data
                vertices[i + 3] = normalFinal.X;
                vertices[i + 4] = normalFinal.Y;
                vertices[i + 5] = normalFinal.Z;
                vertAmountAssigned++;
            }

        }
        /// <summary> Calculates the Normals perpendicular to each Tris Orientation. </summary>
        public void CalcNormalsFlat()
        {
            //calc normals
            for (int i = 0; i < triangles.Length - 3; i += 3)
            {
                //Coords for the vertices of the face       vert-num         x                  vert-num         y                  vert-num         z
                Vector3 vert01 = new Vector3(vertices[triangles[i + 0] * 8 + 0], vertices[triangles[i + 0] * 8 + 1], vertices[triangles[i + 0] * 8 + 2]);
                Vector3 vert02 = new Vector3(vertices[triangles[i + 1] * 8 + 0], vertices[triangles[i + 1] * 8 + 1], vertices[triangles[i + 1] * 8 + 2]);
                Vector3 vert03 = new Vector3(vertices[triangles[i + 2] * 8 + 0], vertices[triangles[i + 2] * 8 + 1], vertices[triangles[i + 2] * 8 + 2]);

                //the normal, just one as the whole face has the same normal-dir
                Vector3 a = vert02 - vert01;
                Vector3 b = vert03 - vert01;
                Vector3 norm = Vector3.Normalize(Vector3.Cross(a, b));

                //assigning normal data
                //vert01
                vertices[triangles[i + 0] * 8 + 3] = norm.X;
                vertices[triangles[i + 0] * 8 + 4] = norm.Y;
                vertices[triangles[i + 0] * 8 + 5] = norm.Z;
                //vert02
                vertices[triangles[i + 1] * 8 + 3] = norm.X;
                vertices[triangles[i + 1] * 8 + 4] = norm.Y;
                vertices[triangles[i + 1] * 8 + 5] = norm.Z;
                //vert03
                vertices[triangles[i + 2] * 8 + 3] = norm.X;
                vertices[triangles[i + 2] * 8 + 4] = norm.Y;
                vertices[triangles[i + 3] * 8 + 5] = norm.Z;
            }
        }
        #endregion

        #region OPENGL
        //opengl-----------------
        /// <summary> 
        /// Generates the VAO, VBO and EBO.  Loads the generated data into the GPU. 
        /// <para> Needs to be called before the mesh can be rendered. </para>
        /// <para> VAO: Vertex Array Object. </para>
        /// <para> VBO: Vertex Buffer Object. </para>
        /// <para> EBO: Element Buffer Object. </para>
        /// </summary>
        public void GenOpenGLData()
        {
            if (!notIndexed) { GenOpenGLDataDefault(); }
            else if (notIndexed) { GenOpenGLDataNotIndexed(); }
        }
        /// <summary> 
        /// Generates the OpenGL data for objects with triangle indices. 
        /// <para> Don't call this directly, 'GenOpenGLData()' will call the right method for creating the OpenGL data. </para>
        /// </summary>
        private void GenOpenGLDataDefault()
        {
            //create VAO and VBO
            vao = glGenVertexArray();
            vbo = glGenBuffer();

            glBindVertexArray(vao);

            glBindBuffer(GL_ARRAY_BUFFER, vbo);

            unsafe
            {
                //the fixed framework makes sure all mentioned pointers('v') are consistent and stay the same
                fixed (float* v = &vertices[0])
                {
                    glBufferData(GL_ARRAY_BUFFER, sizeof(float) * vertices.Length, v, GL_STATIC_DRAW);
                }


                glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, ebo);
                //the fixed framework makes sure all mentioned pointers('e') are consistent and stay the same
                fixed (uint* e = &triangles[0])
                {
                    glBufferData(GL_ELEMENT_ARRAY_BUFFER, sizeof(uint) * triangles.Length, e, GL_STATIC_DRAW);
                }

                //tell vao how to interpret the data in vbo
                //arg01: vbo-index - for using mutiple vbo/ pointing to the same one multiple times
                //arg02: size-per-vertex - how many vars per vertex
                //arg03: type of data - e.g. int/float/etc.
                //arg04: normalize - normalizes the data (changes it)
                //arg05: amount of bytes inbetween each pair of relevant data - 5 * float-size, because there are 5 floats for each set of data out of which we on use the first two as specified in arg02
                //arg06: offset between start of the data-set as a whole and the first set of relevant data
                //arg06&(void*): pointer of type void, (void*)0 is a pointer to "first position"
                glVertexAttribPointer(0, 3, GL_FLOAT, false, sizeof(float) * 8, (void*)0);
                glEnableVertexAttribArray(0); //enables the vertex-attrib-array at the index given in arg01 above

                //same as above but now for the middle 3 values, the normal coordinates of the vertex, used to be the r,g,b values of the respective vertex-color
                glVertexAttribPointer(1, 3, GL_FLOAT, false, sizeof(float) * 8, (void*)(sizeof(float) * 3));
                glEnableVertexAttribArray(1); //enables the vertex-attrib-array at the index given in arg01 above

                //same as above but now for the latter 2 values, the uv coordinates of the vertex
                glVertexAttribPointer(2, 2, GL_FLOAT, false, sizeof(float) * 8, (void*)(sizeof(float) * 6));
                glEnableVertexAttribArray(2); //enables the vertex-attrib-array at the index given in arg01 above
            }

            glBindBuffer(GL_ARRAY_BUFFER, 0);
            glBindVertexArray(0);
        }
        /// <summary> 
        /// Generates the OpenGL data for objects without triangle indices. 
        /// <para> Don't call this directly, 'GenOpenGLData()' will call the right method for creating the OpenGL data. </para>
        /// </summary>
        private void GenOpenGLDataNotIndexed()
        {
            //create VAO and VBO
            vao = glGenVertexArray();
            vbo = glGenBuffer();

            glBindVertexArray(vao);

            glBindBuffer(GL_ARRAY_BUFFER, vbo);

            unsafe
            {
                //the fixed framework makes sure all mentioned pointers('v') are consistent and stay the same
                fixed (float* v = &vertices[0])
                {
                    glBufferData(GL_ARRAY_BUFFER, sizeof(float) * vertices.Length, v, GL_STATIC_DRAW);
                }

                //tell vao how to interpret the data in vbo
                //arg01: vbo-index - for using mutiple vbo/ pointing to the same one multiple times
                //arg02: size-per-vertex - how many vars per vertex
                //arg03: type of data - e.g. int/float/etc.
                //arg04: normalize - normalizes the data (changes it)
                //arg05: amount of bytes inbetween each pair of relevant data - 5 * float-size, because there are 5 floats for each set of data out of which we on use the first two as specified in arg02
                //arg06: offset between start of the data-set as a whole and the first set of relevant data
                //arg06&(void*): pointer of type void, (void*)0 is a pointer to "first position"
                glVertexAttribPointer(0, 3, GL_FLOAT, false, sizeof(float) * 8, (void*)0);
                glEnableVertexAttribArray(0); //enables the vertex-attrib-array at the index given in arg01 above

                //same as above but now for the middle 3 values, the normal coordinates of the vertex, used to be the r,g,b values of the respective vertex-color
                glVertexAttribPointer(1, 3, GL_FLOAT, false, sizeof(float) * 8, (void*)(sizeof(float) * 3));
                glEnableVertexAttribArray(1); //enables the vertex-attrib-array at the index given in arg01 above

                //same as above but now for the latter 2 values, the uv coordinates of the vertex
                glVertexAttribPointer(2, 2, GL_FLOAT, false, sizeof(float) * 8, (void*)(sizeof(float) * 6));
                glEnableVertexAttribArray(2); //enables the vertex-attrib-array at the index given in arg01 above
            }

            glBindBuffer(GL_ARRAY_BUFFER, 0);
            glBindVertexArray(0);
        }
        #endregion
    }
}
