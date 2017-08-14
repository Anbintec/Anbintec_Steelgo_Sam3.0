begin tran
begin try
	ALTER TABLE [dbo].[Sam3_Rel_FolioCuantificacion_ItemCode] add Rel_ItemCode_Diametro_ID int null
commit tran
end try
begin catch
	rollback tran
end catch

begin tran
begin try

	ALTER TABLE [dbo].[Sam3_Rel_FolioCuantificacion_ItemCode] DROP CONSTRAINT [FK__Rel_Folio__ItemC__742F31CF]

	ALTER TABLE [dbo].[Sam3_Rel_FolioCuantificacion_ItemCode] alter column ItemCodeId int null

	alter table [dbo].[Sam3_Rel_FolioCuantificacion_ItemCode] add foreign key (Rel_ItemCode_Diametro_ID)
	references [dbo].[Sam3_Rel_ItemCode_Diametro](Rel_ItemCode_Diametro_ID)
	
	commit tran
end try
begin catch
	EXECUTE Sam3_GetErrorInfo;
	rollback tran
end catch


