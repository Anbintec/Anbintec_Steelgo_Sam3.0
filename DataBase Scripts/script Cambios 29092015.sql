USE [steelgo-sam3]
GO

ALTER TABLE [dbo].[Sam3_Corte] DROP CONSTRAINT [FK_Corte_NumeroUnicoCorte]
GO
 
ALTER TABLE [dbo].[Sam3_Corte] ALTER COLUMN [NumeroUnicoCorteID] INT NULL
GO

ALTER TABLE [dbo].[Sam3_CorteDetalle] DROP CONSTRAINT [FK_CorteDetalle_MaterialSpool]
GO

ALTER TABLE [dbo].[Sam3_CorteDetalle] DROP CONSTRAINT [FK_CorteDetalle_OrdenTrabajoSpool]
GO

ALTER TABLE [dbo].[Sam3_CorteDetalle] DROP CONSTRAINT [FK_CorteDetalle_Maquina]
GO




