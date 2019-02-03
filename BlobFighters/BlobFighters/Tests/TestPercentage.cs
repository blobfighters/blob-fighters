using BlobFighters.Core;
using BlobFighters.Objects;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
        HealthIndicator healthP1, healthP2;
        SpriteFont font;
        Vector2 mousePos;
        Texture2D wood;

        protected override void OnInit()
        {
            BackgroundColor = Color.White;
            font = GameManager.Instance.Content.Load<SpriteFont>("Percentage");//load the spriteFont file

            
            
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
