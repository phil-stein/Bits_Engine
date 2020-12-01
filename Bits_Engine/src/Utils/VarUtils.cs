using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Numerics;
using System.Text;

namespace BitsCore.Utils
{
    public static class VarUtils
    {
        #region VAR_TO_BYTES
        //conversion---------------
        /// <summary> Converts the given int to  an array of bytes. </summary>
        /// <param name="var"> The int to be converted. </param>
        public static byte[] VarToBytes(int var)
        {
            return BitConverter.GetBytes(var);
        }
        /// <summary> Converts the given uint to  an array of bytes. </summary>
        /// <param name="var"> The uint to be converted. </param>
        public static byte[] VarToBytes(uint var)
        {
            return BitConverter.GetBytes(var);
        }
        /// <summary> Converts the given float to  an array of bytes. </summary>
        /// <param name="var"> The float to be converted. </param>
        public static byte[] VarToBytes(float var)
        {
            return BitConverter.GetBytes(var);
        }
        /// <summary> Converts the given string to  an array of bytes. </summary>
        /// <param name="var"> The string to be converted. </param>
        public static byte[] VarToBytes(string var)
        {
            Encoding encoding = Encoding.Unicode;
            return encoding.GetBytes(var);
        }
        /// <summary> Converts the given char to a two byte array. </summary>
        /// <param name="var"> The char to be converted. </param>
        public static byte[] VarToBytes(char var)
        {
            return BitConverter.GetBytes(var);
        }
        /// <summary> Converts the given Vector3 to  an array of bytes. </summary>
        /// <param name="var"> The Vector3 to be converted. </param>
        public static byte[] VarToBytes(Vector3 var)
        {
            List<byte> bytes = new List<byte>();
            bytes.AddRange(BitConverter.GetBytes(var.X));
            bytes.AddRange(BitConverter.GetBytes(var.Y));
            bytes.AddRange(BitConverter.GetBytes(var.Z));

            return bytes.ToArray();
        }
        /// <summary> Converts the given Vector2 to  an array of bytes. </summary>
        /// <param name="var"> The Vector2 to be converted. </param>
        public static byte[] VarToBytes(Vector2 var)
        {
            List<byte> bytes = new List<byte>();
            bytes.AddRange(BitConverter.GetBytes(var.X));
            bytes.AddRange(BitConverter.GetBytes(var.Y));

            return bytes.ToArray();
        }
        #endregion

        #region BYTES_TO_VAR
        /// <summary> Converts the given array of bytes to an int. </summary>
        /// <param name="bytes"> The byte-array to be converted. </param>
        public static int BytesToInt(byte[] bytes)
        {
            return BitConverter.ToInt32(bytes);
        }
        /// <summary> Converts the given array of bytes to a uint. </summary>
        /// <param name="bytes"> The byte-array to be converted. </param>
        public static uint BytesToUint(byte[] bytes)
        {
            return BitConverter.ToUInt32(bytes);
        }
        /// <summary> Converts the given array of bytes to a float. </summary>
        /// <param name="bytes"> The byte-array to be converted. </param>
        public static float BytesToFloat(byte[] bytes)
        {
            return BitConverter.ToSingle(bytes);
        }
        /// <summary> Converts the given array of bytes to a string. </summary>
        /// <param name="bytes"> The byte-array to be converted. </param>
        public static string BytesToString(byte[] bytes)
        {
            Encoding encoding = Encoding.Unicode;
            return encoding.GetString(bytes);
        }
        /// <summary> Converts the given two byte array to a char. </summary>
        /// <param name="bytes"> The bytes to be converted. </param>
        public static char VarToBytes(byte[] bytes)
        {
            System.Diagnostics.Debug.WriteLineIf(bytes.Length > 2, "!!! Byte array pasdsed to VarUtils.VarToBytes(char) too long !!!");
            return BitConverter.ToChar(bytes);
        }
        /// <summary> Converts the given array of bytes to a Vector3. </summary>
        /// <param name="bytes"> The byte-array to be converted. </param>
        public static Vector3 BytesToVector3(byte[] bytes)
        {
            if(bytes.Length < sizeof(float) * 3) { System.Diagnostics.Debug.WriteLine("!!! Bytes passed to BytesToVector3() aren't long enough for 3 floats !!!"); return Vector3.Zero; }

            //extract the 3 floats, that make up the vector, from the byte array
            byte[] x = ArrayUtils.GetFirstX(bytes, sizeof(float));
            byte[] y = ArrayUtils.GetBetweenXAndY(bytes, sizeof(float), sizeof(float) * 2);
            byte[] z = ArrayUtils.GetLastX(bytes, sizeof(float));
 
            return new Vector3(BitConverter.ToSingle(x), BitConverter.ToSingle(y), BitConverter.ToSingle(z));
        }
        /// <summary> Converts the given array of bytes to a Vector2. </summary>
        /// <param name="bytes"> The byte-array to be converted. </param>
        public static Vector2 BytesToVector2(byte[] bytes)
        {
            if (bytes.Length < sizeof(float) * 2) { System.Diagnostics.Debug.WriteLine("!!! Bytes passed to BytesToVector2() aren't long enough for 2 floats !!!"); return Vector2.Zero; }

            //extract the 3 floats, that make up the vector, from the byte array
            byte[] x = ArrayUtils.GetFirstX(bytes, sizeof(float));
            byte[] y = ArrayUtils.GetLastX(bytes, sizeof(float));

            return new Vector2(BitConverter.ToSingle(x), BitConverter.ToSingle(y));
        }
        #endregion

        //string---------------
        /// <summary> The the rest of a string after the key. </summary>
        /// <param name="str"> The sting to be checked. </param>
        /// <param name="key"> The key to be checked for. </param>
        public static string GetStringAfterKey(string str, string key)
        {
            //get the index of the passed key-word
            int index = str.IndexOf(key);
            if (index != -1)
            {
                return str.Substring(index + key.Length);
            }
            //if the index is -1 that means the passed key was not contained in the str
            Debug.WriteLine("!!! Passed string did not contain the key-word !!!");
            return "";
        }

        /// <summary> Returns the float contained in the string. </summary>
        /// <param name="floatStr"> The string to ve converted. </param>
        public static float StringToFloat(string floatStr)
        {
            try
            {
                return float.Parse(floatStr, CultureInfo.InvariantCulture.NumberFormat);
            }
            catch(Exception e) { throw e; }
        }

        /// <summary> Returns the int contained in the string. </summary>
        /// <param name="intStr"> The string to ve converted. </param>
        public static int StringToInt(string intStr)
        {
            try
            {
                return int.Parse(intStr, CultureInfo.InvariantCulture.NumberFormat);
            }
            catch (Exception e) { throw e; }
        }

    }
}
