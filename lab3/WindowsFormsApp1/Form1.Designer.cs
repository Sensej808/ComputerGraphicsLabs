namespace WindowsFormsApp1
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.задание1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.задание2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.задание3ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.частьАToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.частьБToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.частьВToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.задание1ToolStripMenuItem,
            this.задание2ToolStripMenuItem,
            this.задание3ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 28);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // задание1ToolStripMenuItem
            // 
            this.задание1ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.частьАToolStripMenuItem,
            this.частьБToolStripMenuItem,
            this.частьВToolStripMenuItem});
            this.задание1ToolStripMenuItem.Name = "задание1ToolStripMenuItem";
            this.задание1ToolStripMenuItem.Size = new System.Drawing.Size(93, 24);
            this.задание1ToolStripMenuItem.Text = "Задание 1";
            // 
            // задание2ToolStripMenuItem
            // 
            this.задание2ToolStripMenuItem.Name = "задание2ToolStripMenuItem";
            this.задание2ToolStripMenuItem.Size = new System.Drawing.Size(93, 24);
            this.задание2ToolStripMenuItem.Text = "Задание 2";
            // 
            // задание3ToolStripMenuItem
            // 
            this.задание3ToolStripMenuItem.Name = "задание3ToolStripMenuItem";
            this.задание3ToolStripMenuItem.Size = new System.Drawing.Size(93, 24);
            this.задание3ToolStripMenuItem.Text = "Задание 3";
            // 
            // частьАToolStripMenuItem
            // 
            this.частьАToolStripMenuItem.Name = "частьАToolStripMenuItem";
            this.частьАToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.частьАToolStripMenuItem.Text = "часть а";
            this.частьАToolStripMenuItem.Click += new System.EventHandler(this.частьАToolStripMenuItem_Click);
            // 
            // частьБToolStripMenuItem
            // 
            this.частьБToolStripMenuItem.Name = "частьБToolStripMenuItem";
            this.частьБToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.частьБToolStripMenuItem.Text = "часть б";
            // 
            // частьВToolStripMenuItem
            // 
            this.частьВToolStripMenuItem.Name = "частьВToolStripMenuItem";
            this.частьВToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.частьВToolStripMenuItem.Text = "Часть в";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Form1";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem задание1ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem задание2ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem задание3ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem частьАToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem частьБToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem частьВToolStripMenuItem;
    }
}

