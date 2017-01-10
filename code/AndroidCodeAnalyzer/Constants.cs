using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndroidCodeAnalyzer
{
    class Constants
    {
        public static string CREATE_TABLE_APP="CREATE TABLE APP(" +
            "ID INTEGER PRIMARY KEY AUTOINCREMENT," +
            "NAME TEXT NOT NULL," +
            "FRIENDLY_NAME TEXT," +
            "SUMMARY TEXT," +
            "CATEGORY TEXT," +
            "WEBSITE TEXT," +
            "LICENSE TEXT," +
            "REPO_TYPE TEXT," +
            "ISSUE_TRACKER TEXT," +
            "SOURCE TEXT NOT NULL" +
            ");";

        public static string CREATE_TABLE_COMMIT = "CREATE TABLE COMMIT_LOG(" +
            "ID INTEGER PRIMARY KEY AUTOINCREMENT," +
            "GUID TEXT NOT NULL," +
            "MESSAGE TEXT," +
            "AUTHOR_NAME TEXT," +
            "AUTHOR_EMAIL TEXT," +
            "DATE_TEXT TEXT," +
            "DATE_TICKS REAL," +
            "APPID INT NOT NULL" +
            ");";

        public static string CREATE_TABLE_FILE = "CREATE TABLE FILE(" +
            "ID INTEGER PRIMARY KEY AUTOINCREMENT," +
            "NAME TEXT," +
            "COMMITID INT NOT NULL," +
            "APPID INT NOT NULL" +
            ");";


        public static string INSERT_TABLE_APP = "INSERT INTO APP (NAME, FRIENDLY_NAME, SUMMARY, CATEGORY, WEBSITE, LICENSE, REPO_TYPE, ISSUE_TRACKER, SOURCE) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}');";

        public static string CONNECTION_STRING = "Data Source={0};Version=3;";

        public static string SELECT_ALL_APP = "SELECT * FROM APP";

        public static string GIT_FDROID = "https://gitlab.com/fdroid/fdroiddata.git";

    }
}
