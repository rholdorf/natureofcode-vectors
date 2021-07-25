using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Noise;

namespace WhatIsAVector.Components
{
    public class MovingCircle1DOpenSimplexNoise : Mover
    {
        private float _offset1;
        private float _offset2;
        private readonly OpenSimplex2F _noise;

        public MovingCircle1DOpenSimplexNoise(
            Game game,
            Color color,
            Vector2 position,
            float radius,
            float mass,
            float screenWidth,
            float screenHeight,
            OpenSimplex2F noise,
            Random random)
            : base(game, color, position, radius, mass, screenWidth, screenHeight)
        {
            _noise = noise;
            _offset1 = (float)random.NextDouble() * 500;
            _offset2 = (float)random.NextDouble() * 1000;
        }

        public override void Update(GameTime gameTime)
        {
            _position.X = ((float)_noise.Noise2(_offset1, 0.5d)).Map(-1, 1, 0, _screenWidth);
            _position.Y = ((float)_noise.Noise2(_offset2, 0.5d)).Map(-1, 1, 0, _screenHeight);
            _offset1 += 0.005f;
            _offset2 += 0.005f;

            base.Update(gameTime);
        }

    }
}
