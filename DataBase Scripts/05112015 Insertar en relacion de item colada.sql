begin tran
begin try

DECLARE @id INT

DECLARE Actualizar CURSOR FOR
SELECT ItemCodeID from Sam3_ItemCode
OPEN Actualizar
FETCH NEXT FROM Actualizar INTO @id
WHILE @@FETCH_STATUS = 0
BEGIN
	
	insert into Sam3_Rel_Itemcode_Colada(ItemCodeID, ColadaID, Activo)
	values (@id, 1, 1)

	FETCH NEXT FROM Actualizar INTO @id
END
CLOSE Actualizar;
DEALLOCATE Actualizar;

commit tran
end try
begin catch
	rollback tran
end catch

select * from [dbo].[Sam3_Rel_Itemcode_Colada]