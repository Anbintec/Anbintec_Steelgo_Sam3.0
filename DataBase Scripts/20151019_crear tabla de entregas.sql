begin tran
begin try

	create table Sam3_FolioPickingTicket(
		FolioPickingTicketID int not null primary key identity(1,1),
		TipoMaterialID int not null,
		DespachoID int not null,
		usuarioModificacion int null,
		FechaModificacion datetime null,
		Activo bit not null default(1),
		foreign key (TipoMaterialID) references Sam3_TipoMaterial(TipoMaterialID)
	)

	create table Sam3_FolioImpresionDocumental(
		FolioImpresionDocumentalID int not null primary key identity,
		SpoolID int not null,
		UsuarioModificacion int null,
		FechaModificacion datetime null,
		Activo bit not null default(1)
	)

	create table Sam3_Entrega(
		EntregaID int not null primary key Identity,
		UsuarioEntregaID int not null,
		UsuarioRecibeID int not null,
		UsuarioModificacion int null,
		FechaModificacion datetime null,
		FechaEntrega datetime not null,
		Activo bit not null default(1),
		FolioPickingTicketID int not null,
		foreign key (FolioPickingTicketID) references Sam3_FolioPickingTicket(FolioPickingTicketID)
	)

	commit tran
end try
begin catch
	rollback tran
end catch
