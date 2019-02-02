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
        PercentageIndicator healthP1, healthP2;
        SpriteFont font;

        protected override void OnInit()
        {
            BackgroundColor = Color.Black;
            font = GameManager.Instance.Content.Load<SpriteFont>("Percentage");//load the spriteFont file
            healthP1 = new PercentageIndicator(font,new Vector2(220, 600));
            healthP2 = new PercentageIndicator(font, new Vector2(900, 600));
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

        protected override void OnDrawGUI(SpriteBatch spriteBatch)
        {
            
        }
    }
}
