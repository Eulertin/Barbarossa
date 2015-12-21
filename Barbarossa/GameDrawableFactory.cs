using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using SFML.Graphics;
using BarbarossaShared;

namespace Barbarossa
{
    class GameDrawableFactory : DrawableFactory
    {
        Dictionary<string, Sprite> _spriteMap = new Dictionary<string, Sprite>();

        Color _dirtColor = new Color(200,100,20);
        Color _grassColor = new Color(60, 230, 80);

        public override IDrawable CreateImage(string path)
        {
            if (!_spriteMap.ContainsKey(path))
            {
                Texture texture = new Texture(path);
                Sprite sprite = new Sprite(texture);
                _spriteMap.Add(path, sprite);
            }

            GameImage drawable = new GameImage(_spriteMap[path]);

            return drawable;
        }

        public override IDrawable CreateDrawablePlatform(Vector2f position, Vector2f size)
        {
            return new GameDrawablePlatform(position, size, _grassThickness, _dirtColor, _grassColor);
        }
    }
}
