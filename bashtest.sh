#!/bin/bash
w=$(pwd)
for f in apps/*
do
cd $w/$f/app/
if grep 'compile.*' build.gradle 
then 
echo “Dependencies Found”
git log --pretty=format:"%h" -- build.gradle | while read -r LINE
do
if git show $LINE build.gradle | egrep '^\-\s+compile.*' > /dev/null
then
lib=$(git show $LINE build.gradle | egrep '^\-\s+compile.*' | cut -d\' -f2)
git log -1 --pretty=format:"$f %cn removed %h $lib on %cd" $LINE build.gradle |  awk '{print $0}'>>/Users/rohan/out.csv

elif git show $LINE build.gradle | egrep '^\+\s+compile.*' > /dev/null
then
lib=$(git show $LINE build.gradle | egrep '^\+\s+compile.*' | cut -d\' -f2)
git log -1 --pretty=format:"$f %cn added %h $lib on %cd" $LINE build.gradle | awk '{print $0}'>>/Users/rohan/out.csv
fi
done

else echo "not found"
fi

done;
