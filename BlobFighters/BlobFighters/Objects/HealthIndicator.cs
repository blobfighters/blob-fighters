using BlobFighters.Core;
using BlobFighters.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq; 
using System.Text;
using System.Threading.Tasks;

namespace BlobFighters.Objects
{   
    class HealthIndicator : GameObject
    {
        const float LivesLeftVerticalOffset = 64f;
        const float LivesIconScale = 0.5f;

        private readonly SpriteFont font;
        private readonly Blob blob;
        private readonly BattleScene scene;
        private readonly int livesLeft;

        private readonly Texture2D faceTexture;
        private readonly Texture2D headTexture;

        public HealthIndicator(SpriteFont inputFont, Vector2 position, Blob blob, int livesLeft) : base("Percentage", position)
        {
            font = inputFont;
            scene = Scene as BattleScene;

            this.blob = blob;
            this.livesLeft = livesLeft;

            faceTexture = TextureManager.Instance.Get("Face");
            headTexture = TextureManager.Instance.Get("Head");
        }

        protected override void OnDrawGUI(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font, Math.Round(blob.Health, 1) + " HP", Position, Color.Black);

            for (int i = 0; i < livesLeft; i++)
            {
                spriteBatch.Draw(headTexture, Position + new Vector2(headTexture.Width * LivesIconScale * i, LivesLeftVerticalOffset),
                    null, blob.Color, 0f, Vector2.Zero, LivesIconScale, SpriteEffects.None, 0f);
                spriteBatch.Draw(faceTexture, Position + new Vector2(headTexture.Width * LivesIconScale * i, LivesLeftVerticalOffset),
                    null, Color.White, 0f, Vector2.Zero, LivesIconScale, SpriteEffects.None, 0f);
            }
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
