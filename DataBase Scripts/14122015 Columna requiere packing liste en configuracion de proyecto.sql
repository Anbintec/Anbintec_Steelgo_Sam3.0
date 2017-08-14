begin tran
begin try

	alter table Sam3_ProyectoConfiguracion
		add RequiereTipoPackingList bit not null default(1)

commit tran
end try
begin catch
	rollback tran
end catch

select * from Sam3_ProyectoConfiguracion