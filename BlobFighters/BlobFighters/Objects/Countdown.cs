using BlobFighters.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobFighters.Objects
{
    public class Countdown : GameObject
    {
        private const float TimePerMessage = 1f;

        private readonly SpriteFont font;
        private readonly Action whenComplete;
        private readonly string[] messages;

        private int currentMessageIndex;
        private float timeRemaining;

        public Countdown(Vector2 position, SpriteFont font, Action whenComplete, params string[] messages) : base(position: position)
        {
            this.font = font;
            this.whenComplete = whenComplete;
            this.messages = messages;

            currentMessageIndex = 0;
            timeRemaining = TimePerMessage;
        }

        protected override void OnUpdate(float deltaTime)
        {
            if (currentMessageIndex >= messages.Length)
                return;

            timeRemaining -= deltaTime;

            if (timeRemaining <= 0)
            {
                if (++currentMessageIndex == messages.Length - 1)
                    whenComplete();

                while (timeRemaining <= 0)
                    timeRemaining += TimePerMessage;
            }
        }

        protected override void OnDraw(SpriteBatch spriteBatch)
        {
        }

        protected override void OnDrawGUI(SpriteBatch spriteBatch)
        {
            if (currentMessageIndex >= messages.Length)
                return;

            Vector2 stringSize = font.MeasureString(messages[currentMessageIndex]);

            spriteBatch.DrawString(font, messages[currentMessageIndex], Position - stringSize * 0.5f, Color.Red);
        }

        protected override void OnDestroy()
        {
        }
    }
}
