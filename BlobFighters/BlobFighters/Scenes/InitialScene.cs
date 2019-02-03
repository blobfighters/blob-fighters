using BlobFighters.Core;
using BlobFighters.Objects;
using FarseerPhysics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobFighters.Scenes
{
    class InitialScene : Scene
    {
        private Texture2D name;
        private Texture2D button;
        private int status = 0;
        protected override void OnDestroy()
        {
        }

        protected override void OnDraw(SpriteBatch spriteBatch)
        {
        }

        protected override void OnDrawGUI(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(name, new Vector2(420, 150), Color.White);
            if (status == 0)
            {
                spriteBatch.Draw(button, new Vector2(560, 400), null, Color.White, 0f, Vector2.Zero, 0.3f, SpriteEffects.None, 0f);
            }
            else {
                spriteBatch.Draw(button, new Vector2(560, 400), null, Color.DarkSeaGreen, 0f, Vector2.Zero, 0.3f, SpriteEffects.None, 0f);
            }
        }

        protected override void OnInit()
        {
            TextureManager.Instance.Load("Images/Canvas", "Canvas");
            button = GameManager.Instance.Content.Load<Texture2D>("Images/startButton");
            name = GameManager.Instance.Content.Load<Texture2D>("Images/Name");
        }

        protected override void OnUpdate(float deltaTime)
        {
            int x = Mouse.GetState().X;
            int y = Mouse.GetState().Y;
            if (Mouse.GetState().LeftButton == ButtonState.Pressed && x> 565 && x<750 && y>407 && y<456)
            {
                status += 1;
                if (status == 4) { GameManager.Instance.LoadScene(new BattleScene()); }
            }
        }
    }
}
