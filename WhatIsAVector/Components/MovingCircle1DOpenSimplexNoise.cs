using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Noise;

namespace WhatIsAVector
{
    public class MovingCircle1DOpenSimplexNoise : DrawableGameComponent
    {
        private Vector2 _position;
        private Color _color;
        private float _screenWidth;
        private float _screenHeight;
        private readonly SpriteBatch _spriteBatch;
        private readonly Game _game;
        private float _offset1;
        private float _offset2;
        private OpenSimplex2F _noise;

        public MovingCircle1DOpenSimplexNoise(
            Game game,
            Color color,
            float screenWidth,
            float screenHeight,
            OpenSimplex2F noise,
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

        //public override void Initialize()
        //{
        //    base.Initialize();
        //}

        public override void Update(GameTime gameTime)
        {
            _position.X = ((float)_noise.Noise2(_offset1, 0.5d)).Map(-1, 1, 0, _screenWidth);
            _position.Y = ((float)_noise.Noise2(_offset2, 0.5d)).Map(-1, 1, 0, _screenHeight);
            _offset1 += 0.005f;
            _offset2 += 0.005f;

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();
            _spriteBatch.DrawCircle(_position, 20, 20, _color);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
