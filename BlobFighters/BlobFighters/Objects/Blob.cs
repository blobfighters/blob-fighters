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

        private const float MaxAttackDamage = 10f;

        private readonly Color color;

        private readonly int playerId;

        private Body body;
        private Body head;
        private readonly Body leftUpperarm;
        private readonly Body leftForearm;
        private readonly Body rightUpperarm;
        private readonly Body rightForearm;
        private readonly WeldJoint leftShoulder;
        private readonly WeldJoint rightShoulder;

        private readonly Texture2D bodyTexture;
        private readonly Texture2D faceTexture;
        private readonly Texture2D headTexture;
        private readonly Texture2D armTexture;

        private List<Body> ownedBodies;

        private int numGroundContacts;
        private int direction;

        private float bodyMovementForce;
        private float damageRatio;

        private float timeUntilJump;

        public float AttackStrength { get; private set; }

        public Blob(Color color, int playerId, Vector2 position) : base($"{color} Blob", position, 0f)
        {
            this.color = color;
            this.playerId = playerId;
            
            bodyTexture = TextureManager.Instance.Get("Body");
            faceTexture = TextureManager.Instance.Get("Face");
            headTexture = TextureManager.Instance.Get("Head");
            armTexture = TextureManager.Instance.Get("Arm");

            ownedBodies = new List<Body>();

            numGroundContacts = 0;
            direction = 1;

            bodyMovementForce = BodyAirMovementForce;
            damageRatio = 0f;

            timeUntilJump = 0f;

            CreateBody();
            CreateHead();
            CreateArm(ArmSpacing, RestingArmAngle, out leftUpperarm, out leftForearm, out leftShoulder);
            CreateArm(-ArmSpacing, -RestingArmAngle, out rightUpperarm, out rightForearm, out rightShoulder);

            leftUpperarm.IgnoreCollisionWith(rightForearm);
            rightUpperarm.IgnoreCollisionWith(leftForearm);
        }

        private void CreateBody()
        {
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
            }), 1f));

            Fixture baseFixture = body.CreateFixture(new CircleShape(BodyWidth * 0.5f, BodyDensity));
            baseFixture.Friction = BodyFriction;
            baseFixture.OnCollision = OnBaseCollision;
            baseFixture.OnSeparation = OnBaseSeparation;
        }

        private void CreateHead()
        {
            head = new Body(Scene.World, Position - new Vector2(0f, BodyHeight + NeckLength), 0f, BodyType.Dynamic);
            head.CreateFixture(new CircleShape(BodyWidth * 0.5f, HeadDensity));

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
            }), 0.5f));

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

        private void DrawBodyPart(Body body, Texture2D texture, Color color, SpriteBatch spriteBatch, Vector2? scale = null, Vector2 offset = default(Vector2))
        {
            spriteBatch.Draw(texture, ConvertUnits.ToDisplayUnits(body.Position), null, color, body.Rotation, new Vector2(texture.Width * 0.5f, texture.Height * 0.5f) + ConvertUnits.ToDisplayUnits(offset),
                scale ?? Vector2.One, SpriteEffects.None, 0f);
        }

        private void DrawBody(SpriteBatch spriteBatch)
        {
            DrawBodyPart(body, bodyTexture, color, spriteBatch, null, new Vector2(0f, (BodyHeight - BodyWidth * 0.5f) * 0.5f));
        }

        private void DrawLeftArm(SpriteBatch spriteBatch)
        {
            DrawBodyPart(leftForearm, armTexture, color, spriteBatch, new Vector2(ForearmScale, 1f));
            DrawBodyPart(leftUpperarm, armTexture, color, spriteBatch);
        }

        private void DrawRightArm(SpriteBatch spriteBatch)
        {
            DrawBodyPart(rightForearm, armTexture, color, spriteBatch, new Vector2(ForearmScale, 1f));
            DrawBodyPart(rightUpperarm, armTexture, color, spriteBatch);
        }

        protected override void OnUpdate(float deltaTime)
        {
            GamePadState state = GamePad.GetState(playerId);

            timeUntilJump = Math.Max(timeUntilJump - deltaTime, 0f);

            if (Math.Abs(state.ThumbSticks.Left.X) >= Deadzone)
            {
                if (state.ThumbSticks.Left.X > 0)
                    direction = -1;
                else if (state.ThumbSticks.Left.X < 0)
                    direction = 1;

                body.ApplyForce(new Vector2(state.ThumbSticks.Left.X * bodyMovementForce, 0f));
            }

            body.ApplyTorque(-body.Rotation * BodyRotationForce);

            if (state.IsButtonDown(Mappings.Jump) && timeUntilJump == 0f && numGroundContacts > 0)
            {
                body.ApplyLinearImpulse(new Vector2(0f, -BodyJumpForce));
                timeUntilJump = JumpCooldown;
            }

            if (state.IsButtonDown(Mappings.Attack))
            {
                leftShoulder.ReferenceAngle = PunchingArmAngle * direction;
                rightShoulder.ReferenceAngle = PunchingArmAngle * direction;
            }
            else
            {
                leftShoulder.ReferenceAngle = RestingArmAngle;
                rightShoulder.ReferenceAngle = -RestingArmAngle;
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

            DrawBodyPart(head, headTexture, color, spriteBatch);
            DrawBodyPart(head, faceTexture, Color.White, spriteBatch, null, new Vector2(direction * 0.1f, 0f));
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
