#!/bin/bash          

## Description: Collects and


#	1) Get list of repos to clone
#	2) Loop through cloning the repos
#	3) Get all versions for these apps
#	4) Run M-Perm on these verions
#	5) Determine who added the permissions-based problems


clear;

### Location of Input file	
	ghLinks=links.txt
#	ghLinks=links_sm.txt
#	ghLinks=links_1.txt


### CollectedRepos
	ghRepos=Collectedrepos


### Location of all versions
	versions=versions


### Build the necessary folders/structure

	mkdir -p $ghRepos
	mkdir -p $versions


# Function: Collect Repos
function collectRepos {
	while read p; do
	#	  echo $p

	# Get the foldername for the app
	appName=${p/https:\/\/github.com/} 
	appName=`sed -e 's#.*/\(\)#\1#' <<< "$appName"`
	appName=${appName/.git/} 

	echo $appName
    cd $ghRepos 
	git clone $p
	cd ..

	done <$ghLinks
}


# loop through all of the out put files to get all releases
function getReleases {


# Only keep releases if they have the proper API
# If no releases, just get the latest version


###	A dirty hack. I left this in so that it would move up a directory if it was already encountered
	Run="False"

	for i in $(ls -d $ghRepos/*); do 

		dirname=${i%%/}; 
	
		appName=${dirname/$ghRepos/} 


		if [ "$Run" == "True" ] # start of a dirty hack
			then
				cd ../../$dirname
				#exit
		else
			cd $dirname
		fi


		Run="True" # The script has already been run, so this will influence what directory is encountered

	
		# Loop through all of the created tags (try getting a count)
		COUNTER=0
		for tag in `git tag`; do

		  ## Make a folder for each tag
		  	outputLoc=../../$versions/$appName/$tag
		  	mkdir -p $outputLoc

			git checkout $tag
			git --work-tree=$outputLoc checkout HEAD -- .
		 	let COUNTER=COUNTER+1 # Determine how many versions were found

		 	#### Check to see if the manifest is 


		 	# Find the manifest file
			loc=`find $outputLoc -name "AndroidManifest.xml";`

			versionInfo=`grep "android:targetSdkVersion=" $loc` 
			### Replace extra chars
			versionInfo=${versionInfo/android:targetSdkVersion=/} 
			versionInfo=${versionInfo/\"/} 
			versionInfo=${versionInfo/\"/} 
			versionInfo=${versionInfo/\/>/} 

			echo $versionInfo

			if [ "$versionInfo" -lt 23 ] 
			then
				# Not the correct API version, so delete the folder
				echo "Delete file"
				rm -r $outputLoc
			fi

		done


		# If the count is 0 (no tags) then output the current version of the project to the directory
		if [ "$COUNTER" -eq 0 ] 
		then
			## Since no versions exist, merely copy over the file contents of the entire project
		pwd
			echo "No versions found"	
			mkdir -p ../../$versions/$appName/SingleVersion
			cp -r .  ../../$versions/$appName/SingleVersion # Copy the only version to the primary directory

		fi
	done


}



#	collectRepos  ## Gather all of the repositories
	getReleases	  ## Get all of the releases from the collected repos
 

## Todo:
#	Check to make sure an appropriate manifest file was found prior to deleting the versions

