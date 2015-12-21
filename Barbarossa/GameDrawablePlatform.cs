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
    class GameDrawablePlatform : IDrawable
    {
        RectangleShape _dirt, _grass;

        public GameDrawablePlatform(Vector2f position, Vector2f size,
            float grassThickness, Color dirtColor, Color grassColor)
        {
            _dirt = new RectangleShape(size);
            _dirt.Position = position;
            _dirt.FillColor = dirtColor;

            _grass = new RectangleShape(new Vector2f(size.X, Math.Min(size.Y, grassThickness)));
            _grass.Position = position;
            _grass.FillColor = grassColor;
        }

        public void Draw(IRenderTarget target)
        {
            RenderWindow window = (RenderWindow)target.GetDrawer();

            window.Draw(_dirt);
            window.Draw(_grass);
        }

        public void UpdatePosition(Vector2f position)
        {
            _dirt.Position = position;
            _grass.Position = position;
        }
    }
}
