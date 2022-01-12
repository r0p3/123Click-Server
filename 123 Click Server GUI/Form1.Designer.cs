namespace _123_Click_Server_GUI
{
    partial class Form1
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
            this.lvOnline = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.rbLog = new System.Windows.Forms.RichTextBox();
            this.tbInput = new System.Windows.Forms.TextBox();
            this.btnKick = new System.Windows.Forms.Button();
            this.btnBan = new System.Windows.Forms.Button();
            this.nudVersion = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nudVersion)).BeginInit();
            this.SuspendLayout();
            // 
            // lvOnline
            // 
            this.lvOnline.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.lvOnline.FullRowSelect = true;
            this.lvOnline.GridLines = true;
            this.lvOnline.HideSelection = false;
            this.lvOnline.Location = new System.Drawing.Point(14, 13);
            this.lvOnline.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.lvOnline.MultiSelect = false;
            this.lvOnline.Name = "lvOnline";
            this.lvOnline.Size = new System.Drawing.Size(285, 490);
            this.lvOnline.TabIndex = 0;
            this.lvOnline.UseCompatibleStateImageBehavior = false;
            this.lvOnline.View = System.Windows.Forms.View.Details;
            this.lvOnline.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.lvOnline_ItemSelectionChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Name";
            this.columnHeader1.Width = 110;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "IP";
            this.columnHeader2.Width = 170;
            // 
            // rbLog
            // 
            this.rbLog.Location = new System.Drawing.Point(309, 13);
            this.rbLog.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.rbLog.Name = "rbLog";
            this.rbLog.Size = new System.Drawing.Size(764, 490);
            this.rbLog.TabIndex = 1;
            this.rbLog.Text = "";
            // 
            // tbInput
            // 
            this.tbInput.Location = new System.Drawing.Point(14, 515);
            this.tbInput.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.tbInput.Name = "tbInput";
            this.tbInput.Size = new System.Drawing.Size(598, 27);
            this.tbInput.TabIndex = 2;
            this.tbInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbInput_KeyDown);
            // 
            // btnKick
            // 
            this.btnKick.Enabled = false;
            this.btnKick.Location = new System.Drawing.Point(826, 515);
            this.btnKick.Name = "btnKick";
            this.btnKick.Size = new System.Drawing.Size(120, 27);
            this.btnKick.TabIndex = 3;
            this.btnKick.Text = "Kick";
            this.btnKick.UseVisualStyleBackColor = true;
            this.btnKick.Click += new System.EventHandler(this.btnKick_Click);
            // 
            // btnBan
            // 
            this.btnBan.Enabled = false;
            this.btnBan.Location = new System.Drawing.Point(956, 515);
            this.btnBan.Name = "btnBan";
            this.btnBan.Size = new System.Drawing.Size(120, 27);
            this.btnBan.TabIndex = 4;
            this.btnBan.Text = "Ban";
            this.btnBan.UseVisualStyleBackColor = true;
            this.btnBan.Click += new System.EventHandler(this.btnBan_Click);
            // 
            // nudVersion
            // 
            this.nudVersion.Location = new System.Drawing.Point(700, 515);
            this.nudVersion.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudVersion.Name = "nudVersion";
            this.nudVersion.Size = new System.Drawing.Size(120, 27);
            this.nudVersion.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(626, 519);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 18);
            this.label1.TabIndex = 6;
            this.label1.Text = "Version";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1087, 553);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.nudVersion);
            this.Controls.Add(this.btnBan);
            this.Controls.Add(this.btnKick);
            this.Controls.Add(this.tbInput);
            this.Controls.Add(this.rbLog);
            this.Controls.Add(this.lvOnline);
            this.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.Name = "Form1";
            this.Text = "123 Click Server";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nudVersion)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView lvOnline;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.RichTextBox rbLog;
        private System.Windows.Forms.TextBox tbInput;
        private System.Windows.Forms.Button btnKick;
        private System.Windows.Forms.Button btnBan;
        private System.Windows.Forms.NumericUpDown nudVersion;
        private System.Windows.Forms.Label label1;
    }
}

