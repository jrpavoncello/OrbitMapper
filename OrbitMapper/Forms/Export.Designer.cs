namespace OrbitMapper.Forms
{
    partial class Export
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
            this.OptionsGroupBox = new System.Windows.Forms.GroupBox();
            this.SaveTessChk = new System.Windows.Forms.CheckBox();
            this.SaveShapeChk = new System.Windows.Forms.CheckBox();
            this.LockChk = new System.Windows.Forms.CheckBox();
            this.WidthLabel = new System.Windows.Forms.Label();
            this.HeightBox = new System.Windows.Forms.TextBox();
            this.SaveButton = new System.Windows.Forms.Button();
            this.HeightLabel = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.WidthBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.OptionsGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // OptionsGroupBox
            // 
            this.OptionsGroupBox.Controls.Add(this.SaveTessChk);
            this.OptionsGroupBox.Controls.Add(this.SaveShapeChk);
            this.OptionsGroupBox.Controls.Add(this.LockChk);
            this.OptionsGroupBox.Controls.Add(this.WidthLabel);
            this.OptionsGroupBox.Controls.Add(this.HeightBox);
            this.OptionsGroupBox.Controls.Add(this.SaveButton);
            this.OptionsGroupBox.Controls.Add(this.HeightLabel);
            this.OptionsGroupBox.Controls.Add(this.comboBox1);
            this.OptionsGroupBox.Controls.Add(this.WidthBox);
            this.OptionsGroupBox.Controls.Add(this.label1);
            this.OptionsGroupBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.OptionsGroupBox.Location = new System.Drawing.Point(0, 0);
            this.OptionsGroupBox.Name = "OptionsGroupBox";
            this.OptionsGroupBox.Size = new System.Drawing.Size(426, 138);
            this.OptionsGroupBox.TabIndex = 1;
            this.OptionsGroupBox.TabStop = false;
            this.OptionsGroupBox.Text = "Options";
            // 
            // SaveTessChk
            // 
            this.SaveTessChk.AutoSize = true;
            this.SaveTessChk.Location = new System.Drawing.Point(7, 44);
            this.SaveTessChk.Name = "SaveTessChk";
            this.SaveTessChk.Size = new System.Drawing.Size(110, 17);
            this.SaveTessChk.TabIndex = 1;
            this.SaveTessChk.Text = "Save Tessellation";
            this.SaveTessChk.UseVisualStyleBackColor = true;
            // 
            // SaveShapeChk
            // 
            this.SaveShapeChk.AutoSize = true;
            this.SaveShapeChk.Checked = true;
            this.SaveShapeChk.CheckState = System.Windows.Forms.CheckState.Checked;
            this.SaveShapeChk.Location = new System.Drawing.Point(7, 20);
            this.SaveShapeChk.Name = "SaveShapeChk";
            this.SaveShapeChk.Size = new System.Drawing.Size(85, 17);
            this.SaveShapeChk.TabIndex = 0;
            this.SaveShapeChk.Text = "Save Shape";
            this.SaveShapeChk.UseVisualStyleBackColor = true;
            // 
            // LockChk
            // 
            this.LockChk.AutoSize = true;
            this.LockChk.Checked = true;
            this.LockChk.CheckState = System.Windows.Forms.CheckState.Checked;
            this.LockChk.Location = new System.Drawing.Point(129, 104);
            this.LockChk.Name = "LockChk";
            this.LockChk.Size = new System.Drawing.Size(78, 17);
            this.LockChk.TabIndex = 9;
            this.LockChk.Text = "Lock Ratio";
            this.LockChk.UseVisualStyleBackColor = true;
            // 
            // WidthLabel
            // 
            this.WidthLabel.AutoSize = true;
            this.WidthLabel.Location = new System.Drawing.Point(123, 16);
            this.WidthLabel.Name = "WidthLabel";
            this.WidthLabel.Size = new System.Drawing.Size(73, 13);
            this.WidthLabel.TabIndex = 5;
            this.WidthLabel.Text = "Width (pixels):";
            // 
            // HeightBox
            // 
            this.HeightBox.Location = new System.Drawing.Point(126, 78);
            this.HeightBox.Name = "HeightBox";
            this.HeightBox.Size = new System.Drawing.Size(100, 20);
            this.HeightBox.TabIndex = 8;
            this.HeightBox.Leave += new System.EventHandler(this.DetermineRatio);
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(332, 104);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(75, 27);
            this.SaveButton.TabIndex = 2;
            this.SaveButton.Text = "Save";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // HeightLabel
            // 
            this.HeightLabel.AutoSize = true;
            this.HeightLabel.Location = new System.Drawing.Point(126, 62);
            this.HeightLabel.Name = "HeightLabel";
            this.HeightLabel.Size = new System.Drawing.Size(76, 13);
            this.HeightLabel.TabIndex = 7;
            this.HeightLabel.Text = "Height (pixels):";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(239, 36);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(168, 21);
            this.comboBox1.TabIndex = 3;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // WidthBox
            // 
            this.WidthBox.Location = new System.Drawing.Point(126, 35);
            this.WidthBox.Name = "WidthBox";
            this.WidthBox.Size = new System.Drawing.Size(100, 20);
            this.WidthBox.TabIndex = 6;
            this.WidthBox.Leave += new System.EventHandler(this.DetermineRatio);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(239, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Choose Shape";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(3, 16);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(420, 351);
            this.pictureBox1.TabIndex = 10;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.pictureBox1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 138);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(426, 370);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Preview";
            // 
            // Export
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(426, 508);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.OptionsGroupBox);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(442, 546);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "Export";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Export As";
            this.OptionsGroupBox.ResumeLayout(false);
            this.OptionsGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox OptionsGroupBox;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.CheckBox SaveTessChk;
        private System.Windows.Forms.CheckBox SaveShapeChk;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label WidthLabel;
        private System.Windows.Forms.TextBox WidthBox;
        private System.Windows.Forms.Label HeightLabel;
        private System.Windows.Forms.TextBox HeightBox;
        private System.Windows.Forms.CheckBox LockChk;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.GroupBox groupBox1;

    }
}