public class PrepareDataset {
	String s;
	Process process;
	
public void dex2jar(String dex2jar_path, String apk_file_path) {
	try {
		process = Runtime.getRuntime().exec(dex2jar_path+" "+apk_file_path+" -o "+apk_file_path+"_findbugs.jar");
	    process.waitFor();
	    process.destroy();		
		System.out.println("Jar Generated");
	}
	catch(Exception e) {
		System.out.print(e);
	}
}
	
public void extractTar(String tar_file,String directory) {
	try {
		process = Runtime.getRuntime().exec("tar -xzf "+tar_file+" -C "+directory);
		process.waitFor();
	    process.destroy();	
		System.out.println("Tar Extracted");
	}catch(Exception e) {
		System.out.print(e);
	}
	}
}
