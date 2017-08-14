IF EXISTS (SELECT * FROM sysobjects WHERE name='Sam3_Trg_ReplicarNumeroUnico') BEGIN
	DROP TRIGGER dbo.Sam3_Trg_ReplicarNumeroUnico
END
GO

CREATE TRIGGER Sam3_Trg_ReplicarNumeroUnico
ON NumeroUnico
FOR UPDATE
AS
BEGIN

	DECLARE @registroTemporal TABLE(
		NumeroUnicoID int,
		Codigo int,
		ProyectoID int,
        ItemCodeID int,
        ColadaID int,
        ProveedorID int,
        FabricanteID int,
        TipoCorte1ID int,
        TipoCorte2ID int,
        Estatus nchar(1),
        Factura nvarchar(25),
        PartidaFactura nvarchar(10),
        OrdenDeCompra nvarchar(20),
        PartidaOrdenDeCompra nvarchar(10),
        Diametro1 decimal(7,4),
        Diametro2 decimal(7,4),
        Cedula nvarchar(10),
        NumeroUnicoCliente nvarchar(50),
        MarcadoAsme bit,
        MarcadoGolpe bit,
        MarcadoPintura bit,
        TieneDano bit,
        Rack varchar(50),
        EsVirtual bit,
        RecepcionID int,
        Activo bit,
        UsuarioModificacion int,
        FechaModificacion datetime,
        Prefijo varchar(20),
        Consecutivo int
	)

	INSERT INTO @registroTemporal
			   (
			    NumeroUnicoID,
				Codigo,
				ProyectoID,
				ItemCodeID,
				ColadaID,
				ProveedorID,
				FabricanteID,
				TipoCorte1ID,
				TipoCorte2ID,
				Estatus,
				Factura,
				PartidaFactura,
				OrdenDeCompra,
				PartidaOrdenDeCompra,
				Diametro1,
				Diametro2,
				Cedula,
				NumeroUnicoCliente,
				MarcadoAsme,
				MarcadoGolpe,
				MarcadoPintura,
				TieneDano,
				Rack,
				FechaModificacion
			   )
			SELECT
				NumeroUnicoID,
				Codigo,
				ProyectoID,
				ItemCodeID,
				ColadaID,
				ProveedorID,
				FabricanteID,
				TipoCorte1ID,
				TipoCorte2ID,
				Estatus,
				Factura,
				PartidaFactura,
				OrdenDeCompra,
				PartidaOrdenDeCompra,
				Diametro1,
				Diametro2,
				Cedula,
				NumeroUnicoCliente,
				MarcadoAsme,
				MarcadoGolpe,
				MarcadoPintura,
				TieneDano,
				Rack,
				FechaModificacion
			FROM inserted

	--Creo un cusor para recorrer los registros insertados
	DECLARE @numeroUnicoID INT,
			@sam3_numeroUnicoID INT,
			@sam3_ProyectoID INT,
			@proyectoID INT,
			@sam3_ItemCodeID INT,
			@itemCodeID INT,
			@sam3_ProveedorID INT, 
			@proveedorID INT,
			@sam3_FabricanteID INT,
			@fabricanteID INT,
			@codigo VARCHAR(100),
			@numeroDigitos INT,
			@prefijo varchar(100),
			@consecutivo int


	DECLARE RecorrerRegistros CURSOR FOR
	SELECT NumeroUnicoID, ProyectoID, ItemCodeID, ProveedorID, FabricanteID, Codigo
		FROM @registroTemporal
	OPEN RecorrerRegistros
	FETCH NEXT FROM RecorrerRegistros 
		INTO @numeroUnicoID, @proyectoID, @itemCodeID, @proveedorID, @fabricanteID, @codigo
	WHILE @@FETCH_STATUS = 0
	BEGIN

		SET @sam3_proyectoID = (
			SELECT Sam3_ProyectoID FROM SAM3.[steelgo-sam3].DBO.Sam3_EquivalenciaProyecto
			WHERE Sam2_ProyectoID = @proyectoID 
			AND Activo = 1
		)
		
		SET @sam3_ItemCodeID = (
			SELECT SAM3_ItemCodeID FROM SAM3.[steelgo-sam3].DBO.Sam3_EquivalenciaItemCode
			WHERE Sam2_ItemCodeID = @itemCodeID
			AND Activo = 1
		)

		SET @sam3_ProveedorID = (
			SELECT Sam3_ProveedorID FROM SAM3.[steelgo-sam3].DBO.Sam3_EquivalenciaProveedor
			WHERE Sam2_ProveedorID = @proveedorID
			AND Activo = 1
		)

		SET @sam3_FabricanteID = (
			SELECT Sam3_FabricanteID FROM SAM3.[steelgo-sam3].DBO.Sam3_EquivalenciaFabricante
			WHERE Sam2_FabricanteID = @fabricanteID
			AND Activo = 1
 		)

		SET @numeroDigitos = (
			SELECT DigitosNumeroUnico FROM ProyectoConfiguracion
			WHERE ProyectoID = @proyectoID
		)

		SET @codigo = @prefijo + '-' + 
			(REPLICATE('0',(@numeroDigitos - LEN(@consecutivo))) + CONVERT(varchar, @consecutivo))

		--Verificamos si existe este numero unico en la base de sam3
		IF (SELECT NumeroUnicoID FROM SAM3.[steelgo-sam3].DBO.Sam3_NumeroUnico
			WHERE Codigo = @codigo AND ProyectoID = @proyectoID) IS NULL
		BEGIN
			--insertamos el numero Unico
			INSERT INTO SAM2.SAM.DBO.NumeroUnico(
			    [ProyectoID]
			   ,[ItemCodeID]
			   ,[ColadaID]
			   ,[ProveedorID]
			   ,[FabricanteID]
			   ,[TipoCorte1ID]
			   ,[TipoCorte2ID]
			   ,[Codigo]
			   ,[Estatus]
			   ,[Factura]
			   ,[PartidaFactura]
			   ,[OrdenDeCompra]
			   ,[PartidaOrdenDeCompra]
			   ,[Diametro1]
			   ,[Diametro2]
			   ,[Cedula]
			   ,[NumeroUnicoCliente]
			   ,[MarcadoAsme]
			   ,[MarcadoGolpe]
			   ,[MarcadoPintura]
			   ,[TieneDano]
			   ,[Rack]
		   )
		   SELECT 
				@sam3_ProyectoID,
				@sam3_ItemCodeID,
				ColadaID,
				@sam3_ProveedorID,
				@sam3_FabricanteID,
				NULL,
				NULL,
				@codigo,
				'A',
				Factura,
				PartidaFactura,
				OrdenDeCompra,
				PartidaOrdenDeCompra,
				Diametro1,
				Diametro2,
				Cedula,
				NumeroUnicoCliente,
				MarcadoAsme,
				MarcadoGolpe,
				MarcadoPintura,
				TieneDano,
				Rack
		   FROM @registroTemporal r
		   WHERE r.NumeroUnicoID = @numeroUnicoID

		   --recuperamos el ID del numero unico en sam2
		   SET @sam3_numeroUnicoID = SCOPE_IDENTITY()

		   IF (@sam3_numeroUnicoID) IS NULL
		   BEGIN
				SET @sam3_numeroUnicoID = (
					SELECT NumeroUnicoID FROM SAM2.SAM.DBO.NumeroUnico
					WHERE Codigo = @codigo AND ProyectoID = @proyectoID
				)
		   END

		   --insertamos en la tabla de equivalencias
		   INSERT INTO Sam3_EquivalenciaNumeroUnico(
				Sam2_NumeroUnicoID,
				Sam3_NumeroUnicoID,
				Activo,
				FechaModificacion,
				UsuarioModificacion
		   )
		   VALUES(
				@sam2_numeroUnicoID,
				@numeroUnicoID,
				1,
				GETDATE(),
				1
		   )

		END
		
		FETCH NEXT FROM RecorrerRegistros 
		INTO @numeroUnicoID, @proyectoID, @itemCodeID, @proveedorID, @fabricanteID, @prefijo, @consecutivo
	END
	CLOSE RecorrerRegistros;
	DEALLOCATE RecorrerRegistros;

END
