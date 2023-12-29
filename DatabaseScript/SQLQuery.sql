BEGIN TRANSACTION;
BEGIN TRY

	CREATE TABLE [Category] (
	  [catid] int identity(1, 1),
	  [catName] varchar(50) UNIQUE not null,
	  [pcatid] int,
	  PRIMARY KEY ([catid]),
	  CONSTRAINT [FK_Category.pcatid]
		FOREIGN KEY ([pcatid])
		  REFERENCES [Category]([catid])
	);

	CREATE TABLE [Role] (
	  [rid] int,
	  [name] varchar(50) UNIQUE not null,
	  PRIMARY KEY ([rid])
	);

	CREATE TABLE [User] (
	  [uid] int identity(1, 1),
	  [name] varchar(50) not null,
	  [profilePic] varchar(MAX) not null,
	  [username] varchar(50) UNIQUE not null,
	  [password] varchar(50) not null,
	  [doj] datetime default GETDATE(),
	  [dob] date,
	  [accountStatus] varchar(20) default 'TRUE',
	  [shippingAddress] nvarchar(MAX),
	  [phone] varchar(25),
	  [roleid] int not null,
	  PRIMARY KEY ([uid]),
	  CONSTRAINT [FK_User.roleid]
		FOREIGN KEY ([roleid])
		  REFERENCES [Role]([rid])
	);

	CREATE TABLE [Logs] (
	  [lid] int identity(1, 1),
	  [timestamp] datetime default GETDATE(),
	  [uid] int not null,
	  PRIMARY KEY ([lid]),
	  CONSTRAINT [FK_Logs.uid]
		FOREIGN KEY ([uid])
		  REFERENCES [User]([uid])
	);

	CREATE TABLE [Product] (
	  [pid] int identity(1, 1),
	  [uid] int not null,
	  [name] nvarchar(100) UNIQUE not null,
	  [productPic] varchar(MAX) not null,
	  [shortDesc] nvarchar(50) not null,
	  [longDesc] nvarchar(MAX) not null,
	  [rating] float,
	  [amount] money not null,
	  [availableQty] int not null,
	  [catid] int not null,
	  PRIMARY KEY ([pid]),
	  CONSTRAINT [FK_Product.uid]
		FOREIGN KEY ([uid])
		  REFERENCES [User]([uid]),
	  CONSTRAINT [FK_Product.catid]
		FOREIGN KEY ([catid])
		  REFERENCES [Category]([catid])
	);

	CREATE TABLE [Payment] (
	  [payid] int,
	  [type] varchar(50) UNIQUE not null,
	  PRIMARY KEY ([payid])
	);

	CREATE TABLE [Cart] (
	  [cid] int identity(1, 1),
	  [createdAt] datetime default GETDATE() not null,
	  [closedAt] datetime,
	  [uid] int not null,
	  [totalAmount] int,
	  [payid] int,
	  [cartStatus] varchar(20) default 'CREATED',
	  PRIMARY KEY ([cid]),
	  CONSTRAINT [FK_Cart.uid]
		FOREIGN KEY ([uid])
		  REFERENCES [User]([uid]),
	  CONSTRAINT [FK_Cart.payid]
		FOREIGN KEY ([payid])
		  REFERENCES [Payment]([payid])
	);

	CREATE TABLE [Review] (
	  [rwid] int identity(1, 1),
	  [pid] int not null,
	  [comment] nvarchar(MAX) not null,
	  [rating] int not null,
	  PRIMARY KEY ([rwid]),
	  CONSTRAINT [FK_Review.pid]
		FOREIGN KEY ([pid])
		  REFERENCES [Product]([pid])
	);

	CREATE TABLE [CartItem] (
	  [cid] int,
	  [pid] int,
	  [pName] nvarchar(100) not null,
	  [pShortDesc] nvarchar(50) not null,
	  [pAmount] money not null,
	  [pBuyQty] int not null,
	  PRIMARY KEY ([cid], [pid]),
	  CONSTRAINT [FK_CartItem.pid]
		FOREIGN KEY ([pid])
		  REFERENCES [Product]([pid]),
	  CONSTRAINT [FK_CartItem.cid]
		FOREIGN KEY ([cid])
		  REFERENCES [Cart]([cid])
	);

COMMIT;
END TRY
BEGIN CATCH
    ROLLBACK;
    DECLARE @ErrorMessage NVARCHAR(4000);
    DECLARE @ErrorSeverity INT;
    DECLARE @ErrorState INT;
    SELECT 
        @ErrorMessage = ERROR_MESSAGE(),
        @ErrorSeverity = ERROR_SEVERITY(),
        @ErrorState = ERROR_STATE();
    RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
END CATCH;