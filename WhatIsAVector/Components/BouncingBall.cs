using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WhatIsAVector.Components
{
    public class BouncingBall : DrawableGameComponent
    {
        private Vector2 _position;
        private Vector2 _velocity;
        private Vector2 _acceleration;
        private Vector2 _weight;
        private float _mass;
        private Color _color;
        private float _screenWidth;
        private float _screenHeight;
        private SpriteBatch _spriteBatch;
        private readonly Game _game;
        private Vector2 _gravity = new(0, 0.2f);
        private float _radius;

        private Rectangle _boundery;

        public BouncingBall(
            Game game,
            Vector2 position,
            Color color,
            float radius,
            float mass,
            float screenWidth,
            float screenHeight)
            : base(game)
        {
            _game = game;
            _position = position;
            _color = color;
            _radius = radius;
            _mass = mass;
            _screenWidth = screenWidth;
            _screenHeight = screenHeight;
        }

        public override void Initialize()
        {
            var circleBoundingBox = (int)_radius;
            // define a boundery to facilitate calculations, taking into account that the position is in the center of the circle
            _boundery = new Rectangle(
                circleBoundingBox,
                circleBoundingBox,
                (int)_screenWidth - circleBoundingBox * 2, // discount left and right
                (int)_screenHeight - circleBoundingBox * 2); // discount top and bottom
            _velocity = new Vector2(0, 0);
            _acceleration = new Vector2(0, 0);
            _weight = Vector2.Multiply(_gravity, _mass);
            _spriteBatch = new SpriteBatch(_game.GraphicsDevice);

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            ApplyForce(_weight);

            _velocity += _acceleration;
            _position += _velocity;

            // reset acceleration, once applyed
            _acceleration.X = 0;
            _acceleration.Y = 0;

            CheckEdges();
            base.Update(gameTime);
        }

        private void ApplyForce(Vector2 force)
        {
            var forceDivByMass = Vector2.Divide(force, _mass);
            _acceleration += forceDivByMass;
        }

        private void CheckEdges()
        {
            if (_position.Y >= _boundery.Bottom)
            {
                _position.Y = _boundery.Bottom - (_position.Y - _boundery.Bottom); // reposition, just in case it went beyond the boundery before the check takes place
                _velocity.Y *= -1;
            }

            if (_position.Y <= _boundery.Top)
            {
                _position.Y = _boundery.Top + (_boundery.Top - _position.Y); // reposition
                _velocity.Y *= -1;
            }

            if (_position.X >= _boundery.Right)
            {
                _position.X = _boundery.Right - (_position.X - _boundery.Right); // reposition
                _velocity.X *= -1;
            }

            if (_position.X <= _boundery.Left)
            {
                _position.X = _boundery.Left + (_boundery.Left - _position.X); // reposition
                _velocity.X *= -1;
            }
        }



        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();
            _spriteBatch.DrawCircle(_position, _radius, 20, _color, 1);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
