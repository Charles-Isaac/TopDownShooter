using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TopDownShooterGDI.Objects
{
    public class Bullet
    {
        public Bullet(PointF x, PointF y)
        {
            this.Position = x;
            this.Vector = y;
        }
        public PointF Position { set; get; }
        public PointF Vector { set; get; }
    }
}
