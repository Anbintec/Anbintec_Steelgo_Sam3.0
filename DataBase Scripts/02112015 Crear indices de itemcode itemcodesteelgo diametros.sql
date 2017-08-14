begin tran
begin try

	IF EXISTS (SELECT name FROM sys.indexes
            WHERE name = N'Sam3_IX_ItemCodeDiametros')
    DROP INDEX Sam3_IX_ItemCodeDiametros ON Sam3_Rel_ItemCode_Diametro;

	CREATE UNIQUE NONCLUSTERED INDEX Sam3_IX_ItemCodeDiametros
    ON Sam3_Rel_ItemCode_Diametro (ItemCodeID, Diametro1ID, Diametro2ID);
	
	IF EXISTS (SELECT name FROM sys.indexes
            WHERE name = N'Sam3_IX_ItemCodeSteelgoDiametros')
    DROP INDEX Sam3_IX_ItemCodeSteelgoDiametros ON Sam3_Rel_ItemCodeSteelgo_Diametro;

	CREATE UNIQUE NONCLUSTERED INDEX Sam3_IX_ItemCodeSteelgoDiametros
    ON Sam3_Rel_ItemCodeSteelgo_Diametro (ItemCodeSteelgoID, Diametro1ID, Diametro2ID);

	commit tran
end try
begin catch
	EXECUTE Sam3_GetErrorInfo;
	rollback tran
end catch