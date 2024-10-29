namespace Lab4
{
    partial class MainScreen
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            button1 = new Button();
            button2 = new Button();
            pictureBox1 = new PictureBox();
            button3 = new Button();
            textBox1 = new TextBox();
            textBox2 = new TextBox();
            button4 = new Button();
            comboBox1 = new ComboBox();
            button5 = new Button();
            textBox3 = new TextBox();
            button6 = new Button();
            button7 = new Button();
            textBox4 = new TextBox();
            textBox5 = new TextBox();
            button8 = new Button();
            button9 = new Button();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            label7 = new Label();
            label8 = new Label();
            button10 = new Button();
            button11 = new Button();
            comboBox2 = new ComboBox();
            button12 = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(820, 12);
            button1.Name = "button1";
            button1.Size = new Size(160, 48);
            button1.TabIndex = 0;
            button1.Text = "Начать создавать полигон";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(820, 66);
            button2.Name = "button2";
            button2.Size = new Size(160, 34);
            button2.TabIndex = 1;
            button2.Text = "Создать полигон";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.BorderStyle = BorderStyle.FixedSingle;
            pictureBox1.Location = new Point(12, 12);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(802, 558);
            pictureBox1.TabIndex = 2;
            pictureBox1.TabStop = false;
            pictureBox1.MouseClick += pictureBox1_MouseClick;
            // 
            // button3
            // 
            button3.Location = new Point(820, 106);
            button3.Name = "button3";
            button3.Size = new Size(160, 31);
            button3.TabIndex = 4;
            button3.Text = "Очистить поле";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(820, 215);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(75, 27);
            textBox1.TabIndex = 5;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(908, 215);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(70, 27);
            textBox2.TabIndex = 6;
            // 
            // button4
            // 
            button4.Location = new Point(822, 248);
            button4.Name = "button4";
            button4.Size = new Size(158, 48);
            button4.TabIndex = 7;
            button4.Text = "Смещение полигона на dx и dy";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(822, 163);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(160, 28);
            comboBox1.TabIndex = 8;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            // 
            // button5
            // 
            button5.Location = new Point(991, 108);
            button5.Name = "button5";
            button5.Size = new Size(156, 49);
            button5.TabIndex = 9;
            button5.Text = "Поворот вокруг точки";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
            // 
            // textBox3
            // 
            textBox3.Location = new Point(987, 37);
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(160, 27);
            textBox3.TabIndex = 10;
            // 
            // button6
            // 
            button6.Location = new Point(986, 70);
            button6.Name = "button6";
            button6.Size = new Size(160, 27);
            button6.TabIndex = 11;
            button6.Text = "Точка";
            button6.UseVisualStyleBackColor = true;
            button6.Click += button6_Click;
            // 
            // button7
            // 
            button7.Location = new Point(990, 163);
            button7.Name = "button7";
            button7.Size = new Size(157, 51);
            button7.TabIndex = 12;
            button7.Text = "Поворот вокруг центра полигона";
            button7.UseVisualStyleBackColor = true;
            button7.Click += button7_Click;
            // 
            // textBox4
            // 
            textBox4.Location = new Point(987, 281);
            textBox4.Name = "textBox4";
            textBox4.Size = new Size(72, 27);
            textBox4.TabIndex = 13;
            // 
            // textBox5
            // 
            textBox5.Location = new Point(1074, 282);
            textBox5.Name = "textBox5";
            textBox5.Size = new Size(72, 27);
            textBox5.TabIndex = 14;
            // 
            // button8
            // 
            button8.Location = new Point(988, 353);
            button8.Name = "button8";
            button8.Size = new Size(158, 63);
            button8.TabIndex = 15;
            button8.Text = "Масштабирование относитльно точки";
            button8.UseVisualStyleBackColor = true;
            button8.Click += button8_Click;
            // 
            // button9
            // 
            button9.Location = new Point(990, 422);
            button9.Name = "button9";
            button9.Size = new Size(156, 71);
            button9.TabIndex = 16;
            button9.Text = "Масштабирование относительно центра полигона";
            button9.UseVisualStyleBackColor = true;
            button9.Click += button9_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(837, 140);
            label1.Name = "label1";
            label1.Size = new Size(127, 20);
            label1.TabIndex = 17;
            label1.Text = "Выбор полигона";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(848, 194);
            label2.Name = "label2";
            label2.Size = new Size(25, 20);
            label2.TabIndex = 18;
            label2.Text = "dx";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(930, 194);
            label3.Name = "label3";
            label3.Size = new Size(25, 20);
            label3.TabIndex = 19;
            label3.Text = "dy";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(1003, 14);
            label4.Name = "label4";
            label4.Size = new Size(117, 20);
            label4.TabIndex = 20;
            label4.Text = "Угол в градусах";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            label5.Location = new Point(1014, 218);
            label5.Name = "label5";
            label5.Size = new Size(106, 20);
            label5.TabIndex = 21;
            label5.Text = "Коэффиценты";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            label6.Location = new Point(1014, 238);
            label6.Name = "label6";
            label6.Size = new Size(98, 20);
            label6.TabIndex = 22;
            label6.Text = "Расстяжения";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(1003, 258);
            label7.Name = "label7";
            label7.Size = new Size(38, 20);
            label7.TabIndex = 23;
            label7.Text = "по x";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(1091, 259);
            label8.Name = "label8";
            label8.Size = new Size(38, 20);
            label8.TabIndex = 24;
            label8.Text = "по y";
            // 
            // button10
            // 
            button10.Location = new Point(987, 320);
            button10.Name = "button10";
            button10.Size = new Size(160, 27);
            button10.TabIndex = 25;
            button10.Text = "Точка";
            button10.UseVisualStyleBackColor = true;
            button10.Click += button6_Click;
            // 
            // button11
            // 
            button11.Location = new Point(825, 320);
            button11.Name = "button11";
            button11.Size = new Size(155, 68);
            button11.TabIndex = 26;
            button11.Text = "Проверка на принадлежность полигону";
            button11.UseVisualStyleBackColor = true;
            button11.Click += button11_Click;
            // 
            // comboBox2
            // 
            comboBox2.FormattingEnabled = true;
            comboBox2.Location = new Point(825, 413);
            comboBox2.Name = "comboBox2";
            comboBox2.Size = new Size(151, 28);
            comboBox2.TabIndex = 27;
            // 
            // button12
            // 
            button12.Location = new Point(848, 464);
            button12.Name = "button12";
            button12.Size = new Size(94, 29);
            button12.TabIndex = 28;
            button12.Text = "Ребро";
            button12.UseVisualStyleBackColor = true;
            button12.Click += button12_Click;
            // 
            // MainScreen
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1159, 582);
            Controls.Add(button12);
            Controls.Add(comboBox2);
            Controls.Add(button11);
            Controls.Add(button10);
            Controls.Add(label8);
            Controls.Add(label7);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(button9);
            Controls.Add(button8);
            Controls.Add(textBox5);
            Controls.Add(textBox4);
            Controls.Add(button7);
            Controls.Add(button6);
            Controls.Add(textBox3);
            Controls.Add(button5);
            Controls.Add(comboBox1);
            Controls.Add(button4);
            Controls.Add(textBox2);
            Controls.Add(textBox1);
            Controls.Add(button3);
            Controls.Add(pictureBox1);
            Controls.Add(button2);
            Controls.Add(button1);
            Name = "MainScreen";
            Text = "Афинные преобразования";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private Button button2;
        private PictureBox pictureBox1;
        private Button button3;
        private TextBox textBox1;
        private TextBox textBox2;
        private Button button4;
        private ComboBox comboBox1;
        private Button button5;
        private TextBox textBox3;
        private Button button6;
        private Button button7;
        private TextBox textBox4;
        private TextBox textBox5;
        private Button button8;
        private Button button9;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private Button button10;
        private Button button11;
        private ComboBox comboBox2;
        private Button button12;
    }
}