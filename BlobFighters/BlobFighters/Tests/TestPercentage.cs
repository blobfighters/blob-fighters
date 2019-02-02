using BlobFighters.Core;
using BlobFighters.Objects;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;   
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobFighters.Tests
{
    class TestPercentage : Scene
    {
        PercentageIndicator health;

        protected override void OnInit()
        {
            BackgroundColor = Color.White;

            health = new PercentageIndicator();
            health.font = GameManager.Instance.Content.Load<SpriteFont>("Percentage");//load the spriteFont file
        }
        protected override void OnDestroy()
        {
        }
 
        protected override void OnDraw(SpriteBatch spriteBatch)
        { 

            spriteBatch.DrawString(health.font, health.score+"%", new Vector2(0, 0), Color.Black);

        }


        protected override void OnUpdate(float deltaTime)
        {
            if (health.score < 100)
            {
                health.score++;
            }
        }
    }
}
