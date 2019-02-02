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
        /// <summary>
        /// The physical body associated with this game object.
        /// </summary>
        protected Body Body { get; private set; }

        /// <summary>
        /// Initializes a new <see cref="PhysicsGameObject"/>.
        /// </summary>
        /// <param name="name">The name of the game object.</param>
        /// <param name="position">The starting position of the game object.</param>
        /// <param name="rotation">the starting rotation of the game object.</param>
        public PhysicsGameObject(string name = null, Vector2 position = default(Vector2), float rotation = 0f) : base(name, position, rotation)
        {
            Body = new Body(Scene.World, position, rotation, BodyType.Dynamic);
        }

        /// <summary>
        /// Called when the game object is updated.
        /// </summary>
        /// <param name="deltaTime"></param>
        protected override void OnUpdate(float deltaTime)
        {
            Position = Body.Position;
            Rotation = Body.Rotation;
        }

        /// <summary>
        /// Called when the game object is drawn.
        /// </summary>
        /// <param name="spriteBatch"></param>
        protected override void OnDraw(SpriteBatch spriteBatch)
        {
        }

        /// <summary>
        /// Called when the GUI is drawn.
        /// </summary>
        /// <param name="spriteBatch"></param>
        protected override void OnDrawGUI(SpriteBatch spriteBatch)
        {
        }

        /// <summary>
        /// Called when the game object is destroyed.
        /// </summary>
        protected override void OnDestroy()
        {
            Scene.World.RemoveBody(Body);
        }
    }
}
