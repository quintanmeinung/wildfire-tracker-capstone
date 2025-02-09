-- Create the WeatherData table
CREATE TABLE WeatherData (
    WeatherId INT PRIMARY KEY,
    Temperature INT NOT NULL,
    WindSpeedDirection VARCHAR(50) NOT NULL,
    AirQualityIndex INT NOT NULL
);

-- Create the FireData table
CREATE TABLE FireData (
    FireId INT PRIMARY KEY,
    Location GEOGRAPHY NOT NULL,
    RadiativePower DECIMAL(10, 2) NOT NULL,
    Polygon GEOGRAPHY NOT NULL,
    WeatherId INT FOREIGN KEY REFERENCES WeatherData(WeatherId)
);

-- Create the User table
CREATE TABLE User (
    UserId INT PRIMARY KEY,
    Username VARCHAR(255) NOT NULL,
    UserLocation GEOGRAPHY
);

-- Create the SavedLocation table
CREATE TABLE SavedLocation (
    LocationId INT PRIMARY KEY,
    Title VARCHAR(255) NOT NULL,
    Location GEOGRAPHY NOT NULL
);

-- Create the UserLocation table
CREATE TABLE UserLocation (
    UserId INT NOT NULL FOREIGN KEY REFERENCES User(UserId),
    LocationId INT NOT NULL FOREIGN KEY REFERENCES SavedLocation(LocationId),
    PRIMARY KEY (UserId, LocationId)
);

-- Create the UserFireSubscription table
CREATE TABLE UserFireSubscription (
    UserId INT NOT NULL FOREIGN KEY REFERENCES User(UserId),
    FireId INT NOT NULL FOREIGN KEY REFERENCES FireData(FireId),
    PRIMARY KEY (UserId, FireId)
);