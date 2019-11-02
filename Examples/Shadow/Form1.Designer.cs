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
            this.label1 = new System.Windows.Forms.Label();
            this.tbLight = new System.Windows.Forms.TextBox();
            this.tbImageSize = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbDark = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbSmooth = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbSampling = new System.Windows.Forms.TextBox();
            this.labels = new System.Windows.Forms.Label();
            this.tbOk = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(2, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Light";
            // 
            // tbLight
            // 
            this.tbLight.Location = new System.Drawing.Point(38, 8);
            this.tbLight.Name = "tbLight";
            this.tbLight.Size = new System.Drawing.Size(100, 20);
            this.tbLight.TabIndex = 1;
            // 
            // tbImageSize
            // 
            this.tbImageSize.Location = new System.Drawing.Point(225, 8);
            this.tbImageSize.Name = "tbImageSize";
            this.tbImageSize.Size = new System.Drawing.Size(40, 20);
            this.tbImageSize.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(144, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "ShadowImage";
            // 
            // tbDark
            // 
            this.tbDark.Location = new System.Drawing.Point(329, 8);
            this.tbDark.Name = "tbDark";
            this.tbDark.Size = new System.Drawing.Size(35, 20);
            this.tbDark.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(271, 11);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Darkness";
            // 
            // tbSmooth
            // 
            this.tbSmooth.Location = new System.Drawing.Point(433, 8);
            this.tbSmooth.Name = "tbSmooth";
            this.tbSmooth.Size = new System.Drawing.Size(52, 20);
            this.tbSmooth.TabIndex = 7;
            this.tbSmooth.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tbSmooth_MouseDown);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(384, 11);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(43, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Smooth";
            // 
            // tbSampling
            // 
            this.tbSampling.Location = new System.Drawing.Point(547, 8);
            this.tbSampling.Name = "tbSampling";
            this.tbSampling.Size = new System.Drawing.Size(30, 20);
            this.tbSampling.TabIndex = 9;
            this.tbSampling.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tbSampling_MouseDown);
            // 
            // labels
            // 
            this.labels.AutoSize = true;
            this.labels.Location = new System.Drawing.Point(491, 11);
            this.labels.Name = "labels";
            this.labels.Size = new System.Drawing.Size(50, 13);
            this.labels.TabIndex = 8;
            this.labels.Text = "Sampling";
            // 
            // tbOk
            // 
            this.tbOk.Location = new System.Drawing.Point(651, 5);
            this.tbOk.Name = "tbOk";
            this.tbOk.Size = new System.Drawing.Size(35, 27);
            this.tbOk.TabIndex = 10;
            this.tbOk.Text = "OK";
            this.tbOk.UseVisualStyleBackColor = true;
            this.tbOk.Click += new System.EventHandler(this.tbOk_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.checkBox1);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.tbLight);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.tbOk);
            this.panel1.Controls.Add(this.tbImageSize);
            this.panel1.Controls.Add(this.tbSampling);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.labels);
            this.panel1.Controls.Add(this.tbDark);
            this.panel1.Controls.Add(this.tbSmooth);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(698, 38);
            this.panel1.TabIndex = 13;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(583, 10);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(65, 17);
            this.checkBox1.TabIndex = 13;
            this.checkBox1.Text = "Shadow";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 38);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(698, 423);
            this.panel2.TabIndex = 14;
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(698, 461);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "Form1";
            this.Text = "Shadow";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbLight;
        private System.Windows.Forms.TextBox tbImageSize;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbDark;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbSmooth;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbSampling;
        private System.Windows.Forms.Label labels;
        private System.Windows.Forms.Button tbOk;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.CheckBox checkBox1;
    }
}

