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
using System.Xml;

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

        private void button2_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = false;

            Thread startProcess = new Thread(CloneFdroidRepo);
            startProcess.Start();
        }

        private void CloneFdroidRepo()
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

            SetMainStatus("Completed - Clone f-droid");
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
                //labelStatus.Text = labelStatus.Text + Environment.NewLine + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + " " + text;
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
                button3.Enabled = true;
                button4.Enabled = true;
                button5.Enabled = true;

                labelStatus.Text = text;
            }
        }

        delegate void UpdateStatusCallback(string text);
        delegate void ProcessCompletedCallback(string text);

        private void button3_Click(object sender, EventArgs e)
        {
            labelStatus.Text = "Started - App Repos Download";

            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = false;

            db = new Database(@"workingDirectory-636197390928077592\database.sqlite", false);           

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

            SetMainStatus("Completed - App Repos Download");

        }

        private void button4_Click(object sender, EventArgs e)
        {
            labelStatus.Text = "Started - Get Commit History";

            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = false;

            db = new Database(@"workingDirectory-636197390928077592\database.sqlite", false);

            Thread startProcess = new Thread(CommitHistory);
            startProcess.Start(false);
        }

        private void CommitHistory(object LogCommitFiles)
        {

            bool includeFiles = (bool)LogCommitFiles;
            string repoRootLocation = string.Format(@"C:/workingDirectory-{0}", 636197393254705970);
            var directories = Directory.GetDirectories(repoRootLocation);
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
                    int i = repo.Commits.Count() - 1 ;
                    foreach(var cx in repo.Commits)
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

        private void AndroidManifestHistory()
        {

            string repoRootLocation = string.Format(@"C:/workingDirectory-{0}", 636197393254705970);
            Repository repo;
            List<Manifest> manifestList;
            Manifest manifest;
            
            var directories = Directory.GetDirectories(repoRootLocation);

            foreach (var directory in directories)
            {
 
                string lastFolderName = Path.GetFileName(directory);
                var manifestFile = Directory.GetFiles(directory, "AndroidManifest.xml", SearchOption.AllDirectories).FirstOrDefault();
                long appId = db.GetAppByName(lastFolderName).Id;

                try
                {
                    UpdateStatus(string.Format("Started - Manifest Histroy for {0}", lastFolderName));

                    if (string.IsNullOrEmpty(manifestFile))
                    {
                        UpdateStatus(string.Format("Failed - Manifest File Not Found for {0}", lastFolderName));
                        continue;
                    }
                    else
                    {
                        manifestList = new List<Manifest>();
                        repo = new Repository(directory);
                        var manifestFileRelativePath = manifestFile.Substring(repoRootLocation.Length + lastFolderName.Length + 2);
                        IEnumerable<LogEntry> fileHistory = repo.Commits.QueryBy(manifestFileRelativePath);

                        foreach (var version in fileHistory)
                        {
                            manifest = new Manifest();
                            manifest.AppID = appId;
                            manifest.AuthorEmail = version.Commit.Author.Email;
                            manifest.AuthorName = version.Commit.Author.Name;
                            manifest.CommitDate = version.Commit.Author.When.LocalDateTime;
                            manifest.CommitGUID = version.Commit.Sha;
                            manifest.CommitID = db.GetCommitId(appId, version.Commit.Sha);

                            //var commit = repo.Lookup<LibGit2Sharp.Commit>(version.Commit.Sha); // or any other way to retreive a specific commit
                            var treeEntry = version.Commit[manifestFileRelativePath];
                            if (treeEntry == null)
                                continue;

                            var blob = (Blob)treeEntry.Target;
                            var contentStream = blob.GetContentStream();
                            using (var tr = new StreamReader(contentStream, Encoding.UTF8))
                            {
                                manifest.Content = tr.ReadToEnd();
                            }

                            manifest.MinSdkVersion = Convert.ToInt32(XMLExtract(manifest.Content, "uses-sdk", "android:minSdkVersion").FirstOrDefault());
                            manifest.TargetSdkVersion = Convert.ToInt32(XMLExtract(manifest.Content, "uses-sdk", "android:targetSdkVersion").FirstOrDefault());
                            manifest.Permission = XMLExtract(manifest.Content, "uses-permission", "android:name");

                            manifestList.Add(manifest);

                        }
                    }

                    db.BatchInsertManifest(manifestList);

                    UpdateStatus(string.Format("Completed - Manifest Histroy for {0}", lastFolderName));
                }
                catch (Exception error)
                {
                    UpdateStatus(string.Format("Failed -  Manifest Histroy for {0} ; {1}", lastFolderName, error.Message));
                    continue;
                }
            }

            SetMainStatus("Completed - Get Manifest History");
        }

        private List<string> XMLExtract(string xml, string node, string attribute)
        {
            List<string> extract = new List<string>();

            var doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlNodeList elemList = doc.GetElementsByTagName(node);
            for (int i = 0; i < elemList.Count; i++)
            {
                if (elemList[i].Attributes[attribute] != null)
                {
                    var attrVal = elemList[i].Attributes[attribute].Value;
                    if (!string.IsNullOrEmpty(attrVal))
                        extract.Add(attrVal);
                }
            }

            return extract;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            labelStatus.Text = "Started - Get Maifest History";

            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = false;

            db = new Database(@"workingDirectory-636197390928077592\database.sqlite", false);

            Thread startProcess = new Thread(AndroidManifestHistory);
            startProcess.Start();
        }

    }
}
