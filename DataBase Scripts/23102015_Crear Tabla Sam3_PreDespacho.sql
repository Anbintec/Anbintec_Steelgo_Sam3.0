CREATE TABLE Sam3_PreDespacho
  (
  PreDespachoID int IDENTITY(1,1) PRIMARY KEY,
  ProyectoID int NOT NULL,
  OrdenTrabajoSpoolID int NOT NULL,
  MaterialSpoolID int NOT NULL,
  NumeroUnicoID int NOT NULL,
  Cantidad int NOT NULL,
  FechaPreDespacho datetime NULL,
  UsuarioModificacion int NULL,
  FechaModificacion datetime NULL,
  Activo bit NOT NULL,
  )

  
ALTER TABLE [dbo].Sam3_PreDespacho  WITH CHECK ADD FOREIGN KEY([NumeroUnicoID])
REFERENCES [dbo].[Sam3_NumeroUnico] ([NumeroUnicoID])
GO

ALTER TABLE [dbo].Sam3_PreDespacho  WITH CHECK ADD FOREIGN KEY([ProyectoID])
REFERENCES [dbo].[Sam3_Proyecto] ([ProyectoID])
GO

