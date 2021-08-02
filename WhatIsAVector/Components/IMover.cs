using Microsoft.Xna.Framework;

namespace WhatIsAVector.Components
{
    public interface IMover
    {
        Vector2 Acceleration { get; }
        Color Color { get; set; }
        float Mass { get; }
        float Mu { get; }
        Vector2 Position { get; }
        float Radius { get; }
        Vector2 Velocity { get; set; }
        Vector2 Weigth { get; }

        void ApplyForce(Vector2 force);
        void ApplyFriction();
        void Draw(GameTime gameTime);
        void Destroy();
        bool Destroyed { get; }
    }
}
