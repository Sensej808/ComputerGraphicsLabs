namespace _3D_Labs
{
    partial class FormLab6
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.Up = new System.Windows.Forms.Button();
            this.Right = new System.Windows.Forms.Button();
            this.Down = new System.Windows.Forms.Button();
            this.Left = new System.Windows.Forms.Button();
            this.Forward = new System.Windows.Forms.Button();
            this.Backward = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.DxText = new System.Windows.Forms.TextBox();
            this.DyText = new System.Windows.Forms.TextBox();
            this.DzText = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.aText = new System.Windows.Forms.TextBox();
            this.bText = new System.Windows.Forms.TextBox();
            this.cText = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.XY = new System.Windows.Forms.Button();
            this.XZ = new System.Windows.Forms.Button();
            this.YZ = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.Axis0X = new System.Windows.Forms.Button();
            this.Axis0Y = new System.Windows.Forms.Button();
            this.Axis0Z = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.TextFi = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.x1Text = new System.Windows.Forms.TextBox();
            this.y1Text = new System.Windows.Forms.TextBox();
            this.z1Text = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.x2Text = new System.Windows.Forms.TextBox();
            this.y2Text = new System.Windows.Forms.TextBox();
            this.z2Text = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point(9, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(873, 625);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // Up
            // 
            this.Up.Location = new System.Drawing.Point(898, 98);
            this.Up.Name = "Up";
            this.Up.Size = new System.Drawing.Size(149, 27);
            this.Up.TabIndex = 1;
            this.Up.Text = "Вверх";
            this.Up.UseVisualStyleBackColor = true;
            this.Up.MouseDown += new System.Windows.Forms.MouseEventHandler(this.CameraMove);
            this.Up.MouseUp += new System.Windows.Forms.MouseEventHandler(this.CameraMoveStop);
            // 
            // Right
            // 
            this.Right.Location = new System.Drawing.Point(898, 131);
            this.Right.Name = "Right";
            this.Right.Size = new System.Drawing.Size(149, 27);
            this.Right.TabIndex = 4;
            this.Right.Text = "Вправо";
            this.Right.UseVisualStyleBackColor = true;
            this.Right.MouseDown += new System.Windows.Forms.MouseEventHandler(this.CameraMove);
            this.Right.MouseUp += new System.Windows.Forms.MouseEventHandler(this.CameraMoveStop);
            // 
            // Down
            // 
            this.Down.Location = new System.Drawing.Point(1053, 97);
            this.Down.Name = "Down";
            this.Down.Size = new System.Drawing.Size(151, 27);
            this.Down.TabIndex = 5;
            this.Down.Text = "Вниз";
            this.Down.UseVisualStyleBackColor = true;
            this.Down.MouseDown += new System.Windows.Forms.MouseEventHandler(this.CameraMove);
            this.Down.MouseUp += new System.Windows.Forms.MouseEventHandler(this.CameraMoveStop);
            // 
            // Left
            // 
            this.Left.Location = new System.Drawing.Point(1050, 130);
            this.Left.Name = "Left";
            this.Left.Size = new System.Drawing.Size(151, 27);
            this.Left.TabIndex = 6;
            this.Left.Text = "Влево";
            this.Left.UseVisualStyleBackColor = true;
            this.Left.MouseDown += new System.Windows.Forms.MouseEventHandler(this.CameraMove);
            this.Left.MouseUp += new System.Windows.Forms.MouseEventHandler(this.CameraMoveStop);
            // 
            // Forward
            // 
            this.Forward.Location = new System.Drawing.Point(898, 163);
            this.Forward.Name = "Forward";
            this.Forward.Size = new System.Drawing.Size(149, 28);
            this.Forward.TabIndex = 7;
            this.Forward.Text = "Вперёд";
            this.Forward.UseVisualStyleBackColor = true;
            this.Forward.MouseDown += new System.Windows.Forms.MouseEventHandler(this.CameraMove);
            this.Forward.MouseUp += new System.Windows.Forms.MouseEventHandler(this.CameraMoveStop);
            // 
            // Backward
            // 
            this.Backward.Location = new System.Drawing.Point(1050, 163);
            this.Backward.Name = "Backward";
            this.Backward.Size = new System.Drawing.Size(149, 28);
            this.Backward.TabIndex = 8;
            this.Backward.Text = "Назад";
            this.Backward.UseVisualStyleBackColor = true;
            this.Backward.MouseDown += new System.Windows.Forms.MouseEventHandler(this.CameraMove);
            this.Backward.MouseUp += new System.Windows.Forms.MouseEventHandler(this.CameraMoveStop);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(979, 64);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(125, 27);
            this.textBox1.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(897, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 20);
            this.label2.TabIndex = 10;
            this.label2.Text = "Скорость:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(901, 194);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(130, 20);
            this.label3.TabIndex = 11;
            this.label3.Text = "Список Объектов";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(901, 217);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(149, 28);
            this.comboBox1.TabIndex = 12;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(913, 248);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(25, 20);
            this.label4.TabIndex = 13;
            this.label4.Text = "dx";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(965, 248);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(25, 20);
            this.label5.TabIndex = 14;
            this.label5.Text = "dy";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(1021, 248);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(25, 20);
            this.label6.TabIndex = 15;
            this.label6.Text = "dz";
            // 
            // DxText
            // 
            this.DxText.Location = new System.Drawing.Point(898, 271);
            this.DxText.Name = "DxText";
            this.DxText.Size = new System.Drawing.Size(51, 27);
            this.DxText.TabIndex = 16;
            // 
            // DyText
            // 
            this.DyText.Location = new System.Drawing.Point(954, 271);
            this.DyText.Name = "DyText";
            this.DyText.Size = new System.Drawing.Size(45, 27);
            this.DyText.TabIndex = 17;
            this.DyText.TextChanged += new System.EventHandler(this.DyText_TextChanged);
            // 
            // DzText
            // 
            this.DzText.Location = new System.Drawing.Point(1005, 271);
            this.DzText.Name = "DzText";
            this.DzText.Size = new System.Drawing.Size(45, 27);
            this.DzText.TabIndex = 18;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(1056, 269);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(95, 29);
            this.button1.TabIndex = 19;
            this.button1.Text = "Смещение";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.ObjectMove);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(912, 301);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(17, 20);
            this.label7.TabIndex = 20;
            this.label7.Text = "a";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(955, 301);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(18, 20);
            this.label8.TabIndex = 21;
            this.label8.Text = "b";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(993, 301);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(16, 20);
            this.label9.TabIndex = 22;
            this.label9.Text = "c";
            // 
            // aText
            // 
            this.aText.Location = new System.Drawing.Point(898, 324);
            this.aText.Name = "aText";
            this.aText.Size = new System.Drawing.Size(40, 27);
            this.aText.TabIndex = 23;
            // 
            // bText
            // 
            this.bText.Location = new System.Drawing.Point(944, 324);
            this.bText.Name = "bText";
            this.bText.Size = new System.Drawing.Size(36, 27);
            this.bText.TabIndex = 24;
            // 
            // cText
            // 
            this.cText.Location = new System.Drawing.Point(986, 324);
            this.cText.Name = "cText";
            this.cText.Size = new System.Drawing.Size(36, 27);
            this.cText.TabIndex = 25;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(890, 357);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(153, 68);
            this.button2.TabIndex = 26;
            this.button2.Text = "Масштабирование относительно центра объекта";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.ScallingRelativeObjectCenter);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(890, 435);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(88, 20);
            this.label10.TabIndex = 27;
            this.label10.Text = "Отражение";
            // 
            // XY
            // 
            this.XY.Location = new System.Drawing.Point(979, 431);
            this.XY.Name = "XY";
            this.XY.Size = new System.Drawing.Size(42, 29);
            this.XY.TabIndex = 28;
            this.XY.Text = "XY";
            this.XY.UseVisualStyleBackColor = true;
            this.XY.Click += new System.EventHandler(this.ReflectionCoordinatePlane);
            // 
            // XZ
            // 
            this.XZ.Location = new System.Drawing.Point(1027, 431);
            this.XZ.Name = "XZ";
            this.XZ.Size = new System.Drawing.Size(42, 29);
            this.XZ.TabIndex = 29;
            this.XZ.Text = "XZ";
            this.XZ.UseVisualStyleBackColor = true;
            this.XZ.Click += new System.EventHandler(this.ReflectionCoordinatePlane);
            // 
            // YZ
            // 
            this.YZ.Location = new System.Drawing.Point(1075, 431);
            this.YZ.Name = "YZ";
            this.YZ.Size = new System.Drawing.Size(42, 29);
            this.YZ.TabIndex = 30;
            this.YZ.Text = "YZ";
            this.YZ.UseVisualStyleBackColor = true;
            this.YZ.Click += new System.EventHandler(this.ReflectionCoordinatePlane);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(888, 492);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(313, 20);
            this.label11.TabIndex = 31;
            this.label11.Text = "Вращение вокруг своей оси параллельной:";
            // 
            // Axis0X
            // 
            this.Axis0X.Location = new System.Drawing.Point(912, 516);
            this.Axis0X.Name = "Axis0X";
            this.Axis0X.Size = new System.Drawing.Size(94, 29);
            this.Axis0X.TabIndex = 32;
            this.Axis0X.Text = "0X";
            this.Axis0X.UseVisualStyleBackColor = true;
            this.Axis0X.Click += new System.EventHandler(this.RotationAroundLineParallelAxisPassingObjectCenter);
            // 
            // Axis0Y
            // 
            this.Axis0Y.Location = new System.Drawing.Point(1007, 515);
            this.Axis0Y.Name = "Axis0Y";
            this.Axis0Y.Size = new System.Drawing.Size(94, 29);
            this.Axis0Y.TabIndex = 33;
            this.Axis0Y.Text = "0Y";
            this.Axis0Y.UseVisualStyleBackColor = true;
            this.Axis0Y.Click += new System.EventHandler(this.RotationAroundLineParallelAxisPassingObjectCenter);
            // 
            // Axis0Z
            // 
            this.Axis0Z.Location = new System.Drawing.Point(1107, 516);
            this.Axis0Z.Name = "Axis0Z";
            this.Axis0Z.Size = new System.Drawing.Size(94, 29);
            this.Axis0Z.TabIndex = 34;
            this.Axis0Z.Text = "0Z";
            this.Axis0Z.UseVisualStyleBackColor = true;
            this.Axis0Z.Click += new System.EventHandler(this.RotationAroundLineParallelAxisPassingObjectCenter);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(890, 463);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(119, 20);
            this.label12.TabIndex = 35;
            this.label12.Text = "Угол вращения:";
            // 
            // TextFi
            // 
            this.TextFi.Location = new System.Drawing.Point(1021, 463);
            this.TextFi.Name = "TextFi";
            this.TextFi.Size = new System.Drawing.Size(125, 27);
            this.TextFi.TabIndex = 36;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(897, 567);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(24, 20);
            this.label14.TabIndex = 38;
            this.label14.Text = "x1";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(896, 587);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(24, 20);
            this.label15.TabIndex = 39;
            this.label15.Text = "y1";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(893, 617);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(24, 20);
            this.label16.TabIndex = 40;
            this.label16.Text = "z1";
            // 
            // x1Text
            // 
            this.x1Text.Location = new System.Drawing.Point(920, 551);
            this.x1Text.Name = "x1Text";
            this.x1Text.Size = new System.Drawing.Size(59, 27);
            this.x1Text.TabIndex = 41;
            // 
            // y1Text
            // 
            this.y1Text.Location = new System.Drawing.Point(920, 584);
            this.y1Text.Name = "y1Text";
            this.y1Text.Size = new System.Drawing.Size(59, 27);
            this.y1Text.TabIndex = 42;
            // 
            // z1Text
            // 
            this.z1Text.Location = new System.Drawing.Point(920, 617);
            this.z1Text.Name = "z1Text";
            this.z1Text.Size = new System.Drawing.Size(59, 27);
            this.z1Text.TabIndex = 43;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(986, 561);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(24, 20);
            this.label17.TabIndex = 44;
            this.label17.Text = "x2";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(985, 591);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(24, 20);
            this.label18.TabIndex = 45;
            this.label18.Text = "y2";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(985, 617);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(24, 20);
            this.label19.TabIndex = 46;
            this.label19.Text = "z2";
            // 
            // x2Text
            // 
            this.x2Text.Location = new System.Drawing.Point(1010, 551);
            this.x2Text.Name = "x2Text";
            this.x2Text.Size = new System.Drawing.Size(59, 27);
            this.x2Text.TabIndex = 47;
            // 
            // y2Text
            // 
            this.y2Text.Location = new System.Drawing.Point(1010, 584);
            this.y2Text.Name = "y2Text";
            this.y2Text.Size = new System.Drawing.Size(59, 27);
            this.y2Text.TabIndex = 48;
            // 
            // z2Text
            // 
            this.z2Text.Location = new System.Drawing.Point(1010, 617);
            this.z2Text.Name = "z2Text";
            this.z2Text.Size = new System.Drawing.Size(59, 27);
            this.z2Text.TabIndex = 49;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(1075, 550);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(126, 94);
            this.button3.TabIndex = 50;
            this.button3.Text = "Поворот вокруг произвольной прямой:";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.RotationAroundStraight);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(896, 29);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(138, 29);
            this.button4.TabIndex = 51;
            this.button4.Text = "Перспективная";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(1040, 29);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(160, 29);
            this.button5.TabIndex = 52;
            this.button5.Text = "Аксонометрическая";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(901, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 20);
            this.label1.TabIndex = 53;
            this.label1.Text = "Камера:";
            // 
            // FormLab6
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1215, 647);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.z2Text);
            this.Controls.Add(this.y2Text);
            this.Controls.Add(this.x2Text);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.z1Text);
            this.Controls.Add(this.y1Text);
            this.Controls.Add(this.x1Text);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.TextFi);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.Axis0Z);
            this.Controls.Add(this.Axis0Y);
            this.Controls.Add(this.Axis0X);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.YZ);
            this.Controls.Add(this.XZ);
            this.Controls.Add(this.XY);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.cText);
            this.Controls.Add(this.bText);
            this.Controls.Add(this.aText);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.DzText);
            this.Controls.Add(this.DyText);
            this.Controls.Add(this.DxText);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.Backward);
            this.Controls.Add(this.Forward);
            this.Controls.Add(this.Left);
            this.Controls.Add(this.Down);
            this.Controls.Add(this.Right);
            this.Controls.Add(this.Up);
            this.Controls.Add(this.pictureBox1);
            this.Name = "FormLab6";
            this.Text = "Lab6";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private PictureBox pictureBox1;
        private Button Up;
        private Button Right;
        private Button Down;
        private Button Left;
        private Button Forward;
        private Button Backward;
        private TextBox textBox1;
        private Label label2;
        private Label label3;
        private ComboBox comboBox1;
        private Label label4;
        private Label label5;
        private Label label6;
        private TextBox DxText;
        private TextBox DyText;
        private TextBox DzText;
        private Button button1;
        private Label label7;
        private Label label8;
        private Label label9;
        private TextBox aText;
        private TextBox bText;
        private TextBox cText;
        private Button button2;
        private Label label10;
        private Button XY;
        private Button XZ;
        private Button YZ;
        private Label label11;
        private Button Axis0X;
        private Button Axis0Y;
        private Button Axis0Z;
        private Label label12;
        private TextBox TextFi;
        private Label label14;
        private Label label15;
        private Label label16;
        private TextBox x1Text;
        private TextBox y1Text;
        private TextBox z1Text;
        private Label label17;
        private Label label18;
        private Label label19;
        private TextBox x2Text;
        private TextBox y2Text;
        private TextBox z2Text;
        private Button button3;
        private Button button4;
        private Button button5;
        private Label label1;
    }
}