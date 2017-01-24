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

It is recommend that the following sequence be followed:
<ol>
	<li> Download F-Droid Repository </li>
	<li> Download Android Projects </li>
	<li> Get Project Commit History </li>
	<li> Get Manifest History </li>
	<li> Process Author Permissions </li>
	<li> Project Permissions </li>
</ol>

#### Screens
