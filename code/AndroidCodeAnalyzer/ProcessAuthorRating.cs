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
    public partial class ProcessAuthorRating : Form
    {
        string workingDirectory;
        Database db;

        public ProcessAuthorRating()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            db = new Database(@"workingDirectory-636197390928077592\database.sqlite", false);

            Thread startProcess = new Thread(ProcessAuthors);
            startProcess.Start();
        }

        private void ProcessAuthors()
        {
            UpdateStatus("Started - Processing Author");
            UpdateStatus("--------------------------------");

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

                var uniqueAuthors = appCommits.Select(x => new { x.AuthorEmail, x.AuthorName }).Distinct();
                foreach(var author in uniqueAuthors)
                {                   
                    authorCommits = appCommits.Where(x => x.AuthorEmail.Equals(author.AuthorEmail,StringComparison.InvariantCultureIgnoreCase)).Count();
                    commitRank = (authorCommits / totalCommits);

                    authorRank = new AuthorRank();
                    authorRank.AuthorEmail = author.AuthorEmail;
                    authorRank.AuthorName = author.AuthorName;
                    authorRank.Rank = commitRank;
                    authorRank.AuthorCommits = Convert.ToInt32(authorCommits);
                    authorRank.AppCommits = Convert.ToInt32(totalCommits);
                    authorRank.AppID = app.AppID;

                    authorRankList.Add(authorRank);
                }

                UpdateStatus("Completed Processing App: " + app.AppID);
            }

            UpdateStatus("Started - Output results to CSV");
            using (StreamWriter w = File.AppendText("ProcessedAuthorRank.csv"))
            {
                w.WriteLine("APPID;AUTHOR_NAME;AUTHOR_EMAIL;AUTHOR_COMMITS;TOTAL_APP_COMMITS;PERCENT_COMMIT");
                foreach (var item in authorRankList)
                {
                    w.WriteLine("{0};{1};{2};{3};{4};{5}",
                    item.AppID, item.AuthorName, item.AuthorEmail, item.AuthorCommits,item.AppCommits,item.Rank);
                }

            }
            UpdateStatus("Completed - Output results to CSV");

            UpdateStatus("--------------------------------");
            UpdateStatus("Completed - Processing Author");                    
        }






        private void UpdateStatus(string text)
        {
            if (this.button1.InvokeRequired)
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

        delegate void UpdateStatusCallback(string text);
    }
}
