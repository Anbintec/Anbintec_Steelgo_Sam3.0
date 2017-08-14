EXEC master.dbo.sp_addlinkedserver 
	@server = N'SAM3', 
	@provider=N'SQLNCLI',
	@srvproduct = 'SQL', 
	@provstr=N'SERVER=STEELGO-DB06\SAM3;User ID=sam' 

EXEC master.dbo.sp_addlinkedsrvlogin 
	@rmtsrvname = N'SAM3', 
	@locallogin = NULL , 
	@useself = N'False', 
	@rmtuser = N'sam', 
	@rmtpassword = N'sam123!'