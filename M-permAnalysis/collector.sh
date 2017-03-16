#!/bin/bash          

## Description: Collects and


#	1) Get list of repos to clone
#	2) Loop through cloning the repos
#	3) Get all versions for these apps
#	4) Run M-Perm on these verions
#	5) Determine who added the permissions-based problems



clear;

### Location of Input file	
#	ghLinks=links.txt
	ghLinks=links_sm.txt
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


# Loop through all of the output files
#	cd $ghRepos
#	for d in $ghRepos; do
#	    echo "$d"
#	done


	for i in $(ls -d $ghRepos/*); do 

		dirname=${i%%/}; 


		# Get all releases for app

		appName=${dirname/$ghRepos/} 
		cd $dirname


		# Loop through all of the created tags (try getting a count)
		COUNTER=0
		for tag in `git tag`; do

		  ## Make a folder for each tag
		  	outputLoc=../../$versions/$appName/$tag
		  	mkdir -p $outputLoc

			git checkout $tag
			git --work-tree=$outputLoc checkout HEAD -- .
		 	let COUNTER=COUNTER+1 # Determine how many versions were found
		done


		## Make sure the versions being collected are actually different
		## Check to make sure each of these versions can be ran  by M-Perm




		echo $COUNTER

		# If the count is 0 (no tags) then output the current version of the project to the directory




		exit
	done


#	cd ..


}




# Function: Get Versions


#	collectRepos
	getReleases




#STR="Hello World!"
#echo $STR    









