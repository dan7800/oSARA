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

        public void BatchInsertCommits(List<Commit> Commits, long AppID)
        {
            using (SQLiteCommand command = new SQLiteCommand(dbConnection))
            {
                dbConnection.Open();
                string commandText;
                using (var transaction = dbConnection.BeginTransaction())
                {
                    foreach (var commit in Commits)
                    {
                        commandText = string.Format(Constants.INSERT_TABLE_COMMIT_LOG,
                            commit.Id,
                            commit.Message.Replace("'", "''"),
                            commit.AuthorName.Replace("'", "''"),
                            commit.AuthorEmail,
                            commit.Date.ToString(),
                            commit.Date.Ticks,
                            AppID);
                        command.CommandText = commandText;
                        //command.ExecuteNonQuery();
                        object obj =  command.ExecuteScalar();

                        foreach(var file in commit.CommitFiles)
                        {
                            commandText = string.Format(Constants.INSERT_TABLE_COMMIT_LOG_FILE,
                                file.Path,
                                file.Operation,
                                commit.Id,
                                (long)obj,
                                AppID);
                            command.CommandText = commandText;
                            command.ExecuteNonQuery();
                        }
                    }

                    transaction.Commit();
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
    }
}
