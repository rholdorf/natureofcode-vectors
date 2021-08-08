using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WhatIsAVector.Components
{
    public class Mover : DrawableGameComponent, IMover
    {
        protected Vector2 _velocity = new();
        protected Vector2 _acceleration = new();
        protected Vector2 _weight = new();
        protected float _mu; // coefficient of friction
        protected float _angle = 0;

        private bool _disposed;
        protected Vector2 _position;
        protected Color _color;
        protected readonly float _screenWidth;
        protected readonly float _screenHeight;
        protected SpriteBatch _spriteBatch;
        protected float _radius;
        private Game _game;
        protected float _mass;
        protected bool _destroyed;

        public Mover(
            Game game,
            Color color,
            Vector2 position,
            float radius,
            float mass,
            float screenWidth,
            float screenHeight)
            : base(game)
        {
            _game = game;
            _color = color;
            _position = position;
            _radius = radius;
            _mass = mass;
            _screenWidth = screenWidth;
            _screenHeight = screenHeight;
            _spriteBatch = new SpriteBatch(_game.GraphicsDevice);
        }

        public Vector2 Position { get { return _position; } }
        
        /// <summary>
        /// Gives "instructions" to where the Position should be next.
        /// </summary>
        public Vector2 Velocity { get { return _velocity; } set { _velocity = value; } }
        public Vector2 Acceleration { get { return _acceleration; } }
        public Vector2 Weigth { get { return _weight; } }
        public float Mu { get { return _mu; } }
        public float Mass { get { return _mass; } }
        public float Radius { get { return _radius; } }
        public Color Color { get { return _color; } set { _color = value; } }
        public float Angle { get { return _angle; } set { _angle = value; } }

        public bool Destroyed { get { return _destroyed; } }

        public void ApplyForce(Vector2 force)
        {
            var forceDivByMass = Vector2.Divide(force, _mass);
            _acceleration += forceDivByMass;
        }

        public void ApplyFriction()
        {
            var friction = _velocity.Copy();
            friction.Normalize();
            friction *= -1;
            friction.SetMagnitude(_mu * _mass);
            ApplyForce(friction); // take a little bit off of velocity
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();
            _spriteBatch.DrawCircle(_position, _radius, (int)_radius, _color);
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        public void Destroy()
        {
            Game.Components.Remove(this);
            _destroyed = true;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _disposed)
            {
                _spriteBatch?.Dispose();
                _spriteBatch = null;
                _game = null;
                _disposed = true;
            }

            base.Dispose(disposing);
        }
    }
}
