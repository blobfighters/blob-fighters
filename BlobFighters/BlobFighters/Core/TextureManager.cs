using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobFighters.Core
{
    public class TextureManager
    {
        private static TextureManager instance;

        public static TextureManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new TextureManager();

                return instance;
            }
        }

        private readonly Dictionary<string, Texture2D> textures;

        public TextureManager()
        {
            textures = new Dictionary<string, Texture2D>();
        }

        public Texture2D Get(string textureId)
        {
            return textures[textureId];
        }

        public void LoadTexture(string path, string textureId)
        {
            if (textures.ContainsKey(textureId))
                throw new InvalidOperationException("Cannot overwrite exsiting texture ID!");

            Texture2D texture = GameManager.Instance.Content.Load<Texture2D>(path);
            textures[textureId] = texture ?? throw new InvalidOperationException($"Could not find the texture {path}.");
        }

        public void ReleaseTextures()
        {
            foreach (Texture2D texture in textures.Values)
                GameManager.Instance.Content.Unload();

            textures.Clear();
        }
    }
}
