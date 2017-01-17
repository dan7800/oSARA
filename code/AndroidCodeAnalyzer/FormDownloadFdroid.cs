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
    public partial class FormDownloadFdroid : Form
    {
        string workingDirectory;

        public FormDownloadFdroid()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var path = textBoxPath.Text;
            if (!Directory.Exists(path))
            {
                MessageBox.Show("The path for the download location is not valid." + Environment.NewLine + "Please enter a valid path.");
                return;
            }

            button2.Enabled = false;
            textBoxPath.Enabled = false;
            workingDirectory = string.Format(@"{0}\workingDirectory-{1}", path, DateTime.Now.Ticks.ToString());

            labelStatus.Text = "Started - Clone F-Droid Repositroy";
            Thread startProcess = new Thread(CloneFdroidRepo);
            startProcess.Start();
        }

        private void CloneFdroidRepo()
        {
            UpdateStatus("Started - Clone F-Droid Repositroy");
            CloneOptions options = new CloneOptions();
            options.BranchName = "master";
            options.Checkout = true;
            string repo = Repository.Clone(Constants.GIT_FDROID, workingDirectory + @"\F-droid", options);
            UpdateStatus("Completed - Clone F-Droid Repositroy");

            UpdateStatus("Started - Analyzing Metadata Files");
            SourceFileParser sp = new SourceFileParser(workingDirectory + @"\F-droid\metadata");
            FileInfo[] f = sp.Files;
            f.Count();
            List<App> apps = sp.ParseFiles();
            UpdateStatus("Total Files Analyzed: " + apps.Count());
            UpdateStatus("Completed - Analyzing Metadata Files");

            UpdateStatus("Started - Create Database");
            Database db = new Database(workingDirectory + @"\database.sqlite", true);
            UpdateStatus("Completed - Create Database");
            UpdateStatus("Started - Insert App Records");
            db.BatchInsertApps(apps);
            UpdateStatus("Completed - Insert App Records");

            SetMainStatus("Completed - Clone F-Droid Repositroy");
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
                textBoxPath.Enabled = true;
                labelStatus.Text = text;
            }
        }

        delegate void UpdateStatusCallback(string text);
        delegate void ProcessCompletedCallback(string text);

    }
}
