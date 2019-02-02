using FarseerPhysics;
using FarseerPhysics.DebugView;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobFighters.Core
{
    public abstract class Scene
    {
        private const string DefaultGameObjectName = "(GameObject)";

        private readonly DebugViewXNA debugView;

        private readonly Dictionary<string, GameObject> gameObjects;

        private readonly List<GameObject> destroyedGameObjects;

        protected Color BackgroundColor { get; set; }

        public World World { get; private set; }

        public Scene()
        {
            World = new World(Vector2.Zero);
            debugView = new DebugViewXNA(World);
            gameObjects = new Dictionary<string, GameObject>();
            destroyedGameObjects = new List<GameObject>();

            BackgroundColor = Color.White;

            debugView.AppendFlags(DebugViewFlags.DebugPanel);
            debugView.AppendFlags(DebugViewFlags.ContactPoints);
            debugView.AppendFlags(DebugViewFlags.ContactNormals);
            debugView.LoadContent(GameManager.Instance.GraphicsDevice, GameManager.Instance.Content);
        }

        public void Init()
        {
            OnInit();
        }

        public void Update(float deltaTime)
        {
            World.Step(deltaTime);

            foreach (GameObject gameObject in gameObjects.Values)
                gameObject.Update(deltaTime);

            foreach (GameObject gameObject in destroyedGameObjects)
                gameObjects.Remove(gameObject.Name);

            destroyedGameObjects.Clear();

            OnUpdate(deltaTime);
        }
        
        public void Draw(SpriteBatch spriteBatch)
        {
            GameManager.Instance.GraphicsDevice.Clear(BackgroundColor);

            spriteBatch.Begin(SpriteSortMode.BackToFront);

            foreach (GameObject gameObject in gameObjects.Values)
                gameObject.Draw(spriteBatch);

            OnDraw(spriteBatch);

            Matrix projection = Matrix.CreateOrthographicOffCenter(0f,
                ConvertUnits.ToSimUnits(GameManager.Instance.GraphicsDevice.Viewport.Width),
                ConvertUnits.ToSimUnits(GameManager.Instance.GraphicsDevice.Viewport.Height), 0f, 0f, 1f);

            debugView.RenderDebugData(ref projection);

            spriteBatch.End();
        }

        public void Destroy()
        {
            OnDestroy();

            foreach (GameObject gameObject in gameObjects.Values)
                gameObject.Destroy();

            gameObjects.Clear();

            debugView.Dispose();
        }

        public string RegisterGameObject(GameObject gameObject)
        {
            string name = gameObject.Name ?? DefaultGameObjectName;

            if (gameObjects.ContainsKey(name))
            {
                string newName;
                for (int i = 0; gameObjects.ContainsKey(newName = name + i); i++) ;
                name = newName;
            }

            gameObjects.Add(name, gameObject);

            return name;
        }

        public GameObject FindGameObject(string name)
        {
            return gameObjects[name];
        }

        public void DestroyGameObject(GameObject gameObject)
        {
            if (gameObject.IsDestroyed)
                return;
            
            destroyedGameObjects.Add(gameObject);
        }

        protected abstract void OnInit();

        protected abstract void OnUpdate(float deltaTime);

        protected abstract void OnDraw(SpriteBatch spriteBatch);

        protected abstract void OnDestroy();
    }
}
