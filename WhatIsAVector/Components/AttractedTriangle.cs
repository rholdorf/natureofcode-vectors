using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WhatIsAVector.Components
{
    public class AttractedTriangle : Mover
    {
        private readonly Texture2D _texture;
        private Vector2 _centerPoint;
        private float _scale;

        public AttractedTriangle(
            Game game,
            Color color,
            Vector2 position,
            float mass,
            float screenWidth,
            float screenHeight,
            Texture2D texture)
            : base(game, color, position, (float)Math.Sqrt(mass), mass, screenWidth, screenHeight)
        {
            _texture = texture;
        }

        public override void Initialize()
        {
            _centerPoint = new Vector2(_texture.Width / 2, _texture.Height / 2);
            _scale =  2f/_radius;
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
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();
            _spriteBatch.Draw(_texture, _position, null, _color, _angle, _centerPoint, _scale, SpriteEffects.None, 1);
            _spriteBatch.End();
        }
    }
}