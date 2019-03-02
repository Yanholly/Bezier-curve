using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BezierCurve
{

    public partial class Form1 : Form
    {

        bool CreateMaPoint = false;
        float dX = 0;
        float dY = 0;
        public List<MaPoint> mahs = new List<MaPoint>();
       
        public struct MaPoint
        {
            public float x, y, z;
            public bool isIClicked;

            public MaPoint(float p1, float p2, bool clicker)
            {
                x = p1;
                y = p2;
                z = 0;
                isIClicked = clicker;
            }

            public MaPoint(float p1, float p2)
            {
                x = p1;
                y = p2;
                z = 0;
                isIClicked = false;
            }

            public MaPoint(float p1, float p2, float p3)
            {
                x = p1;
                y = p2;
                z = p3;
                isIClicked = false;
            }

            public static MaPoint operator +(MaPoint a, MaPoint b)
            {
                MaPoint c = new MaPoint(a.x + b.x, a.y + b.y, a.z + b.z);
                return c;
            }
            public static MaPoint operator -(MaPoint a, MaPoint b)
            {
                MaPoint c = new MaPoint(a.x - b.x, a.y - b.y, a.z - b.z);
                return c;
            }

            public static MaPoint operator *(float s, MaPoint a)
            {
                MaPoint c = new MaPoint(a.x *s, a.y *s, a.z *s);
                return c;
            }
        };

        public Form1()
        {
            mahs.Add(new MaPoint(400, 200));
            mahs.Add(new MaPoint(400, 100));
            mahs.Add(new MaPoint(500, 100));
            mahs.Add(new MaPoint(500, 200));

            InitializeComponent();
             
        }

        MaPoint getBezierPoint(List<MaPoint> points, int numPoints, float t)
        {
            List<MaPoint> tmp = new List<MaPoint>();
            tmp = points.GetRange(0, points.Count);

            int i = numPoints - 1;
            while (i > 0)
            {
                for (int k = 0; k < i; k++)
                    tmp[k] = tmp[k] + t * (tmp[k + 1] - tmp[k]);
                i--;
            }
            return tmp[0];
        }

        private void button1_Click(object sender, EventArgs e)
        {
            mahs.Clear();
            pictureBox1.Invalidate();
        }


       

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Pen Ospen = new Pen(Color.Black, 1);
            Pen Linecpen = new Pen(Color.LightGray, 2);
            SolidBrush brush = new SolidBrush(Color.Gray);
            SolidBrush Linebrush = new SolidBrush(Color.Red);
            label1.Text = mahs.Count().ToString();
           
            e.Graphics.DrawLine(Ospen, 701, 0, 701, 801);
            e.Graphics.DrawLine(Ospen, 0, 401, 1401, 401);
   

            for (int i = 1; i < mahs.Count(); i++)
            {
                e.Graphics.DrawLine(Linecpen, mahs[i].x+700, mahs[i].y+400, mahs[i-1].x+700, mahs[i-1].y+400);
            }
            if(mahs.Count>0)
            for (float i = 0; i < 1; i += (float)0.0001)
            {
                e.Graphics.FillEllipse(Linebrush, getBezierPoint(mahs, mahs.Count(), i).x - 1+700, getBezierPoint(mahs, mahs.Count(), i).y - 1+400, 3, 3);
            }


            for (int i = 0; i < mahs.Count(); i++)
            {
                e.Graphics.FillEllipse(brush, mahs[i].x - 7+700, mahs[i].y - 7+400, 14, 14);
            }

        }

      private void button4_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.W)
            {
                for (int i = 0; i < mahs.Count(); i++)
                {
                    mahs[i] = new MaPoint(mahs[i].x, mahs[i].y * (float)Math.Cos(0.0872665) + mahs[i].z * (float)Math.Sin(-0.0872665),
                                                     -mahs[i].y * (float)Math.Sin(-0.0872665) + mahs[i].z * (float)Math.Cos(0.0872665));
                }
                pictureBox1.Invalidate();
            }
            if (e.KeyData == Keys.S)
            {
                for (int i = 0; i < mahs.Count(); i++)
                {
                    mahs[i] = new MaPoint(mahs[i].x, mahs[i].y * (float)Math.Cos(0.0872665) + mahs[i].z * (float)Math.Sin(0.0872665),
                                                     -mahs[i].y * (float)Math.Sin(0.0872665) + mahs[i].z * (float)Math.Cos(0.0872665));
                }
                pictureBox1.Invalidate();
            }
            if (e.KeyData == Keys.D)
            {
                for (int i = 0; i < mahs.Count(); i++)
                {
                    mahs[i] = new MaPoint(mahs[i].x * (float)Math.Cos(0.0872665) + mahs[i].z * (float)Math.Sin(0.0872665), mahs[i].y,
                                                     -mahs[i].x * (float)Math.Sin(0.0872665) + mahs[i].z * (float)Math.Cos(0.0872665));
                }
                pictureBox1.Invalidate();
            }
            if (e.KeyData == Keys.A)
            {
                for (int i = 0; i < mahs.Count(); i++)
                {
                    mahs[i] = new MaPoint(mahs[i].x * (float)Math.Cos(-0.0872665) + mahs[i].z * (float)Math.Sin(-0.0872665), mahs[i].y,
                                                      -mahs[i].x * (float)Math.Sin(-0.0872665) + mahs[i].z * (float)Math.Cos(-0.0872665));
                }
                pictureBox1.Invalidate();
            }
        }

  
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            bool tmp = true;
            for (int i = 0; i < mahs.Count(); i++)
            {
                if(Math.Pow(( e.X-700-mahs[i].x), 2) + Math.Pow((e.Y -400 - mahs[i].y), 2) <= Math.Pow(7, 2))
                {
                    mahs[i] = new MaPoint(mahs[i].x, mahs[i].y,true);
                    dX = e.X - mahs[i].x;
                    dY = e.Y - mahs[i].y;
                    tmp = false;
                    break;
                }
                
            }
            if (tmp == true)
                CreateMaPoint = true;

        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if(CreateMaPoint == true)
            {
                mahs.Add(new MaPoint(e.X-700, e.Y-400));
                pictureBox1.Invalidate();
                CreateMaPoint = false;
            }
            for (int i = 0; i < mahs.Count(); i++)
            {
              mahs[i] = new MaPoint(mahs[i].x, mahs[i].y, false);   
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            
            for (int i = 0; i < mahs.Count(); i++)
                {
                 if (mahs[i].isIClicked == true)
                    {
                    
                        mahs[i] = new MaPoint(e.X - dX, e.Y - dY, true);
                       
                        pictureBox1.Invalidate();
                        break;
                    }
                }
    
        }
    }
}
