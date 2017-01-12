using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndroidCodeAnalyzer
{
    class Constants
    {
        public static string COLUMN_APP_ID = "ID";
        public static string COLUMN_APP_NAME = "NAME";
        public static string COLUMN_APP_FRIENDLY_NAME = "FRIENDLY_NAME";
        public static string COLUMN_APP_SUMMARY = "SUMMARY";
        public static string COLUMN_APP_CATEGORY = "CATEGORY";
        public static string COLUMN_APP_WEBSITE = "WEBSITE";
        public static string COLUMN_APP_LICENSE = "LICENSE";
        public static string COLUMN_APP_REPO_TYPE = "REPO_TYPE";
        public static string COLUMN_APP_ISSUE_TRACKER = "ISSUE_TRACKER";
        public static string COLUMN_APP_SOURCE = "SOURCE";



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

        public static string CREATE_TABLE_COMMIT_LOG = "CREATE TABLE COMMIT_LOG(" +
            "ID INTEGER PRIMARY KEY AUTOINCREMENT," +
            "GUID TEXT NOT NULL," +
            "MESSAGE TEXT," +
            "AUTHOR_NAME TEXT," +
            "AUTHOR_EMAIL TEXT," +
            "DATE_TEXT TEXT," +
            "DATE_TICKS REAL," +
            "APPID INTEGER NOT NULL" +
            ");";

        public static string CREATE_TABLE_COMMIT_LOG_FILE = "CREATE TABLE COMMIT_LOG_FILE(" +
            "ID INTEGER PRIMARY KEY AUTOINCREMENT," +
            "PATH TEXT," +
            "OPERATION TEXT," +
            "COMMITID INTEGER NOT NULL," +
            "APPID INTEGER NOT NULL" +
            ");";

        public static string CREATE_TABLE_APP_CLONE = "CREATE TABLE APP_CLONE(" +
            "ID INTEGER PRIMARY KEY AUTOINCREMENT," +
            "APPID INTEGER NOT NULL," +
            "LAST_DOWNLOAD_DATE TEXT," +
            "LAST_DOWNLOAD_DATE_TICKS REAL" +
            ");";


        public static string INSERT_TABLE_APP = "INSERT INTO APP (NAME, FRIENDLY_NAME, SUMMARY, CATEGORY, WEBSITE, LICENSE, REPO_TYPE, ISSUE_TRACKER, SOURCE) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}');";

        public static string INSERT_TABLE_COMMIT_LOG = "INSERT INTO COMMIT_LOG (GUID, MESSAGE, AUTHOR_NAME, AUTHOR_EMAIL, DATE_TEXT, DATE_TICKS, APPID) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', {5}, {6});";

        public static string INSERT_TABLE_COMMIT_LOG_FILE = "INSERT INTO COMMIT_LOG_FILE (PATH, OPERATION, COMMITID, APPID) VALUES ('{0}', '{1}', {2}, {3});";

        public static string CONNECTION_STRING = "Data Source={0};Version=3;";

        public static string UPSERT_TABLE_APP_CLONE = "INSERT OR REPLACE INTO APP_CLONE (APPID, LAST_DOWNLOAD_DATE, LAST_DOWNLOAD_DATE_TICKS, ID) " +
                "VALUES (  " +
                "{0}, " +
                "'{1}', " +
                "{2}, " +
                "(SELECT ID FROM APP_CLONE WHERE APPID = {0})" +
                ");";

        public static string SELECT_ALL_APP = "SELECT * FROM APP WHERE REPO_TYPE='git' AND SOURCE LIKE 'https://github.com/%' ";

        public static string GIT_FDROID = "https://gitlab.com/fdroid/fdroiddata.git";

    }
}
