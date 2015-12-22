using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using System.Xml;

namespace BarbarossaShared
{

    public class Player : IActiveCollider, IPassiveCollider, ISaveable
    {
        Vector2f _position;
        public Vector2f Position { get { return _position; } }
        Vector2f _size;
        public Vector2f Size { get { return _size; } }
        
        Vector2f _speed;
        
        float _horizontalAcceleration;
        float _jumpAcceleration;
        bool _isGravitaionallyInfluenced;
        
        ControlInfo _controlInfo;
        float _maxHorizontalSpeed;
        bool _onGround;
        public bool OnGround { set { _onGround = value; } }

        IDrawable _drawable;

        public Player(IDrawable drawable, Vector2f position, Vector2f size)
        {
            _drawable = drawable;
            _position = position;
            _size = size;
            _horizontalAcceleration = 150f;
            _jumpAcceleration = 400f;
            _maxHorizontalSpeed = 100f;
            _isGravitaionallyInfluenced = true;
        }

        bool IPassiveCollider.IsSolid { get { return false; } }

        public Vector2f GetProposedMovement(float deltaTime)
        {
            return _speed * deltaTime;
        }

        Vector2f IActiveCollider.Size { get { return _size; } }

        public void ApplyForce(Vector2f force)
        {
            _speed += force;
        }

        Collision IPassiveCollider.CheckCollision(IActiveCollider activeCollider, Vector2f proposedMovement)
        {
            return CollisionTester.TestCollision(activeCollider,proposedMovement, this, true, true, true, true);
        }

        bool IMoveable.IsGravitationallyInfluenced()
        {
            return _isGravitaionallyInfluenced;
        }

        void IMoveable.Move(float deltaTime)
        {
            _position += deltaTime * _speed;
            _drawable.UpdatePosition(_position);
            if (Math.Abs(_speed.Y) < 0.5)
                _speed.Y = 0;
        }

        public void Update(float deltaTime)
        {
            if (_controlInfo.Left)
            {
                float maxAcceleration = _maxHorizontalSpeed + _speed.X;
                if (maxAcceleration > 0)
                {
                    ApplyForce(new Vector2f(-Math.Min(deltaTime * _horizontalAcceleration, maxAcceleration), 0));
                }
            }
            else if (_controlInfo.Right)
            {
                float maxAcceleration = _maxHorizontalSpeed - _speed.X;
                if (maxAcceleration > 0)
                {
                    ApplyForce(new Vector2f(Math.Min(deltaTime * _horizontalAcceleration, maxAcceleration), 0));
                }
            }
            else
            {
                if (_speed.X < 0)
                {
                    ApplyForce(new Vector2f(Math.Min(deltaTime * _horizontalAcceleration, -(_speed.X)), 0));
                }
                else if (_speed.X > 0)
                {
                    ApplyForce(new Vector2f(Math.Max(-(deltaTime * _horizontalAcceleration), -(_speed.X)), 0));
                }
            }
            if (_onGround)
            {
                _onGround = false;
                if (_controlInfo.Up)
                {
                    _speed.Y = -_jumpAcceleration;
                }
            }
        }

        public void SetControlInfoObject(ControlInfo cio)
        {
            _controlInfo = cio;
        }

        public System.Xml.XmlNode GetSaveNode(System.Xml.XmlDocument doc)
        {
            XmlNode root = doc.CreateElement("Logic");

            XmlNode node = doc.CreateElement("Type");
            XmlAttribute attr = doc.CreateAttribute("type");
            attr.Value = "Player";
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
    }
}