#!/bin/bash
declare -i found
declare -i not_found 
found=0
not_found=0
w=$(pwd)
for f in Apps/*
do
	cd $w/$f/app/
	if [ $? -eq 0 ]; then
		if grep 'compile.*' build.gradle| awk 'BEGIN {print "Dependencies" }
		                                               { print $0 }'>>/Users/rohan/Capstone/output.csv
			then 
			found=found+1
			git log --pretty=format:"%h" -- build.gradle | while read -r LINE
			do
				if git show $LINE build.gradle | egrep '^\-\s+compile.*' > /dev/null
					then
					lib=$(git show $LINE build.gradle | egrep '^\-\s+compile.*' | cut -d\' -f2)
					git log -1 --pretty=format:"$lib,$f,%cn,removed,%h,%ad,$found" $LINE build.gradle |  awk '{print $0}'>>/Users/rohan/Capstone/output.csv

					elif git show $LINE build.gradle | egrep '^\+\s+compile.*' > /dev/null
					then
					lib=$(git show $LINE build.gradle | egrep '^\+\s+compile.*' | cut -d\' -f2)
					git log -1 --pretty=format:"$lib,$f,%cn,added,%h,%ad,$found" $LINE build.gradle | awk '{print $0 }'>>/Users/rohan/Capstone/output.csv
				fi
			done
			else echo "Dependencies not found">>/Users/rohan/Capstone/result.txt
			fi
	else   #end of if 
		not_found=not_found+1;
		echo $not_found,$f>>/Users/rohan/Capstone/notfound.csv
	fi    		
done; #end of for
