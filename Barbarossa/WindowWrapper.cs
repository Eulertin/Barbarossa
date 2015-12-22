using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BarbarossaShared;
using SFML.Graphics;

namespace Barbarossa
{
    class WindowWrapper : IGraphicsDevice
    {
        RenderWindow _window;
        View _view;

        public WindowWrapper(RenderWindow window)
        {
            _window = window;
            _view = window.GetView();
        }

        public void Clear()
        {
            _window.Clear(new Color(50,127,255));
        }

        public void SetUnderlying(object u)
        {
            throw new NotImplementedException();
        }

        public object GetDrawer()
        {
            return _window;
        }

        public void SetOrigin(SFML.System.Vector2f origin)
        {
            _view.Center = origin;
            _window.SetView(_view);
        }
    }
}
