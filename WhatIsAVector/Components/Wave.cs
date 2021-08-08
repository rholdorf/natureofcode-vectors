using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WhatIsAVector.Components
{
    public class Wave : DrawableGameComponent
    {
        private bool _disposed;
        private float _amplitude;
        private float _period;
        private float _phase;
        private float _tau = (float)Math.PI * 2;
        private readonly float _screenWidth;
        private readonly float _screenHeight;
        private float _halfHeight;
        private SpriteBatch _spriteBatch;
        private Color _color;
        private Texture2D _texture;
        private Game _game;


        public Wave(Game game, float amplitude, float period, float phase, float screenWidth, float screenHeight, Color color)
            : base(game)
        {
            _game = game;
            _amplitude = amplitude;
            _period = period;
            _phase = phase;
            _spriteBatch = new SpriteBatch(game.GraphicsDevice);
            _screenWidth = screenWidth;
            _screenHeight = screenHeight;
            _halfHeight = _screenHeight / 2;
            _color = color;
        }

        public override void Initialize()
        {
            _texture = _game.Content.Load<Texture2D>("Shapes/circle_border");
            base.Initialize();
        }

        private float Calculate(float x)
        {
            return (float)Math.Sin(_phase + _tau * x / _period) * _amplitude;
        }

        public override void Update(GameTime gameTime)
        {
            _phase += 0.1f;
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();
            for (int x = 0; x < _screenWidth; x += 10)
            {
                float y = Calculate(x) + _halfHeight;
                _spriteBatch.Draw(_texture, new Rectangle(x, (int)y, 20, 20), _color);
            }
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _disposed)
            {
                _spriteBatch?.Dispose();
                _spriteBatch = null;
                _texture?.Dispose();
                _texture = null;

                _disposed = true;
            }

            base.Dispose(disposing);
        }

    }
}
