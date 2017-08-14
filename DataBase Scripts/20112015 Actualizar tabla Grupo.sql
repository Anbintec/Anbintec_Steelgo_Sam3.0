begin tran
begin try
	Alter table Sam3_Grupo ADD TieneD2 bit
	update Sam3_Grupo set TieneD2=0
end try
begin catch
	rollback tran
end catch



