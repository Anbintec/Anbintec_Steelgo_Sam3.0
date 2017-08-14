DECLARE @orID INT, @cantidad int, @itemCodeID int, 
	@roiID int, @contador int, 
	@nuID int, @relnuID int

declare @relnum table(
	NumeroUnicoID int,
	Rel_NumeroUnico_FCB_ID int
)

DECLARE Actualizar CURSOR FOR
	SELECT ord.OrdenRecepcionID, 
		roi.Cantidad,
		rit.ItemCodeID,
		roi.Rel_OrdenRecepcion_ItemCode_ID
	from Sam3_OrdenRecepcion ord 
	inner join Sam3_Rel_OrdenRecepcion_ItemCode roi on ord.OrdenRecepcionID = roi.OrdenRecepcionID
	inner join Sam3_Rel_ItemCode_Diametro rit on roi.Rel_ItemCode_Diametro_ID = rit.Rel_ItemCode_Diametro_ID
	where ord.Activo = 1 and roi.Activo = 1 and rit.Activo = 1
OPEN Actualizar
FETCH NEXT FROM Actualizar INTO @orID, @cantidad, @itemCodeID, @roiID
WHILE @@FETCH_STATUS = 0
BEGIN
	
	insert into @relnum(NumeroUnicoID, Rel_NumeroUnico_FCB_ID)
	select seq.NumeroUnicoID, seq.Rel_NumeroUnico_RelFC_RelB_ID
	from 
	(
		select tbl.*,
			relnu.Rel_NumeroUnico_RelFC_RelB_ID, 
			ROW_NUMBER() over (order by tbl.NumeroUnicoID) rownum
		from Sam3_NumeroUnico as tbl inner join Sam3_Rel_NumeroUnico_RelFC_RelB relnu
		on tbl.NumeroUnicoID = relnu.NumeroUnicoID
		where relnu.OrdenRecepcionID is null and tbl.ItemCodeID = @itemCodeID
	) seq
	where rownum between 1 and @cantidad

	--Recorrer los registros encontrados

	DECLARE recorrer CURSOR FOR
	SELECT NumeroUnicoID, Rel_NumeroUnico_FCB_ID from @relnum
	OPEN recorrer
	FETCH NEXT FROM recorrer INTO @nuID, @relnuID
	WHILE @@FETCH_STATUS = 0
	BEGIN
		
		update Sam3_Rel_NumeroUnico_RelFC_RelB set OrdenRecepcionID = @orID
		where NumeroUnicoID = @nuID and Rel_NumeroUnico_RelFC_RelB_ID = @relnuID

	FETCH NEXT FROM recorrer INTO @nuID, @relnuID
	END
	CLOSE recorrer;
	DEALLOCATE recorrer;

	delete @relnum
	--fin de recorrido

	FETCH NEXT FROM Actualizar INTO @orID, @cantidad, @itemCodeID, @roiID
END
CLOSE Actualizar;
DEALLOCATE Actualizar;