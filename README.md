## oSARA
This tool analyzes the AndroidManifest.xml file associated with Android code repositories to capture the history of all permission changes.

### Context
The motivation for building this tool was to understand the types of users that make permission changes to apps, how frequently the persmissions are added & reverted and when in the app's commit history the changes are made.

### Setup & Running
The tool was developed as a Windows Form Application using Visual Studio 2017. It utilizes <a href='https://f-droid.org/' target='_blank'> F-Droid</a>'s  collection of open source Android code repositories to perform the analysis on. Presently, the tool targets repositories hosted on GitHub.

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
	<li> <a href='#download-f-droid-repository'>Download F-Droid Repository</a></li>
	<li> <a href='#download-android-project-repositories'>Download Android Projects</a></li>
	<li> <a href='#get-android-project-commit-history-log'>Get Project Commit History</a></li>
	<li> <a href='#get-manifest-history'>Get Manifest History </a></li>
	<li> <a href='#process-author-rating'>Process Author Permissions</a></li>
	<li> <a href='#process-permissions'>Process Permissions</a></li>
</ol>

#### Screens

##### Main/Home Screen
The following is the main/home screen of the tool. From this screen, the user can perform the required download/processing tasks.
<table>
	<tr>
		<td>
			<img src='http://imgur.com/wzGt7xf.png'/>
		</td>
	</tr>
</table>

##### Download F-Droid Repository
The following screen is utilized to download the F-Droid repository. The download location needs to be procreated and provided prior to starting the process. The database will also be created in this location.
<table>
	<tr>
		<td>
			<img src='http://imgur.com/BMaYo30.png'/>
		</td>
		<td>
			<img src='http://imgur.com/wEJxQeA.png'/>
		</td>
	</tr>
</table>

##### Download Android Project Repositories
The following screen is utilized to download the Android project repositories. The download location needs to be procreated and provided prior to starting the process. Database location should point to the 'database.sqlite' file created in the previous step.
<table>
	<tr>
		<td>
			<img src='http://imgur.com/kpw4qBr.png'/>
		</td>
		<td>
			<img src='http://imgur.com/snmheZU.png'/>
		</td>
	</tr>
</table>

##### Get Android Project Commit History Log
The following screen is utilized to download the commit history for the Android projects. The database location should point to the 'database.sqlite' file and the download location should point to same location entered in the previous step.
<table>
	<tr>
		<td>
			<img src='http://imgur.com/iKHTQWd.png'/>
		</td>
	</tr>
</table>

##### Get Manifest History
The following screen is utilized to download manifest file history of the Android project. The database location should point to the 'database.sqlite' file and the download location should point to same location entered in the previous step.
<table>
	<tr>
		<td>
			<img src='http://imgur.com/CTr6Jny.png'/>
		</td>
	</tr>
</table>

##### Process Author Rating
The following screen is utilized to calculate the author rating (i.e. Developer Commit Ratio). The tool outputs the data into a CSV file.
The database location should point to the 'database.sqlite' file. The CVS output location should be created prior to running the tool (delete the csv file if already exists in the location).
<table>
	<tr>
		<td>
			<img src='http://imgur.com/8rc1w3w.png'/>
		</td>
		<td>
			<img src='http://imgur.com/zqTSUDP.png'/>
		</td>
	</tr>
</table>

##### Process Permissions
The following screen is utilized to provide a Add/Remove history of the permissions associated with an app. The tool outputs the data into a CSV file.
The database location should point to the 'database.sqlite' file. The CVS output location should be created prior to running the tool (delete the csv file if already exists in the location).
<table>
	<tr>
		<td>
			<img src='http://imgur.com/Ln3BMpV.png'/>
		</td>
		<td>
			<img src='http://imgur.com/ZD7UFRl.png'/>
		</td>
	</tr>
</table>





