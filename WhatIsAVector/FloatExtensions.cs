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

        /// <summary>
        /// Returns a correlated value to the current value. According to the
        /// specified minFrom and maxFrom it will determine a corresponding
        /// value in the range of the minTo up to maxTo.
        /// </summary>
        /// <param name="value">The value to correlate, must be in the range of minFrom and maxFrom.</param>
        /// <param name="minFrom">The inclusive lower bound of the "from" range. Must be less than the maxFrom.</param>
        /// <param name="maxFrom">The inclusive upper bound of the "from" range. Must be grater than the minFrom.</param>
        /// <param name="minTo">The inclusive lower bound of the "to" range. Must be less than the maxTo.</param>
        /// <param name="maxTo">The inclusive upper bound of the "to" range. Must be greater than the minTo.</param>
        /// <returns></returns>
        public static float Map(this float value, float minFrom, float maxFrom, float minTo, float maxTo)
        {
            if (value < minFrom || value > maxFrom)
                throw new ArgumentOutOfRangeException(nameof(value), "The " + nameof(value) + " must be greater than or equals to " + nameof(minFrom) + " and less than or equals to " + nameof(maxFrom) + ".");

            if (maxFrom < minFrom)
                throw new ArgumentOutOfRangeException(nameof(minFrom), "must be less than " + nameof(maxFrom) + ".");

            if (maxTo < minTo)
                throw new ArgumentOutOfRangeException(nameof(minTo), "must be less than " + nameof(maxTo) + ".");

            var fromHowMany = maxFrom - minFrom;
            var middle = fromHowMany / 2f;
            var offset = middle + value;
            var percent = offset / fromHowMany;
            var toHowMany = maxTo - minTo;
            var ret = toHowMany * percent;
            ret.Clamp(minTo, maxTo);
            return ret;
        }
    }
}
