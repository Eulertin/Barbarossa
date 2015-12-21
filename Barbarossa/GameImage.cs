using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;
using BarbarossaShared;

namespace Barbarossa
{
    class GameImage : IDrawable
    {
        Sprite _sprite;
        Vector2f _position;
        
        public GameImage(Sprite sprite)
        {
            _sprite = sprite;
        }

        public void Draw(BarbarossaShared.IRenderTarget target)
        {
            _sprite.Position = _position;
            (target.GetDrawer() as RenderWindow).Draw(_sprite);
        }


        public void UpdatePosition(Vector2f position)
        {
            _position = position;
        }
    }
}
