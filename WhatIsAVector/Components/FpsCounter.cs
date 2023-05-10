using System;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WhatIsAVector.Components
{
    /// <summary>
    /// A game component that counts FPS and UPS, also gives other useful performance information.
    /// </summary>
    public sealed class FpsCounter : DrawableGameComponent
    {
        private bool _disposed;
        private const int SAMPLES_PER_SEC = 4;  // how many times do we calculate FPS & UPS every second
        private readonly TimeSpan _refreshTime = TimeSpan.FromMilliseconds(1000 / SAMPLES_PER_SEC);
        private TimeSpan _elapsedTime = TimeSpan.Zero;
        private int _fps;
        private int _ups;
        private int _oldFps;
        private int _oldUps;
        private int _frameCounter;
        private int _updateCounter;
        private SpriteBatch _spriteBatch;
        private readonly SpriteFont _font;
        private readonly Vector2 _position;
        private readonly Color _color;
        private readonly Color _shadow = new(0, 0, 0, 100);
        private readonly StringBuilder _outputSb = new();
        private string _fpsUps = string.Empty;

        public FpsCounter(Game game, SpriteFont font, Vector2 pos, Color color)
            : base(game)
        {
            _spriteBatch = new SpriteBatch(game.GraphicsDevice);
            _font = font;
            _position = pos;
            _color = color;
        }

        /// <summary>
        /// Allows performance monitor to calculate update rate.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            _elapsedTime += gameTime.ElapsedGameTime;
            _updateCounter++;

            if (_elapsedTime > _refreshTime)
            {
                _elapsedTime -= _refreshTime;
                _fps = _frameCounter * SAMPLES_PER_SEC;
                _ups = _updateCounter * SAMPLES_PER_SEC;
                _frameCounter = 0;
                _updateCounter = 0;

                if (_fps != _oldFps || _ups != _oldUps)
                {
                    _oldFps = _fps;
                    _oldUps = _ups;
                    _outputSb.Clear();
                    _outputSb.Append('F');
                    _outputSb.Append(_fps);
                    _outputSb.Append(" U");
                    _outputSb.Append(_ups);
                    _fpsUps = _outputSb.ToString();
                }
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// Allows performance monitor to calculate draw rate.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            _frameCounter++; // increment frame counter
            _spriteBatch.Begin();
            _spriteBatch.DrawString(_font, _fpsUps, _position + new Vector2(1, 1), _shadow); // shadow
            _spriteBatch.DrawString(_font, _fpsUps, _position, _color);
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