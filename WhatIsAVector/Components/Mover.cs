using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WhatIsAVector.Components
{
    public class Mover : DrawableGameComponent
    {
        private bool _disposed;
        protected Vector2 _position;
        protected Color _color;
        protected readonly float _screenWidth;
        protected readonly float _screenHeight;
        private SpriteBatch _spriteBatch;
        protected float _radius;
        private Game _game;

        public Mover(
            Game game,
            Color color,
            Vector2 position,
            float radius,
            float screenWidth,
            float screenHeight)
            : base(game)
        {
            _game = game;
            _color = color;
            _position = position;
            _radius = radius;
            _screenWidth = screenWidth;
            _screenHeight = screenHeight;
            _spriteBatch = new SpriteBatch(_game.GraphicsDevice);
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();
            _spriteBatch.DrawCircle(_position, _radius, (int)_radius, _color);
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _disposed)
            {
                _spriteBatch.Dispose();
                _spriteBatch = null;
                _disposed = true;
            }

            base.Dispose(disposing);
        }
    }
}
