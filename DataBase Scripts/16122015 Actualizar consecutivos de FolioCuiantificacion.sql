DECLARE @id INT, @consecutivo int, @folioCID int

DECLARE Actualizar CURSOR FOR
SELECT FolioAvisoEntradaID from Sam3_FolioAvisoEntrada
OPEN Actualizar
FETCH NEXT FROM Actualizar INTO @id
WHILE @@FETCH_STATUS = 0
BEGIN
	
	set @consecutivo = 1

	DECLARE ActualizarFC CURSOR FOR
	SELECT FolioCuantificacionID from Sam3_FolioCuantificacion 
	where FolioAvisoEntradaID = @id
	OPEN ActualizarFC
	FETCH NEXT FROM ActualizarFC INTO @folioCID
	WHILE @@FETCH_STATUS = 0
	BEGIN
		
		update Sam3_FolioCuantificacion set Consecutivo = @consecutivo
		where FolioCuantificacionID = @folioCID
		
		set @consecutivo = @consecutivo + 1	

		FETCH NEXT FROM ActualizarFC INTO @folioCID
	END
	CLOSE ActualizarFC;
	DEALLOCATE ActualizarFC;

	FETCH NEXT FROM Actualizar INTO @id
END
CLOSE Actualizar;
DEALLOCATE Actualizar;