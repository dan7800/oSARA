#!/usr/bin/env python
import  sqlite3
from sqlite3 import Error
import pandas as pd
import numpy as np


newlyadd = 'newly_added'
perm_rem = 'permanently-removed'
rem_update = 'remove-update'
addup = 'add-update'
most_updated = 'most_updated'

try:
    conn = sqlite3.connect("database_new.sqlite")
except Error as e:
    print(e)

df = pd.read_sql_query("select * from DCL_Production;", conn)
df['Date'] = pd.to_datetime(df.Date)
df['clean'] = None
df['status'] = None

def dep_cleaning():
    target = df['Dependencies']
    list = []
    for item in target:
        end = -1
        while(not item[end].isalpha()):
            end += -1
        list.append(item[:end])
    return list

def operations():
    list1 = dep_cleaning()
    df['clean'] = list1
    names = df['Name'].values.tolist()
    raw_df = pd.DataFrame.copy(df)
    # print(raw_df)
    unique_elements = set(list1)
    unique_names = set(names)

    for item1 in unique_names:
        for element in unique_elements:
            new_df = raw_df.loc[(df['Name'] == item1) & (df['clean'] == element)]
            # temp_df = pd.read_sql_query("SELECT * FROM DCL_Production WHERE Name = (?) AND clean = (?) order by Date;",conn, params=(item1, element))
            if (not new_df.empty):
                new_df.sort_values(by = ['Date'], ascending = False)
                length = len(new_df)
                # print(length)
                if length <= 3:
                    if length == 1:
                        indeks = new_df.index
                        raw_df.loc[indeks, 'status'] = newlyadd
                        print(raw_df.loc[raw_df['status'] == newlyadd])
                    elif length == 2:
                        indeks = new_df.index.tolist()
                        raw_df.loc[indeks[0], 'status'] = newlyadd
                        raw_df.loc[indeks[1], 'status'] = perm_rem
                        print(raw_df.loc[raw_df['status'] == newlyadd])
                    elif length == 3:
                        indeks = new_df.index.tolist()
                        raw_df.loc[indeks[0], 'status'] = newlyadd
                        raw_df.loc[indeks[1], 'status'] = rem_update
                        raw_df.loc[indeks[2], 'status'] = addup
                        print(raw_df.loc[raw_df['status'] == newlyadd])
                else:
                    if (length % 2 == 0):
                        indeks = new_df.index.tolist()
                        raw_df.loc[indeks[0], 'status'] = newlyadd
                        raw_df.loc[indeks[-1], 'status'] = perm_rem
                        for i in range(1,(len(indeks)-1)):
                            if (i % 2 == 0):
                                raw_df.loc[indeks[i], 'status'] = addup
                            else:
                                raw_df.loc[indeks[i], 'status'] = rem_update
                        print(raw_df.loc[raw_df['status'] == newlyadd])
                    else:
                        indeks = new_df.index.tolist()
                        raw_df.loc[indeks[0], 'status'] = newlyadd
                        raw_df.loc[indeks[-1], 'status'] = most_updated
                        for j in range(1, (len(indeks) - 1)):
                            if (j % 2 == 0):
                                raw_df.loc[indeks[j], 'status'] = addup
                            else:
                                raw_df.loc[indeks[j], 'status'] = rem_update
                        print(raw_df.loc[raw_df['status'] == newlyadd])
    return raw_df


final_df = operations()
final_df.to_sql("DCL_Production", conn, if_exists="replace")
conn.commit()

