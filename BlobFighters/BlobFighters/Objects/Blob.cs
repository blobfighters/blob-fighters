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

        private Body body;
        private Body head;
        private readonly Body leftArm;
        private readonly Body rightArm;

        private List<Body> ownedBodies;

        private int numGroundContacts;

        private float bodyMovementForce;

        public Blob() : base("Blob", new Vector2(0f, -1f), 0f)
        {
            ownedBodies = new List<Body>();

            numGroundContacts = 0;
            bodyMovementForce = BodyAirMovementForce;

            CreateBody();
            CreateHead();
            leftArm = CreateArm(BodyWidth * 0.5f);
            rightArm = CreateArm(-BodyWidth * 0.5f);
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

        private Body CreateArm(float offset)
        {
            Body shoulder = CreateArmSegment(Position - new Vector2(offset, BodyHeight));
            Body forearm = CreateArmSegment(Position - new Vector2(offset, BodyHeight - ArmLength));

            JointFactory.CreateWeldJoint(Scene.World, shoulder, forearm, new Vector2(0f, -ArmLength * 0.5f), new Vector2(0f, ArmLength * 0.5f)).FrequencyHz = ArmStiffness;
            JointFactory.CreateWeldJoint(Scene.World, body, forearm, new Vector2(offset, -BodyHeight), new Vector2(0f, -ArmLength * 0.5f)).FrequencyHz = ArmStiffness;

            shoulder.IgnoreCollisionWith(body);
            shoulder.IgnoreCollisionWith(head);
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
