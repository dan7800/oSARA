import java.io.File;
import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.PreparedStatement;
import java.sql.SQLException;
import java.sql.Statement;

import javax.xml.parsers.DocumentBuilder;
import javax.xml.parsers.DocumentBuilderFactory;

import org.w3c.dom.Document;
import org.w3c.dom.Element;
import org.w3c.dom.Node;
import org.w3c.dom.NodeList;

public class XML2DB {

	private String db = "jdbc:sqlite:Dataset/report.db";
	
	public void connect(String xmlfile_path) throws Exception{
		Connection conn = null;
		try{
			conn = DriverManager.getConnection(db);
			
			String st = "CREATE TABLE IF NOT EXISTS DBreport (\n"
	                + "	PROJECT text,\n"
	                + "	BUG_TYPE text,\n"
	                + "	BUG_CATEGORY text,\n"
	                + " SOURCE_LINE text,\n"
	                +" SHORT_MESSAGE \n"
	                + ");";
			Statement stmt = conn.createStatement();
			stmt.execute(st);
			System.out.println("Coonection Established");
	
    

        File xmlFile = new File(xmlfile_path);

        DocumentBuilderFactory dbFactory = DocumentBuilderFactory.newInstance();
        DocumentBuilder dBuilder = dbFactory.newDocumentBuilder();
        Document doc = dBuilder.parse(xmlFile);
        
        
        NodeList nList = doc.getElementsByTagName("BugCollection");
        
        for (int i = 0; i < nList.getLength(); i++) {
            System.out.println("Processing element " + (i+1) + "/" + nList.getLength());
            Node node = nList.item(i);
            
            if (node.getNodeType() == Node.ELEMENT_NODE) {
                Element element = (Element) node;
                String projectJar = element.getElementsByTagName("Project").item(0).getTextContent();
                
                NodeList bugInstance = element.getElementsByTagName("BugInstance");	
                
                for (int j = 0; j < bugInstance.getLength(); j++) {
                    Node bug = bugInstance.item(j);
                    if (bug.getNodeType() == Node.ELEMENT_NODE) {
                        Element currentBug = (Element) bug;
                        String bugType =  currentBug.getAttributes().getNamedItem("type").getNodeValue() ;
                        String bugCategory =  currentBug.getAttributes().getNamedItem("category").getNodeValue();
                        String bugLine =  currentBug.getElementsByTagName("SourceLine").item(0).getTextContent();
                       // String class =  currentBug.getElementsByTagName("Class").item(0).getAttributes().getNamedItem("sourcefile").getTextContent();
                        String sMessage = currentBug.getElementsByTagName("ShortMessage").item(0).getTextContent();
                        String lMessage = currentBug.getElementsByTagName("LongMessage").item(0).getTextContent();
                    
                        String sql="INSERT INTO DBreport(PROJECT,BUG_TYPE,BUG_CATEGORY,SOURCE_LINE,SHORT_MESSAGE) VALUES(?,?,?,?,?) ";
                        
                        PreparedStatement pstmt=conn.prepareStatement(sql);
                        pstmt.setString(1,xmlfile_path);
                        pstmt.setString(2,bugType);
                        pstmt.setString(3,bugCategory);
                        pstmt.setString(4,bugLine);
                        pstmt.setString(5,sMessage);
                        pstmt.executeUpdate();
                    }
                }
            
        }
        System.out.println("getAndReadXml finished, processed " + nList.getLength() + " substances!");
    }		
	}
	catch(SQLException e){
		System.out.println("1 "+e.getMessage());
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
