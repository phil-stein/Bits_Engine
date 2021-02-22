using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace BitsCore.Utils
{
    /// <summary> Some additional Maths Functions. </summary>
    public static class MathUtils
    {
        //Degree and Radians
        /// <summary> Convert degree to radians. </summary>
        /// <param name="degree"> The amount of rotation in degrees. </param>
        public static float DegreeToRadians(float degree)
        {
            return degree * (MathF.PI / 180);
        }
        /// <summary> Convert radians to degree. </summary>
        /// <param name="radians"> The amount of rotation in radians. </param>
        public static float RadiansToDegree(float radians)
        {
            return radians * (180 / MathF.PI);
        }


        // lerp
        /// <summary> Linear-Interpolation between the start and end values by the given percentage </summary>
        static public float Lerp(float start, float end, float percentage)
        {
            return start + percentage * (end - start);
        }
        /// <summary> Linear-Interpolation between the start and end values by the given percentage </summary>
        static public Vector3 Lerp(Vector3 start, Vector3 end, float percentage)
        {
            return new Vector3(start.X + percentage * (end.X - start.X), start.Y + percentage * (end.Y - start.Y), start.Z + percentage * (end.Z - start.Z));
        }

        //Vector Matrix-Multiplication
        /// <summary> Multiply a Vector3 with a Matrix4x4. </summary>
        /// <param name="vec"> The vector used for the multiplication. </param>
        /// <param name="mat"> The matrix used for the multiplication. </param>
        public static Vector3 Vec3ByMatrix4x4(Vector3 vec, Matrix4x4 mat)
        {
            float x = (vec.X * mat.M11) + (vec.X * mat.M12) + (vec.X * mat.M13) + (vec.X * mat.M14);
            float y = (vec.Y * mat.M21) + (vec.Y * mat.M22) + (vec.Y * mat.M23) + (vec.Y * mat.M24);
            float z = (vec.Z * mat.M31) + (vec.Z * mat.M32) + (vec.Z * mat.M33) + (vec.Z * mat.M34);

            return new Vector3(x, y, z);
        }
        /// <summary> Multiply a Vector4 with a Matrix4x4. </summary>
        /// <param name="vec"> The vector used for the multiplication. </param>
        /// <param name="mat"> The matrix used for the multiplication. </param>
        public static Vector3 Vec3ByMatrix4x4(Vector4 vec, Matrix4x4 mat)
        {
            float x = (vec.X * mat.M11) + (vec.X * mat.M12) + (vec.X * mat.M13) + (vec.X * mat.M14);
            float y = (vec.Y * mat.M21) + (vec.Y * mat.M22) + (vec.Y * mat.M23) + (vec.Y * mat.M24);
            float z = (vec.Z * mat.M31) + (vec.Z * mat.M32) + (vec.Z * mat.M33) + (vec.Z * mat.M34);

            return new Vector3(x, y, z);
        }
        /// <summary> Multiply a Vector4 with a Matrix4x4. </summary>
        /// <param name="vec"> The vector used for the multiplication. </param>
        /// <param name="mat"> The matrix used for the multiplication. </param>
        public static Vector4 Vec4ByMatrix4x4(Vector4 vec, Matrix4x4 mat)
        {
            float x = (vec.X * mat.M11) + (vec.X * mat.M12) + (vec.X * mat.M13) + (vec.X * mat.M14);
            float y = (vec.Y * mat.M21) + (vec.Y * mat.M22) + (vec.Y * mat.M23) + (vec.Y * mat.M24);
            float z = (vec.Z * mat.M31) + (vec.Z * mat.M32) + (vec.Z * mat.M33) + (vec.Z * mat.M34);

            return new Vector4(x, y, z, 1f);
        }
        /// <summary> Multiply a Vector3 with a Matrix4x4. </summary>
        /// <param name="vec"> The vector used for the multiplication. </param>
        /// <param name="mat"> The matrix used for the multiplication. </param>
        public static Vector4 Vec4ByMatrix4x4(Vector3 vec, Matrix4x4 mat)
        {
            float x = (vec.X * mat.M11) + (vec.X * mat.M12) + (vec.X * mat.M13) + (vec.X * mat.M14);
            float y = (vec.Y * mat.M21) + (vec.Y * mat.M22) + (vec.Y * mat.M23) + (vec.Y * mat.M24);
            float z = (vec.Z * mat.M31) + (vec.Z * mat.M32) + (vec.Z * mat.M33) + (vec.Z * mat.M34);

            return new Vector4(x, y, z, 1f);
        }
    }
}
