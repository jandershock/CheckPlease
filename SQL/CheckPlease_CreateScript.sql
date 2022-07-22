USE [master]

IF db_id('CheckPlease') IS NULl
  CREATE DATABASE [CheckPlease]
GO

USE [CheckPlease]
GO


DROP TABLE IF EXISTS [GroupOrderUsers];
DROP TABLE IF EXISTS [GroupOrder];
DROP TABLE IF EXISTS [FoodItems];
DROP TABLE IF EXISTS [Restaurants];
DROP TABLE IF EXISTS [Users];
GO


CREATE TABLE [Users] (
  [Id] int PRIMARY KEY NOT NULL,
  [FirstName] nvarchar(255) NOT NULL,
  [LastName] nvarchar(255) NOT NULL
)
GO

CREATE TABLE [GroupOrder] (
  [Id] int PRIMARY KEY NOT NULL,
  [OwnerId] int NOT NULL,
  [RestaurantId] int NOT NULL,
  [IsReady] bit NOT NULL
)
GO

CREATE TABLE [GroupOrderUsers] (
  [Id] int PRIMARY KEY NOT NULL,
  [UserId] int NOT NULL,
  [GroupOrderId] int NOT NULL,
  [HasOrdered] bit NOT NULL
)
GO

CREATE TABLE [Restaurants] (
  [Id] int PRIMARY KEY NOT NULL,
  [Name] nvarchar(255) NOT NULL
)
GO

CREATE TABLE [FoodItems] (
  [Id] int PRIMARY KEY NOT NULL,
  [Description] nvarchar(255) NOT NULL,
  [RestaurantId] int NOT NULL,
  [Price] decimal NOT NULL,
  [Type] nvarchar(255) NOT NULL
)
GO

ALTER TABLE [GroupOrder] ADD FOREIGN KEY ([RestaurantId]) REFERENCES [Restaurants] ([Id]) ON DELETE CASCADE
GO

ALTER TABLE [GroupOrder] ADD FOREIGN KEY ([OwnerId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
GO

ALTER TABLE [GroupOrderUsers] ADD FOREIGN KEY ([GroupOrderId]) REFERENCES [GroupOrder] ([Id]) ON DELETE CASCADE
GO

ALTER TABLE [FoodItems] ADD FOREIGN KEY ([RestaurantId]) REFERENCES [Restaurants] ([Id]) ON DELETE CASCADE
GO