IF EXISTS (SELECT * FROM sysobjects WHERE name='Sam3_Trg_ReplicarAcero') BEGIN
	DROP TRIGGER dbo.Sam3_Trg_ReplicarAcero
END
GO

CREATE TRIGGER Sam3_Trg_ReplicarAcero
ON Sam3_Acero
FOR INSERT
AS
BEGIN

	DECLARE @registroTemporal TABLE(
		AceroID INT,
		FamiliaAceroID INT,
		Nomenclatura VARCHAR(MAX) NULL,
		VerificadoPorCalidad bit
	)

	INSERT INTO @registroTemporal
			   (
				AceroID,
				FamiliaAceroID,
				Nomenclatura,
				VerificadoPorCalidad
			   )
			SELECT
				AceroID,
				FamiliaAceroID,
				Nomenclatura,
				VerificadoPorCalidad
			FROM inserted

	--Creo un cusor para recorrer los registros insertados
	DECLARE @sam2_familiaAceroID INT,
			@aceroID INT,
			@nomenclatura VARCHAR(MAX)


	DECLARE RecorrerRegistros CURSOR FOR
	SELECT AceroID FROM @registroTemporal
	OPEN RecorrerRegistros
	FETCH NEXT FROM RecorrerRegistros INTO @aceroID
	WHILE @@FETCH_STATUS = 0
	BEGIN
		SET @nomenclatura = (SELECT Nomenclatura 
							FROM @registroTemporal 
							WHERE AceroID = @aceroID)

		--Obtengo la equivalencia de ID de la familia de material de SAM 2
		SET @sam2_familiaAceroID = (SELECT Sam2_FamiliaAceroID 
									   FROM Sam3_EquivalenciaFamiliaAcero
									   WHERE Sam3_FamiliaAceroID =(
											SELECT FamiliaAceroID
											FROM @registroTemporal
											WHERE AceroID = @aceroID
										)
									   )

		--Si no existe un registro que corresponda a ese nombre y familia material
		IF (SELECT AceroID FROM SAM2.SAM.DBO.Acero
			WHERE Nomenclatura = @nomenclatura) is null
		BEGIN

			INSERT INTO SAM2.SAM.DBO.Acero
				(
					FamiliaAceroID,
					Nomenclatura,
					VerificadoPorCalidad,
					FechaModificacion
				)
			SELECT 
					@sam2_familiaAceroID,
					Nomenclatura,
					VerificadoPorCalidad,
					GETDATE()
			FROM @registroTemporal
			WHERE AceroID = @aceroID

			INSERT INTO [dbo].[Sam3_EquivalenciaAcero]
				   ([Sam2_AceroID]
				   ,[Sam3_AceroID]
				   ,[Activo]
				   ,[FechaModificacion]
				   ,[UsuarioModificacion])
			 SELECT 
				ISNULL((
					SELECT AceroID
					FROM SAM2.SAM.DBO.Acero
					WHERE Nomenclatura = @nomenclatura
				),0),
				AceroID,
				1,
				GETDATE(),
				1
			 FROM @registroTemporal
			 WHERE AceroID = @aceroID

		END

		FETCH NEXT FROM RecorrerRegistros INTO @aceroID
	END
	CLOSE RecorrerRegistros;
	DEALLOCATE RecorrerRegistros;


END
