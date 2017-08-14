begin tran
begin try

	alter table [dbo].[Sam3_Incidencia]
	add ResueltoPor varchar(250) null,
	RespondidoPor varchar(250) null,
	RegistradoPor varchar(250) null

	commit tran

end try
begin catch
rollback tran
end catch