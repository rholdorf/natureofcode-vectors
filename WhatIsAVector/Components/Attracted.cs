using System;
using Microsoft.Xna.Framework;

namespace WhatIsAVector.Components
{
    public class Attracted : Mover
    {
        public Attracted(
            Game game,
            Color color,
            Vector2 position,
            float mass,
            float screenWidth,
            float screenHeight)
            : base(game, color, position, (float)Math.Sqrt(mass), mass, screenWidth, screenHeight)
        {
        }

    }
}
