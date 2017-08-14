IF EXISTS (SELECT * FROM sysobjects WHERE name='Sam3_Trg_ReplicarPatio') BEGIN
	DROP TRIGGER dbo.Sam3_Trg_ReplicarPatio
END
GO

CREATE TRIGGER Sam3_Trg_ReplicarPatio
ON Sam3_Patio
FOR INSERT
AS
BEGIN

	DECLARE @registroTemporal TABLE(
		PatioID INT,
		Nombre VARCHAR(MAX),
		Propietario VARCHAR(MAX),
		Descripcion VARCHAR(MAX)
	)

	INSERT INTO @registroTemporal
			   (
			    PatioID,
				Nombre,
				Propietario,
				Descripcion
			   )
			SELECT
				PatioID,
				Nombre,
				Propietario,
				Descripcion
			FROM inserted

	--Creo un cusor para recorrer los registros insertados
	DECLARE @patioID INT,
			@nombre VARCHAR(MAX)


	DECLARE RecorrerRegistros CURSOR FOR
	SELECT PatioID FROM @registroTemporal
	OPEN RecorrerRegistros
	FETCH NEXT FROM RecorrerRegistros INTO @patioID
	WHILE @@FETCH_STATUS = 0
	BEGIN
		SET @nombre = (SELECT Nombre 
							FROM @registroTemporal 
							WHERE PatioID = @patioID)

		--Si no existe un registro que corresponda a ese nombre y familia material
		IF (SELECT PatioID FROM SAM2.SAM.DBO.Patio
			WHERE Nombre = @nombre) is null
		BEGIN

			INSERT INTO SAM2.SAM.DBO.Patio
				(
					Nombre,
					Propietario,
					Descripcion,
					FechaModificacion
				)
			SELECT 
					Nombre,
					Propietario,
					Descripcion,
					GETDATE()
			FROM @registroTemporal
			WHERE PatioID = @patioID

			INSERT INTO [dbo].[Sam3_EquivalenciaPatio]
				   ([Sam2_PatioID]
				   ,[Sam3_PatioID]
				   ,[Activo]
				   ,[FechaModificacion]
				   ,[UsuarioModificacion])
			 SELECT 
				ISNULL((
					SELECT PatioID
					FROM SAM2.SAM.DBO.Patio
					WHERE Nombre = @nombre
				),0),
				PatioID,
				1,
				GETDATE(),
				1
			 FROM @registroTemporal
			 WHERE PatioID = @patioID

		END

		FETCH NEXT FROM RecorrerRegistros INTO @patioID
	END
	CLOSE RecorrerRegistros;
	DEALLOCATE RecorrerRegistros;


END
