## oSARA
This tool analyzes the AndroidManifest.xml file of Android code repositories to capture the history of all permission changes.

### Context
The motivation for building this tool was to understand the types of users that make permission changes to apps, how frequently the persmissions are added & reverted and when in the apps commit history the changes are made.

### Setup & Running
The tool was developed as Windows Form Application using Visual Studio 2017. It utilizes <a href='https://f-droid.org/' target='_blank'> F-Droid's </a> collection of open source Android code repositories to perform the analysis on. Presently, the tool targets repositories hosted on GitHub.

To run the tool, either:
<ol>
	<li>
		clone/download the GitHub repository. Open the solution in Visual Studio and build the project
	</li>
	<li>
		extract the files from the executable.zip archive and run 'AndroidCodeAnalyzer.exe'
	</li>
</ol>

It is recommend that the following sequence be followed when running the tool:
<ol>
	<li> Download F-Droid Repository </li>
	<li> Download Android Projects </li>
	<li> Get Project Commit History </li>
	<li> Get Manifest History </li>
	<li> Process Author Permissions </li>s
	<li> Project Permissions </li>
</ol>

#### Screens

##### Main/Home Screen
The following is the main/home screen of the tool. From this screen, the user can perform the required download/processing tasks.<br/>
<img src='http://imgur.com/wzGt7xf.png'/><br/>

##### Download F-Droid Repository
The following screen is utilized to download the F-Droid repository. The download location needs to be procreated and provided prior to starting the process. The database will also be created in this location.<br/>
<img src='http://imgur.com/BMaYo30.png'/><br/>
<img src='http://imgur.com/wEJxQeA.png' width='70%' height='70%'/><br/>




