BEGIN TRANSACTION;
BEGIN TRY

DBCC CHECKIDENT ('Category', RESEED, 0);
DBCC CHECKIDENT ('User', RESEED, 0);
DBCC CHECKIDENT ('Logs', RESEED, 0);
DBCC CHECKIDENT ('Product', RESEED, 0);
DBCC CHECKIDENT ('Cart', RESEED, 0);
DBCC CHECKIDENT ('Review', RESEED, 0);

-- Insert into Category
INSERT INTO [Category] ([catName]) VALUES ('Electronics');
INSERT INTO [Category] ([catName], [pcatid]) VALUES ('Mobiles', 1);

-- Insert into Role
INSERT INTO [Role] ([rid], [name]) VALUES (1, 'Admin');
INSERT INTO [Role] ([rid], [name]) VALUES (2, 'Vendor');
INSERT INTO [Role] ([rid], [name]) VALUES (3, 'Customer');

-- Insert into User
INSERT INTO [User] ([name], [profilePic], [username], [password], [dob], [roleid]) 
VALUES ('Admin', '~/uploads/userImages/user.png', 'Admin', 'Admin', '2000-01-01', 1);
INSERT INTO [User] ([name], [profilePic], [username], [password], [dob], [roleid]) 
VALUES ('Vendor', '~/uploads/userImages/user.png', 'Vendor', 'Vendor', '2000-01-01', 2);
INSERT INTO [User] ([name], [profilePic], [username], [password], [dob], [roleid]) 
VALUES ('Abdul Moiz', '~/uploads/userImages/user.png', 'Abdul Moiz', 'Abdul Moiz', '2000-01-01', 3);
INSERT INTO [User] ([name], [profilePic], [username], [password], [dob], [roleid]) 
VALUES ('Ahmed Zaman', '~/uploads/userImages/user.png', 'Ahmed Zaman', 'Ahmed Zaman', '2000-01-01', 3);

-- Insert into Logs
INSERT INTO [Logs] ([uid]) VALUES (1);
INSERT INTO [Logs] ([uid]) VALUES (2);

-- Insert into Product
INSERT INTO [Product] ([uid], [name], [productPic], [shortDesc], [longDesc], [amount], [availableQty], [catid]) 
VALUES (1, 'iPhone 13', '~/uploads/productImages/iphone13.jpeg', 'Latest iPhone 13', 'The iPhone 13 is the latest smartphone from Apple.', 999.00, 10, 2);
INSERT INTO [Product] ([uid], [name], [productPic], [shortDesc], [longDesc], [amount], [availableQty], [catid]) 
VALUES (2, 'Samsung Galaxy S21', '~/uploads/productImages/galaxys21.jpeg', 'Latest Samsung Galaxy S21', 'The Samsung Galaxy S21 is the latest smartphone from Samsung.', 899.00, 20, 2);

-- Insert into Payment
INSERT INTO [Payment] ([payid], [type]) VALUES (1, 'Cash');
INSERT INTO [Payment] ([payid], [type]) VALUES (2, 'Card');

-- Insert into Cart
INSERT INTO [Cart] ([uid]) VALUES (3);
INSERT INTO [Cart] ([uid]) VALUES (4);

-- Insert into Review
INSERT INTO [Review] ([pid], [comment], [rating]) VALUES (1, 'Great phone with latest features.', 5);
INSERT INTO [Review] ([pid], [comment], [rating]) VALUES (2, 'Good phone with nice camera.', 4);

-- Insert into CartItem
INSERT INTO [CartItem] ([cid], [pid], [pName], [pShortDesc], [pAmount], [pBuyQty]) 
VALUES (1, 1, 'iPhone 13', 'Latest iPhone 13', 999.00, 1);
INSERT INTO [CartItem] ([cid], [pid], [pName], [pShortDesc], [pAmount], [pBuyQty]) 
VALUES (2, 2, 'Samsung Galaxy S21', 'Latest Samsung Galaxy S21', 899.00, 1);


-- COMMIT;
END TRY
BEGIN CATCH
    ROLLBACK;
    DECLARE @ErrorMessage NVARCHAR(4000);
    DECLARE @ErrorSeverity INT;
    DECLARE @ErrorState INT;
	print('Rollbacked');
    SELECT 
        @ErrorMessage = ERROR_MESSAGE(),
        @ErrorSeverity = ERROR_SEVERITY(),
        @ErrorState = ERROR_STATE();
    RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
END CATCH;