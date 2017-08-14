begin tran 
begin try

	alter table Sam3_NumeroUnico
		alter column Factura nvarchar(max) null

	alter table Sam3_NumeroUnico
		alter column OrdenDeCompra nvarchar(max) null

	commit tran
end try
begin catch
	exec Sam3_GetErrorInfo
	rollback tran
end catch
go

begin tran 
begin try

	alter table sam2.sam.dbo.NumeroUnico
		alter column Factura nvarchar(max) null

	alter table sam2.sam.dbo.NumeroUnico
		alter column OrdenDeCompra nvarchar(max) null

	commit tran
end try
begin catch
	exec Sam3_GetErrorInfo
	rollback tran
end catch
go