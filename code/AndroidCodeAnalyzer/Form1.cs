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
    public partial class Form1 : Form
    {
        string workingDirectory;
        public Form1()
        {
            InitializeComponent();
            workingDirectory = "workingDirectory-" + DateTime.Now.Ticks.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SourceFileParser sp = new SourceFileParser(@"\\VBOXSVR\Projects\AndroidCodeAnalyzer\AndroidCodeAnalyzer\files\");
            FileInfo[] f =  sp.Files;
            f.Count();
            List<App> apps =  sp.ParseFiles();
            apps.Count();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;

            Thread startProcess = new Thread(StartProcess);
            startProcess.Start();        
        }

        private void StartProcess()
        {
            UpdateStatus("Started - Clone f-Droid Repo");
            CloneOptions options = new CloneOptions();
            options.BranchName = "master";
            options.Checkout = true;
            string repo = Repository.Clone(Constants.GIT_FDROID, workingDirectory + @"\F-droid", options);
            UpdateStatus("Completed - Clone f-Droid Repo");

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

            ProcessCompleted();
        }
        

        private void UpdateStatus(string text)
        {
            if (this.button2.InvokeRequired)
            {
                UpdateStatusCallback callback = new UpdateStatusCallback(UpdateStatus);
                this.Invoke(callback,new object[] { text});
            }
            else
            {
                labelStatus.Text = labelStatus.Text + Environment.NewLine + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + " " + text;
            }
        }

        private void ProcessCompleted()
        {
            if (this.button2.InvokeRequired)
            {
                ProcessCompletedCallback callback = new ProcessCompletedCallback(ProcessCompleted);
                this.Invoke(callback);
            }
            else
            {
                button2.Enabled = true;
            }
        }

        delegate void UpdateStatusCallback(string text);
        delegate void ProcessCompletedCallback();
    }
}
