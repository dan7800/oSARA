import java.io.File;
import java.io.IOException;
import java.util.ArrayList;
import java.util.List;
public class SearchGit {

  private String fileNameToSearch;
  private String fileEndsWith;
  private List<String> result = new ArrayList<String>();
  public String getFileNameToSearch() {
	return fileNameToSearch;
  }

  public void setFileNameToSearch(String fileNameToSearch) {
	this.fileNameToSearch = fileNameToSearch;
  }

  public List<String> getResult() {
	return result;
  }

  public List<String> search(String dataset_path,String fileEndsWith) throws IOException {
	this.fileEndsWith = fileEndsWith;  
	  
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
	return result;
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
	    if (file.canRead()) {
		for (File temp : file.listFiles()) {
		    if (temp.isDirectory()) {
		    	String[] files = temp.list();
		    	for(String eachfile: files ) {
		    		if(eachfile.equalsIgnoreCase(fileEndsWith)) {
		    		    result.add(temp.getAbsoluteFile().toString());
		    		}
		    	}
	    	}
		    else {
		    }
		}
	    }
	 } else {
		System.out.println(file.getAbsoluteFile() + "Permission Denied");
	 }
      }

  }
