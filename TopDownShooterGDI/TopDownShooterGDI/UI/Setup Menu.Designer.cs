namespace TopDownShooterGDI.UI
{
    partial class Setup_Menu
    {
        /// <summary> 
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur de composants

        /// <summary> 
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas 
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtNAI = new System.Windows.Forms.TextBox();
            this.lblNAI = new System.Windows.Forms.Label();
            this.btnBack = new System.Windows.Forms.Button();
            this.brnOK = new System.Windows.Forms.Button();
            this.txtSHP = new System.Windows.Forms.TextBox();
            this.lblSHP = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtMSpeed = new System.Windows.Forms.TextBox();
            this.lblFireRate = new System.Windows.Forms.Label();
            this.txtFireRate = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtNAI
            // 
            this.txtNAI.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtNAI.Location = new System.Drawing.Point(66, 195);
            this.txtNAI.Name = "txtNAI";
            this.txtNAI.Size = new System.Drawing.Size(100, 20);
            this.txtNAI.TabIndex = 0;
            this.txtNAI.Text = "1";
            // 
            // lblNAI
            // 
            this.lblNAI.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblNAI.AutoSize = true;
            this.lblNAI.Location = new System.Drawing.Point(63, 179);
            this.lblNAI.Name = "lblNAI";
            this.lblNAI.Size = new System.Drawing.Size(106, 13);
            this.lblNAI.TabIndex = 1;
            this.lblNAI.Text = "Number of enemy AI:";
            // 
            // btnBack
            // 
            this.btnBack.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnBack.Location = new System.Drawing.Point(121, 221);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(75, 23);
            this.btnBack.TabIndex = 2;
            this.btnBack.Text = "Back";
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // brnOK
            // 
            this.brnOK.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.brnOK.Location = new System.Drawing.Point(40, 221);
            this.brnOK.Name = "brnOK";
            this.brnOK.Size = new System.Drawing.Size(75, 23);
            this.brnOK.TabIndex = 3;
            this.brnOK.Text = "OK";
            this.brnOK.UseVisualStyleBackColor = true;
            this.brnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // txtSHP
            // 
            this.txtSHP.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtSHP.Location = new System.Drawing.Point(66, 156);
            this.txtSHP.Name = "txtSHP";
            this.txtSHP.Size = new System.Drawing.Size(100, 20);
            this.txtSHP.TabIndex = 4;
            this.txtSHP.Text = "1";
            // 
            // lblSHP
            // 
            this.lblSHP.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblSHP.AutoSize = true;
            this.lblSHP.Location = new System.Drawing.Point(63, 140);
            this.lblSHP.Name = "lblSHP";
            this.lblSHP.Size = new System.Drawing.Size(64, 13);
            this.lblSHP.TabIndex = 5;
            this.lblSHP.Text = "Starting HP:";
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(63, 101);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(133, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Starting Movement Speed:";
            // 
            // txtMSpeed
            // 
            this.txtMSpeed.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtMSpeed.Location = new System.Drawing.Point(66, 117);
            this.txtMSpeed.Name = "txtMSpeed";
            this.txtMSpeed.Size = new System.Drawing.Size(100, 20);
            this.txtMSpeed.TabIndex = 6;
            this.txtMSpeed.Text = "5";
            // 
            // lblFireRate
            // 
            this.lblFireRate.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblFireRate.AutoSize = true;
            this.lblFireRate.Location = new System.Drawing.Point(63, 62);
            this.lblFireRate.Name = "lblFireRate";
            this.lblFireRate.Size = new System.Drawing.Size(89, 13);
            this.lblFireRate.TabIndex = 9;
            this.lblFireRate.Text = "Starting FireRate:";
            // 
            // txtFireRate
            // 
            this.txtFireRate.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtFireRate.Location = new System.Drawing.Point(66, 78);
            this.txtFireRate.Name = "txtFireRate";
            this.txtFireRate.Size = new System.Drawing.Size(100, 20);
            this.txtFireRate.TabIndex = 8;
            this.txtFireRate.Text = "5";
            // 
            // Setup_Menu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.lblFireRate);
            this.Controls.Add(this.txtFireRate);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtMSpeed);
            this.Controls.Add(this.lblSHP);
            this.Controls.Add(this.txtSHP);
            this.Controls.Add(this.brnOK);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.lblNAI);
            this.Controls.Add(this.txtNAI);
            this.Name = "Setup_Menu";
            this.Size = new System.Drawing.Size(236, 272);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtNAI;
        private System.Windows.Forms.Label lblNAI;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Button brnOK;
        private System.Windows.Forms.TextBox txtSHP;
        private System.Windows.Forms.Label lblSHP;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtMSpeed;
        private System.Windows.Forms.Label lblFireRate;
        private System.Windows.Forms.TextBox txtFireRate;
    }
}
