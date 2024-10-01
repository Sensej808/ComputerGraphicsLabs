using System;
using System.Drawing;
using System.Runtime.ConstrainedExecution;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Task1 : Form
    {
        enum Tool { Brush, Flood, Mosaic, FairyStick};

        public Task1()
        {
            InitializeComponent();
            set_size();
            pen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
            pen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
            cur_tool = Tool.Brush;
        }

        int center_x, center_y;
        bool[,] mask;
        Tool cur_tool;
        bool isMouseDown;
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

        public void GetMask(int x, int y, Color border)
        {
            Point cur = new Point(x, y); 
            if (y >= 0 && y < map.Height && map.GetPixel(x, y).ToArgb() != border.ToArgb() && !mask[x, y])
            {
                Point l = cur;
                Point r = cur;
                if (l.X != 0)
                    while (map.GetPixel(l.X - 1, l.Y).ToArgb() != border.ToArgb())
                    {
                        l.X--;
                        if (l.X == 0) break;
                    }
                if (r.X != map.Width - 1)
                    while (map.GetPixel(r.X + 1, r.Y).ToArgb() != border.ToArgb())
                    {
                        r.X++;
                        if (r.X == map.Width - 1) break;
                    }
                for(int i = l.X; i <= r.X; i++)
                {
                    mask[i, y] = true;
                }
                for (int i = l.X; i <= r.X; i++)
                {
                    GetMask(i, y + 1, border);
                }
                for (int i = l.X; i <= r.X; i++)
                {
                    GetMask(i, y - 1, border);
                }

            }
        }

        public void MosaicFlood(int x, int y, Color border, Bitmap pattern)
        {
            mask = new bool[map.Width, map.Height];
            GetMask(x,y, border);
            for (int i = 0; i < mask.GetLength(0); i++)
            {
                for (int j = 0; j < mask.GetLength(1); j++)
                {
                    if (mask[i, j])
                    {
                        map.SetPixel(i, j, pattern.GetPixel((i - x % pattern.Width)%pattern.Width, (j - y % pattern.Height)%pattern.Height));
                    }
                }
            }
        }
        private void set_size()
        {
            Size rec = pictureBox1.Size;
            map = new Bitmap(rec.Width, rec.Height);
            mask = new bool[map.Width, map.Height];
            g = Graphics.FromImage(map);
        }
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (cur_tool == Tool.Flood)
            {
                floodArea(e.X, e.Y, pen.Color);
                pictureBox1.Image = map;
            }
            else if (cur_tool == Tool.Mosaic)
            {
                center_x = e.X;
                center_y = e.Y;
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


        private void LoadImageButton_Click(object sender, EventArgs e)
        {
            cur_tool = Tool.Mosaic;
            mosaic.Checked = true;
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

        private void Brush_CheckedChanged(object sender, EventArgs e)
        {
            if (Brush.Checked)
            {
                cur_tool = Tool.Brush;
            }
        }

        private void Flood_CheckedChanged(object sender, EventArgs e)
        {
            if (Flood.Checked)
            {
                cur_tool = Tool.Flood;
            }
        }

        private void mosaic_CheckedChanged(object sender, EventArgs e)
        {
            if (mosaic.Checked)
            {
                cur_tool = Tool.Mosaic;
            }
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
