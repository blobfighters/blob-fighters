using FarseerPhysics;
using FarseerPhysics.DebugView;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoEngine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobFighters.Core
{
    public abstract class Scene
    {
        /// <summary>
        /// The default name for game objects with unassigned names.
        /// </summary>
        private const string DefaultGameObjectName = "(GameObject)";

        /// <summary>
        /// The map containing all game objects in the scene.
        /// </summary>
        private readonly Dictionary<string, GameObject> gameObjects;

        /// <summary>
        /// A list containing all destroyed game objects from the last frame.
        /// </summary>
        private readonly List<GameObject> addedGameObjects;

        /// <summary>
        /// A list containing all destroyed game objects from the last frame.
        /// </summary>
        private readonly List<GameObject> destroyedGameObjects;

        /// <summary>
        /// The camera rendering the scene.
        /// </summary>
        protected Camera Camera { get; private set; }

        /// <summary>
        /// The renderer used for debugging physics.
        /// </summary>
        protected DebugViewXNA DebugView;

        /// <summary>
        /// The background color of the scene.
        /// </summary>
        protected Color BackgroundColor { get; set; }

        /// <summary>
        /// The background texture.
        /// </summary>
        protected Texture2D BackgroundTexture { get; set; }

        /// <summary>
        /// The physics world in the scene.
        /// </summary>
        public World World { get; private set; }

        /// <summary>
        /// Initializes a new <see cref="Scene"/> instance.
        /// </summary>
        public Scene()
        {
            World = new World(Vector2.Zero);
            DebugView = new DebugViewXNA(World);
            gameObjects = new Dictionary<string, GameObject>();
            addedGameObjects = new List<GameObject>();
            destroyedGameObjects = new List<GameObject>();

            Camera = new Camera();
            BackgroundColor = Color.White;

            DebugView.AppendFlags(DebugViewFlags.DebugPanel);
            DebugView.AppendFlags(DebugViewFlags.ContactPoints);
            DebugView.AppendFlags(DebugViewFlags.ContactNormals);
            DebugView.LoadContent(GameManager.Instance.GraphicsDevice, GameManager.Instance.Content);
            DebugView.Enabled = false;
        }

        /// <summary>
        /// Initializes this scene.
        /// </summary>
        public void Init()
        {
            OnInit();
        }

        /// <summary>
        /// Updates this scene and all its contained game objects.
        /// </summary>
        /// <param name="deltaTime"></param>
        public void Update(float deltaTime)
        {
            foreach (GameObject gameObject in addedGameObjects)
                gameObjects.Add(gameObject.Name, gameObject);

            addedGameObjects.Clear();

            World.Step(deltaTime);

            foreach (GameObject gameObject in gameObjects.Values)
                gameObject.Update(deltaTime);

            foreach (GameObject gameObject in destroyedGameObjects)
                gameObjects.Remove(gameObject.Name);

            destroyedGameObjects.Clear();

            OnUpdate(deltaTime);
        }
        
        /// <summary>
        /// Draws this scene.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            GameManager.Instance.GraphicsDevice.Clear(BackgroundColor);

            if (BackgroundTexture != null)
            {
                Matrix m = Camera.ViewMatrix;

                Rectangle rect = new Rectangle(0, 0, GameManager.Instance.GraphicsDevice.Viewport.Width * 4,
                    GameManager.Instance.GraphicsDevice.Viewport.Height * 4);

                spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.LinearWrap, transformMatrix: Camera.ViewMatrix);

                spriteBatch.Draw(BackgroundTexture, Camera.Position, rect, Color.White, 0f, Vector2.Zero, 0.25f, SpriteEffects.None, 0f);

                spriteBatch.End();
            }

            spriteBatch.Begin(transformMatrix: Camera.ViewMatrix);

            foreach (GameObject gameObject in gameObjects.Values)
                gameObject.Draw(spriteBatch);

            OnDraw(spriteBatch);

            Matrix projection = Matrix.CreateOrthographicOffCenter(0f,
                ConvertUnits.ToSimUnits(GameManager.Instance.GraphicsDevice.Viewport.Width),
                ConvertUnits.ToSimUnits(GameManager.Instance.GraphicsDevice.Viewport.Height), 0f, 0f, 1f);

            spriteBatch.End();

            spriteBatch.Begin();

            foreach (GameObject gameObject in gameObjects.Values)
                gameObject.DrawGUI(spriteBatch);

            OnDrawGUI(spriteBatch);

            spriteBatch.End();

            spriteBatch.Begin(transformMatrix: Camera.SimViewMatrix);

            DebugView.RenderDebugData(projection, Camera.SimViewMatrix);

            spriteBatch.End();
        }

        /// <summary>
        /// Destroys this scene and removes all gameobjects inside it.
        /// </summary>
        public void Destroy()
        {
            OnDestroy();

            foreach (GameObject gameObject in gameObjects.Values)
                gameObject.Destroy();

            gameObjects.Clear();

            DebugView.Dispose();

            TextureManager.Instance.Release();
        }

        /// <summary>
        /// Registers a game object with the scene.
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns>The new name for the game object.</returns>
        public string RegisterGameObject(GameObject gameObject)
        {
            if (gameObject.Scene != null)
                throw new InvalidOperationException("Cannot re-register a GameObject with a scene.");

            string name = gameObject.Name ?? DefaultGameObjectName;

            if (gameObjects.ContainsKey(name))
            {
                string newName;
                for (int i = 0; gameObjects.ContainsKey(newName = name + i); i++) ;
                name = newName;
            }

            addedGameObjects.Add(gameObject);

            return name;
        }

        /// <summary>
        /// Returns the <see cref="GameObject"/> of the given name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public GameObject FindGameObject(string name)
        {
            return gameObjects[name];
        }

        /// <summary>
        /// Destroys the provided game object.
        /// </summary>
        /// <param name="gameObject"></param>
        public void DestroyGameObject(GameObject gameObject)
        {
            if (gameObject.IsDestroyed)
                return;
            
            destroyedGameObjects.Add(gameObject);
        }

        /// <summary>
        /// Called when the scene is initialized.
        /// </summary>
        protected abstract void OnInit();

        /// <summary>
        /// Called when the scene is updated.
        /// </summary>
        /// <param name="deltaTime"></param>
        protected abstract void OnUpdate(float deltaTime);

        /// <summary>
        /// Called when the scene is drawn.
        /// </summary>
        /// <param name="spriteBatch"></param>
        protected abstract void OnDraw(SpriteBatch spriteBatch);

        /// <summary>
        /// Called when the GUI is drawn.
        /// </summary>
        /// <param name="spriteBatch"></param>
        protected abstract void OnDrawGUI(SpriteBatch spriteBatch);

        /// <summary>
        /// Called when the scene is destroyed.
        /// </summary>
        protected abstract void OnDestroy();
    }
}
