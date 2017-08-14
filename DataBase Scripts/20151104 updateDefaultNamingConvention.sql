--====================================
--  Create database trigger template 
--====================================
USE [steelgo-sam3]
GO

CREATE TRIGGER updateDefaultNamingConvention ON Sam3_Rel_FolioAvisoLlegada_Proyecto AFTER INSERT,UPDATE 
AS 
	DECLARE @ProyectoID int, @folioID int, @ProyectoIDActual int, @EntidadIDToBe int, @ConsecutivoToBe int
	--Get Updated Values
	SET @ProyectoID = (SELECT ProyectoID FROM inserted)
	SET @folioID = (SELECT FolioAvisoLlegadaID FROM inserted)
	--Get Current Project
	SET @ProyectoIDActual = (SELECT fal.ProyectoNombrado FROM Sam3_FolioAvisoLlegada fal WHERE fal.FolioAvisoLlegadaID = @folioID)
	--Check if the current value it's default
	IF (@ProyectoIDActual = 1 AND @ProyectoIDActual != @ProyectoID)
		--Obtain base entity and consecutive value
		SELECT @EntidadIDToBe = rpec.Entidad, @ConsecutivoToBe = rpec.ConsecutivoFolioAvisoLlegada FROM Sam3_Rel_Proyecto_Entidad_Configuracion rpec WHERE rpec.Proyecto = @ProyectoID AND rpec.Activo = 1;
	IF (@ProyectoIDActual = 1 AND @ProyectoIDActual != @ProyectoID)
		--UPDATE CONSECUTIVO IN Sam3_Rel_Proyecto_Entidad_Configuracion
		UPDATE Sam3_Rel_Proyecto_Entidad_Configuracion SET ConsecutivoFolioAvisoLlegada = (@ConsecutivoToBe + 1) WHERE Proyecto = @ProyectoID AND Activo = 1;
	IF (@ProyectoIDActual = 1 AND @ProyectoIDActual != @ProyectoID)
		--UPDATE @EntidadIDToBe & @ConsecutivoToBe IN Sam3_FolioAvisoLlegada
		UPDATE Sam3_FolioAvisoLlegada SET Consecutivo = @ConsecutivoToBe,ProyectoNombrado = @ProyectoID WHERE FolioAvisoLlegadaID = @folioID;
GO