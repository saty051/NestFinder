--------------------
-- Seeding Down
--------------------
DECLARE @UserID as int
SET @UserID = (SELECT id FROM Users WHERE Username='Demo')

DELETE FROM Users WHERE Username='Demo'
DELETE FROM PropertyTypes WHERE LatestUpdatedBy=@UserID
DELETE FROM FurnishingTypes WHERE LatestUpdatedBy=@UserID
DELETE FROM Cities WHERE LatestUpdatedBy=@UserID
DELETE FROM Properties WHERE PostedBy=@UserID
