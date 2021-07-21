using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WhatIsAVector.Components
{
    public class CreateTexture2DPerlinNoise : DrawableGameComponent
    {
        private Vector2 _position;
        private Color _color;
        private readonly SpriteBatch _spriteBatch;
        private readonly Game _game;
        private Perlin _perlin;
        private float _inc = 0.01f;
        private Texture2D _texture;

        public CreateTexture2DPerlinNoise(
            Game game,
            Color color,
            int width,
            int height,
            Perlin perlin)
            : base(game)
        {
            _position = new Vector2(0, 0);
            _color = color;
            _game = game;
            _perlin = perlin;
            _spriteBatch = new SpriteBatch(_game.GraphicsDevice);
            _texture = new Texture2D(GraphicsDevice, width, height);
            FillTexture();
        }

        private void FillTexture()
        {
            var yoff = 0f;
            var index = 0;
            var colorData = new byte[_texture.Width * _texture.Height * 4];
            for (int y = 0; y < _texture.Height; y++)
            {
                var xoff = 0f;
                for (int x = 0; x < _texture.Width; x++)
                {
                    var r = _perlin.NoiseByte(xoff, yoff);
                    colorData[index++] = r; // red
                    colorData[index++] = r; // green
                    colorData[index++] = r; // blue
                    colorData[index++] = 255; // alpha
                    xoff += _inc;
                }
                yoff += _inc;
            }
            _texture.SetData(colorData);
        }

        //public override void Update(GameTime gameTime)
        //{
        //    base.Update(gameTime);
        //}

        public override void Draw(GameTime gameTime)
        {
            FillTexture(); // too slow to call it in the Update method, unless add logic to only draw every x calls
            _spriteBatch.Begin();
            _spriteBatch.Draw(_texture, _position, _color);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
