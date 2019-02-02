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

        private Dictionary<string, GameObject> gameObjects;

        private List<GameObject> destroyedGameObjects;

        protected Color BackgroundColor { get; set; }

        public Scene()
        {
            gameObjects = new Dictionary<string, GameObject>();
            destroyedGameObjects = new List<GameObject>();

            BackgroundColor = Color.White;
        }

        public void Init()
        {
            OnInit();
        }

        public void Update(float deltaTime)
        {
            foreach (GameObject gameObject in gameObjects.Values)
                gameObject.Update(deltaTime);

            OnUpdate(deltaTime);
        }
        
        public void Draw(SpriteBatch spriteBatch)
        {
            GameManager.Instance.GraphicsDevice.Clear(BackgroundColor);

            spriteBatch.Begin();

            foreach (GameObject gameObject in gameObjects.Values)
                gameObject.Draw(spriteBatch);

            OnDraw(spriteBatch);

            spriteBatch.End();
        }

        public void Destroy()
        {
            OnDestroy();

            foreach (GameObject gameObject in gameObjects.Values)
                gameObject.Destroy();

            gameObjects.Clear();
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
