import java.util.List;
import org.eclipse.jgit.api.errors.GitAPIException;
import org.eclipse.jgit.api.errors.NoHeadException;

public class Bug_Analysis_Main{
	private static String dataset_path_findbugsjar="Dataset/Findbugs";
	private static String dataset_path_git="Dataset/Git";
	
	public static void main(String[] args) throws Exception {
		List<String> JARSearchResult;
		List<String> GITSearchResult;
		List<String> XMLSearchResult;
		LogData logData;
		XML2DB xml2db;
		ExecuteFindBugs execFindBugs;
		SearchFiles searchFile = new SearchFiles();
		
		////-----------------------------------------------------------////
		// Prepare Dataset for evaluation
		// Extract source code from .tar and run Dex2Jar on apk files to work on FindBugs
		/*SearchApkTar searchApkTar = new SearchApkTar();
		searchApkTar.search(dataset_path_findbugsjar);*/
		
		////-----------------------------------------------------------////
		//Search Jar files to run findbugs. Reports are generated in XML format
		/*JARSearchResult = searchFile.search(dataset_path_findbugsjar,"_findbugs.jar");
		execFindBugs = new ExecuteFindBugs();
		for(String file: JARSearchResult){
		execFindBugs.executeFindbugs(file);
		}
		System.out.println("Findbugs execution done!!");
		*/
		
		
		
		////-----------------------------------------------------------////
		//Search finbugs xml reports and add those data into database
		
		XMLSearchResult = searchFile.search(dataset_path_findbugsjar,"_report.xml");	
		xml2db = new XML2DB();
		for(String file: XMLSearchResult) {
			try{
		    	xml2db.connect(file);
		    }catch(Exception e) {
		    	e.printStackTrace();
		    }
		}
		System.out.println("Data inserted into DB");
		
		
		////-----------------------------------------------------------////
		/*SearchGit searchGit = new SearchGit();
		searchGit.search(dataset_path_git);
		GITSearchResult = searchGit.search(dataset_path_git, ".git");
		for(String file : GITSearchResult){
			logData = new LogData();
			try {
				logData.extractGit(file);
				} catch (NoHeadException e) {
					e.printStackTrace();
				} catch (GitAPIException e) {
					e.printStackTrace();
				}
		}
		for(String file : GITSearchResult) {
			FileLogData fileLog = new FileLogData();
			fileLog.extractGitFileLog(file);
		}
		*/
		}
}
