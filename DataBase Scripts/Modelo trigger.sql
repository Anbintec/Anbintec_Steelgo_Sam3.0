IF EXISTS (SELECT * FROM sysobjects WHERE name='Sam3_Trg_ReplicarDiametros') BEGIN
	DROP TRIGGER dbo.Sam3_Trg_ReplicarDiametros
END
GO

CREATE TRIGGER Sam3_Trg_ReplicarDiametros
ON Sam3_Diametro
FOR INSERT
AS
BEGIN

	DECLARE @registroTemporal TABLE(
		DiametroID int,
		Valor decimal(7,4),
		VerificadoPorCalidad bit
	)

	INSERT INTO @registroTemporal
			   (
			    DiametroID,
				Valor,
				VerificadoPorCalidad
			   )
			SELECT
				DiametroID,
				Valor,
				VerificadoPorCalidad
			FROM inserted

	--Creo un cusor para recorrer los registros insertados
	DECLARE @valor INT, @Sam3diametroID int


	DECLARE RecorrerRegistros CURSOR FOR
	SELECT Valor, DiametroID FROM @registroTemporal
	OPEN RecorrerRegistros
	FETCH NEXT FROM RecorrerRegistros INTO @valor, @Sam3diametroID
	WHILE @@FETCH_STATUS = 0
	BEGIN

		if (select DiametroID from SAM2.SAM.dbo.Diametro
				where Valor = @valor) is null
		begin
			insert into SAM2.SAM.dbo.Diametro(valor, VerificadoPorCalidad)
			select Valor,
				VerificadoPorCalidad
			from @registroTemporal
			where Valor = @valor

			insert into Sam3_EquivalenciaDiametro(
				Sam2_DiametroID,
				Sam3_DiametroID,
				Activo
			)
			values(
				(
					select DiametroID from SAM2.SAM.dbo.Diametro
					where Valor = @valor
				),
				@Sam3diametroID,
				1
			)

		end

		FETCH NEXT FROM RecorrerRegistros INTO @valor, @Sam3diametroID
	END
	CLOSE RecorrerRegistros;
	DEALLOCATE RecorrerRegistros;


END
