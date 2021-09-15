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
        private Texture2D _arrow;
        private readonly Perlin _perlin = new();
        private readonly Random _random = new();
        private readonly OpenSimplex2F _noise = new(1);
        private Color _backgroundColor = new Color(0, 0, 0, 10);
        private Vector3 _camTarget;
        private Vector3 _camPosition;

        /// <summary>
        /// Console games generally set of a field of view of about 60 degrees, while PC games often set the field of view higher, in the 80-100 degree range. The difference is generally due to the size of the screen viewed and the distance from it.  The higher the field of view, the more of the scene that will be rendered on screen.
        /// </summary>
        private readonly float _fieldOfViewDegrees = 60f;

        /// <summary>
        /// Camera Lens. The Projection Matrix is used to convert 3D view space to 2D. In a nutshell, this is your actual camera lens and is created by specifying calling CreatePerspectiveFieldOfView() or CreateOrthographicFieldOfView().  With Orthographic projection, the size of things remain the same regardless to their depth within the scene.  For Perspective rendering it simulates the way an eye works, by rendering things smaller as they get further away.  As a general rule, for a 2D game you use Orthographic, while in 3D you use Perspective projection.  When creating a Perspective view we specify the field of view ( think of this as the degrees of visibility from the center of your eye view ), the aspect ratio ( the proportions between width and height of the display ), near and far plane ( minimum and maximum depth to render with camera… basically the range of the camera ).  These values all go together to calculate something called the view frustum, which can be thought of as a pyramid in 3D space representing what is currently available.
        /// </summary>
        private Matrix _projectionMatrix;

        /// <summary>
        /// Camera Location. The View Matrix is used to transform coordinates from World to View space. A much easier way to envision the View matrix is it represents the position and orientation of the camera.  It is created by passing in the camera location, where the camera is pointing and by specifying which axis represents Up in the universe.  XNA uses a Y-up orientation, which is important to be aware of when creating 3D models.  Blender by default treats Z as the up/down axis, while 3D Studio MAX uses the Y-axis as Up.
        /// </summary>
        private Matrix _viewMatrix;

        /// <summary>
        /// Object Position / Orientation in 3D scene. The World matrix is used to position your entity within the scene. Essentially this is your position in the 3D world.  In addition to positional information, the World matrix can also represent an objects orientation
        /// </summary>
        private Matrix _worldMatrix;
        private Model _model;
        private bool _cameraOrbiting = true;
        private Matrix _rotationMatrix = Matrix.CreateRotationY(MathHelper.ToRadians(1f));

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _hudFont = Content.Load<SpriteFont>("Fonts/Hud");
            _arrow = Content.Load<Texture2D>("Shapes/arrow_w");

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
            //AddAttractorTriangle(10);

            //Components.Add(new SpinningRectangle(this, Color.White, new Rectangle(0, 0, 128, 64), WIDTH, HEIGHT));

            //Components.Add(new Wave(game: this, amplitude: 50, period: 300, phase: 10, screenWidth: WIDTH, screenHeight: HEIGHT, color: Color.White));
            //Components.Add(new Pendulum(this, Color.White, new Vector2(WIDTH / 4 * 3, HEIGHT / 2), WIDTH, HEIGHT));

            Components.Add(new FpsCounter(this, _hudFont, new Vector2(5, 5), Color.Yellow));
            _translationMatrix = Matrix.CreateTranslation(WIDTH / 2f, HEIGHT / 2f, 0f);

            //Setup Camera
            _camTarget = new Vector3(0f, 0f, 0f);
            _camPosition = new Vector3(0f, 0f, -5);
            _projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.ToRadians(_fieldOfViewDegrees), 
                _graphics.GraphicsDevice.Viewport.AspectRatio, 
                1f, 
                1000f);
            _viewMatrix = Matrix.CreateLookAt(_camPosition, _camTarget, new Vector3(0f, 1f, 0f));// Y up
            _worldMatrix = Matrix.CreateWorld(_camTarget, Vector3.Forward, Vector3.Up);
            _model = Content.Load<Model>("MonoCube/MonoCube");

            base.Initialize();
        }


        private void AddAttractorTriangle(int howMany)
        {
            var attracteds = new List<AttractedTriangle>();

            for (int i = 0; i < howMany; i++)
            {
                var attracted = new AttractedTriangle(
                    game: this,
                    color: new Color(_random.Next(127, 256), _random.Next(127, 256), _random.Next(127, 256)),
                    position: new Vector2(_random.Next(0, WIDTH / 4 * 3), _random.Next(0, HEIGHT / 4 * 3)),
                    mass: (float)_random.Next(50, 500),
                    screenWidth: WIDTH,
                    screenHeight: HEIGHT,
                    texture: _arrow);
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

            if (keyboardState.IsKeyDown(Keys.Left))
            {
                _camPosition.X -= 0.1f;
                _camTarget.X -= 0.1f;
            }

            if (keyboardState.IsKeyDown(Keys.Right))
            {
                _camPosition.X += 0.1f;
                _camTarget.X += 0.1f;
            }

            if (keyboardState.IsKeyDown(Keys.Up))
            {
                _camPosition.Y -= 0.1f;
                _camTarget.Y -= 0.1f;
            }

            if (keyboardState.IsKeyDown(Keys.Down))
            {
                _camPosition.Y += 0.1f;
                _camTarget.Y += 0.1f;
            }

            if (keyboardState.IsKeyDown(Keys.OemPlus))
                _camPosition.Z += 0.1f;

            if (keyboardState.IsKeyDown(Keys.OemMinus))
                _camPosition.Z -= 0.1f;

            if (keyboardState.IsKeyDown(Keys.Space))
                _cameraOrbiting = !_cameraOrbiting;

            if (_cameraOrbiting)
                _camPosition = Vector3.Transform(_camPosition, _rotationMatrix);

            _viewMatrix = Matrix.CreateLookAt(_camPosition, _camTarget, Vector3.Up);


            base.Update(gameTime);
        }

        private int ccc = 0;
        protected override void Draw(GameTime gameTime)
        {
            //if (ccc < 2)
            //{
            //    _graphics.GraphicsDevice.Clear(Color.Black);
            //    ccc++;
            //}

            //_graphics.GraphicsDevice.Clear(Color.Black);
            //_spriteBatch.Begin(transformMatrix: _translationMatrix);
            //_spriteBatch.Begin();
            //_spriteBatch.FillRectangle(0, 0, WIDTH, HEIGHT, _backgroundColor); // fade effect
            //_spriteBatch.End();


            GraphicsDevice.Clear(Color.Black);
            foreach (ModelMesh mesh in _model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.AmbientLightColor = new Vector3(0f, 0f, 1f);
                    effect.View = _viewMatrix;
                    effect.World = _worldMatrix;
                    effect.Projection = _projectionMatrix;
                }
                mesh.Draw();
            }

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
                _arrow?.Dispose();
                _arrow = null;
                _disposed = true;
            }

            base.Dispose(disposing);
        }

    }
}
