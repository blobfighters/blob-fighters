using BlobFighters.Core;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq; 
using System.Text;
using System.Threading.Tasks;

namespace BlobFighters.Objects
{   
    class PercentageIndicator : GameObject
    {
        public SpriteFont font;
        public int score = 0;
        public PercentageIndicator() : base("Percentage")
        {
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
