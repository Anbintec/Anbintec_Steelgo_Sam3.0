begin tran
begin try

	alter table [dbo].[Sam3_FolioAvisoLlegada]
		Add CuadrillaDescarga varchar(max) null

	alter table [dbo].[Sam3_Rel_FolioAvisoLlegada_PaseSalida_Archivo]
		add IncidenciaID int null

commit tran
end try
begin catch
	rollback tran
end catch
go

begin tran
begin try

	update [dbo].[Sam3_Rel_FolioAvisoLlegada_PaseSalida_Archivo] set IncidenciaID = 0

	alter table [dbo].[Sam3_Rel_FolioAvisoLlegada_PaseSalida_Archivo]
		alter column IncidenciaID int not null

	alter table	[dbo].[Sam3_Rel_FolioAvisoLlegada_PaseSalida_Archivo]
		add	foreign key (IncidenciaID) references Sam3_Incidencia(IncidenciaID)

commit tran
end try
begin catch
	rollback tran
end catch
go