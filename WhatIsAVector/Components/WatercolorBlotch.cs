using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WhatIsAVector.Components;

public class WatercolorBlotch : DrawableGameComponent
{
    private bool _disposed;
    private readonly Vector2 _position;
    private readonly Color _color;
    private SpriteBatch _spriteBatch;
    private Game _game;
    private Texture2D _texture;
    private List<Vector2> _polygon;

    public WatercolorBlotch(
        Game game,
        Color color,
        Vector2 position)
        : base(game)
    {
        _game = game;
        _color = color;
        _position = position;
        _spriteBatch = new SpriteBatch(_game.GraphicsDevice);
        _polygon = Primitives2D.CreateCircle(20, 8);
    }
    
    // public override void Update(GameTime gameTime)
    // {
    //     base.Update(gameTime);
    // }

    public override void Draw(GameTime gameTime)
    {
        _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
        _spriteBatch.DrawPoints(_position, _polygon, _color, 1f);
        _spriteBatch.End();

        base.Draw(gameTime);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing && _disposed)
        {
            _spriteBatch?.Dispose();
            _spriteBatch = null;
            _texture?.Dispose();
            _texture = null;

            _game = null;
            _disposed = true;
        }

        base.Dispose(disposing);
    }
}