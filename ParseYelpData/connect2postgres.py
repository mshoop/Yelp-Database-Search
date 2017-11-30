import json
import psycopg2
import time

numberInserts = 500

def cleanStr4SQL(s):
    return s.replace("'", "`").replace("\n", " ")#.replace("_","%[_]%")

def insertBusiness():
    try:
        conn = psycopg2.connect("dbname= 'milestone3' user='postgres' host='localhost' password='12345'")
    except:
        print('Unable to connect to the postgreSQL database')
        exit(1)

    cur = conn.cursor()

    with open('C:\\Users\Shoop\Desktop\yelp_business.JSON', 'r') as f:
        line = f.readline()
        count_line = 0
        count_error = 0
        inserted = 0
        # read each JSON abject and extract data
        #while count_line < numberInserts:
        while line:
            data = json.loads(line)

            sql_str = "INSERT INTO business (business_id,name,full_address,city,state,numcheckins) VALUES ('" + cleanStr4SQL(
                data['business_id']) + "','" + cleanStr4SQL(data['name']) + "','" + cleanStr4SQL(
                data['full_address']) + "','" + cleanStr4SQL(data['city']) + "','" + cleanStr4SQL(
                data['state']) + "','" + "0')"  # openstatus

            try:
                cur.execute(sql_str)
                inserted +=1
            except:
      #          print("Insert to businessTABLE failed!")
                count_error += 1

            conn.commit()
            line = f.readline()
            count_line += 1
        # end of while loop
        print("Business")
        print("count line: " + str(count_line))
        print("inserted: " + str(inserted))
        print("count error: " + str(count_error))
        print()
        f.close()

def insertUser():
    try:
        conn = psycopg2.connect("dbname= 'milestone3' user='postgres' host='localhost' password='12345'")
    except:
        print('Unable to connect to the postgreSQL database')
        exit(1)

    cur = conn.cursor()

    with open('C:\\Users\Shoop\Desktop\yelp_user.JSON','r') as f:
        outfile =  open('yelp_user.txt', 'w',encoding="utf-8-sig")
        count_line = 0
        count_error = 0
        inserted = 0
        line = f.readline()

        #read each JSON abject and extract data
        #while count_line < numberInserts:
        while line:
            data = json.loads(line)

            sql_str = "INSERT INTO users (user_id,name,review_count,average_stars,yelping_since) VALUES ('" + cleanStr4SQL(data['user_id']) + "','" + cleanStr4SQL(data['name']) + "','" + cleanStr4SQL(str(data['review_count'])) + "','" + cleanStr4SQL(str(data['average_stars'])) + "','" + cleanStr4SQL(data['yelping_since']) + "')"  # openstatus

            try:
                cur.execute(sql_str)
                inserted +=1
            except:
       #         print("Insert to userTable failed!")
                count_error += 1
            conn.commit()
            count_line += 1
            line = f.readline()

        #end of while loop
        print("User")
        print("count line: " + str(count_line))
        print("inserted: " + str(inserted))
        print("count error: " + str(count_error))
        print()
        f.close()
        pass

def insertCategories():
    try:
        conn = psycopg2.connect("dbname= 'milestone3' user='postgres' host='localhost' password='12345'")
    except:
        print('Unable to connect to the postgreSQL database')
        exit(1)

    cur = conn.cursor()

    with open('C:\\Users\Shoop\Desktop\yelp_business.JSON', 'r') as f:
        line = f.readline()
        count_line = 0
        count_error = 0
        inserted = 0
        # read each JSON abject and extract data
        #while count_line < numberInserts:
        while line:
            data = json.loads(line)

            for cat in data['categories']:

                sql_str = "INSERT INTO categories (business_id,category) VALUES ('" + cleanStr4SQL(
                    data['business_id']) + "','" + cleanStr4SQL(cat) + "')"

                try:
                    cur.execute(sql_str)
                    inserted +=1
                except:
        #            print("Insert to categoriesTable failed!")
                    count_error += 1

                conn.commit()
            #end of for loop
            line = f.readline()
            count_line += 1
        # end of while loop
        print("Categories")
        print("count line: " + str(count_line))
        print("inserted: " + str(inserted))
        print("count error: " + str(count_error))
        print()
        f.close()

def insertTip():
    try:
        conn = psycopg2.connect("dbname= 'milestone3' user='postgres' host='localhost' password='12345'")
    except:
        print('Unable to connect to the postgreSQL database')
        exit(1)

    cur = conn.cursor()

    with open('C:\\Users\Shoop\Desktop\yelp_tip.JSON', 'r') as f:
        line = f.readline()
        count_line = 0
        count_error = 0
        inserted = 0

        # read each JSON abject and extract data
        #while count_line < numberInserts * 5:
        while line:
            data = json.loads(line)

            sql_str = "INSERT INTO tip (business_id,user_id,txt,thedate,likes) VALUES ('" + cleanStr4SQL(
                data['business_id']) + "','" + cleanStr4SQL(data['user_id']) + "','" + cleanStr4SQL(
                data['text']) + "','" + cleanStr4SQL(data['date']) + "','" + cleanStr4SQL(str(
                data['likes'])) + "')"  # openstatus
            try:
                cur.execute(sql_str)
                inserted += 1
            except:
#                print("Insert to tipTable failed!")
                count_error += 1

            conn.commit()
            line = f.readline()
            count_line += 1
        # end of while loop
        print("TIP")
        print("count line: " + str(count_line))
        print("inserted: " + str(inserted))
        print("count error: " + str(count_error))
        print()
        f.close()

def insertFriends():
    try:
        conn = psycopg2.connect("dbname= 'milestone3' user='postgres' host='localhost' password='12345'")
    except:
        print('Unable to connect to the postgreSQL database')
        exit(1)

    cur = conn.cursor()

    with open('C:\\Users\Shoop\Desktop\yelp_user.JSON', 'r') as f:
        line = f.readline()
        count_line = 0
        count_error = 0
        inserted = 0
        # read each JSON abject and extract data
        #while count_line < numberInserts:
        while line:
            data = json.loads(line)

            for cat in data['friends']:
                sql_str = "INSERT INTO friends (user_id,friend_id) VALUES ('" + cleanStr4SQL(data['user_id']) + "','" + cleanStr4SQL(cat) + "')"
                try:
                    cur.execute(sql_str)
                    inserted += 1
                except:
#                    print("Insert to friendsTABLE failed!")
                    count_error += 1
                conn.commit()
            #end of for loop
            line = f.readline()
            count_line += 1
        # end of while loop
        print("Friends")
        print("inserted: " + str(inserted))
        print("count line: " + str(count_line))
        print("count error: " + str(count_error))
        print()
        f.close()
        pass

def insertHour():
    try:
        conn = psycopg2.connect("dbname= 'milestone3' user='postgres' host='localhost' password='12345'")
    except:
        print('Unable to connect to the postgreSQL database')
        exit(1)

    cur = conn.cursor()

    with open('C:\\Users\Shoop\Desktop\yelp_business.JSON', 'r') as f:
        line = f.readline()
        count_line = 0
        count_error = 0
        inserted = 0

        # read each JSON abject and extract data
        #while count_line < numberInserts:
        while line:
            data = json.loads(line)
            for day in data['hours']:
                sql_str = "INSERT INTO hours (business_id,day,open,close) VALUES ('" + cleanStr4SQL(
                    data['business_id']) + "','" + cleanStr4SQL(day) + "','" + cleanStr4SQL(data['hours'][day]['open']) + "','" + cleanStr4SQL(data['hours'][day]['close']) + "')"

                try:
                    cur.execute(sql_str)
                    inserted += 1
                except:
                    count_error += 1
            #end of for loop
            conn.commit()
            line = f.readline()
            count_line += 1
        # end of while loop
        print("Hour")
        print("count line: " + str(count_line))
        print("inserted: " + str(inserted))
        print("count error: " + str(count_error))
        print()
        f.close()

def insertVotes():
    try:
        conn = psycopg2.connect("dbname= 'milestone3' user='postgres' host='localhost' password='12345'")
    except:
        print('Unable to connect to the postgreSQL database')
        exit(1)

    cur = conn.cursor()

    with open('C:\\Users\Shoop\Desktop\yelp_user.JSON', 'r') as f:
        line = f.readline()
        count_line = 0
        count_error = 0
        inserted = 0
        # read each JSON abject and extract data
        #while count_line < numberInserts:
        while line:
            data = json.loads(line)

            #for cat in data['votes']:
            for c in data['votes']:
                sql_str = "INSERT INTO votes (user_id,type,count) VALUES ('" + cleanStr4SQL(data['user_id']) + "','" + cleanStr4SQL(c) + "','" + str(data['votes'][c]) + "')"
                try:
                    cur.execute(sql_str)
                    inserted += 1
                except:
                    #print("Insert to votesTABLE failed!")
                    count_error += 1
                conn.commit()
            #end of for loop
            line = f.readline()
            count_line += 1
        # end of while loop
        print("votes")
        print("count line: " + str(count_line))
        print("inserted: " + str(inserted))
        print("count error: " + str(count_error))
        print()
        f.close()

def insertCheckinData():
    try:
        conn = psycopg2.connect("dbname= 'milestone3' user='postgres' host='localhost' password='12345'")
    except:
        print('Unable to connect to the postgreSQL database')
        exit(1)

    cur = conn.cursor()

    with open('C:\\Users\Shoop\Desktop\yelp_checkin.JSON', 'r') as f:
        line = f.readline()
        count_line = 0
        count_error = 0
        inserted = 0
        # read each JSON abject and extract data
        #while count_line < 25:
        while line:
            data = json.loads(line)

            checkin_info = data['checkin_info']

            dictionary = {}

            for checkin_info, checkin_info_freq in checkin_info.items():
                tokenize = checkin_info.split('-')
                day = tokenize[1]
                time = int(tokenize[0])
                #Day is number between 0-6 sunday-saturday
                #time is 0-23 for hours in day
                timeS = get_checkin_int(time)
                dictionary[day] = dictionary.get(day, [0 for i in range(4)])
                dictionary[day][timeS] = dictionary[day][timeS] + checkin_info_freq


            for k, v in list(dictionary.items()):
                #print(k)
                #print(v)
                for i,value in enumerate(v):
                    if value != 0:
                        timeString = get_checkin_time(i)
                        day = k
                        count = value
                        sql_str = "INSERT INTO checkin (business_id,time,day,count) VALUES ('" + cleanStr4SQL(data['business_id']) + "','" + cleanStr4SQL(timeString) + "','" + str(day) + "','" + str(count) + "')"
                        try:
                            cur.execute(sql_str)
                            inserted += 1
                        except:
                            #  print("Insert to checkin table failed!")
                            count_error += 1
                        conn.commit()
            #end of for loop
            line = f.readline()
            count_line += 1
        # end of while loop
        print("CHECKIN")
        print("count line: " + str(count_line))
        print("inserted: " + str(inserted))
        print("count error: " + str(count_error))
        print()
        f.close()

def get_checkin_int(time):
    if time >= 6 and time < 12:
        return 0
    elif time >= 12 and time < 17:
        return 1
    elif time >= 17 and time < 23:
        return 2
    elif time >= 23 and time <= 24:
        return 3
    elif time < 6:
        return 3
    #else:
 #       return "error"

def get_checkin_time(time):
    if time == 0:
        return "morning"
    elif time == 1:
        return "afternoon"
    elif time == 2:
        return "evening"
    elif time == 3:
        return "night"
    #else:
#        return "error"

startTime = time.time()

# Uncomment each function to parse data 
# Run each function one at a time
# Run a second time if not all data was parsed on the first run
#insertBusiness()
#insertUser()
#insertCategories()
#insertTip()
#insertFriends()
#insertHour()
#insertVotes()
#insertCheckinData()
print("\nElapsed Time(seconds): " + str(time.time()-startTime))