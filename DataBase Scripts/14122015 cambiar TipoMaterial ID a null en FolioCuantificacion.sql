begin tran
begin try

	alter table [dbo].[Sam3_FolioCuantificacion]
		alter column [TipoMaterialID] int null

commit tran
end try
begin catch
	rollback tran
end catch