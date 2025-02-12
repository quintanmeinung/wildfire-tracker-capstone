-- Drop foreign key constraints

ALTER TABLE [FireData]
DROP CONSTRAINT [FK_FireData_WeatherData];

ALTER TABLE [UserLocation]
DROP CONSTRAINT [FK_UserLocation_User];

ALTER TABLE [UserLocation]
DROP CONSTRAINT [FK_UserLocation_SavedLocation];

ALTER TABLE [UserFireSubscription]
DROP CONSTRAINT [FK_UserFireSubscription_User];

ALTER TABLE [UserFireSubscription]
DROP CONSTRAINT [FK_UserFireSubscription_FireData];

-- Drop tables

DROP TABLE [UserFireSubscription];
DROP TABLE [UserLocation];
DROP TABLE [SavedLocation];
DROP TABLE [User];
DROP TABLE [FireData];
DROP TABLE [WeatherData];