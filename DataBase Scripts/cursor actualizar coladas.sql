--select * from Sam3_Colada
--select * from Sam3_EquivalenciaColada

--alter table Sam3_EquivalenciaColada CHECK CONSTRAINT ALL

--alter table Sam3_EquivalenciaColada alter column Sam3_ColadaID int null

DECLARE @sam2_ColadaID INT,
		@fabricanteID int,
		@NumeroColada varchar(max),
		@certificado varchar(max),
		@proyectoID int

declare @tabla table(ColadaID int, sam2_ColadaID int)

DECLARE RecorrerRegistros CURSOR FOR
	SELECT Sam2_ColadaID
		FROM Sam3_EquivalenciaColada
	OPEN RecorrerRegistros
	FETCH NEXT FROM RecorrerRegistros 
		INTO @sam2_coladaID
	WHILE @@FETCH_STATUS = 0
	BEGIN

		set @fabricanteID = (
			select Sam3_FabricanteID from Sam3_EquivalenciaFabricante
			where Sam2_FabricanteID = (
			select FabricanteID from SAM2.SAM.DBO.Colada
			WHERE ColadaID = @sam2_ColadaID)
			and Activo = 1
		)

		set @NumeroColada = (
			select NumeroColada from SAM2.SAM.DBO.Colada
			WHERE ColadaID = @sam2_ColadaID
		)

		set @certificado = (
			select NumeroCertificado from SAM2.SAM.DBO.Colada
			WHERE ColadaID = @sam2_ColadaID
		)
		
		set @proyectoID = (
			select Sam3_ProyectoID from Sam3_EquivalenciaProyecto
			where Sam2_ProyectoID =(
				select ProyectoID from SAM2.SAM.DBO.Colada
				WHERE ColadaID = @sam2_ColadaID
			)
			and Activo = 1
		)
		
		update Sam3_EquivalenciaColada set Sam3_ColadaID = (
			select ColadaID from Sam3_Colada 
			where ((@fabricanteID is not null and FabricanteID = @fabricanteID)
				or (@fabricanteID is null and FabricanteID is null)
			)
			and (
				(@proyectoID is not null and ProyectoID = @proyectoID)
				or (@proyectoID is null and ProyectoID is null)
			)
			and NumeroColada = @NumeroColada
			and NumeroCertificado like '%' + @certificado + '%'
		)
		where Sam2_ColadaID = @sam2_ColadaID

		FETCH NEXT FROM RecorrerRegistros 
		INTO @sam2_coladaID
	END
	CLOSE RecorrerRegistros;
	DEALLOCATE RecorrerRegistros;

	select * from Sam3_EquivalenciaColada order by Sam3_ColadaID