using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndroidCodeAnalyzer
{
    class Commit
    {
        string guid;
        string message;
        string authorName;
        string authorEmail;
        long appID;
        DateTime date;
        List<CommitFile> commitFiles;

        public string GUID { get => guid; set => guid = value; }
        public string Message { get => message; set => message = value; }
        public string AuthorName { get => authorName; set => authorName = value; }
        public string AuthorEmail { get => authorEmail; set => authorEmail = value; }
        public DateTime Date { get => date; set => date = value; }
        internal List<CommitFile> CommitFiles { get => commitFiles; set => commitFiles = value; }
        public long AppID { get => appID; set => appID = value; }

        public Commit()
        {
            guid = string.Empty;
            message = string.Empty;
            authorEmail = string.Empty;
            authorName = string.Empty;
            commitFiles = new List<CommitFile>();
            date = DateTime.Now;
        }
    }
}
