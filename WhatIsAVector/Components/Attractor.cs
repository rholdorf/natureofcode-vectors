﻿using System;
using Microsoft.Xna.Framework;

namespace WhatIsAVector.Components
{
    public class Attractor : Mover
    {
        private float G = 1f; // gravitational constant

        private Attracted[] _attracteds;
        private float _doubleRadius;

        public Attractor(
            Game game,
            Color color,
            Vector2 position,
            float mass,
            float screenWidth,
            float screenHeight,
            Attracted[] attracteds)
            : base(game, color, position, (float)Math.Sqrt(mass), mass, screenWidth, screenHeight)
        {
            _attracteds = attracteds;
        }

        public override void Initialize()
        {
            _doubleRadius = _radius * 2;
            base.Initialize();
        }

        private void Attract(Attracted attracted)
        {
            var force = Vector2.Subtract(_position, attracted.Position);
            var distance = force.Length();
            if (distance > _doubleRadius)
            {
                var distanceSquared = distance * distance;
                var magnitude = G * (_mass * attracted.Mass) / distanceSquared;
                force.SetMagnitude(magnitude);
                attracted.ApplyForce(force);
                attracted.Color= Color.White;
            }
            else
            {
                attracted.Color = Color.Red;
            }
            // else, thy should have collided
        }

        public override void Update(GameTime gameTime)
        {
            for (int i = 0; i < _attracteds.Length; i++)
                Attract(_attracteds[i]);

            base.Update(gameTime);
        }
    }
}
