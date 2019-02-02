using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobFighters.Core
{
    public abstract class GameObject
    {
        /// <summary>
        /// The name of this game object.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The position of the game object.
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// The rotation of the game object, in radians.
        /// </summary>
        public float Rotation { get; set; }

        /// <summary>
        /// Used to determine if the game object has been destroyed.
        /// </summary>
        public bool IsDestroyed { get; private set; }

        /// <summary>
        /// The scene owning this game object.
        /// </summary>
        public Scene Scene { get; private set; }

        /// <summary>
        /// Initializes a new <see cref="GameObject"/> instance.
        /// </summary>
        /// <param name="name">The name of the game object.</param>
        /// <param name="position">The starting position of the game object.</param>
        /// <param name="rotation">The starting rotation of the game object.</param>
        public GameObject(string name = null, Vector2 position = default(Vector2), float rotation = 0f)
        {
            Name = name;
            Position = position;
            Rotation = rotation;
            IsDestroyed = false;

            Scene currentScene = GameManager.Instance.ActiveScene;

            // Avoid getting a duplicate name; let the scene determine the name if duplicate.
            Name = currentScene.RegisterGameObject(this);

            Scene = currentScene;
        }

        /// <summary>
        /// Updates the logic controlling this game object.
        /// </summary>
        /// <param name="deltaTime"></param>
        public void Update(float deltaTime)
        {
            OnUpdate(deltaTime);
        }

        /// <summary>
        /// Draws the game object.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            OnDraw(spriteBatch);
        }

        /// <summary>
        /// Destroys the game object.
        /// </summary>
        public void Destroy()
        {
            OnDestroy();
            Scene.DestroyGameObject(this);
            IsDestroyed = true;
        }

        /// <summary>
        /// Called when the game object is updated.
        /// </summary>
        /// <param name="deltaTime"></param>
        protected abstract void OnUpdate(float deltaTime);

        /// <summary>
        /// Called when the game object is drawn.
        /// </summary>
        /// <param name="spriteBatch"></param>
        protected abstract void OnDraw(SpriteBatch spriteBatch);

        /// <summary>
        /// Called when the game object is destroyed.
        /// </summary>
        protected abstract void OnDestroy();
    }
}
