using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;

namespace Barbarossa
{
    interface IDrawable
    {
        void UpdatePosition(Vector2f position);
    }

    class Player : IActiveCollider, IPassiveCollider
    {
        Vector2f _position;
        Vector2f _speed;
        Vector2f _size;
        float _horizontalAcceleration;
        float _jumpAcceleration;
        bool _isGravitaionallyInfluenced;
        ControllInfo _controllInfo;
        float _maxHorizontalSpeed;
        bool _onGround;
        public bool OnGround { set { _onGround = value; } }

        IDrawable _drawable;

        public Player(IDrawable drawable)
        {
            _drawable = drawable;
            _position = new Vector2f();
            _size = new Vector2f(16, 50);
            _horizontalAcceleration = 40f;
            _jumpAcceleration = 100f;
            _maxHorizontalSpeed = 75f;
        }

        public Player(Vector2f position, Vector2f size, IDrawable drawable)
        {
            _drawable = drawable;
            _position = new Vector2f();
            _size = new Vector2f(16, 50);
            _horizontalAcceleration = 40f;
            _jumpAcceleration = 100f;
            _maxHorizontalSpeed = 75f;
        }

        bool IPassiveCollider.IsSolid { get { return false; } }

        Vector2f IPositionable.Position { get { return _position; } }

        Vector2f IActiveCollider.ProposedMovement { get { return _speed; } }

        Vector2f IActiveCollider.Size { get { return _size; } }

        public void ApplyForce(Vector2f force)
        {
            _speed += force;
        }

        Collision IPassiveCollider.CheckCollision(IActiveCollider activeCollider)
        {
            throw new NotImplementedException();
        }

        bool IMoveable.IsGravitationallyInfluenced()
        {
            return _isGravitaionallyInfluenced;
        }

        void IMoveable.Move(float deltaTime)
        {
            _position += deltaTime * _speed;
            _drawable.UpdatePosition(_position);
        }

        void IMoveable.Update(float deltaTime)
        {
            if (_controllInfo.Left)
            {
                float maxAcceleration = -(_maxHorizontalSpeed) - _speed.X;
                if (maxAcceleration > 0)
                {
                    ApplyForce(new Vector2f(-deltaTime * Math.Min(_horizontalAcceleration, maxAcceleration), 0));
                }
            }
            else if (_controllInfo.Right)
            {
                float maxAcceleration = _maxHorizontalSpeed - _speed.X;
                if (maxAcceleration > 0)
                {
                    ApplyForce(new Vector2f(deltaTime * Math.Min(_horizontalAcceleration, maxAcceleration), 0));
                }
            }
            else
            {
                if (_speed.X < 0)
                {
                    ApplyForce(new Vector2f(deltaTime * Math.Min(_horizontalAcceleration, -(_speed.X)), 0));
                }
                else if (_speed.X < 0)
                {
                    ApplyForce(new Vector2f(deltaTime * Math.Min(-(_horizontalAcceleration), -(_speed.X)), 0));
                }
                if (_onGround)
                {
                    _onGround = false;
                    if (_controllInfo.Up)
                    {
                        ApplyForce(new Vector2f(_jumpAcceleration, 0));
                    }
                }
            }
        }
    }
}
