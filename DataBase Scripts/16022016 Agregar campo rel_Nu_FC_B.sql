begin tran

Alter table [dbo].[Sam3_Rel_NumeroUnico_RelFC_RelB]
	add [OrdenRecepcionID] int null,
	foreign key ([OrdenRecepcionID]) references Sam3_OrdenRecepcion([OrdenRecepcionID])

rollback tran