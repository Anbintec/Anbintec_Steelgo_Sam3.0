IF EXISTS (SELECT * FROM sysobjects WHERE name='Sam3_Trg_ReplicarFamiliaMaterial') BEGIN
	DROP TRIGGER dbo.Sam3_Trg_ReplicarFamiliaMaterial
END
GO

CREATE TRIGGER Sam3_Trg_ReplicarFamiliaMaterial
ON Sam3_FamiliaMaterial
FOR INSERT
AS
BEGIN

	DECLARE @registroTemporal TABLE(
		Nombre VARCHAR(MAX) NULL,
		Descripcion VARCHAR(MAX) NULL
	)

	INSERT INTO @registroTemporal
			   (Nombre
			   ,Descripcion)
			SELECT 
				Nombre,
				Descripcion
			FROM inserted

	--Creo un cusor para recorrer los registros insertados
	DECLARE @nombre VARCHAR(MAX), @descripcion VARCHAR(MAX) 
	DECLARE RecorrerRegistros CURSOR FOR
	SELECT Nombre, Descripcion FROM @registroTemporal
	OPEN RecorrerRegistros
	FETCH NEXT FROM RecorrerRegistros INTO @nombre, @descripcion
	WHILE @@FETCH_STATUS = 0
	BEGIN
		
		IF (SELECT FamiliaMaterialID FROM SAM2.SAM.DBO.FamiliaMaterial
			WHERE Nombre = @nombre) is null
		BEGIN

			INSERT INTO SAM2.SAM.DBO.FamiliaMaterial
				(
					Nombre
					, Descripcion
					, FechaModificacion
				)
			VALUES
				(
					@nombre
					, @descripcion
					, GETDATE()
				)

			INSERT INTO [dbo].[Sam3_EquivalenciaFamiliaMaterial]
				   ([Sam2_FamiliaMaterialID]
				   ,[Sam3_FamiliaMaterialID]
				   ,[Activo]
				   ,[FechaModificacion]
				   ,[UsuarioModificacion])
			 VALUES
				   (
				    (SELECT FamiliaMaterialID 
					 FROM SAM2.SAM.DBO.FamiliaMaterial
					 WHERE Nombre = @nombre 
					 ),
					 (
					 SELECT FamiliaMaterialID
					 FROM Sam3_FamiliaMaterial
					 WHERE Nombre = @nombre
					 ),
					 1,
					 GETDATE(),
					 1
				   )

		END

		FETCH NEXT FROM RecorrerRegistros INTO @nombre, @descripcion
	END
	CLOSE RecorrerRegistros;
	DEALLOCATE RecorrerRegistros;


END
