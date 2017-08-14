begin tran 
begin try

	ALTER TABLE [dbo].[Sam3_Rel_OrdenRecepcion_ItemCode] add
		Rel_ItemCode_Diametro_ID int null

	ALTER TABLE [dbo].[Sam3_Rel_OrdenRecepcion_ItemCode] add
	Cantidad int null

commit tran
end try
begin catch
	EXECUTE Sam3_GetErrorInfo;
	rollback tran
end catch
go

begin tran 
begin try

	ALTER TABLE [dbo].[Sam3_Rel_OrdenRecepcion_ItemCode] DROP CONSTRAINT [FK__Sam3_Rel___ItemC__0B129727]

	ALTER TABLE [dbo].[Sam3_Rel_OrdenRecepcion_ItemCode] alter column ItemCodeID int null

	ALTER TABLE [dbo].[Sam3_Rel_OrdenRecepcion_ItemCode] add
		foreign key (Rel_ItemCode_Diametro_Id) references Sam3_Rel_ItemCode_Diametro(Rel_ItemCode_Diametro_Id)


commit tran
end try
begin catch
	EXECUTE Sam3_GetErrorInfo;
	rollback tran
end catch