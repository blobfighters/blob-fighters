using BlobFighters.Core;
using BlobFighters.Objects;
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

        protected override void OnInit()
        {
            blob = new Blob();
        }

        protected override void OnUpdate(float deltaTime)
        {
        }
        
        protected override void OnDraw(SpriteBatch spriteBatch)
        {
        }

        protected override void OnDestroy()
        {
        }
    }
}
