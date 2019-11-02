using Drawing3d;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
namespace Sample
{
    public partial class Form1 : Form
    {
        MyDevice Device = new MyDevice();
        public Form1()
        {
            InitializeComponent();
            Device.WinControl = this;

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Device.SelectModus = checkBox1.Checked;
        }
    }
    public class MyDevice:OpenGlDevice
    {
        internal bool SelectModus = true;
        CtrlRectangle MarcRect = new CtrlRectangle();
     
        protected override void OnCreated()
        {
            base.OnCreated();
          
            MarcRect.MouseUp += MarcRect_MouseUp;
        }

        private void MarcRect_MouseUp(object sender, Drawing3d.HandledMouseEventArgs e)
        {
            Selector.SnapInside(MarcRect.Rectangle);
            if (Form.ModifierKeys != Keys.Shift) MarkList.Clear();
            for (int i = 0; i < SnappItems.Count; i++)
            {
                if (IndexOfTag(SnappItems[i].Tag) < 0)
                    MarkList.Add(SnappItems[i].Tag);
            }
            e.Handled = true;
            MarcRect.OnLogout(false);
        }

     

        
        List<object> MarkList = new List<object>();
        int IndexOfTag(object Tag)
        {
            for (int i = 0; i < MarkList.Count; i++)
            {
                if ((int)MarkList[i] == (int)Tag) return i;
            }
            return -1;
        }
        public override void OnPaint()
        {
           
            base.OnPaint();
            PushTag(1);
            if (IndexOfTag(1) >= 0)
                Emission = Color.Red;
            drawBox(new xyz(-6, 0, 0), new xyz(4, 4, 7));
            PopTag();
            Emission = Color.Black;
            PushTag(2);
            if ( (IndexOfTag(2) >= 0))
                Emission = Color.Red;
            drawBox(new xyz(1, 0, 0), new xyz(4, 4, 5));
            PopTag();
            Emission = Color.Black;
          
         
        }
     
        protected override void onMouseDown(MouseEventArgs e)
        {
            if (SelectModus)
            {
                   MarcRect.Creating = true;
                   MarcRect.OnLogin(this, 0);
            }
            else
                base.onMouseDown(e); // Navigating
        }
    }
}
