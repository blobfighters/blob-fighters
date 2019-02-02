using BlobFighters.Core;
using FarseerPhysics.DebugView;
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
    public class TestScene : Scene
    {
        protected override void OnInit()
        {
            BackgroundColor = Color.Black;

            TextureManager.Instance.Load("Images/Octane", "Octane");

            new TestGameObject();
            new TestPhysicsGameObject();
        }

        protected override void OnDestroy()
        {
        }

        protected override void OnDraw(SpriteBatch spriteBatch)
        {
        }

        protected override void OnUpdate(float deltaTime)
        {
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                new TestPhysicsGameObject();
        }

        protected override void OnDrawGUI(SpriteBatch spriteBatch)
        {
        }
    }
}
