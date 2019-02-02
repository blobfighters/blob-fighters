using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BlobFighters.Core
{
    public class PhysicsGameObject : GameObject
    {
        protected Body Body { get; private set; }

        public PhysicsGameObject(string name = null, Vector2 position = default(Vector2), float rotation = 0f) : base(name, position, rotation)
        {
            Body = new Body(Scene.World, position, rotation, BodyType.Dynamic);
        }

        protected override void OnUpdate(float deltaTime)
        {
            Position = Body.Position;
            Rotation = Body.Rotation;
        }

        protected override void OnDraw(SpriteBatch spriteBatch)
        {
        }

        protected override void OnDestroy()
        {
            Console.WriteLine(Name + " destroyed!");
            Scene.World.RemoveBody(Body);
        }
    }
}
