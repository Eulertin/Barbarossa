using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using SFML.System;

namespace BarbarossaShared
{
    public class Monster : ISaveable, IPassiveCollider, IMoveable
    {
        Vector2f[] _movePath;

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

        float _maxSpeed;
        Vector2f _speed;

        int _pathStep;

        IDrawable _drawableObject;

        public Monster(IDrawable drawable, Vector2f[] movePath, Vector2f size, float maxSpeed)
        {
            _drawableObject = drawable;
            _position = movePath[0];
            _movePath = movePath;
            _size = size;
            _maxSpeed = maxSpeed;
            _pathStep = 1;
        }

        public System.Xml.XmlNode GetSaveNode(System.Xml.XmlDocument doc)
        {
            XmlNode root = doc.CreateElement("Logic");

            XmlNode node = doc.CreateElement("Type");
            XmlAttribute attr = doc.CreateAttribute("type");
            attr.Value = "Player";
            node.Attributes.Append(attr);
            root.AppendChild(node);

            node = doc.CreateElement("MovePath");
            XmlNode subNode;

            for (int i = 0; i < _movePath.Length; i++)
            {
                subNode = doc.CreateElement("Path-Vector" + i);
                attr = doc.CreateAttribute("x");
                attr.Value = _movePath[i].X.ToString();
                subNode.Attributes.Append(attr);
                attr = doc.CreateAttribute("y");
                attr.Value = _movePath[i].Y.ToString();
                subNode.Attributes.Append(attr);
                node.AppendChild(subNode);
            }

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

        public Collision CheckCollision(IActiveCollider activeCollider, SFML.System.Vector2f proposedMovement)
        {
            return CollisionTester.TestCollision(activeCollider, proposedMovement, this, true, true, true, true);
        }

        public bool IsSolid
        {
            get { return false; }
        }

        public void Update(float deltaTime)
        {
            steer(deltaTime * _maxSpeed);
        }

        private void steer(float movementLeft)
        {
            if (BarbMath.PythFit(_movePath[_pathStep] - _position, movementLeft))
            {
                _speed = (_movePath[_pathStep] - _position) * movementLeft / BarbMath.Pyth(_movePath[_pathStep] - _position);
            }
            else
            {
                if (++_pathStep == _movePath.Length)
                    _pathStep = 0;
                steer(movementLeft - BarbMath.Pyth(_movePath[_pathStep] - _position));
            }
        }

        public void Move(float deltaTime)
        {
            _position += _speed;
        }

        public void ApplyForce(Vector2f force)
        {
            throw new NotImplementedException();
        }

        public bool IsGravitationallyInfluenced()
        {
            return false;
        }
    }
}
