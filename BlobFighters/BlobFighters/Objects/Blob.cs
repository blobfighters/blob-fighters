using BlobFighters.Core;
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

        private const float BodyJumpForce = 10f;

        private const float NeckLength = 0.15f;
        private const float NeckPivot = 0.25f;

        private const float HeadDensity = 0.1f;
        private const float HeadStiffness = 4f;

        private const float ArmLength = 0.5f;
        private const float ArmWidth = 0.15f;
        private const float ArmStiffness = 7.5f;
        private const float ArmAngle = 0.5f;

        private const float ForearmScale = 0.75f;

        private readonly Color color;

        private Body body;
        private Body head;
        private readonly Body leftUpperarm;
        private readonly Body leftForearm;
        private readonly Body rightUpperarm;
        private readonly Body rightForearm;

        private readonly Texture2D bodyTexture;
        private readonly Texture2D faceTexture;
        private readonly Texture2D headTexture;
        private readonly Texture2D armTexture;

        private List<Body> ownedBodies;

        private int numGroundContacts;

        private float bodyMovementForce;

        public Blob(Color color, Vector2 position) : base($"{color} Blob", position, 0f)
        {
            this.color = color;
            
            bodyTexture = TextureManager.Instance.Get("Body");
            faceTexture = TextureManager.Instance.Get("Face");
            headTexture = TextureManager.Instance.Get("Head");
            armTexture = TextureManager.Instance.Get("Arm");

            ownedBodies = new List<Body>();

            numGroundContacts = 0;
            bodyMovementForce = BodyAirMovementForce;

            CreateBody();
            CreateHead();
            CreateArm(-BodyWidth * 0.5f, -ArmAngle, out leftUpperarm, out leftForearm);
            CreateArm(BodyWidth * 0.5f, ArmAngle, out rightUpperarm, out rightForearm);
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

        private Body CreateArm(float offset, float angle, out Body upperarm, out Body forearm)
        {
            upperarm = CreateArmSegment(Position - new Vector2(offset, BodyHeight));
            forearm = CreateArmSegment(Position - new Vector2(offset, BodyHeight - ArmLength));

            WeldJoint elbow = JointFactory.CreateWeldJoint(Scene.World, forearm, upperarm, new Vector2(0f, -ArmLength * 0.5f), new Vector2(0f, ArmLength * 0.5f));
            elbow.FrequencyHz = ArmStiffness;

            WeldJoint shoulder = JointFactory.CreateWeldJoint(Scene.World, body, upperarm, new Vector2(-offset, -BodyHeight + NeckLength), new Vector2(0f, -ArmLength * 0.5f));
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

        protected override void OnUpdate(float deltaTime)
        {
            GamePadState state = GamePad.GetState(0);

            body.ApplyForce(new Vector2(state.ThumbSticks.Left.X * bodyMovementForce, 0f));
            body.ApplyTorque(-body.Rotation * BodyRotationForce);

            if (state.Buttons.A == ButtonState.Pressed && numGroundContacts > 0)
                body.ApplyLinearImpulse(new Vector2(0f, -BodyJumpForce));
        }

        protected override void OnDraw(SpriteBatch spriteBatch)
        {
            DrawBodyPart(body, bodyTexture, color, spriteBatch, null, new Vector2(0f, (BodyHeight - BodyWidth * 0.5f) * 0.5f));
            DrawBodyPart(leftForearm, armTexture, color, spriteBatch, new Vector2(ForearmScale, 1f));
            DrawBodyPart(leftUpperarm, armTexture, color, spriteBatch);
            DrawBodyPart(rightForearm, armTexture, color, spriteBatch, new Vector2(ForearmScale, 1f));
            DrawBodyPart(rightUpperarm, armTexture, color, spriteBatch);
            DrawBodyPart(head, headTexture, color, spriteBatch);
            DrawBodyPart(head, faceTexture, Color.White, spriteBatch);
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
