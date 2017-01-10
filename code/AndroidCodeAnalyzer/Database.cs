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

                using(SQLiteCommand command  = new SQLiteCommand(dbConnection))
                {
                    dbConnection.Open();

                    command.CommandText = Constants.CREATE_TABLE_APP;
                    command.ExecuteNonQuery();

                    command.CommandText = Constants.CREATE_TABLE_COMMIT;
                    command.ExecuteNonQuery();

                    command.CommandText = Constants.CREATE_TABLE_FILE;
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
                string commandTex;
                using (var transaction = dbConnection.BeginTransaction())
                {
                    foreach(var app in apps)
                    {
                        commandTex = string.Format(Constants.INSERT_TABLE_APP,
                            app.Name.Replace("'", "''"),
                            app.FriendlyName.Replace("'", "''"),
                            app.Summary.Replace("'", "''"),
                            app.Category.Replace("'", "''"),
                            app.Website, 
                            app.License.Replace("'", "''"),
                            app.RepoType,
                            app.IssueTracker,
                            app.Source);
                        command.CommandText = commandTex;
                        command.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }

                dbConnection.Close();
            }
        }
    }
}
