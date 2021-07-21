using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WhatIsAVector.Components
{
    public class RollingGraphNoise : DrawableGameComponent
    {
        private Vector2 _position;
        private Color _color;
        private float _screenWidth;
        private float _halfHeight;
        private readonly SpriteBatch _spriteBatch;
        private readonly Game _game;
        private Perlin _perlin;
        private List<Vector2> _points = new List<Vector2>();
        private float _inc = 0.01f;
        private float _start = 0f;


        public RollingGraphNoise(
            Game game,
            Color color,
            float screenWidth,
            float screenHeight,
            Perlin perlin)
            : base(game)
        {
            _screenWidth = screenWidth;
            _halfHeight = screenHeight / 2f;
            _position = new Vector2(0, 0);
            _color = color;
            _game = game;
            _perlin = perlin;
            _spriteBatch = new SpriteBatch(_game.GraphicsDevice);
        }

        public override void Update(GameTime gameTime)
        {
            _points.Clear();
            var xoff = _start;

            for (int x = 0; x < _screenWidth; x++)
            {
                var y = _halfHeight + ((float)_perlin.Noise(xoff) * _halfHeight);
                _points.Add(new Vector2(x, y));
                xoff += 0.01f;
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
    }
}
