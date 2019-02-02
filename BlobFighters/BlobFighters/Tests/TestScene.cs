using BlobFighters.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobFighters.Tests
{
    public class TestScene : Scene
    {
        protected override void OnInit()
        {
            BackgroundColor = Color.White;

            TextureManager.Instance.Load("Images/Octane", "Octane");

            new TestGameObject();
        }

        protected override void OnDestroy()
        {
        }

        protected override void OnDraw(SpriteBatch spriteBatch)
        {
        }

        protected override void OnUpdate(float deltaTime)
        {
        }
    }
}
