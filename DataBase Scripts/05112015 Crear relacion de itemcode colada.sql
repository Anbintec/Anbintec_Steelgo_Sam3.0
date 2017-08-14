select * from Sam3_Colada

begin tran
begin try

	ALTER TABLE [dbo].[Sam3_ItemCode] DROP CONSTRAINT [FK_Sam3_ItemCode_Sam3_Colada]
	ALTER TABLE [dbo].[Sam3_ItemCode] DROP COLUMN ColadaID

	ALTER TABLE [dbo].[Sam3_Rel_FolioCuantificacion_ItemCode] ADD ColadaID INT NULL
	ALTER TABLE [dbo].[Sam3_Rel_Bulto_ItemCode] ADD ColadaID INT NULL

	UPDATE [dbo].[Sam3_Rel_FolioCuantificacion_ItemCode] SET ColadaID = 1
	UPDATE [dbo].[Sam3_Rel_Bulto_ItemCode] SET ColadaID = 1
	
	ALTER TABLE [dbo].[Sam3_Rel_FolioCuantificacion_ItemCode] ALTER COLUMN ColadaID INT NOT NULL
	ALTER TABLE [dbo].[Sam3_Rel_Bulto_ItemCode] ALTER COLUMN ColadaID INT NOT NULL 

	ALTER TABLE [dbo].[Sam3_Rel_FolioCuantificacion_ItemCode] ADD 
		FOREIGN KEY (ColadaID) REFERENCES Sam3_Colada(ColadaID)
		
	ALTER TABLE [dbo].[Sam3_Rel_Bulto_ItemCode] ADD 
		FOREIGN KEY (ColadaID) REFERENCES Sam3_Colada(ColadaID)

	CREATE TABLE Sam3_Rel_Itemcode_Colada(
		Rel_Itemcode_Colada INT NOT NULL PRIMARY KEY IDENTITY,
		ItemCodeID INT NOT NULL,
		ColadaID INT NOT NULL,
		Activo BIT NOT NULL DEFAULT(1),
		FechaModificacion DATETIME NULL,
		UsuarioModificacion INT NULL,
		FOREIGN KEY (ItemCodeID) REFERENCES Sam3_ItemCode(ItemCodeID),
		FOREIGN KEY (ColadaID) REFERENCES Sam3_Colada(ColadaID)
	)

commit tran
end try
begin catch
	exec [dbo].[Sam3_GetErrorInfo]
	rollback tran
end catch
