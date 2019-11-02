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
        public Form1()
        {
            InitializeComponent();
            Device.WinControl = this;
        }
    }
    class MyDevice : OpenGlDevice
    {

        const int StarCount = 6;

        public List<xyArray> Stars = new List<xyArray>();
        public CtrlRectangle ControlRect = new CtrlRectangle();
        public CtrlRectangle MarkeredRect = new CtrlRectangle();
        List<Color> Colors = new List<Color>();
        protected override void onMouseDown(MouseEventArgs e)
        {
 
            base.onMouseDown(e);
            ControlRect = new CtrlRectangle(this);
            ControlRect.Creating = true;
       }
        protected override void onMouseUp(MouseEventArgs e)
        {
            base.onMouseUp(e);
            if (ControlRect.Device != null)
            {
                ControlRect.OnLogout(false);
                MouseUpEvent();
            }
        }
       

        private void MouseUpEvent()
        {
            System.Drawing.RectangleF R = ControlRect.Rectangle;
            System.Drawing.RectangleF envRect = new System.Drawing.RectangleF();

            List<ITransform2d> InsideStars = new List<ITransform2d>();
            {
                if (ControlRect.A.dist(ControlRect.B) < 0.01) // no moves betweeen Down and Up
                {
                    if ((SnappItems.Count > 0) && (SnappItems[0].Tag is int))
                    {
                        int Index = (int)SnappItems[0].Tag;
                        // Start a Marked rectangle for the rectangle where the pointer is
                        InsideStars.Add(Stars[Index]);
                    //    MarkeredRect.ShowMarked(this, Stars[Index].GetMaxrect(Matrix.identity), InsideStars);
                        return;
                    }
                }

                else
                { // Rectangle no empty .. Moves between down and up
                    for (int i = 0; i < Stars.Count; i++)
                    {
                        System.Drawing.RectangleF RF = Stars[i].GetMaxrect(Matrix.identity);
                        if (R.Contains(RF))
                        {
                            InsideStars.Add(Stars[i]);
                            envRect = Utils.Union(envRect, RF);
                        }
                    }
                    if (envRect.Size.Width > 0)
                    {
                        MarkeredRect = new CtrlRectangle(this, envRect, InsideStars);
                        MarkeredRect.Creating = false;
                    }
                }

            }
        }

        public override void OnPaint()
        {

            Color C = Emission;
            for (int i = 0; i < StarCount; i++)
            {
                Emission = Colors[i];
                PushTag(i);
                drawPolyLine(Stars[i]);
                PopTag();
            }
            Emission = C;
     
            base.OnPaint();


        }
        Random R = new Random(Guid.NewGuid().GetHashCode());
        double getRandom()
        {
            double a = R.NextDouble();

            return (a - 0.5) * 20;
        }
        protected override void OnCreated()
        {
            base.OnCreated();
            Navigating = false;
            MarkeredRect.LiveTransform = true;
            MarkeredRect.Creating = false;
            for (int i = 0; i < StarCount; i++)
            {
                
                xyArray Star = xyArray.CreateStar(new xy(getRandom(), getRandom()), 6 + (int)(R.NextDouble() * 6), 2, 5);
                Stars.Add(Star);
                byte[] CO = new byte[3];
                R.NextBytes(CO);
                Colors.Add(Color.FromArgb(255,CO[0], CO[1], CO[2]));
            }
           
           
            LightEnabled = false;
        }
    }
}
