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
    public partial class FormManifestHistory : Form
    {
        string dbPath, downloadPath, workingDirectory;

        public FormManifestHistory()
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

            labelStatus.Text = "Started - Get Maifest History";
            Thread startProcess = new Thread(AndroidManifestHistory);
            startProcess.Start();
        }


        private void AndroidManifestHistory()
        {
            Database db = new Database(dbPath, false);
            Repository repo;
            List<Manifest> manifestList;
            Manifest manifest;

            var directories = Directory.GetDirectories(workingDirectory);

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
                        var manifestFileRelativePath = manifestFile.Substring(workingDirectory.Length + lastFolderName.Length + 2);
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
