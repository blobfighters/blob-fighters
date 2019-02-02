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
        public string Name { get; private set; }

        public Vector2 Position { get; set; }

        public float Rotation { get; set; }

        public bool IsDestroyed { get; private set; }

        protected Scene Scene { get; private set; }

        public GameObject(string name = null, Vector2 position = default(Vector2), float rotation = 0f)
        {
            Name = name;
            Position = position;
            Rotation = rotation;
            IsDestroyed = false;

            Scene = GameManager.Instance.ActiveScene;

            Name = Scene.RegisterGameObject(this);
        }

        public void Update(float deltaTime)
        {
            OnUpdate(deltaTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            OnDraw(spriteBatch);
        }

        public void Destroy()
        {
            OnDestroy();
            Scene.DestroyGameObject(this);
            IsDestroyed = true;
        }

        protected abstract void OnUpdate(float deltaTime);

        protected abstract void OnDraw(SpriteBatch spriteBatch);

        protected abstract void OnDestroy();
    }
}
