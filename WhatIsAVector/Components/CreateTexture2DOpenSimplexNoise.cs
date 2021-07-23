using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Noise;

namespace WhatIsAVector.Components

{
    public class CreateTexture2DOpenSimplexNoise : DrawableGameComponent
    {
        private bool _disposed;
        private Vector2 _position;
        private Color _color;
        private SpriteBatch _spriteBatch;
        private readonly Game _game;
        private OpenSimplex2F _noise;
        private float _inc = 0.01f;
        private Texture2D _texture;
        private float zoff = 0f;

        public CreateTexture2DOpenSimplexNoise(
            Game game,
            Vector2 position,
            Color color,
            int width,
            int height,
            OpenSimplex2F noise)
            : base(game)
        {
            _position = position;
            _color = color;
            _game = game;
            _noise = noise;
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
                    var noise = _noise.Noise3_XYBeforeZ(xoff, yoff, zoff);
                    var r = (byte)((noise + 1f) * 128f);
                    colorData[index++] = r; // red
                    colorData[index++] = r; // green
                    colorData[index++] = r; // blue
                    colorData[index++] = 255; // alpha
                    xoff += _inc;
                }
                yoff += _inc;
            }

            zoff += _inc;
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

        protected override void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
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
