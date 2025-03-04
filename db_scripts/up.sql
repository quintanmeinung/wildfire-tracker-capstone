-- Create the WeatherData table
CREATE TABLE [WeatherData] (
    [WeatherId] int PRIMARY KEY,
    [Temperature] int NOT NULL,
    [WindSpeedDirection] varchar(50) NOT NULL,
    [AirQualityIndex] int NOT NULL
);

-- Create the FireData table
CREATE TABLE [FireData] (
    [FireId] int PRIMARY KEY,
    [Location] geography NOT NULL,
    [RadiativePower] decimal(10, 2) NOT NULL,
    [Polygon] geography NOT NULL,
    [WeatherId] int
);

-- Create the User table
CREATE TABLE [User] (
    [UserId] nvarchar(450) PRIMARY KEY,
    [FirstName] varchar(50) NOT NULL,
    [LastName] varchar(50) NOT NULL,
    [UserLocation] geography
);

-- Create the SavedLocation table
CREATE TABLE [SavedLocation] (
    [LocationId] int PRIMARY KEY,
    [Title] varchar(50) NOT NULL,
    [Location] geography NOT NULL
);

-- Create the UserLocation table
CREATE TABLE [UserLocation] (
    [UserId] int NOT NULL,
    [LocationId] int NOT NULL,
    PRIMARY KEY ([UserId], [LocationId])
);

-- Create the UserFireSubscription table
CREATE TABLE [UserFireSubscription] (
    [UserId] int NOT NULL,
    [FireId] int NOT NULL,
    PRIMARY KEY ([UserId], [FireId])
);

-- Add foreign key constraints using ALTER TABLE

-- Add foreign key to FireData referencing WeatherData
ALTER TABLE [FireData]
ADD CONSTRAINT [FK_FireData_WeatherData]
FOREIGN KEY ([WeatherId]) REFERENCES [WeatherData]([WeatherId]);

-- Add foreign key to UserLocation referencing User
ALTER TABLE [UserLocation]
ADD CONSTRAINT [FK_UserLocation_User]
FOREIGN KEY ([UserId]) REFERENCES [User]([UserId]);

-- Add foreign key to UserLocation referencing SavedLocation
ALTER TABLE [UserLocation]
ADD CONSTRAINT [FK_UserLocation_SavedLocation]
FOREIGN KEY ([LocationId]) REFERENCES [SavedLocation]([LocationId]);

-- Add foreign key to UserFireSubscription referencing User
ALTER TABLE [UserFireSubscription]
ADD CONSTRAINT [FK_UserFireSubscription_User]
FOREIGN KEY ([UserId]) REFERENCES [User]([UserId]);

-- Add foreign key to UserFireSubscription referencing FireData
ALTER TABLE [UserFireSubscription]
ADD CONSTRAINT [FK_UserFireSubscription_FireData]
FOREIGN KEY ([FireId]) REFERENCES [FireData]([FireId]);