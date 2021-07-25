using System;
using Microsoft.Xna.Framework;

namespace WhatIsAVector.Components
{
    public class Attracted : Mover
    {
        public Attracted(
            Game game,
            Color color,
            Vector2 position,
            float mass,
            float screenWidth,
            float screenHeight)
            : base(game, color, position, (float)Math.Sqrt(mass), mass, screenWidth, screenHeight)
        {
        }

        public override void Update(GameTime gameTime)
        {
            _velocity += _acceleration;
            _position += _velocity;
            // reset acceleration, once applyed
            _acceleration.X = 0;
            _acceleration.Y = 0;

            base.Update(gameTime);
        }
    }
}
