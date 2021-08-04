using System;
namespace WhatIsAVector
{
    public static class FloatExtensions
    {
        /// <summary>
        /// If the value is less than the min, then set the value to the min.
        /// If the value is greater than the max, then set the value to the max.
        /// Otherwise the value remains the same.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="low"></param>
        /// <param name="high"></param>
        public static float Clamp(ref this float value, float low, float high)
        {
            if (value < low)
            {
                value = low;
                return low;
            }

            if (value > high)
            {
                value = high;
                return high;
            }

            // otherwise value remains the same
            return value;
        }

        /// <summary>
        /// Re-maps a number from one range to another.
        /// </summary>
        /// <param name="n">The value to correlate, must be in the range of minFrom and maxFrom.</param>
        /// <param name="start1">The inclusive lower bound of the "from" range. Must be less than the maxFrom.</param>
        /// <param name="stop1">The inclusive upper bound of the "from" range. Must be grater than the minFrom.</param>
        /// <param name="start2">The inclusive lower bound of the "to" range. Must be less than the maxTo.</param>
        /// <param name="stop2">The inclusive upper bound of the "to" range. Must be greater than the minTo.</param>
        /// <returns></returns>
        public static float Map(this float n, float start1, float stop1, float start2, float stop2, bool withinBounds = false)
        {
            var a = n - start1;
            var b = stop1 - start1;
            var c = stop2 - start2;
            var d = start2;

            var ret = (a / b * c) + d;
            if (!withinBounds)
                return ret;

            if (start2 < stop2)
                return Constrain(ret, start2, stop2);

            return Constrain(ret, stop2, start2);
        }

        public static float Constrain(this float value, float low, float high)
        {
            return Math.Max(Math.Min(value, high), low);
        }
    }
}
