using BlobFighters.Core;
using BlobFighters.InputManagement;
using FarseerPhysics; 
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoEngine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobFighters.Objects
{
    public class Blob : GameObject
    {
        private const float BodyWidth = 0.75f;
        private const float BodyHeight = 1f;

        private const float BodyAngularDamping = 17.5f;
        private const float BodyGroundLinearDamping = 2.5f;
        private const float BodyAirLinearDamping = 0.5f;
        private const float BodyDensity = 1f;
        private const float BodyFriction = 0.25f;

        private const float BodyGroundMovementForce = 60f;
        private const float BodyAirMovementForce = 20f;

        private const float BodyRotationForce = 40f;

        private const float BodyJumpForce = 25f;

        private const float NeckLength = 0.15f;
        private const float NeckPivot = 0.25f;

        private const float HeadDensity = 0.1f;
        private const float HeadStiffness = 4f;

        private const float ArmLength = 0.5f;
        private const float ArmWidth = 0.15f;
        private const float ArmStiffness = 15f;
        private const float ArmSpacing = 0.25f;

        private const float RestingArmAngle = 0.25f;
        private const float PunchingArmAngle = 1.75f;

        private const float ForearmScale = 0.75f;

        private const float Deadzone = 0.25f;

        private const float JumpCooldown = 0.25f;

        private const float MaxAttackStrength = 20f;
        private const float AttackDecayRate = 5f;
        private const float VerticalHitBias = 2f;
        private const float BodyAttackScale = 1.5f;
        private const float HeadAttackScale = 4f;

        private const float DrawSpeed = 10f;
        private const float MaxDrawDistance = 2.5f;
        private const float MaxHealth = 100f;
        private const float HealthDamageFactor = 0.5f;

        private readonly Vector2 cursorOffset = new Vector2(BodyWidth, BodyWidth * 0.5f);

        public Color Color { get; private set; }

        private Body body;
        private Body head;
        private readonly Body leftUpperarm;
        private readonly Body leftForearm;
        private readonly Body rightUpperarm;
        private readonly Body rightForearm;
        private readonly WeldJoint leftShoulder;
        private readonly WeldJoint rightShoulder;

        private readonly Texture2D cursorTexture;
        private readonly Texture2D bodyTexture;
        private readonly Texture2D faceTexture;
        private readonly Texture2D headTexture;
        private readonly Texture2D armTexture;

        private Vector2 cursorPosition;

        private List<Body> ownedBodies;

        private IDrawable currentDrawable;

        private int numGroundContacts;
        private int direction;

        private float bodyMovementForce;

        private float timeUntilJump;

        public bool InputEnabled { get; set; }

        public int PlayerId { get; private set; }

        public float Health { get; set; }

        public bool Forfeited { get; private set; }

        public float AttackStrength { get; private set; }

        public bool IsDead => Health <= 0f;

        public Vector2 AbsoluteCursorPosition => Position + cursorOffset * new Vector2(-direction, 1f) + cursorPosition;

        public Blob(Color color, int playerId, Vector2 position) : base($"{color} Blob", position, 0f)
        {
            InputEnabled = false;
            Color = color;
            PlayerId = playerId;
            Health = MaxHealth;
            Forfeited = false;

            cursorTexture = TextureManager.Instance.Get("Cursor");
            bodyTexture = TextureManager.Instance.Get("Body");
            faceTexture = TextureManager.Instance.Get("Face");
            headTexture = TextureManager.Instance.Get("Head");
            armTexture = TextureManager.Instance.Get("Arm");

            cursorPosition = Vector2.Zero;

            ownedBodies = new List<Body>();

            numGroundContacts = 0;
            direction = 1;

            bodyMovementForce = BodyAirMovementForce;

            timeUntilJump = 0f;

            CreateBody();
            CreateHead();
            CreateArm(ArmSpacing, RestingArmAngle, out leftUpperarm, out leftForearm, out leftShoulder);
            CreateArm(-ArmSpacing, -RestingArmAngle, out rightUpperarm, out rightForearm, out rightShoulder);

            InputManager.Instance.OnButtonStateChanged += ButtonStateChanged;
        }

        private void ButtonStateChanged(int playerId, Buttons button, ButtonState state)
        {
            if (!InputEnabled || playerId != PlayerId)
                return;

            if (state == ButtonState.Pressed)
            {
                switch (button)
                {
                    case Mappings.Jump:
                        if (timeUntilJump == 0f && numGroundContacts > 0)
                        {
                            body.ApplyLinearImpulse(new Vector2(0f, -BodyJumpForce));
                            timeUntilJump = JumpCooldown;
                        }
                        break;
                    case Mappings.Attack:
                        if (AttackStrength == 0f)
                            AttackStrength = 1f;

                        break;
                }
            }
        }

        private void CreateBody()
        {
            BodyPart bp = new BodyPart(BodyPartType.Body, this);

            body = new Body(Scene.World, Position, 0f, BodyType.Dynamic)
            {
                AngularDamping = BodyAngularDamping,
                LinearDamping = BodyAirLinearDamping
            };

            body.CreateFixture(new PolygonShape(new FarseerPhysics.Common.Vertices(new Vector2[]
            {
                new Vector2(-BodyWidth * 0.5f, -BodyHeight),
                new Vector2(BodyWidth * 0.5f, -BodyHeight),
                new Vector2(BodyWidth * 0.5f, 0f),
                new Vector2(-BodyWidth * 0.5f, 0f),
            }), 1f), bp);

            Fixture baseFixture = body.CreateFixture(new CircleShape(BodyWidth * 0.5f, BodyDensity), bp);
            baseFixture.Friction = BodyFriction;
            baseFixture.OnCollision = OnBaseCollision;
            baseFixture.OnSeparation = OnBaseSeparation;
        }

        private void CreateHead()
        {
            head = new Body(Scene.World, Position - new Vector2(0f, BodyHeight + NeckLength), 0f, BodyType.Dynamic);
            head.CreateFixture(new CircleShape(BodyWidth * 0.5f, HeadDensity), new BodyPart(BodyPartType.Head, this));

            JointFactory.CreateWeldJoint(Scene.World, body, head, new Vector2(0f, -BodyHeight), new Vector2(0f, NeckPivot)).FrequencyHz = HeadStiffness;
        }

        private Body CreateArm(float offset, float angle, out Body upperarm, out Body forearm, out WeldJoint shoulder)
        {
            upperarm = CreateArmSegment(Position - new Vector2(offset, BodyHeight));
            forearm = CreateArmSegment(Position - new Vector2(offset, BodyHeight - ArmLength));

            WeldJoint elbow = JointFactory.CreateWeldJoint(Scene.World, forearm, upperarm, new Vector2(0f, -ArmLength * 0.5f), new Vector2(0f, ArmLength * 0.5f));
            elbow.FrequencyHz = ArmStiffness;

            shoulder = JointFactory.CreateWeldJoint(Scene.World, body, upperarm, new Vector2(-offset, -BodyHeight + NeckLength), new Vector2(0f, -ArmLength * 0.5f));
            shoulder.FrequencyHz = ArmStiffness;
            shoulder.ReferenceAngle = angle;

            upperarm.IgnoreCollisionWith(body);
            upperarm.IgnoreCollisionWith(head);
            forearm.IgnoreCollisionWith(body);
            forearm.IgnoreCollisionWith(head);

            forearm.OnCollision += OnArmCollision;

            return forearm;
        }

        private Body CreateArmSegment(Vector2 position)
        {
            Body segment = new Body(Scene.World, position, 0f, BodyType.Dynamic);

            segment.CreateFixture(new PolygonShape(new FarseerPhysics.Common.Vertices(new Vector2[]
            {
                new Vector2(-ArmWidth * 0.5f, -ArmLength * 0.5f),
                new Vector2(ArmWidth * 0.5f, -ArmLength * 0.5f),
                new Vector2(ArmWidth * 0.5f, ArmLength * 0.5f),
                new Vector2(-ArmWidth * 0.5f, ArmLength * 0.5f)
            }), 0.5f), new BodyPart(BodyPartType.Arm, this));

            return segment;
        }

        private bool OnBaseCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            body.LinearDamping = BodyGroundLinearDamping;
            bodyMovementForce = BodyGroundMovementForce;

            numGroundContacts++;
            return true;
        }

        private void OnBaseSeparation(Fixture fixtureA, Fixture fixtureB)
        {
            if (--numGroundContacts > 0)
                return;

            body.LinearDamping = BodyAirLinearDamping;
            bodyMovementForce = BodyAirMovementForce;
        }

        private bool OnArmCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (!(fixtureB.UserData is BodyPart bodyPart))
                return true;

            if (bodyPart.Blob == this)
                return false;

            if (AttackStrength == 0f)
                return true;

            float attackPower = AttackStrength * MaxAttackStrength;

            switch (bodyPart.BodyPartType)
            {
                case BodyPartType.Body:
                    attackPower *= BodyAttackScale;
                    break;
                case BodyPartType.Head:
                    attackPower *= HeadAttackScale;
                    break;
            }

            AttackStrength = 0f;

            bodyPart.Blob.Health = Math.Max(0f, bodyPart.Blob.Health - attackPower * HealthDamageFactor);
            bodyPart.Blob.body.ApplyLinearImpulse(((bodyPart.Blob.Position - Position) - Vector2.UnitY * VerticalHitBias) * attackPower * (1 - bodyPart.Blob.Health / MaxHealth));

            return true;
        }

        private void DrawBodyPart(Body body, Texture2D texture, Color color, SpriteBatch spriteBatch, Vector2? scale = null, Vector2 offset = default(Vector2))
        {
            spriteBatch.Draw(texture, ConvertUnits.ToDisplayUnits(body.Position), null, color, body.Rotation,
                new Vector2(texture.Width * 0.5f, texture.Height * 0.5f) + ConvertUnits.ToDisplayUnits(offset),
                scale ?? Vector2.One, SpriteEffects.None, 0f);
        }

        private void DrawBody(SpriteBatch spriteBatch)
        {
            DrawBodyPart(body, bodyTexture, Color, spriteBatch, null, new Vector2(0f, (BodyHeight - BodyWidth * 0.5f) * 0.5f));
        }

        private void DrawLeftArm(SpriteBatch spriteBatch)
        {
            DrawBodyPart(leftForearm, armTexture, Color, spriteBatch, new Vector2(ForearmScale, 1f));
            DrawBodyPart(leftUpperarm, armTexture, Color, spriteBatch);
        }

        private void DrawRightArm(SpriteBatch spriteBatch)
        {
            DrawBodyPart(rightForearm, armTexture, Color, spriteBatch, new Vector2(ForearmScale, 1f));
            DrawBodyPart(rightUpperarm, armTexture, Color, spriteBatch);
        }

        protected override void OnUpdate(float deltaTime)
        {
            Position = body.Position;
            Rotation = body.Rotation;

            body.ApplyTorque(-body.Rotation * BodyRotationForce);

            if (!InputEnabled)
                return;

            GamePadState state = GamePad.GetState(PlayerId);

            if (state.IsButtonDown(Mappings.Forfeit))
                Forfeited = true;

            timeUntilJump = Math.Max(timeUntilJump - deltaTime, 0f);
            AttackStrength = Math.Max(AttackStrength - deltaTime * AttackDecayRate, 0f);

            if (Math.Abs(state.ThumbSticks.Left.X) >= Deadzone)
            {
                if (currentDrawable == null)
                {
                    if (state.ThumbSticks.Left.X > 0)
                        direction = -1;
                    else if (state.ThumbSticks.Left.X < 0)
                        direction = 1;
                }

                body.ApplyForce(new Vector2(state.ThumbSticks.Left.X * bodyMovementForce, 0f));
            }

            if (AttackStrength > 0)
            {
                leftShoulder.ReferenceAngle = PunchingArmAngle * direction;
                rightShoulder.ReferenceAngle = PunchingArmAngle * direction;
            }
            else
            {
                leftShoulder.ReferenceAngle = RestingArmAngle;
                rightShoulder.ReferenceAngle = -RestingArmAngle;
            }

            if (state.Triggers.Right > 0.5f)
            {
                cursorPosition += new Vector2(state.ThumbSticks.Right.X, -state.ThumbSticks.Right.Y) * DrawSpeed * deltaTime;

                if (cursorPosition.Length() > MaxDrawDistance)
                {
                    cursorPosition.Normalize();
                    cursorPosition *= MaxDrawDistance;
                }

                if (currentDrawable == null)
                    currentDrawable = new Terrain(AbsoluteCursorPosition);

                currentDrawable?.SetPosition(AbsoluteCursorPosition);
            }
            else
            {
                cursorPosition = new Vector2(state.ThumbSticks.Right.X, -state.ThumbSticks.Right.Y) * MaxDrawDistance;

                if (currentDrawable != null)
                {
                    currentDrawable.StopDrawing();
                    currentDrawable = null;
                }
            }
        }

        protected override void OnDraw(SpriteBatch spriteBatch)
        {
            if (direction == -1)
            {
                DrawRightArm(spriteBatch);
                DrawBody(spriteBatch);
                DrawLeftArm(spriteBatch);
            }
            else
            {
                DrawLeftArm(spriteBatch);
                DrawBody(spriteBatch);
                DrawRightArm(spriteBatch);
            }

            DrawBodyPart(head, headTexture, Color, spriteBatch);
            DrawBodyPart(head, faceTexture, Color.White, spriteBatch, null, new Vector2(direction * 0.1f, 0f));

            spriteBatch.Draw(cursorTexture, ConvertUnits.ToDisplayUnits(AbsoluteCursorPosition), null, Color.White, 0f,
                new Vector2(cursorTexture.Width, cursorTexture.Height) * 0.5f, 1f, SpriteEffects.None, 0f);
        }

        protected override void OnDrawGUI(SpriteBatch spriteBatch)
        {
        }

        protected override void OnDestroy()
        {
            foreach (Body body in ownedBodies)
                Scene.World.RemoveBody(body);

            ownedBodies.Clear();
        }
    }
}
