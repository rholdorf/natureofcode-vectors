using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WhatIsAVector
{
    public class ScreenBuffer : IDisposable
    {
        private bool _disposed;
        private GraphicsDevice _graphicsDevice;
        private SpriteBatch _spriteBatch;
        private RenderTarget2D _renderTarget;
        private int _width;
        private int _height;

        public ScreenBuffer(Game game, int width, int height)
        {
            _graphicsDevice = game.GraphicsDevice;
            _width = width;
            _height = height;
        }

        public void Dispose()
        {
            if (_disposed)
                return;

            _renderTarget?.Dispose();
            _renderTarget = null;
            _disposed = true;
        }

        public void Initialize()
        {
            _renderTarget = new RenderTarget2D(
                _graphicsDevice,
                _width,
                _height,
                false,
                SurfaceFormat.Color,
                DepthFormat.None,
                _graphicsDevice.PresentationParameters.MultiSampleCount,
                RenderTargetUsage.DiscardContents);
        }
    }
}
