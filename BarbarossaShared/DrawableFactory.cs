using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;

namespace BarbarossaShared
{
    public abstract class DrawableFactory
    {
        protected float _grassThickness = 20f;

        public abstract IDrawable CreateImage(string path);

        public abstract IDrawable CreateDrawablePlatform(Vector2f position, Vector2f size);
    }
}
