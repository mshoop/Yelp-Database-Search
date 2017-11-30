--CHECKIN
--TRIGGER FUNCTION
CREATE OR REPLACE FUNCTION NumCheckinTriggerFunction() RETURNS TRIGGER AS $UpdateNumCheckin$
BEGIN
	UPDATE business
	SET numcheckins = table2.checkinsum
	FROM business B INNER JOIN
		(SELECT checkin.business_id, sum(count) AS checkinsum 
		FROM checkin 
		GROUP BY checkin.business_id) AS table2
	ON B.business_id = table2.business_ID
	WHERE table2.business_ID = business.business_ID;
	RETURN NULL;
END;
$UpdateNumCheckin$ LANGUAGE plpgsql;

--TRIGGER
CREATE TRIGGER UpdateNumCheckin
AFTER UPDATE ON checkin
EXECUTE PROCEDURE  NumCheckinTriggerFunction()

--TEST
UPDATE checkin
SET count = 5555
WHERE checkin.business_id = 'P1fJb2WQ1mXoiudj8UE44w';

SELECT * FROM BUSINESS
WHERE business.business_id = 'P1fJb2WQ1mXoiudj8UE44w';


--REVIEWCOUNT
--TRIGGER FUNCTION
CREATE OR REPLACE FUNCTION ReviewCountUpdate() RETURNS TRIGGER AS $UpdateReviewCount$
BEGIN
	UPDATE business
	SET review_count = table2.tipsum
	FROM business B INNER JOIN
		(SELECT tip.business_id, COUNT(business_id) AS tipsum 
		FROM tip 
		GROUP BY tip.business_id) AS table2
	ON B.business_id = table2.business_ID
	WHERE table2.business_ID = business.business_ID;
	RETURN NULL;
END;
$UpdateReviewCount$ LANGUAGE plpgsql;

--TRIGGER
CREATE TRIGGER UpdateReviewCount
AFTER INSERT ON tip
EXECUTE PROCEDURE ReviewCountUpdate()

--TEST
SELECT review_count FROM BUSINESS
WHERE business.business_id = 'McikHxxEqZ2X0joaRNKlaw';

INSERT INTO tip(business_id, user_id, txt, thedate, likes) VALUES ('McikHxxEqZ2X0joaRNKlaw','rpOyqD_893cqmDAtJLbdog','this is text','"2016-04-28','4');

SELECT review_count FROM BUSINESS
WHERE business.business_id = 'McikHxxEqZ2X0joaRNKlaw';