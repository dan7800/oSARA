#!/bin/bash 

### Description: Loop through the oSARA library data set to determine if a library dependency is new, or is merely an update



# db=../data/database-Win.sqlite
db="/Users/imagine/Desktop/RCode/data/database-Win.sqlite"

clear;


### Remove all numbers from String
### Eliminate all version info
function cleanString {

	strVal=$1
	strVal=${strVal//./}  # Remove .
	strVal=${strVal//:/}  # Remove :
	strVal=${strVal//@/}  # Remove @
	strVal=${strVal//[0-9]/}

	echo $strVal
}



##  Function to determine update type.
##	Only check the String
function updateTypeStringMatch { 

	str_orig=`cleanString $1`
	str_update=`cleanString $2`


	#echo $str_orig
	#echo $str_update


#	str_orig=$1
#	str_orig=${str_orig//./}  # Remove .
#	str_orig=${str_orig//:/}  # Remove :
#	str_orig=${str_orig//@/}  # Remove @
#	str_orig=${str_orig//[0-9]/}

#	str_update=$2
#	str_update=${str_update//./}  # Remove .
#	str_update=${str_update//:/}  # Remove :
#	str_update=${str_update//@/}  # Remove @
#	str_update=${str_update//[0-9]/}



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




#cleanString "net.i2p.android:client:0.7"


## Test the String matching function


updateTypeStringMatch "com.anthonycr.grant:permissions:1.0" "com.anthonycr.grant:permissions:1.1.2"
updateTypeStringMatch "com.google.dagger:dagger:2.0.1" "com.google.dagger:dagger:2.0.1"
updateTypeStringMatch "net.i2p.android:client:0.6@aar" "net.i2p.android:client:0.7" ## will cause problems

updateTypeStringMatch "org.jsoup:jsoup:1.9.1" "org.jsoup:jsoup:1.9.2"
updateTypeStringMatch "com.squareup:otto:1.3.8" "libs/jsoup-1.8.1.jar"


## Remove all numbers


## Not Similar
#updateTypeStringMatch dan smit


## Similar


# ":libnetcipher" - Same
#"com.android.support:appcompat-v7:23.0.1" "com.android.support:appcompat-v7:23.1.0"


#echo "Script Complete"
