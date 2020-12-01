using BitsCore.DataManagement;
using BitsCore.ObjectData.Materials;
using System.Numerics;

namespace BitsCore.ObjectData.Components
{
    public class Model : Component
    {
        public Mesh mesh;
        public string meshFileName;

        /// <summary> The Material of the Mesh. </summary>
        public Material material { get; private set; }
        
        public Model(Mesh _mesh, Material _material, string _meshFileName)
        {
            this.mesh = _mesh;
            this.material = _material;
            this.meshFileName = _meshFileName;
        }
        public Model(Material _material, string _meshFilePath)
        {
            this.mesh = AssetManager.GetMesh(meshFileName);
            this.material = _material;
            this.meshFileName = _meshFilePath;
        }

        internal override void OnAdd()
        {
        }

        internal override void OnRemove()
        {
        }

        #region SET_METHODS

        /// <summary> Sets the Material of the Mesh-Component. </summary>
        /// <param name="mat"> The Material that will be assigned to the Mesh-Component. </param>
        public void SetMaterial(Material mat)
        {
            material = mat;
            material.shader.Load();
        }

        #endregion

        #region GET_METHODS
        /// <summary> Returns the Positions of the Vertices Transformed to World-Space. </summary>
        public float[] GetVerticesWorldPosition()
        {
            float[] verts = new float[mesh.vertices.Length];
            for (int i = 0; i < mesh.vertices.Length; i += 8)
            {
                Vector3 vert = new Vector3(mesh.vertices[i + 0], mesh.vertices[i + 1], mesh.vertices[i + 2]);
                //Vector3 newVert = Maths.Vec3ByMatrix4x4(vert, gameObject.transform.GetModelMatrix());
                Vector4 newVert = Vector4.Transform(vert, gameObject.transform.GetModelMatrix());
                //newVert = Vector4.Transform(newVert, Camera3D.inst.GetViewMatrix());
                //newVert = Vector4.Transform(newVert, Camera3D.inst.GetProjectionMatrix());

                verts[i + 0] = newVert.X;
                verts[i + 1] = newVert.Y;
                verts[i + 2] = newVert.Z;
                verts[i + 3] = mesh.vertices[i + 3];
                verts[i + 4] = mesh.vertices[i + 4];
                verts[i + 5] = mesh.vertices[i + 5];
                verts[i + 6] = mesh.vertices[i + 6];
                verts[i + 7] = mesh.vertices[i + 7];

                //Debug.WriteLine("VertWorld - X: " + newVert.X.ToString() + ", Y: " + newVert.Y.ToString() + ", Z: " + newVert.Z.ToString() + ", W: " + newVert.W.ToString());
            }

            return verts;
        }

        #endregion

        public void CalcNormals(bool smooth)
        {
            if (smooth)
            {
                mesh.CalcNormalsSmooth();
            }
            else
            {
                mesh.CalcNormalsFlat();
            }
        }
        public void GenOpenGLData()
        {
            mesh.GenOpenGLData();
        }
    }
}
