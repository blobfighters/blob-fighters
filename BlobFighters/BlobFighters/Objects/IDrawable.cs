using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobFighters.Objects
{
    public interface IDrawable
    {
        void StopDrawing();
        void SetPosition(Vector2 position);
    }
}
