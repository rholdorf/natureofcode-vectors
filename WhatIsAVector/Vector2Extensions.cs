using System;
using Microsoft.Xna.Framework;

namespace WhatIsAVector
{
    public static class Vector2Extensions
    {
        public static void Clamp(ref this Vector2 vector, float ammount)
        {
            var newX = MathHelper.Clamp(vector.X, 0, ammount);
            var newY = MathHelper.Clamp(vector.Y, 0, ammount);
            vector.X = newX;
            vector.Y = newY;
        }

        public static void SetMagnitude(ref this Vector2 vector, float ammount)
        {
            // magnitude is the vector "length"
            vector.Normalize();
            vector *= ammount;
        }

        public static Vector2 Copy(this Vector2 vector)
        {
            return new Vector2(vector.X, vector.Y);
        }

        public static Vector2 Randomize(this Vector2 vector, Random random)
        {
            var angle = random.NextDouble() * (Math.PI * 2);
            vector.X = (float)Math.Cos(angle);
            vector.Y = (float)Math.Sin(angle);
            return vector;
        }

        /// <summary>
        /// Calculate the angle of rotation for this 2D vector.
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static float Heading(this Vector2 vector)
        {
            return (float)Math.Atan2(vector.Y, vector.X);
        }

        /// <summary>
        /// Rotate the 2d vector by an angle.
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="angle">Angle in radians.</param>
        public static void Rotate(this ref Vector2 vector, float angle)
        {
            var newHeading = vector.Heading() + angle;
            var mag = vector.Length();

            vector.X = (float)Math.Cos(newHeading) * mag;
            vector.Y = (float)Math.Sin(newHeading) * mag;
        }

    }
}
