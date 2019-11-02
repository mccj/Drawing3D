using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Drawing3d;
namespace Sample
{
    public partial class Form1 : Form
    {
        MyDevice Device = new MyDevice();
        public static Form1 CurrentForm = null;
        public Form1()
        {
            InitializeComponent();
            CurrentForm = this;
            Device.WinControl = panel2;
        }

        private void cbBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (cbBox1.Checked)
            {
                lbSolids.Items.Add(Device.Box1);
            }
            else
                lbSolids.Items.Remove(Device.Box1);
          
        }
        private void cbBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (cbBox2.Checked)
            {
                lbSolids.Items.Add(Device.Box2);
            }
            else
                lbSolids.Items.Remove(Device.Box2);
          
        }
        private void listBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
            Solid S = lbSolids.Items[e.Index] as Solid;
            e.Graphics.DrawString(S.Name, lbSolids.Font, Brushes.Black, new PointF(0, e.Index * lbSolids.Font.Height));
        }

        private void xbSphere_CheckedChanged(object sender, EventArgs e)
        {

            if (xbSphere.Checked)
            {
                lbSolids.Items.Add(Device.Sphere);
            }
            else
            {
               lbSolids.Items.Remove(Device.Sphere);
            }
         
        }

        private void cbCone_CheckedChanged(object sender, EventArgs e)
        {
            if (cbCone.Checked)
            {
                lbSolids.Items.Add(Device.Cone);
            }
            else
                lbSolids.Items.Remove(Device.Cone);
          
        }

        private void cbTorus_CheckedChanged(object sender, EventArgs e)
        {
            if (cbTorus.Checked)
            {
                lbSolids.Items.Add(Device.Torus);
            }
            else
                lbSolids.Items.Remove(Device.Torus);
        }

        private void cbExtruded_CheckedChanged(object sender, EventArgs e)
        {
            if (cbExtruded.Checked)
            {
                lbSolids.Items.Add(Device.Extrude);
            }
            else
                lbSolids.Items.Remove(Device.Extrude);
          
        }
        SetOperation3D.SetOperation CurrentOperation = SetOperation3D.SetOperation.AorB; 
     
        private void rbIntersect_CheckedChanged(object sender, EventArgs e)
        {
            CurrentOperation = SetOperation3D.SetOperation.AandB;
        }
       private void rbDifference_CheckedChanged(object sender, EventArgs e)
        {
          
            CurrentOperation = SetOperation3D.SetOperation.AnotB;
        }

       

        private void rbUnion_CheckedChanged(object sender, EventArgs e)
        {
            CurrentOperation = SetOperation3D.SetOperation.AorB;
         
        }

       
     
        private void btnExchange_Click(object sender, EventArgs e)
        {
            if (lbSolids.Items.Count >= 2)
            {
                Device.Solids = SetOperation3D.GetCombinedSolids(lbSolids.Items[0] as DiscreteSolid, lbSolids.Items[1] as DiscreteSolid, CurrentOperation, Device.Traces);
                if (!Device.ShowSolids) btnShowSolids_Click(null, null);
            }
            else
                MessageBox.Show("At least two Solids must Selected");
       
        }

    
       
       

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            ListBox LB = lbSolids;
            for (int i = 0; i < LB.Items.Count; i++)
            {
                DiscreteSolid S = (LB.Items[i] as DiscreteSolid);
                S.SetSmooth(checkBox1.Checked);
            }
            if (Device.Solids!=null)
            for (int i = 0; i < Device.Solids.Count; i++)
            {
                Device.Solids[i].SetSmooth(checkBox1.Checked);
            }
        }

       

        private void btnShowSolids_Click(object sender, EventArgs e)
        {
            Device.ShowSolids = !Device.ShowSolids;
            if (Device.ShowSolids) btnShowSolids.Text = "Show Source Solids";
            else btnShowSolids.Text = "Result";
        }
    }
    public class MyDevice : OpenGlDevice
    {
        internal bool ShowSolids = false;
        internal List<DiscreteSolid> Solids = null;
        internal List<List<Line3D>> Traces = new List<List<Line3D>>();
        //     public List<DiscreteSolid> SetResult = null;
        //internal SolidBox Box1 = new SolidBox(new xyz(1, 1, 2), new xyz(3.5, 3.3, 4.5));
        //internal SolidBox Box2 = new SolidBox(new xyz(-2, -3, 1), new xyz(6.5, 6.5, 6));
        internal SolidBox Box1 = new SolidBox(new xyz(-1, -1, -1), new xyz(5, 5, 5));
        internal SolidBox Box2 = new SolidBox(new xyz(1, 1, 1), new xyz(5, 5, 5));
        internal DiscreteSphere Sphere = new DiscreteSphere(new xyz(0, 0, 0), 4, 16, 8);// kein Fehler
         internal DiscreteCone Cone = new DiscreteCone(5, 4, System.Math.PI / 4, 35);
         internal DiscreteTorus Torus = new DiscreteTorus(2, 5, 20, 20);
         internal DiscreteExtrude Extrude = new DiscreteExtrude();
       
       
        internal List<List<Line3D>> Trace = new List<List<Line3D>>();
        protected override void OnCreated()
        {
            base.OnCreated();
            Box1.Name = "Box1";
            Box2.Name = "Box2";
            Sphere.Name = "Sphere";
            Cone.Name = "Cone";
            Torus.Name = "Torus";
            
           
            xyArray A1 = new xyArray();
            Loxy Li = new Loxy();
           
            A1.data = new xy[] { new xy(0, 0), new xy(6, 0), new xy(6, 6), new xy(0, 6), new xy(0, 0) };
            Li.Add(A1);
            A1 = new xyArray();
            A1.data = new xy[] { new xy(2, 2), new xy(2, 4), new xy(4, 4), new xy(4, 2), new xy(2, 2) };
            Li.Add(A1);
            Extrude = new DiscreteExtrude(Li, 4);
            Extrude.Refresh();
            Extrude.Name = "Extruded";
    
         

            Extrude = new DiscreteExtrude(Li, 4);
            Extrude.Refresh();
            Extrude.Name = "Extruded";
            Torus.Transformation = Matrix.Translation(new xyz(0, 0, 0.5));
            // Outlook
            BackColor = Color.White;
            Cursor = D3DCursors.C1001;
        }
        bool ShowAll = true;
        Form1 Form()
        {
            return Form1.CurrentForm;
        }
      

        public override void OnPaint()
        {
           base.OnPaint();
         
            if ((ShowSolids) && (Solids != null))
            {
         


                for (int i = 0; i < Solids.Count; i++)
                {
                    Solids[i].Paint(this);
      
              }
                return;
            }
         
            if (ShowAll)

            {
                ListBox LB = Form().lbSolids;
                for (int i = 0; i < LB.Items.Count; i++)
                {
                    Solid S = (LB.Items[i] as Solid);
                    S.Paint(this);
                
                }
                for (int i = 0; i < Trace.Count; i++)
                {
                    Emission = Color.Red;
                    PenWidth = 1;
                    xyz Old = new xyz();
                    for (int j = 0; j < Trace[i].Count; j++)
                    {
                        int n = j - 1;
                        if (n ==-1)
                            n=Trace[i].Count - 1;

                        Old = Trace[i][n].B;
                        if (Old.dist(Trace[i][j].A) > 0.01)
                        {
                        }
                        drawLine(Trace[i][j].A, Trace[i][j].B);
                    }
                    Emission = Color.Black;
                }

            }

         
        }
       
        
    }
}
