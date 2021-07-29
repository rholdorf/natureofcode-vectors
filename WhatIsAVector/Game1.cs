using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Noise;
using WhatIsAVector.Components;

namespace WhatIsAVector
{
    public class Game1 : Game
    {
        private bool _disposed;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        //private RenderTarget2D _screenBuffer;
        private Matrix _translationMatrix;

        private const int WIDTH = 800;
        private const int HEIGHT = 600;

        private SpriteFont _hudFont;

        private readonly Perlin _perlin = new();
        private readonly Random _random = new();
        private readonly OpenSimplex2F _noise = new(1);

        private Color _backgroundColor = new Color(0, 0, 0, 10);



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
            //_screenBuffer = new RenderTarget2D(_graphics.GraphicsDevice, WIDTH, HEIGHT, false, SurfaceFormat.Color, DepthFormat.None, _graphics.GraphicsDevice.PresentationParameters.MultiSampleCount, RenderTargetUsage.PreserveContents);

            Primitives2D.Initialize(GraphicsDevice);

            //Components.Add(new CreateTexture2DPerlinNoise(this, new Vector2(0, 0), Color.White, WIDTH / 2, HEIGHT / 2, _perlin));
            //Components.Add(new CreateTexture2DOpenSimplexNoise(this, new Vector2(WIDTH / 2, 0), Color.White, WIDTH / 2, HEIGHT / 2, _noise));

            //Components.Add(new MovingCircle1DPerlinNoise(this, Color.White, new Vector2(WIDTH / 2, HEIGHT / 2), 20f, 1, WIDTH, HEIGHT, _perlin, _random));
            //Components.Add(new MovingCircle1DOpenSimplexNoise(this, Color.Yellow, new Vector2(WIDTH / 2, HEIGHT / 2), 20f, 1, WIDTH, HEIGHT, _noise, _random));

            //Components.Add(new RollingGraph1DPerlinNoise(this, Color.Red, WIDTH, HEIGHT, _perlin));
            //Components.Add(new RollingGraph1DOpenSimplexNoise(this, Color.Yellow, WIDTH, HEIGHT, _noise));


            //AddBouncingBalls(5);
            //AddBallDrag(5);

            //AddAttractor(50);
            AddAttractorTriangle(5);

            //Components.Add(new SpinningRectangle(this, Color.White, new Rectangle(0, 0, 128, 64), WIDTH, HEIGHT));

            Components.Add(new FpsCounter(this, _hudFont, new Vector2(5, 5), Color.Yellow));
            _translationMatrix = Matrix.CreateTranslation(WIDTH / 2f, HEIGHT / 2f, 0f);

            base.Initialize();
        }


        private void AddAttractorTriangle(int howMany)
        {
            var attracteds = new List<AttractedTriangle>();

            for (int i = 0; i < howMany; i++)
            {
                var attracted = new AttractedTriangle(
                    game: this,
                    color: Color.Orange,
                    position: new Vector2(_random.Next(0, WIDTH / 4 * 3), _random.Next(0, HEIGHT / 4 * 3)),
                    mass: (float)_random.Next(50, 500),
                    screenWidth: WIDTH,
                    screenHeight: HEIGHT);
                attracted.Velocity = new Vector2().Randomize(_random);
                Components.Add(attracted);
                attracteds.Add(attracted);
            }

            Components.Add(new Attractor(
                game: this,
                color: Color.White,
                position: new Vector2(WIDTH / 2, HEIGHT / 2),
                mass: 200f,
                screenWidth: WIDTH,
                screenHeight: HEIGHT,
                attracteds: attracteds.ToArray()));
        }

        private void AddAttractor(int howMany)
        {
            var attracteds = new List<Attracted>();

            for (int i = 0; i < howMany; i++)
            {
                var attracted = new Attracted(
                    game: this,
                    color: Color.Orange,
                    position: new Vector2(_random.Next(0, WIDTH/4*3), _random.Next(0, HEIGHT/4*3)),
                    mass: (float)_random.Next(50, 500),
                    screenWidth: WIDTH,
                    screenHeight: HEIGHT);
                attracted.Velocity = new Vector2().Randomize(_random);
                Components.Add(attracted);
                attracteds.Add(attracted);
            }

            Components.Add(new Attractor(
                game: this,
                color: Color.White,
                position: new Vector2(WIDTH / 2, HEIGHT / 2),
                mass: 200f,
                screenWidth: WIDTH,
                screenHeight: HEIGHT,
                attracteds: attracteds.ToArray()));
        }

        private void AddBouncingBalls(int howMany)
        {
            for (int i = 0; i < howMany; i++)
            {
                Components.Add(new BouncingBall(
                    game: this,
                    position: new Vector2((float)WIDTH / howMany * i, (float)HEIGHT / 1.25f - (_random.Next(0, howMany * 2))),
                    color: Color.White,
                    radius: 10f * (float)(Math.Pow(_random.NextDouble() + 1, 2)),
                    mass: 2,
                    screenWidth: WIDTH,
                    screenHeight: HEIGHT));
            }
        }

        private void AddBallDrag(int howMany)
        {
            for (int i = 0; i < howMany; i++)
            {
                Components.Add(new BallDrag(
                    game: this,
                    position: new Vector2(_random.Next(0, WIDTH), 20),
                    color: Color.Red,
                    radius: 10f * (float)(Math.Pow(_random.NextDouble() + 1, 2)),
                    mass: 2,
                    screenWidth: WIDTH,
                    screenHeight: HEIGHT));
            }
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
             _graphics.GraphicsDevice.Clear(Color.CornflowerBlue);
            //_spriteBatch.Begin(transformMatrix: _translationMatrix);
            //_spriteBatch.Begin();
            //_spriteBatch.FillRectangle(0, 0, WIDTH, HEIGHT, _backgroundColor); // fade effect
            //_spriteBatch.End();

            base.Draw(gameTime);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                _graphics?.Dispose();
                _graphics = null;
                _spriteBatch?.Dispose();
                _spriteBatch = null;
                _disposed = true;
            }

            base.Dispose(disposing);
        }

    }
}
