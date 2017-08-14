begin tran
begin try

INSERT INTO [dbo].[TipoMovimiento]
           ([EsEntrada]
           ,[EsTransferenciaProcesos]
           ,[ApareceEnSaldos]
           ,[DisponibleMovimientosUI]
           ,[Nombre]
           ,[NombreIngles]
           ,[Descripcion]
           ,[FechaModificacion]
		   ,[TipoMovimientoID])
     VALUES
           (1
           ,0
           ,0
           ,0
           ,'Aumento de Inventario por Actualización MM'
           ,'Inventory increased by Update MM'
           ,'Actualizacion de MM Complemento de recepción'
           ,GETDATE()
		   ,(select MAX(TipoMovimientoID) + 1 from TipoMovimiento))

INSERT INTO [dbo].[TipoMovimiento]
           ([EsEntrada]
           ,[EsTransferenciaProcesos]
           ,[ApareceEnSaldos]
           ,[DisponibleMovimientosUI]
           ,[Nombre]
           ,[NombreIngles]
           ,[Descripcion]
           ,[FechaModificacion]
		   ,[TipoMovimientoID])
     VALUES
           (0
           ,0
           ,0
           ,0
           ,'Reducción de Inventario por Actualización MM'
           ,'Update Inventory reduction by MM'
           ,'Actualizacion de MM Complemento de recepción'
           ,GETDATE()
		   ,(select MAX(TipoMovimientoID) + 1 from TipoMovimiento))
commit tran
end try
begin catch
	rollback tran
end catch