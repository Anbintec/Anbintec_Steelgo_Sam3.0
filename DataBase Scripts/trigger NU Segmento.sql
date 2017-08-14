IF EXISTS (SELECT * FROM sysobjects WHERE name='Sam3_Trg_ReplicarNumeroUnicoSegmento') BEGIN
	DROP TRIGGER dbo.Sam3_Trg_ReplicarNumeroUnicoSegmento
END
GO

CREATE TRIGGER Sam3_Trg_ReplicarNumeroUnicoSegmento
ON Sam3_NumeroUnicoSegmento
FOR INSERT
AS
BEGIN

	DECLARE @registroTemporal TABLE(
		[NumeroUnicoSegmentoID] [int],
		[NumeroUnicoID] [int],
		[ProyectoID] [int],
		[Segmento] [nchar](1),
		[CantidadDanada] [int],
		[InventarioFisico] [int],
		[InventarioBuenEstado] [int],
		[InventarioCongelado] [int] ,
		[InventarioTransferenciaCorte] [int],
		[InventarioDisponibleCruce] [int],
		[Rack] [varchar](50),
		[Activo] [bit],
		[UsuarioModificacion] [int],
		[FechaModificacion] [datetime]
	)

	INSERT INTO @registroTemporal
			   (
				NumeroUnicoSegmentoID,
				NumeroUnicoID,
				ProyectoID,
				Segmento,
				CantidadDanada,
				InventarioFisico,
				InventarioBuenEstado,
				InventarioCongelado,
				InventarioTransferenciaCorte,
				InventarioDisponibleCruce,
				Rack,
				Activo,
				UsuarioModificacion,
				FechaModificacion
			   )
			SELECT
				NumeroUnicoSegmentoID,
				NumeroUnicoID,
				ProyectoID,
				Segmento,
				CantidadDanada,
				InventarioFisico,
				InventarioBuenEstado,
				InventarioCongelado,
				InventarioTransferenciaCorte,
				InventarioDisponibleCruce,
				Rack,
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

		IF (SELECT NumeroUnicoID FROM SAM2.SAM.DBO.NumeroUnicoSegmento
			WHERE NumeroUnicoID = @sam2_numeroUnicoID) IS NULL
		BEGIN
			--insertar en sam2
			INSERT INTO SAM2.SAM.DBO.NumeroUnicoSegmento(
			   NumeroUnicoID,
			   ProyectoID,
			   Segmento,
			   CantidadDanada,
			   InventarioFisico,
			   InventarioBuenEstado,
			   InventarioCongelado,
			   InventarioTransferenciaCorte,
			   InventarioDisponibleCruce,
			   Rack
			)
			SELECT 
				@sam2_numeroUnicoID,
				@sam2_proyectoID,
				Segmento,
				CantidadDanada,
				InventarioFisico,
				InventarioBuenEstado,
				InventarioCongelado,
				InventarioTransferenciaCorte,
				InventarioDisponibleCruce,
				Rack
			FROM Sam3_NumeroUnicoSegmento nus
			WHERE nus.NumeroUnicoID = @numeroUnicoID


		END

		FETCH NEXT FROM RecorrerRegistros INTO @numeroUnicoID, @proyectoID
	END
	CLOSE RecorrerRegistros;
	DEALLOCATE RecorrerRegistros;


END
