using BlobFighters.Core;
using BlobFighters.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobFighters.Scenes
{
    public class BattleScene : Scene
    {
        private Blob blob;
        private Ground ground;

        protected override void OnInit()
        {
            blob = new Blob();
            ground = new Ground();

            Camera.Position += new Vector2(0f, -GameManager.Instance.GraphicsDevice.Viewport.Height * 0.5f);

            World.Gravity = new Vector2(0f, 30f);
        }

        protected override void OnUpdate(float deltaTime)
        {
        }
        
        protected override void OnDraw(SpriteBatch spriteBatch)
        {
        }

        protected override void OnDrawGUI(SpriteBatch spriteBatch)
        {
        }

        protected override void OnDestroy()
        {
        }
    }
}
