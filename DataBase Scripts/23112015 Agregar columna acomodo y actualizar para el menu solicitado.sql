
begin tran
begin try

	Alter table [dbo].[Sam3_MenuGeneral] add
		Acomodo int null default(0)

commit tran
end try
begin catch
	rollback tran
end catch
go

begin tran
begin try

	update [dbo].[Sam3_MenuGeneral] set Acomodo = 1 where Liga = '/Cuantificacion/Cuantificacion'
	update [dbo].[Sam3_MenuGeneral] set Acomodo = 2 where Liga = '/OrdenRecepcion/GenerarOrdenRecepcion'
	update [dbo].[Sam3_MenuGeneral] set Acomodo = 3 where Liga = '/ComplementoRecepcion/ComplementoRecepcion'
	update [dbo].[Sam3_MenuGeneral] set Acomodo = 4 where Liga = '/OrdenAlmacenaje/GenerarOrdenAlmacenaje'
	update [dbo].[Sam3_MenuGeneral] set Acomodo = 5 where Liga = '/Almacenaje/Almacenaje'
		
commit tran
end try
begin catch
	rollback tran
end catch

select * from Sam3_MenuGeneral where IDPadre = 22 order by IDPadre

