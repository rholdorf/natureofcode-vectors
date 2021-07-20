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
        /// <param name="min"></param>
        /// <param name="max"></param>
        public static void Clamp(ref this float value, float min, float max)
        {
            if (min > max)
                throw new ArgumentOutOfRangeException(nameof(min), "min value must be less than max");

            if (value < min)
            {
                value = min;
                return;
            }

            if (value > max)
            {
                value = max;
                return;
            }

            // otherwise value remains the same
        }
    }
}
