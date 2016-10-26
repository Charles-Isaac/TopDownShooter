using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;

namespace TopDownShooterGDI
{
    public class Player
    {
        public Player(PointF position, PointF vector, bool AI, int firerate, int defaultlife, int playerspeed, int defaultplayerlife)
        {
            Random rng = new Random();
            this.Position = position;
            this.Vector = vector;
            this.IsAI = AI;
            this.Score = 0;
            this.LastTickMovement = Environment.TickCount;
            this.LastTickShot = Environment.TickCount;
            this.AILastTickMovement = Environment.TickCount;
            this.PlayerSpeed = playerspeed;
            this.PlayerInv = Environment.TickCount;
            this.AINervousness = rng.Next(800, 1200);
            this.BulletSpeed = 0.5f;
            this.DefaultPlayerLife = defaultplayerlife;
            this.PlayerLife = DefaultPlayerLife;
            this.FireRate = firerate;

    }
        public List<Bullet> PlayerBullet = new List<Bullet>();
        public PointF Position { set; get; }
        public PointF Vector { set; get; }
        public int Score { set; get; }
        public bool IsAI { set; get; }
        public long LastTickMovement { set; get; }
        public long AILastTickMovement { set; get; }
        public long LastTickShot { set; get; }
        public int PlayerSpeed { set; get; }
        public long PlayerInv { set; get; }
        public int AINervousness { set; get; }
        public float BulletSpeed { set; get; }
        public int PlayerLife { set; get; }
        public int DefaultPlayerLife { set; get; }
        public int FireRate { set; get; }
    }
}
