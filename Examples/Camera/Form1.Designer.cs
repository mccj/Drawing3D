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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button7 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button7);
            this.groupBox1.Controls.Add(this.button3);
            this.groupBox1.Controls.Add(this.button9);
            this.groupBox1.Controls.Add(this.button8);
            this.groupBox1.Controls.Add(this.button6);
            this.groupBox1.Controls.Add(this.button5);
            this.groupBox1.Controls.Add(this.button4);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Location = new System.Drawing.Point(737, 21);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(116, 134);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Navigate";
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(80, 98);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(27, 30);
            this.button7.TabIndex = 10;
            this.button7.Text = "B";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.Back_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(14, 98);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(27, 30);
            this.button3.TabIndex = 9;
            this.button3.Text = "F";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.Forward_Click);
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(47, 63);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(27, 30);
            this.button9.TabIndex = 8;
            this.button9.Text = "S";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.Stop_Click);
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(47, 99);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(27, 30);
            this.button8.TabIndex = 7;
            this.button8.Text = "D";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.Down_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(80, 63);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(27, 30);
            this.button6.TabIndex = 5;
            this.button6.Text = "R";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.Right_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(80, 27);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(27, 30);
            this.button5.TabIndex = 4;
            this.button5.Text = "\\";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.RollDown_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(47, 27);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(27, 30);
            this.button4.TabIndex = 3;
            this.button4.Text = "U";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.Up_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(14, 63);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(27, 30);
            this.button2.TabIndex = 1;
            this.button2.Text = "L";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.Left_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(14, 27);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(27, 30);
            this.button1.TabIndex = 0;
            this.button1.Text = "/";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.RollUp_Click);
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(882, 553);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form1";
            this.Text = "Camera with Animation";
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }


        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
    }
}

