using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WhatIsAVector.Components
{
    public class MovingCircleNoise : DrawableGameComponent
    {
        private Vector2 _position;
        private Color _color;
        private float _screenWidth;
        private float _screenHeight;
        private readonly SpriteBatch _spriteBatch;
        private readonly Game _game;

        private float _offset1;
        private float _offset2 = 1000;
        protected Perlin _perlin = new Perlin();



        public MovingCircleNoise(Game game, Color color, float screenWidth, float screenHeight)
            : base(game)
        {
            _screenWidth = screenWidth;
            _screenHeight = screenHeight;
            _position = new Vector2(_screenWidth / 2, _screenHeight / 2);
            _color = color;
            _game = game;
            _spriteBatch = new SpriteBatch(_game.GraphicsDevice);
        }

        //public override void Initialize()
        //{
        //    base.Initialize();
        //}

        public override void Update(GameTime gameTime)
        {
            _position.X = ((float)_perlin.Noise(_offset1)).Map(-1, 1, 0, _screenWidth);
            _position.Y = ((float)_perlin.Noise(_offset2)).Map(-1, 1, 0, _screenHeight);
            _offset1 += 0.02f;
            _offset2 += 0.02f;

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
