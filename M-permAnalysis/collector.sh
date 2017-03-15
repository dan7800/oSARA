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




### Build the necessary folders/structure

	mkdir -p $ghRepos




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


	mkdir $appName
	cd $appName		
	git clone $p

	cd ..
	cd ..

	done <$ghLinks
}


# loop through all of the out put files to get all releases
function getReleases {


# Only keep releases if they have the proper API
# If no releases, just get the latest version


# Loop through all of the output files





	echo "Get Rel"
}




# Function: Get Versions


#	collectRepos
	getReleases




#STR="Hello World!"
#echo $STR    



            