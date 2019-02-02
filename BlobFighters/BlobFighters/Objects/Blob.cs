using BlobFighters.Core;
using FarseerPhysics;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobFighters.Objects
{
    public class Blob : GameObject
    {
        private const float BodyWidth = 0.5f;
        private const float BodyHeight = 1f;

        private Body body;
        private Body head;
        private Body leftArm;
        private Body rightArm;

        private List<Body> ownedBodies;

        public Blob() : base("Blob", Vector2.Zero, 0f)
        {
            ownedBodies = new List<Body>();

            CreateBody();
        }

        private void CreateBody()
        {
            body = new Body(Scene.World, Position);

            body.CreateFixture(new PolygonShape(new FarseerPhysics.Common.Vertices(new Vector2[]
            {
                new Vector2(-BodyWidth * 0.5f, -BodyHeight * 0.5f), // Top left corner
                new Vector2(BodyWidth * 0.5f, -BodyHeight * 0.5f), // Top right corner
                new Vector2(BodyWidth * 0.5f, BodyHeight * 0.5f), // Bottom right corner
                new Vector2(-BodyWidth * 0.5f, BodyHeight * 0.5f), // Bottom left corner
            }), 1f));
        }

        protected override void OnUpdate(float deltaTime)
        {
        }

        protected override void OnDraw(SpriteBatch spriteBatch)
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
