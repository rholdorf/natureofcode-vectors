using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Noise;

namespace WhatIsAVector.Components
{
    public class MovingCircle1DPerlinNoise : DrawableGameComponent
    {
        private bool _disposed;
        private Vector2 _position;
        private Color _color;
        private float _screenWidth;
        private float _screenHeight;
        private SpriteBatch _spriteBatch;
        private readonly Game _game;
        private float _offset1;
        private float _offset2;
        private Perlin _noise;

        public MovingCircle1DPerlinNoise(
            Game game,
            Color color,
            float screenWidth,
            float screenHeight,
            Perlin noise,
            Random random)
            : base(game)
        {
            _screenWidth = screenWidth;
            _screenHeight = screenHeight;
            _position = new Vector2(_screenWidth / 2, _screenHeight / 2);
            _color = color;
            _game = game;
            _noise = noise;
            _spriteBatch = new SpriteBatch(_game.GraphicsDevice);
            _offset1 = (float)random.NextDouble() * 500;
            _offset2 = (float)random.NextDouble() * 1000;
        }

        public override void Update(GameTime gameTime)
        {
            _position.X = (1 + _noise.Noise(_offset1)) * _screenWidth / 2;
            _position.Y = (1 + _noise.Noise(_offset2)) * _screenHeight / 2;
            _offset1 += 0.015f;
            _offset2 += 0.015f;

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();
            _spriteBatch.DrawCircle(_position, 20, 20, _color);
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _disposed)
            {
                _spriteBatch?.Dispose();
                _spriteBatch = null;
                _disposed = true;
            }

            base.Dispose(disposing);
        }
    }
}
