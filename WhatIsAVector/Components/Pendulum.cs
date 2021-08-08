using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WhatIsAVector.Components
{
    public class Pendulum : DrawableGameComponent
    {
        private bool _disposed;
        protected Vector2 _position;
        protected Color _color;
        protected readonly float _screenWidth;
        protected readonly float _screenHeight;
        protected SpriteBatch _spriteBatch;
        private Game _game;
        protected bool _destroyed;
        private Texture2D _texture;
        private float _angle = (float)Math.PI / 4;
        private float _angularVelocity;
        private float _angularAcceleration;
        private readonly float _gravity = 0.4f;
        private float _r; // arm lenght
        private Vector2 _origin;

        public Pendulum(
            Game game,
            Color color,
            Vector2 position,
            float screenWidth,
            float screenHeight)
            : base(game)
        {
            _game = game;
            _color = color;
            _position = position;
            _screenWidth = screenWidth;
            _screenHeight = screenHeight;
            _spriteBatch = new SpriteBatch(_game.GraphicsDevice);
            _origin = new Vector2(screenWidth / 2, 0);
            _r = screenHeight / 2;
        }

        public override void Initialize()
        {
            _texture = _game.Content.Load<Texture2D>("Shapes/circle_border");
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            _angularAcceleration = (-1f * _gravity / _r) * (float)Math.Sin(_angle); // calculate acceleration
            _angularVelocity += _angularAcceleration; // increment velocity
            _angle += _angularVelocity; // apply velocity to the angle

            // polar to cartesian conversion
            _position.X = _r * (float)Math.Sin(_angle); 
            _position.Y = _r * (float)Math.Cos(_angle);

            // make sure the position is relative to the pendulum's origin
            _position += _origin;

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();
            _spriteBatch.DrawLine(_origin, _position, _color);
            _spriteBatch.Draw(_texture, new Rectangle((int)_position.X-10, (int)_position.Y-10, 20, 20), _color);
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

                _game = null;
                _disposed = true;
            }

            base.Dispose(disposing);
        }
    }
}
