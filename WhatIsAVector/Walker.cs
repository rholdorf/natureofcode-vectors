using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace WhatIsAVector
{
    public class Walker
    {
        private Vector2 _position;
        private Vector2 _velocity;
        private Vector2 _acceleration;

        private readonly Random _random;


        public Walker(float x, float y, Random random)
        {
            _position = new Vector2(x, y);
            _random = random;
            _velocity = new Vector2(_random.Next(0, 5), _random.Next(0, 5));
            _velocity *= _random.Next(3);
            _acceleration = new Vector2(_random.Next(-5, 5), _random.Next(-5, 5));
        }

        public void Update(MouseState mouseState)
        {
            var mouse = new Vector2(mouseState.X, mouseState.Y);
            _acceleration = mouse - _position;
            _acceleration.SetMagnitude(1f);
            _velocity += _acceleration;
            _position += _velocity;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawCircle(_position, 32, 32, Color.White, 2);
        }
    }
}
