using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

     
namespace Barbarossa
{
   class LogicManager
    {
        List< IMovable > _movableList;
        List<IPositionable> _positionableList;

        public void Update( float deltaTime )
        {
            foreach (IMovable movable in _movableList)
            {
                movable.Update(deltaTime);
            }
        }

        public void AddObject(Object newObject)
        {
            if (newObject is IPositionable)
            {
                _positionableList.Add(newObject as IPositionable);

                if (newObject is IMovable)
                {
                    _movableList.Add(newObject as IMovable);
                }
            }

        }
    }
}
