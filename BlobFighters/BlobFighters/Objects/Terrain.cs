using BlobFighters.Core;
using FarseerPhysics;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobFighters.Objects
{
    public class Terrain : GameObject, IDrawable
    {
        private readonly Body body;

        private readonly List<Vector2> vertices;

        private Vector2 lastPosition;

        private bool drawing;

        public Terrain(Vector2 position) : base("Terrain", position)
        {
            drawing = true;
            body = new Body(Scene.World);

            lastPosition = position;

            vertices = new List<Vector2>
            {
                Position
            };
        }

        private void DrawSegment()
        {
            body.CreateFixture(new EdgeShape(lastPosition, Position));

            lastPosition = Position;

            vertices.Add(Position);
        }

        protected override void OnDestroy()
        {
        }

        protected override void OnDraw(SpriteBatch spriteBatch)
        {
            Vector2 last = vertices[0];

            foreach (Vector2 v in vertices)
            {
                spriteBatch.DrawLine(ConvertUnits.ToDisplayUnits(last), ConvertUnits.ToDisplayUnits(v), Color.Black, 5f);
                last = v;
            }
        }

        protected override void OnDrawGUI(SpriteBatch spriteBatch)
        {
        }

        protected override void OnUpdate(float deltaTime)
        {
            if (drawing && Position != lastPosition)
                DrawSegment();
        }

        public void StopDrawing()
        {
            drawing = false;
        }

        public void SetPosition(Vector2 position)
        {
            Position = position;
        }
    }
}
