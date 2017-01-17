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
    public partial class FormCommitHistory : Form
    {
        string dbPath, downloadPath, workingDirectory;

        public FormCommitHistory()
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

            labelStatus.Text = "Started - Get Commit History";
            Thread startProcess = new Thread(CommitHistory);
            startProcess.Start(false);
        }


        private void CommitHistory(object LogCommitFiles)
        {

            bool includeFiles = (bool)LogCommitFiles;
            var directories = Directory.GetDirectories(workingDirectory);
            Database db = new Database(dbPath, false);
            Repository repo;
            List<Commit> commiList;
            Commit commit;
            CommitFile commitFile;

            foreach (var directory in directories)
            {

                string lastFolderName = Path.GetFileName(directory);
                long appId = db.GetAppByName(lastFolderName).Id;
                try
                {
                    repo = new Repository(directory);
                    commiList = new List<Commit>();

                    int commitCount = repo.Commits.Count();
                    UpdateStatus(string.Format("Started - Commit Histroy for {0} ; Total Commits: {1}", lastFolderName, commitCount));
                    int i = repo.Commits.Count() - 1;
                    foreach (var cx in repo.Commits)
                    // for (int i = commitCount - 1; i >= 0; i--)
                    {
                        commit = new Commit();
                        commit.AuthorEmail = cx.Author.Email;
                        commit.AuthorEmail = cx.Author.Email;
                        commit.AuthorName = cx.Author.Name;
                        commit.Date = cx.Author.When.LocalDateTime;
                        commit.Message = cx.Message;
                        commit.GUID = cx.Sha;

                        if (includeFiles)
                        {
                            if (i == commitCount - 1)
                            {
                                Tree firstCommit = repo.Lookup<Tree>(repo.Commits.ElementAt(i).Tree.Sha);
                                Tree lastCommit = repo.Lookup<Tree>(repo.Commits.ElementAt(0).Tree.Sha);

                                var changes = repo.Diff.Compare<TreeChanges>(lastCommit, firstCommit);
                                foreach (var item in changes)
                                {
                                    if (item.Status != ChangeKind.Deleted)
                                    {
                                        commitFile = new CommitFile(item.Path, ChangeKind.Added.ToString());
                                        commit.CommitFiles.Add(commitFile);
                                    }
                                }
                            }
                            else
                            {
                                var changes = repo.Diff.Compare<TreeChanges>(repo.Commits.ElementAt(i + 1).Tree, repo.Commits.ElementAt(i).Tree);
                                foreach (var item in changes)
                                {
                                    commitFile = new CommitFile(item.Path, item.Status.ToString());
                                    commit.CommitFiles.Add(commitFile);
                                }
                            }
                        }

                        commiList.Add(commit);

                        i--;
                    }

                    db.BatchInsertCommits(commiList, appId);

                    UpdateStatus(string.Format("Complted - Commit Histroy for {0}", lastFolderName));
                }
                catch (Exception error)
                {
                    UpdateStatus(string.Format("Failed - Commit Histroy for {0} ; {1}", lastFolderName, error.Message));
                    continue;
                }
            }

            SetMainStatus("Completed - Get Commit History");
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
