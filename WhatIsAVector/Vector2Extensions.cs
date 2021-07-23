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
            vector.Normalize();
            vector *= ammount;
        }

        public static Vector2 Copy(this Vector2 vector)
        {
            return new Vector2(vector.X, vector.Y);
        }

    }
}
