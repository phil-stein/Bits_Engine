using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Text;

namespace BitsCore.Utils
{
    /// <summary> 
    /// Collection of functions returning random values. 
    /// <para>RNG = Random Number Generator. </para>
    /// </summary>
    public static class RNG
    {
        #region RAND_FLOAT
        /// <summary> Returns a float between 0[Inclusive] and 1[Inclusive]. </summary>
        public static float ZeroToOne()
        {
            Random random = new Random();
            float num = (float)random.NextDouble();
            num += num > 0.99999f ? Single.Epsilon : 0f; //if the generated number is above a threshold, add the smallest positive number a float can represet, so the result is inclusive of 1.0f. 
            return num;
        }

        /// <summary> Returns a float between 0[Inclusive] and 'max'[Inclusive]. </summary>
        /// <param name="max"> The max. value of the returned float. [Inclusive] </param>
        public static float ZeroToMax(float max)
        {

            return ZeroToOne() * max;
        }

        /// <summary> Returns a float between 'min'[Inclusive] and 'max'[Inclusive]. </summary>
        /// <param name="max"> The max. value of the returned float. [Inclusive] </param>
        /// <param name="min"> The min. value of the returned float. [Inclusive] </param>
        public static float MinToMax(float min, float max)
        {
            return (ZeroToOne() * (max*2f)) - max;
        }
        #endregion

        #region RAND_DOUBLE
        /// <summary> Returns a double between 0[Inclusive] and 1[Inclusive]. </summary>
        public static double ZeroToOneDouble()
        {
            Random random = new Random();
            double num = random.NextDouble();
            num += num > 0.99999f ? Double.Epsilon : 0f; //if the generated number is above a threshold, add the smallest positive number a double can represet, so the result is inclusive of 1.0f. 
            return num;
        }
        /// <summary> Returns a double between 0[Inclusive] and 'max'[Inclusive]. </summary>
        /// <param name="max"> The max. value of the returned double. [Inclusive] </param>
        public static double ZeroToMaxDouble(double max)
        {            
            return ZeroToOneDouble() * max;
        }

        /// <summary> Returns a double between 'min'[Inclusive] and 'max'[Inclusive]. </summary>
        /// <param name="max"> The max. value of the returned double. [Inclusive] </param>
        /// <param name="min"> The min. value of the returned double. [Inclusive] </param>
        public static double MinToMaxDouble(double min, double max)
        {
            return (ZeroToOneDouble() * (max * 2f)) - max;
        }
        #endregion

        #region RAND_VECTOR3
        /// <summary> Returns a Vector3, with X,Y,Z values between 0[Inclusive] and 1[Inclusive]. </summary>
        public static Vector3 ZeroToOneVector()
        {
            return new Vector3(ZeroToOne(), ZeroToOne(), ZeroToOne());
        }

        /// <summary> Returns a Vector3, with X,Y,Z values between 0[Inclusive] and 'max'[Inclusive]. </summary>
        /// <param name="max"> The max. value of the returned Vector3s X,Y,Z values. [Inclusive] </param>
        public static Vector3 ZeroToMaxVector(float max)
        {
            return new Vector3(ZeroToMax(max), ZeroToMax(max), ZeroToMax(max));
        }

        /// <summary> Returns a Vector3, with X,Y,Z values between 'min'[Inclusive] and 'max'[Inclusive]. </summary>
        /// <param name="max"> The max. value of the returned Vector3s X,Y,Z values. [Inclusive] </param>
        /// <param name="min"> The min. value of the returned Vector3s X,Y,Z values. [Inclusive] </param>
        public static Vector3 MinToMaxVector(float min, float max)
        {
            return new Vector3(MinToMax(min, max), MinToMax(min, max), MinToMax(min, max));
        }

        /// <summary> Returns a Vector3, with X,Y,Z values between '0'[Inclusive] and 'maxY,-Y,-Z'[Inclusive]. </summary>
        /// <param name="maxX"> The max. value of the returned Vector3s X value. [Inclusive] </param>
        /// <param name="maxY"> The max. value of the returned Vector3s Y value. [Inclusive] </param>
        /// <param name="maxZ"> The max. value of the returned Vector3s Z value. [Inclusive] </param>
        public static Vector3 ZeroToVecMaxVector(float maxX, float maxY, float maxZ)
        {
            return new Vector3(ZeroToMax(maxX), ZeroToMax(maxY), ZeroToMax(maxZ));
        }

        /// <summary> Returns a Vector3, with X,Y,Z values between '0'[Inclusive] and 'maxValues'[Inclusive]. </summary>
        /// <param name="maxValues"> The max. values of the returned Vector3s X,Y,Z values. [Inclusive] </param>
        public static Vector3 ZeroToVecMaxVector(Vector3 maxValues)
        {
            return new Vector3(ZeroToMax(maxValues.X), ZeroToMax(maxValues.Y), ZeroToMax(maxValues.Z));
        }

        /// <summary> Returns a Vector3, with X,Y,Z values between 'minValues'[Inclusive] and 'maxValues'[Inclusive]. </summary>
        /// <param name="minValues"> The min. values of the returned Vector3s X,Y,Z values. [Inclusive] </param>
        /// <param name="maxValues"> The max. values of the returned Vector3s X,Y,Z values. [Inclusive] </param>
        public static Vector3 VecMinToVecMaxVector(Vector3 minValues, Vector3 maxValues)
        {
            return new Vector3(MinToMax(minValues.X, maxValues.X), MinToMax(minValues.Y, maxValues.Y), MinToMax(minValues.Z, maxValues.Z));
        }
        #endregion

        #region RAND_MISC
        
        /// <summary> Returns a Boolean, with a randomly picked value [true or false]. </summary>
        public static bool Bool()
        {
            return ZeroToOne() > 0.5f;
        }

        #endregion
    }
}
