IF EXISTS (SELECT * FROM sysobjects WHERE name='Sam3_Trg_ReplicarFamiliaAcero') BEGIN
	DROP TRIGGER dbo.Sam3_Trg_ReplicarFamiliaAcero
END
GO

CREATE TRIGGER Sam3_Trg_ReplicarFamiliaAcero
ON Sam3_FamiliaAcero
FOR INSERT
AS
BEGIN

	DECLARE @registroTemporal TABLE(
		FamiliaAceroID INT,
		FamiliaMaterialID INT,
		Nombre VARCHAR(MAX) NULL,
		Descripcion VARCHAR(MAX) NULL,
		VerificadoPorCalidad bit
	)

	INSERT INTO @registroTemporal
			   (FamiliaAceroID
			   ,FamiliaMaterialID
			   ,Nombre
			   ,Descripcion
			   ,VerificadoPorCalidad)
			SELECT
				FamiliaAceroID, 
				FamiliaMaterialID,
				Nombre,
				Descripcion,
				VerificadoPorCalidad
			FROM inserted

	--Creo un cusor para recorrer los registros insertados
	DECLARE @sam2_familiaMaterialID INT,
			@familiaAceroID INT,
			@nombre VARCHAR(MAX)


	DECLARE RecorrerRegistros CURSOR FOR
	SELECT FamiliaAceroID FROM @registroTemporal
	OPEN RecorrerRegistros
	FETCH NEXT FROM RecorrerRegistros INTO @FamiliaAceroID
	WHILE @@FETCH_STATUS = 0
	BEGIN
		SET @nombre = (SELECT Nombre 
							FROM @registroTemporal 
							WHERE FamiliaAceroID = @familiaAceroID)

		--Obtengo la equivalencia de ID de la familia de material de SAM 2
		SET @sam2_familiaMaterialID = (SELECT Sam2_FamiliaMaterialID 
									   FROM Sam3_EquivalenciaFamiliaMaterial
									   WHERE Sam3_FamiliaMaterialID =(
											SELECT FamiliaMaterialID
											FROM @registroTemporal
											WHERE FamiliaAceroID = @familiaAceroID
										)
									   )
		--Si no existe un registro que corresponda a ese nombre y familia material
		IF (SELECT FamiliaAceroID FROM SAM2.SAM.DBO.FamiliaAcero
			WHERE Nombre = @nombre AND FamiliaMaterialID = @sam2_familiaMaterialID 
			) is null
		BEGIN

			INSERT INTO SAM2.SAM.DBO.FamiliaAcero
				(
					FamiliaMaterialID,
					Nombre,
					Descripcion,
					VerificadoPorCalidad,
					FechaModificacion
				)
			SELECT 
					@sam2_familiaMaterialID,
					Nombre,
					Descripcion,
					VerificadoPorCalidad,
					GETDATE()
			FROM @registroTemporal
			WHERE FamiliaAceroID = @familiaAceroID


			INSERT INTO [dbo].[Sam3_EquivalenciaFamiliaAcero]
					   ([Sam2_FamiliaAceroID]
					   ,[Sam3_FamiliaAceroID]
					   ,[Activo]
					   ,[FechaModificacion]
					   ,[UsuarioModificacion])
				 SELECT 
					ISNULL((
						SELECT FamiliaAceroID
						FROM SAM2.SAM.DBO.FamiliaAcero
						WHERE Nombre = @nombre
						AND FamiliaMaterialID = @sam2_familiaMaterialID
					),0),
					rt.FamiliaAceroID,
					1,
					GETDATE(),
					1
				 FROM @registroTemporal rt
				 WHERE rt.FamiliaAceroID = @familiaAceroID
		END

		FETCH NEXT FROM RecorrerRegistros INTO @familiaAceroID
	END
	CLOSE RecorrerRegistros;
	DEALLOCATE RecorrerRegistros;


END
