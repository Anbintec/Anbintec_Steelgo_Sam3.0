begin tran
begin try


ALTER TABLE [dbo].[Sam3_Rel_ItemCode_ItemCodeSteelgo] DROP CONSTRAINT [FK__Sam3_Rel___ItemC__027D5126]

ALTER TABLE [dbo].[Sam3_Rel_ItemCode_ItemCodeSteelgo] DROP CONSTRAINT [FK__Sam3_Rel___ItemC__0371755F]

alter table [dbo].[Sam3_Rel_ItemCode_ItemCodeSteelgo] alter column ItemCodeID int null

alter table [dbo].[Sam3_Rel_ItemCode_ItemCodeSteelgo] alter column ItemCodeSteelgoID int null

alter table [dbo].[Sam3_Rel_ItemCode_ItemCodeSteelgo] add Rel_ItemCode_Diametro_ID int null

alter table [dbo].[Sam3_Rel_ItemCode_ItemCodeSteelgo] add Rel_ItemCodeSteelgo_Diametro_ID int null

alter table [dbo].[Sam3_Rel_ItemCode_ItemCodeSteelgo] add foreign key (Rel_ItemCode_Diametro_ID)
	references [dbo].[Sam3_Rel_ItemCode_Diametro](Rel_ItemCode_Diametro_ID)

alter table [dbo].[Sam3_Rel_ItemCode_ItemCodeSteelgo] add foreign key (Rel_ItemCodeSteelgo_Diametro_ID)
	references [dbo].[Sam3_Rel_ItemCodeSteelgo_Diametro](Rel_ItemCodeSteelgo_Diametro_ID)



rollback tran
end try
begin catch
	EXECUTE Sam3_GetErrorInfo;
	rollback tran
end catch



