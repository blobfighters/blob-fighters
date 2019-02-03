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
        public float damage;
        private Vector2 location;
        Blob blob;
        public PercentageIndicator(SpriteFont inputFont, Vector2 inputLocation, Blob blobIn) : base("Percentage")
        {
            font = inputFont;
            location = inputLocation;
            blob = blobIn;
        }

        protected override void OnDestroy()
        {
        }

        protected override void OnDraw(SpriteBatch spriteBatch)
        {
        }

        protected override void OnDrawGUI(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font,  Math.Round(blob.DamageRatio,1) + "%", location, Color.Black);
        }

        protected override void OnUpdate(float deltaTime)
        {
    
        }
    }
        
}
