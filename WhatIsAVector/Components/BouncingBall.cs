using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace WhatIsAVector.Components
{
    public class BouncingBall : DrawableGameComponent
    {
        private bool _disposed;
        protected Vector2 _position;
        protected Vector2 _velocity;
        protected Vector2 _acceleration;
        protected Vector2 _weight;
        protected float _mu; // coefficient of friction
        protected Vector2 _gravity = new(0, 0.2f);
        protected readonly float _mass;
        protected Color _color;
        protected readonly float _radius;
        protected readonly float _screenWidth;
        protected readonly float _screenHeight;
        protected readonly Game _game;
        private SpriteBatch _spriteBatch;

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
            _mu = 0.01f;
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            var keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.Space))
            {
                ApplyForce(new Vector2(0.1f, 0)); // "wind" from left to right
            }

            ApplyForce(_weight);

            _velocity += _acceleration;
            _position += _velocity;

            // reset acceleration, once applyed
            _acceleration.X = 0;
            _acceleration.Y = 0;

            CheckEdges();
            base.Update(gameTime);
        }

        protected void ApplyForce(Vector2 force)
        {
            var forceDivByMass = Vector2.Divide(force, _mass);
            _acceleration += forceDivByMass;
        }

        private void ApplyFriction()
        {
            if (_position.Y >= _boundery.Bottom)
            {
                var friction = _velocity.Copy();
                friction.Normalize();
                friction *= -1;
                friction.SetMagnitude(_mu * _mass);
                ApplyForce(friction); // take a little bit off of velocity
            }
        }

        private void CheckEdges()
        {
            if (_position.Y >= _boundery.Bottom)
            {
                _position.Y = _boundery.Bottom; // reposition, just in case it went beyond the boundery before the check takes place
                _velocity.Y *= -1;
                ApplyFriction();
            }

            if (_position.Y <= _boundery.Top)
            {
                _position.Y = _boundery.Top; // reposition
                _velocity.Y *= -1;
                ApplyFriction();
            }

            if (_position.X >= _boundery.Right)
            {
                _position.X = _boundery.Right; // reposition
                _velocity.X *= -1;
                ApplyFriction();
            }

            if (_position.X <= _boundery.Left)
            {
                _position.X = _boundery.Left; // reposition
                _velocity.X *= -1;
                ApplyFriction();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();
            _spriteBatch.DrawCircle(_position, _radius, 20, _color, 1);
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
