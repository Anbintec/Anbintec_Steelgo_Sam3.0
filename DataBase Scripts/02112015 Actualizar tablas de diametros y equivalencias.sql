begin tran
begin try

	DBCC CHECKIDENT ('dbo.Sam3_Diametro', RESEED, 0)
	declare @diametroID int, @nuevoID int
	
	DECLARE ActualizarDiametros CURSOR FOR
	SELECT DiametroID from sam2.sam.dbo.Diametro
	OPEN ActualizarDiametros
	FETCH NEXT FROM ActualizarDiametros INTO @diametroID
	WHILE @@FETCH_STATUS = 0
	BEGIN

		insert into Sam3_Diametro (
			Valor, 
			VerificadoPorCalidad,
			Activo
		)
		select Valor, 
			VerificadoPorCalidad,
			1
		from sam2.sam.dbo.Diametro d
		where d.DiametroID = @diametroID

		set @nuevoID = SCOPE_IDENTITY()

		insert into Sam3_EquivalenciaDiametro(
			Sam2_DiametroID,
			Sam3_DiametroID,
			Activo
		)
		values(
			@diametroID,
			@nuevoID,
			1
		)

	FETCH NEXT FROM ActualizarDiametros INTO @diametroID
	END
	CLOSE ActualizarDiametros;
	DEALLOCATE ActualizarDiametros;

	select * from Sam3_Diametro
	select * from Sam3_EquivalenciaDiametro
	commit tran
end try
begin catch
	DBCC CHECKIDENT ('dbo.Sam3_Diametro', RESEED, 0)
	EXECUTE Sam3_GetErrorInfo;
	CLOSE ActualizarDiametros;
	DEALLOCATE ActualizarDiametros;
	rollback tran
end catch