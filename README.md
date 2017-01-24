## oSARA
This tool analyzes the AndroidManifest.xml file of Android code repositories to capture the history of all permission changes.

### Context
The motivation for building this tool was to understand the types of users that make permission changes to apps, how frequently the persmissions are added & reverted and when in the apps commit history the changes are made.

### Setup & Running
The tool was developed as Windows Form Application using Visual Studio 2017. It utilizes <href src='https://f-droid.org/'> F-Droid's </a> collection of open source Android code repositories to perform the analysis on. Presently, the tool targets repositories hosted on GitHub.
