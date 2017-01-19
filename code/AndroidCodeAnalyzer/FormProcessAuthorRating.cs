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
    public partial class FormProcessAuthorRating : Form
    {
        string dbPath,csvPath, workingDirectory;

        public FormProcessAuthorRating()
        {
            InitializeComponent();
        }


        private void ProcessAuthors()
        {
            UpdateStatus("Started - Processing Author");
            UpdateStatus("--------------------------------");

            Database db = new Database(dbPath, false);
            List<Commit> commitList = db.GetAllCommits();

            //Get unique apps
            var uniqueApps = commitList.Select(x => new { x.AppID}).Distinct();
            UpdateStatus("Unique Apps -" + uniqueApps.Count());

            double totalCommits = 0;
            double authorCommits = 0;
            double commitRank = 0;
            List<Commit> appCommits;
            List<AuthorRank> authorRankList = new List<AuthorRank>();
            AuthorRank authorRank;

            foreach (var app in uniqueApps)
            {
                UpdateStatus("Started Processing App: " + app.AppID);

                appCommits = commitList.Where(x => x.AppID == app.AppID).ToList();

                totalCommits = appCommits.Count();

                var uniqueAuthors = appCommits.Select(x => new { x.AuthorEmail }).Distinct();
                foreach(var author in uniqueAuthors)
                {                   
                    authorCommits = appCommits.Where(x => x.AuthorEmail.Equals(author.AuthorEmail,StringComparison.InvariantCultureIgnoreCase)).Count();
                    commitRank = (authorCommits / totalCommits);

                    authorRank = new AuthorRank();
                    authorRank.AuthorEmail = author.AuthorEmail;
                    //authorRank.AuthorName = author.AuthorName;
                    authorRank.Rank = commitRank;
                    authorRank.AuthorCommits = Convert.ToInt32(authorCommits);
                    authorRank.AppCommits = Convert.ToInt32(totalCommits);
                    authorRank.AppID = app.AppID;

                    authorRankList.Add(authorRank);
                }

                UpdateStatus("Completed Processing App: " + app.AppID);
            }

            UpdateStatus("Started - Output results to CSV");
            using (StreamWriter w = File.AppendText(string.Format(@"{0}\ProcessedAuthorRank.csv", workingDirectory)))
            {
                w.WriteLine("APPID;AUTHOR_EMAIL;AUTHOR_COMMITS;TOTAL_APP_COMMITS;PERCENT_COMMIT");
                foreach (var item in authorRankList)
                {
                    w.WriteLine("{0};{1};{2};{3};{4}",
                    item.AppID, item.AuthorEmail, item.AuthorCommits,item.AppCommits,item.Rank);
                }

            }
            UpdateStatus("Completed - Output results to CSV");

            UpdateStatus("--------------------------------");
            UpdateStatus("Completed - Processing Author");
            SetMainStatus("Completed - Processing Author Rating");
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


        delegate void UpdateStatusCallback(string text);
        delegate void ProcessCompletedCallback(string text);


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

            labelStatus.Text = "Started - Processing Author Rating";

            Thread startProcess = new Thread(ProcessAuthors);
            startProcess.Start();
        }
    }
}
