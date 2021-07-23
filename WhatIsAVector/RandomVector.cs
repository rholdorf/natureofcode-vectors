using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WhatIsAVector
{
    public class RandomVector
    {
        private Vector2 _vector;
        private readonly Random _random;

        public RandomVector()
        {
            _random = new Random(0);
            _vector = new Vector2();
        }

        public void Update()
        {
            _vector.X = _random.Next(-100, 101);
            _vector.Y = _random.Next(-100, 101);

            _vector.Normalize();
            _vector *= _random.Next(50, 101);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawLine(0, 0, _vector.X, _vector.Y, Color.Wheat, 4);
        }
    }
}
