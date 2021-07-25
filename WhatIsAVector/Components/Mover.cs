﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WhatIsAVector.Components
{
    public class Mover : DrawableGameComponent
    {
        protected Vector2 _velocity = new();
        protected Vector2 _acceleration = new();
        protected Vector2 _weight = new();
        protected float _mu; // coefficient of friction

        private bool _disposed;
        protected Vector2 _position;
        protected Color _color;
        protected readonly float _screenWidth;
        protected readonly float _screenHeight;
        private SpriteBatch _spriteBatch;
        protected float _radius;
        private Game _game;
        protected float _mass;

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
        public Vector2 Acceleration { get { return _acceleration; } }
        public Vector2 Weigth { get { return _weight; } }
        public float Mu { get { return _mu; } }
        public float Mass { get { return _mass; } }

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

        protected override void Dispose(bool disposing)
        {
            if (disposing && _disposed)
            {
                _spriteBatch.Dispose();
                _spriteBatch = null;
                _game = null;
                _disposed = true;
            }

            base.Dispose(disposing);
        }
    }
}
