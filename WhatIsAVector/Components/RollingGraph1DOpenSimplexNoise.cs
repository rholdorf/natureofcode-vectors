using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Noise;

namespace WhatIsAVector.Components
{
    public class RollingGraph1DOpenSimplexNoise : DrawableGameComponent
    {
        private bool _disposed;
        private Vector2 _position;
        private Color _color;
        private float _screenWidth;
        private float _halfHeight;
        private SpriteBatch _spriteBatch;
        private readonly Game _game;
        private OpenSimplex2F _noise;
        private List<Vector2> _points = new List<Vector2>();
        private float _inc = 0.005f;
        private float _start = 0f;

        public RollingGraph1DOpenSimplexNoise(
            Game game,
            Color color,
            float screenWidth,
            float screenHeight,
            OpenSimplex2F noise)
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
                var y = (1 + (float)_noise.Noise2(xoff, _inc)) * _halfHeight;
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
                _spriteBatch.Dispose();
                _spriteBatch = null;
                _disposed = true;
            }

            base.Dispose(disposing);
        }
    }
}
