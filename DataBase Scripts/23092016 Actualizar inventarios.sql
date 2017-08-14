begin tran
begin try

declare @sam2_NumeroUnicoID int, 
		@sam3_NumeroUnicoID int,
		@inventarioCongelado int,
		@inventarioDisponibleCruce int,
		@cantidadRecibida int,
		@cantidadDanada int,
		@inventarioFisico int,
		@inventarioBuenEstado int,
		@tipoMaterialID int

DECLARE actualizar CURSOR FOR   
    SELECT Sam2_NumeroUnicoID, SAM3_NumeroUnicoID from Sam3_EquivalenciaNumeroUnico
	WHERE Activo = 1
    OPEN actualizar  
    FETCH NEXT FROM actualizar INTO @sam2_NumeroUnicoID, @sam3_NumeroUnicoID  
	WHILE @@FETCH_STATUS = 0  
	BEGIN 
	
	--recuperamos el tipo de material (tubo o accesorio)
	set @tipoMaterialID = (
		select it.TipoMaterialID 
		from SAM.dbo.ItemCode it inner join SAM.dbo.NumeroUnico nu 
		on it.ItemCodeID = nu.ItemCodeID
		where nu.NumeroUnicoID = @sam2_NumeroUnicoID
	)

	--verificamos si ya existen congelados en el numero unico
	if ((select InventarioCongelado from SAM.dbo.NumeroUnicoInventario 
			where NumeroUnicoID = @sam2_NumeroUnicoID) > 0)
	begin
		set @inventarioFisico = (
			select InventarioFisico 
			from SAM.dbo.NumeroUnicoInventario
			where NumeroUnicoID = @sam2_NumeroUnicoID
		)
		set @inventarioBuenEstado = (
			select InventarioBuenEstado 
			from SAM.dbo.NumeroUnicoInventario
			where NumeroUnicoID = @sam2_NumeroUnicoID
		)
		set @cantidadRecibida = (
			select CantidadRecibida 
			from SAM.dbo.NumeroUnicoInventario
			where NumeroUnicoID = @sam2_NumeroUnicoID
		)
		set @cantidadDanada = (
			select CantidadDanada 
			from SAM.dbo.NumeroUnicoInventario
			where NumeroUnicoID = @sam2_NumeroUnicoID
		)
		set @inventarioCongelado = (
			select InventarioCongelado 
			from SAM.dbo.NumeroUnicoInventario
			where NumeroUnicoID = @sam2_NumeroUnicoID
		)
		set @inventarioDisponibleCruce = (
			select InventarioDisponibleCruce 
			from SAM.dbo.NumeroUnicoInventario
			where NumeroUnicoID = @sam2_NumeroUnicoID
		)

		--Actualizamos el inventario
		update Sam3_NumeroUnicoInventario set
			CantidadRecibida = @cantidadRecibida,
			CantidadDanada = @cantidadDanada,
			InventarioFisico = @inventarioFisico,
			InventarioBuenEstado = @inventarioBuenEstado,
			InventarioCongelado = @inventarioCongelado,
			InventarioDisponibleCruce = @inventarioDisponibleCruce
		where NumeroUnicoID = @sam3_NumeroUnicoID

		--si es un tubo actualizamos tambien el segmento
		if @tipoMaterialID = 1
		begin
			set @inventarioFisico = (
			select InventarioFisico 
			from SAM.dbo.NumeroUnicoSegmento
			where NumeroUnicoID = @sam2_NumeroUnicoID
		)
			set @inventarioBuenEstado = (
			select InventarioBuenEstado 
			from SAM.dbo.NumeroUnicoSegmento
			where NumeroUnicoID = @sam2_NumeroUnicoID
		)
			set @cantidadDanada = (
			select CantidadDanada 
			from SAM.dbo.NumeroUnicoSegmento
			where NumeroUnicoID = @sam2_NumeroUnicoID
		)
			set @inventarioCongelado = (
			select InventarioCongelado 
			from SAM.dbo.NumeroUnicoSegmento
			where NumeroUnicoID = @sam2_NumeroUnicoID
		)
			set @inventarioDisponibleCruce = (
			select InventarioDisponibleCruce 
			from SAM.dbo.NumeroUnicoSegmento
			where NumeroUnicoID = @sam2_NumeroUnicoID
		)

			--actualizamos el segmento
			update Sam3_NumeroUnicoSegmento set
				CantidadDanada = @cantidadDanada,
				InventarioFisico = @inventarioFisico,
				InventarioBuenEstado = @inventarioBuenEstado,
				InventarioCongelado = @inventarioCongelado,
				InventarioDisponibleCruce = @inventarioDisponibleCruce
			where NumeroUnicoID = @sam3_NumeroUnicoID
		end
	end

	FETCH NEXT FROM actualizar INTO @sam2_NumeroUnicoID, @sam3_NumeroUnicoID
	END 
    CLOSE actualizar  
    DEALLOCATE actualizar  


commit tran
end try
begin catch
	exec [dbo].[Sam3_GetErrorInfo]
	rollback tran
end catch
go
