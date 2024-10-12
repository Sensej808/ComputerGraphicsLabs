using System.Windows.Forms;

namespace WindowsFormsApp1
{
    partial class Task1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.colorDialog = new System.Windows.Forms.ColorDialog();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.clear_button = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.mosaic = new System.Windows.Forms.RadioButton();
            this.Flood = new System.Windows.Forms.RadioButton();
            this.Brush = new System.Windows.Forms.RadioButton();
            this.button4 = new System.Windows.Forms.Button();
            this.fairyStick = new System.Windows.Forms.RadioButton();
            this.color1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point(-1, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(662, 450);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
            this.pictureBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseUp);
            // 
            // clear_button
            // 
            this.clear_button.Location = new System.Drawing.Point(121, 396);
            this.clear_button.Name = "clear_button";
            this.clear_button.Size = new System.Drawing.Size(75, 23);
            this.clear_button.TabIndex = 1;
            this.clear_button.Text = "clear";
            this.clear_button.UseVisualStyleBackColor = true;
            this.clear_button.Click += new System.EventHandler(this.clear_button_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.color1);
            this.panel1.Controls.Add(this.fairyStick);
            this.panel1.Controls.Add(this.mosaic);
            this.panel1.Controls.Add(this.Flood);
            this.panel1.Controls.Add(this.Brush);
            this.panel1.Controls.Add(this.button4);
            this.panel1.Controls.Add(this.clear_button);
            this.panel1.Location = new System.Drawing.Point(675, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 434);
            this.panel1.TabIndex = 2;
            // 
            // mosaic
            // 
            this.mosaic.AutoSize = true;
            this.mosaic.Location = new System.Drawing.Point(29, 239);
            this.mosaic.Name = "mosaic";
            this.mosaic.Size = new System.Drawing.Size(107, 20);
            this.mosaic.TabIndex = 7;
            this.mosaic.TabStop = true;
            this.mosaic.Text = "MosaicFlood";
            this.mosaic.UseVisualStyleBackColor = true;
            this.mosaic.CheckedChanged += new System.EventHandler(this.mosaic_CheckedChanged);
            // 
            // Flood
            // 
            this.Flood.AutoSize = true;
            this.Flood.Location = new System.Drawing.Point(29, 213);
            this.Flood.Name = "Flood";
            this.Flood.Size = new System.Drawing.Size(63, 20);
            this.Flood.TabIndex = 6;
            this.Flood.TabStop = true;
            this.Flood.Text = "Flood";
            this.Flood.UseVisualStyleBackColor = true;
            this.Flood.CheckedChanged += new System.EventHandler(this.Flood_CheckedChanged);
            // 
            // Brush
            // 
            this.Brush.AutoSize = true;
            this.Brush.Checked = true;
            this.Brush.Location = new System.Drawing.Point(29, 187);
            this.Brush.Name = "Brush";
            this.Brush.Size = new System.Drawing.Size(62, 20);
            this.Brush.TabIndex = 5;
            this.Brush.TabStop = true;
            this.Brush.Text = "Brush";
            this.Brush.UseVisualStyleBackColor = true;
            this.Brush.CheckedChanged += new System.EventHandler(this.Brush_CheckedChanged);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(157, 239);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(22, 23);
            this.button4.TabIndex = 4;
            this.button4.Text = "П";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.LoadImageButton_Click);
            // 
            // fairyStick
            // 
            this.fairyStick.AutoSize = true;
            this.fairyStick.Location = new System.Drawing.Point(29, 265);
            this.fairyStick.Name = "fairyStick";
            this.fairyStick.Size = new System.Drawing.Size(90, 20);
            this.fairyStick.TabIndex = 7;
            this.fairyStick.TabStop = true;
            this.fairyStick.Text = "Fairy Stick";
            this.fairyStick.UseVisualStyleBackColor = true;
            this.fairyStick.CheckedChanged += new System.EventHandler(this.fairy_CheckedChanged);
            // 
            // color1
            // 
            this.color1.Location = new System.Drawing.Point(9, 24);
            this.color1.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.color1.Name = "color1";
            this.color1.Size = new System.Drawing.Size(170, 50);
            this.color1.TabIndex = 8;
            this.color1.UseVisualStyleBackColor = true;
            this.color1.Click += new System.EventHandler(this.color1_Click);
            // 
            // Task1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(887, 450);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pictureBox1);
            this.Name = "Task1";
            this.Text = "Task1";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ColorDialog colorDialog;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button clear_button;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.RadioButton mosaic;
        private System.Windows.Forms.RadioButton Flood;
        private System.Windows.Forms.RadioButton Brush;
        private System.Windows.Forms.RadioButton fairyStick;
        private System.Windows.Forms.Button color1;
    }
}