using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarCopm
{
    public partial class Form1 : Form
    {
        List<Car> cars = new List<Car>();
        bool startDrawing = false;
        Bitmap map;
        Bitmap finalImage;

        public Form1()
        {
            InitializeComponent();
        }

        Timer t;
        private void Form1_Load(object sender, EventArgs e)
        {
            map = new Bitmap(@"C:\Users\levabala\Documents\Visual Studio 2012\Projects\Road1.bmp");//this.Size.Width,this.Size.Height);
            finalImage = new Bitmap(this.Size.Width, this.Size.Height);

            Start();

            t = new Timer();
            t.Interval = 1;
            t.Enabled = true;

            t.Tick += LTick;
            t.Tick += LTick;
            t.Tick += LTick;
        }

        PointF startPoint = new PointF(150f, 250f);        
        private void Start()
        {
            cars.Add(new LevasCar(startPoint, map));
            cars.Add(new TriggerMemoryCar(startPoint, map));            
                     
        }
        
        private void LTick(object sender, EventArgs e)
        {
            List<Car> toRemove = new List<Car>();
            string str = "Moving:  ";
            foreach (Car c in cars)
            {
                if (!c.Tick())
                {
                    toRemove.Add(c);                    
                }
                str += "Alpha: " + c.moving.alpha.ToString() + " Length: " + c.moving.length.ToString();
            }

            foreach (Car c in toRemove) cars.Remove(c);
            if (toRemove.Count > 0) Fail();

            //Text = str;
            Invalidate();
        }

        private void Restart()
        {
            List<Car> toRemove = new List<Car>(cars);

            foreach (Car c in toRemove) cars.Remove(c);
            t.Stop();
        }

        private void Fail()
        {
            //t.Stop();
            MessageBox.Show("Your car ran away ^_^\n(Restart -> press S)", "Ooops..",
                  MessageBoxButtons.OK, MessageBoxIcon.Error);            
        }

        Font startWordFont = new Font("Arial", 16);        
        private void Form1_Paint(object sender, PaintEventArgs e)
        {            
            Graphics g = e.Graphics;
            g.Clear(Color.White);

            String startWord = "START\nHERE->";
            SizeF startSIZE = g.MeasureString(startWord, startWordFont);


            g.DrawImage(map,new Rectangle(0, 0,  map.Width, map.Height));
            g.DrawString(startWord, startWordFont, Brushes.DarkRed, startPoint.X - startSIZE.Width, startPoint.Y - startSIZE.Height*0.75f);

            PointF p,p2;
            Pen sensC;
            foreach (Car c in cars)
            {
                g.FillEllipse(c.color, c.pos.X-10f, c.pos.Y-10f, 20, 20);

                p = c.pos;
                p.X += c.moving.dx * 30;
                p.Y += c.moving.dy * 30;
                p2 = c.moving.startPoint;                
                //g.DrawLine(Pens.Green, p2, p);
                
                for (int i = 0; i < c.sensors.Count; i++)
                {
                    if (c.sensorsValues[i] == 1f) g.DrawEllipse(Pens.Green, c.sensors[i].endPoint.X - 2.5f, c.sensors[i].endPoint.Y - 2.5f, 5, 5);
                    else g.FillEllipse(Brushes.DarkRed, c.sensors[i].endPoint.X - 2.5f, c.sensors[i].endPoint.Y - 2.5f, 5, 5);

                    //sensorsValues[i].ToString();
                    //else Text = c.moving.length.ToString();//c.sensorsValues[i].ToString();// c.sensors[i].length.ToString() + " " + c.sensors[i].alpha.ToString();                    
                }
                Text = Math.Round(cars[0].rotateRation,2).ToString();
                //acceleration vector
                /*p = c.pos;
                p.X += c.acc.dx * 100000 - 10;
                p.Y += c.acc.dy * 100000;
                p2 = c.acc.startPoint;
                p2.X -= 10;
                g.DrawLine(Pens.DarkBlue, p2, p);*/
            }

        }

        Point lastPos;
        private void Form1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            return;
            startDrawing = true;
            Text = "Mapping ON";

            Graphics g = Graphics.FromImage(map);
            g.Clear(Color.White);

            lastPos = e.Location;
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            return;
            startDrawing = false;
            Text = "Mapping OFF";
        }

        int brushSize = 5;
        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            return;
            if (!startDrawing) return;
            Text = "Draw";

            //Pen WideBlack = new Pen(Color.FromArgb(255, 0, 0, 0), 10);            

            Graphics g = Graphics.FromImage(map);
            //g.DrawLine(WideBlack, lastPos.X, lastPos.Y, e.X, e.Y);
            g.FillEllipse(Brushes.Black, e.X, e.Y, 10, 10);

            //lastPos = e.Location;
            Invalidate();
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 's') Start();
            else if (e.KeyChar == 'r') Restart();
        }

        private void trackBarSpeed_ValueChanged(object sender, EventArgs e)
        {
            foreach (Car c in cars) c.basicAcc = (float)(trackBarSpeed.Value / 10);
        }
    }
}
