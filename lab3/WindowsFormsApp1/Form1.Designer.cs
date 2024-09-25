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
            this.Part1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Part1aToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Part1bToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Part1cToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.задание2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.задание3ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Part1ToolStripMenuItem,
            this.задание2ToolStripMenuItem,
            this.задание3ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(968, 28);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.menuStrip1_ItemClicked);
            // 
            // Part1ToolStripMenuItem
            // 
            this.Part1ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Part1aToolStripMenuItem,
            this.Part1bToolStripMenuItem,
            this.Part1cToolStripMenuItem});
            this.Part1ToolStripMenuItem.Name = "Part1ToolStripMenuItem";
            this.Part1ToolStripMenuItem.Size = new System.Drawing.Size(93, 24);
            this.Part1ToolStripMenuItem.Text = "Задание 1";
            // 
            // Part1aToolStripMenuItem
            // 
            this.Part1aToolStripMenuItem.Name = "Part1aToolStripMenuItem";
            this.Part1aToolStripMenuItem.Size = new System.Drawing.Size(144, 26);
            this.Part1aToolStripMenuItem.Text = "Часть а";
            this.Part1aToolStripMenuItem.Click += new System.EventHandler(this.Part1aToolStripMenuItem_Click);
            // 
            // Part1bToolStripMenuItem
            // 
            this.Part1bToolStripMenuItem.Name = "Part1bToolStripMenuItem";
            this.Part1bToolStripMenuItem.Size = new System.Drawing.Size(144, 26);
            this.Part1bToolStripMenuItem.Text = "Часть б";
            this.Part1bToolStripMenuItem.Click += new System.EventHandler(this.Part1bToolStripMenuItem_Click);
            // 
            // Part1cToolStripMenuItem
            // 
            this.Part1cToolStripMenuItem.Name = "Part1cToolStripMenuItem";
            this.Part1cToolStripMenuItem.Size = new System.Drawing.Size(144, 26);
            this.Part1cToolStripMenuItem.Text = "Часть в";
            this.Part1cToolStripMenuItem.Click += new System.EventHandler(this.Part1cToolStripMenuItem_Click);
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
            this.задание3ToolStripMenuItem.Click += new System.EventHandler(this.Part3ToolStripMenyItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(968, 465);
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
        private System.Windows.Forms.ToolStripMenuItem задание2ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem задание3ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem Part1ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem Part1aToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem Part1bToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem Part1cToolStripMenuItem;
    }
}

