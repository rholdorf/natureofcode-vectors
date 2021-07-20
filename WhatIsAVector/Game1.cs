using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace WhatIsAVector
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Walker _walker;
        private Random _random = new Random();

        private Matrix _translationMatrix;

        private const int WIDTH = 800;
        private const int HEIGHT = 600;

        private Stopwatch _stopwatch;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = WIDTH;
            _graphics.PreferredBackBufferHeight = HEIGHT;
            _graphics.IsFullScreen = false;
            _graphics.ApplyChanges();

            _walker = new Walker(WIDTH / 2, HEIGHT / 2, _random);

            var vp = GraphicsDevice.Viewport;

            _translationMatrix = Matrix.CreateTranslation(WIDTH / 2, HEIGHT / 2, 0f);
            _stopwatch = new Stopwatch();
            Primitives2D.Initialize(GraphicsDevice);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            var mouseState = Mouse.GetState();
            var keyboardState = Keyboard.GetState();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || keyboardState.IsKeyDown(Keys.Escape))
                Exit();

            if (keyboardState.IsKeyDown(Keys.OemTilde))
                Console.WriteLine(_stopwatch.Elapsed.TotalMilliseconds);

            _walker.Update(mouseState);
            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _stopwatch.Restart();
            //_spriteBatch.Begin(transformMatrix: _translationMatrix);
            _spriteBatch.Begin();


            _walker.Draw(_spriteBatch);


            _spriteBatch.End();

            base.Draw(gameTime);
            _stopwatch.Stop();
        }



    }
}
