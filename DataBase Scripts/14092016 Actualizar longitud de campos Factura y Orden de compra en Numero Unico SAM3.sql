begin tran 

alter table Sam3_NumeroUnico
	alter column Factura nvarchar(50) null

alter table Sam3_NumeroUnico
	alter column OrdenDeCompra nvarchar(50) null

commit tran