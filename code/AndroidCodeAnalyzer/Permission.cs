using System;

namespace AndroidCodeAnalyzer
{
    class Permission
    {
        long appID;
        string commitGUID;
        long commitID;
        string permissionName;
        string authorName;
        string authorEmail;
        DateTime date;

        public long AppID { get => appID; set => appID = value; }
        public string CommitGUID { get => commitGUID; set => commitGUID = value; }
        public long CommitID { get => commitID; set => commitID = value; }
        public string PermissionName { get => permissionName; set => permissionName = value; }
        public string AuthorName { get => authorName; set => authorName = value; }
        public string AuthorEmail { get => authorEmail; set => authorEmail = value; }
        public DateTime Date { get => date; set => date = value; }
    }

    class ProcessedPermission
    {
        long appID;
        string commitGUID;
        long commitID;
        string permissionName;
        string authorName;
        string authorEmail;
        DateTime date;
        ActionType action;

        public long AppID { get => appID; set => appID = value; }
        public string CommitGUID { get => commitGUID; set => commitGUID = value; }
        public long CommitID { get => commitID; set => commitID = value; }
        public string PermissionName { get => permissionName; set => permissionName = value; }
        public string AuthorName { get => authorName; set => authorName = value; }
        public string AuthorEmail { get => authorEmail; set => authorEmail = value; }
        public DateTime Date { get => date; set => date = value; }
        internal ActionType Action { get => action; set => action = value; }

        public enum ActionType
        {
            ADD, REMOVE
        }
    }
}
