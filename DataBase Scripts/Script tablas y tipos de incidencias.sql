BEGIN TRAN
BEGIN TRY

	USE [steelgo-sam3]

	CREATE TABLE Sam3_ClasificacionIncidencia(
		ClasificacionIncidenciaID INT NOT NULL PRIMARY KEY IDENTITY(1,1),
		Nombre VARCHAR(150) NOT NULL,
		Activo BIT NOT NULL DEFAULT(1),
		FechaModificacion DATETIME NULL,
		UsuarioModificacion INT NULL
	)

	CREATE TABLE Sam3_TipoIncidencia(
		TipoIncidenciaID INT NOT NULL PRIMARY KEY IDENTITY(1,1),
		Nombre VARCHAR(150) NOT NULL,
		Activo BIT NOT NULL DEFAULT(1),
		FechaModificacion DATETIME NULL,
		UsuarioModificacion INT NULL
	)

	CREATE TABLE Sam3_Rel_Incidencia_FolioAvisoLlegada(
		Rel_Incidencia_FolioAvisoLlegada_ID INT NOT NULL PRIMARY KEY IDENTITY(1,1),
		IncidenciaID INT NOT NULL,
		FolioAvisoLlegadaID INT NOT NULL,
		Activo BIT NOT NULL DEFAULT(1),
		FechaModificacion DATETIME NULL,
		UsuarioModificacion INT NULL,
		FOREIGN KEY (IncidenciaID) REFERENCES Sam3_Incidencia(IncidenciaID),
		FOREIGN KEY (FolioAvisoLlegadaID) REFERENCES Sam3_FolioAvisoLlegada(FolioAvisoLlegadaID)
	)

	CREATE TABLE Sam3_Rel_Incidencia_FolioAvisoEntrada(
		Rel_Incidencia_FolioAvisoEntrada_ID INT NOT NULL PRIMARY KEY IDENTITY(1,1),
		IncidenciaID INT NOT NULL,
		FolioAvisoEntradaID INT NOT NULL,
		Activo BIT NOT NULL DEFAULT(1),
		FechaModificacion DATETIME NULL,
		UsuarioModificacion INT NULL,
		FOREIGN KEY (IncidenciaID) REFERENCES Sam3_Incidencia(IncidenciaID),
		FOREIGN KEY (FolioAvisoEntradaID) REFERENCES Sam3_FolioAvisoEntrada(FolioAvisoEntradaID)
	)

	CREATE TABLE Sam3_Rel_Incidencia_PaseSalida(
		Rel_Incidencia_PaseSalida_ID INT NOT NULL PRIMARY KEY IDENTITY(1,1),
		IncidenciaID INT NOT NULL,
		FolioAvisoLlegadaID INT NOT NULL,
		Activo BIT NOT NULL DEFAULT(1),
		FechaModificacion DATETIME NULL,
		UsuarioModificacion INT NULL,
		FOREIGN KEY (IncidenciaID) REFERENCES Sam3_Incidencia(IncidenciaID),
		FOREIGN KEY (FolioAvisoLlegadaID) REFERENCES Sam3_FolioAvisoLlegada(FolioAvisoLlegadaID)
	)

	CREATE TABLE Sam3_Rel_Incidencia_FolioCuantificacion(
		Rel_Incidencia_FolioCuantificacion_ID INT NOT NULL PRIMARY KEY IDENTITY(1,1),
		IncidenciaID INT NOT NULL,
		FolioCuantificacionID INT NOT NULL,
		Activo BIT NOT NULL DEFAULT(1),
		FechaModificacion DATETIME NULL,
		UsuarioModificacion INT NULL,
		FOREIGN KEY (IncidenciaID) REFERENCES Sam3_Incidencia(IncidenciaID),
		FOREIGN KEY (FolioCuantificacionID) REFERENCES Sam3_FolioCuantificacion(FolioCuantificacionID)
	)

	CREATE TABLE Sam3_Rel_Incidencia_OrdenRecepcion(
		Rel_Incidencia_OrdenRecepcion_ID INT NOT NULL PRIMARY KEY IDENTITY(1,1),
		IncidenciaID INT NOT NULL,
		OrdenRecepcionID INT NOT NULL,
		Activo BIT NOT NULL DEFAULT(1),
		FechaModificacion DATETIME NULL,
		UsuarioModificacion INT NULL,
		FOREIGN KEY (IncidenciaID) REFERENCES Sam3_Incidencia(IncidenciaID),
		FOREIGN KEY (OrdenRecepcionID) REFERENCES Sam3_OrdenRecepcion(OrdenRecepcionID)
	)

	CREATE TABLE Sam3_Rel_Incidencia_ComplementoRecepcion(
		Rel_Incidencia_ComplementoRecepcion_ID INT NOT NULL PRIMARY KEY IDENTITY(1,1),
		IncidenciaID INT NOT NULL,
		FolioAvisoEntradaID INT NOT NULL,
		Activo BIT NOT NULL DEFAULT(1),
		FechaModificacion DATETIME NULL,
		UsuarioModificacion INT NULL,
		FOREIGN KEY (IncidenciaID) REFERENCES Sam3_Incidencia(IncidenciaID),
		FOREIGN KEY (FolioAvisoEntradaID) REFERENCES Sam3_FolioAvisoEntrada(FolioAvisoEntradaID)
	)

	CREATE TABLE Sam3_Rel_Incidencia_ItemCode(
		Rel_Incidencia_ItemCode_ID INT NOT NULL PRIMARY KEY IDENTITY(1,1),
		IncidenciaID INT NOT NULL,
		ItemCodeID INT NOT NULL,
		Activo BIT NOT NULL DEFAULT(1),
		FechaModificacion DATETIME NULL,
		UsuarioModificacion INT NULL,
		FOREIGN KEY (IncidenciaID) REFERENCES Sam3_Incidencia(IncidenciaID),
		FOREIGN KEY (ItemCodeID) REFERENCES Sam3_ItemCode(ItemCodeID)
	)

	CREATE TABLE Sam3_Rel_Incidencia_OrdenAlmacenaje(
		Rel_Incidencia_OrdenAlmacenaje_ID INT NOT NULL PRIMARY KEY IDENTITY(1,1),
		IncidenciaID INT NOT NULL,
		OrdenalmacenajeID INT NOT NULL,
		Activo BIT NOT NULL DEFAULT(1),
		FechaModificacion DATETIME NULL,
		UsuarioModificacion INT NULL,
		FOREIGN KEY (IncidenciaID) REFERENCES Sam3_Incidencia(IncidenciaID),
		FOREIGN KEY (OrdenAlmacenajeID) REFERENCES Sam3_OrdenAlmacenaje(OrdenAlmacenajeID)
	)

	CREATE TABLE Sam3_Rel_Incidencia_Despacho(
		Rel_Incidencia_Despacho_ID INT NOT NULL PRIMARY KEY IDENTITY(1,1),
		IncidenciaID INT NOT NULL,
		DespachoID INT NOT NULL,
		Activo BIT NOT NULL DEFAULT(1),
		FechaModificacion DATETIME NULL,
		UsuarioModificacion INT NULL,
		FOREIGN KEY (IncidenciaID) REFERENCES Sam3_Incidencia(IncidenciaID),
		FOREIGN KEY (DespachoID) REFERENCES Sam3_Despacho(DespachoID)
	)

	CREATE TABLE Sam3_Rel_Incidencia_Corte(
		Rel_Incidencia_Corte_ID INT NOT NULL PRIMARY KEY IDENTITY(1,1),
		IncidenciaID INT NOT NULL,
		CorteID INT NOT NULL,
		Activo BIT NOT NULL DEFAULT(1),
		FechaModificacion DATETIME NULL,
		UsuarioModificacion INT NULL,
		FOREIGN KEY (IncidenciaID) REFERENCES Sam3_Incidencia(IncidenciaID),
		FOREIGN KEY (CorteID) REFERENCES Sam3_Corte(CorteID)
	)

	INSERT INTO Sam3_TipoIncidencia(Nombre, Activo, FechaModificacion, UsuarioModificacion)
	VALUES('Folio aviso entrada', 1, GETDATE(), 1)
	INSERT INTO Sam3_TipoIncidencia(Nombre, Activo, FechaModificacion, UsuarioModificacion)
	VALUES('Entrada de material', 1, GETDATE(), 1)
	INSERT INTO Sam3_TipoIncidencia(Nombre, Activo, FechaModificacion, UsuarioModificacion)
	VALUES('Pase salida', 0, GETDATE(), 1)
	INSERT INTO Sam3_TipoIncidencia(Nombre, Activo, FechaModificacion, UsuarioModificacion)
	VALUES('Packing list', 1, GETDATE(), 1)
	INSERT INTO Sam3_TipoIncidencia(Nombre, Activo, FechaModificacion, UsuarioModificacion)
	VALUES('Orden de recepción', 1, GETDATE(), 1)
	INSERT INTO Sam3_TipoIncidencia(Nombre, Activo, FechaModificacion, UsuarioModificacion)
	VALUES('Complemento de recepción', 0, GETDATE(), 1)
	INSERT INTO Sam3_TipoIncidencia(Nombre, Activo, FechaModificacion, UsuarioModificacion)
	VALUES('ItemCode', 1, GETDATE(), 1)
	INSERT INTO Sam3_TipoIncidencia(Nombre, Activo, FechaModificacion, UsuarioModificacion)
	VALUES('Orden de almacenaje', 1, GETDATE(), 1)
	INSERT INTO Sam3_TipoIncidencia(Nombre, Activo, FechaModificacion, UsuarioModificacion)
	VALUES('Número único', 1, GETDATE(), 1)
	INSERT INTO Sam3_TipoIncidencia(Nombre, Activo, FechaModificacion, UsuarioModificacion)
	VALUES('Despacho', 1, GETDATE(), 1)
	INSERT INTO Sam3_TipoIncidencia(Nombre, Activo, FechaModificacion, UsuarioModificacion)
	VALUES('Corte', 1, GETDATE(), 1)

	INSERT INTO Sam3_ClasificacionIncidencia(Nombre, Activo, FechaModificacion, UsuarioModificacion)
	VALUES('Materiales', 1, GETDATE(), 1)
	INSERT INTO Sam3_ClasificacionIncidencia(Nombre, Activo, FechaModificacion, UsuarioModificacion)
	VALUES('Ingeniería', 1, GETDATE(), 1)
	INSERT INTO Sam3_ClasificacionIncidencia(Nombre, Activo, FechaModificacion, UsuarioModificacion)
	VALUES('Calidad', 1, GETDATE(), 1)

	ALTER TABLE Sam3_Incidencia ADD
		TipoIncidenciaID INT NULL,
		MotivoCancelacion VARCHAR(MAX) NULL,
		DetalleResolucion VARCHAR(MAX) NULL,
		UsuarioResuelveID INT NULL,
		FechaSolucion DATETIME NULL,
		ClasificacionID INT NULL,
		FOREIGN KEY (TipoIncidenciaID) REFERENCES Sam3_TipoIncidencia(TipoIncidenciaId),
		FOREIGN KEY (UsuarioResuelveID) REFERENCES Sam3_Usuario(UsuarioID),
		FOREIGN KEY (ClasificacionID) REFERENCES Sam3_ClasificacionIncidencia(ClasificacionIncidenciaID)

	ALTER TABLE Sam3_Incidencia ALTER COLUMN TipoIncidenciaID INT NOT NULL
	ALTER TABLE Sam3_Incidencia ALTER COLUMN ClasificacionID INT NOT NULL

	CREATE TABLE [dbo].[Sam3_Rel_Incidencia_Documento](
		[Rel_Incidencia_DocumentoID] [int] IDENTITY(1,1) NOT NULL,
		[IncidenciaID] [int] NOT NULL,
		[DocumentoID] [int] NOT NULL,
		[Nombre] [varchar](200) NOT NULL,
		[Extencion] [varchar](5) NOT NULL,
		[Activo] [bit] NOT NULL,
		[UsuarioModificacion] [int] NULL,
		[FechaModificacion] [datetime] NULL,
		[DocGuid] [uniqueidentifier] NULL,
		[Url] [varchar](max) NULL,
		[TipoArchivoID] [int] NOT NULL,
		[ContentType] [varchar](50) NULL,
		[Descripcion] [varchar](max) NULL,
		FOREIGN KEY (IncidenciaID) REFERENCES Sam3_incidencia(IncidenciaID)
	)

	COMMIT TRAN
END TRY
BEGIN CATCH
	ROLLBACK TRAN
END CATCH

