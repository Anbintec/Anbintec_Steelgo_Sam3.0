begin tran
begin try

	alter table [dbo].[Sam3_Rel_NumeroUnico_RelFC_RelB]
	add MM int null default (0)

commit tran 
end try
begin catch
	rollback tran
end catch
go

begin tran
begin try

	update [dbo].[Sam3_Rel_NumeroUnico_RelFC_RelB] set MM = 0
	where MM is null

	alter table [dbo].[Sam3_Rel_NumeroUnico_RelFC_RelB]
	alter column MM int not null


commit tran 
end try
begin catch
	rollback tran
end catch
go

DECLARE @id INT
DECLARE Actualizar CURSOR FOR
SELECT Rel_FolioCuantificacion_ItemCode_ID from Sam3_Rel_FolioCuantificacion_ItemCode
OPEN Actualizar
FETCH NEXT FROM Actualizar INTO @id
WHILE @@FETCH_STATUS = 0
BEGIN
	
	update [dbo].[Sam3_Rel_NumeroUnico_RelFC_RelB] set MM = (
		isnull((select MM from Sam3_Rel_FolioCuantificacion_ItemCode
		where Rel_FolioCuantificacion_ItemCode_ID = @id), 0)
	)
	where MM = 0 and Rel_FolioCuantificacion_ItemCode_ID = @id

	FETCH NEXT FROM Actualizar INTO @id
END
CLOSE Actualizar;
DEALLOCATE Actualizar;
go


DECLARE @id INT
DECLARE Actualizar CURSOR FOR
SELECT Rel_Bulto_ItemCode_ID from Sam3_Rel_Bulto_ItemCode
OPEN Actualizar
FETCH NEXT FROM Actualizar INTO @id
WHILE @@FETCH_STATUS = 0
BEGIN
	
	update [dbo].[Sam3_Rel_NumeroUnico_RelFC_RelB] set MM = (
		isnull((select MM from Sam3_Rel_FolioCuantificacion_ItemCode
		where Rel_FolioCuantificacion_ItemCode_ID = @id), 0)
	)
	where MM = 0 and Rel_Bulto_ItemCode_ID = @id

	FETCH NEXT FROM Actualizar INTO @id
END
CLOSE Actualizar;
DEALLOCATE Actualizar;
go

select * from [dbo].[Sam3_Rel_NumeroUnico_RelFC_RelB]