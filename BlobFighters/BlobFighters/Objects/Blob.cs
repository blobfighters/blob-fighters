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
        private const float HeadMaxTorque = 0.25f;
        private const float HeadImpulse = 1f;
        private const float HeadAngleLimit = 0.5f;
        private const float HeadCorrectionFactor = 10f;

        private Body body;
        private Body head;
        private Body leftArm;
        private Body rightArm;

        private RevoluteJoint headJoint;

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
                new Vector2(-BodyWidth * 0.5f, -BodyHeight), // Top left corner
                new Vector2(BodyWidth * 0.5f, -BodyHeight), // Top right corner
                new Vector2(BodyWidth * 0.5f, 0f), // Bottom right corner
                new Vector2(-BodyWidth * 0.5f, 0f), // Bottom left corner
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

            headJoint = JointFactory.CreateRevoluteJoint(Scene.World, body, head, new Vector2(0f, NeckPivot));
            headJoint.MotorEnabled = true;
            headJoint.MaxMotorTorque = HeadMaxTorque;
            headJoint.MotorImpulse = HeadImpulse;
            headJoint.LimitEnabled = true;
            headJoint.LowerLimit = -HeadAngleLimit;
            headJoint.UpperLimit = HeadAngleLimit;
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

            headJoint.MotorSpeed = -headJoint.JointAngle * HeadCorrectionFactor;

            if (state.Buttons.A == ButtonState.Pressed && numGroundContacts > 0)
                body.ApplyLinearImpulse(new Vector2(0f, -10f));
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
