using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WhatIsAVector.Components
{
    public class AttractedTriangle : Mover
    {
        private bool _popped;
        private float _halfRadius;

        public AttractedTriangle(
            Game game,
            Color color,
            Vector2 position,
            float mass,
            float screenWidth,
            float screenHeight)
            : base(game, color, position, (float)Math.Sqrt(mass), mass, screenWidth, screenHeight)
        {
        }

        public override void Initialize()
        {
            _halfRadius = _radius / 2f;
            base.Initialize();
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
            _spriteBatch.Begin();

            var vS = new Vector2[3];
            vS[0] = new Vector2(_position.X - _radius, _position.Y - _halfRadius);
            vS[1] = new Vector2(_position.X - _radius, _position.Y + _halfRadius);
            vS[2] = new Vector2(_position.X + Radius, Position.Y);

            _spriteBatch.DrawLine(vS[0], vS[2], _color, 1f);
            _spriteBatch.DrawLine(vS[1], vS[2], _color, 1f);
            _spriteBatch.DrawLine(vS[0], vS[1], _color, 1f);

            _spriteBatch.End();

            //base.Draw(gameTime);
        }
    }
}