using BlobFighters.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobFighters.Tests
{
    public class TestGameObject : GameObject
    {
        private readonly Texture2D texture;

        public TestGameObject() : base("Test!")
        {
            texture = TextureManager.Instance.Get("Octane");
        }

        protected override void OnUpdate(float deltaTime)
        {
            KeyboardState state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.Left))
                Position += new Vector2(-100f * deltaTime, 0f);
            else if (state.IsKeyDown(Keys.Right))
                Position += new Vector2(100f * deltaTime, 0f);
        }

        protected override void OnDestroy()
        {
        }

        protected override void OnDraw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Position, Color.White);
        }
    }
}
