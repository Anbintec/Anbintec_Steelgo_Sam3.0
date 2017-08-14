begin try
begin tran

	DECLARE @relItemDiametroID int, @itemCodeID int

	DECLARE RecorrerRegistros CURSOR FOR
	SELECT ItemCodeID FROM Sam3_ItemCode where Activo = 1
	OPEN RecorrerRegistros
	FETCH NEXT FROM RecorrerRegistros INTO @itemCodeID
	WHILE @@FETCH_STATUS = 0
	BEGIN
		
		set @relItemDiametroID = (
			select Rel_ItemCode_Diametro_ID
			from Sam3_Rel_ItemCode_Diametro d 
			where d.ItemCodeID = @itemCodeID
		)

		update Sam3_Rel_FolioCuantificacion_ItemCode 
			set Rel_ItemCode_Diametro_ID = @relItemDiametroID
		where ItemCodeID = @itemCodeID

		update Sam3_Rel_Bulto_ItemCode
			set Rel_ItemCode_Diametro_ID = @relItemDiametroID
		where ItemCodeID = @itemCodeID

		update Sam3_Rel_OrdenRecepcion_ItemCode
			set Rel_ItemCode_Diametro_ID = @relItemDiametroID
		where ItemCodeID = @itemCodeID

		FETCH NEXT FROM RecorrerRegistros INTO @itemCodeID
	END
	CLOSE RecorrerRegistros;
	DEALLOCATE RecorrerRegistros;

	--select * from Sam3_Rel_FolioCuantificacion_ItemCode
	--select * from Sam3_Rel_Bulto_ItemCode
	--select * from Sam3_Rel_OrdenRecepcion_ItemCode

commit tran
end try
begin catch
	EXECUTE Sam3_GetErrorInfo;
	rollback tran
end catch
go