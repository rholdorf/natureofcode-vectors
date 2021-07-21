using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using WhatIsAVector.Components;

namespace WhatIsAVector
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Matrix _translationMatrix;

        private const int WIDTH = 800;
        private const int HEIGHT = 600;

        private SpriteFont _hudFont;
        private FpsCounter _fpsCounter;
        private MovingCircleNoise _movingCircleNoise;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _hudFont = Content.Load<SpriteFont>("Fonts/Hud");
            _fpsCounter = new FpsCounter(this, _hudFont, new Vector2(5, 5), Color.Yellow);
            _movingCircleNoise = new MovingCircleNoise(this, Color.White, WIDTH, HEIGHT);

            Components.Add(_fpsCounter);
            Components.Add(_movingCircleNoise);

            _graphics.PreferredBackBufferWidth = WIDTH;
            _graphics.PreferredBackBufferHeight = HEIGHT;
            _graphics.IsFullScreen = false;
            _graphics.ApplyChanges();
            Primitives2D.Initialize(GraphicsDevice);

            _translationMatrix = Matrix.CreateTranslation(WIDTH / 2, HEIGHT / 2, 0f);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            var keyboardState = Keyboard.GetState();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || keyboardState.IsKeyDown(Keys.Escape))
                Exit();


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            //GraphicsDevice.Clear(Color.CornflowerBlue);

            //_spriteBatch.Begin(transformMatrix: _translationMatrix);
            //_spriteBatch.Begin();

            //_spriteBatch.DrawCircle(_vector2, 20, 20, Color.White);

            //_spriteBatch.End();

            base.Draw(gameTime);
        }



    }
}
