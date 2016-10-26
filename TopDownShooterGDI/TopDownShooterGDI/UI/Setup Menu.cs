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
    public partial class Setup_Menu : UserControl
    {
        private static Setup_Menu _Instance;
        public static Setup_Menu Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new Setup_Menu();
                }
                return _Instance;
            }
        }
        public Setup_Menu()
        {
            InitializeComponent();

            //  this.Anchor = (AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom);
            /////this.Width = ((frmJeu)this.Parent).Width;
            ///this.Height = ((frmJeu)this.Parent).Parent.Height;
            this.Dock = DockStyle.Fill;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            int AI, Speed, HP, FR;
            if (!int.TryParse(txtNAI.Text, out AI))
            {
                Environment.Exit(0);
            }

            if (!int.TryParse(txtMSpeed.Text, out Speed))
            {
                Environment.Exit(0);
            }

            if (!int.TryParse(txtSHP.Text, out HP))
            {
                Environment.Exit(0);
            }

            if (!int.TryParse(txtFireRate.Text, out FR))
            {
                Environment.Exit(0);
            }




            ((frmJeu)this.Parent).Setup(AI, Speed, HP, FR);
            this.Hide();
        }
    }
}
