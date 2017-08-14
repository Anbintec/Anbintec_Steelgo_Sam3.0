/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Sam3_Entidad SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Sam3_Proyecto SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Sam3_Rel_Proyecto_Entidad_Configuracion
	(
	Rel_Proyecto_Entidad_Configuracion_ID int NOT NULL IDENTITY (1, 1),
	Proyecto int NOT NULL,
	Entidad int NOT NULL,
	PreFijoFolioAvisoLlegada nchar(10) NOT NULL,
	PostFijoFolioAvisoLlegada nchar(10) NOT NULL,
	CantidadCerosFolioAvisoLlegada int NOT NULL,
	ConsecutivoFolioAvisoLlegada int NOT NULL,
	PreFijoFolioPackingList nchar(10) NOT NULL,
	PostFijoFolioPackingList nchar(10) NOT NULL,
	CantidadCerosFolioPackingList int NOT NULL,
	ConsecutivoFolioPackingList int NOT NULL,
	PreFijoFolioOrdenRecepcion nchar(10) NOT NULL,
	PostFijoFolioOrdenRecepcion nchar(10) NOT NULL,
	CantidadCerosFolioOrdenRecepcion int NOT NULL,
	ConsecutivoFolioOrdenRecepcion int NOT NULL,
	PreFijoFolioOrdenAlmacenaje nchar(10) NOT NULL,
	PostFijoFolioOrdenAlmacenaje nchar(10) NOT NULL,
	CantidadCerosFolioOrdenAlmacenaje int NOT NULL,
	ConsecutivoFolioOrdenAlmacenaje int NOT NULL,
	PreFijoFolioIncidencias nchar(10) NOT NULL,
	PostFijoFolioIncidencias nchar(10) NOT NULL,
	CantidadCerosFolioIncidencias int NOT NULL,
	ConsecutivoIncidencias int NOT NULL,
	PreFijoFolioPickingTicket nchar(10) NOT NULL,
	PostFijoFolioPickingTicker nchar(10) NOT NULL,
	CantidadCerosFolioPickingTicket int NOT NULL,
	ConsecutivoPickingTicket int NOT NULL,
	Activo int NOT NULL,
	FechaModificacion datetime NULL,
	UsuarioModificacion int NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Sam3_Rel_Proyecto_Entidad_Configuracion ADD CONSTRAINT
	PK_Rel_Proyecto_Entidad_Configuracion PRIMARY KEY CLUSTERED 
	(
	Rel_Proyecto_Entidad_Configuracion_ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.Sam3_Rel_Proyecto_Entidad_Configuracion ADD CONSTRAINT
	FK_Rel_Proyecto_Entidad_Configuracion_Sam3_Proyecto FOREIGN KEY
	(
	Rel_Proyecto_Entidad_Configuracion_ID
	) REFERENCES dbo.Sam3_Proyecto
	(
	ProyectoID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Sam3_Rel_Proyecto_Entidad_Configuracion ADD CONSTRAINT
	FK_Rel_Proyecto_Entidad_Configuracion_Sam3_Entidad FOREIGN KEY
	(
	Rel_Proyecto_Entidad_Configuracion_ID
	) REFERENCES dbo.Sam3_Entidad
	(
	EntidadID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Sam3_Rel_Proyecto_Entidad_Configuracion SET (LOCK_ESCALATION = TABLE)
GO

ALTER TABLE dbo.Sam3_FolioAvisoLlegada ADD Entidad int NOT NULL DEFAULT(1)
GO
ALTER TABLE dbo.Sam3_FolioAvisoLlegada ADD CONSTRAINT
	FK_Sam3_FolioAvisoLlegada_Sam3_Entidad FOREIGN KEY
	(
	Entidad
	) REFERENCES dbo.Sam3_Entidad
	(
	EntidadID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
GO

ALTER TABLE dbo.Sam3_FolioAvisoLlegada ADD ProyectoNombrado int NOT NULL DEFAULT(1)
GO

ALTER TABLE dbo.Sam3_FolioAvisoLlegada ADD CONSTRAINT
	FK_Sam3_FolioAvisoLlegada_Sam3_Proyecto FOREIGN KEY
	(
	ProyectoNombrado
	) REFERENCES dbo.Sam3_Proyecto
	(
	ProyectoID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION
GO

COMMIT
