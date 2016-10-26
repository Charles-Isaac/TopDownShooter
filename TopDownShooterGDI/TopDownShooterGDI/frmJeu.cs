//#define Experimental   //garantie le non fonctionnement du projet
#define ShowFPS

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using TopDownShooterGDI.Objects;

namespace TopDownShooterGDI
{

    public partial class frmJeu : Form
    {

        #region Members

        SizeF Format;
        Point FPS = new Point(0, 0);
#if Experimental
        Point DefaultResolution = new Point(0, 0);
#else
        Point DefaultResolution;
#endif
        int NbrPlayer = 1;
#if ShowFPS
        int NbrAI = 50;  //Test des FPS avec 50 joueurs
#else
        int NbrAI = 5;
#endif
        int DefaultFireRate = 400;
        int DefaultPlayerSpeed = 20;
        int DefaultPlayerLife = 1;

        int NMur;
        public int[,] WallX;
        public int[,] WallY;

        
        
        Point Light;
        List<Player> Players = new List<Player>();
        Pen myPen = new Pen(Color.Blue);
        Brush Br = new SolidBrush(Color.Black);
        Point[] Shadows = new Point[6];
        Point[,] ShadowArray;
        //TopDownShooter.Objects.ShadowPolygon SP;

#endregion
        public frmJeu()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            if (!this.Controls.Contains(UI.Main_Menu.Instance))
            {
                this.Controls.Add(UI.Main_Menu.Instance);
                UI.Main_Menu.Instance.BringToFront();
            }
            else
            {
                UI.Main_Menu.Instance.Show();
                UI.Main_Menu.Instance.BringToFront();
            }
        }
        public void Setup(int ai, int speed, int hp, int firerate)
        {
            NbrAI = ai;
            DefaultFireRate = firerate;
            DefaultPlayerSpeed = speed;
            DefaultPlayerLife = hp;


        }
        public void StartGame()
        {
            this.Controls.Clear();
            this.Focus();
            Random rng = new Random();
#if !Experimental
            DefaultResolution = new Point(this.Width, this.Height);
#endif
            myPen.Color = Color.Blue;
            myPen.Width = 5;
            for (int i = 0; i < NbrPlayer; i++)
            {
                Players.Add(new Player(new PointF(rng.Next(5, DefaultResolution.X), rng.Next(5, DefaultResolution.Y)), new PointF(0, 0), false, DefaultFireRate, DefaultPlayerLife, DefaultPlayerSpeed, DefaultPlayerLife));
                while (Players[i].Position.X > 25 * 4 && Players[i].Position.X < 75 * 4 && Players[i].Position.Y > 25 * 4 && Players[i].Position.Y < 75 * 4)
                {
                    Players[i].Position = new PointF(rng.Next(1, DefaultResolution.X), rng.Next(1, DefaultResolution.Y));
                }
                if ((Players[i].Position.X > 25 && Players[i].Position.X < 75 && Players[i].Position.Y > 25 && Players[i].Position.Y < 75))
                {
                    MessageBox.Show("Test");
                }
            }
            for (int i = NbrPlayer; i < NbrPlayer + NbrAI; i++)
            {
                Players.Add(new Player(new PointF(rng.Next(5, DefaultResolution.X), rng.Next(5, DefaultResolution.Y)), new PointF(0, 0), true, DefaultFireRate, DefaultPlayerLife, DefaultPlayerSpeed, DefaultPlayerLife));
                while (Players[i].Position.X >= 25 * 4 && Players[i].Position.X < 75 * 4 && Players[i].Position.Y >= 25 * 4 && Players[i].Position.Y <= 75 * 4)
                {
                    Players[i].Position = new PointF(rng.Next(1, DefaultResolution.X), rng.Next(1, DefaultResolution.Y));
                }

            }

            if (!GetMap(out WallX, out WallY))
            {
                MessageBox.Show("Error fetching map from server");
                //drop game
            }

            //   AllocConsole();
            UI.Main_Menu.Instance.Dispose();    //necesary?
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.frmJeu_Paint);
            FPS.X = System.Environment.TickCount;
            while (true)
            {
                FPS.Y++;

                this.Refresh();
                Application.DoEvents();

                if (FPS.Y > 100)
                {
                    FPS.Y = 1;
                    FPS.X = System.Environment.TickCount - 1;
                }
            }
        }



#region Console
        /* [DllImport("kernel32.dll", SetLastError = true)]
         [return: MarshalAs(UnmanagedType.Bool)]
         static extern bool AllocConsole();*/
#endregion
        private void frmJeu_Load(object sender, EventArgs e)
        {
            


        }
        private void frmJeu_Paint(object sender, PaintEventArgs e)
        {
            Light = this.PointToClient(Cursor.Position);
            /*if (this.Height > this.Width - (this.Width / 8))
            {
                e.Graphics.TranslateTransform(0f, this.Height - (this.Height * Format.Height) - this.Width / 16);


                Light.X = (int)(Light.X / Format.Width);
                Light.Y = (int)(Light.Y / Format.Height - (this.Height - (this.Height * Format.Height) - this.Width / 16));
            }
            else*/
      //      {
                Light.X = (int)(Light.X / Format.Width);
                Light.Y = (int)(Light.Y / Format.Height);
     //       }
            e.Graphics.ScaleTransform(Format.Width, Format.Height, System.Drawing.Drawing2D.MatrixOrder.Append);



                                    // for (int i = 0; i < Players.Count; i++)
            {
                if (!GetPlayerPosition(ref Players))
                {
                    MessageBox.Show("stuff");
                }
            }

            Brush PlayerBrush = new SolidBrush(Color.Red);
            for (int i = 0; i < Players.Count; i++)
            {
                e.Graphics.FillEllipse(new SolidBrush(Color.Red), Players[i].Position.X - 5, Players[i].Position.Y - 5, 10, 10);
            }

            
            

            //FOV
            //e.Graphics.FillPie(Br, Players[0].Position.X - 2000, Players[0].Position.Y - 2000, 4000, 4000, (float)(Math.Atan2(Light.Y - Player1.Y, Light.X - Player1.X) / Math.PI * 180 +80), 200f);


            ShadowArray = ShadowPolygon.ReturnMeAnArray(NMur, WallX, WallY, new PointF(Players[0].Position.X, Players[0].Position.Y), DefaultResolution.X/*this.Width*/, DefaultResolution.Y/*this.Height*/);


            for (int i = 0; i < NMur; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    Shadows[j] = ShadowArray[i, j];
                }
                try
                {
                    e.Graphics.FillPolygon(Br, Shadows);
                }
                catch { }
            }
            BulletMovement.CalculateBulletMovement(ref Players, DefaultResolution.X, DefaultResolution.Y, WallX, WallY, NMur);
            for (int i = 0; i < Players.Count; i++)
            {
                for (int j = 0; j < Players[i].PlayerBullet.Count; j++)
                {
                    e.Graphics.FillEllipse(new SolidBrush(Color.Red), Players[i].PlayerBullet[j].Position.X - 2.5f, Players[i].PlayerBullet[j].Position.Y - 2.5f, 5, 5);
                }
            }
            for (int i = 0; i < NMur; i++)
            {
                e.Graphics.DrawLine(myPen, WallX[i, 0], WallY[i, 0], WallX[i, 1], WallY[i, 1]);
            }

            e.Graphics.FillEllipse(new SolidBrush(Color.Green), Players[0].Position.X - 5, Players[0].Position.Y - 5, 10, 10);

            e.Graphics.DrawLine(new Pen(Color.Green), Players[0].Position, Light);
            for (int i = 0; i < Players.Count && i < 5; i++) // && i < 5 est utilisé pour eviter d'afficher plsude scores que la boite ne le permet
            {
                e.Graphics.DrawString(Players[i].Score.ToString(), new Font("Time New Roman", 18), new SolidBrush(Color.Yellow), 180, 120 + 30 * i);
            }

            e.Graphics.DrawLine(new Pen(Color.Black, 5.0f), new Point(DefaultResolution.X, 0), DefaultResolution);



#if ShowFPS
             //FPS
            e.Graphics.DrawString(((long)FPS.Y * 1000 / (Environment.TickCount - FPS.X)).ToString(), new Font("Time New Roman", 18), new SolidBrush(Color.Red), 25,25);
#endif

        }
#region KeyEvents
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
#if Experimental
            if (e.KeyData == Keys.P)
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
            }

            if (e.KeyData == Keys.H)
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
            }

             if (e.KeyData == Keys.OemMinus || e.KeyData == Keys.Oemplus)
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


             }
#else
            base.OnKeyDown(e);
            switch (e.KeyCode)
            {
                /*case Keys.I:
                    Players[0].IsAI = !Players[0].IsAI;
                    break;*/ //On/Off AI main player, used for debug purpose (and cheating)
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
                    Light = this.PointToClient(Cursor.Position);
                    Light.X = (int)(Light.X / Format.Width);
                    Light.Y = (int)(Light.Y / Format.Height);
                    Players[0].Position = Light;
                    break;
                default:
                    return;
            }
            HandleArrows();
            e.Handled = true;
#endif
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
            
            if ((arrowsPressed & ArrowsPressed.Up) == ArrowsPressed.None && (arrowsPressed & ArrowsPressed.Down) != ArrowsPressed.None)
            {
                Players[0].Vector = new PointF(Players[0].Vector.X, 1);
                //VY = 1;
            }

            if ((arrowsPressed & ArrowsPressed.Down) == ArrowsPressed.None && (arrowsPressed & ArrowsPressed.Up) != ArrowsPressed.None)
            {
                Players[0].Vector = new PointF(Players[0].Vector.X, -1);
                //VY = -1;
            }

            if ((arrowsPressed & ArrowsPressed.Right) == ArrowsPressed.None && (arrowsPressed & ArrowsPressed.Left) != ArrowsPressed.None)
            {
                Players[0].Vector = new PointF(-1, Players[0].Vector.Y);
                //VX = -1;
            }

            if ((arrowsPressed & ArrowsPressed.Left) == ArrowsPressed.None && (arrowsPressed & ArrowsPressed.Right) != ArrowsPressed.None)
            {
                Players[0].Vector = new PointF(1, Players[0].Vector.Y);
                //VX = 1;
            }

            if (((arrowsPressed & ArrowsPressed.Up) | (arrowsPressed & ArrowsPressed.Down)) == ArrowsPressed.None)
            {
                Players[0].Vector = new PointF(Players[0].Vector.X, 0);
                //VY = 0;
            }

            if (((arrowsPressed & ArrowsPressed.Right) | (arrowsPressed & ArrowsPressed.Left)) == ArrowsPressed.None)
            {
                Players[0].Vector = new PointF(0, Players[0].Vector.Y);
                //VX = 0;
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
        }
#endregion
        private void frmJeu_MouseClick(object sender, MouseEventArgs e)
        {
            Players[0].PlayerBullet.Add(new Bullet(Players[0].Position, new PointF((Light.X - Players[0].Position.X)/ (float)Math.Sqrt((Light.X - Players[0].Position.X) * (Light.X - Players[0].Position.X)+ (Light.Y - Players[0].Position.Y)* (Light.Y - Players[0].Position.Y)),
                (Light.Y - Players[0].Position.Y) / (float)Math.Sqrt((Light.X - Players[0].Position.X) * (Light.X - Players[0].Position.X) + (Light.Y - Players[0].Position.Y) * (Light.Y - Players[0].Position.Y)))));
            Players[0].Score--;
        }
        
        private bool GetPlayerPosition(ref List<Player> Players)
        {

            AICode.Zaratoustra(ref Players, NMur, WallX, WallY, DefaultResolution.X, DefaultResolution.Y, NbrPlayer, NbrAI);
           
            return true;
        }
        private bool GetMap(out int[,] WallX, out int[,] WallY)
        {
            NMur = 15;
#if Experimental
                 int NPlayer = 2;
                 for (int i = 0; i < NPlayer; i++)
                 {
                     Score.Add(0);
                 }
#endif

            WallX = new int[NMur, 2];
            WallY = new int[NMur, 2];

            Random rng = new Random();
            
            WallX[0, 0] = 25 * 4;
            WallY[0, 0] = 25 * 4;
            WallX[0, 1] = 75 * 4;
            WallY[0, 1] = 25 * 4;

            WallX[1, 0] = 25 * 4;
            WallY[1, 0] = 75 * 4;
            WallX[1, 1] = 75 * 4;
            WallY[1, 1] = 75 * 4;

            WallX[2, 0] = 25 * 4;
            WallY[2, 0] = 25 * 4;
            WallX[2, 1] = 25 * 4;
            WallY[2, 1] = 75 * 4;

            WallX[3, 0] = 75 * 4;
            WallY[3, 0] = 25 * 4;
            WallX[3, 1] = 75 * 4;
            WallY[3, 1] = 75 * 4;

            for (int i = 4; i < NMur; i++)
            {
                WallX[i, 0] = rng.Next(5, DefaultResolution.X - 5);
                WallY[i, 0] = rng.Next(5, DefaultResolution.Y - 5);
                WallX[i, 1] = rng.Next(5, DefaultResolution.X - 5);
                WallY[i, 1] = rng.Next(5, DefaultResolution.Y - 5);

                for (int j = 0; j < i; j++)
                {
                    if (Intersecting.IsIntersecting(new Point(WallX[i,0], WallY[i,0]), new Point(WallX[i, 1], WallY[i, 1]), new Point(WallX[j, 0], WallY[j, 0]), new Point(WallX[j, 1], WallY[j, 1])))
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

        private void frmJeu_Resize(object sender, EventArgs e)
        {
#if !Experimental
            Format.Width = 1;
            Format.Height = 1;
            return;
#else
            if (this.Height < this.Width - (this.Width / 8))
            {
                Format.Width = (float)(this.Width - (this.Width - this.Height)) / DefaultResolution.X;
                Format.Height = (float)this.Height / DefaultResolution.Y;
            }
            else
            {
                this.Height = this.Width - (this.Width / 8);
                Format.Width = (float)(this.Width - (this.Width - this.Height)) / DefaultResolution.X;
                Format.Height = (float)this.Height / DefaultResolution.Y;
                //    Format.Width = (float)(this.Width - (this.Width / 8)) / DefaultResolution.X;
                //    Format.Height = (float)(this.Width - (this.Width / 8)) / DefaultResolution.Y;
            }
#endif

        }
    }
}
