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
            this.panel1 = new System.Windows.Forms.Panel();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.btnShowSolids = new System.Windows.Forms.Button();
            this.lbSolids = new System.Windows.Forms.ListBox();
            this.btnExchange = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rbDifference = new System.Windows.Forms.RadioButton();
            this.rbUnion = new System.Windows.Forms.RadioButton();
            this.rbIntersect = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbExtruded = new System.Windows.Forms.CheckBox();
            this.cbTorus = new System.Windows.Forms.CheckBox();
            this.cbCone = new System.Windows.Forms.CheckBox();
            this.xbSphere = new System.Windows.Forms.CheckBox();
            this.cbBox2 = new System.Windows.Forms.CheckBox();
            this.cbBox1 = new System.Windows.Forms.CheckBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.checkBox1);
            this.panel1.Controls.Add(this.btnShowSolids);
            this.panel1.Controls.Add(this.lbSolids);
            this.panel1.Controls.Add(this.btnExchange);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(736, 128);
            this.panel1.TabIndex = 0;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(577, 84);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(62, 17);
            this.checkBox1.TabIndex = 14;
            this.checkBox1.Text = "Smooth";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // btnShowSolids
            // 
            this.btnShowSolids.Location = new System.Drawing.Point(577, 49);
            this.btnShowSolids.Margin = new System.Windows.Forms.Padding(4);
            this.btnShowSolids.Name = "btnShowSolids";
            this.btnShowSolids.Size = new System.Drawing.Size(144, 28);
            this.btnShowSolids.TabIndex = 8;
            this.btnShowSolids.Text = "Show Source Solids";
            this.btnShowSolids.UseVisualStyleBackColor = true;
            this.btnShowSolids.Click += new System.EventHandler(this.btnShowSolids_Click);
            // 
            // lbSolids
            // 
            this.lbSolids.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.lbSolids.FormattingEnabled = true;
            this.lbSolids.Location = new System.Drawing.Point(399, 25);
            this.lbSolids.Margin = new System.Windows.Forms.Padding(4);
            this.lbSolids.Name = "lbSolids";
            this.lbSolids.Size = new System.Drawing.Size(152, 95);
            this.lbSolids.TabIndex = 5;
            this.lbSolids.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.listBox1_DrawItem);
            // 
            // btnExchange
            // 
            this.btnExchange.Location = new System.Drawing.Point(577, 13);
            this.btnExchange.Margin = new System.Windows.Forms.Padding(4);
            this.btnExchange.Name = "btnExchange";
            this.btnExchange.Size = new System.Drawing.Size(144, 28);
            this.btnExchange.TabIndex = 4;
            this.btnExchange.Text = "Apply Operation";
            this.btnExchange.UseVisualStyleBackColor = true;
            this.btnExchange.Click += new System.EventHandler(this.btnExchange_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(417, 5);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Used Solids";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rbDifference);
            this.groupBox2.Controls.Add(this.rbUnion);
            this.groupBox2.Controls.Add(this.rbIntersect);
            this.groupBox2.Location = new System.Drawing.Point(213, 0);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox2.Size = new System.Drawing.Size(165, 123);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "SetOperations";
            // 
            // rbDifference
            // 
            this.rbDifference.AutoSize = true;
            this.rbDifference.Location = new System.Drawing.Point(8, 68);
            this.rbDifference.Margin = new System.Windows.Forms.Padding(4);
            this.rbDifference.Name = "rbDifference";
            this.rbDifference.Size = new System.Drawing.Size(74, 17);
            this.rbDifference.TabIndex = 3;
            this.rbDifference.Text = "Difference";
            this.rbDifference.UseVisualStyleBackColor = true;
            this.rbDifference.CheckedChanged += new System.EventHandler(this.rbDifference_CheckedChanged);
            // 
            // rbUnion
            // 
            this.rbUnion.AutoSize = true;
            this.rbUnion.Checked = true;
            this.rbUnion.Location = new System.Drawing.Point(8, 46);
            this.rbUnion.Margin = new System.Windows.Forms.Padding(4);
            this.rbUnion.Name = "rbUnion";
            this.rbUnion.Size = new System.Drawing.Size(53, 17);
            this.rbUnion.TabIndex = 2;
            this.rbUnion.TabStop = true;
            this.rbUnion.Text = "Union";
            this.rbUnion.UseVisualStyleBackColor = true;
            this.rbUnion.CheckedChanged += new System.EventHandler(this.rbUnion_CheckedChanged);
            // 
            // rbIntersect
            // 
            this.rbIntersect.AutoSize = true;
            this.rbIntersect.Location = new System.Drawing.Point(8, 24);
            this.rbIntersect.Margin = new System.Windows.Forms.Padding(4);
            this.rbIntersect.Name = "rbIntersect";
            this.rbIntersect.Size = new System.Drawing.Size(66, 17);
            this.rbIntersect.TabIndex = 1;
            this.rbIntersect.Text = "Intersect";
            this.rbIntersect.UseVisualStyleBackColor = true;
            this.rbIntersect.CheckedChanged += new System.EventHandler(this.rbIntersect_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbExtruded);
            this.groupBox1.Controls.Add(this.cbTorus);
            this.groupBox1.Controls.Add(this.cbCone);
            this.groupBox1.Controls.Add(this.xbSphere);
            this.groupBox1.Controls.Add(this.cbBox2);
            this.groupBox1.Controls.Add(this.cbBox1);
            this.groupBox1.Location = new System.Drawing.Point(4, 0);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(201, 123);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "choose two solids";
            // 
            // cbExtruded
            // 
            this.cbExtruded.AutoSize = true;
            this.cbExtruded.Location = new System.Drawing.Point(111, 82);
            this.cbExtruded.Margin = new System.Windows.Forms.Padding(4);
            this.cbExtruded.Name = "cbExtruded";
            this.cbExtruded.Size = new System.Drawing.Size(68, 17);
            this.cbExtruded.TabIndex = 5;
            this.cbExtruded.Text = "Extruded";
            this.cbExtruded.UseVisualStyleBackColor = true;
            this.cbExtruded.CheckedChanged += new System.EventHandler(this.cbExtruded_CheckedChanged);
            // 
            // cbTorus
            // 
            this.cbTorus.AutoSize = true;
            this.cbTorus.Location = new System.Drawing.Point(111, 53);
            this.cbTorus.Margin = new System.Windows.Forms.Padding(4);
            this.cbTorus.Name = "cbTorus";
            this.cbTorus.Size = new System.Drawing.Size(53, 17);
            this.cbTorus.TabIndex = 4;
            this.cbTorus.Text = "Torus";
            this.cbTorus.UseVisualStyleBackColor = true;
            this.cbTorus.CheckedChanged += new System.EventHandler(this.cbTorus_CheckedChanged);
            // 
            // cbCone
            // 
            this.cbCone.AutoSize = true;
            this.cbCone.Location = new System.Drawing.Point(111, 23);
            this.cbCone.Margin = new System.Windows.Forms.Padding(4);
            this.cbCone.Name = "cbCone";
            this.cbCone.Size = new System.Drawing.Size(51, 17);
            this.cbCone.TabIndex = 3;
            this.cbCone.Text = "Cone";
            this.cbCone.UseVisualStyleBackColor = true;
            this.cbCone.CheckedChanged += new System.EventHandler(this.cbCone_CheckedChanged);
            // 
            // xbSphere
            // 
            this.xbSphere.AutoSize = true;
            this.xbSphere.Location = new System.Drawing.Point(9, 84);
            this.xbSphere.Margin = new System.Windows.Forms.Padding(4);
            this.xbSphere.Name = "xbSphere";
            this.xbSphere.Size = new System.Drawing.Size(60, 17);
            this.xbSphere.TabIndex = 2;
            this.xbSphere.Text = "Sphere";
            this.xbSphere.UseVisualStyleBackColor = true;
            this.xbSphere.CheckedChanged += new System.EventHandler(this.xbSphere_CheckedChanged);
            // 
            // cbBox2
            // 
            this.cbBox2.AutoSize = true;
            this.cbBox2.Location = new System.Drawing.Point(9, 54);
            this.cbBox2.Margin = new System.Windows.Forms.Padding(4);
            this.cbBox2.Name = "cbBox2";
            this.cbBox2.Size = new System.Drawing.Size(50, 17);
            this.cbBox2.TabIndex = 1;
            this.cbBox2.Text = "Box2";
            this.cbBox2.UseVisualStyleBackColor = true;
            this.cbBox2.CheckedChanged += new System.EventHandler(this.cbBox2_CheckedChanged);
            // 
            // cbBox1
            // 
            this.cbBox1.AutoSize = true;
            this.cbBox1.Location = new System.Drawing.Point(9, 25);
            this.cbBox1.Margin = new System.Windows.Forms.Padding(4);
            this.cbBox1.Name = "cbBox1";
            this.cbBox1.Size = new System.Drawing.Size(50, 17);
            this.cbBox1.TabIndex = 0;
            this.cbBox1.Text = "Box1";
            this.cbBox1.UseVisualStyleBackColor = true;
            this.cbBox1.CheckedChanged += new System.EventHandler(this.cbBox1_CheckedChanged);
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 128);
            this.panel2.Margin = new System.Windows.Forms.Padding(4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(736, 433);
            this.panel2.TabIndex = 1;
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(736, 561);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.Text = "Set operations";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnExchange;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.CheckBox xbSphere;
        public System.Windows.Forms.CheckBox cbBox2;
        public System.Windows.Forms.CheckBox cbBox1;
        public System.Windows.Forms.CheckBox cbExtruded;
        public System.Windows.Forms.CheckBox cbTorus;
        public System.Windows.Forms.CheckBox cbCone;
        public System.Windows.Forms.RadioButton rbUnion;
        public System.Windows.Forms.RadioButton rbIntersect;
        public System.Windows.Forms.RadioButton rbDifference;
        public System.Windows.Forms.ListBox lbSolids;
        private System.Windows.Forms.Button btnShowSolids;
        private System.Windows.Forms.CheckBox checkBox1;
    }
}

