import java.io.File;
import java.io.IOException;
import java.util.ArrayList;
import java.util.List;

public class SearchXML {


	
  private String fileNameToSearch;
  private List<String> result = new ArrayList<String>();

  XML2DB xml2db;
  public String getFileNameToSearch() {
	return fileNameToSearch;
  }

  public void setFileNameToSearch(String fileNameToSearch) {
	this.fileNameToSearch = fileNameToSearch;
  }

  public List<String> getResult() {
	return result;
  }

  public void search(String dataset_path) throws IOException {
	  
	  xml2db = new XML2DB();
	  
	searchDirectory(new File(dataset_path));

	int count = getResult().size();
	if(count ==0){
	    System.out.println("\nNo result found!");
	}else{
	    System.out.println("\nFound " + count + " result!\n");
	    for (String matched : getResult()){
		System.out.println("Found : " + matched);
	    }
	}
  }
  public void searchDirectory(File directory) throws IOException {
	if (directory.isDirectory()) {
		
		search(directory);
		
	} else {
	    System.out.println(directory.getAbsoluteFile() + " is not a directory!");
	}
  }

  private void search(File file) throws IOException {
	if (file.isDirectory()) {
            //do you have permission to read this directory?
	    if (file.canRead()) {
		for (File temp : file.listFiles()) {
		    if (temp.isDirectory()) {
			search(temp);
		    } 
		    else {
			if ((temp.getName().toLowerCase().endsWith("_report.xml"))) {
			    result.add(temp.getAbsoluteFile().toString());
			    try{
			    	xml2db.connect(temp.getAbsolutePath());
			    }catch(Exception e) {
			    	e.printStackTrace();
		    }
		}}
	    }

	 } else {
		System.out.println(file.getAbsoluteFile() + "Permission Denied");
	 }
      }

  }

}