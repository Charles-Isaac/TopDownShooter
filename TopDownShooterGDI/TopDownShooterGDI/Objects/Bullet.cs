using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;

namespace TopDownShooterGDI
{
    public class Bullet
    {
        public Bullet(PointF x, PointF y)
        {
            this.Position = x;
            this.Vector = y;
            this.BulletLastTick = System.Environment.TickCount;
        }
        public long BulletLastTick { set; get; }
        public PointF Position { set; get; }
        public PointF Vector { set; get; }
    }
}
