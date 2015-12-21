using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using SFML.System;

namespace BarbarossaShared
{
    public class Monster : ISaveable, IPassiveCollider
    {
        Vector2f _size;
        public SFML.System.Vector2f Size
        {
            get { return _size; }
        }

        Vector2f _position;
        public SFML.System.Vector2f Position
        {
            get { return _position; }
        }

        IDrawable _drawableObject;

        public Monster(IDrawable drawable)
        {
            _drawableObject = drawable;
        }

        public System.Xml.XmlNode GetSaveNode(System.Xml.XmlDocument doc)
        {
            throw new NotImplementedException();
        }

        public Collision CheckCollision(IActiveCollider activeCollider, SFML.System.Vector2f proposedMovement)
        {
            return CollisionTester.TestCollision(activeCollider, proposedMovement, this, true, true, true, true);
        }

        public bool IsSolid
        {
            get { return false; }
        }
    }
}
