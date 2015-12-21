using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;


namespace Barbarossa
{

class LogicManager
    {
        List< IPositionable > _positionableList;
        List< IMoveable > _moveableList;
        List< IPassiveCollider > _passiveColliderList;
        List<IHasDrawable> _hasDrawableList;
        List<Player> _playerList;

        Vector2f _gravitation;

        public LogicManager()
        {
            _positionableList = new List<IPositionable>();
            _moveableList = new List<IMoveable>();
            _passiveColliderList = new List<IPassiveCollider>();
            _hasDrawableList = new List<IHasDrawable>();
            _gravitation = new Vector2f(0, (float)9.81);
        }

        public LogicManager(Vector2f gravitation)
        {
            _positionableList = new List<IPositionable>();
            _moveableList = new List<IMoveable>();
            _passiveColliderList = new List<IPassiveCollider>();
            _hasDrawableList = new List<IHasDrawable>();
            _gravitation = gravitation;
        }

        public void Update( float deltaTime )
        {
            foreach (IMoveable moveable in _moveableList)
            {
                if (moveable.IsGravitationallyInfluenced())                  //Gravitation
                {
                    moveable.ApplyForce(_gravitation);
                }

                if (moveable is IActiveCollider)                             //Collision
                {
                    IActiveCollider aCollider = moveable as IActiveCollider;
                    Vector2f position = aCollider.Position;
                    Vector2f proposedMovement = aCollider.ProposedMovement;
                    Vector2f size = aCollider.Size;
                    List<Collision> CollisionList = new List<Collision>();

                    foreach (IPassiveCollider pCollider in _passiveColliderList)
                    {
                        Collision newCollision = pCollider.CheckCollision(aCollider);
                        if (newCollision != null)
                        {
                            CollisionList.Add(newCollision);
                        }
                    }
                    CollisionList.Sort();

                    bool vertical = false, horizontal = false;

                    foreach (Collision currentCollision in CollisionList)
                    {
                        //spezifische funktionen für collider
                        if ( currentCollision.Collider.IsSolid )
                        {
                            if (!horizontal)
                            {
                                if (currentCollision.Type == CollisionType.Left || currentCollision.Type == CollisionType.Right)
                                {
                                    aCollider.ApplyForce(new Vector2f(-(aCollider.ProposedMovement - currentCollision.MovementBefore).X, 0));
                                    if (vertical)
                                    {
                                        break;
                                    }
                                    horizontal = true;
                                }
                            }

                            if (!vertical)
                            {
                                if (currentCollision.Type == CollisionType.Top || currentCollision.Type == CollisionType.Bot)
                                {
                                    aCollider.ApplyForce(new Vector2f(0,-(aCollider.ProposedMovement - currentCollision.MovementBefore).Y));
                                    if (currentCollision.Type == CollisionType.Top)
                                    {
                                        if (aCollider is Player)
                                        {
                                            Player player = aCollider as Player;
                                            player.OnGround = true;
                                        }
                                    }
                                    if (horizontal)
                                    {
                                        break;
                                    }
                                    vertical = true;
                                }
                            }
                        }
                    }
                }
                moveable.Move(deltaTime);
            }
        }

        public void AddObject(Object newObject)
        { 
            if (newObject is IPositionable)
            {
                _positionableList.Add(newObject as IPositionable);

                if (newObject is IMoveable)
                {
                    _moveableList.Add(newObject as IMoveable);
                    if (newObject is Player)
                    {
                        _playerList.Add(newObject as Player);
                    }
                }

                if (newObject is IPassiveCollider)
                {
                    _passiveColliderList.Add(newObject as IPassiveCollider);
                }
            }

            if (newObject is IHasDrawable)
            {
                _hasDrawableList.Add(newObject as IHasDrawable);
            }
        }

        public bool Remove(Object leaver)
        {
            bool success = false;
            if (leaver is IPositionable)
            {
                success |= _positionableList.Remove(leaver as IPositionable);

                if (leaver is IMoveable)
                {
                    success |= _moveableList.Remove(leaver as IMoveable);

                    if (leaver is Player)
                    {
                        success |= _playerList.Remove(leaver as Player);
                    }
                }

                if (leaver is IPassiveCollider)
                {
                    success |= _passiveColliderList.Remove(leaver as IPassiveCollider);
                }
            }
            if (leaver is IHasDrawable)
            {
                success |= _hasDrawableList.Remove(leaver as IHasDrawable);
            }
            return success;
        }
    }
}