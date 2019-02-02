using System;
using System.Collections.Generic;
using BlobFighters.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoEngine.Core;

namespace BlobFighters.Objects
{
    public class MaterialBrowser : GameObject
    {

        readonly List<Texture2D> materials = new List<Texture2D>();

        private readonly Texture2D materialBorder = TextureManager.Instance.Get("MaterialBorder");

        int index = 0;

        public MaterialBrowser(Vector2 position) : base("MaterialBrowser", position)
        {
            Texture2D terrain = TextureManager.Instance.Get("Terrain");
            Texture2D wood = TextureManager.Instance.Get("Wood");
            Texture2D gunPowder = TextureManager.Instance.Get("GunPowder");
            Texture2D material4 = TextureManager.Instance.Get("Wood");

            materials.Add(terrain);
            materials.Add(wood);
            materials.Add(gunPowder);
            InputManager.Instance.OnButtonStateChanged += Instance_OnButtonStateChanged;
        }



        protected override void OnDestroy()
        {
        }

        protected override void OnDraw(SpriteBatch spriteBatch)
        {
            Vector2 position = new Vector2(-GameManager.Instance.GraphicsDevice.Viewport.Width / 4, GameManager.Instance.GraphicsDevice.Viewport.Height / 2);

            for (int x = 0; x < materials.Count; x++)
            {
                spriteBatch.Draw(materials[x], position, Color.White);
                if (x == index)
                    spriteBatch.Draw(materialBorder, position, Color.White);
                position.X += 60f;
            }

        }

        protected override void OnUpdate(float deltaTime)
        {
        }

        void Instance_OnButtonStateChanged(int playerID, Buttons button, ButtonState state)
        {
            if (state != ButtonState.Pressed)
                return;
            switch (button) 
            {
                case Buttons.LeftShoulder:
                    MoveLeft();
                    break;

                case Buttons.RightShoulder:
                    MoveRight();
                    break;
            }
        }


        public void MoveLeft()
        {
            GamePadState state = GamePad.GetState(0);       //might need to change this id later for multiplayer
            if (state.Buttons.LeftShoulder == ButtonState.Pressed)
            {
                if (index == 0)
                    index = materials.Count - 1;
                else
                    index--;
            }
        }

        public void MoveRight()
        {
            GamePadState state = GamePad.GetState(0);       //might need to change this id later for multiplayer
            if (state.Buttons.RightShoulder == ButtonState.Pressed)
            {
                if (index == materials.Count - 1)
                    index = 0;
                else
                    index++;
            }
        }
    }
}
