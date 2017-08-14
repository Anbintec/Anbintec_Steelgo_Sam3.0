begin tran
begin try

	IF EXISTS (SELECT name FROM sys.indexes
            WHERE name = N'Sam3_IX_Rel_ItemCode_Colada')
    DROP INDEX Sam3_IX_Rel_ItemCode_Colada ON [dbo].[Sam3_Rel_Itemcode_Colada];

	CREATE UNIQUE NONCLUSTERED INDEX Sam3_IX_Rel_ItemCode_Colada
    ON [dbo].[Sam3_Rel_Itemcode_Colada] (ItemCodeID, ColadaID);

	commit tran
end try
begin catch
	EXECUTE Sam3_GetErrorInfo;
	rollback tran
end catch