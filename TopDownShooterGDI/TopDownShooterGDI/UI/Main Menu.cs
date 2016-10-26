using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TopDownShooterGDI.UI
{
    public partial class Main_Menu : UserControl
    {
        private static Main_Menu _Instance;
        public static Main_Menu Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new Main_Menu();
                }
                return _Instance;
            }
        }
        public Main_Menu()
        {
            InitializeComponent();
            this.Anchor = (AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {

            ((frmJeu)this.Parent).StartGame();
        }

        private void btnSetup_Click(object sender, EventArgs e)
        {
            if (!this.Parent.Controls.Contains(UI.Setup_Menu.Instance))
            {
                this.Parent.Controls.Add(UI.Setup_Menu.Instance);
                Setup_Menu.Instance.BringToFront();
            }
            else
            {
                Setup_Menu.Instance.Show();
                //Setup_Menu.Instance.BringToFront();
            }
        }
    }
}
