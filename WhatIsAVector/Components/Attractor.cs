using System;
using Microsoft.Xna.Framework;

namespace WhatIsAVector.Components
{
    public class Attractor : Mover
    {
        private float G = 1f; // gravitational constant

        private Attracted[] _attracteds;

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

        private void Attract(Attracted attracted)
        {
            var force = Vector2.Subtract(_position, attracted.Position);
            var distanceSquared = force.LengthSquared();

            var magnitude = G * (_mass * attracted.Mass) / distanceSquared;
            force.SetMagnitude(magnitude);
            attracted.ApplyForce(force);
        }

        public override void Update(GameTime gameTime)
        {
            for (int i = 0; i < _attracteds.Length; i++)
                Attract(_attracteds[i]);

            base.Update(gameTime);
        }
    }
}
