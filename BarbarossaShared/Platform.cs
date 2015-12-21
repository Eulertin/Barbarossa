using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using System.Xml;

namespace BarbarossaShared
{
    public class Platform : ISaveable, IPassiveCollider
    {
        Vector2f _position, _size;

        IDrawable _drawableObject;

        public Platform(IDrawable drawable, Vector2f position, Vector2f size)
        {
            _drawableObject = drawable;
            _position = position;
            _size = size;
        }

        public System.Xml.XmlNode GetSaveNode(System.Xml.XmlDocument doc)
        {
            XmlNode root = doc.CreateElement("Logic");

            XmlNode node = doc.CreateElement("Type");
            XmlAttribute attr = doc.CreateAttribute("type");
            attr.Value = "Platform";
            node.Attributes.Append(attr);
            root.AppendChild(node);

            node = doc.CreateElement("Position");
            attr = doc.CreateAttribute("x");
            attr.Value = _position.X.ToString();
            node.Attributes.Append(attr);
            attr = doc.CreateAttribute("y");
            attr.Value = _position.Y.ToString();
            node.Attributes.Append(attr);
            root.AppendChild(node);
            node = doc.CreateElement("Size");
            attr = doc.CreateAttribute("width");
            attr.Value = _size.X.ToString();
            node.Attributes.Append(attr);
            attr = doc.CreateAttribute("height");
            attr.Value = _size.Y.ToString();
            node.Attributes.Append(attr);
            root.AppendChild(node);

            return root;
        }

        public Collision CheckCollision(IActiveCollider activeCollider, Vector2f proposedMovement)
        {
            return CollisionTester.TestCollision(activeCollider,proposedMovement,this, true, true, true, true);
        }

        public bool IsSolid
        {
            get { return true; }
        }

        public Vector2f Position
        {
            get { return _position; }
        }

        public Vector2f Size
        {
            get { return _size; }
        }
    }
}
