using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WhatIsAVector.Components
{
    public class AttractedTriangle : Mover
    {
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

        public override void Update(GameTime gameTime)
        {
            _velocity += _acceleration;
            _position += _velocity;
            // reset acceleration, once applyed
            _acceleration.X = 0;
            _acceleration.Y = 0;

            _angle = _velocity.Heading();
            //_angle += 0.00f;

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();
            //Matrix translationMatrix = Matrix.CreateTranslation(_position.X, _position.Y, 0f);
            //_spriteBatch.Begin(transformMatrix: translationMatrix);

            var foo = _position.Copy();
            //foo.Rotate(_angle);


            var vS = new Vector2[3];
            vS[0] = new Vector2(foo.X - _radius, foo.Y - _halfRadius);
            vS[1] = new Vector2(foo.X - _radius, foo.Y + _halfRadius);
            vS[2] = new Vector2(foo.X + Radius, foo.Y);


            vS[0].Rotate(_angle);
            vS[1].Rotate(_angle);
            vS[2].Rotate(_angle);

            _spriteBatch.DrawLine(vS[0], vS[2], _color, 1f);
            _spriteBatch.DrawLine(vS[1], vS[2], _color, 1f);
            _spriteBatch.DrawLine(vS[0], vS[1], _color, 1f);

            _spriteBatch.End();

            //base.Draw(gameTime);
        }


    }
}