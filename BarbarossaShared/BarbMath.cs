using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;

namespace BarbarossaShared
{
    class BarbMath
    {
        /// <summary>
        /// Löst ein 2x2 lineares Gleichungssystem
        /// </summary>
        /// <param name="v">Der erste Richtungsvektor</param>
        /// <param name="p">Der erste Abschnittsvektor</param>
        /// <param name="w">Der zweite Richtungsvektor</param>
        /// <param name="q">Der zweite Abschnittsvektor</param>
        /// <returns>Der Lösungsvektor</returns>
        public static Vector2f LinSolve(Vector2f v, Vector2f p, Vector2f w, Vector2f q)
        {
            Vector2f b = q - p;
            w = -w;
            Vector2f a = new Vector2f();

            if (v.X == 0)
            {
                if (v.Y == 0 || w.X == 0)
                    throw new Exception("Keine oder unendliche viele Lösungen!");

                a.Y = b.X / w.X;
                a.X = (b.Y - a.Y * w.Y) / v.Y;
            }
            else
            {
                float r = v.Y / v.X;
                v.Y = 0;
                w.Y = w.Y - r * w.X;
                b.Y = b.Y - r * b.X;

                if (w.Y == 0)
                    throw new Exception("Keine oder unendliche viele Lösungen!");

                a.Y = b.Y / w.Y;
                a.X = (b.X - a.Y * w.X) / v.X;
            }
            return a;
        }

        public static bool PythFit(Vector2f v, float m)
        {
            return v.X * v.X + v.Y * v.Y < m * m;
        }

        public static float Pyth(Vector2f v)
        {
            return Convert.ToSingle(Math.Sqrt(v.X * v.X + v.Y * v.Y));
        }
    }
}
