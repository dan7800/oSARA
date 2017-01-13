using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndroidCodeAnalyzer
{
    class Database
    {
        SQLiteConnection dbConnection;

        public Database(string databaseFile, bool forceCreate)
        {
            if (forceCreate)
            {
                SQLiteConnection.CreateFile(databaseFile);

                var dbConnectionString = string.Format(Constants.CONNECTION_STRING, databaseFile);
                dbConnection = new SQLiteConnection(dbConnectionString);

                using (SQLiteCommand command = new SQLiteCommand(dbConnection))
                {
                    dbConnection.Open();

                    command.CommandText = Constants.CREATE_TABLE_APP;
                    command.ExecuteNonQuery();

                    command.CommandText = Constants.CREATE_TABLE_COMMIT_LOG;
                    command.ExecuteNonQuery();

                    command.CommandText = Constants.CREATE_TABLE_COMMIT_LOG_FILE;
                    command.ExecuteNonQuery();

                    command.CommandText = Constants.CREATE_TABLE_APP_CLONE;
                    command.ExecuteNonQuery();

                    command.CommandText = Constants.CREATE_TABLE_MANIFEST;
                    command.ExecuteNonQuery();

                    command.CommandText = Constants.CREATE_TABLE_MANIFEST_PERMISSION;
                    command.ExecuteNonQuery();

                    command.CommandText = Constants.CREATE_TABLE_MANIFEST_SDK;
                    command.ExecuteNonQuery();

                    dbConnection.Close();
                }
            }
            else
            {
                var dbConnectionString = string.Format(Constants.CONNECTION_STRING, databaseFile);
                dbConnection = new SQLiteConnection(dbConnectionString);
            }
        }

        public void BatchInsertApps(List<App> apps)
        {
            using (SQLiteCommand command = new SQLiteCommand(dbConnection))
            {
                dbConnection.Open();
                string commandText;
                using (var transaction = dbConnection.BeginTransaction())
                {
                    foreach (var app in apps)
                    {
                        commandText = string.Format(Constants.INSERT_TABLE_APP,
                            app.Name.Replace("'", "''"),
                            app.FriendlyName.Replace("'", "''"),
                            app.Summary.Replace("'", "''"),
                            app.Category.Replace("'", "''"),
                            app.Website,
                            app.License.Replace("'", "''"),
                            app.RepoType,
                            app.IssueTracker,
                            app.Source);
                        command.CommandText = commandText;
                        command.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }

                dbConnection.Close();
            }
        }

        public void BatchInsertManifest(List<Manifest> manifests)
        {
            using (SQLiteCommand command = new SQLiteCommand(dbConnection))
            {
                dbConnection.Open();
                string commandText;
                using (var transaction = dbConnection.BeginTransaction())
                {
                    foreach (var manifest in manifests)
                    {
                        commandText = string.Format(Constants.INSERT_TABLE_MANIFEST,
                            manifest.AppID,
                            manifest.CommitGUID,
                            manifest.CommitID,
                            manifest.Content.Replace("'", "''"),
                            manifest.AuthorName.Replace("'", "''"),
                            manifest.AuthorEmail,
                            manifest.CommitDate.ToString(),
                            manifest.CommitDate.Ticks);
                        command.CommandText = commandText;
                        command.ExecuteNonQuery();

                        commandText = string.Format(Constants.INSERT_TABLE_MANIFEST_SDK,
                            manifest.AppID,
                            manifest.CommitGUID,
                            manifest.CommitID,
                            manifest.MinSdkVersion == 0 ? "null" : manifest.MinSdkVersion.ToString(),
                            manifest.TargetSdkVersion == 0 ? "null":manifest.TargetSdkVersion.ToString(),
                            manifest.AuthorName.Replace("'", "''"),
                            manifest.AuthorEmail,
                            manifest.CommitDate.ToString(),
                            manifest.CommitDate.Ticks);
                        command.CommandText = commandText;
                        command.ExecuteNonQuery();

                        foreach(var permission in manifest.Permission)
                        {
                            commandText = string.Format(Constants.INSERT_TABLE_MANIFEST_PERMISSION,
                                manifest.AppID,
                                manifest.CommitGUID,
                                manifest.CommitID,
                                permission,
                                manifest.AuthorName.Replace("'", "''"),
                                manifest.AuthorEmail,
                                manifest.CommitDate.ToString(),
                                manifest.CommitDate.Ticks);
                            command.CommandText = commandText;
                            command.ExecuteNonQuery();
                        }
                    }

                    transaction.Commit();
                }

                dbConnection.Close();
            }
        }

        public void BatchInsertCommits(List<Commit> Commits, long AppID)
        {
            using (SQLiteCommand command = new SQLiteCommand(dbConnection))
            {
                dbConnection.Open();
                string commandText;
                try
                {
                    using (var transaction = dbConnection.BeginTransaction())
                    {
                        foreach (var commit in Commits)
                        {
                            commandText = string.Format(Constants.INSERT_TABLE_COMMIT_LOG,
                                commit.GUID,
                                commit.Message.Replace("'", "''"),
                                commit.AuthorName.Replace("'", "''"),
                                commit.AuthorEmail,
                                commit.Date.ToString(),
                                commit.Date.Ticks,
                                AppID);
                            command.CommandText = commandText;
                            //command.ExecuteNonQuery();
                            object obj = command.ExecuteScalar();

                            foreach (var file in commit.CommitFiles)
                            {
                                commandText = string.Format(Constants.INSERT_TABLE_COMMIT_LOG_FILE,
                                    file.Path.Replace("'", "''"),
                                    file.Operation,
                                    commit.GUID,
                                    (long)obj,
                                    AppID);
                                command.CommandText = commandText;
                                command.ExecuteNonQuery();
                            }
                        }

                        transaction.Commit();
                    }
                }
                catch(Exception error)
                {
                    dbConnection.Close();
                    throw error;
                }

                dbConnection.Close();
            }
        }

        public App GetAppByName(string Name)
        {
            App app = new App();
            using (SQLiteCommand command = new SQLiteCommand(dbConnection))
            {
                dbConnection.Open();
                using (var transaction = dbConnection.BeginTransaction())
                {

                    command.CommandText = string.Format(Constants.SELECT_APP,Name);
                    command.CommandType = System.Data.CommandType.Text;
                    SQLiteDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        app = new App();
                        app.Name = reader[Constants.COLUMN_APP_NAME].ToString();
                        app.FriendlyName = reader[Constants.COLUMN_APP_FRIENDLY_NAME].ToString();
                        app.Summary = reader[Constants.COLUMN_APP_SUMMARY].ToString();
                        app.Category = reader[Constants.COLUMN_APP_CATEGORY].ToString();
                        app.Website = reader[Constants.COLUMN_APP_WEBSITE].ToString();
                        app.License = reader[Constants.COLUMN_APP_LICENSE].ToString();
                        app.RepoType = reader[Constants.COLUMN_APP_REPO_TYPE].ToString();
                        app.IssueTracker = reader[Constants.COLUMN_APP_ISSUE_TRACKER].ToString();
                        app.Source = reader[Constants.COLUMN_APP_SOURCE].ToString();
                        app.Id = Convert.ToInt64(reader[Constants.COLUMN_APP_ID]);
                    }
                }

                dbConnection.Close();
            }

            return app;
        }

        public long GetCommitId(long AppID, string CommitGUID)
        {
            long id = new long();
            using (SQLiteCommand command = new SQLiteCommand(dbConnection))
            {
                dbConnection.Open();
                using (var transaction = dbConnection.BeginTransaction())
                {

                    command.CommandText = string.Format(Constants.SELECT_COMMIT, AppID,CommitGUID);
                    command.CommandType = System.Data.CommandType.Text;
                    SQLiteDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        id = Convert.ToInt64(reader[Constants.COLUMN_COMMIT_LOG_ID].ToString());
                    }
                }

                dbConnection.Close();
            }

            return id;
        }

        public List<Commit> GetAllCommits()
        {
            List<Commit> commitList = new List<Commit>();
            Commit commit;
            using (SQLiteCommand command = new SQLiteCommand(dbConnection))
            {
                dbConnection.Open();
                using (var transaction = dbConnection.BeginTransaction())
                {
                    command.CommandText = string.Format(Constants.SELECT_ALL_COMMIT);
                    command.CommandType = System.Data.CommandType.Text;
                    SQLiteDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        commit = new Commit();
                        commit.AuthorEmail = reader[Constants.COLUMN_COMMIT_LOG_AUTHOR_EMAIL].ToString();
                        commit.AuthorName = reader[Constants.COLUMN_COMMIT_LOG_AUTHOR_NAME].ToString();
                        commit.Date = new DateTime(Convert.ToInt64(reader[Constants.COLUMN_COMMIT_LOG_DATE_TICKS]));
                        commit.GUID = reader[Constants.COLUMN_COMMIT_LOG_GUID].ToString();
                        commit.Message = reader[Constants.COLUMN_COMMIT_LOG_MESSAGE].ToString();
                        commit.AppID = Convert.ToInt64(reader[Constants.COLUMN_COMMIT_LOG_APPID]);

                        commitList.Add(commit);
                    }
                }

                dbConnection.Close();
            }

            return commitList;
        }

        public void UpsertAppDonwload(long appId, DateTime dowloadDate)
        {
            string commandText = string.Format(Constants.UPSERT_TABLE_APP_CLONE, appId, dowloadDate.ToString(), dowloadDate.Ticks);

            using (SQLiteCommand command = new SQLiteCommand(dbConnection))
            {
                dbConnection.Open();

                using (var transaction = dbConnection.BeginTransaction())
                {
                    command.CommandText = commandText;
                    command.ExecuteNonQuery();
                    transaction.Commit();
                }

                dbConnection.Close();
            }

        }

        public List<App> GetApps()
        {
            List<App> apps = new List<App>();

            using (SQLiteCommand command = new SQLiteCommand(dbConnection))
            {
                dbConnection.Open();
                using (var transaction = dbConnection.BeginTransaction())
                {

                    command.CommandText = Constants.SELECT_ALL_APP;
                    command.CommandType = System.Data.CommandType.Text;
                    SQLiteDataReader reader = command.ExecuteReader();
                    App app;
                    while (reader.Read())
                    {
                        app = new App();
                        app.Name = reader[Constants.COLUMN_APP_NAME].ToString();
                        app.FriendlyName = reader[Constants.COLUMN_APP_FRIENDLY_NAME].ToString();
                        app.Summary = reader[Constants.COLUMN_APP_SUMMARY].ToString();
                        app.Category = reader[Constants.COLUMN_APP_CATEGORY].ToString();
                        app.Website = reader[Constants.COLUMN_APP_WEBSITE].ToString();
                        app.License = reader[Constants.COLUMN_APP_LICENSE].ToString();
                        app.RepoType = reader[Constants.COLUMN_APP_REPO_TYPE].ToString();
                        app.IssueTracker = reader[Constants.COLUMN_APP_ISSUE_TRACKER].ToString();
                        app.Source = reader[Constants.COLUMN_APP_SOURCE].ToString();
                        app.Id = Convert.ToInt64(reader[Constants.COLUMN_APP_ID]);

                        apps.Add(app);
                    }
                }

                dbConnection.Close();
            }

            return apps;
        }

        public List<Permission> GetPermissions()
        {
            List<Permission> permissionList = new List<Permission>();

            using (SQLiteCommand command = new SQLiteCommand(dbConnection))
            {
                dbConnection.Open();
                using (var transaction = dbConnection.BeginTransaction())
                {

                    command.CommandText = Constants.SELECT_ALL_PERMISSION_HISTORY;
                    command.CommandType = System.Data.CommandType.Text;
                    SQLiteDataReader reader = command.ExecuteReader();
                    Permission permission;
                    while (reader.Read())
                    {
                        permission = new Permission();
                        permission.AppID = Convert.ToInt64(reader[Constants.COLUMN_MANIFEST_PERMISSION_APPID]);
                        permission.CommitGUID = reader[Constants.COLUMN_MANIFEST_PERMISSION_COMMIT_GUID].ToString();
                        permission.CommitID = Convert.ToInt64(reader[Constants.COLUMN_MANIFEST_PERMISSION_COMMITID]);
                        permission.PermissionName = reader[Constants.COLUMN_MANIFEST_PERMISSION_PERMISSION].ToString();
                        permission.AuthorName = reader[Constants.COLUMN_MANIFEST_PERMISSION_AUTHOR_NAME].ToString();
                        permission.AuthorEmail = reader[Constants.COLUMN_MANIFEST_PERMISSION_AUTHOR_EMAIL].ToString();
                        permission.Date = new DateTime(Convert.ToInt64(reader[Constants.COLUMN_MANIFEST_PERMISSION_DATE_TICKS]));

                        permissionList.Add(permission);
                    }
                }

                dbConnection.Close();
            }

            return permissionList;
        }
    }
}
