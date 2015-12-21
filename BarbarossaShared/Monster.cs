using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarbarossaShared
{
    public class Monster
    {
        IDrawable _drawable;

        public Monster(IDrawable drawable)
        {
            _drawable = drawable;
        }
    }
}
