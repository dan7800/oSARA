import java.io.File;
import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.PreparedStatement;
import java.sql.SQLException;
import java.sql.Statement;
import java.util.Objects;
import javax.xml.parsers.DocumentBuilder;
import javax.xml.parsers.DocumentBuilderFactory;
import org.w3c.dom.Document;
import org.w3c.dom.Element;
import org.w3c.dom.Node;
import org.w3c.dom.NodeList;

public class XML2DB {

	private String db = "jdbc:sqlite:Dataset/gitreport.db";
	
	public void connect(String xmlfile_path) throws Exception{
		Connection conn = null;
		try{
			conn = DriverManager.getConnection(db);
			String st = "CREATE TABLE IF NOT EXISTS FindBugsResult (\n"
	                + "	PROJECT text,\n"
					+ " BUG_INSTANCE_HASH text\n,"
	                + "	BUG_TYPE text,\n"
					+ " BUG_PRIORITY, \n"
	                + "	BUG_CATEGORY text,\n"
	                + " SOURCE_LINE text,\n"
	                + " SHORT_MESSAGE, \n"
	                + " VERSION \n"
	                + ");";
			Statement stmt = conn.createStatement();
			stmt.execute(st);
			
        File xmlFile = new File(xmlfile_path);

        DocumentBuilderFactory dbFactory = DocumentBuilderFactory.newInstance();
        DocumentBuilder dBuilder = dbFactory.newDocumentBuilder();
        Document doc = dBuilder.parse(xmlFile);
        
        String[] jarDir = xmlfile_path.split("/");
        String jarFilePath = jarDir[jarDir.length-1];
        String[] jarFilePathDir = jarFilePath.split("\\_");
        String jarFile = jarFilePathDir[0]+jarFilePathDir[1]; 
        NodeList nList = doc.getElementsByTagName("BugCollection");
        
        for (int i = 0; i < nList.getLength(); i++) {
            Node node = nList.item(i);
            
            if (node.getNodeType() == Node.ELEMENT_NODE) {
                Element element = (Element) node;                
                NodeList bugInstance = element.getElementsByTagName("BugInstance");	
                for (int j = 0; j < bugInstance.getLength(); j++) {
                    Node bug = bugInstance.item(j);
                    if (bug.getNodeType() == Node.ELEMENT_NODE) {
                        Element currentBug 	= 		(Element) bug;
                        String bugType 		=  		currentBug.getAttributes().getNamedItem("type").getNodeValue();
                        String bugPriority	=  		currentBug.getAttributes().getNamedItem("priority").getNodeValue();
                        String instanceHash =  		currentBug.getAttributes().getNamedItem("instanceHash").getNodeValue();
                        String bugCategory 	=  		currentBug.getAttributes().getNamedItem("category").getNodeValue();
                        NodeList sourceFileList	=  	currentBug.getElementsByTagName("Class");
                        String sourceFile 	= 		((Element)sourceFileList.item(0)).getAttributes().getNamedItem("classname").getNodeValue();
                        String sMessage 	= 		currentBug.getElementsByTagName("ShortMessage").item(0).getTextContent();
                        
                        String sourceFileArray[] = sourceFile.split("\\.");
                        String srcFileName = sourceFileArray[sourceFileArray.length-1];
                        if(srcFileName.contains("$")) {
                        	String srcFileNameDemo[] =  srcFileName.split("\\$");
                        	srcFileName = srcFileNameDemo[0];
                        }
                        srcFileName = srcFileName+".java";
                        if(Objects.equals(bugType, new String("NM_CLASS_NAMING_CONVENTION"))){
                        }
                        else if(sourceFile.contains("android")){
                        }
                        else{
                        String sql="INSERT INTO FindBugsResult(PROJECT,BUG_INSTANCE_HASH,BUG_TYPE,BUG_PRIORITY,BUG_CATEGORY,SOURCE_LINE,SHORT_MESSAGE) VALUES(?,?,?,?,?,?,?) ";
                        
                        PreparedStatement pstmt=conn.prepareStatement(sql);
                        pstmt.setString(1,jarFile);
                        pstmt.setString(2,instanceHash);
                        pstmt.setString(3,bugType);
                        pstmt.setString(4,bugPriority);
                        pstmt.setString(5,bugCategory);
                        pstmt.setString(6,srcFileName);
                        pstmt.setString(7,sMessage);
                        pstmt.executeUpdate();
                        }
                    }
                }
            }
        }
        System.out.println("Data inserted for "+jarFile);
	}
	catch(SQLException e){
		e.printStackTrace();
	}
	finally{
		try{
		if(conn!=null){
			conn.close();
		}
		}catch(SQLException e){
			System.out.println(e.getMessage());
		}
	}}
}
