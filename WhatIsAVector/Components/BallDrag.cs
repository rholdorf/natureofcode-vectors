using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace WhatIsAVector.Components
{
    public class BallDrag : BouncingBall
    {
        private bool _disposed;
        private SpriteBatch _spriteBatch;
        private readonly Rectangle _viscousArea;
        private readonly Color _viscousColor = new Color(15, 15, 15, 10);


        public BallDrag(Game game, Vector2 position, Color color, float radius, float mass, float screenWidth, float screenHeight)
            : base(game, position, color, radius, mass, screenWidth, screenHeight)
        {
            _spriteBatch = new SpriteBatch(game.GraphicsDevice);
            _viscousArea = new Rectangle(0, (int)screenHeight / 2, (int)screenWidth, (int)screenHeight / 2);
        }

        public override void Update(GameTime gameTime)
        {
            ApplyDrag();
            base.Update(gameTime);
        }

        private void ApplyDrag()
        {
            if ((_velocity.X == 0 && _velocity.Y == 0)
                || _position.Y < _viscousArea.Y)
                return;

            var drag = _velocity.Copy();
            drag.Normalize();
            drag *= -1;
            var c = 0.1f; // coefficient of drag
            var speedSquared = _velocity.LengthSquared();
            drag.SetMagnitude(c * speedSquared);
            ApplyForce(drag);
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin(blendState: BlendState.AlphaBlend);
            _spriteBatch.FillRectangle(_viscousArea, _viscousColor);
            _spriteBatch.End();
            base.Draw(gameTime); // base will draw the circle
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
