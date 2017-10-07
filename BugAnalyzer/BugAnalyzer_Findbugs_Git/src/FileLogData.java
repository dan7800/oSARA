import java.io.*;
import java.util.List;
import org.eclipse.jgit.api.*;
import org.eclipse.jgit.api.errors.GitAPIException;
import org.eclipse.jgit.api.errors.NoHeadException;
import org.eclipse.jgit.internal.storage.file.FileRepository;
import org.eclipse.jgit.lib.Repository;
import org.eclipse.jgit.revwalk.RevCommit;
import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.PreparedStatement;
import java.sql.Statement;

public class FileLogData {
	private Repository localRepo;
	private Git git;
	private String projectName;
	private String db = "jdbc:sqlite:Dataset/gitreport.db";
	private Iterable<RevCommit> logs;
	
public void extractGitFileLog(String filepath) throws IOException, NoHeadException, GitAPIException {
	localRepo = new FileRepository(filepath+"/.git");
	git = new Git(localRepo);
	SearchFiles getListOfFiles = new SearchFiles();
	List<String> listOfFiles = getListOfFiles.search(filepath, ".java");
	for(String file: listOfFiles) {
	logs = git.log().addPath(file).call();
	String[] projectpathstring = filepath.split("/");
    projectName = projectpathstring[projectpathstring.length-1];
    String[] filePathString = file.split("/");
    String sourceFileName = filePathString[filePathString.length-1];
	Connection conn = null;
		try {
		conn = DriverManager.getConnection(db);		
		String st = "CREATE TABLE IF NOT EXISTS GITFileLogReport (\n"
                + "	PROJECT text,\n"
				+ " SOURCEFILE text\n,"
                + "	COMMIT_ID text,\n"
				+ " COMMIT_MSG, \n"
                + " VERSION \n"
                + ");";
		Statement stmt = conn.createStatement();
		stmt.execute(st);
    for (RevCommit rev : logs) {
    	String sql="INSERT INTO GITFileLogReport(PROJECT,SOURCEFILE,COMMIT_ID,COMMIT_MSG) VALUES(?,?,?,?) ";
               PreparedStatement pstmt=conn.prepareStatement(sql);
               pstmt.setString(1,projectName);
               pstmt.setString(2, sourceFileName);
               pstmt.setString(3,rev.getId().getName());
               pstmt.setString(4,rev.getShortMessage());
               pstmt.executeUpdate();
            }  
		} catch (Exception e) {
			e.printStackTrace();
		}	
	}
	}
}