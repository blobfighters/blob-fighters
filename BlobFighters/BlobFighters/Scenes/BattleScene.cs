using BlobFighters.Core;
using BlobFighters.Objects;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
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
        public const int StartingNumberOfLives = 3;

        private const float ScalePadding = 1000f;
        private const float MaxHeightDifference = 50f;
        private const int MaxFramesImpaled = 30;
        private const int MaxCollisionsPerFrame = 30;

        private Blob blueBlob;
        private Blob orangeBlob;
        private Ground ground;
        private Countdown countdown;

        HealthIndicator healthP1, healthP2;
        SpriteFont font;

        private Vector2 cameraCurrent;
        private Vector2 cameraTarget;

        private readonly int blueLivesLeft;
        private readonly int orangeLivesLeft;
        private readonly string initialMessage;

        private int blueFrameContacts;
        private int blueFramesImpaled;

        private int orangeContacts;
        private int orangeFramesImpaled;

        public BattleScene(int blueLives, int orangeLives, string initialMessage)
        {
            if (blueLives <= 0)
            {
                this.initialMessage = "Orange won! Let's play again!";
                blueLivesLeft = orangeLivesLeft = StartingNumberOfLives;
            }
            else if (orangeLives <= 0)
            {
                this.initialMessage = "Blue won! Let's play again!";
                blueLivesLeft = orangeLivesLeft = StartingNumberOfLives;
            }
            else
            {
                this.initialMessage = initialMessage;
                blueLivesLeft = blueLives;
                orangeLivesLeft = orangeLives;
            }

            blueFramesImpaled = 0;
            orangeFramesImpaled = 0;
        }

        protected override void OnInit()
        {
            TextureManager.Instance.Load("Images/Cursor", "Cursor");
            TextureManager.Instance.Load("Images/Body", "Body");
            TextureManager.Instance.Load("Images/Face", "Face");
            TextureManager.Instance.Load("Images/Head", "Head");
            TextureManager.Instance.Load("Images/Arm", "Arm");

            blueBlob = new Blob(Color.LightBlue, 0, new Vector2(-3f, -1f));
            orangeBlob = new Blob(Color.Orange, 1, new Vector2(3f, -1f));
            font = GameManager.Instance.Content.Load<SpriteFont>("Percentage");//load the spriteFont file
            healthP2 = new HealthIndicator(font, new Vector2(256, GameManager.Instance.Height - 125), blueBlob, blueLivesLeft);
            healthP1 = new HealthIndicator(font, new Vector2(GameManager.Instance.Width - 448, GameManager.Instance.Height - 125), orangeBlob, orangeLivesLeft);
            ground = new Ground();
            countdown = new Countdown(new Vector2(GameManager.Instance.Width * 0.5f, 256f), font, () =>
            {
                blueBlob.InputEnabled = orangeBlob.InputEnabled = true;
            }, initialMessage, "3", "2", "1", "Go!");

            Camera.Position += new Vector2(0f, -GameManager.Instance.GraphicsDevice.Viewport.Height * 0.5f);
            Camera.Scale = new Vector2(0.5f);

            World.Gravity = new Vector2(0f, 30f);
            World.ContactManager.BeginContact = BeginContact;
        }

        private bool BeginContact(Contact contact)
        {
            AddContact(contact.FixtureA);
            AddContact(contact.FixtureB);

            return true;
        }

        private void AddContact(Fixture fixture)
        {
            if (!(fixture.UserData is BodyPart bodyPart))
                return;

            if (bodyPart.Blob == blueBlob)
                blueFrameContacts++;
            else if (bodyPart.Blob == orangeBlob)
                orangeContacts++;
        }

        protected override void OnUpdate(float deltaTime)
        {
            GamePadState state = GamePad.GetState(0);

            cameraCurrent = Camera.Position;
            cameraTarget = (blueBlob.Position + orangeBlob.Position) / 2f;
            Camera.Position = new Vector2(ConvertUnits.ToDisplayUnits(cameraTarget.X) - Camera.Origin.X, ConvertUnits.ToDisplayUnits(cameraTarget.Y) - Camera.Origin.Y * 2f);

            Camera.Scale = new Vector2(Math.Min(0.5f, GameManager.Instance.Height / (ConvertUnits.ToDisplayUnits(blueBlob.Position - orangeBlob.Position).Length() + ScalePadding)));

            // DEATH ZONE

            if (blueFrameContacts > 0)
                blueFramesImpaled++;
            else
                blueFramesImpaled = 0;

            if (orangeContacts > 0)
                orangeFramesImpaled++;
            else
                orangeFramesImpaled = 0;

            if (blueBlob.Forfeited)
                GameManager.Instance.LoadScene(new BattleScene(blueLivesLeft - 1, orangeLivesLeft, "Blue forfeited!"));
            else if (orangeBlob.Forfeited)
                GameManager.Instance.LoadScene(new BattleScene(blueLivesLeft, orangeLivesLeft - 1, "Orange forfeited!"));
            else if (blueBlob.IsDead)
                GameManager.Instance.LoadScene(new BattleScene(blueLivesLeft - 1, orangeLivesLeft, "Blue ran out of health!"));
            else if (orangeBlob.IsDead)
                GameManager.Instance.LoadScene(new BattleScene(blueLivesLeft, orangeLivesLeft - 1, "Orange ran out of health!"));
            else if (Math.Abs(blueBlob.Position.Y - orangeBlob.Position.Y) > MaxHeightDifference)
            {
                if (blueBlob.Position.Y > orangeBlob.Position.Y)
                    GameManager.Instance.LoadScene(new BattleScene(blueLivesLeft - 1, orangeLivesLeft, "Blue fell too far!"));
                else
                    GameManager.Instance.LoadScene(new BattleScene(blueLivesLeft, orangeLivesLeft - 1, "Orange fell too far!"));
            }
            else if (blueFrameContacts > MaxCollisionsPerFrame || blueFramesImpaled > MaxFramesImpaled)
                GameManager.Instance.LoadScene(new BattleScene(blueLivesLeft - 1, orangeLivesLeft, "Blue got trapped!"));
            else if (orangeContacts > MaxCollisionsPerFrame || orangeFramesImpaled > MaxFramesImpaled)
                GameManager.Instance.LoadScene(new BattleScene(blueLivesLeft, orangeLivesLeft - 1, "Orange got trapped!"));

            blueFrameContacts = 0;
            orangeContacts = 0;
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
