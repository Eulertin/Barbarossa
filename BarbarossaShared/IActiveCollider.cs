using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;

namespace BarbarossaShared
{
    public interface IActiveCollider : IMoveable
    {
        Vector2f GetProposedMovement(float deltaTime);
        Vector2f Size { get; }
    }
}
