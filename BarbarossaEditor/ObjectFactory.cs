using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BarbarossaShared;
using SFML.System;

namespace BarbarossaEditor
{
    class ObjectFactory
    {
        EditorDrawableFactory _drawableFactory;

        public Vector2f StandardPlatformSize = new Vector2f(200,100);

        public ObjectFactory(EditorDrawableFactory drawableFactory)
        {
            _drawableFactory = drawableFactory;
        }

        public ObjectConnector CreatePlatform(Vector2f position, Vector2f size)
        {
            IDrawable drawablePlatform = _drawableFactory.CreateDrawablePlatform(position,size) ;
            Platform platform = new Platform(drawablePlatform,position, size);
            return new ObjectConnector(platform,drawablePlatform, ObjectConnector.NextName("platform"));
        }

        public ObjectConnector CreatePlatform(Vector2f midPosition)
        {
            return CreatePlatform(midPosition, StandardPlatformSize);
        }

        public ObjectConnector CreatePlayer(Vector2f startPosition)
        {
            EditorImage drawablePlayer = (EditorImage)_drawableFactory.CreateImage("..\\..\\..\\Bilder\\spieler.png");
            drawablePlayer.UpdatePosition(startPosition);
            Player player = new Player(drawablePlayer, startPosition, drawablePlayer.Size);
            return new ObjectConnector(player, drawablePlayer, ObjectConnector.NextName("player"));
        }
    }
}
