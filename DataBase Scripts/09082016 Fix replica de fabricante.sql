USE [steelgo-sam3]
GO
/****** Object:  Trigger [dbo].[Sam3_Trg_ReplicarFabricante]    Script Date: 09/08/2016 01:56:57 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER TRIGGER [dbo].[Sam3_Trg_ReplicarFabricante]
   ON  [dbo].[Sam3_Fabricante]
   FOR INSERT
AS 
BEGIN
	declare @sam2_FabricanteID int, @sam3_FabricanteID int

	set @sam3_FabricanteID = (
			select FabricanteID from inserted
		)

	IF( 
		(SELECT FabricanteID FROM sam2.sam.dbo.Fabricante where Nombre COLLATE DATABASE_DEFAULT = (
			SELECT Nombre FROM inserted
		)) is null
	)
	BEGIN
		INSERT INTO sam2.sam.dbo.Fabricante (Nombre, Descripcion, Direccion, Telefono)
			SELECT Nombre, Descripcion, Direccion, Telefono
			FROM inserted

		set @sam2_FabricanteID = (
			SELECT FabricanteID FROM sam2.sam.dbo.Fabricante where Nombre COLLATE DATABASE_DEFAULT = (
				SELECT Nombre FROM inserted
			)
		)

		INSERT INTO Sam3_EquivalenciaFabricante (Sam2_FabricanteID, Sam3_FabricanteID, Activo)
		values (@sam2_FabricanteID, @sam3_FabricanteID, 1)
	END
	ELSE

		set @sam2_FabricanteID = (
			SELECT FabricanteID FROM sam2.sam.dbo.Fabricante where Nombre COLLATE DATABASE_DEFAULT = (
				SELECT Nombre FROM inserted
			)
		)
		
		insert into Sam3_EquivalenciaFabricante (Sam2_FabricanteID, Sam3_FabricanteID, Activo)
		values (@sam2_FabricanteID, @sam3_FabricanteID, 1) 

END
