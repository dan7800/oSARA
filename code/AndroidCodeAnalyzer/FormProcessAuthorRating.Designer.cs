namespace AndroidCodeAnalyzer
{
    partial class FormProcessAuthorRating
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
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxCSVPath = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxDBPath = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.textBoxLog = new System.Windows.Forms.TextBox();
            this.labelStatus = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 73);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(107, 13);
            this.label3.TabIndex = 31;
            this.label3.Text = "CSV Ouput Location:";
            // 
            // textBoxCSVPath
            // 
            this.textBoxCSVPath.Location = new System.Drawing.Point(138, 69);
            this.textBoxCSVPath.Name = "textBoxCSVPath";
            this.textBoxCSVPath.Size = new System.Drawing.Size(406, 20);
            this.textBoxCSVPath.TabIndex = 30;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(119, 13);
            this.label2.TabIndex = 29;
            this.label2.Text = "Database File Location:";
            // 
            // textBoxDBPath
            // 
            this.textBoxDBPath.Location = new System.Drawing.Point(138, 43);
            this.textBoxDBPath.Name = "textBoxDBPath";
            this.textBoxDBPath.Size = new System.Drawing.Size(406, 20);
            this.textBoxDBPath.TabIndex = 28;
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Location = new System.Drawing.Point(13, 98);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(531, 33);
            this.button2.TabIndex = 27;
            this.button2.Text = "Start Process";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // textBoxLog
            // 
            this.textBoxLog.Location = new System.Drawing.Point(13, 169);
            this.textBoxLog.Multiline = true;
            this.textBoxLog.Name = "textBoxLog";
            this.textBoxLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxLog.Size = new System.Drawing.Size(534, 281);
            this.textBoxLog.TabIndex = 26;
            // 
            // labelStatus
            // 
            this.labelStatus.AutoSize = true;
            this.labelStatus.Location = new System.Drawing.Point(13, 144);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(40, 13);
            this.labelStatus.TabIndex = 25;
            this.labelStatus.Text = "Status:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(206, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(144, 17);
            this.label1.TabIndex = 24;
            this.label1.Text = "Process Author Rating";
            // 
            // FormProcessAuthorRating
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(560, 461);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBoxCSVPath);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxDBPath);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.textBoxLog);
            this.Controls.Add(this.labelStatus);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormProcessAuthorRating";
            this.Text = "Android Manifest Analyzer";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxCSVPath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxDBPath;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox textBoxLog;
        private System.Windows.Forms.Label labelStatus;
        private System.Windows.Forms.Label label1;
    }
}