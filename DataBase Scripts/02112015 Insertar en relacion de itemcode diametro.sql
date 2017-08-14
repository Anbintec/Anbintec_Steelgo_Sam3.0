begin try
begin tran

	DECLARE @itemCodeID int, @ItemCodeSteelgoID int, @diametro1ID int, @diametro2ID int

	DECLARE RecorrerRegistros CURSOR FOR
	SELECT ItemCodeID FROM Sam3_ItemCode where Activo = 1
	and ItemCodeID in (Select ItemCodeID from Sam3_NumeroUnico)
	OPEN RecorrerRegistros
	FETCH NEXT FROM RecorrerRegistros INTO @itemCodeID
	WHILE @@FETCH_STATUS = 0
	BEGIN
		
		set @diametro1ID = (
			select DiametroID 
			from Sam3_Diametro
			where Valor = (
				select top 1 Diametro1
				from Sam3_NumeroUnico
				where ItemCodeID = @itemCodeID
			)
		)
		set @diametro2ID = (
			select DiametroID 
			from Sam3_Diametro
			where Valor = (
				select top 1 Diametro2
				from Sam3_NumeroUnico
				where ItemCodeID = @itemCodeID
			)
		)

		insert into Sam3_Rel_ItemCode_Diametro(
			ItemCodeID,
			Diametro1ID,
			Diametro2ID,
			Activo
		)
		values(
			@itemCodeID,
			@diametro1ID,
			@diametro2ID,
			1
		)

		FETCH NEXT FROM RecorrerRegistros INTO @itemCodeID
	END
	CLOSE RecorrerRegistros;
	DEALLOCATE RecorrerRegistros;

commit tran
end try
begin catch
	EXECUTE Sam3_GetErrorInfo;
	rollback tran
end catch
go

begin try
begin tran

	DECLARE @ItemCodeSteelgoID int, @diametro1ID int, @diametro2ID int

	DECLARE RecorrerRegistros CURSOR FOR
	SELECT ItemCodeSteelgoID FROM Sam3_ItemCodeSteelgo where Activo = 1
	OPEN RecorrerRegistros
	FETCH NEXT FROM RecorrerRegistros INTO @ItemCodeSteelgoID
	WHILE @@FETCH_STATUS = 0
	BEGIN
		
		set @diametro1ID = (
			select DiametroID from Sam3_Diametro
			where Valor = 4
		)
		set @diametro2ID = (
			select DiametroID from Sam3_Diametro
			where Valor = 2
		)

		insert into Sam3_Rel_ItemCodeSteelgo_Diametro(
			ItemCodeSteelgoID,
			Diametro1ID,
			Diametro2ID,
			Activo
		)
		values(
			@ItemCodeSteelgoID,
			@diametro1ID,
			@diametro2ID,
			1
		)

		FETCH NEXT FROM RecorrerRegistros INTO @ItemCodeSteelgoID
	END
	CLOSE RecorrerRegistros;
	DEALLOCATE RecorrerRegistros;

commit tran
end try
begin catch
	EXECUTE Sam3_GetErrorInfo;
	rollback tran
end catch
go

begin tran
begin try

	update Sam3_NumeroUnico set Diametro1 = 4, Diametro2 = 2
	select * from Sam3_NumeroUnico
commit tran
end try
begin catch
	EXECUTE Sam3_GetErrorInfo;
	rollback tran
end catch
go

begin try
begin tran

	DECLARE @relItemCodeSteelgoID int, @relItemCodeID int, @id int,
	@id1 int, @id2 int

	DECLARE RecorrerRegistros CURSOR FOR
	SELECT ItemCodeSteelgoID, ItemCodeID, Rel_ItemCode_ItemCodeSteelgo 
		FROM Sam3_Rel_ItemCode_ItemCodeSteelgo where Activo = 1
	OPEN RecorrerRegistros
	FETCH NEXT FROM RecorrerRegistros INTO @relItemCodeSteelgoID, @relItemCodeID, @id
	WHILE @@FETCH_STATUS = 0
	BEGIN
		
		set @id1 = (
			select Rel_ItemCode_Diametro_ID from sam3_Rel_ItemCode_Diametro
			where ItemCodeID = @relItemCodeID
		)

		set @id2 = (
			select Rel_ItemCodeSteelgo_Diametro_ID 
			from Sam3_Rel_ItemCodeSteelgo_Diametro
			where ItemCodeSteelgoID = @relItemCodeSteelgoID
		)

		update Sam3_Rel_ItemCode_ItemCodeSteelgo set 
			Rel_ItemCode_Diametro_ID = @id1,
			Rel_ItemCodeSteelgo_Diametro_ID = @id2
		Where Rel_ItemCode_ItemCodeSteelgo = @id
		and ItemCodeID = @relItemCodeID and ItemCodeSteelgoID = @relItemCodeSteelgoID
		

		FETCH NEXT FROM RecorrerRegistros INTO @relItemCodeSteelgoID, @relItemCodeID, @id
	END
	CLOSE RecorrerRegistros;
	DEALLOCATE RecorrerRegistros;

commit tran
end try
begin catch
	EXECUTE Sam3_GetErrorInfo;
	rollback tran
end catch
go