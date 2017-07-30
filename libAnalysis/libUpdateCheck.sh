#!/bin/bash 

### Description: Loop through the oSARA library data set to determine if a library dependency is new, or is merely an update



# db=../data/database-Win.sqlite
db="/Users/imagine/Desktop/RCode/data/database-Win.sqlite"

clear;


### Remove all numbers from String
### Eliminate all version info
function cleanString {
	strVal=$1
	IFS=0123456789
	set -f # Disable glob

	strVal=${strVal//./}  # Remove .
	strVal=${strVal//:/}  # Remove :


	echo $strVal
}



##  Function to determine update type.
##	Only check the String
function updateTypeStringMatch { 

	str_orig=`cleanString $1`
	str_update=`cleanString $2`


	if [ $str_orig == $str_update ]
		then
		echo "update"  # Same
	else
		echo "new" # Diff
	fi



}


## Loop through all of the updates to determine the name of the dependency and app to see if the new dependcy is like the old one


## Adds - See if a removed dependency had a similar name



## Remove: Does the next commit have a similar dependency


#echo $db

#test=`sqlite3 db  "SELECT count(*) FROM from dcl_1;"`
#test=`sqlite3 database  "SELECT count(*) FROM table;"`
#test=`sqlite3 $db  "SELECT count(*) FROM dcl_1;"`

#echo $test





######### Tests ##########

### Removing all numbers

#newVal=`removeAllNumbers DaAAAAAn123`
#echo DanAAA $newVal



## Test the String matching function


updateTypeStringMatch "com.anthonycr.grant:permissions:1.0" "com.anthonycr.grant:permissions:1.1.2"


## Remove all numbers


## Not Similar
#updateTypeStringMatch dan smit


## Similar


# ":libnetcipher" - Same
#"com.android.support:appcompat-v7:23.0.1" "com.android.support:appcompat-v7:23.1.0"


#echo "Script Complete"
