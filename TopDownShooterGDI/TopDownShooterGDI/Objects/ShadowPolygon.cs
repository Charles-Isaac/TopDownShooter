using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TopDownShooterGDI
{
    static class ShadowPolygon
    {
        public static Point[,] ReturnMeAnArray(int NMur, int[,] WallX, int[,] WallY, PointF LightSource, int Width, int Height)
        {
            Point[,] tempPt = new Point[NMur, 6];
            int[,,] PolyGone = new int[NMur/* * 2 + 4*/, 4, 2];  //int[NO of the polygon, NO of the point of the angles of the polygon, X or Y]
            Double TDVAngle0X, TDVAngle0Y, TDVAngle1X, TDVAngle1Y;
            for (int i = 0; i < NMur; i++)
            {
                TDVAngle1X = (double)(WallX[i, 0] - LightSource.X);
                TDVAngle1Y = (double)(WallY[i, 0] - LightSource.Y);
                TDVAngle0X = (double)(WallX[i, 1] - LightSource.X);
                TDVAngle0Y = (double)(WallY[i, 1] - LightSource.Y);

                PolyGone[i, 0, 0] = WallX[i, 0];
                PolyGone[i, 0, 1] = WallY[i, 0];
                PolyGone[i, 1, 0] = WallX[i, 1];
                PolyGone[i, 1, 1] = WallY[i, 1];

                if (TDVAngle0X > 0)
                {
                    
                    PolyGone[i, 2, 0] = Width;
                    PolyGone[i, 2, 1] = (int)(LightSource.Y + TDVAngle0Y * (Width - LightSource.X) / TDVAngle0X);
                }
                else
                {
                    if (TDVAngle0X < 0)
                    {
                        PolyGone[i, 2, 0] = 0;
                        PolyGone[i, 2, 1] = (int)(LightSource.Y + TDVAngle0Y * (0 - LightSource.X) / TDVAngle0X);
                    }
                    else
                    {
                        PolyGone[i, 2, 0] = (int)LightSource.X;
                        if (TDVAngle0Y > 0)
                        {
                            PolyGone[i, 2, 1] = Height;
                        }
                        else
                        {
                            if (TDVAngle0Y < 0)
                            {
                                PolyGone[i, 2, 1] = 0;
                            }
                            else
                            {
                                PolyGone[i, 2, 1] = (int)(LightSource.Y);
                            }
                        }
                    }
                }






                if (TDVAngle1X > 0)
                {
                    PolyGone[i, 3, 0] = Width;
                    PolyGone[i, 3, 1] = (int)(LightSource.Y + TDVAngle1Y * (Width - LightSource.X) / TDVAngle1X);
                }
                else
                {
                    if (TDVAngle1X < 0)
                    {
                        PolyGone[i, 3, 0] = 0;
                        PolyGone[i, 3, 1] = (int)(LightSource.Y + TDVAngle1Y * (0 - LightSource.X) / TDVAngle1X);
                    }
                    else
                    {
                        PolyGone[i, 3, 0] = (int)LightSource.X;
                        if (TDVAngle1Y > 0)
                        {
                            PolyGone[i, 3, 1] = Height;
                        }
                        else
                        {
                            if (TDVAngle1Y < 0)
                            {
                                PolyGone[i, 3, 1] = 0;
                            }
                            else
                            {
                                PolyGone[i, 3, 1] = (int)(LightSource.Y);
                            }
                        }
                    }
                }

            }

            
            for (int i = 0; i < NMur; i++)
            {
                tempPt[i, 0].X = PolyGone[i, 0, 0];
                tempPt[i, 0].Y = PolyGone[i, 0, 1];
                tempPt[i, 1].X = PolyGone[i, 1, 0];
                tempPt[i, 1].Y = PolyGone[i, 1, 1];
                tempPt[i, 2].X = PolyGone[i, 2, 0];
                tempPt[i, 2].Y = PolyGone[i, 2, 1];

                tempPt[i, 3].X = PolyGone[i, 2, 0];
                tempPt[i, 3].Y = PolyGone[i, 2, 1];
                tempPt[i, 4].X = PolyGone[i, 3, 0];
                tempPt[i, 4].Y = PolyGone[i, 3, 1];

                tempPt[i, 5].X = PolyGone[i, 3, 0];
                tempPt[i, 5].Y = PolyGone[i, 3, 1];

                if (tempPt[i,2].X != tempPt[i,5].X)
                {
                    if (((tempPt[i,1].X - tempPt[i,0].X) * (LightSource.Y - tempPt[i,0].Y) - (tempPt[i,1].Y - tempPt[i,0].Y) * (LightSource.X - tempPt[i,0].X)) < 0)
                    {
                        if (tempPt[i,2].Y > 0)
                        {
                            tempPt[i,3].Y = 0;
                        }
                        if (tempPt[i,5].Y > 0)
                        {
                            tempPt[i,4].Y = 0;
                        }
                    }
                    else
                    {
                        if (((tempPt[i,1].X - tempPt[i,0].X) * (LightSource.Y - tempPt[i,0].Y) - (tempPt[i,1].Y - tempPt[i,0].Y) * (LightSource.X - tempPt[i,0].X)) > 0)
                        {
                            if (tempPt[i,2].Y < Height)
                            {
                                tempPt[i,3].Y = Height;
                            }
                            if (tempPt[i,5].Y < Height)
                            {
                                tempPt[i,4].Y = Height;
                            }
                        }
                    }
                }
            }



            return tempPt;
        }
    }
}
