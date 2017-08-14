IF EXISTS (SELECT * FROM sysobjects WHERE name='Sam3_Trg_ReplicarItemCode') BEGIN
	DROP TRIGGER dbo.Sam3_Trg_ReplicarItemCode
END
GO

CREATE TRIGGER Sam3_Trg_ReplicarItemCode
ON Sam3_ItemCode
FOR INSERT
AS
BEGIN

	DECLARE @registroTemporal TABLE(
		ItemCodeID INT,
		ProyectoID INT,
		TipoMaterialID INT,
		Codigo VARCHAR(MAX),
		ItemCodeCliente VARCHAR(MAX),
		DescripcionEspanol VARCHAR(MAX),
		DescripcionIngles VARCHAR(MAX),
		Peso DECIMAL,
		DescripcionInterna VARCHAR(MAX),
		Diametro1 DECIMAL,
		Diametro2 DECIMAL,
		FamiliaAceroID INT,
		Cantidad INT,
		ColadaID INT
	)

	INSERT INTO @registroTemporal
			   (
			    ItemCodeID,
				ProyectoID,
				TipoMaterialID,
				Codigo,
				ItemCodeCliente,
				DescripcionEspanol,
				DescripcionIngles,
				Peso,
				DescripcionInterna,
				Diametro1,
				Diametro2,
				FamiliaAceroID,
				Cantidad,
				ColadaID
			   )
			SELECT
				ItemCodeID,
				ProyectoID,
				TipoMaterialID,
				Codigo,
				ItemCodeCliente,
				DescripcionEspanol,
				DescripcionIngles,
				Peso,
				DescripcionInterna,
				Diametro1,
				Diametro2,
				FamiliaAceroID,
				Cantidad,
				ColadaID
			FROM inserted

	--Creo un cusor para recorrer los registros insertados
	DECLARE @itemCodeID INT,
			@codigo VARCHAR(MAX),
			@proyectoID INT,
			@sam2_ProyectoID INT,
			@Sam2_itemCodeID INT,
			@Sam2_FamiliaAceroID INT


	DECLARE RecorrerRegistros CURSOR FOR
	SELECT ProyectoID, Codigo, ItemCodeID FROM @registroTemporal
	OPEN RecorrerRegistros
	FETCH NEXT FROM RecorrerRegistros INTO @proyectoID, @codigo, @itemCodeID
	WHILE @@FETCH_STATUS = 0
	BEGIN

		SET @sam2_ProyectoID = (SELECT Sam2_ProyectoID FROM Sam3_EquivalenciaProyecto
								WHERE Sam3_ProyectoID = @proyectoID)

		IF (SELECT ItemCodeID FROM SAM2.SAM.DBO.ItemCode
			WHERE ProyectoID = @sam2_ProyectoID AND Codigo = @codigo) IS NULL
		BEGIN

			INSERT INTO SAM2.SAM.DBO.ItemCode(
				ProyectoID,
				TipoMaterialID,
				Codigo,
				ItemCodeCliente,
				DescripcionEspanol,
				DescripcionIngles,
				Peso,
				DescripcionInterna,
				Diametro1,
				Diametro2,
				FamiliaAceroID
			)
			SELECT 
				@sam2_ProyectoID,
				it.TipoMaterialID,
				it.Codigo,
				it.ItemCodeCliente,
				it.DescripcionEspanol,
				it.DescripcionIngles,
				it.Peso,
				it.DescripcionInterna,
				it.Diametro1,
				it.Diametro2,
				it.FamiliaAceroID
			FROM Sam3_ItemCode it
			WHERE it.ItemCodeID = @itemCodeID

			--Insertar equivalencia
			INSERT INTO Sam3_EquivalenciaItemCode(
				Sam2_ItemCodeID,
				Sam3_ItemCodeID,
				Activo,
				FechaModificacion,
				UsuarioModificacion
			)
			SELECT 
				ISNULL((
					SELECT ItemCodeID FROM SAM2.SAM.DBO.ItemCode
					WHERE ProyectoID = @sam2_ProyectoID AND Codigo = @codigo
				),0),
				it.ItemCodeID,
				1,
				GETDATE(),
				1
			FROM Sam3_ItemCode it
			WHERE it.ItemCodeID = @itemCodeID
		END
		ELSE -- El ItemCode ya Existe
			
			SET @Sam2_itemCodeID = (SELECT ItemCodeID FROM SAM2.SAM.DBO.ItemCode
									WHERE ProyectoID = @sam2_ProyectoID AND Codigo = @codigo)

			SET @Sam2_FamiliaAceroID = (
				SELECT Sam2_FamiliaAceroID FROM Sam3_EquivalenciaFamiliaAcero
				WHERE Sam3_FamiliaAceroID = (SELECT FamiliaAceroID FROM @registroTemporal WHERE ItemCodeID = @itemCodeID)
			)
			
			--Actualizar informacion del ItemCode
			UPDATE SAM2.SAM.DBO.ItemCode SET
				Codigo = (SELECT Codigo FROM @registroTemporal WHERE ItemCodeID = @itemCodeID),
				ItemCodeCliente = (SELECT ItemCodeCliente FROM @registroTemporal WHERE ItemCodeID = @itemCodeID),
				DescripcionEspanol = (SELECT DescripcionEspanol FROM @registroTemporal WHERE ItemCodeID = @itemCodeID),
				DescripcionIngles = (SELECT DescripcionIngles FROM @registroTemporal WHERE ItemCodeID = @itemCodeID),
				Peso = (SELECT Peso FROM @registroTemporal WHERE ItemCodeID = @itemCodeID),
				DescripcionInterna =(SELECT DescripcionInterna FROM @registroTemporal WHERE ItemCodeID = @itemCodeID),
				Diametro1 = (SELECT Diametro1 FROM @registroTemporal WHERE ItemCodeID = @itemCodeID),
				Diametro2 = (SELECT Diametro2 FROM @registroTemporal WHERE ItemCodeID = @itemCodeID),
				FamiliaAceroID = @Sam2_FamiliaAceroID
			WHERE ItemCodeID = @Sam2_itemCodeID

			IF (SELECT EquivalenciaItemCodeID FROM Sam3_EquivalenciaItemCode
				WHERE Sam2_ItemCodeID = @Sam2_itemCodeID AND Sam3_ItemCodeID = @itemCodeID) IS NULL
			BEGIN

				INSERT INTO Sam3_EquivalenciaItemCode
				(
					Sam2_ItemCodeID,
					Sam3_ItemCodeID,
					Activo,
					FechaModificacion,
					UsuarioModificacion
				)
				VALUES
				(
					@Sam2_itemCodeID,
					@itemCodeID,
					1,
					GETDATE(),
					1
				)

			END

		FETCH NEXT FROM RecorrerRegistros INTO @proyectoID, @codigo, @itemCodeID
	END
	CLOSE RecorrerRegistros;
	DEALLOCATE RecorrerRegistros;


END
