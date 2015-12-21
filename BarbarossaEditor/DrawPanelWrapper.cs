using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BarbarossaShared;
using MyControls;
using System.Drawing;
using SFML.System;

namespace BarbarossaEditor
{
    class DrawPanelWrapper : IGraphicsDevice
    {
        Graphics g;
        Vector2f _origin;

        public void SetUnderlying(object u)
        {
            g = (Graphics)u;
            g.TranslateTransform(_origin.X,_origin.Y);
        }

        public void Clear()
        {
            g.Clear(Color.CornflowerBlue);
        }

        public object GetDrawer()
        {
            return g;
        }


        public void SetOrigin(Vector2f origin)
        {
            _origin = origin;
        }
    }
}
