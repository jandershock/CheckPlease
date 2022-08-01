USE [CheckPlease]
GO

INSERT INTO UserProfiles (Email, FirebaseUserId)
VALUES ('asdf@asdf.com', 'UPvkK3dzZqWl1JHBC6HpgEm1wH33'),
	   ('frodo@shire.com', '7HoKCw88YIeukPWO0wsSVdvUKTC2'),
	   ('sam@shire.com', 'wAZVvJnTJHM0paAgzAyv1KGXIoF2'),
	   ('merry@shire.com', '3iORHCSuyabUpBbrSTize6fBjC32'),
	   ('pippin@shire.com', '6xRC0pETgxRbZk6QGpc3JXfE6wn1')

INSERT INTO Restaurants ([Name])
VALUES ('Mediterranean Cuisine'),
	   ('Greek Cafe')

INSERT INTO GroupOrders (OwnerId, RestaurantId, IsReady)
VALUES	(2, 1, 0),
		(3, 2, 0)

INSERT INTO GroupOrdersUserProfiles (UserProfileId, GroupOrderId, HasOrdered)
VALUES	(2, 1, 0),
		(3, 1, 0),
		(4, 1, 1),
		(5, 1, 0),

		(3, 2, 1),
		(2, 2, 0)

INSERT INTO FoodItems ([Description], RestaurantId, Price, [Type])
VALUES	('Chicken Gyro Platter', 1, 10.99, 'Entree'),
		('Chicken Shawarma Platter', 1, 10.99, 'Entree'),
		('Dr. Pepper', 1, 1.99, 'Drink'),
		('Coca-Cola', 1, 1.99, 'Drink'),
		('Tiramisu', 1, 5.48, 'Dessert'),
		('Baklava', 1, 4.38, 'Dessert'),

		('Chicken Platter', 2, 12.00, 'Entree'),
		('Mix Grill', 2, 12.50, 'Entree'),
		('Chicken Shawarma', 2, 13.00, 'Entree'),
		('Soft Drink', 2, 2.29, 'Drink'),
		('Ice Tea', 2, 2.29, 'Drink'),
		('Lemonade', 2, 2.29, 'Drink'),
		('Baklava', 2, 3.25, 'Dessert'),
		('Cheesecake', 2, 3.99, 'Dessert')

INSERT INTO FoodItemsGoup (FoodItemId, GroupOrdersUserProfilesId)
VALUES	(1, 3),
		(3, 3),
		(5, 3),

		(7, 5),
		(10, 5),
		(14, 5)