using BlobFighters.Core;
using FarseerPhysics;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

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
            spriteBatch.DrawRectangle(new RectangleF(ConvertUnits.ToDisplayUnits(new Vector2(-GroundWidth * 0.5f, 0)),
                new Size2(ConvertUnits.ToDisplayUnits(GroundWidth), ConvertUnits.ToDisplayUnits(GroundDepth))), Color.Black, 5f);
        }

        protected override void OnDrawGUI(SpriteBatch spriteBatch)
        {
        }

        protected override void OnDestroy()
        {
        }
    }
}
