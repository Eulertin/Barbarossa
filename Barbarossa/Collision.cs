using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
namespace Barbarossa
{

    public enum CollisionType
    {
        Top,
        Bot,
        Left,
        Right
    }

    class Collision : IComparable<Collision>
    {
        public Collision(Vector2f movementBefore, IPassiveCollider collider, CollisionType type)
        {
            _movement = movementBefore;
            _distance = Math.Sqrt(movementBefore.X * movementBefore.X + movementBefore.Y * movementBefore.Y);
            _collider = collider;
            _type = type; }

        Vector2f _movement;
        public Vector2f MovementBefore { get { return _movement; } }

        double _distance;
        IPassiveCollider _collider;
        public IPassiveCollider Collider { get { return _collider; } }
        
        CollisionType _type;
        public CollisionType Type { get { return _type; } }

        int IComparable<Collision>.CompareTo(Collision other)
        {
            return (int)(_distance - other._distance);
        }
    }
}
