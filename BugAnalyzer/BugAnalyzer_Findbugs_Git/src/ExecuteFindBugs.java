
public class ExecuteFindBugs {
	public String findbugs_path = "java -jar findbugs-3.0.1/lib/findbugs.jar";
	Process process;
	public void executeFindbugs(String jarfile_path) {
	try{
		System.out.println(jarfile_path);
		process = Runtime.getRuntime().exec(findbugs_path+" -textui -xml:withMessages -output "+jarfile_path+"_report.xml "+jarfile_path);
		process.waitFor();
		System.out.println("FindBugs Executed!\n");
	}catch(Exception e) {
		System.out.println(e);
	}
}
}
