using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WhatIsAVector.Components
{
    class SpinningRectangle : DrawableGameComponent
    {
        private SpriteBatch _spriteBatch;
        private bool _disposed;
        private Color _color;
        private readonly float _screenWidth;
        private readonly float _screenHeight;
        private Vector2 _position;
        private Texture2D _texture;
        private Matrix _translationMatrix;
        private float _rotation;
        private Vector2 _anchorPoint;

        public SpinningRectangle(
            Game game,
            Color color,
            Rectangle rectangle,
            float screenWidth,
            float screenHeight)
            : base(game)
        {
            _spriteBatch = new SpriteBatch(game.GraphicsDevice);
            _color = color;
            _screenWidth = screenWidth;
            _screenHeight = screenHeight;
            _position = new Vector2(rectangle.X, rectangle.Y);
            _texture = new Texture2D(game.GraphicsDevice, rectangle.Width, rectangle.Height);
            _anchorPoint = new Vector2(rectangle.Width / 2f, rectangle.Height / 2f);
        }

        public override void Initialize()
        {
            var colorData = new Color[_texture.Width * _texture.Height];
            for (int i = 0; i < colorData.Length; i++)
                colorData[i] = _color;
            _texture.SetData(colorData);

            _translationMatrix = Matrix.CreateTranslation(_screenWidth / 2, _screenHeight / 2, 0f);
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            _rotation += 0.01f;
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin(transformMatrix: _translationMatrix);
            _spriteBatch.Draw(_texture, _position, null, Color.White, _rotation, _anchorPoint, 1, SpriteEffects.None, 1);
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
