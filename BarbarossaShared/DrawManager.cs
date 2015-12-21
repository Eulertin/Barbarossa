using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarbarossaShared
{
    public class DrawManager
    {
        List<IDrawable> _drawableObjectList = new List<IDrawable>();

        IGraphicsDevice _graphicsDevice;

        public DrawManager(IGraphicsDevice graphicsDevice)
        {
            _graphicsDevice = graphicsDevice;
        }

        public void UpdateView()
        {
            
        }

        public void AddObject(IDrawable drawable)
        {
            if (_drawableObjectList.Contains(drawable))
                throw new Exception("Das Objekt liegt bereits vor!");
            else
                _drawableObjectList.Add(drawable);
        }

        public void Draw()
        {
            _graphicsDevice.Clear();

            foreach (IDrawable drawable in _drawableObjectList)
            {
                drawable.Draw(_graphicsDevice);
            }
        }
    }
}
