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
        private const float ButtonScale = 0.25f;

        private Texture2D name;
        private Texture2D button;

        protected override void OnInit()
        {
            TextureManager.Instance.Load("Images/Canvas", "Canvas");
            button = GameManager.Instance.Content.Load<Texture2D>("Images/startButton");
            name = GameManager.Instance.Content.Load<Texture2D>("Images/LineFighter");

            new Button(button, new Vector2((GameManager.Instance.Width - button.Width * ButtonScale) * 0.5f,
                (GameManager.Instance.Height - button.Height * ButtonScale) * 0.5f + 256), () =>
            {
                GameManager.Instance.LoadScene(new BattleScene(BattleScene.StartingNumberOfLives, BattleScene.StartingNumberOfLives, "Best of 5!"));
            }, ButtonScale);
        }

        protected override void OnDestroy()
        {
        }

        protected override void OnDraw(SpriteBatch spriteBatch)
        {
        }

        protected override void OnDrawGUI(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(name, new Vector2((GameManager.Instance.Width - name.Width) * 0.5f,
                (GameManager.Instance.Height - name.Height) * 0.5f - 256), Color.White);
        }

        protected override void OnUpdate(float deltaTime)
        {
        }
    }
}
