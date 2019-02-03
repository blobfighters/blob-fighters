using BlobFighters.Core;
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
    public class Button : GameObject
    {
        private readonly Texture2D texture;
        private readonly Rectangle rect;
        private readonly Action clicked;

        private Color color;
        private bool mouseDown;

        public Button(Texture2D texture, Vector2 position, Action clicked, float scale = 1f) : base("Button", position)
        {
            this.texture = texture;
            this.clicked = clicked;

            rect = new Rectangle(position.ToPoint(), new Point(texture.Width, texture.Height));
            rect.Size = (rect.Size.ToVector2() * scale).ToPoint();

            color = Color.White;
            mouseDown = false;
        }

        protected override void OnDrawGUI(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, rect, null, color, 0f, Vector2.Zero, SpriteEffects.None, 0f);
        }

        protected override void OnUpdate(float deltaTime)
        {
            MouseState state = Mouse.GetState();
            
            if (state.Position.X >= rect.X && state.Position.Y > rect.Y &&
                state.Position.X <= rect.Right && state.Position.Y <= rect.Bottom)
            {
                color = Color.Gray;

                if (!mouseDown && state.LeftButton == ButtonState.Pressed)
                    clicked();
            }
            else
            {
                color = Color.White;
            }

            mouseDown = state.LeftButton == ButtonState.Pressed;
        }

        protected override void OnDestroy()
        {
        }

        protected override void OnDraw(SpriteBatch spriteBatch)
        {
        }
    }
}
