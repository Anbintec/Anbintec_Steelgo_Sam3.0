begin tran 

alter table NumeroUnico
	alter column Factura nvarchar(50) null

alter table NumeroUnico
	alter column OrdenDeCompra nvarchar(50) null

rollback tran
