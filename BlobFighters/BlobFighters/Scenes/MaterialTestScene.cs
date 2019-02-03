using System;
using BlobFighters.Core;
using BlobFighters.Objects;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace BlobFighters.Scenes
{
    public class MaterialTestScene : Scene
    {

        protected override void OnDestroy()
        {
        }

        protected override void OnDraw(SpriteBatch spriteBatch)
        {
        }

        protected override void OnDrawGUI(SpriteBatch spriteBatch)
        {
        }

        protected override void OnInit()
        {
            BackgroundColor = Color.Black;

            TextureManager.Instance.Load("Images/Terrain", "Terrain");
            TextureManager.Instance.Load("Images/Wood", "Wood");
            TextureManager.Instance.Load("Images/GunPowder", "GunPowder");

            TextureManager.Instance.Load("Images/MaterialBorder", "MaterialBorder");
            Vector2 position = new Vector2(5f, 2f);
            new MaterialBrowser(position);
        }

        protected override void OnUpdate(float deltaTime)
        {
        }
    }
}
