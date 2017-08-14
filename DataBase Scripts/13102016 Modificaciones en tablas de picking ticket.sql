begin tran
begin try
	ALTER TABLE [dbo].[Sam3_FolioPickingTicket] DROP CONSTRAINT [FK__Sam3_Foli__TipoM__7CA47C3F]
	commit tran
end try
begin catch
	exec Sam3_GetErrorInfo
	rollback tran
end catch
go

begin tran
begin try

	delete Sam3_Entrega
	delete Sam3_FolioPickingTicket
	alter table Sam3_FolioPickingTicket drop column TipoMaterialID
	alter table Sam3_FolioPickingTicket drop column DespachoID
	alter table Sam3_FolioPickingTicket add OrdenTrabajoSpoolID int null

	create table Sam3_DetalleFolioPickingTicket(
		DetalleFolioPickingTicketID int not null identity primary key,
		FolioPickingTicketID int not null,
		OrdenTrabajoMaterialID int null,
		DespachoID int not null,
		FechaModificacion Datetime null, 
		UsuarioModificacion int null,
		Activo bit not null default(1),
		foreign key (FolioPickingTicketID) references Sam3_FolioPickingTicket(FolioPickingTicketID),
		foreign key (DespachoID) references Sam3_Despacho(DespachoID)
	)

	DBCC CHECKIDENT ('Sam3_FolioPickingTicket', RESEED, 1);
	DBCC CHECKIDENT ('Sam3_Entrega', RESEED, 1);   
	commit tran
end try
begin catch
	exec Sam3_GetErrorInfo
	rollback tran
end catch

select * from Sam3_Entrega
select * from Sam3_FolioPickingTicket
select * from Sam3_DetalleFolioPickingTicket
