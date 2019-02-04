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
        private const float TopMargin = 0f;
        private const float ButtonScale = 0.25f;
        private const float ControlsScale = 0.65f;

        private Texture2D name;
        private Texture2D controlsTexture;
        private Texture2D startButton;
        private Texture2D exitButton;

        protected override void OnInit()
        {
            TextureManager.Instance.Load("Images/Canvas", "Canvas");
            TextureManager.Instance.Load("Images/Controls", "Controls");
            TextureManager.Instance.Load("Images/startButton", "Start");
            TextureManager.Instance.Load("Images/exitButton", "Exit");

            controlsTexture = TextureManager.Instance.Get("Controls");
            startButton = TextureManager.Instance.Get("Start");
            exitButton = TextureManager.Instance.Get("Exit");
            
            name = GameManager.Instance.Content.Load<Texture2D>("Images/LineFighter");

            new Button(startButton, new Vector2((GameManager.Instance.Width - startButton.Width * ButtonScale) * 0.5f,
                GameManager.Instance.Height - 128f), () =>
            {
                GameManager.Instance.LoadScene(new BattleScene(BattleScene.StartingNumberOfLives, BattleScene.StartingNumberOfLives, "Best of 5!"));
            }, ButtonScale);

            new Button(exitButton, new Vector2((GameManager.Instance.Width - exitButton.Width * ButtonScale) * 0.5f,
                GameManager.Instance.Height - 64f), () =>
            {
                GameManager.Instance.Exit();
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
                TopMargin), Color.White);

            spriteBatch.Draw(controlsTexture, new Vector2((GameManager.Instance.Width - controlsTexture.Width * ControlsScale) * 0.5f,
                (GameManager.Instance.Height - controlsTexture.Height * ControlsScale) * 0.5f), null, Color.White, 0f, Vector2.Zero, ControlsScale, SpriteEffects.None, 0f);
        }

        protected override void OnUpdate(float deltaTime)
        {
        }
    }
}
