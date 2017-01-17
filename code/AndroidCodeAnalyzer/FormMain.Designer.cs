namespace AndroidCodeAnalyzer
{
    partial class formMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(formMain));
            this.label1 = new System.Windows.Forms.Label();
            this.buttonDownloadFdroid = new System.Windows.Forms.Button();
            this.buttonDownloadRepos = new System.Windows.Forms.Button();
            this.buttonCommitHistory = new System.Windows.Forms.Button();
            this.buttonManifestHistory = new System.Windows.Forms.Button();
            this.buttonProcessAuthor = new System.Windows.Forms.Button();
            this.buttonProcessPermissions = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Select Action:";
            // 
            // buttonDownloadFdroid
            // 
            this.buttonDownloadFdroid.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.buttonDownloadFdroid.Location = new System.Drawing.Point(12, 42);
            this.buttonDownloadFdroid.Name = "buttonDownloadFdroid";
            this.buttonDownloadFdroid.Size = new System.Drawing.Size(339, 39);
            this.buttonDownloadFdroid.TabIndex = 1;
            this.buttonDownloadFdroid.Text = "Download F-Droid Repository";
            this.buttonDownloadFdroid.UseVisualStyleBackColor = true;
            this.buttonDownloadFdroid.Click += new System.EventHandler(this.buttonDownloadFdroid_Click);
            // 
            // buttonDownloadRepos
            // 
            this.buttonDownloadRepos.Location = new System.Drawing.Point(12, 97);
            this.buttonDownloadRepos.Name = "buttonDownloadRepos";
            this.buttonDownloadRepos.Size = new System.Drawing.Size(339, 39);
            this.buttonDownloadRepos.TabIndex = 2;
            this.buttonDownloadRepos.Text = "Download Android Project Repositories";
            this.buttonDownloadRepos.UseVisualStyleBackColor = true;
            this.buttonDownloadRepos.Click += new System.EventHandler(this.buttonDownloadRepos_Click);
            // 
            // buttonCommitHistory
            // 
            this.buttonCommitHistory.Location = new System.Drawing.Point(12, 152);
            this.buttonCommitHistory.Name = "buttonCommitHistory";
            this.buttonCommitHistory.Size = new System.Drawing.Size(339, 39);
            this.buttonCommitHistory.TabIndex = 3;
            this.buttonCommitHistory.Text = "Get Android Project Commit History Log";
            this.buttonCommitHistory.UseVisualStyleBackColor = true;
            this.buttonCommitHistory.Click += new System.EventHandler(this.buttonCommitHistory_Click);
            // 
            // buttonManifestHistory
            // 
            this.buttonManifestHistory.Location = new System.Drawing.Point(12, 207);
            this.buttonManifestHistory.Name = "buttonManifestHistory";
            this.buttonManifestHistory.Size = new System.Drawing.Size(339, 39);
            this.buttonManifestHistory.TabIndex = 4;
            this.buttonManifestHistory.Text = "Get Android Project Manifest History";
            this.buttonManifestHistory.UseVisualStyleBackColor = true;
            this.buttonManifestHistory.Click += new System.EventHandler(this.buttonManifestHistory_Click);
            // 
            // buttonProcessAuthor
            // 
            this.buttonProcessAuthor.Location = new System.Drawing.Point(12, 262);
            this.buttonProcessAuthor.Name = "buttonProcessAuthor";
            this.buttonProcessAuthor.Size = new System.Drawing.Size(339, 39);
            this.buttonProcessAuthor.TabIndex = 5;
            this.buttonProcessAuthor.Text = "Process Author Rating";
            this.buttonProcessAuthor.UseVisualStyleBackColor = true;
            this.buttonProcessAuthor.Click += new System.EventHandler(this.buttonProcessAuthor_Click);
            // 
            // buttonProcessPermissions
            // 
            this.buttonProcessPermissions.Location = new System.Drawing.Point(12, 317);
            this.buttonProcessPermissions.Name = "buttonProcessPermissions";
            this.buttonProcessPermissions.Size = new System.Drawing.Size(339, 39);
            this.buttonProcessPermissions.TabIndex = 6;
            this.buttonProcessPermissions.Text = "Process Permissions";
            this.buttonProcessPermissions.UseVisualStyleBackColor = true;
            this.buttonProcessPermissions.Click += new System.EventHandler(this.buttonProcessPermissions_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 376);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(281, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Contact/Feedback: axp6201@rit.edu ; dxkvse@rit.edu";
            // 
            // formMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(363, 401);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.buttonProcessPermissions);
            this.Controls.Add(this.buttonProcessAuthor);
            this.Controls.Add(this.buttonManifestHistory);
            this.Controls.Add(this.buttonCommitHistory);
            this.Controls.Add(this.buttonDownloadRepos);
            this.Controls.Add(this.buttonDownloadFdroid);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "formMain";
            this.Text = "Android Manifest Analyzer";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonDownloadFdroid;
        private System.Windows.Forms.Button buttonDownloadRepos;
        private System.Windows.Forms.Button buttonCommitHistory;
        private System.Windows.Forms.Button buttonManifestHistory;
        private System.Windows.Forms.Button buttonProcessAuthor;
        private System.Windows.Forms.Button buttonProcessPermissions;
        private System.Windows.Forms.Label label2;
    }
}