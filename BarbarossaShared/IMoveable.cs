using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;

namespace BarbarossaShared
{
    public interface IMoveable : IPositionable
    {
        void Update(float deltaTime);
        void Move(float deltaTime);
        void ApplyForce(Vector2f force);
        bool IsGravitationallyInfluenced();
    }
}
