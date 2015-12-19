using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;

namespace Barbarossa
{
    interface IMovable : IPositionable
    {
        void Update(float deltaTime);
    }
}
