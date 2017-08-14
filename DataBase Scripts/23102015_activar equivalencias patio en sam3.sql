
begin tran
begin try

	declare @sam2_patioId1 int, @sam2_patioId2 int

	set @sam2_patioId1 = (
		select PatioID from sam2.sam.dbo.Proyecto
		where Nombre = 'Proyecto Integracion SAM3'
	)

	set @sam2_patioId2 = (
		select PatioID from sam2.sam.dbo.Proyecto
		where Nombre = 'Proyecto SAM3'
	)

	update Sam3_EquivalenciaPatio set Activo = 1, Sam2_PatioID = @sam2_patioId1
	where Sam3_PatioID = 1

	update Sam3_EquivalenciaPatio set Activo = 1, Sam2_PatioID = @sam2_patioId2
	where Sam3_PatioID = 2

	commit tran
end try
begin catch
	rollback tran
end catch

select * from Sam3_EquivalenciaPatio where Sam3_PatioID = 6
select * from Sam3_Patio

select * from sam2.sam.dbo.Proyecto where PatioID = 4