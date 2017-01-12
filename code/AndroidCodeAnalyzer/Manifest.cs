using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndroidCodeAnalyzer
{
    class Manifest
    {
        List<string> permission;
        int? minSdkVersion;
        int? targetSdkVersion;
        string authorName;
        string authorEmail;
        string commitGUID;
        long appID;
        long commitID;
        DateTime commitDate;
        string content;

        public List<string> Permission { get => permission; set => permission = value; }
        public int? MinSdkVersion { get => minSdkVersion; set => minSdkVersion = value; }
        public int? TargetSdkVersion { get => targetSdkVersion; set => targetSdkVersion = value; }
        public string AuthorName { get => authorName; set => authorName = value; }
        public string AuthorEmail { get => authorEmail; set => authorEmail = value; }
        public string CommitGUID { get => commitGUID; set => commitGUID = value; }
        public long AppID { get => appID; set => appID = value; }
        public long CommitID { get => commitID; set => commitID = value; }
        public DateTime CommitDate { get => commitDate; set => commitDate = value; }
        public string Content { get => content; set => content = value; }
    }
}
