using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TopDownShooterGDI.Objects
{
    static class AICode
    {

        public static void Zaratoustra(ref List<Player> Players, int NMur, int[,] WallX, int[,] WallY, int Width, int Height, int NbrPlayer, int NbrAI)
        {
            Random rng = new Random();
            for (int i = 0; i < Players.Count; i++)
            {
                if (Players[i].IsAI)
                {
                    //ShortAI, Movement part

                    if (System.Environment.TickCount - Players[i].AILastTickMovement > Players[i].AINervousness)
                    {
                        Players[i].AILastTickMovement = System.Environment.TickCount;

                        

                        Players[i].Vector = new PointF(rng.Next(-Players[i].PlayerSpeed, Players[i].PlayerSpeed + 1), rng.Next(-Players[i].PlayerSpeed, Players[i].PlayerSpeed + 1));
                    }
                    PointF temppt;
                    temppt = Players[i].Position;

                    Players[i].Position = new PointF((temppt.X + Players[i].Vector.X * (System.Environment.TickCount - Players[i].LastTickMovement) / 100), (temppt.Y + Players[i].Vector.Y * (System.Environment.TickCount - Players[i].LastTickMovement) / 100));



                    Players[i].LastTickMovement = System.Environment.TickCount;
                    for (int j = 0; j < NMur && Players[i].Position != temppt; j++)
                    {
                        if (Intersecting.IsIntersecting(temppt, Players[i].Position, new Point(WallX[j, 0], WallY[j, 0]), new Point(WallX[j, 1], WallY[j, 1])))
                        {
                            Players[i].Position = temppt;
                            Players[i].Vector = new PointF(rng.Next(-Players[i].PlayerSpeed, Players[i].PlayerSpeed + 1), rng.Next(-Players[i].PlayerSpeed, Players[i].PlayerSpeed + 1));
                        }
                    }
                    //     EnemyPosition = player2;
                    if (Players[i].Position.X < 1)
                    {
                        Players[i].Vector = new PointF(0, Players[i].Vector.Y);
                        Players[i].Position = new PointF(1, Players[i].Position.Y);
                    }
                    else
                    {
                        if (Players[i].Position.X > Width - 1)
                        {
                            Players[i].Vector = new PointF(0, Players[i].Vector.Y);
                            Players[i].Position = new PointF(Width - 1, Players[i].Position.Y);
                        }
                    }

                    if (Players[i].Position.Y < 1)
                    {
                        Players[i].Vector = new PointF(Players[i].Vector.X, 0);
                        Players[i].Position = new PointF(Players[i].Position.X, 1);
                    }
                    else
                    {
                        if (Players[i].Position.Y > Height - 1)
                        {
                            Players[i].Vector = new PointF(Players[i].Vector.X, 0);
                            Players[i].Position = new PointF(Players[i].Position.X, Height - 1);
                        }
                    }
                    //short AI, Shooting part
                    bool tempBool = true;
                    for (int j = 0; j < NMur && tempBool; j++)
                    {
                        if (Intersecting.IsIntersecting(Players[i].Position, Players[0].Position, new Point(WallX[j, 0], WallY[j, 0]), new Point(WallX[j, 1], WallY[j, 1])))
                        {
                            tempBool = false;
                        }
                    }
                    if (tempBool && System.Environment.TickCount - Players[i].LastTickShot > Players[i].FireRate)
                    {
                        Players[i].LastTickShot = System.Environment.TickCount;
                        Players[i].Score--;
                        Players[i].PlayerBullet.Add(new Bullet(Players[i].Position, new PointF((Players[0].Position.X - Players[i].Position.X) / (float)Math.Sqrt((Players[0].Position.X - Players[i].Position.X) * (Players[0].Position.X - Players[i].Position.X) + (Players[0].Position.Y - Players[i].Position.Y) * (Players[0].Position.Y - Players[i].Position.Y)),
                        (Players[0].Position.Y - Players[i].Position.Y) / (float)Math.Sqrt((Players[0].Position.X - Players[i].Position.X) * (Players[0].Position.X - Players[i].Position.X) + (Players[0].Position.Y - Players[i].Position.Y) * (Players[0].Position.Y - Players[i].Position.Y)))));
                    }
                }
                else //Is NOT an AI
                {
                    PointF temppt;
                    temppt = Players[i].Position;

                    Players[i].Position = new PointF((temppt.X + Players[i].Vector.X * (System.Environment.TickCount - Players[i].LastTickMovement) * Players[i].PlayerSpeed / 100), (temppt.Y + Players[i].Vector.Y * (System.Environment.TickCount - Players[i].LastTickMovement) * Players[i].PlayerSpeed / 100));
                    
                    Players[i].LastTickMovement = System.Environment.TickCount;
                    for (int j = 0; j < NMur && Players[i].Position != temppt; j++)
                    {
                        if (Intersecting.IsIntersecting(temppt, Players[i].Position, new Point(WallX[j, 0], WallY[j, 0]), new Point(WallX[j, 1], WallY[j, 1])))
                        {
                            Players[i].Position = temppt;
                        }
                    }
                    //     EnemyPosition = player2;
                    if (Players[i].Position.X < 0)
                    {
                        Players[i].Vector = new PointF(0, Players[i].Vector.Y);
                        Players[i].Position = new PointF(0, Players[i].Position.Y);
                    }
                    else
                    {
                        if (Players[i].Position.X > Width)
                        {
                            Players[i].Vector = new PointF(0, Players[i].Vector.Y);
                            Players[i].Position = new PointF(Width, Players[i].Position.Y);
                        }
                    }

                    if (Players[i].Position.Y < 0)
                    {
                        Players[i].Vector = new PointF(Players[i].Vector.X, 0);
                        Players[i].Position = new PointF(Players[i].Position.X, 0);
                    }
                    else
                    {
                        if (Players[i].Position.Y > Height)
                        {
                            Players[i].Vector = new PointF(Players[i].Vector.X, 0);
                            Players[i].Position = new PointF(Players[i].Position.X, Height);
                        }
                    }
                }
            }
        }
    }
}
