using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndroidCodeAnalyzer
{
    class AuthorRank
    {
        long appID;
        int authorCommits;
        int appCommits;
        double rank;
        string authorName;
        string authorEmail;

        public long AppID { get => appID; set => appID = value; }
        public int AuthorCommits { get => authorCommits; set => authorCommits = value; }
        public double Rank { get => rank; set => rank = value; }
        public string AuthorName { get => authorName; set => authorName = value; }
        public string AuthorEmail { get => authorEmail; set => authorEmail = value; }
        public int AppCommits { get => appCommits; set => appCommits = value; }
    }
}
