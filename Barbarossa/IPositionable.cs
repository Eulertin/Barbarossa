﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;

namespace Barbarossa
{
    public interface IPositionable
    {
        Vector2f Position { get; }
    }
}
