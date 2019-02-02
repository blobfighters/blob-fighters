using BlobFighters.Core;
using Microsoft.Xna.Framework;
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
        private SpriteFont font;
        public int score = 0;
        private Vector2 location;
        public PercentageIndicator(SpriteFont inputFont, Vector2 inputLocation) : base("Percentage")
        {
            font = inputFont;
            location = inputLocation;
        }

        protected override void OnDestroy()
        {
        }

        protected override void OnDraw(SpriteBatch spriteBatch)
        {
        }

        protected override void OnDrawGUI(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font, score + "%", location, Color.Black);
        }

        protected override void OnUpdate(float deltaTime)
        {
            if (score < 150)
            {
                score++;
            }
        }
    }
        
}
