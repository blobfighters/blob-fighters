using BlobFighters.Core;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobFighters.Scenes
{
    public class TestScene : Scene
    {
        protected override void OnDestroy()
        {
            Console.WriteLine("OnDestroy");
        }

        protected override void OnDraw(SpriteBatch spriteBatch)
        {
            Console.WriteLine("OnDraw");
        }

        protected override void OnInit()
        {
            Console.WriteLine("OnInit");
        }

        protected override void OnUpdate(float deltaTime)
        {
            Console.WriteLine("OnUpdate");
        }
    }
}
