begin tran
begin try

alter table Sam3_MTR 
	add Rel_ItemCode_Diametro_ID int  not null

alter table Sam3_MTR 
	add foreign key (Rel_ItemCode_Diametro_ID) 
		references Sam3_Rel_ItemCode_Diametro(Rel_ItemCode_Diametro_ID)

commit tran
end try
begin catch
	exec [dbo].[Sam3_GetErrorInfo]
	rollback tran
end catch
go

begin tran
begin try
declare @MTRID int, @itemCodeID int, @relDiametro int
DECLARE actualizar CURSOR FOR   
    SELECT MTRID, ItemCodeID from Sam3_MTR
    OPEN actualizar  
    FETCH NEXT FROM actualizar INTO @MTRID, @itemCodeID  
	WHILE @@FETCH_STATUS = 0  
	BEGIN 
	set @relDiametro = (
		select top 1 Rel_ItemCode_Diametro_ID from Sam3_Rel_ItemCode_Diametro
		where ItemCodeID = @itemCodeID
	)

	update Sam3_MTR set Rel_ItemCode_Diametro_ID = @relDiametro
	where MTRID = @MTRID and ItemCodeID = @itemCodeID

	FETCH NEXT FROM actualizar INTO @MTRID, @itemCodeID
	END 
    CLOSE actualizar  
    DEALLOCATE actualizar  

--alter table Sam3_MTR
--	alter column Rel_ItemCode_Diametro_ID int not null

commit tran
end try
begin catch
	exec [dbo].[Sam3_GetErrorInfo]
	rollback tran
end catch
go

begin tran
begin try

alter table Sam3_MTR
	alter column Rel_ItemCode_Diametro_ID int not null

commit tran
end try
begin catch
	exec [dbo].[Sam3_GetErrorInfo]
	rollback tran
end catch
go
