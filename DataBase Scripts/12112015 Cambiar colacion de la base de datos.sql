
USE [steelgo-sam3]
GO

ALTER TABLE [dbo].[Sam3_MaterialSpool] DROP CONSTRAINT [CK_MaterialSpool_Diametro1]
GO

ALTER TABLE [dbo].[Sam3_MaterialSpool]  WITH NOCHECK ADD  CONSTRAINT [CK_MaterialSpool_Diametro1] CHECK  (([Diametro1]>(0)))
GO

ALTER TABLE [dbo].[Sam3_MaterialSpool] CHECK CONSTRAINT [CK_MaterialSpool_Diametro1]
GO


USE master;
GO
ALTER DATABASE [steelgo-sam3]
SET SINGLE_USER
WITH ROLLBACK IMMEDIATE;
GO

ALTER DATABASE [steelgo-sam3]
COLLATE Latin1_General_CI_AS
GO

ALTER DATABASE [steelgo-sam3]
SET MULTI_USER;
GO


