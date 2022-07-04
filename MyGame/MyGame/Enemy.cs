using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    public class Enemy
    {
        public double x;
        public double y;
        public int type;
        public double speedAxisX;
        public double speedAxisY;

        public Enemy(int x1, int y1, int type1)
        {
            x = x1;
            y = y1;
            type = type1;

            var rad = Math.Atan(y / x);
            if (x > 0)
            {
                rad += Math.PI;
            }
            speedAxisX = Math.Cos(rad);
            speedAxisY = Math.Sin(rad);
        }
    }
}
