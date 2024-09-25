namespace WindowsFormsApp1
{
    partial class Task3
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
            this.canvas = new System.Windows.Forms.PictureBox();
            this.color1 = new System.Windows.Forms.Button();
            this.color2 = new System.Windows.Forms.Button();
            this.color3 = new System.Windows.Forms.Button();
            this.Clear = new System.Windows.Forms.Button();
            this.colorDialog = new System.Windows.Forms.ColorDialog();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.canvas)).BeginInit();
            this.SuspendLayout();
            // 
            // canvas
            // 
            this.canvas.BackColor = System.Drawing.Color.White;
            this.canvas.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.canvas.Location = new System.Drawing.Point(11, 10);
            this.canvas.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.canvas.Name = "canvas";
            this.canvas.Size = new System.Drawing.Size(523, 491);
            this.canvas.TabIndex = 0;
            this.canvas.TabStop = false;
            this.canvas.MouseClick += new System.Windows.Forms.MouseEventHandler(this.canvas_MouseClick);
            // 
            // color1
            // 
            this.color1.Location = new System.Drawing.Point(667, 12);
            this.color1.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.color1.Name = "color1";
            this.color1.Size = new System.Drawing.Size(170, 50);
            this.color1.TabIndex = 1;
            this.color1.UseVisualStyleBackColor = true;
            this.color1.Click += new System.EventHandler(this.color_Click);
            // 
            // color2
            // 
            this.color2.Location = new System.Drawing.Point(667, 62);
            this.color2.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.color2.Name = "color2";
            this.color2.Size = new System.Drawing.Size(170, 50);
            this.color2.TabIndex = 2;
            this.color2.UseVisualStyleBackColor = true;
            this.color2.Click += new System.EventHandler(this.color_Click);
            // 
            // color3
            // 
            this.color3.Location = new System.Drawing.Point(667, 112);
            this.color3.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.color3.Name = "color3";
            this.color3.Size = new System.Drawing.Size(170, 50);
            this.color3.TabIndex = 3;
            this.color3.UseVisualStyleBackColor = true;
            this.color3.Click += new System.EventHandler(this.color_Click);
            // 
            // Clear
            // 
            this.Clear.Location = new System.Drawing.Point(667, 452);
            this.Clear.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.Clear.Name = "Clear";
            this.Clear.Size = new System.Drawing.Size(170, 49);
            this.Clear.TabIndex = 4;
            this.Clear.Text = "Отчистить";
            this.Clear.UseVisualStyleBackColor = true;
            this.Clear.Click += new System.EventHandler(this.Clear_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(562, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(97, 16);
            this.label2.TabIndex = 6;
            this.label2.Text = "Первая точка";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(565, 79);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 16);
            this.label1.TabIndex = 7;
            this.label1.Text = "Вторая точка";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(566, 129);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(94, 16);
            this.label3.TabIndex = 8;
            this.label3.Text = "Третья точка";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(562, 313);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(44, 16);
            this.label4.TabIndex = 9;
            this.label4.Text = "label4";
            // 
            // Task3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(851, 514);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Clear);
            this.Controls.Add(this.color3);
            this.Controls.Add(this.color2);
            this.Controls.Add(this.color1);
            this.Controls.Add(this.canvas);
            this.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.Name = "Task3";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Градиентное окрашивание треугольника";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.GradientTraing_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.canvas)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.ColorDialog colorDialog;

        private System.Windows.Forms.PictureBox canvas;
        private System.Windows.Forms.Button color1;
        private System.Windows.Forms.Button color2;
        private System.Windows.Forms.Button color3;
        private System.Windows.Forms.Button Clear;

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
    }
}