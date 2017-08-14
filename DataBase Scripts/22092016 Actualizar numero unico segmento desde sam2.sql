ALTER TRIGGER ActualizarNUSegmentoEnSAM3
ON NumeroUnicoSegmento
AFTER UPDATE
AS
BEGIN

	Declare @sam3_NumeroUnicoID int,
			@sam3_ProyectoID int,
			@segmento nchar(1)

	IF UPDATE (InventarioCongelado)
	BEGIN
	--Obtener la equivalencia del numeroUnico
	SET @sam3_NumeroUnicoID = (
		SELECT Sam3_NumeroUnicoID FROM SAM3.[steelgo-sam3].dbo.Sam3_EquivalenciaNumeroUnico
		WHERE Sam2_NumeroUnicoID = (
			SELECT NumeroUnicoID FROM inserted
		)
	)
	--Obtener la equivalencia del proyecto
	SET @sam3_ProyectoID = (
		SELECT Sam3_ProyectoID FROM SAM3.[steelgo-sam3].dbo.Sam3_EquivalenciaProyecto
		WHERE Sam2_ProyectoID = (
			SELECT ProyectoID FROM inserted
		)
	)

	SET @segmento = (SELECT Segmento FROM inserted)

	--actualizar el inventario
	UPDATE SAM3.[steelgo-sam3].dbo.Sam3_NumeroUnicoSegmento SET
		InventarioBuenEstado = (SELECT InventarioBuenEstado FROM inserted),
		InventarioFisico = (SELECT InventarioFisico FROM inserted),
		InventarioCongelado = (SELECT InventarioCongelado FROM inserted),
		InventarioDisponibleCruce = (SELECT InventarioDisponibleCruce FROM inserted),
		CantidadDanada = (SELECT CantidadDanada FROM inserted),
		InventarioTransferenciaCorte = (SELECT InventarioTransferenciaCorte FROM inserted),
		FechaModificacion = GETDATE()
	WHERE NumeroUnicoID = @sam3_NumeroUnicoID 
	AND ProyectoID = @sam3_ProyectoID 
	AND Segmento = @segmento
	END
END