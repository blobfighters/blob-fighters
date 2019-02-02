using BlobFighters.Core;
using FarseerPhysics;
using FarseerPhysics.Collision.Shapes;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobFighters.Tests
{
    public class TestPhysicsGameObject : PhysicsGameObject
    {
        public TestPhysicsGameObject() : base("TestPhysicsGameObject")
        {
            Body.CreateFixture(new CircleShape(1, 1f));
            Body.LinearDamping = 1f;
        }

        protected override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);

            MouseState state = Mouse.GetState();

            Body.ApplyForce((ConvertUnits.ToSimUnits(state.Position.ToVector2()) - Body.Position) * 10f);

            if (state.RightButton == ButtonState.Pressed)
                Destroy();
        }

        protected override void OnDraw(SpriteBatch spriteBatch)
        {
            base.OnDraw(spriteBatch);
        }
    }
}
