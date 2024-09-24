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
        private bool isMouseDown = false;
        Graphics g;
        Pen pen = new Pen(Color.Black, 3f);
        int prev_x = -1, prev_y = -1;
        Bitmap map;
        private void set_size()
        {
            Rectangle rec = Screen.GetBounds(this);
            map = new Bitmap(rec.Width, rec.Height);
            g = Graphics.FromImage(map);
        }
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            isMouseDown = true;
            prev_x = e.X;
            prev_y = e.Y;
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            isMouseDown = false;
            prev_x = -1;
            prev_y = -1;
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
