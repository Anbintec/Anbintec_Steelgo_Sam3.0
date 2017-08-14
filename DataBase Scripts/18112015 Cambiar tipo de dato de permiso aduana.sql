begin tran
begin try

	alter table [dbo].[Sam3_PermisoAduana]
		alter column [NumeroPermiso] varchar(max) null

commit tran
end try
begin catch
	rollback tran
end catch