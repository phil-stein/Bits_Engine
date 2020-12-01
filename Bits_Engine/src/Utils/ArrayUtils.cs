using BitsCore.ObjectData.Components;
using BitsCore.ObjectData;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Numerics;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace BitsCore.Utils
{
    public static class ArrayUtils
    {
        #region SHIFT_RIGHT
        //array shift right-------------------------------------
        /// <summary> Shifts an array one position to the right. </summary>
        /// <param name="arr"> The array to be shifted. </param>
        public static object[] ShiftRight(object[] arr)
        {
            var temp = arr[arr.Length - 1];
            Array.Copy(arr, 0, arr, 1, arr.Length - 1);
            arr[0] = temp;

            return arr;
        }
        /// <summary> Shifts an array one position to the right. </summary>
        /// <param name="arr"> The array to be shifted. </param>
        public static byte[] ShiftRight(byte[] arr)
        {
            var temp = arr[arr.Length - 1];
            Array.Copy(arr, 0, arr, 1, arr.Length - 1);
            arr[0] = temp;

            return arr;
        }
        /// <summary> Shifts an array one position to the right. </summary>
        /// <param name="arr"> The array to be shifted. </param>
        public static int[] ShiftRight(int[] arr)
        {
            var temp = arr[arr.Length - 1];
            Array.Copy(arr, 0, arr, 1, arr.Length - 1);
            arr[0] = temp;

            return arr;
        }
        /// <summary> Shifts an array one position to the right. </summary>
        /// <param name="arr"> The array to be shifted. </param>
        public static uint[] ShiftRight(uint[] arr)
        {
            var temp = arr[arr.Length - 1];
            Array.Copy(arr, 0, arr, 1, arr.Length - 1);
            arr[0] = temp;

            return arr;
        }
        /// <summary> Shifts an array one position to the right. </summary>
        /// <param name="arr"> The array to be shifted. </param>
        public static float[] ShiftRight(float[] arr)
        {
            var temp = arr[arr.Length - 1];
            Array.Copy(arr, 0, arr, 1, arr.Length - 1);
            arr[0] = temp;

            return arr;
        }
        /// <summary> Shifts an array one position to the right. </summary>
        /// <param name="arr"> The array to be shifted. </param>
        public static double[] ShiftRight(double[] arr)
        {
            var temp = arr[arr.Length - 1];
            Array.Copy(arr, 0, arr, 1, arr.Length - 1);
            arr[0] = temp;

            return arr;
        }
        /// <summary> Shifts an array one position to the right. </summary>
        /// <param name="arr"> The array to be shifted. </param>
        public static GameObject[] ShiftRight(GameObject[] arr)
        {
            var temp = arr[arr.Length - 1];
            Array.Copy(arr, 0, arr, 1, arr.Length - 1);
            arr[0] = temp;

            return arr;
        }
        /// <summary> Shifts an array one position to the right. </summary>
        /// <param name="arr"> The array to be shifted. </param>
        public static Component[] ShiftRight(Component[] arr)
        {
            var temp = arr[arr.Length - 1];
            Array.Copy(arr, 0, arr, 1, arr.Length - 1);
            arr[0] = temp;

            return arr;
        }
        #endregion

        #region SHIFT_LEFT
        //array shift left---------------------------------------
        /// <summary> Shifts an array one position to the right. </summary>
        /// <param name="arr"> The array to be shifted. </param>
        public static object[] ShiftLeft(object[] arr)
        {
            var temp = arr[0];
            Array.Copy(arr, 1, arr, 0, arr.Length - 1);
            arr[arr.Length - 1] = temp;

            return arr;
        }
        /// <summary> Shifts an array one position to the right. </summary>
        /// <param name="arr"> The array to be shifted. </param>
        public static byte[] ShiftLeft(byte[] arr)
        {
            var temp = arr[0];
            Array.Copy(arr, 1, arr, 0, arr.Length - 1);
            arr[arr.Length - 1] = temp;

            return arr;
        }
        /// <summary> Shifts an array one position to the right. </summary>
        /// <param name="arr"> The array to be shifted. </param>
        public static int[] ShiftLeft(int[] arr)
        {
            var temp = arr[0];
            Array.Copy(arr, 1, arr, 0, arr.Length - 1);
            arr[arr.Length - 1] = temp;

            return arr;
        }
        /// <summary> Shifts an array one position to the right. </summary>
        /// <param name="arr"> The array to be shifted. </param>
        public static uint[] ShiftLeft(uint[] arr)
        {
            var temp = arr[0];
            Array.Copy(arr, 1, arr, 0, arr.Length - 1);
            arr[arr.Length - 1] = temp;

            return arr;
        }
        /// <summary> Shifts an array one position to the right. </summary>
        /// <param name="arr"> The array to be shifted. </param>
        public static float[] ShiftLeft(float[] arr)
        {
            var temp = arr[0];
            Array.Copy(arr, 1, arr, 0, arr.Length - 1);
            arr[arr.Length - 1] = temp;

            return arr;
        }
        /// <summary> Shifts an array one position to the right. </summary>
        /// <param name="arr"> The array to be shifted. </param>
        public static double[] ShiftLeft(double[] arr)
        {
            var temp = arr[0];
            Array.Copy(arr, 1, arr, 0, arr.Length - 1);
            arr[arr.Length - 1] = temp;

            return arr;
        }
        /// <summary> Shifts an array one position to the right. </summary>
        /// <param name="arr"> The array to be shifted. </param>
        public static GameObject[] ShiftLeft(GameObject[] arr)
        {
            var temp = arr[0];
            Array.Copy(arr, 1, arr, 0, arr.Length - 1);
            arr[arr.Length - 1] = temp;

            return arr;
        }
        /// <summary> Shifts an array one position to the right. </summary>
        /// <param name="arr"> The array to be shifted. </param>
        public static Component[] ShiftLeft(Component[] arr)
        {
            var temp = arr[0];
            Array.Copy(arr, 1, arr, 0, arr.Length - 1);
            arr[arr.Length - 1] = temp;

            return arr;
        }
        #endregion

        #region COMBINE
        //array-combine-----------------------------------------
        /// <summary> Adds the 'last' array behind the 'first' array. </summary>
        /// <param name="first"> Array that will be first in the returned array. </param>
        /// <param name="last"> Array that will be last in the returned array. </param>
        public static object[] Combine(object[] first, object[] last)
        {
            object[] newArray = new object[first.Length + last.Length];
            Array.Copy(first, newArray, first.Length);
            Array.Copy(last, 0, newArray, first.Length, last.Length);
            return newArray;
        }
        /// <summary> Adds the 'last' array behind the 'first' array. </summary>
        /// <param name="first"> Array that will be first in the returned array. </param>
        /// <param name="last"> Array that will be last in the returned array. </param>
        public static byte[] Combine(byte[] first, byte[] last)
        {
            byte[] newArray = new byte[first.Length + last.Length];
            Array.Copy(first, newArray, first.Length);
            Array.Copy(last, 0, newArray, first.Length, last.Length);
            return newArray;
        }
        /// <summary> Adds the 'last' array behind the 'first' array. </summary>
        /// <param name="first"> Array that will be first in the returned array. </param>
        /// <param name="last"> Array that will be last in the returned array. </param>
        public static int[] Combine(int[] first, int[] last)
        {
            int[] newArray = new int[first.Length + last.Length];
            Array.Copy(first, newArray, first.Length);
            Array.Copy(last, 0, newArray, first.Length, last.Length);
            return newArray;
        }
        /// <summary> Adds the 'last' array behind the 'first' array. </summary>
        /// <param name="first"> Array that will be first in the returned array. </param>
        /// <param name="last"> Array that will be last in the returned array. </param>
        public static uint[] Combine(uint[] first, uint[] last)
        {
            uint[] newArray = new uint[first.Length + last.Length];
            Array.Copy(first, newArray, first.Length);
            Array.Copy(last, 0, newArray, first.Length, last.Length);
            return newArray;
        }
        /// <summary> Adds the 'last' array behind the 'first' array. </summary>
        /// <param name="first"> Array that will be first in the returned array. </param>
        /// <param name="last"> Array that will be last in the returned array. </param>
        public static float[] Combine(float[] first, float[] last)
        {
            float[] newArray = new float[first.Length + last.Length];
            Array.Copy(first, newArray, first.Length);
            Array.Copy(last, 0, newArray, first.Length, last.Length);
            return newArray;
        }
        /// <summary> Adds the 'last' array behind the 'first' array. </summary>
        /// <param name="first"> Array that will be first in the returned array. </param>
        /// <param name="last"> Array that will be last in the returned array. </param>
        public static double[] Combine(double[] first, double[] last)
        {
            double[] newArray = new double[first.Length + last.Length];
            Array.Copy(first, newArray, first.Length);
            Array.Copy(last, 0, newArray, first.Length, last.Length);
            return newArray;
        }
        /// <summary> Adds the 'last' array behind the 'first' array. </summary>
        /// <param name="first"> Array that will be first in the returned array. </param>
        /// <param name="last"> Array that will be last in the returned array. </param>
        public static Vector3[] Combine(Vector3[] first, Vector3[] last)
        {
            Vector3[] newArray = new Vector3[first.Length + last.Length];
            Array.Copy(first, newArray, first.Length);
            Array.Copy(last, 0, newArray, first.Length, last.Length);
            return newArray;
        }
        /// <summary> Adds the 'last' array behind the 'first' array. </summary>
        /// <param name="first"> Array that will be first in the returned array. </param>
        /// <param name="last"> Array that will be last in the returned array. </param>
        public static GameObject[] Combine(GameObject[] first, GameObject[] last)
        {
            GameObject[] newArray = new GameObject[first.Length + last.Length];
            Array.Copy(first, newArray, first.Length);
            Array.Copy(last, 0, newArray, first.Length, last.Length);
            return newArray;
        }
        /// <summary> Adds the 'last' array behind the 'first' array. </summary>
        /// <param name="first"> Array that will be first in the returned array. </param>
        /// <param name="last"> Array that will be last in the returned array. </param>
        public static Component[] Combine(Component[] first, Component[] last)
        {
            Component[] newArray = new Component[first.Length + last.Length];
            Array.Copy(first, newArray, first.Length);
            Array.Copy(last, 0, newArray, first.Length, last.Length);
            return newArray;
        }
        #endregion

        #region CONVERSION
        //conversion-------------------------------------------
        /// <summary> Converts an int-array to a byte-array. </summary>
        /// <param name="arr"> The int-array to be converted. </param>
        public static byte[] IntToByte(int[] arr)
        {
            byte[] byteArray = new byte[arr.Length * sizeof(int)];
            Buffer.BlockCopy(arr, 0, byteArray, 0, byteArray.Length);
            return byteArray;
        }
        /// <summary> Converts a byte-array to an int-array. </summary>
        /// <param name="arr"> The byte-array to be converted. </param>
        public static int[] BytesToInt(byte[] arr)
        {
            int[] intArray = new int[arr.Length / sizeof(int)];
            Buffer.BlockCopy(arr, 0, intArray, 0, arr.Length);
            return intArray;
        }
        /// <summary> Converts an uint-array to a byte-array. </summary>
        /// <param name="arr"> The uint-array to be converted. </param>
        public static byte[] UintToByte(uint[] arr)
        {
            byte[] byteArray = new byte[arr.Length * sizeof(uint)];
            Buffer.BlockCopy(arr, 0, byteArray, 0, byteArray.Length);
            return byteArray;
        }
        /// <summary> Converts a byte-array to an uint-array. </summary>
        /// <param name="arr"> The byte-array to be converted. </param>
        public static uint[] BytesToUint(byte[] arr)
        {
            uint[] intArray = new uint[arr.Length / sizeof(uint)];
            Buffer.BlockCopy(arr, 0, intArray, 0, arr.Length);
            return intArray;
        }
        /// <summary> Converts a float-array to a byte-array. </summary>
        /// <param name="arr"> The float-array to be converted. </param>
        public static byte[] FloatToByte(float[] arr)
        {
            var byteArray = new byte[arr.Length * sizeof(float)];
            Buffer.BlockCopy(arr, 0, byteArray, 0, byteArray.Length);
            return byteArray;
        }
        /// <summary> Converts a byte-array to a float-array. </summary>
        /// <param name="arr"> The byte-array to be converted. </param>
        public static float[] BytesToFloat(byte[] arr)
        {
            float[] floatArray = new float[arr.Length / sizeof(float)];
            Buffer.BlockCopy(arr, 0, floatArray, 0, arr.Length);
            return floatArray;
        }
        /// <summary> Converts a Vector3-array to a byte-array. </summary>
        /// <param name="arr"> The Vector3-array to be converted. </param>
        public static byte[] Vector3ToByte(Vector3[] arr)
        {
            List<byte> bytes = new List<byte>(); //3 floats per vector
            foreach(Vector3 vec in arr)
            {
                bytes.AddRange(VarUtils.VarToBytes(vec));
            }
            return bytes.ToArray();
        }
        /// <summary> Converts a byte-array to a Vector3-array. </summary>
        /// <param name="arr"> The byte-array to be converted. </param>
        public static Vector3[] BytesToVector3(byte[] arr)
        {
            //Debug.WriteLine("\n  |ArrayUtils.BytesToVector----------------");
            Vector3[] vecs = new Vector3[arr.Length / (sizeof(float)*3)];
            //Debug.WriteLine("  |Arr-Len: " + arr.Length.ToString() + ", sizeof(float): " + sizeof(float).ToString() + ", sizeof(float)*3: " + (sizeof(float) * 3).ToString() + ", Vecs-Len: " + vecs.Length.ToString() + "\n");
            int vecsPos = 0;
            for(int i = 0; i <= arr.Length / sizeof(float) * 3; i+= sizeof(float) * 3)
            {
                vecs[vecsPos] = VarUtils.BytesToVector3(ArrayUtils.GetBetweenXAndY(arr, i, i + sizeof(float) * 3));
                vecsPos++;
            }
            return vecs;
        }
        #endregion

        #region GET_FIRST_X
        //GetFirstX--------------------------------------------
        /// <summary> Get the first x items from the given array. </summary>
        /// <param name="arr"> The array to be extracted from. </param>
        /// <param name="x"> The amount of items to get. </param>
        public static object[] GetFirstX(object[] arr, int x)
        {
            if (x > arr.Length) { System.Diagnostics.Debug.WriteLine("!!! 'x' too big in ArrayUtils.GetLastX() !!!"); return null; }
            else if (x < 1) { System.Diagnostics.Debug.WriteLine("!!! 'x' too small in ArrayUtils.GetFirstX() !!!"); return null; }
            Array.Resize(ref arr, x);
            return arr;
        }
        /// <summary> Get the first x items from the given array. </summary>
        /// <param name="arr"> The array to be extracted from. </param>
        /// <param name="x"> The amount of items to get. </param>
        public static byte[] GetFirstX(byte[] arr, int x)
        {
            if (x > arr.Length) { System.Diagnostics.Debug.WriteLine("!!! 'x' too big in ArrayUtils.GetFirstX() !!!"); return null; }
            else if (x < 1) { System.Diagnostics.Debug.WriteLine("!!! 'x' too small in ArrayUtils.GetFirstX() !!!"); return null; }
            Array.Resize(ref arr, x);
            return arr;
        }
        /// <summary> Get the first x items from the given array. </summary>
        /// <param name="arr"> The array to be extracted from. </param>
        /// <param name="x"> The amount of items to get. </param>
        public static int[] GetFirstX(int[] arr, int x)
        {
            if (x > arr.Length) { System.Diagnostics.Debug.WriteLine("!!! 'x' too big in ArrayUtils.GetFirstX() !!!"); return null; }
            else if (x < 1) { System.Diagnostics.Debug.WriteLine("!!! 'x' too small in ArrayUtils.GetFirstX() !!!"); return null; }
            Array.Resize(ref arr, x);
            return arr;
        }
        /// <summary> Get the first x items from the given array. </summary>
        /// <param name="arr"> The array to be extracted from. </param>
        /// <param name="x"> The amount of items to get. </param>
        public static uint[] GetFirstX(uint[] arr, int x)
        {
            if (x > arr.Length) { System.Diagnostics.Debug.WriteLine("!!! 'x' too big in ArrayUtils.GetFirstX() !!!"); return null; }
            else if (x < 1) { System.Diagnostics.Debug.WriteLine("!!! 'x' too small in ArrayUtils.GetFirstX() !!!"); return null; }
            Array.Resize(ref arr, x);
            return arr;
        }
        /// <summary> Get the first x items from the given array. </summary>
        /// <param name="arr"> The array to be extracted from. </param>
        /// <param name="x"> The amount of items to get. </param>
        public static float[] GetFirstX(float[] arr, int x)
        {
            if (x > arr.Length) { System.Diagnostics.Debug.WriteLine("!!! 'x' too big in ArrayUtils.GetFirstX() !!!"); return null; }
            else if (x < 1) { System.Diagnostics.Debug.WriteLine("!!! 'x' too small in ArrayUtils.GetFirstX() !!!"); return null; }
            Array.Resize(ref arr, x);
            return arr;
        }
        /// <summary> Get the first x items from the given array. </summary>
        /// <param name="arr"> The array to be extracted from. </param>
        /// <param name="x"> The amount of items to get. </param>
        public static double[] GetFirstX(double[] arr, int x)
        {
            if (x > arr.Length) { System.Diagnostics.Debug.WriteLine("!!! 'x' too big in ArrayUtils.GetFirstX() !!!"); return null; }
            else if (x < 1) { System.Diagnostics.Debug.WriteLine("!!! 'x' too small in ArrayUtils.GetFirstX() !!!"); return null; }
            Array.Resize(ref arr, x);
            return arr;
        }
        /// <summary> Get the first x items from the given array. </summary>
        /// <param name="arr"> The array to be extracted from. </param>
        /// <param name="x"> The amount of items to get. </param>
        public static Vector3[] GetFirstX(Vector3[] arr, int x)
        {
            if (x > arr.Length) { System.Diagnostics.Debug.WriteLine("!!! 'x' too big in ArrayUtils.GetFirstX() !!!"); return null; }
            else if (x < 1) { System.Diagnostics.Debug.WriteLine("!!! 'x' too small in ArrayUtils.GetFirstX() !!!"); return null; }
            Array.Resize(ref arr, x);
            return arr;
        }
        /// <summary> Get the first x items from the given array. </summary>
        /// <param name="arr"> The array to be extracted from. </param>
        /// <param name="x"> The amount of items to get. </param>
        public static GameObject[] GetFirstX(GameObject[] arr, int x)
        {
            if (x > arr.Length) { System.Diagnostics.Debug.WriteLine("!!! 'x' too big in ArrayUtils.GetFirstX() !!!"); return null; }
            else if (x < 1) { System.Diagnostics.Debug.WriteLine("!!! 'x' too small in ArrayUtils.GetFirstX() !!!"); return null; }
            Array.Resize(ref arr, x);
            return arr;
        }
        /// <summary> Get the first x items from the given array. </summary>
        /// <param name="arr"> The array to be extracted from. </param>
        /// <param name="x"> The amount of items to get. </param>
        public static Component[] GetFirstX(Component[] arr, int x)
        {
            if (x > arr.Length) { System.Diagnostics.Debug.WriteLine("!!! 'x' too big in ArrayUtils.GetFirstX() !!!"); return null; }
            else if (x < 1) { System.Diagnostics.Debug.WriteLine("!!! 'x' too small in ArrayUtils.GetFirstX() !!!"); return null; }
            Array.Resize(ref arr, x);
            return arr;
        }
        #endregion

        #region GET_LAST_X
        //GetLastX--------------------------------------------
        /// <summary> Get the last x items from the given array. </summary>
        /// <param name="arr"> The array to be extracted from. </param>
        /// <param name="x"> The amount of items to get. </param>
        public static object[] GetLastX(object[] arr, int x)
        {
            if (x > arr.Length) { System.Diagnostics.Debug.WriteLine("!!! 'x' too big in ArrayUtils.GetLastX() !!!"); return null; }
            else if (x < 1) { System.Diagnostics.Debug.WriteLine("!!! 'x' too small in ArrayUtils.GetLastX() !!!"); return null; }
            int exclude = arr.Length - x;
            object[] result = new object[x];
            for (int i = 0; i < x; i++)
            {
                result[i] = arr[exclude + i];
            }
            return result;
        }
        /// <summary> Get the last x items from the given array. </summary>
        /// <param name="arr"> The array to be extracted from. </param>
        /// <param name="x"> The amount of items to get. </param>
        public static byte[] GetLastX(byte[] arr, int x)
        {
            if (x > arr.Length) { System.Diagnostics.Debug.WriteLine("!!! 'x' too big in ArrayUtils.GetLastX() !!!"); return null; }
            else if (x < 1) { System.Diagnostics.Debug.WriteLine("!!! 'x' too small in ArrayUtils.GetLastX() !!!"); return null; }
            int exclude = arr.Length - x;
            byte[] result = new byte[x];
            for(int i = 0; i < x; i++)
            {
                result[i] = arr[exclude + i];
            }
            return result;
        }
        /// <summary> Get the last x items from the given array. </summary>
        /// <param name="arr"> The array to be extracted from. </param>
        /// <param name="x"> The amount of items to get. </param>
        public static int[] GetLastX(int[] arr, int x)
        {
            if (x > arr.Length) { System.Diagnostics.Debug.WriteLine("!!! 'x' too big in ArrayUtils.GetLastX() !!!"); return null; }
            else if (x < 1) { System.Diagnostics.Debug.WriteLine("!!! 'x' too small in ArrayUtils.GetLastX() !!!"); return null; }
            int exclude = arr.Length - x;
            int[] result = new int[x];
            for (int i = 0; i < x; i++)
            {
                result[i] = arr[exclude + i];
            }
            return result;
        }
        /// <summary> Get the last x items from the given array. </summary>
        /// <param name="arr"> The array to be extracted from. </param>
        /// <param name="x"> The amount of items to get. </param>
        public static uint[] GetLastX(uint[] arr, int x)
        {
            if (x > arr.Length) { System.Diagnostics.Debug.WriteLine("!!! 'x' too big in ArrayUtils.GetLastX() !!!"); return null; }
            else if (x < 1) { System.Diagnostics.Debug.WriteLine("!!! 'x' too small in ArrayUtils.GetLastX() !!!"); return null; }
            int exclude = arr.Length - x;
            uint[] result = new uint[x];
            for (int i = 0; i < x; i++)
            {
                result[i] = arr[exclude + i];
            }
            return result;
        }
        /// <summary> Get the last x items from the given array. </summary>
        /// <param name="arr"> The array to be extracted from. </param>
        /// <param name="x"> The amount of items to get. </param>
        public static float[] GetLastX(float[] arr, int x)
        {
            if (x > arr.Length) { System.Diagnostics.Debug.WriteLine("!!! 'x' too big in ArrayUtils.GetLastX() !!!"); return null; }
            else if (x < 1) { System.Diagnostics.Debug.WriteLine("!!! 'x' too small in ArrayUtils.GetLastX() !!!"); return null; }
            int exclude = arr.Length - x;
            float[] result = new float[x];
            for (int i = 0; i < x; i++)
            {
                result[i] = arr[exclude + i];
            }
            return result;
        }
        /// <summary> Get the last x items from the given array. </summary>
        /// <param name="arr"> The array to be extracted from. </param>
        /// <param name="x"> The amount of items to get. </param>
        public static double[] GetLastX(double[] arr, int x)
        {
            if (x > arr.Length) { System.Diagnostics.Debug.WriteLine("!!! 'x' too big in ArrayUtils.GetLastX() !!!"); return null; }
            else if (x < 1) { System.Diagnostics.Debug.WriteLine("!!! 'x' too small in ArrayUtils.GetLastX() !!!"); return null; }
            int exclude = arr.Length - x;
            double[] result = new double[x];
            for (int i = 0; i < x; i++)
            {
                result[i] = arr[exclude + i];
            }
            return result;
        }
        /// <summary> Get the last x items from the given array. </summary>
        /// <param name="arr"> The array to be extracted from. </param>
        /// <param name="x"> The amount of items to get. </param>
        public static Vector3[] GetLastX(Vector3[] arr, int x)
        {
            if (x > arr.Length) { System.Diagnostics.Debug.WriteLine("!!! 'x' too big in ArrayUtils.GetLastX() !!!"); return null; }
            else if (x < 1) { System.Diagnostics.Debug.WriteLine("!!! 'x' too small in ArrayUtils.GetLastX() !!!"); return null; }
            int exclude = arr.Length - x;
            Vector3[] result = new Vector3[x];
            for (int i = 0; i < x; i++)
            {
                result[i] = arr[exclude + i];
            }
            return result;
        }
        /// <summary> Get the last x items from the given array. </summary>
        /// <param name="arr"> The array to be extracted from. </param>
        /// <param name="x"> The amount of items to get. </param>
        public static GameObject[] GetLastX(GameObject[] arr, int x)
        {
            if (x > arr.Length) { System.Diagnostics.Debug.WriteLine("!!! 'x' too big in ArrayUtils.GetLastX() !!!"); return null; }
            else if (x < 1) { System.Diagnostics.Debug.WriteLine("!!! 'x' too small in ArrayUtils.GetLastX() !!!"); return null; }
            int exclude = arr.Length - x;
            GameObject[] result = new GameObject[x];
            for (int i = 0; i < x; i++)
            {
                result[i] = arr[exclude + i];
            }
            return result;
        }
        /// <summary> Get the last x items from the given array. </summary>
        /// <param name="arr"> The array to be extracted from. </param>
        /// <param name="x"> The amount of items to get. </param>
        public static Component[] GetLastX(Component[] arr, int x)
        {
            if (x > arr.Length) { System.Diagnostics.Debug.WriteLine("!!! 'x' too big in ArrayUtils.GetLastX() !!!"); return null; }
            else if (x < 1) { System.Diagnostics.Debug.WriteLine("!!! 'x' too small in ArrayUtils.GetLastX() !!!"); return null; }
            int exclude = arr.Length - x;
            Component[] result = new Component[x];
            for (int i = 0; i < x; i++)
            {
                result[i] = arr[exclude + i];
            }
            return result;
        }
        #endregion

        #region GET_BETWEEN_X_AND_Y
        //GetBetweenXAndY--------------------------------------------
        /// <summary> Get the all items with indices higher that or equal to x[INCLUSIVE] and lower than or equal to y[INCLUSIVE] from the given array. </summary>
        /// <param name="arr"> The array to be extracted from. </param>
        /// <param name="x"> The amount of items to be skipped at the start. </param>
        /// <param name="y"> The amount of items to be skipped at the end. </param>
        public static byte[] GetBetweenXAndY(byte[] arr, int x, int y)
        {
            int span = y - x +1 ; //+1 to make y inclusive
            if (span > arr.Length) { System.Diagnostics.Debug.WriteLine("!!! Span in ArrayUtils.GetBetweenXAndY too big !!!!"); return null; }
            else if (span < 1) { System.Diagnostics.Debug.WriteLine("!!! Span in ArrayUtils.GetBetweenXAndY too small !!!!"); return null; }

            //Debug.WriteLine("   > Arr-Len: " + arr.Length.ToString() + ", span: " + span.ToString() + ", x: " + x.ToString());

            byte[] result = new byte[span];
            for (int i = 0; i < span; i++)
            {
                result[i] = arr[x + i];
            }
            return result;
        }
        /// <summary> Get the all items with indices higher that or equal to x[INCLUSIVE] and lower than or equal to y[INCLUSIVE] from the given array. </summary>
        /// <param name="arr"> The array to be extracted from. </param>
        /// <param name="x"> The amount of items to be skipped at the start. </param>
        /// <param name="y"> The amount of items to be skipped at the end. </param>
        public static int[] GetBetweenXAndY(int[] arr, int x, int y)
        {
            int span = y - x +1; //+1 to make y inclusive
            if (span > arr.Length) { System.Diagnostics.Debug.WriteLine("!!! Span in ArrayUtils.GetBetweenXAndY too big !!!!"); return null; }
            else if (span < 1) { System.Diagnostics.Debug.WriteLine("!!! Span in ArrayUtils.GetBetweenXAndY too small !!!!"); return null; }

            int[] result = new int[span];
            for (int i = 0; i < span; i++)
            {
                result[i] = arr[x + i];
            }
            return result;
        }
        /// <summary> Get the all items with indices higher that or equal to x[INCLUSIVE] and lower than or equal to y[INCLUSIVE] from the given array. </summary>
        /// <param name="arr"> The array to be extracted from. </param>
        /// <param name="x"> The amount of items to be skipped at the start. </param>
        /// <param name="y"> The amount of items to be skipped at the end. </param>
        public static uint[] GetBetweenXAndY(uint[] arr, int x, int y)
        {
            int span = y - x + 1; //+1 to make y inclusive
            if (span > arr.Length) { System.Diagnostics.Debug.WriteLine("!!! Span in ArrayUtils.GetBetweenXAndY too big !!!!"); return null; }
            else if (span < 1) { System.Diagnostics.Debug.WriteLine("!!! Span in ArrayUtils.GetBetweenXAndY too small !!!!"); return null; }

            uint[] result = new uint[span];
            for (int i = 0; i < span; i++)
            {
                result[i] = arr[x + i];
            }
            return result;
        }
        /// <summary> Get the all items with indices higher that or equal to x[INCLUSIVE] and lower than or equal to y[INCLUSIVE] from the given array. </summary>
        /// <param name="arr"> The array to be extracted from. </param>
        /// <param name="x"> The amount of items to be skipped at the start. </param>
        /// <param name="y"> The amount of items to be skipped at the end. </param>
        public static float[] GetBetweenXAndY(float[] arr, int x, int y)
        {
            int span = y - x + 1; //+1 to make y inclusive
            if (span > arr.Length) { System.Diagnostics.Debug.WriteLine("!!! Span in ArrayUtils.GetBetweenXAndY too big !!!!"); return null; }
            else if (span < 1) { System.Diagnostics.Debug.WriteLine("!!! Span in ArrayUtils.GetBetweenXAndY too small !!!!"); return null; }

            float[] result = new float[span];
            for (int i = 0; i < span; i++)
            {
                result[i] = arr[x + i];
            }
            return result;
        }
        /// <summary> Get the all items with indices higher that or equal to x[INCLUSIVE] and lower than or equal to y[INCLUSIVE] from the given array. </summary>
        /// <param name="arr"> The array to be extracted from. </param>
        /// <param name="x"> The amount of items to be skipped at the start. </param>
        /// <param name="y"> The amount of items to be skipped at the end. </param>
        public static double[] GetBetweenXAndY(double[] arr, int x, int y)
        {
            int span = y - x + 1; //+1 to make y inclusive
            if (span > arr.Length) { System.Diagnostics.Debug.WriteLine("!!! Span in ArrayUtils.GetBetweenXAndY too big !!!!"); return null; }
            else if (span < 1) { System.Diagnostics.Debug.WriteLine("!!! Span in ArrayUtils.GetBetweenXAndY too small !!!!"); return null; }

            double[] result = new double[span];
            for (int i = 0; i < span; i++)
            {
                result[i] = arr[x + i];
            }
            return result;
        }
        /// <summary> Get the all items with indices higher that or equal to x[INCLUSIVE] and lower than or equal to y[INCLUSIVE] from the given array. </summary>
        /// <param name="arr"> The array to be extracted from. </param>
        /// <param name="x"> The amount of items to be skipped at the start. </param>
        /// <param name="y"> The amount of items to be skipped at the end. </param>
        public static Vector3[] GetBetweenXAndY(Vector3[] arr, int x, int y)
        {
            int span = y - x + 1; //+1 to make y inclusive
            if (span > arr.Length) { System.Diagnostics.Debug.WriteLine("!!! Span in ArrayUtils.GetBetweenXAndY too big !!!!"); return null; }
            else if (span < 1) { System.Diagnostics.Debug.WriteLine("!!! Span in ArrayUtils.GetBetweenXAndY too small !!!!"); return null; }

            Vector3[] result = new Vector3[span];
            for (int i = 0; i < span; i++)
            {
                result[i] = arr[x + i];
            }
            return result;
        }
        /// <summary> Get the all items with indices higher that or equal to x[INCLUSIVE] and lower than or equal to y[INCLUSIVE] from the given array. </summary>
        /// <param name="arr"> The array to be extracted from. </param>
        /// <param name="x"> The amount of items to be skipped at the start. </param>
        /// <param name="y"> The amount of items to be skipped at the end. </param>
        public static GameObject[] GetBetweenXAndY(GameObject[] arr, int x, int y)
        {
            int span = y - x + 1; //+1 to make y inclusive
            if (span > arr.Length) { System.Diagnostics.Debug.WriteLine("!!! Span in ArrayUtils.GetBetweenXAndY too big !!!!"); return null; }
            else if (span < 1) { System.Diagnostics.Debug.WriteLine("!!! Span in ArrayUtils.GetBetweenXAndY too small !!!!"); return null; }

            GameObject[] result = new GameObject[span];
            for (int i = 0; i < span; i++)
            {
                result[i] = arr[x + i];
            }
            return result;
        }
        /// <summary> Get the all items with indices higher that or equal to x[INCLUSIVE] and lower than or equal to y[INCLUSIVE] from the given array. </summary>
        /// <param name="arr"> The array to be extracted from. </param>
        /// <param name="x"> The amount of items to be skipped at the start. </param>
        /// <param name="y"> The amount of items to be skipped at the end. </param>
        public static Component[] GetBetweenXAndY(Component[] arr, int x, int y)
        {
            int span = y - x + 1; //+1 to make y inclusive
            if (span > arr.Length) { System.Diagnostics.Debug.WriteLine("!!! Span in ArrayUtils.GetBetweenXAndY too big !!!!"); return null; }
            else if (span < 1) { System.Diagnostics.Debug.WriteLine("!!! Span in ArrayUtils.GetBetweenXAndY too small !!!!"); return null; }

            Component[] result = new Component[span];
            for (int i = 0; i < span; i++)
            {
                result[i] = arr[x + i];
            }
            return result;
        }
        #endregion

        #region GET_BETWEEN_X_AND_Y_EXCLUSIVE
        //GetBetweenXAndYExcl---------------------------------------
        /// <summary> Get the all items with indices higher that or equal to x[INCLUSIVE] and lower than y[EXCLUSIVE] from the given array. </summary>
        /// <param name="arr"> The array to be extracted from. </param>
        /// <param name="x"> The amount of items to be skipped at the start. </param>
        /// <param name="y"> The amount of items to be skipped at the end. </param>
        public static object[] GetBetweenXAndYExcl(object[] arr, int x, int y)
        {
            int span = y - x;
            if (span > arr.Length) { System.Diagnostics.Debug.WriteLine("!!! Span in ArrayUtils.GetBetweenXAndY too big !!!!"); return null; }
            else if (span < 1) { System.Diagnostics.Debug.WriteLine("!!! Span in ArrayUtils.GetBetweenXAndY too small !!!!"); return null; }

            object[] result = new object[span];
            for (int i = 0; i < span; i++)
            {
                result[i] = arr[x + i];
            }
            return result;
        }
        /// <summary> Get the all items with indices higher that or equal to x[INCLUSIVE] and lower than y[EXCLUSIVE] from the given array. </summary>
        /// <param name="arr"> The array to be extracted from. </param>
        /// <param name="x"> The amount of items to be skipped at the start. </param>
        /// <param name="y"> The amount of items to be skipped at the end. </param>
        public static byte[] GetBetweenXAndYExcl(byte[] arr, int x, int y)
        {
            int span = y - x;
            if (span > arr.Length) { System.Diagnostics.Debug.WriteLine("!!! Span in ArrayUtils.GetBetweenXAndY too big !!!!"); return null; }
            else if (span < 1) { System.Diagnostics.Debug.WriteLine("!!! Span in ArrayUtils.GetBetweenXAndY too small !!!!"); return null; }

            //Debug.WriteLine("   > Arr-Len: " + arr.Length.ToString() + ", span: " + span.ToString() + ", x: " + x.ToString());

            byte[] result = new byte[span];
            for (int i = 0; i < span; i++)
            {
                result[i] = arr[x + i];
            }
            return result;
        }
        /// <summary> Get the all items with indices higher that or equal to x[INCLUSIVE] and lower than y[EXCLUSIVE] from the given array. </summary>
        /// <param name="arr"> The array to be extracted from. </param>
        /// <param name="x"> The amount of items to be skipped at the start. </param>
        /// <param name="y"> The amount of items to be skipped at the end. </param>
        public static int[] GetBetweenXAndYExcl(int[] arr, int x, int y)
        {
            int span = y - x;
            if (span > arr.Length) { System.Diagnostics.Debug.WriteLine("!!! Span in ArrayUtils.GetBetweenXAndY too big !!!!"); return null; }
            else if (span < 1) { System.Diagnostics.Debug.WriteLine("!!! Span in ArrayUtils.GetBetweenXAndY too small !!!!"); return null; }

            int[] result = new int[span];
            for (int i = 0; i < span; i++)
            {
                result[i] = arr[x + i];
            }
            return result;
        }
        /// <summary> Get the all items with indices higher that or equal to x[INCLUSIVE] and lower than y[EXCLUSIVE] from the given array. </summary>
        /// <param name="arr"> The array to be extracted from. </param>
        /// <param name="x"> The amount of items to be skipped at the start. </param>
        /// <param name="y"> The amount of items to be skipped at the end. </param>
        public static uint[] GetBetweenXAndYExcl(uint[] arr, int x, int y)
        {
            int span = y - x;
            if (span > arr.Length) { System.Diagnostics.Debug.WriteLine("!!! Span in ArrayUtils.GetBetweenXAndY too big !!!!"); return null; }
            else if (span < 1) { System.Diagnostics.Debug.WriteLine("!!! Span in ArrayUtils.GetBetweenXAndY too small !!!!"); return null; }

            uint[] result = new uint[span];
            for (int i = 0; i < span; i++)
            {
                result[i] = arr[x + i];
            }
            return result;
        }
        /// <summary> Get the all items with indices higher that or equal to x[INCLUSIVE] and lower than y[EXCLUSIVE] from the given array. </summary>
        /// <param name="arr"> The array to be extracted from. </param>
        /// <param name="x"> The amount of items to be skipped at the start. </param>
        /// <param name="y"> The amount of items to be skipped at the end. </param>
        public static float[] GetBetweenXAndYExcl(float[] arr, int x, int y)
        {
            int span = y - x;
            if (span > arr.Length) { System.Diagnostics.Debug.WriteLine("!!! Span in ArrayUtils.GetBetweenXAndY too big !!!!"); return null; }
            else if (span < 1) { System.Diagnostics.Debug.WriteLine("!!! Span in ArrayUtils.GetBetweenXAndY too small !!!!"); return null; }

            float[] result = new float[span];
            for (int i = 0; i < span; i++)
            {
                result[i] = arr[x + i];
            }
            return result;
        }
        /// <summary> Get the all items with indices higher that or equal to x[INCLUSIVE] and lower than y[EXCLUSIVE] from the given array. </summary>
        /// <param name="arr"> The array to be extracted from. </param>
        /// <param name="x"> The amount of items to be skipped at the start. </param>
        /// <param name="y"> The amount of items to be skipped at the end. </param>
        public static double[] GetBetweenXAndYExcl(double[] arr, int x, int y)
        {
            int span = y - x;
            if (span > arr.Length) { System.Diagnostics.Debug.WriteLine("!!! Span in ArrayUtils.GetBetweenXAndY too big !!!!"); return null; }
            else if (span < 1) { System.Diagnostics.Debug.WriteLine("!!! Span in ArrayUtils.GetBetweenXAndY too small !!!!"); return null; }

            double[] result = new double[span];
            for (int i = 0; i < span; i++)
            {
                result[i] = arr[x + i];
            }
            return result;
        }
        /// <summary> Get the all items with indices higher that or equal to x[INCLUSIVE] and lower than y[EXCLUSIVE] from the given array. </summary>
        /// <param name="arr"> The array to be extracted from. </param>
        /// <param name="x"> The amount of items to be skipped at the start. </param>
        /// <param name="y"> The amount of items to be skipped at the end. </param>
        public static Vector3[] GetBetweenXAndYExcl(Vector3[] arr, int x, int y)
        {
            int span = y - x;
            if (span > arr.Length) { System.Diagnostics.Debug.WriteLine("!!! Span in ArrayUtils.GetBetweenXAndY too big !!!!"); return null; }
            else if (span < 1) { System.Diagnostics.Debug.WriteLine("!!! Span in ArrayUtils.GetBetweenXAndY too small !!!!"); return null; }

            Vector3[] result = new Vector3[span];
            for (int i = 0; i < span; i++)
            {
                result[i] = arr[x + i];
            }
            return result;
        }
        /// <summary> Get the all items with indices higher that or equal to x[INCLUSIVE] and lower than y[EXCLUSIVE] from the given array. </summary>
        /// <param name="arr"> The array to be extracted from. </param>
        /// <param name="x"> The amount of items to be skipped at the start. </param>
        /// <param name="y"> The amount of items to be skipped at the end. </param>
        public static GameObject[] GetBetweenXAndYExcl(GameObject[] arr, int x, int y)
        {
            int span = y - x;
            if (span > arr.Length) { System.Diagnostics.Debug.WriteLine("!!! Span in ArrayUtils.GetBetweenXAndY too big !!!!"); return null; }
            else if (span < 1) { System.Diagnostics.Debug.WriteLine("!!! Span in ArrayUtils.GetBetweenXAndY too small !!!!"); return null; }

            GameObject[] result = new GameObject[span];
            for (int i = 0; i < span; i++)
            {
                result[i] = arr[x + i];
            }
            return result;
        }
        /// <summary> Get the all items with indices higher that or equal to x[INCLUSIVE] and lower than y[EXCLUSIVE] from the given array. </summary>
        /// <param name="arr"> The array to be extracted from. </param>
        /// <param name="x"> The amount of items to be skipped at the start. </param>
        /// <param name="y"> The amount of items to be skipped at the end. </param>
        public static Component[] GetBetweenXAndYExcl(Component[] arr, int x, int y)
        {
            int span = y - x;
            if (span > arr.Length) { System.Diagnostics.Debug.WriteLine("!!! Span in ArrayUtils.GetBetweenXAndY too big !!!!"); return null; }
            else if (span < 1) { System.Diagnostics.Debug.WriteLine("!!! Span in ArrayUtils.GetBetweenXAndY too small !!!!"); return null; }

            Component[] result = new Component[span];
            for (int i = 0; i < span; i++)
            {
                result[i] = arr[x + i];
            }
            return result;
        }
        #endregion

        #region REMOVE_AT_X
        //RemoveAtX-------------------------------------------------------
        /// <summary> Removes element from array at index 'x'. </summary>
        /// <param name="arr"> The array to be edited. </param>
        /// <param name="x"> The index of the elem to be removed. </param>
        public static object[] RemoveAtX(object[] arr, int x)
        {
            if (x != 0 && x != arr.Length - 1)
            {
                object[] first = GetFirstX(arr, x);
                object[] second = GetLastX(arr, arr.Length - x - 1);

                return Combine(first, second);
            }
            else if (x == arr.Length - 1)
            {
                return GetFirstX(arr, x);
            }
            else
            {
                return GetLastX(arr, arr.Length - x - 1); ;
            }

        }
        /// <summary> Removes element from array at index 'x'. </summary>
        /// <param name="arr"> The array to be edited. </param>
        /// <param name="x"> The index of the elem to be removed. </param>
        public static byte[] RemoveAtX(byte[] arr, int x)
        {
            if (x != 0 && x != arr.Length - 1)
            {
                byte[] first = GetFirstX(arr, x);
                byte[] second = GetLastX(arr, arr.Length - x - 1);

                return Combine(first, second);
            }
            else if (x == arr.Length - 1)
            {
                return GetFirstX(arr, x);
            }
            else
            {
                return GetLastX(arr, arr.Length - x - 1); ;
            }

        }
        /// <summary> Removes element from array at index 'x'. </summary>
        /// <param name="arr"> The array to be edited. </param>
        /// <param name="x"> The index of the elem to be removed. </param>
        public static int[] RemoveAtX(int[] arr, int x)
        {
            if(x != 0 && x != arr.Length -1)
            {
                int[] first = GetFirstX(arr, x);
                int[] second = GetLastX(arr, arr.Length - x - 1);

                return Combine(first, second);
            }
            else if(x == arr.Length -1)
            {
                return GetFirstX(arr, x);
            }
            else
            {
                return GetLastX(arr, arr.Length - x - 1); ;
            }

        }
        /// <summary> Removes element from array at index 'x'. </summary>
        /// <param name="arr"> The array to be edited. </param>
        /// <param name="x"> The index of the elem to be removed. </param>
        public static uint[] RemoveAtX(uint[] arr, int x)
        {
            if (x != 0 && x != arr.Length - 1)
            {
                uint[] first = GetFirstX(arr, x);
                uint[] second = GetLastX(arr, arr.Length - x - 1);

                return Combine(first, second);
            }
            else if (x == arr.Length - 1)
            {
                return GetFirstX(arr, x);
            }
            else
            {
                return GetLastX(arr, arr.Length - x - 1); ;
            }

        }
        /// <summary> Removes element from array at index 'x'. </summary>
        /// <param name="arr"> The array to be edited. </param>
        /// <param name="x"> The index of the elem to be removed. </param>
        public static float[] RemoveAtX(float[] arr, int x)
        {
            if (x != 0 && x != arr.Length - 1)
            {
                float[] first = GetFirstX(arr, x);
                float[] second = GetLastX(arr, arr.Length - x - 1);

                return Combine(first, second);
            }
            else if (x == arr.Length - 1)
            {
                return GetFirstX(arr, x);
            }
            else
            {
                return GetLastX(arr, arr.Length - x - 1); ;
            }

        }
        /// <summary> Removes element from array at index 'x'. </summary>
        /// <param name="arr"> The array to be edited. </param>
        /// <param name="x"> The index of the elem to be removed. </param>
        public static double[] RemoveAtX(double[] arr, int x)
        {
            if (x != 0 && x != arr.Length - 1)
            {
                double[] first = GetFirstX(arr, x);
                double[] second = GetLastX(arr, arr.Length - x - 1);

                return Combine(first, second);
            }
            else if (x == arr.Length - 1)
            {
                return GetFirstX(arr, x);
            }
            else
            {
                return GetLastX(arr, arr.Length - x - 1); ;
            }

        }
        /// <summary> Removes element from array at index 'x'. </summary>
        /// <param name="arr"> The array to be edited. </param>
        /// <param name="x"> The index of the elem to be removed. </param>
        public static Vector3[] RemoveAtX(Vector3[] arr, int x)
        {
            if (x != 0 && x != arr.Length - 1)
            {
                Vector3[] first = GetFirstX(arr, x);
                Vector3[] second = GetLastX(arr, arr.Length - x - 1);

                return Combine(first, second);
            }
            else if (x == arr.Length - 1)
            {
                return GetFirstX(arr, x);
            }
            else
            {
                return GetLastX(arr, arr.Length - x - 1); ;
            }

        }
        /// <summary> Removes element from array at index 'x'. </summary>
        /// <param name="arr"> The array to be edited. </param>
        /// <param name="x"> The index of the elem to be removed. </param>
        public static GameObject[] RemoveAtX(GameObject[] arr, int x)
        {
            if (x != 0 && x != arr.Length - 1)
            {
                GameObject[] first = GetFirstX(arr, x);
                GameObject[] second = GetLastX(arr, arr.Length - x - 1);

                return Combine(first, second);
            }
            else if (x == arr.Length - 1)
            {
                return GetFirstX(arr, x);
            }
            else
            {
                return GetLastX(arr, arr.Length - x - 1); ;
            }

        }
        /// <summary> Removes element from array at index 'x'. </summary>
        /// <param name="arr"> The array to be edited. </param>
        /// <param name="x"> The index of the elem to be removed. </param>
        public static Component[] RemoveAtX(Component[] arr, int x)
        {
            if (x != 0 && x != arr.Length - 1)
            {
                Component[] first = GetFirstX(arr, x);
                Component[] second = GetLastX(arr, arr.Length - x - 1);

                return Combine(first, second);
            }
            else if (x == arr.Length - 1)
            {
                return GetFirstX(arr, x);
            }
            else
            {
                return GetLastX(arr, arr.Length - x - 1); ;
            }

        }
        #endregion

        #region ADD_TO_END
        //AddToEnd----------------------------------------------------
        /// <summary> Add an item to the end of an array. </summary>
        /// <param name="arr"> The array to be edited. </param>
        /// <param name="item"> The item to be added to the array. </param>
        public static object[] AddToEnd(object[] arr, object item)
        {
            Array.Resize(ref arr, arr.Length + 1);
            arr[arr.Length - 1] = item;
            return arr;
        }
        /// <summary> Add an item to the end of an array. </summary>
        /// <param name="arr"> The array to be edited. </param>
        /// <param name="item"> The item to be added to the array. </param>
        public static byte[] AddToEnd(byte[] arr, byte item)
        {
            Array.Resize(ref arr, arr.Length + 1);
            arr[arr.Length - 1] = item;
            return arr;
        }
        /// <summary> Add an item to the end of an array. </summary>
        /// <param name="arr"> The array to be edited. </param>
        /// <param name="item"> The item to be added to the array. </param>
        public static int[] AddToEnd(int[] arr, int item)
        {
            Array.Resize(ref arr, arr.Length + 1);
            arr[arr.Length - 1] = item;
            return arr;
        }
        /// <summary> Add an item to the end of an array. </summary>
        /// <param name="arr"> The array to be edited. </param>
        /// <param name="item"> The item to be added to the array. </param>
        public static uint[] AddToEnd(uint[] arr, uint item)
        {
            Array.Resize(ref arr, arr.Length + 1);
            arr[arr.Length - 1] = item;
            return arr;
        }
        /// <summary> Add an item to the end of an array. </summary>
        /// <param name="arr"> The array to be edited. </param>
        /// <param name="item"> The item to be added to the array. </param>
        public static float[] AddToEnd(float[] arr, float item)
        {
            Array.Resize(ref arr, arr.Length + 1);
            arr[arr.Length - 1] = item;
            return arr;
        }
        /// <summary> Add an item to the end of an array. </summary>
        /// <param name="arr"> The array to be edited. </param>
        /// <param name="item"> The item to be added to the array. </param>
        public static double[] AddToEnd(double[] arr, double item)
        {
            Array.Resize(ref arr, arr.Length + 1);
            arr[arr.Length - 1] = item;
            return arr;
        }
        /// <summary> Add an item to the end of an array. </summary>
        /// <param name="arr"> The array to be edited. </param>
        /// <param name="item"> The item to be added to the array. </param>
        public static Vector3[] AddToEnd(Vector3[] arr, Vector3 item)
        {
            Array.Resize(ref arr, arr.Length + 1);
            arr[arr.Length - 1] = item;
            return arr;
        }
        /// <summary> Add an item to the end of an array. </summary>
        /// <param name="arr"> The array to be edited. </param>
        /// <param name="item"> The item to be added to the array. </param>
        public static GameObject[] AddToEnd(GameObject[] arr, GameObject item)
        {
            Array.Resize(ref arr, arr.Length + 1);
            arr[arr.Length - 1] = item;
            return arr;
        }

        #endregion

        //Print---------------------------------------------------------

        //not done
        /// <summary> Prints an Array of Ints to the Debug.Console, in a readable Format. </summary>
        static void Print2DArrayInt(int[,] arr)
        {
            //taken from https://stackoverflow.com/questions/12826760/printing-2d-array-in-matrix-format by "markmuetz"
            int rowLength = arr.GetLength(0);
            int colLength = arr.GetLength(1);
            System.Diagnostics.Debug.WriteLine("\n" + "---Start---" + "\n '--' = '00'");
            for (int i = 0; i < rowLength; i++)
            {
                string row = "Row" + i.ToString("00") + ": ";
                for (int j = 0; j < colLength; j++)
                {
                    if (j == 0) { row += "(" + (arr[i, j] * 8).ToString("00") + ") " + "- "; }
                    else
                    {
                        string str = "";
                        if (arr[i, j] == 0) { str = " -- "; }
                        else
                        {
                            str = " " + ((int)(arr[i, j] - 1) / 3).ToString("00") + "|" + (arr[i, j] * 3).ToString("00") + " "; // "-(" + (arr[i, j]*8).ToString() + ") ";
                        }

                        row += str; //== " 00 " ? " -- " : str;
                    }
                }
                System.Diagnostics.Debug.WriteLine(row);
            }

            System.Diagnostics.Debug.WriteLine("---End---" + "\n");
        }
    }
}
