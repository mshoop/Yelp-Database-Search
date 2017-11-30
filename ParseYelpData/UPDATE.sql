--CHECKIN
--UPDATE
UPDATE business
SET numcheckins = table2.checkinsum
FROM business B INNER JOIN
	(SELECT checkin.business_id, sum(count) AS checkinsum 
	FROM checkin 
	GROUP BY checkin.business_id) AS table2
ON B.business_id = table2.business_ID
WHERE table2.business_ID = business.business_ID


--REVIEWCOUNT (TIP COUNT)
--UPDATE
UPDATE business
SET review_count = table2.tipsum
FROM business B INNER JOIN
	(SELECT tip.business_id, COUNT(business_id) AS tipsum 
	FROM tip 
	GROUP BY tip.business_id) AS table2
ON B.business_id = table2.business_ID
WHERE table2.business_ID = business.business_ID
