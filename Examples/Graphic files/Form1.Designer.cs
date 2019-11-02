namespace Sample
{
    partial class Form1
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.btnStopanimation = new System.Windows.Forms.Button();
            this.btnContinue = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.lbAnimations = new System.Windows.Forms.ListBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.openFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnAnimate = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "DGN/DWG Files (*.dgn; *.dwg)|*.dgn; *.dwg\"";
            this.openFileDialog1.FilterIndex = 2;
            // 
            // btnStopanimation
            // 
            this.btnStopanimation.Location = new System.Drawing.Point(351, 0);
            this.btnStopanimation.Margin = new System.Windows.Forms.Padding(4);
            this.btnStopanimation.Name = "btnStopanimation";
            this.btnStopanimation.Size = new System.Drawing.Size(136, 28);
            this.btnStopanimation.TabIndex = 2;
            this.btnStopanimation.Text = "Stop Animation";
            this.btnStopanimation.UseVisualStyleBackColor = true;
            this.btnStopanimation.Visible = false;
            this.btnStopanimation.Click += new System.EventHandler(this.btnStopanimation_Click);
            // 
            // btnContinue
            // 
            this.btnContinue.Location = new System.Drawing.Point(495, 0);
            this.btnContinue.Margin = new System.Windows.Forms.Padding(4);
            this.btnContinue.Name = "btnContinue";
            this.btnContinue.Size = new System.Drawing.Size(151, 28);
            this.btnContinue.TabIndex = 3;
            this.btnContinue.Text = "Continue Animation";
            this.btnContinue.UseVisualStyleBackColor = true;
            this.btnContinue.Visible = false;
            this.btnContinue.Click += new System.EventHandler(this.btnContinue_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(655, 0);
            this.button2.Margin = new System.Windows.Forms.Padding(4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(170, 28);
            this.button2.TabIndex = 4;
            this.button2.Text = "Animation ID";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Visible = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // lbAnimations
            // 
            this.lbAnimations.FormattingEnabled = true;
            this.lbAnimations.Location = new System.Drawing.Point(655, 32);
            this.lbAnimations.Margin = new System.Windows.Forms.Padding(4);
            this.lbAnimations.Name = "lbAnimations";
            this.lbAnimations.Size = new System.Drawing.Size(240, 121);
            this.lbAnimations.TabIndex = 5;
            this.lbAnimations.Visible = false;
            this.lbAnimations.SelectedIndexChanged += new System.EventHandler(this.lbAnimations_SelectedIndexChanged);
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openFileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(576, 24);
            this.menuStrip1.TabIndex = 7;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // openFileToolStripMenuItem
            // 
            this.openFileToolStripMenuItem.Name = "openFileToolStripMenuItem";
            this.openFileToolStripMenuItem.Size = new System.Drawing.Size(66, 20);
            this.openFileToolStripMenuItem.Text = "OpenFile";
            this.openFileToolStripMenuItem.Click += new System.EventHandler(this.openFileToolStripMenuItem_Click);
            // 
            // btnAnimate
            // 
            this.btnAnimate.Location = new System.Drawing.Point(243, 0);
            this.btnAnimate.Margin = new System.Windows.Forms.Padding(4);
            this.btnAnimate.Name = "btnAnimate";
            this.btnAnimate.Size = new System.Drawing.Size(100, 28);
            this.btnAnimate.TabIndex = 1;
            this.btnAnimate.Text = "Animate";
            this.btnAnimate.UseVisualStyleBackColor = true;
            this.btnAnimate.Visible = false;
            this.btnAnimate.Click += new System.EventHandler(this.btnAnimate_Click);
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(576, 444);
            this.Controls.Add(this.lbAnimations);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.btnContinue);
            this.Controls.Add(this.btnStopanimation);
            this.Controls.Add(this.btnAnimate);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.Text = "Scenes";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button btnStopanimation;
        private System.Windows.Forms.Button btnContinue;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ListBox lbAnimations;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem openFileToolStripMenuItem;
        private System.Windows.Forms.Button btnAnimate;
    }
}

