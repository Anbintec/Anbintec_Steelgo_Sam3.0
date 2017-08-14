begin tran
begin try

	create table Sam3_Rel_NumeroUnico_RelFC_RelB(
		Rel_NumeroUnico_RelFC_RelB_ID int not null primary key identity,
		NumeroUnicoID int not null,
		Rel_FolioCuantificacion_ItemCode_ID int null,
		Rel_Bulto_ItemCode_ID int null,
		Activo bit not null default(1),
		FechaModificacion datetime null,
		UsuarioModificacion int null,
		foreign key (NumeroUnicoID) references Sam3_NumeroUnico(NumeroUnicoID),
		foreign key (Rel_FolioCuantificacion_ItemCode_ID) references Sam3_Rel_FolioCuantificacion_ItemCode(Rel_FolioCuantificacion_ItemCode_ID),
		foreign key (Rel_Bulto_ItemCode_ID) references Sam3_Rel_Bulto_ItemCode(Rel_Bulto_ItemCode_ID) 
	)

commit tran
end try
begin catch
	rollback tran
end catch