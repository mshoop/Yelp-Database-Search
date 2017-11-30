CREATE TABLE Business (
	business_id CHAR(22),
	name VARCHAR(100),
	full_address VARCHAR(150),
	city VARCHAR(50),
	state VARCHAR(10),
	numCheckins INTEGER,
	review_count INTEGER,
	PRIMARY KEY(business_id)
);

CREATE TABLE Users (
	user_id CHAR(22),
	name VARCHAR(100),
	review_count INTEGER,
	average_stars FLOAT,
	yelping_since VARCHAR(8),
	PRIMARY KEY(user_id)
);

CREATE TABLE Tip (
	business_id CHAR(22),
	user_id CHAR(22),
	txt VARCHAR(1000),
	thedate DATE,
	likes INTEGER,
	PRIMARY KEY (business_id,user_id,thedate),
	FOREIGN KEY(business_id) REFERENCES Business(business_id),
	FOREIGN KEY(user_id) REFERENCES Users(user_id)
);

CREATE TABLE CheckIn (
	business_id CHAR(22),
	time VARCHAR(25),
	day INTEGER,
	count INTEGER,
	PRIMARY KEY (business_id, time, day),
	FOREIGN KEY(business_id) REFERENCES Business(business_id)
);

CREATE TABLE Categories (
	business_id CHAR(22),
	category VARCHAR(50),
	PRIMARY KEY(business_id, category),
	FOREIGN KEY(business_id) REFERENCES Business(business_id)
);

CREATE TABLE Hours (
	business_id CHAR(22),
	day VARCHAR(10),
	open TIME, --hopefully time works with the data
	close TIME,
	PRIMARY KEY (business_id,day),
	FOREIGN KEY (business_id) REFERENCES Business(business_id)
);

CREATE TABLE Votes (
	user_id CHAR(22),
	type VARCHAR(25),
	count INTEGER,
	PRIMARY KEY(user_id,type),
	FOREIGN KEY(user_id) REFERENCES Users(user_id)
);
 
CREATE TABLE Friends (
	user_id CHAR(22),
	friend_id CHAR(22),
	PRIMARY KEY(user_id,friend_id),
	FOREIGN KEY(user_id) REFERENCES Users(user_id)
);