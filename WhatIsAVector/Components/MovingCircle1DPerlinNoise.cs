using System;
using Microsoft.Xna.Framework;
using Noise;

namespace WhatIsAVector.Components
{
    public class MovingCircle1DPerlinNoise : Mover
    {
        private float _offset1;
        private float _offset2;
        private readonly Perlin _noise;

        public MovingCircle1DPerlinNoise(
            Game game,
            Color color,
            Vector2 position,
            float radius,
            float mass,
            float screenWidth,
            float screenHeight,
            Perlin noise,
            Random random)
            : base(game, color, position, radius, mass, screenWidth, screenHeight)
        {
            _noise = noise;
            _offset1 = (float)random.NextDouble() * 500;
            _offset2 = (float)random.NextDouble() * 1000;
        }

        public override void Update(GameTime gameTime)
        {
            _position.X = (1 + _noise.Noise(_offset1)) * _screenWidth / 2;
            _position.Y = (1 + _noise.Noise(_offset2)) * _screenHeight / 2;
            _offset1 += 0.015f;
            _offset2 += 0.015f;

            base.Update(gameTime);
        }
    }
}
