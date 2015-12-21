using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BarbarossaShared;
using SFML.System;
using System.Drawing;

namespace BarbarossaEditor
{
    class EditorDrawableFactory : DrawableFactory
    {
        Dictionary<string, Image> _spriteMap = new Dictionary<string, Image>();
        Brush _dirtBrush, _grassBrush;

        public EditorDrawableFactory()
        {
            _dirtBrush = new SolidBrush(Color.SandyBrown);
            _grassBrush = new SolidBrush(Color.LightGreen);
        }

        public override IDrawable CreateImage(string path)
        {
            if (!_spriteMap.ContainsKey(path))
            {
                Image image = new Bitmap(path);
                _spriteMap.Add(path, image);
            }

            EditorImage drawable = new EditorImage(_spriteMap[path],path);

            return drawable;

        }

        public override IDrawable CreateDrawablePlatform(Vector2f position, Vector2f size)
        {
            return new EditorDrawablePlatform(position, size, _grassThickness, _dirtBrush, _grassBrush);
        }
    }
}
