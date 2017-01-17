using LibGit2Sharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AndroidCodeAnalyzer
{
    public partial class FormDownloadRepos : Form
    {
        string dbPath, downloadPath, workingDirectory;

        public FormDownloadRepos()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dbPath = textBoxDBPath.Text;
            downloadPath = textBoxDownloadPath.Text;

            if (!Directory.Exists(downloadPath))
            {
                MessageBox.Show("The path for the download location is not valid." + Environment.NewLine + "Please enter a valid path.");
                return;
            }

            if (!File.Exists(dbPath))
            {
                MessageBox.Show("The path for the database file (database.sqlite) is not valid." + Environment.NewLine + "Please enter the valid path.");
                return;
            }

            button2.Enabled = false;
            textBoxDBPath.Enabled = false;
            textBoxDownloadPath.Enabled = false;
            workingDirectory = string.Format(@"{0}\Apps", downloadPath);

            labelStatus.Text = "Started - App Repository Download";
            Thread startProcess = new Thread(StartRepoDownload);
            startProcess.Start();
        }

        private void StartRepoDownload()
        {

            string repoLocation;
            Database db = new Database(dbPath, false);
            List<App> apps = db.GetApps();
            foreach (var app in apps)
            {
                repoLocation = string.Format(@"{0}\{1}", workingDirectory, app.Name);
                if (Directory.Exists(repoLocation))
                    continue;
                try
                {
                    UpdateStatus(string.Format("Started - Clone {0}", app.Name));
                    CloneOptions options = new CloneOptions();
                    options.BranchName = "master";
                    options.Checkout = true;
                    Repository.Clone(app.Source, repoLocation, options);
                    db.UpsertAppDonwload(app.Id, DateTime.Now);
                    UpdateStatus(string.Format("Completed - Clone {0}", app.Name));
                }
                catch (Exception error)
                {
                    UpdateStatus(string.Format("Failed - Clone {0} ; {1}", app.Name, error.Message));
                    continue;
                }
            }

            SetMainStatus("Completed - App Repository Download");

        }

        private void UpdateStatus(string text)
        {
            if (this.button2.InvokeRequired)
            {
                UpdateStatusCallback callback = new UpdateStatusCallback(UpdateStatus);
                this.Invoke(callback, new object[] { text });
            }
            else
            {
                textBoxLog.Text = textBoxLog.Text + Environment.NewLine + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + " " + text;
            }
        }

        private void SetMainStatus(string text)
        {
            if (this.button2.InvokeRequired)
            {
                ProcessCompletedCallback callback = new ProcessCompletedCallback(SetMainStatus);
                this.Invoke(callback, new object[] { text });
            }
            else
            {
                button2.Enabled = true;
                textBoxDBPath.Enabled = true;
                textBoxDownloadPath.Enabled = true;
                labelStatus.Text = text;
            }
        }


        delegate void UpdateStatusCallback(string text);
        delegate void ProcessCompletedCallback(string text);
    }
}
