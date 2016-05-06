using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;

namespace TopDownShooterGDI
{
    public partial class frmJeu : Form
    {

        public static int WindowsSizeX, WindowsSizeY;
        List<int> Score = new List<int>();
        long AIMovTime;
        Point AIDir = new Point(0, 0);
        Point EnemyPosition;
        long AILastTick;
        long MovementLastTick;
        long BulletLastTick;
        long EnemyBulletLastTick;
        long AILastShotTick;
        int NMur;
        int[,] WallX;
        int[,] WallY;
        Point Light;
        Point Player2;
        Pen myPen = new Pen(Color.Blue);
        Brush Br = new SolidBrush(Color.Black);
        Point[] Shadows = new Point[6];
        Point[,] tempPt;
        TopDownShooter.Objects.ShadowPolygon SP;
        List<Objects.Bullet> Bullet = new List<Objects.Bullet>();
        List<Objects.Bullet> EnemyBullet = new List<Objects.Bullet>();
        int VY;
        int VX;
        Point Player1 = new Point(500,500);
        

        public frmJeu()
        {
            InitializeComponent();
            
            this.DoubleBuffered = true;
        }

        private void frmJeu_Load(object sender, EventArgs e)
        {
            EnemyPosition = new Point(50, 50);
            SP = new TopDownShooter.Objects.ShadowPolygon();
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            myPen.Color = Color.Blue;
            WindowsSizeX = this.Width;
            WindowsSizeY = this.Height;
            myPen.Width = 5;



            if (!GetMap(out WallX, out WallY))
            {
                MessageBox.Show("Error fetching map from server");
                //drop game
            }

            MovementLastTick = System.Environment.TickCount;
            BulletLastTick = System.Environment.TickCount;
            EnemyBulletLastTick = System.Environment.TickCount;
            AILastShotTick = System.Environment.TickCount;
            AILastTick = System.Environment.TickCount;

            while (true)
            {
                this.Refresh();
                Application.DoEvents();
            }

        }/*
        DateTime _lastCheckTime = DateTime.Now;
        long _frameCount = 0;

        // called whenever a map is updated
        void OnMapUpdated()
        {
            Interlocked.Increment(ref _frameCount);
        }

        // called every once in a while
        double GetFps()
        {
            double secondsElapsed = (DateTime.Now - _lastCheckTime).TotalSeconds;
            long count = Interlocked.Exchange(ref _frameCount, 0);
            double fps = count / secondsElapsed;
            _lastCheckTime = DateTime.Now;
            return fps;
        }

        int Vall = 0;*/
        private void timer1_Tick(object sender, EventArgs e)
        {
           /* Vall++;
            if (Vall % 100 == 0)
            {
                
                MessageBox.Show(GetFps().ToString());
            }
            OnMapUpdated();*/
            this.Refresh();
        }

        private void frmJeu_Resize(object sender, EventArgs e)
        {
            WindowsSizeX = this.Width;
            WindowsSizeY = this.Height;
        }

        private void frmJeu_Paint(object sender, PaintEventArgs e)
        {
            //var g = e.Graphics;

            //calculate the scale ratio to fit a 320x200 box in the form
            /*var width = g.VisibleClipBounds.Width;
            var height = g.VisibleClipBounds.Height;
            var widthRatio = width / 700;
            var heightRatio = height / 700;
            var scaleRatio = Math.Min(widthRatio, heightRatio);
            e.Graphics.ScaleTransform(scaleRatio, scaleRatio);*/
            if (!GetPlayerPosition(out Player2))
            {
                MessageBox.Show("stuff");
            }
            Brush enemy = new SolidBrush(Color.Red);
            
            e.Graphics.FillEllipse(enemy, Player2.X - 5, Player2.Y - 5, 10, 10);
            //Light = System.Windows.Forms.Cursor.Position;
            Light = this.PointToClient(Cursor.Position);

            e.Graphics.FillPie(Br, Player1.X - 2000, Player1.Y - 2000, 4000, 4000, (float)(Math.Atan2(Light.Y - Player1.Y, Light.X - Player1.X) / Math.PI * 180 +80), 200f);




            tempPt = SP.ReturnMeAnArray(NMur, WallX, WallY, Player1);

            
            for (int i = 0; i < NMur; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    Shadows[j] = tempPt[i, j];
                }
               
                e.Graphics.FillPolygon(Br, Shadows);
                //e.Graphics.DrawLine(myPen, Polygon[i / 4, i % 4, 0], Polygon[i / 4, i % 4, 1], Polygon[i / 4, (i + 1) % 4, 0], Polygon[i / 4, (i + 1) % 4, 1]);
                               
            }
            BulletMovement();
            foreach (Objects.Bullet Bull in Bullet)
            {
                e.Graphics.FillEllipse(new SolidBrush(Color.Red), Bull.Position.X, Bull.Position.Y, 5, 5);
            }
            EnemyBulletMovement();
            foreach (Objects.Bullet Bull in EnemyBullet)
            {
                e.Graphics.FillEllipse(new SolidBrush(Color.Red), Bull.Position.X, Bull.Position.Y, 5, 5);
            }
            for (int i = 0; i < NMur; i++)
            {
                e.Graphics.DrawLine(myPen, WallX[i, 0], WallY[i, 0], WallX[i, 1], WallY[i, 1]);
            }
            e.Graphics.FillEllipse(enemy, Player1.X-5, Player1.Y-5, 10, 10);

            e.Graphics.DrawLine(new Pen(Color.Green), Player1, Light);
            for (int i = 0; i < Score.Count; i++)
            {
                e.Graphics.DrawString(Score[i].ToString(), new Font("Time New Roman", 18), new SolidBrush(Color.Yellow), 180, 120 + 30 * i);
            }
        }
         
        enum ArrowsPressed
        {
            None = 0x00,
            Left = 0x01,
            Right = 0x02,
            Up = 0x04,
            Down = 0x08,
            Space = 0x10,
            Escape = 0x20,
            All = 0x3F
        }

        ArrowsPressed arrowsPressed;
         
        void ChangeArrowsState(ArrowsPressed changed, bool isPressed)
        {
            if (isPressed)
            {
                arrowsPressed |= changed;
            }
            else
            {
                arrowsPressed &= ArrowsPressed.All ^ changed;
            }
        }
         
        protected override void OnKeyDown(KeyEventArgs e)
        {
            /*if (e.KeyData == Keys.P)
            {
                arrowsPressed = ArrowsPressed.None;
                if (timer1.Enabled)
                {
                    timer1.Stop();
                }
                else
                {
                    timer1.Start();
                }
                return;
            }*/

            /*if (e.KeyData == Keys.H)
            {
                arrowsPressed = ArrowsPressed.None;
                timer1.Stop();

                var RessourcesH = Assembly.GetExecutingAssembly();
                using (Stream stream = RessourcesH.GetManifestResourceStream("AnimationV1.Resources.Help.txt"))
                using (StreamReader StreamR = new StreamReader(stream))
                {
                    MessageBox.Show(StreamR.ReadToEnd());
                }
                timer1.Start();
                return;
            }*/

           /* if (e.KeyData == Keys.OemMinus || e.KeyData == Keys.Oemplus)
            {
                if (e.KeyData == Keys.OemMinus)
                {
                    if (timer1.Interval > 1)
                    {
                        timer1.Interval--;
                    }
                }
                else
                {
                    timer1.Interval++;
                }


            }*/

            base.OnKeyDown(e);
            switch (e.KeyCode)
            {
                case Keys.S:
                    ChangeArrowsState(ArrowsPressed.Down, true);
                    break;
                case Keys.W:
                    ChangeArrowsState(ArrowsPressed.Up, true);
                    break;
                case Keys.A:
                    ChangeArrowsState(ArrowsPressed.Left, true);
                    break;
                case Keys.D:
                    ChangeArrowsState(ArrowsPressed.Right, true);
                    break;
                case Keys.Escape:
                    Environment.Exit(0);
                    break;
                case Keys.Space:
                    ChangeArrowsState(ArrowsPressed.Space, true);
                    Player1 = this.PointToClient(Cursor.Position);
                    break;
                default:
                    return;
            }
            HandleArrows();
            e.Handled = true;
        }
        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            switch (e.KeyCode)
            {
                case Keys.S:
                    ChangeArrowsState(ArrowsPressed.Down, false);
                    break;
                case Keys.W:
                    ChangeArrowsState(ArrowsPressed.Up, false);
                    break;
                case Keys.A:
                    ChangeArrowsState(ArrowsPressed.Left, false);
                    break;
                case Keys.D:
                    ChangeArrowsState(ArrowsPressed.Right, false);
                    break;
                case Keys.Space:
                    ChangeArrowsState(ArrowsPressed.Space, false);
                    break;
                default:
                    return;
            }
            HandleArrows();
            e.Handled = true;
        }
         
        private void HandleArrows()
        {

            //MessageBox.Show(arrowsPressed.ToString());
            if ((arrowsPressed & ArrowsPressed.Up) == ArrowsPressed.None && (arrowsPressed & ArrowsPressed.Down) != ArrowsPressed.None)
            {
                VY = 1;
            }

            if ((arrowsPressed & ArrowsPressed.Down) == ArrowsPressed.None && (arrowsPressed & ArrowsPressed.Up) != ArrowsPressed.None)
            {
                VY = -1;
            }

            if ((arrowsPressed & ArrowsPressed.Right) == ArrowsPressed.None && (arrowsPressed & ArrowsPressed.Left) != ArrowsPressed.None)
            {
                VX = -1;
            }

            if ((arrowsPressed & ArrowsPressed.Left) == ArrowsPressed.None && (arrowsPressed & ArrowsPressed.Right) != ArrowsPressed.None)
            {
                VX = 1;
            }

            if (((arrowsPressed & ArrowsPressed.Up) | (arrowsPressed & ArrowsPressed.Down)) == ArrowsPressed.None)
            {
                VY = 0;
            }

            if (((arrowsPressed & ArrowsPressed.Right) | (arrowsPressed & ArrowsPressed.Left)) == ArrowsPressed.None)
            {
                VX = 0;
            }

            //  Do whatever is needed using position
        }
         
        private void frmJeu_KeyDown(object sender, KeyEventArgs e)
        {
            
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
            {
                MessageBox.Show("Enter presssed: ");
                Application.Exit();
            }
            else
            {
                //MessageBox.Show("Test");
            }
        }

        private void frmJeu_MouseClick(object sender, MouseEventArgs e)
        {
            Bullet.Add(new Objects.Bullet(Player1, new PointF((Light.X - Player1.X)/ (float)Math.Sqrt((Light.X - Player1.X) * (Light.X - Player1.X)+ (Light.Y - Player1.Y)* (Light.Y - Player1.Y)),
                (Light.Y - Player1.Y) / (float)Math.Sqrt((Light.X - Player1.X) * (Light.X - Player1.X) + (Light.Y - Player1.Y) * (Light.Y - Player1.Y)))));
            
            //timer1.Enabled = true;
        }
        private bool IsIntersecting(PointF a, PointF b, PointF c, PointF d)
        {
            float denominator = ((b.X - a.X) * (d.Y - c.Y)) - ((b.Y - a.Y) * (d.X - c.X));
            float numerator1 = ((a.Y - c.Y) * (d.X - c.X)) - ((a.X - c.X) * (d.Y - c.Y));
            float numerator2 = ((a.Y - c.Y) * (b.X - a.X)) - ((a.X - c.X) * (b.Y - a.Y));
            
            if (denominator == 0) return numerator1 == 0 && numerator2 == 0;

            float r = numerator1 / denominator;
            float s = numerator2 / denominator;

            return (r >= 0 && r <= 1) && (s >= 0 && s <= 1);
        }
        private bool GetPlayerPosition(out Point player2)
        {
            Point tempPlayer = new Point();
            tempPlayer = Player1;

            Player1.X += (int)(VX * (System.Environment.TickCount - MovementLastTick)) / 5;
            Player1.Y += (int)(VY * (System.Environment.TickCount - MovementLastTick)) / 5;
            MovementLastTick = System.Environment.TickCount;
            for (int i = 0; i < NMur; i++)
            {
                if (IsIntersecting(Player1, tempPlayer, new Point(WallX[i,0], WallY[i,0]), new Point(WallX[i,1], WallY[i,1])))
                {
                    Player1 = tempPlayer;
                }
            }
            if (Player1.X < 0)
            {
                Player1.X = 0;
            }
            else
            {
                if (Player1.X > this.Width)
                {
                    Player1.X = this.Width;
                }
            }

            if (Player1.Y < 0)
            {
                Player1.Y = 0;
            }
            else
            {
                if (Player1.Y > this.Height)
                {
                    Player1.Y = this.Height;
                }
            }

            //short AI
            if (System.Environment.TickCount - AIMovTime > 1000)
            {
                AIMovTime = System.Environment.TickCount;

                Random rng = new Random();
                
                AIDir.X = rng.Next(-5, 6);
                AIDir.Y = rng.Next(-5, 6);
                
            }
            
            player2 = new Point();
            player2 = new Point((int)(EnemyPosition.X + AIDir.X * (System.Environment.TickCount - AILastTick) / 50), (int)(EnemyPosition.Y + AIDir.Y * (System.Environment.TickCount - AILastTick) / 50));

            /*Point tttt = new Point();
            if (player2.X != 50)
            {
                tttt = player2;
                using (Graphics g = this.CreateGraphics())
                {
                    g.DrawString(player2.X.ToString() + player2.Y.ToString(), new Font("Time New Roman", 24), new SolidBrush(Color.Red), 100, 100);
                }
            }*/
            Point temppt = new Point();
            temppt = player2;
            AILastTick = System.Environment.TickCount;
            for (int i = 0; i < NMur; i++)
            {
                if (IsIntersecting(player2, EnemyPosition, new Point(WallX[i, 0], WallY[i, 0]), new Point(WallX[i, 1], WallY[i, 1])))
                {
                    player2 = EnemyPosition;
                }
            }
            EnemyPosition = player2;
            if (player2.X < 0)
            {
                AIDir.X = -AIDir.X;
                player2.X = 0;
            }
            else
            {
                if (player2.X > this.Width)
                {
                    AIDir.X = -AIDir.X;
                    player2.X = this.Width;
                }
            }

            if (player2.Y < 0)
            {
                AIDir.Y = -AIDir.Y;
                player2.Y = 0;
            }
            else
            {
                if (player2.Y > this.Height)
                {
                    AIDir.Y = -AIDir.Y;
                    player2.Y = this.Height;
                }
            }
            



            
            //short AI
            bool tempBool = true;
            for (int i = 0; i < NMur; i++)
            {
                if (IsIntersecting(player2, Player1, new Point(WallX[i, 0], WallY[i, 0]), new Point(WallX[i, 1], WallY[i, 1])))
                {
                    tempBool = false;
                }
            }
            if (tempBool && System.Environment.TickCount - AILastShotTick > 300)
            {
                AILastShotTick = System.Environment.TickCount;
                EnemyBullet.Add(new Objects.Bullet(player2, new PointF((Player1.X - player2.X) / (float)Math.Sqrt((Player1.X - player2.X) * (Player1.X - player2.X) + (Player1.Y - player2.Y) * (Player1.Y - player2.Y)),
                (Player1.Y - player2.Y) / (float)Math.Sqrt((Player1.X - player2.X) * (Player1.X - player2.X) + (Player1.Y - player2.Y) * (Player1.Y - player2.Y)))));
            }
            
            return true;
        }
        private bool GetMap(out int[,] WallX, out int[,] WallY)
        {
            NMur = 20;
            {
                int NPlayer = 2;
                for (int i = 0; i < NPlayer; i++)
                {
                    Score.Add(0);
                }
            }
            

            WallX = new int[NMur, 2];
            WallY = new int[NMur, 2];

            Random rng = new Random();
            /*for (int i = 0; i < NMur; i++)
            {
                WallX[i, 0] = rng.Next(5, this.Width - 5);
                WallY[i, 0] = rng.Next(5, this.Height - 5);
                WallX[i, 1] = WallX[i, 0] + 1;
                WallY[i, 1] = WallY[i, 0];
            }*/
            WallX[0, 0] = 50 * 4;
            WallY[0, 0] = 25 * 4;
            WallX[0, 1] = 75 * 4;
            WallY[0, 1] = 50 * 4;
            WallX[1, 0] = 50 * 4;
            WallY[1, 0] = 75 * 4;
            WallX[1, 1] = 75 * 4;
            WallY[1, 1] = 50 * 4;
            WallX[2, 0] = 25 * 4;
            WallY[2, 0] = 50 * 4;
            WallX[2, 1] = 50 * 4;
            WallY[2, 1] = 75 * 4;
            WallX[3, 0] = 25 * 4;
            WallY[3, 0] = 50 * 4;
            WallX[3, 1] = 50 * 4;
            WallY[3, 1] = 25 * 4;


            for (int i = 4; i < NMur; i++)
            {
                
                WallX[i, 0] = rng.Next(5, this.Width - 5);
                WallY[i, 0] = rng.Next(5, this.Height - 5);
                WallX[i, 1] = rng.Next(5, this.Width - 5);
                WallY[i, 1] = rng.Next(5, this.Height - 5);

                for (int j = 0; j < i; j++)
                {
                    if (IsIntersecting(new Point(WallX[i,0], WallY[i,0]), new Point(WallX[i, 1], WallY[i, 1]), new Point(WallX[j, 0], WallY[j, 0]), new Point(WallX[j, 1], WallY[j, 1])))
                    {
                        j = i;
                        i--;
                    }
                }

            }
            


            for (int i = 0; i < NMur; i++)
            {
                if (WallX[i, 0] < WallX[i, 1])
                {
                    int temp1 = WallX[i, 0];
                    WallX[i, 0] = WallX[i, 1];
                    WallX[i, 1] = temp1;

                    temp1 = WallY[i, 0];
                    WallY[i, 0] = WallY[i, 1];
                    WallY[i, 1] = temp1;
                }

            }
            return true;
        }
        private void BulletMovement()
        {
            PointF testPoint = new PointF();
            long LastTick = System.Environment.TickCount - BulletLastTick;
            BulletLastTick = System.Environment.TickCount;
            for (int i = Bullet.Count - 1; i >= 0; i--)
            {
                testPoint = Bullet[i].Position;
                Bullet[i].Position = new PointF(Bullet[i].Vector.X * LastTick * 1 + Bullet[i].Position.X, Bullet[i].Vector.Y * LastTick * 1 + Bullet[i].Position.Y);
                if (Bullet[i].Position.X < -20 || Bullet[i].Position.X > this.Width +20|| Bullet[i].Position.Y < -20 || Bullet[i].Position.Y > this.Height +20)
                {
                    Bullet.RemoveAt(i);
                }
                else
                {
                    int j = 0;
                    while (j < NMur && !IsIntersecting(Bullet[i].Position, testPoint, new Point(WallX[j, 0], WallY[j, 0]), new Point(WallX[j, 1], WallY[j, 1])))
                    {
                        j++;
                    }
                    if (j != NMur)
                    {
                        Bullet.RemoveAt(i);
                    }
                    else
                    {
                        if (IsIntersecting(Bullet[i].Position, testPoint, new Point(Player2.X, Player2.Y - 5), new Point(Player2.X, Player2.Y + 5)) || IsIntersecting(Bullet[i].Position, testPoint, new Point(Player2.X - 5, Player2.Y), new Point(Player2.X + 5, Player2.Y)))
                        {
                            Random rng = new Random();
                            EnemyPosition = new Point(rng.Next(1, this.Width), rng.Next(1, this.Height));
                            Score[0]++;
                            //MessageBox.Show("YÉ MOURU!");
                        }
                    }
                }

                // safePendingList.RemoveAt(i);
            }
            //Bullet.Where(w => true).ToList().ForEach(s => s.Position = new PointF(s.Position.X + s.Vector.X, s.Position.Y + s.Vector.Y));
        }
        private void EnemyBulletMovement()
        {
            PointF testPoint = new PointF();
            long LastTick = System.Environment.TickCount - EnemyBulletLastTick;
            EnemyBulletLastTick = System.Environment.TickCount;


            for (int i = EnemyBullet.Count - 1; i >= 0; i--)
            {
                testPoint = EnemyBullet[i].Position;
                EnemyBullet[i].Position = new PointF(EnemyBullet[i].Vector.X * LastTick * 1 + EnemyBullet[i].Position.X, EnemyBullet[i].Vector.Y * LastTick * 1 + EnemyBullet[i].Position.Y);
                if (EnemyBullet[i].Position.X < -20 || EnemyBullet[i].Position.X > this.Width +20 || EnemyBullet[i].Position.Y < -20 || EnemyBullet[i].Position.Y > this.Height +20)
                {
                    EnemyBullet.RemoveAt(i);
                }
                else
                {
                    int j = 0;
                    while (j < NMur && !IsIntersecting(EnemyBullet[i].Position, testPoint, new Point(WallX[j, 0], WallY[j, 0]), new Point(WallX[j, 1], WallY[j, 1])))
                    {
                        j++;
                    }
                    if (j != NMur)
                    {
                        EnemyBullet.RemoveAt(i);
                    }
                    else
                    {
                        if (IsIntersecting(EnemyBullet[i].Position, testPoint, new Point(Player1.X, Player1.Y - 5), new Point(Player1.X, Player1.Y + 5)) || IsIntersecting(EnemyBullet[i].Position, testPoint, new Point(Player1.X - 5, Player1.Y), new Point(Player1.X + 5, Player1.Y)))
                        {
                            Random rng = new Random();
                            Player1 = new Point(rng.Next(1, this.Width), rng.Next(1, this.Height));
                            Score[1]++;
                            //MessageBox.Show("TÉ MOURU!");
                        }
                    }
                }
            }
        }
    }
}
