using System;
using BlobFighters.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BlobFighters.Objects
{
    public class MaterialBrowser : GameObject
    {

        private Texture2D material1;

        private Texture2D material2;

        private Texture2D material3;

        private Texture2D material4;



        public MaterialBrowser(Vector2 position) : base("MaterialBrowser", position)
        { 
        }



        protected override void OnDestroy()
        {
            throw new NotImplementedException();
        }

        protected override void OnDraw(SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }

        protected override void OnUpdate(float deltaTime)
        {
            throw new NotImplementedException();
        }
    }
}
