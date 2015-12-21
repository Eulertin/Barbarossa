using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;

namespace BarbarossaShared
{
    public interface IDrawable
    {
        void Draw(IRenderTarget target);

        void UpdatePosition(Vector2f position);
    }

    interface ScrollCenter
    {
        //public Vector2f Position { get; }
    }
}
