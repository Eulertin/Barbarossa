using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;

namespace BarbarossaShared
{
    public interface IPassiveCollider : IPositionable
    {
        //Returns movement vector which is shortened if collision is being detected
        Collision CheckCollision(IActiveCollider activeCollider, Vector2f proposedMovement);
        bool IsSolid { get; }
        Vector2f Size { get; }
    }
}
