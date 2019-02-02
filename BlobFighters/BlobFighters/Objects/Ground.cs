using BlobFighters.Core;
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
    public class Ground : GameObject
    {
        private const float GroundWidth = 20f;
        private const float GroundDepth = 20f;

        private Body body;

        public Ground() : base("Ground")
        {
            body = new Body(Scene.World, Position, 0f, BodyType.Static);
            body.CreateFixture(new PolygonShape(new FarseerPhysics.Common.Vertices(new Vector2[]
            {
                new Vector2(-GroundWidth * 0.5f, 0),
                new Vector2(GroundWidth * 0.5f, 0),
                new Vector2(GroundWidth * 0.5f, GroundDepth),
                new Vector2(-GroundWidth * 0.5f, GroundDepth),
            }), 1f)).Friction = 1f;
        }

        protected override void OnUpdate(float deltaTime)
        {
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
