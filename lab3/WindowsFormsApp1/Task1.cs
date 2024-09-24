using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Task1 : Form
    {

        public Task1()
        {
            InitializeComponent();
            set_size();
            pen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
            pen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
        }
        private bool isFloodPressed = false;
        private bool isMouseDown = false;
        Graphics g;
        Pen pen = new Pen(Color.Black, 3f);
        int prev_x = -1, prev_y = -1;
        Bitmap map;

        public void floodArea(int x, int y, Color c)
        {
            Point cur = new Point(x, y);
            if (y >= 0 && y < map.Height && map.GetPixel(x,y).ToArgb() != c.ToArgb()) 
            {
                Point l = cur;
                Point r = cur;
                if (l.X != 0)
                    while (map.GetPixel( l.X - 1,l.Y).ToArgb() != c.ToArgb()) {
                        var t = map.GetPixel(l.X - 1, l.Y);
                        l.X--;
                        if (l.X == 0) break;
                    }
                if (r.X != map.Width - 1)
                    while (map.GetPixel(r.X + 1, r.Y).ToArgb() != c.ToArgb())
                    {
                        r.X++;
                        if (r.X == map.Width - 1) break;
                    }
                g.DrawLine(new Pen(c,1f),l,r);
                for (int i = l.X; i <= r.X; i++)
                {
                    floodArea(i, y + 1, c);
                }
                for (int i = l.X; i <= r.X; i++)
                {
                    floodArea(i, y - 1, c);
                }
                
            }

        }
        private void set_size()
        {
            Rectangle rec = Screen.GetBounds(this);
            map = new Bitmap(rec.Width, rec.Height);
            g = Graphics.FromImage(map);
        }
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (isFloodPressed)
            {
                floodArea(e.X, e.Y, pen.Color);
                pictureBox1.Image = map;
            }
            else
            {
                isMouseDown = true;
                prev_x = e.X;
                prev_y = e.Y;
            }
            

        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            isMouseDown = false;
            prev_x = -1;
            prev_y = -1;
        }

        private void flood_button_Click(object sender, EventArgs e)
        {
            isFloodPressed = !isFloodPressed;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isMouseDown) return;
            g.DrawLine(pen, prev_x, prev_y, e.X, e.Y);
            prev_x = e.X;
            prev_y = e.Y;
            pictureBox1.Image = map;
        }
    }
}
