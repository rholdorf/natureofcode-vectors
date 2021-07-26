using System;
using Microsoft.Xna.Framework;

namespace WhatIsAVector.Components
{
    public class Attracted : Mover
    {
        private bool _popped;

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

        public bool Popped { get { return _popped; } set { _popped = value; } }

        public override void Update(GameTime gameTime)
        {
            if (_popped)
                return;

            _velocity += _acceleration;
            _position += _velocity;
            // reset acceleration, once applyed
            _acceleration.X = 0;
            _acceleration.Y = 0;

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (_popped)
                return;

            base.Draw(gameTime);
        }
    }
}
