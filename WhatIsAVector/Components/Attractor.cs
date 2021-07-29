using System;
using Microsoft.Xna.Framework;

namespace WhatIsAVector.Components
{
    public class Attractor : Mover
    {
        private float G = 1f; // gravitational constant

        private readonly IMover[] _attracteds;
        private float _doubleRadius;

        public Attractor(
            Game game,
            Color color,
            Vector2 position,
            float mass,
            float screenWidth,
            float screenHeight,
            IMover[] attracteds)
            : base(game, color, position, (float)Math.Sqrt(mass), mass, screenWidth, screenHeight)
        {
            _attracteds = attracteds;
        }

        public override void Initialize()
        {
            _doubleRadius = _radius * 2;
            base.Initialize();
        }

        private void Attract(IMover attracted)
        {
            var force = Vector2.Subtract(_position, attracted.Position);
            var distance = force.Length();
            if (distance > _doubleRadius)
            {
                var distanceSquared = distance * distance;
                var magnitude = G * (_mass * attracted.Mass) / distanceSquared;
                force.SetMagnitude(magnitude);
                attracted.ApplyForce(force);
                attracted.Color = Color.White;
            }
            else
            {
                // collided
                attracted.Color = Color.Red;
            }
        }

        public override void Update(GameTime gameTime)
        {
            for (int i = 0; i < _attracteds.Length; i++)
            {
                var attracted = _attracteds[i];
                Attract(attracted);
            }

            base.Update(gameTime);
        }
    }
}
