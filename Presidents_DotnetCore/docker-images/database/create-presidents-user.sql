USE [master]
GO

IF EXISTS(
	SELECT [name] 
	FROM master.sys.server_principals
    WHERE name = 'presidents-user'
	)
BEGIN
	print 'user already exists...deleting'	
	DROP LOGIN [presidents-user]
END

GO

CREATE LOGIN [presidents-user] 
WITH 
	PASSWORD=N'YayPresidents!', 
	DEFAULT_DATABASE=[master], 
	DEFAULT_LANGUAGE=[us_english], 
	CHECK_EXPIRATION=OFF, 
	CHECK_POLICY=OFF
GO

ALTER LOGIN [presidents-user] ENABLE
GO

ALTER SERVER ROLE [dbcreator] ADD MEMBER [presidents-user]
GO