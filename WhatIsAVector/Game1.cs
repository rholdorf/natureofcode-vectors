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

        private Texture2D _texture2D;


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

            Primitives2D.Initialize(GraphicsDevice);

            CreateSampleMap();

            base.Initialize();
        }


        private void CreateSampleMap()
        {
            var w = 512;
            var h = 512;
            _texture2D = new Texture2D(GraphicsDevice, w, h);
            var colorData = new Color[w * h];
            var perlin = new Perlin();
            var counter = 0;
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    Color color = GetColor(
                        (1d + perlin.NoiseOctaves(
                            (double)x / 32,
                            (double)y / 32,
                            0.5d)) / 2d);
                    colorData[counter++] = color;
                }
            }
            _texture2D.SetData(colorData);
        }

        private Color GetColor(double color)
        {
            if (color < 0.35d)
                return new Color(60, 110, 200); //water

            if (color < 0.45d)
                return new Color(64, 104, 192); //shallow water

            if (color < 0.48d)
                return new Color(208, 207, 130); //sand

            if (color < 0.55d)
                return new Color(84, 150, 29); //grass

            if (color < 0.6d)
                return new Color(61, 105, 22); //forest

            if (color < 0.7d)
                return new Color(91, 68, 61); //mountain

            if (color < 0.87d)
                return new Color(75, 58, 54); //high mountain

            return new Color(255, 254, 255); //snow

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

            _walker.Update(mouseState);
            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            //_spriteBatch.Begin(transformMatrix: _translationMatrix);
            _spriteBatch.Begin();

            _spriteBatch.Draw(_texture2D, new Vector2(0, 0), Color.White);

            _walker.Draw(_spriteBatch);


            _spriteBatch.End();

            base.Draw(gameTime);
        }



    }
}
