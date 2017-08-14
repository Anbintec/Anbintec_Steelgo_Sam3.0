begin tran
begin try

DECLARE @id INT, @cantidad int, @relID int, @relBultoID int

DECLARE Actualizar CURSOR FOR
SELECT ItemCodeID, Cantidad from Sam3_ItemCode where Activo = 1
OPEN Actualizar
FETCH NEXT FROM Actualizar INTO @id, @cantidad
WHILE @@FETCH_STATUS = 0
BEGIN
	
	if (select Rel_FolioCuantificacion_ItemCode_ID from Sam3_Rel_FolioCuantificacion_ItemCode
			where ItemCodeID = @id and Activo = 1) > 0 
	begin
		DECLARE ActualizarRelFolioC CURSOR FOR
		SELECT Rel_FolioCuantificacion_ItemCode_ID from Sam3_Rel_FolioCuantificacion_ItemCode 
			where Activo = 1 and ItemCodeID = @id
		OPEN ActualizarRelFolioC
		FETCH NEXT FROM ActualizarRelFolioC INTO @relID
		WHILE @@FETCH_STATUS = 0
		BEGIN

			update Sam3_Rel_FolioCuantificacion_ItemCode set
				Cantidad = @cantidad
			where Rel_FolioCuantificacion_ItemCode_ID = @relID
			and ItemCodeID = @id

		FETCH NEXT FROM ActualizarRelFolioC INTO @relID
		END
		CLOSE ActualizarRelFolioC;
		DEALLOCATE ActualizarRelFolioC;
	end
	
	if (select Rel_Bulto_ItemCode_ID from Sam3_Rel_Bulto_ItemCode
			where ItemCodeID = @id and Activo = 1) > 0
	begin
		DECLARE ActualizarRelbulto CURSOR FOR
		SELECT Rel_Bulto_ItemCode_ID from Sam3_Rel_Bulto_ItemCode 
			where Activo = 1 and ItemCodeID = @id
		OPEN ActualizarRelbulto
		FETCH NEXT FROM ActualizarRelbulto INTO @relBultoID
		WHILE @@FETCH_STATUS = 0
		BEGIN

			update Sam3_Rel_Bulto_ItemCode set
				Cantidad = @cantidad
			where Rel_Bulto_ItemCode_ID = @relBultoID
			and ItemCodeID = @id

		FETCH NEXT FROM ActualizarRelbulto INTO @relBultoID
		END
		CLOSE ActualizarRelbulto;
		DEALLOCATE ActualizarRelbulto;
	end
	

	FETCH NEXT FROM Actualizar INTO @id, @cantidad
END
CLOSE Actualizar;
DEALLOCATE Actualizar;

commit tran
end try
begin catch
	rollback tran
end catch

--select * from Sam3_ItemCode where Cantidad <= 0 or Cantidad is null or Activo = 0
--select * from Sam3_Rel_FolioCuantificacion_ItemCode
--select * from Sam3_Rel_Bulto_ItemCode