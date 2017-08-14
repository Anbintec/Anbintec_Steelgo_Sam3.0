IF EXISTS (SELECT * FROM sysobjects WHERE name='Sam3_Trg_ReplicarNumeroUnicoInventario') BEGIN
	DROP TRIGGER dbo.Sam3_Trg_ReplicarNumeroUnicoInventario
END
GO

CREATE TRIGGER Sam3_Trg_ReplicarNumeroUnicoInventario
ON Sam3_NumeroUnicoInventario
FOR INSERT
AS
BEGIN

	DECLARE @registroTemporal TABLE(
		[NumeroUnicoID] [int],
		[ProyectoID] [int],
		[CantidadRecibida] [int],
		[CantidadDanada] [int],
		[InventarioFisico] [int],
		[InventarioBuenEstado] [int],
		[InventarioCongelado] [int],
		[InventarioTransferenciaCorte] [int],
		[InventarioDisponibleCruce] [int],
		[EsVirtual] [bit],
		[Activo] [bit],
		[UsuarioModificacion] [int],
		[FechaModificacion] [datetime]
	)

	INSERT INTO @registroTemporal
			   (
				NumeroUnicoID,
				ProyectoID,
				CantidadRecibida,
				CantidadDanada,
				InventarioFisico,
				InventarioBuenEstado,
				InventarioCongelado,
				InventarioTransferenciaCorte,
				InventarioDisponibleCruce,
				EsVirtual,
				Activo,
				UsuarioModificacion,
				FechaModificacion
			   )
			SELECT
				NumeroUnicoID,
				ProyectoID,
				CantidadRecibida,
				CantidadDanada,
				InventarioFisico,
				InventarioBuenEstado,
				InventarioCongelado,
				InventarioTransferenciaCorte,
				InventarioDisponibleCruce,
				EsVirtual,
				Activo,
				UsuarioModificacion,
				FechaModificacion
			FROM inserted

	--Creo un cusor para recorrer los registros insertados
	DECLARE @numeroUnicoID INT,
			@sam2_numeroUnicoID INT,
			@proyectoID INT,
			@sam2_proyectoID INT


	DECLARE RecorrerRegistros CURSOR FOR
	SELECT NumeroUnicoID, ProyectoID FROM @registroTemporal
	OPEN RecorrerRegistros
	FETCH NEXT FROM RecorrerRegistros INTO @numeroUnicoID, @proyectoID
	WHILE @@FETCH_STATUS = 0
	BEGIN

		SET @sam2_numeroUnicoID = (
				SELECT Sam2_NumeroUnicoID FROM Sam3_EquivalenciaNumeroUnico
				WHERE Sam3_NumeroUnicoID = @numeroUnicoID
			)

		SET @sam2_proyectoID = (
			SELECT SAm2_ProyectoID FROM Sam3_EquivalenciaProyecto 
			WHERE Sam3_ProyectoID = @proyectoID
		)

		IF (SELECT NumeroUnicoID FROM SAM2.SAM.DBO.NumeroUnicoInventario
			WHERE NumeroUnicoID = @sam2_numeroUnicoID) IS NULL
		BEGIN
			--insertar en sam2
			INSERT INTO SAM2.SAM.DBO.NumeroUnicoInventario
					   ([NumeroUnicoID]
					   ,[ProyectoID]
					   ,[CantidadRecibida]
					   ,[CantidadDanada]
					   ,[InventarioFisico]
					   ,[InventarioBuenEstado]
					   ,[InventarioCongelado]
					   ,[InventarioTransferenciaCorte]
					   ,[InventarioDisponibleCruce]
					   )
			SELECT 
				@sam2_numeroUnicoID,
				@sam2_proyectoID,
				CantidadRecibida,
				CantidadDanada,
				InventarioFisico,
				InventarioBuenEstado,
				InventarioCongelado,
				InventarioTransferenciaCorte,
				InventarioDisponibleCruce
			FROM Sam3_NumeroUnicoInventario nui
			WHERE nui.NumeroUnicoID = @numeroUnicoID


		END

		FETCH NEXT FROM RecorrerRegistros INTO @numeroUnicoID, @proyectoID
	END
	CLOSE RecorrerRegistros;
	DEALLOCATE RecorrerRegistros;


END
