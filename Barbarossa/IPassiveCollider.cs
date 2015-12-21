using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;

namespace Barbarossa
{
    interface IPassiveCollider : IPositionable
    {
        //Returns movement vector which is shortened if collision is being detected
        Collision CheckCollision(IActiveCollider activeCollider);
        bool IsSolid { get; }
    }
}
