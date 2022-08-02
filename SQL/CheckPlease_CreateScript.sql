USE [master]

IF db_id('CheckPlease') IS NULl
  CREATE DATABASE [CheckPlease]
GO

USE [CheckPlease]
GO


DROP TABLE IF EXISTS [FoodItemsGoup];
DROP TABLE IF EXISTS [GroupOrdersUserProfiles];
DROP TABLE IF EXISTS [GroupOrders];
DROP TABLE IF EXISTS [FoodItems];
DROP TABLE IF EXISTS [Restaurants];
DROP TABLE IF EXISTS [UserProfiles];
DROP TABLE IF EXISTS [FoodItems];
GO


CREATE TABLE [UserProfiles] (
  [Id] int IDENTITY(1,1) PRIMARY KEY NOT NULL,
  [Email] nvarchar(255) NOT NULL,
  [FirebaseUserId] varchar(28) NOT NULL

  CONSTRAINT [UQ_FirebaseUserId] UNIQUE([FirebaseUserId])
)
GO

CREATE TABLE [GroupOrders] (
  [Id] int PRIMARY KEY IDENTITY(1,1) NOT NULL,
  [OwnerId] int NOT NULL,
  [RestaurantId] int NOT NULL,
  [IsReady] bit NOT NULL
)
GO

CREATE TABLE [GroupOrdersUserProfiles] (
  [Id] int PRIMARY KEY IDENTITY(1,1) NOT NULL,
  [UserProfileId] int NOT NULL,
  [GroupOrderId] int NOT NULL,
  [HasOrdered] bit NOT NULL
)
GO

CREATE TABLE [Restaurants] (
  [Id] int PRIMARY KEY IDENTITY(1,1) NOT NULL,
  [Name] nvarchar(255) NOT NULL
)
GO

CREATE TABLE [FoodItems] (
  [Id] int PRIMARY KEY IDENTITY(1,1) NOT NULL,
  [Description] nvarchar(255) NOT NULL,
  [RestaurantId] int NOT NULL,
  [Price] decimal(18,2) NOT NULL,
  [Type] nvarchar(255) NOT NULL
)
GO

CREATE TABLE [FoodItemsGoup] (
  [Id] int PRIMARY KEY IDENTITY(1,1) NOT NULL,
  [FoodItemId] int NOT NULL,
  [GroupOrdersUserProfilesId] int NOT NULL
)
GO

ALTER TABLE [GroupOrders] ADD FOREIGN KEY ([OwnerId]) REFERENCES [UserProfiles] ([Id]) ON DELETE CASCADE
GO

ALTER TABLE [GroupOrders] ADD FOREIGN KEY ([RestaurantId]) REFERENCES [Restaurants] ([Id]) ON DELETE CASCADE
GO

ALTER TABLE [GroupOrdersUserProfiles] ADD FOREIGN KEY ([GroupOrderId]) REFERENCES [GroupOrders] ([Id]) ON DELETE CASCADE
GO

ALTER TABLE [FoodItems] ADD FOREIGN KEY ([RestaurantId]) REFERENCES [Restaurants] ([Id]) ON DELETE CASCADE
GO

ALTER TABLE [FoodItemsGoup] ADD FOREIGN KEY ([FoodItemId]) REFERENCES [FoodItems] ([Id])
GO

ALTER TABLE [FoodItemsGoup] ADD FOREIGN KEY ([GroupOrdersUserProfilesId]) REFERENCES [GroupOrdersUserProfiles] ([Id]) ON DELETE CASCADE
GO