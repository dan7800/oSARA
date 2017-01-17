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

        public static string COLUMN_COMMIT_LOG_ID = "ID";
        public static string COLUMN_COMMIT_LOG_GUID = "GUID";
        public static string COLUMN_COMMIT_LOG_MESSAGE = "MESSAGE";
        public static string COLUMN_COMMIT_LOG_AUTHOR_NAME = "AUTHOR_NAME";
        public static string COLUMN_COMMIT_LOG_AUTHOR_EMAIL = "AUTHOR_EMAIL";
        public static string COLUMN_COMMIT_LOG_DATE_TEXT = "DATE_TEXT";
        public static string COLUMN_COMMIT_LOG_DATE_TICKS = "DATE_TICKS";
        public static string COLUMN_COMMIT_LOG_APPID = "APPID";

        public static string COLUMN_MANIFEST_PERMISSION_ID = "ID";
        public static string COLUMN_MANIFEST_PERMISSION_APPID = "APPID";
        public static string COLUMN_MANIFEST_PERMISSION_COMMIT_GUID = "COMMIT_GUID";
        public static string COLUMN_MANIFEST_PERMISSION_COMMITID = "COMMITID";
        public static string COLUMN_MANIFEST_PERMISSION_PERMISSION = "PERMISSION";
        public static string COLUMN_MANIFEST_PERMISSION_AUTHOR_NAME = "AUTHOR_NAME";
        public static string COLUMN_MANIFEST_PERMISSION_AUTHOR_EMAIL = "AUTHOR_EMAIL";
        public static string COLUMN_MANIFEST_PERMISSION_DATE_TICKS = "DATE_TICKS";


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
            "ID INTEGER PRIMARY KEY AUTOINCREMENT, " +
            "PATH TEXT, " +
            "OPERATION TEXT, " +
            "COMMIT_GUID TEXT, " +
            "COMMITID INTEGER NOT NULL, " +
            "APPID INTEGER NOT NULL" +
            ");";

        public static string CREATE_TABLE_APP_CLONE = "CREATE TABLE APP_CLONE(" +
            "ID INTEGER PRIMARY KEY AUTOINCREMENT," +
            "APPID INTEGER NOT NULL," +
            "LAST_DOWNLOAD_DATE TEXT," +
            "LAST_DOWNLOAD_DATE_TICKS REAL" +
            ");";

        public static string CREATE_TABLE_MANIFEST = "CREATE TABLE MANIFEST(" +
            "ID INTEGER PRIMARY KEY AUTOINCREMENT," +
            "APPID INTEGER NOT NULL," +
            "COMMIT_GUID TEXT, " +
            "COMMITID INTEGER NOT NULL, " +
            "CONTENT TEXT, " +
            "AUTHOR_NAME TEXT, " +
            "AUTHOR_EMAIL, " +
            "DATE_TEXT TEXT," +
            "DATE_TICKS REAL " + 
            ");";

        public static string CREATE_TABLE_MANIFEST_PERMISSION = "CREATE TABLE MANIFEST_PERMISSION(" +
            "ID INTEGER PRIMARY KEY AUTOINCREMENT," +
            "APPID INTEGER NOT NULL," +
            "COMMIT_GUID TEXT, " +
            "COMMITID INTEGER NOT NULL, " +
            "PERMISSION TEXT, " +
            "AUTHOR_NAME TEXT, " +
            "AUTHOR_EMAIL, " +
            "DATE_TEXT TEXT," +
            "DATE_TICKS REAL " +
            ");";

        public static string CREATE_TABLE_MANIFEST_SDK = "CREATE TABLE MANIFEST_SDK(" +
            "ID INTEGER PRIMARY KEY AUTOINCREMENT," +
            "APPID INTEGER NOT NULL," +
            "COMMIT_GUID TEXT, " +
            "COMMITID INTEGER NOT NULL, " +
            "MIN_SDK INTEGER, " +
            "TARGET_SDK INTEGER, " +
            "AUTHOR_NAME TEXT, " +
            "AUTHOR_EMAIL, " +
            "DATE_TEXT TEXT," +
            "DATE_TICKS REAL " +
            ");";


        public static string INSERT_TABLE_MANIFEST = "INSERT INTO MANIFEST (APPID, COMMIT_GUID, COMMITID, CONTENT, AUTHOR_NAME, AUTHOR_EMAIL, DATE_TEXT, DATE_TICKS) VALUES ({0}, '{1}', {2}, '{3}', '{4}', '{5}', '{6}', {7});";

        public static string INSERT_TABLE_MANIFEST_PERMISSION = "INSERT INTO MANIFEST_PERMISSION (APPID, COMMIT_GUID, COMMITID, PERMISSION, AUTHOR_NAME, AUTHOR_EMAIL, DATE_TEXT, DATE_TICKS) VALUES ({0}, '{1}', {2}, '{3}', '{4}', '{5}', '{6}', {7});";

        public static string INSERT_TABLE_MANIFEST_SDK = "INSERT INTO MANIFEST_SDK (APPID, COMMIT_GUID, COMMITID, MIN_SDK, TARGET_SDK, AUTHOR_NAME, AUTHOR_EMAIL, DATE_TEXT, DATE_TICKS) VALUES ({0}, '{1}', {2}, {3}, {4}, '{5}', '{6}', '{7}', {8});";

        public static string INSERT_TABLE_APP = "INSERT INTO APP (NAME, FRIENDLY_NAME, SUMMARY, CATEGORY, WEBSITE, LICENSE, REPO_TYPE, ISSUE_TRACKER, SOURCE) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}');";

        public static string INSERT_TABLE_COMMIT_LOG = "INSERT INTO COMMIT_LOG (GUID, MESSAGE, AUTHOR_NAME, AUTHOR_EMAIL, DATE_TEXT, DATE_TICKS, APPID) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', {5}, {6}); SELECT last_insert_rowid();";

        public static string INSERT_TABLE_COMMIT_LOG_FILE = "INSERT INTO COMMIT_LOG_FILE (PATH, OPERATION, COMMIT_GUID, COMMITID, APPID) VALUES ('{0}', '{1}', '{2}', {3}, {4});";

        public static string CONNECTION_STRING = "Data Source={0};Version=3;";

        public static string UPSERT_TABLE_APP_CLONE = "INSERT OR REPLACE INTO APP_CLONE (APPID, LAST_DOWNLOAD_DATE, LAST_DOWNLOAD_DATE_TICKS, ID) " +
                "VALUES (  " +
                "{0}, " +
                "'{1}', " +
                "{2}, " +
                "(SELECT ID FROM APP_CLONE WHERE APPID = {0})" +
                ");";

        public static string SELECT_ALL_APP = "SELECT * FROM APP WHERE REPO_TYPE='git' AND SOURCE LIKE 'https://github.com/%' ";

        public static string SELECT_APP = "SELECT * FROM APP WHERE NAME='{0}' ";

        public static string SELECT_COMMIT = "SELECT * FROM COMMIT_LOG WHERE APPID={0} AND GUID='{1}' ";

        public static string SELECT_ALL_COMMIT = "SELECT * FROM COMMIT_LOG";

        public static string SELECT_ALL_PERMISSION_HISTORY = "SELECT * FROM MANIFEST_PERMISSION";

        public static string GIT_FDROID = "https://gitlab.com/fdroid/fdroiddata.git";




        /*
        Query to get the most recent date      
   Select u.APPID, u.DATE_TICKS, u.DATE_TEXT, u.GUID
   From COMMIT_LOG as u
   Inner Join (
       Select COMMIT_LOG.APPID
             ,max(COMMIT_LOG.DATE_TICKS) as [DATE_TICKS]
       From COMMIT_LOG
       Group By COMMIT_LOG.APPID) As [q]
   On u.APPID = q.APPID
   And u.DATE_TICKS = q.DATE_TICKS

       */


        /*

        SELECT
  ProcessedPermissions.APPID AS APP_ID,
  APP.FRIENDLY_NAME AS APP_NAME,
  ProcessedPermissions.AUTHOR_NAME AS AUTHOR_NAME,
  ProcessedPermissions.AUTHOR_EMAIL AS AUTHOR_EMAIL,
  ProcessedAuthorRank.PERCENT_COMMIT AS AUTHOR_PERCENT,
  ProcessedPermissions.PERMISSION AS PERMISSION,
  ProcessedPermissions.ACTION AS ACTION,
  ProcessedPermissions.DATE_TEXT AS COMMIT_DATE_TEXT,
  ProcessedPermissions.DATE_TICKS AS COMMIT_DATE_TICKS,
  ProcessedPermissions.COMMIT_GUID AS COMMIT_GUID,
  COMMIT_LOG.MESSAGE AS COMMIT_MESSAGE
FROM ProcessedPermissions
INNER JOIN ProcessedAuthorRank ON (
    ProcessedAuthorRank.AUTHOR_EMAIL = ProcessedPermissions.AUTHOR_EMAIL AND
    ProcessedAuthorRank.AUTHOR_NAME = ProcessedPermissions.AUTHOR_NAME AND
    ProcessedAuthorRank.APPID = ProcessedPermissions.APPID)
INNER JOIN APP ON (
    APP.ID = ProcessedPermissions.APPID
    )
INNER JOIN COMMIT_LOG ON (
    COMMIT_LOG.GUID = ProcessedPermissions.COMMIT_GUID AND
    COMMIT_LOG.APPID = ProcessedPermissions.APPID
    )
ORDER BY APP_ID, COMMIT_DATE_TICKS


-------------------------------------------------
CREATE VIEW view_Apps_isDangerous AS
SELECT
  ProcessedPermissionsAuthors.APP_ID,
  ProcessedPermissionsAuthors.APP_NAME,
  ProcessedPermissionsAuthors.AUTHOR_NAME,
  ProcessedPermissionsAuthors.AUTHOR_EMAIL,
  ProcessedPermissionsAuthors.AUTHOR_PERCENT,
  ProcessedPermissionsAuthors.PERMISSION,
  DangerousPermission.isDangerous,
  ProcessedPermissionsAuthors.ACTION,
  ProcessedPermissionsAuthors.COMMIT_DATE_TEXT,
  ProcessedPermissionsAuthors.COMMIT_DATE_TICKS,
  ProcessedPermissionsAuthors.COMMIT_GUID,
  ProcessedPermissionsAuthors.COMMIT_MESSAGE
FROM ProcessedPermissionsAuthors
LEFT JOIN DangerousPermission ON (
    DangerousPermission.PERMISSION = ProcessedPermissionsAuthors.PERMISSION
    )
ORDER BY APP_ID, COMMIT_DATE_TICKS

         */

    }
}
