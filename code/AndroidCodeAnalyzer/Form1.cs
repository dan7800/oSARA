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
        Database db;
        List<App> apps;
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
            db = new Database(workingDirectory + @"\database.sqlite", true);
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
                //labelStatus.Text = labelStatus.Text + Environment.NewLine + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + " " + text;
                textBoxLog.Text = textBoxLog.Text + Environment.NewLine + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + " " + text;
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

        private void button3_Click(object sender, EventArgs e)
        {
           // db = new Database(@"workingDirectory-636197390928077592\database.sqlite", false);           

            Thread startProcess = new Thread(StartRepoDownload);
            startProcess.Start();

        }

        private void StartRepoDownload()
        {
            string repoLocation;
            apps = db.GetApps();
            foreach (var app in apps)
            {
                repoLocation = string.Format(@"C:/{0}\{1}", workingDirectory, app.Name);
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

        }

        private void button4_Click(object sender, EventArgs e)
        {
            db = new Database(@"workingDirectory-636197390928077592\database.sqlite", false);

            Thread startProcess = new Thread(CommitHistory);
            startProcess.Start();
        }

        private void CommitHistory()
        {
            string repoRootLocation = string.Format(@"C:/workingDirectory-{0}", 636197393254705970);
            var directories = Directory.GetDirectories(repoRootLocation);
            Repository repo;
            List<Commit> commiList;
            Commit commit;
            CommitFile commitFile;

            foreach (var directory in directories)
            {
                repo = new Repository(directory);
                commiList = new List<Commit>();

                for (int i = 0; i < repo.Commits.Count(); i++)
                {
                    commit = new Commit();
                    commit.AuthorEmail = repo.Commits.ElementAt(i).Author.Email;
                    commit.AuthorName = repo.Commits.ElementAt(i).Author.Name;
                    commit.Date = repo.Commits.ElementAt(i).Author.When.LocalDateTime;
                    commit.Message = repo.Commits.ElementAt(i).Message;
                    commit.Id = repo.Commits.ElementAt(i).Sha;


                    if (i == repo.Commits.Count() - 1)
                    {
                        Tree firstCommit = repo.Lookup<Tree>(repo.Commits.ElementAt(i).Tree.Sha);
                        Tree lastCommit = repo.Lookup<Tree>(repo.Commits.ElementAt(0).Tree.Sha);

                        var changes = repo.Diff.Compare<TreeChanges>(firstCommit, lastCommit);
                        foreach (var item in changes)
                        {
                            if (item.Status != ChangeKind.Added)
                            {
                                commitFile = new CommitFile(item.Path, ChangeKind.Added.ToString());
                                commit.CommitFiles.Add(commitFile);
                            }
                        }
                    }
                    else
                    {
                        var changes =  repo.Diff.Compare<TreeChanges>(repo.Commits.ElementAt(i).Tree, repo.Commits.ElementAt(i + 1).Tree);
                        foreach(var item in changes)
                        {
                            commitFile = new CommitFile(item.Path, item.Status.ToString());
                            commit.CommitFiles.Add(commitFile);
                        }                        
                    }


                }


               
                
                //BranchCollection b = repo.Branches;
                //foreach(var bb in b){
                //    ICommitLog l = bb.Commits;
                //    int c = l.Count();
                //}
                
            }
        }

        //static void CompareTrees(Repository repo)
        //{
        //    using (repo)
        //    {
        //        Tree commitTree = repo.Head.Tip.Tree; // Main Tree
        //        Tree parentCommitTree = repo.Head.Tip.Parents.Single().Tree; // Secondary Tree

        //        var patch = repo.Diff.Compare<Patch>(parentCommitTree, commitTree); // Difference

        //        foreach (var ptc in patch)
        //        {
        //            Console.WriteLine(ptc.Status + " -> " + ptc.Path); // Status -> File Path
        //        }
        //    }
        //}
        //public String[] FilesToMerge(Commit commit)
        //{
        //    var fileList = new List<String>();
        //    foreach (var parent in commit.Parents)
        //    {
        //        foreach (TreeEntryChanges change in repo.Diff.Compare<TreeChanges>(parent.Tree, commit.Tree))
        //        {
        //            fileList.Add(change.Path);
        //        }
        //    }
        //    return fileList.ToArray();
        //}
    }
}
