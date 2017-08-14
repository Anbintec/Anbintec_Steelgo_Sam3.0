begin tran
begin try

declare @folioID int, @clienteSam2 int, @clienteID int

DECLARE repararFolios CURSOR FOR
SELECT FolioAvisoLlegadaID, ClienteID from Sam3_FolioAvisoLlegada
OPEN repararFolios
FETCH NEXT FROM repararFolios INTO @folioID, @clienteSam2
WHILE @@FETCH_STATUS = 0
BEGIN
	
	set @clienteID = (
		select ClienteID from Sam3_Cliente where Sam2ClienteID = @clienteSam2
	)

	update Sam3_FolioAvisoLlegada set ClienteID = @clienteID 
	where FolioAvisoLlegadaID = @folioID 

	FETCH NEXT FROM repararFolios INTO @folioID, @clienteSam2
END
CLOSE repararFolios;
DEALLOCATE repararFolios;

select * from Sam3_FolioAvisoLlegada

	commit tran
end try
begin catch
	rollback tran
end catch




