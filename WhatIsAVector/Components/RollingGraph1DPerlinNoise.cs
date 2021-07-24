using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Noise;

namespace WhatIsAVector.Components
{
    public class RollingGraph1DPerlinNoise : DrawableGameComponent
    {
        private bool _disposed;
        private Vector2 _position;
        private Color _color;
        private readonly float _screenWidth;
        private readonly float _halfHeight;
        private SpriteBatch _spriteBatch;
        private readonly Game _game;
        private readonly Perlin _noise;
        private readonly List<Vector2> _points = new();
        private readonly float _inc = 0.01f;
        private float _start = 0f;

        public RollingGraph1DPerlinNoise(
            Game game,
            Color color,
            float screenWidth,
            float screenHeight,
            Perlin noise)
            : base(game)
        {
            _screenWidth = screenWidth;
            _halfHeight = screenHeight / 2f;
            _position = new Vector2(0, 0);
            _color = color;
            _game = game;
            _noise = noise;
            _spriteBatch = new SpriteBatch(_game.GraphicsDevice);
        }

        public override void Update(GameTime gameTime)
        {
            _points.Clear();
            var xoff = _start;

            for (int x = 0; x < _screenWidth; x++)
            {
                var y = (1 + (float)_noise.Noise(xoff, _inc)) * _halfHeight;
                _points.Add(new Vector2(x, y));
                xoff += _inc;
            }
            _start += _inc;

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();
            _spriteBatch.DrawPoints(_position, _points, _color, 1);
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                _spriteBatch?.Dispose();
                _spriteBatch = null;
                _disposed = true;
            }

            base.Dispose(disposing);
        }
    }
}

