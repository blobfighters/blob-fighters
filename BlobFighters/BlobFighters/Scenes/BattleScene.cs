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
    public class BattleScene : Scene
    {
        private Blob blob1;
        private Blob blob2;
        private Ground ground;

        private Vector2 cameraCurrent;
        private Vector2 cameraTarget;

        protected override void OnInit()
        {
            TextureManager.Instance.Load("Images/Canvas", "Canvas");
            TextureManager.Instance.Load("Images/Body", "Body");
            TextureManager.Instance.Load("Images/Face", "Face");
            TextureManager.Instance.Load("Images/Head", "Head");
            TextureManager.Instance.Load("Images/Arm", "Arm");

            //BackgroundTexture = TextureManager.Instance.Get("Canvas");

            blob1 = new Blob(Color.LightBlue, 0, new Vector2(-3f, -1f));
            blob2 = new Blob(Color.Orange, 1, new Vector2(3f, -1f));
            ground = new Ground();

            Camera.Position += new Vector2(0f, -GameManager.Instance.GraphicsDevice.Viewport.Height * 0.5f);
            Camera.Scale = new Vector2(0.5f);

            World.Gravity = new Vector2(0f, 30f);
        }

        protected override void OnUpdate(float deltaTime)
        {
            GamePadState state = GamePad.GetState(0);
            Camera.Position += new Vector2(state.ThumbSticks.Right.X * 1000f, 0f) * deltaTime;

            if (state.Buttons.Start == ButtonState.Pressed)
                GameManager.Instance.LoadScene(new BattleScene());
                
            cameraCurrent = Camera.Position;
            cameraTarget = (blob1.Position + blob2.Position) / 2f;
            Camera.Position = new Vector2(ConvertUnits.ToDisplayUnits(cameraTarget.X) - Camera.Origin.X, ConvertUnits.ToDisplayUnits(cameraTarget.Y) - Camera.Origin.Y * 2f);


        }
        
        protected override void OnDraw(SpriteBatch spriteBatch)
        {
        }

        protected override void OnDrawGUI(SpriteBatch spriteBatch)
        {
        }

        protected override void OnDestroy()
        {
        }

    }
}
