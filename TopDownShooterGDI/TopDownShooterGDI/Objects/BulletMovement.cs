using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;

namespace TopDownShooterGDI
{
    static class BulletMovement
    {
        public static void CalculateBulletMovement(ref List<Player> Players, int Width, int Height, int[,] WallX, int[,] WallY, int NMur)
        {
            for (int i = 0; i < Players.Count; i++)
            {
                PointF testPoint = new PointF();

                for (int j = Players[i].PlayerBullet.Count - 1; j >= 0; j--)
                {
                    long LastTick = System.Environment.TickCount - Players[i].PlayerBullet[j].BulletLastTick;
                    Players[i].PlayerBullet[j].BulletLastTick = System.Environment.TickCount;

                    testPoint = Players[i].PlayerBullet[j].Position;
                    Players[i].PlayerBullet[j].Position = new PointF(Players[i].PlayerBullet[j].Vector.X * Players[i].BulletSpeed * LastTick + Players[i].PlayerBullet[j].Position.X, Players[i].PlayerBullet[j].Vector.Y * Players[i].BulletSpeed * LastTick + Players[i].PlayerBullet[j].Position.Y);
                    if (Players[i].PlayerBullet[j].Position.X < -20 || Players[i].PlayerBullet[j].Position.X > Width + 20 || Players[i].PlayerBullet[j].Position.Y < -20 || Players[i].PlayerBullet[j].Position.Y > Height + 20)
                    {
                        Players[i].PlayerBullet.RemoveAt(j);
                    }
                    else
                    /*·*/
                    {
                        int k = 0;
                        while (k < NMur && !Intersecting.IsIntersecting(Players[i].PlayerBullet[j].Position, testPoint, new Point(WallX[k, 0], WallY[k, 0]), new Point(WallX[k, 1], WallY[k, 1])))
                        {
                            k++;
                        }
                        int l = 0;
                        while (l < Players.Count && (i == l || !(Intersecting.IsIntersecting(Players[i].PlayerBullet[j].Position,
                            testPoint, new PointF(Players[l].Position.X, Players[l].Position.Y - 5),
                            new PointF(Players[l].Position.X, Players[l].Position.Y + 5)) ||
                            Intersecting.IsIntersecting(Players[i].PlayerBullet[j].Position,
                            testPoint, new PointF(Players[l].Position.X - 5, Players[l].Position.Y),
                            new PointF(Players[l].Position.X + 5, Players[l].Position.Y)))))
                        {
                            l++;
                        }
                        if (l < Players.Count)
                        {
                            if (System.Environment.TickCount - Players[l].PlayerInv > 2000/*Invicible time after death/spawn*/)
                            {
                                //l
                                Random rng = new Random();
                                Players[l].PlayerLife--;
                                if (Players[l].PlayerLife <= 0)
                                {
                                    Players[l].PlayerLife = Players[l].DefaultPlayerLife;



                                    do
                                    {
                                        Players[l].Position = new PointF(rng.Next(1, Width), rng.Next(1, Height));
                                    } while (Players[l].Position.X > 25 * 4 && Players[l].Position.X < 75 * 4 && Players[l].Position.Y > 25 * 4 && Players[l].Position.Y < 75 * 4);
                                    Players[l].AINervousness = rng.Next(800, 1200);
                                    Players[l].PlayerInv = System.Environment.TickCount;
                                }

                                //i
                                Players[i].Score += 25;
                                Players[i].PlayerBullet.RemoveAt(j);
                                //MessageBox.Show("YÉ MOURU!");
                            }
                        }
                        else
                        {
                            if (k != NMur)
                            {
                                Players[i].PlayerBullet.RemoveAt(j);
                            }
                        }
                    }

                    // safePendingList.RemoveAt(i);
                }
            }

        }
    }
}
