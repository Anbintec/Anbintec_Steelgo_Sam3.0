begin tran
begin try
	
	--Quitar diamertros de ItemCode
	alter table Sam3_Itemcode drop column Diametro1
	alter table Sam3_Itemcode drop column Diametro2
	--Quitar diametros de ItemCodeSteelgo
	alter table Sam3_ItemCodeSteelgo drop column Diametro1
	alter table Sam3_ItemCodeSteelgo drop column Diametro2

	--Crear tablas de relacion con diametros
	create table Sam3_Rel_ItemCode_Diametro(
		Rel_ItemCode_Diametro_ID int not null primary key identity(1,1),
		ItemCodeID int not null,
		Diametro1ID int not null,
		Diametro2ID int not null,
		Activo bit not null default(1),
		FechaModificacion datetime null,
		UsuarioModificacion int null,
		foreign key (Diametro1ID) references Sam3_Diametro(DiametroID),
		foreign key (Diametro2ID) references Sam3_Diametro(DiametroID),
		foreign key (ItemCodeID) references Sam3_ItemCode(ItemCodeID)
	)

	create table Sam3_Rel_ItemCodeSteelgo_Diametro(
		Rel_ItemCodeSteelgo_Diametro_ID int not null primary key identity(1,1),
		ItemCodeSteelgoID int not null,
		Diametro1ID int not null,
		Diametro2ID int not null,
		Activo bit not null default(1),
		FechaModificacion datetime null,
		UsuarioModificacion int null,
		foreign key (Diametro1ID) references Sam3_Diametro(DiametroID),
		foreign key (Diametro2ID) references Sam3_Diametro(DiametroID),
		foreign key (ItemCodeSteelgoID) references Sam3_ItemCodeSteelgo(ItemCodeSteelgoID)
	)

	create table Sam3_EquivalenciaDiametro(
		EquivalenciaDiametroID int not null primary key Identity(1,1),
		Sam2_DiametroID int null,
		Sam3_DiametroID int null,
		Activo bit not null default(1),
		FechaModificacion datetime null,
		UsuarioModificacion int null
	)

	--select top 1 * from Sam3_ItemCode
	--select top 1 * from Sam3_ItemCodeSteelgo
	commit tran
end try
begin catch
	EXECUTE Sam3_GetErrorInfo;
	rollback tran
end catch
go
