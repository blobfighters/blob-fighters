using BlobFighters.Core;
using BlobFighters.Scenes;
using BlobFighters.Tests;
using FarseerPhysics.DebugView;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoEngine.Core;

namespace BlobFighters
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class GameManager : Game
    {
        /// <summary>
        /// The width of the game window.
        /// </summary>
        private const int Width = 1920;

        /// <summary>
        /// The height of the game window.
        /// </summary>
        private const int Height = 1080;

        /// <summary>
        /// The private singleton of this instance.
        /// </summary>
        private static GameManager instance;

        /// <summary>
        /// The singleton of this instance.
        /// </summary>
        public static GameManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new GameManager();

                return instance;
            }
        }

        /// <summary>
        /// The <see cref="GraphicsDeviceManager"/> of this instance.
        /// </summary>
        private readonly GraphicsDeviceManager graphics;

        /// <summary>
        /// The active running scene.
        /// </summary>
        public Scene ActiveScene { get; private set; }

        /// <summary>
        /// The scene to become active in the next update.
        /// </summary>
        private Scene pendingScene;

        /// <summary>
        /// The <see cref="SpriteBatch"/> used for rendering.
        /// </summary>
        private SpriteBatch spriteBatch;

        /// <summary>
        /// Initializes a new <see cref="GameManager"/> instance.
        /// </summary>
        private GameManager()
        {
            IsMouseVisible = true;

            graphics = new GraphicsDeviceManager(this)
            {
                IsFullScreen = true,
                PreferredBackBufferWidth = Width,
                PreferredBackBufferHeight = Height
            };

            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Loads the provided scene in the next frame.
        /// </summary>
        /// <param name="scene"></param>
        public void LoadScene(Scene scene)
        {
            pendingScene = scene;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            LoadScene(new MaterialTestScene());

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            InputManager.Instance.Refresh();

            if (pendingScene != null)
            {
                ActiveScene?.Destroy();

                ActiveScene = pendingScene;
                pendingScene = null;

                ActiveScene.Init();
            }

            ActiveScene?.Update(gameTime.ElapsedGameTime.Milliseconds * 0.001f);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            ActiveScene.Draw(spriteBatch);

            base.Draw(gameTime);
        }
    }
}
