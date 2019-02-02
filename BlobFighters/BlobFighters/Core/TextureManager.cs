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
        /// <summary>
        /// The private <see cref="TextureManager"/> singleton.
        /// </summary>
        private static TextureManager instance;

        /// <summary>
        /// The <see cref="TextureManager"/> singleton instance.
        /// </summary>
        public static TextureManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new TextureManager();

                return instance;
            }
        }

        /// <summary>
        /// A map of all the currently-loaded textures.
        /// </summary>
        private readonly Dictionary<string, Texture2D> textures;

        /// <summary>
        /// Initializes the <see cref="TextureManager"/>.
        /// </summary>
        private TextureManager()
        {
            textures = new Dictionary<string, Texture2D>();
        }

        /// <summary>
        /// Returns a previously-loaded <see cref="Texture2D"/> with the given ID.
        /// </summary>
        /// <param name="textureId"></param>
        /// <returns></returns>
        public Texture2D Get(string textureId)
        {
            return textures[textureId];
        }

        /// <summary>
        /// Loads the given texture and assigns it the given ID.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="textureId"></param>
        public void Load(string path, string textureId)
        {
            if (textures.ContainsKey(textureId))
                throw new InvalidOperationException("Cannot overwrite exsiting texture ID!");

            Texture2D texture = GameManager.Instance.Content.Load<Texture2D>(path);
            textures[textureId] = texture ?? throw new InvalidOperationException($"Could not find the texture {path}.");
        }

        /// <summary>
        /// Releases all loaded textures.
        /// </summary>
        public void Release()
        {
            foreach (Texture2D texture in textures.Values)
                GameManager.Instance.Content.Unload();

            textures.Clear();
        }
    }
}
