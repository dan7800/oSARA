import java.io.File;
import java.io.IOException;
import java.util.ArrayList;
import java.util.List;

public class SearchApkTar {
	private  String dex2jar_file_path="Dex2Jar/dex2jar-2.0/d2j-dex2jar.sh";
	private String fileNameToSearch;
	private List<String> result = new ArrayList<String>();
	PrepareDataset pdataset;
	
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
	  
	pdataset = new PrepareDataset();  
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
	  System.out.println("Searching directory ... " + file.getAbsoluteFile());
            //do you have permission to read this directory?
	    if (file.canRead()) {
		for (File temp : file.listFiles()) {
		    if (temp.isDirectory()) {
			search(temp);
		    } 
		    else {
			if ((temp.getName().toLowerCase().endsWith(".apk"))) {
			    result.add(temp.getAbsoluteFile().toString());
			    pdataset.dex2jar(dex2jar_file_path, temp.getAbsolutePath());
		    }
			if ((temp.getName().toLowerCase().endsWith(".tar.gz"))) {
			    result.add(temp.getAbsoluteFile().toString());
			    pdataset.extractTar(temp.getAbsolutePath(),temp.getParent());
		    	}
		    }
	    }
	 } else {
		System.out.println(file.getAbsoluteFile() + "Permission Denied");
	 	}
     }
  }
}