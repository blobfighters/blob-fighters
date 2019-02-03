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
        private const float ScalePadding = 1000f;

        private Blob blob1;
        private Blob blob2;
        private Ground ground;
        PercentageIndicator healthP1, healthP2;
        SpriteFont font;

        private Vector2 cameraCurrent;
        private Vector2 cameraTarget;

        private Vector2 materialsPosition1;
        private Vector2 materialsPosition2;

        protected override void OnInit()
        {
            TextureManager.Instance.Load("Images/Cursor", "Cursor");
            TextureManager.Instance.Load("Images/Body", "Body");
            TextureManager.Instance.Load("Images/Face", "Face");
            TextureManager.Instance.Load("Images/Head", "Head");
            TextureManager.Instance.Load("Images/Arm", "Arm");

            TextureManager.Instance.Load("Images/Terrain", "Terrain");
            TextureManager.Instance.Load("Images/Wood", "Wood");
            TextureManager.Instance.Load("Images/GunPowder", "GunPowder");

            TextureManager.Instance.Load("Images/MaterialBorder", "MaterialBorder");

            //BackgroundTexture = TextureManager.Instance.Get("Canvas");

            blob1 = new Blob(Color.LightBlue, 0, new Vector2(-3f, -1f));
            blob2 = new Blob(Color.Orange, 1, new Vector2(3f, -1f));
            font = GameManager.Instance.Content.Load<SpriteFont>("Percentage");//load the spriteFont file
            healthP1 = new PercentageIndicator(font, new Vector2(GameManager.Width - 1035, GameManager.Height - 125),blob1);
            healthP2 = new PercentageIndicator(font, new Vector2(GameManager.Width - 365, GameManager.Height - 125),blob2);
            ground = new Ground();

            Camera.Position += new Vector2(0f, -GameManager.Instance.GraphicsDevice.Viewport.Height * 0.5f);
            Camera.Scale = new Vector2(0.5f);

            materialsPosition1 = new Vector2(GameManager.Width - 1100, GameManager.Height - 30);
            new MaterialBrowser(materialsPosition1, blob1.PlayerId);
            materialsPosition2 = new Vector2(GameManager.Width - 430, GameManager.Height - 30);
            new MaterialBrowser(materialsPosition2, blob2.PlayerId);

            World.Gravity = new Vector2(0f, 30f);
        }

        protected override void OnUpdate(float deltaTime)
        {
            GamePadState state = GamePad.GetState(0);

            if (state.Buttons.Start == ButtonState.Pressed)
                GameManager.Instance.LoadScene(new BattleScene());
                
            cameraCurrent = Camera.Position;
            cameraTarget = (blob1.Position + blob2.Position) / 2f;
            Camera.Position = new Vector2(ConvertUnits.ToDisplayUnits(cameraTarget.X) - Camera.Origin.X, ConvertUnits.ToDisplayUnits(cameraTarget.Y) - Camera.Origin.Y * 2f);

            Camera.Scale = new Vector2(Math.Min(0.5f, GameManager.Height / (ConvertUnits.ToDisplayUnits(blob1.Position - blob2.Position).Length() + ScalePadding)));
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
