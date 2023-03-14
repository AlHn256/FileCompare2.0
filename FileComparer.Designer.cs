namespace FileCompare2._0
{
    partial class FileComparer
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
            System.Windows.Forms.Button FirstFileBtn;
            System.Windows.Forms.Button ChangeBut;
            System.Windows.Forms.Button ComparButton;
            System.Windows.Forms.Button CopyFilesButton;
            System.Windows.Forms.Button FirstDirBtn;
            System.Windows.Forms.Button SecondDirBtn;
            this.TextBoxFile2 = new System.Windows.Forms.TextBox();
            this.TextBoxFile1 = new System.Windows.Forms.TextBox();
            this.FirstDirTextBox = new System.Windows.Forms.TextBox();
            this.RTB = new System.Windows.Forms.RichTextBox();
            this.F1Label = new System.Windows.Forms.Label();
            this.F2Label = new System.Windows.Forms.Label();
            this.MissFilesLab = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.StopButton = new System.Windows.Forms.Button();
            this.CopyDirTextBox = new System.Windows.Forms.TextBox();
            this.StartButton = new System.Windows.Forms.Button();
            this.SecondDirTextBox = new System.Windows.Forms.TextBox();
            FirstFileBtn = new System.Windows.Forms.Button();
            ChangeBut = new System.Windows.Forms.Button();
            ComparButton = new System.Windows.Forms.Button();
            CopyFilesButton = new System.Windows.Forms.Button();
            FirstDirBtn = new System.Windows.Forms.Button();
            SecondDirBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // FirstFileBtn
            // 
            FirstFileBtn.Location = new System.Drawing.Point(928, 102);
            FirstFileBtn.Name = "FirstFileBtn";
            FirstFileBtn.Size = new System.Drawing.Size(26, 23);
            FirstFileBtn.TabIndex = 12;
            FirstFileBtn.Text = "...";
            FirstFileBtn.UseVisualStyleBackColor = true;
            FirstFileBtn.Click += new System.EventHandler(this.FirstDirBtn_Click);
            // 
            // ChangeBut
            // 
            ChangeBut.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            ChangeBut.Location = new System.Drawing.Point(12, 130);
            ChangeBut.Name = "ChangeBut";
            ChangeBut.Size = new System.Drawing.Size(868, 37);
            ChangeBut.TabIndex = 19;
            ChangeBut.Text = "<>";
            ChangeBut.UseVisualStyleBackColor = true;
            ChangeBut.Click += new System.EventHandler(this.ChangeBut_Click);
            // 
            // ComparButton
            // 
            ComparButton.Location = new System.Drawing.Point(889, 137);
            ComparButton.Name = "ComparButton";
            ComparButton.Size = new System.Drawing.Size(65, 23);
            ComparButton.TabIndex = 22;
            ComparButton.Text = "Compar";
            ComparButton.UseVisualStyleBackColor = true;
            ComparButton.Click += new System.EventHandler(this.TestButton_Click);
            // 
            // CopyFilesButton
            // 
            CopyFilesButton.Location = new System.Drawing.Point(889, 198);
            CopyFilesButton.Name = "CopyFilesButton";
            CopyFilesButton.Size = new System.Drawing.Size(65, 23);
            CopyFilesButton.TabIndex = 27;
            CopyFilesButton.Text = "Copy Files";
            CopyFilesButton.UseVisualStyleBackColor = true;
            CopyFilesButton.Click += new System.EventHandler(this.CopyFilesButton_Click);
            // 
            // FirstDirBtn
            // 
            FirstDirBtn.Location = new System.Drawing.Point(889, 10);
            FirstDirBtn.Name = "FirstDirBtn";
            FirstDirBtn.Size = new System.Drawing.Size(65, 20);
            FirstDirBtn.TabIndex = 30;
            FirstDirBtn.Text = "...";
            FirstDirBtn.UseVisualStyleBackColor = true;
            FirstDirBtn.Click += new System.EventHandler(this.FirstDirBtn_Click_1);
            // 
            // SecondDirBtn
            // 
            SecondDirBtn.Location = new System.Drawing.Point(889, 65);
            SecondDirBtn.Name = "SecondDirBtn";
            SecondDirBtn.Size = new System.Drawing.Size(65, 20);
            SecondDirBtn.TabIndex = 31;
            SecondDirBtn.Text = "...";
            SecondDirBtn.UseVisualStyleBackColor = true;
            SecondDirBtn.Click += new System.EventHandler(this.SecondDirBtn_Click);
            // 
            // TextBoxFile2
            // 
            this.TextBoxFile2.Location = new System.Drawing.Point(12, 173);
            this.TextBoxFile2.Name = "TextBoxFile2";
            this.TextBoxFile2.Size = new System.Drawing.Size(868, 20);
            this.TextBoxFile2.TabIndex = 18;
            this.TextBoxFile2.TextChanged += new System.EventHandler(this.TextBoxFile2_TextChanged);
            // 
            // TextBoxFile1
            // 
            this.TextBoxFile1.Location = new System.Drawing.Point(12, 104);
            this.TextBoxFile1.Name = "TextBoxFile1";
            this.TextBoxFile1.Size = new System.Drawing.Size(868, 20);
            this.TextBoxFile1.TabIndex = 14;
            this.TextBoxFile1.TextChanged += new System.EventHandler(this.TextBoxFile1_TextChanged);
            // 
            // FirstDirTextBox
            // 
            this.FirstDirTextBox.Location = new System.Drawing.Point(12, 10);
            this.FirstDirTextBox.Name = "FirstDirTextBox";
            this.FirstDirTextBox.Size = new System.Drawing.Size(868, 20);
            this.FirstDirTextBox.TabIndex = 13;
            // 
            // RTB
            // 
            this.RTB.Location = new System.Drawing.Point(12, 228);
            this.RTB.Name = "RTB";
            this.RTB.Size = new System.Drawing.Size(1360, 388);
            this.RTB.TabIndex = 10;
            this.RTB.Text = "";
            // 
            // F1Label
            // 
            this.F1Label.AutoSize = true;
            this.F1Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.F1Label.Location = new System.Drawing.Point(886, 107);
            this.F1Label.Name = "F1Label";
            this.F1Label.Size = new System.Drawing.Size(14, 13);
            this.F1Label.TabIndex = 20;
            this.F1Label.Text = "0";
            // 
            // F2Label
            // 
            this.F2Label.AutoSize = true;
            this.F2Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.F2Label.Location = new System.Drawing.Point(886, 176);
            this.F2Label.Name = "F2Label";
            this.F2Label.Size = new System.Drawing.Size(14, 13);
            this.F2Label.TabIndex = 21;
            this.F2Label.Text = "0";
            // 
            // MissFilesLab
            // 
            this.MissFilesLab.AutoSize = true;
            this.MissFilesLab.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.MissFilesLab.Location = new System.Drawing.Point(13, 204);
            this.MissFilesLab.Name = "MissFilesLab";
            this.MissFilesLab.Size = new System.Drawing.Size(73, 13);
            this.MissFilesLab.TabIndex = 23;
            this.MissFilesLab.Text = "Miss Files 0";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 36);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(868, 23);
            this.progressBar1.TabIndex = 24;
            // 
            // StopButton
            // 
            this.StopButton.Location = new System.Drawing.Point(889, 36);
            this.StopButton.Name = "StopButton";
            this.StopButton.Size = new System.Drawing.Size(65, 23);
            this.StopButton.TabIndex = 25;
            this.StopButton.Text = "Stop";
            this.StopButton.UseVisualStyleBackColor = true;
            this.StopButton.Click += new System.EventHandler(this.StopButton_Click);
            // 
            // CopyDirTextBox
            // 
            this.CopyDirTextBox.Location = new System.Drawing.Point(125, 201);
            this.CopyDirTextBox.Name = "CopyDirTextBox";
            this.CopyDirTextBox.Size = new System.Drawing.Size(755, 20);
            this.CopyDirTextBox.TabIndex = 26;
            // 
            // StartButton
            // 
            this.StartButton.Location = new System.Drawing.Point(960, 7);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(412, 214);
            this.StartButton.TabIndex = 28;
            this.StartButton.Text = "Start";
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // SecondDirTextBox
            // 
            this.SecondDirTextBox.Location = new System.Drawing.Point(12, 65);
            this.SecondDirTextBox.Name = "SecondDirTextBox";
            this.SecondDirTextBox.Size = new System.Drawing.Size(868, 20);
            this.SecondDirTextBox.TabIndex = 29;
            // 
            // FileComparer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1384, 628);
            this.Controls.Add(SecondDirBtn);
            this.Controls.Add(FirstDirBtn);
            this.Controls.Add(this.SecondDirTextBox);
            this.Controls.Add(this.StartButton);
            this.Controls.Add(CopyFilesButton);
            this.Controls.Add(this.CopyDirTextBox);
            this.Controls.Add(this.StopButton);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.MissFilesLab);
            this.Controls.Add(ComparButton);
            this.Controls.Add(this.F2Label);
            this.Controls.Add(this.F1Label);
            this.Controls.Add(ChangeBut);
            this.Controls.Add(this.TextBoxFile2);
            this.Controls.Add(this.TextBoxFile1);
            this.Controls.Add(this.FirstDirTextBox);
            this.Controls.Add(FirstFileBtn);
            this.Controls.Add(this.RTB);
            this.Name = "FileComparer";
            this.Text = "FileComparer";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        internal System.Windows.Forms.TextBox TextBoxFile2;
        internal System.Windows.Forms.TextBox TextBoxFile1;
        internal System.Windows.Forms.TextBox FirstDirTextBox;
        internal System.Windows.Forms.RichTextBox RTB;
        private System.Windows.Forms.Label F1Label;
        private System.Windows.Forms.Label F2Label;
        private System.Windows.Forms.Label MissFilesLab;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button StopButton;
        internal System.Windows.Forms.TextBox CopyDirTextBox;
        private System.Windows.Forms.Button StartButton;
        internal System.Windows.Forms.TextBox SecondDirTextBox;
    }
}

