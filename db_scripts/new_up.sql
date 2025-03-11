CREATE TABLE [Users] (
  [UserId] nvarchar(450) PRIMARY KEY,
  [FirstName] nvarchar(50),
  [LastName] nvarchar(50)
);

CREATE TABLE [UserLocations] (
  [UserId] nvarchar(450) NOT NULL,
  [Title] nvarchar(100),
  [Latitude] decimal(8,6) NOT NULL,
  [Longitude] decimal(9,6) NOT NULL
);

CREATE TABLE [Fires] (
  [FireId] int PRIMARY KEY IDENTITY(1, 1),
  [Latitude] decimal(8,6) NOT NULL,
  [Longitude] decimal(9,6) NOT NULL,
  [RadiativePower] decimal(10,2),
  [Polygon] geography
);

CREATE TABLE [UserFireSubscriptions] (
  [UserId] nvarchar(450) NOT NULL,
  [FireId] int NOT NULL,
  PRIMARY KEY ([UserId], [FireId])
);

CREATE TABLE [AqiStations] (
  [Name] nvarchar(50),
  [StationId] nvarchar(10)
);

ALTER TABLE [UserLocations] ADD CONSTRAINT [FK_UserLocations_Users] FOREIGN KEY ([UserId]) REFERENCES [Users] ([UserId])
GO

ALTER TABLE [UserFireSubscriptions] ADD CONSTRAINT [FK_UserFireSubscriptions_Users] FOREIGN KEY ([UserId]) REFERENCES [Users] ([UserId])
GO

ALTER TABLE [UserFireSubscriptions] ADD CONSTRAINT [FK_UserFireSubscriptions_Fires] FOREIGN KEY ([FireId]) REFERENCES [Fires] ([FireId])
GO