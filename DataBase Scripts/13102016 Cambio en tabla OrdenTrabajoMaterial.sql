USE [SAM]
GO

ALTER TABLE [dbo].[OrdenTrabajoMaterial] DROP CONSTRAINT [FK_OrdenTrabajoMaterial_Despacho]
GO

USE [SAM]
GO

ALTER TABLE [dbo].[OrdenTrabajoMaterial] DROP CONSTRAINT [FK_OrdenTrabajoMaterial_CorteDetalle]
GO

