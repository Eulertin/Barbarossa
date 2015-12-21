using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;

namespace BarbarossaShared
{
    public interface IRenderTarget
    {
        object GetDrawer();
    }

    public interface IGraphicsDevice : IRenderTarget
    {
        void Clear();

        void SetUnderlying(object u);

        void SetOrigin(Vector2f origin);
    }
}
