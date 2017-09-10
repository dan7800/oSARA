import java.io.IOException;

public class Bug_Analysis_Main {

	private static String dataset_path="Dataset/Files";
	
	
	public static void main(String[] args) throws IOException {
		
		// Prepare Dataset for evaluation
		// Extract source code from .tar and run Dex2Jar on apk files to work on FindBugs
		//SearchApkTar searchApkTar = new SearchApkTar();
		//searchApkTar.search(dataset_path);
		
		//Search Jar files to run findbugs. Reports are generated in XML format
		SearchJar searchJar = new SearchJar();
		searchJar.search(dataset_path);
		
		//Search finbugs xml reports and add those data into database
		SearchXML searchXML = new SearchXML();
		searchXML.search(dataset_path);		
	}

}
