using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;

namespace Barbarossa
{
    interface IActiveCollider : IMoveable
    {
        Vector2f ProposedMovement { get; }
        Vector2f Size { get; }
    }
}
