using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Noise;
using WhatIsAVector.Components;

namespace WhatIsAVector
{
    public class Game1 : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Matrix _translationMatrix;

        private const int WIDTH = 800;
        private const int HEIGHT = 600;

        private SpriteFont _hudFont;

        private Perlin _perlin = new();
        private Random _random = new();





        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _hudFont = Content.Load<SpriteFont>("Fonts/Hud");

            _graphics.PreferredBackBufferWidth = WIDTH;
            _graphics.PreferredBackBufferHeight = HEIGHT;
            _graphics.IsFullScreen = false;
            _graphics.ApplyChanges();
            Primitives2D.Initialize(GraphicsDevice);

            Components.Add(new CreateTexture2DPerlinNoise(this, Color.White, WIDTH / 2, HEIGHT / 2, _perlin));
            //Components.Add(new MovingCircle1DPerlinNoise(this, Color.White, WIDTH, HEIGHT, _perlin, _random));
            //Components.Add(new RollingGraph1DPerlinNoise(this, Color.Red, WIDTH, HEIGHT, _perlin));



            Components.Add(new FpsCounter(this, _hudFont, new Vector2(5, 5), Color.Yellow));
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
            GraphicsDevice.Clear(Color.CornflowerBlue);

            //_spriteBatch.Begin(transformMatrix: _translationMatrix);
            _spriteBatch.Begin();




            _spriteBatch.End();

            base.Draw(gameTime);
        }



    }
}
