using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobFighters.Objects
{
    public class BodyPart
    {
        public BodyPartType BodyPartType { get; private set; }

        public Blob Blob { get; private set; }

        public BodyPart(BodyPartType bodyPartType, Blob blob)
        {
            BodyPartType = bodyPartType;
            Blob = blob;
        }
    }
}
