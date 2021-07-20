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

        private Random _random = new Random();

        private Matrix _translationMatrix;

        private const int WIDTH = 800;
        private const int HEIGHT = 600;

        private Perlin _perlin = new Perlin();

        private Vector2 _vector2;
        private float _offset;

        private SpriteFont _hudFont;
        private FpsCounter _fpsCounter;

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
            Components.Add(_fpsCounter);

            _graphics.PreferredBackBufferWidth = WIDTH;
            _graphics.PreferredBackBufferHeight = HEIGHT;
            _graphics.IsFullScreen = false;
            _graphics.ApplyChanges();
            Primitives2D.Initialize(GraphicsDevice);

            _translationMatrix = Matrix.CreateTranslation(WIDTH / 2, HEIGHT / 2, 0f);

            _vector2 = new Vector2(WIDTH / 2, HEIGHT / 2);

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

            _vector2.X += (float)_perlin.Noise(_offset);
            _offset += 0.01f;

            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            //_spriteBatch.Begin(transformMatrix: _translationMatrix);
            _spriteBatch.Begin();

            _spriteBatch.DrawCircle(_vector2, 20, 20, Color.White);



            _spriteBatch.End();

            base.Draw(gameTime);
        }



    }
}
