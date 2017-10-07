import java.io.*;
import java.util.ArrayList;
import java.util.List;

import org.eclipse.jgit.api.*;
import org.eclipse.jgit.api.errors.GitAPIException;
import org.eclipse.jgit.api.errors.NoHeadException;
import org.eclipse.jgit.internal.storage.file.FileRepository;
import org.eclipse.jgit.lib.ObjectId;
import org.eclipse.jgit.lib.Ref;
import org.eclipse.jgit.lib.Repository;
import org.eclipse.jgit.revwalk.RevCommit;


import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.PreparedStatement;
import java.sql.SQLException;
import java.sql.Statement;

public class LogData {
	private Repository localRepo;
	private Git git;
	private List<ObjectId> tagIds;
	private List<Ref> call;
	private List<String> versions;
	private String projectName;
	private String db = "jdbc:sqlite:Dataset/gitreport.db";
	
	
public void extractGit(String filepath) throws IOException, NoHeadException, GitAPIException {
	System.out.println("filepath: "+filepath);
	localRepo = new FileRepository(filepath+"/.git");
	git = new Git(localRepo);
	tagIds = new ArrayList<ObjectId>();
	call = git.tagList().call();
	versions = new ArrayList<String>();
	int count = 0;
	int countlog = 0;
	String[] filepathstring = filepath.split("/");
    projectName = filepathstring[filepathstring.length-1];
	Connection conn = null;
	
		try {
			conn = DriverManager.getConnection(db);
		
		String st = "CREATE TABLE IF NOT EXISTS GITReport (\n"
                + "	PROJECT text,\n"
				+ " VERSION text\n,"
                + "	COMMIT_ID text,\n"
				+ " COMMIT_MSG \n"
                + ");";
		Statement stmt = conn.createStatement();
		stmt.execute(st);
    for (Ref ref : call) {
        count++;
        Ref peeledRef = localRepo.peel(ref);
        if(peeledRef.getPeeledObjectId() != null) {
        	tagIds.add(peeledRef.getPeeledObjectId());
        } else {
        	tagIds.add(ref.getObjectId());
        }
        versions.add(ref.getName().split("/")[ref.getName().split("/").length-1]);
    }
    for (int i=0;i<tagIds.size()-1;i++) {
    	countlog=0;
    	if(i==0) {
    		LogCommand log = new Git(localRepo).log();
        	log.add(tagIds.get(0));
            Iterable<RevCommit> logs = log.call();
            System.out.println("Version: "+versions.get(0));
            for (RevCommit rev : logs) {
            	countlog++;
                String sql="INSERT INTO GITReport(PROJECT,VERSION,COMMIT_ID,COMMIT_MSG) VALUES(?,?,?,?) ";
              
                PreparedStatement pstmt=conn.prepareStatement(sql);
                pstmt.setString(1,projectName);
                pstmt.setString(2, versions.get(0));
                pstmt.setString(3,rev.getId().getName());
                pstmt.setString(4,rev.getShortMessage());
                pstmt.executeUpdate();
            
            
            }
            countlog=0;
            LogCommand newlog = new Git(localRepo).log();
        	newlog.addRange(tagIds.get(i), tagIds.get(i+1));
            Iterable<RevCommit> newlogs = newlog.call();
            	for (RevCommit rev : newlogs) {
            	countlog++;
            	String sql="INSERT INTO GITReport(PROJECT,VERSION,COMMIT_ID,COMMIT_MSG) VALUES(?,?,?,?) ";
                
                PreparedStatement pstmt=conn.prepareStatement(sql);
                pstmt.setString(1,projectName);
                pstmt.setString(2, versions.get(i+1));
                pstmt.setString(3,rev.getId().getName());
                pstmt.setString(4,rev.getShortMessage());
                pstmt.executeUpdate();
            }
            System.out.println("Commits :"+countlog);
    	}else {
    		LogCommand log = new Git(localRepo).log();
    		log.addRange(tagIds.get(i), tagIds.get(i+1));
    		Iterable<RevCommit> logs = log.call();
    		for (RevCommit rev : logs) {
				countlog++;
				String sql="INSERT INTO GITReport(PROJECT,VERSION,COMMIT_ID,COMMIT_MSG) VALUES(?,?,?,?) ";
                
                PreparedStatement pstmt=conn.prepareStatement(sql);
                pstmt.setString(1,projectName);
                pstmt.setString(2, versions.get(i+1));
                pstmt.setString(3,rev.getId().getName());
                pstmt.setString(4,rev.getShortMessage());
                pstmt.executeUpdate();
    		}
    	}
    }
		} catch (SQLException e) {
			e.printStackTrace();
		}
	}	
}