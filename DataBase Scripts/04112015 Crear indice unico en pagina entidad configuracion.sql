begin tran
begin try

	IF EXISTS (SELECT name FROM sys.indexes
            WHERE name = N'Sam3_IX_Proyecto_Entidad_Configuracion')
    DROP INDEX Sam3_IX_Proyecto_Entidad_Configuracion ON [dbo].[Sam3_Rel_Proyecto_Entidad_Configuracion];

	CREATE UNIQUE NONCLUSTERED INDEX Sam3_IX_Proyecto_Entidad_Configuracion
    ON [dbo].[Sam3_Rel_Proyecto_Entidad_Configuracion] (Proyecto, Entidad, Activo);

	commit tran
end try
begin catch
	EXECUTE Sam3_GetErrorInfo;
	rollback tran
end catch