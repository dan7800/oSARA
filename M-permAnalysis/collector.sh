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


		echo $dirname
		cd $dirname
		#ls

		# Will show all tags
		#git show-ref --tags -d

#git tag
#git show
#git fetch --tags
#git fetch tag
#pwd



		#git show-ref --tags

#git checkout scrumchatter-1.6.2
#git checkout tags/<tag_name>



#git checkout tags/scrumchatter-1.6.2

#git checkout tags/scrumchatter-1.6.2 -f --prefix=/version


#git --work-tree=tags/scrumchatter-1.6.2 checkout HEAD -- version




#mkdir -p ../../$versions/$dirname ## Create the primary output location



#git tag ## Returns list of existing tags
#git --work-tree=versions checkout tags/scrumchatter-1.6.0 -- .



# Loop through all of the created tags (try getting a count)

echo $dirname

exit


for tag in `git tag`; do
  echo "Tag is" $tag

  ## Make a folder for each tag
  outputLoc=../../$versions/$dirname/$tag
  mkdir -p $outputLoc


  ## Output the file there



done



#git tag




		exit
	done


#	cd ..


}




# Function: Get Versions


#	collectRepos
	getReleases




#STR="Hello World!"
#echo $STR    









