using System;
using System.Drawing;
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

        private bool isMosaicPressed = false;
        private bool isFloodPressed = false;
        private bool isMouseDown = false;
        private Bitmap originalImage;
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

        public void MosaicFlood(int x, int y, Color border, Bitmap pattern)
        {
            g.DrawImage(pattern,x - pattern.Width / 2,y - pattern.Height / 2);
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
            else if (isMosaicPressed)
            {
                MosaicFlood(e.X, e.Y, pen.Color, originalImage);
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
            isMosaicPressed = !false;
        }

        private void LoadImageButton_Click(object sender, EventArgs e)
        {
            isMosaicPressed = true;
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.png;*.jpg;*.jpeg;*.bmp|All files|*.*";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    originalImage = new Bitmap(openFileDialog.FileName);
                    
                }
            }
        }

        private void clear_button_Click(object sender, EventArgs e)
        {
            g.Clear(BackColor);
            pictureBox1.Image = map;
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
