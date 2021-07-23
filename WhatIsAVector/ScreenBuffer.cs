using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WhatIsAVector
{
    public class ScreenBuffer : IDisposable
    {
        private bool _disposed;
        private readonly Game _game;
        private RenderTarget2D _renderTarget;
        private readonly int _width;
        private readonly int _height;

        public ScreenBuffer(Game game, int width, int height)
        {
            _game = game;
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
                _game.GraphicsDevice,
                _width,
                _height,
                false,
                SurfaceFormat.Color,
                DepthFormat.None,
                _game.GraphicsDevice.PresentationParameters.MultiSampleCount,
                RenderTargetUsage.DiscardContents);
        }
    }
}
