using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    public class Bullet
    {
        public int x;
        public int y;
        public float rotation;
        public double direction;
        public Bullet(int x1, int y1, float rotation1, float direction1)
        {
            x = x1;
            y = y1;
            rotation = rotation1;
            direction = direction1;
        }
    }
}
