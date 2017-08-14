begin tran 
begin try

	alter table [dbo].[Sam3_Rel_FolioCuantificacion_ItemCode]
		add Cantidad INT null

	alter table [dbo].[Sam3_Rel_Bulto_ItemCode]
		add Cantidad INT null

	commit tran
end try
begin catch
	rollback tran
end catch
