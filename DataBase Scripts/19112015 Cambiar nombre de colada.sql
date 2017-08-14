begin tran
begin try
	
	update [dbo].[Sam3_Colada] set NumeroColada = '' where ColadaID = 1
	update [dbo].[Sam3_Colada] set NumeroColada = 'Sin Colada PL' where ColadaID = 3

commit tran
end try
begin catch
	rollback tran
end catch
select * from Sam3_Colada