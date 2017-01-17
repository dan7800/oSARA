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
    public partial class FormProcessPermissions : Form
    {
        string dbPath, csvPath, workingDirectory;

        public FormProcessPermissions()
        {
            InitializeComponent();
        }

        private void ProcessPermissons()
        {
            UpdateStatus("Started - Processing Permissions");
            UpdateStatus("--------------------------------");

            List<ProcessedPermission>  historyList = new List<ProcessedPermission>();
            ProcessedPermission historyItem;

            Database db = new Database(dbPath, false);
            List<Permission> permissionCommits = db.GetPermissions();
            
            //Get unique apps
            var uniqueApps = permissionCommits.Select(x => new { x.AppID }).Distinct();
            UpdateStatus("Unique Apps -"+uniqueApps.Count());

            foreach (var app in uniqueApps)
            {
                long currentApp = app.AppID;

                UpdateStatus("Started Processing App: " + currentApp);

                //Get all commits that belong to the current app
                var appCommits = permissionCommits.Where(a => a.AppID == currentApp);

                //Get unique commits for the current app (a single commit can have multiple records)
                var uniqueCommits = appCommits.Select(x => new { x.Date }).Distinct().OrderBy(z => z.Date);

                int i = 0;
                List<Permission> previous = new List<Permission>();
                List<Permission> current = new List<Permission>();

                foreach (var uniqueCommit in uniqueCommits)
                {
                    //Treat the fist commit as an ADD operation
                    if (i == 0)
                    {
                        current = appCommits.Where(x => x.Date == uniqueCommit.Date).ToList();
                        foreach (var item in current)
                        {
                            historyItem = new ProcessedPermission();
                            historyItem.AppID = item.AppID;
                            historyItem.Date = item.Date;
                            historyItem.CommitID = item.CommitID;
                            historyItem.CommitGUID = item.CommitGUID;                            
                            historyItem.AuthorName = item.AuthorName;
                            historyItem.AuthorEmail = item.AuthorEmail;                            
                            historyItem.PermissionName = item.PermissionName;                            
                            historyItem.Action = ProcessedPermission.ActionType.ADD;

                            historyList.Add(historyItem);
                        }
                        previous = current.ToList();
                    }
                    else
                    {
                        //If the Permission is present in the current commit, but not in the previous commit, then the Permission is has been Added
                        //If the Permission is present in the previous commit, but not in the current commit, then the Permission has been Removed

                        //Get the all commit records of the app that have the same date value. This resultset is the 'current' commit for this iteration
                        current = appCommits.Where(x => x.Date == uniqueCommit.Date).ToList();

                        foreach (var item in current)
                        {
                            if (!previous.Exists(f => f.PermissionName.Equals(item.PermissionName,StringComparison.InvariantCultureIgnoreCase)))
                            {
                                historyItem = new ProcessedPermission();
                                historyItem.AppID = item.AppID;
                                historyItem.Date = item.Date;
                                historyItem.CommitID = item.CommitID;
                                historyItem.CommitGUID = item.CommitGUID;                                
                                historyItem.AuthorName = item.AuthorName;
                                historyItem.AuthorEmail = item.AuthorEmail;                                
                                historyItem.PermissionName = item.PermissionName;                                
                                historyItem.Action = ProcessedPermission.ActionType.ADD;

                                historyList.Add(historyItem);
                            }
                        }

                        foreach (var item in previous)
                        {
                            if (!current.Exists(f => f.PermissionName.Equals(item.PermissionName,StringComparison.InvariantCultureIgnoreCase)))
                            {
                                historyItem = new ProcessedPermission();
                                historyItem.AppID = item.AppID;
                                historyItem.Date = current[0].Date;
                                historyItem.CommitID = current[0].CommitID;
                                historyItem.CommitGUID = current[0].CommitGUID;
                                historyItem.AuthorName = current[0].AuthorName;
                                historyItem.AuthorEmail = current[0].AuthorEmail;
                                historyItem.PermissionName = item.PermissionName;
                                historyItem.Action = ProcessedPermission.ActionType.REMOVE;

                                historyList.Add(historyItem);
                            }

                        }

                        //set the current commit set to 'previous' so it can be used in the next iteration
                        previous = current.ToList();
                    }
                    i++;
                }

                UpdateStatus("Completed Processing App: " + currentApp);
            }

            UpdateStatus("Started - Output results to CSV");
            using (StreamWriter w = File.AppendText(string.Format(@"{0}\ProcessedPermissions.csv", workingDirectory)))
            {
                w.WriteLine("APPID;COMMITID;COMMIT_GUID;DATE_TEXT;DATE_TICKS;PERMISSION;ACTION;AUTHOR_NAME;AUTHOR_EMAIL");
                foreach (var item in historyList)
                {
                    w.WriteLine("{0};{1};{2};{3};{4};{5};{6};{7};{8}",
                    item.AppID, item.CommitID, item.CommitGUID, item.Date.ToString(),item.Date.Ticks, item.PermissionName, item.Action.ToString(), item.AuthorName, item.AuthorEmail);
                }
                
            }
            UpdateStatus("Completed - Output results to CSV");

            UpdateStatus("----------------------------------");
            UpdateStatus("Completed - Processing Permissions");
            SetMainStatus("Completed - Processing Permissions");
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
                textBoxCSVPath.Enabled = true;
                labelStatus.Text = text;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dbPath = textBoxDBPath.Text;
            csvPath = textBoxCSVPath.Text;

            if (!File.Exists(dbPath))
            {
                MessageBox.Show("The path for the database file (database.sqlite) is not valid." + Environment.NewLine + "Please enter the valid path.");
                return;
            }

            if (!Directory.Exists(csvPath))
            {
                MessageBox.Show("The path for the CSV output file is not valid." + Environment.NewLine + "Please enter the valid path.");
                return;
            }

            button2.Enabled = false;
            textBoxDBPath.Enabled = false;
            textBoxCSVPath.Enabled = false;

            workingDirectory = csvPath;

            labelStatus.Text = "Started - Processing Permissions";

            Thread startProcess = new Thread(ProcessPermissons);
            startProcess.Start();
        }

        delegate void UpdateStatusCallback(string text);
        delegate void ProcessCompletedCallback(string text);
    }
}
