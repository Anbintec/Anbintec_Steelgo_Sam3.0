DECLARE @id INT, 
	@familiaAceroID int, 
	@familiaMaterialID int,
	@nomenclatura varchar(150),
	@nombreFamiliaAceroSAM2 varchar(150),
	@nombreFamiliaMaterialSAM2 varchar(150)

DECLARE ActualizarAceros CURSOR FOR
SELECT AceroID from Sam3_Acero where AceroID not in (1,2)
OPEN ActualizarAceros
FETCH NEXT FROM ActualizarAceros INTO @id
WHILE @@FETCH_STATUS = 0
BEGIN
	--Obtenemos la nomenclatura del acer
	set @nomenclatura = (select Nomenclatura from Sam3_Acero where AceroID = @id)

	--traemos el nombre de la familia material de sam2 de acuerdo a la nomenclatura
	set @nombreFamiliaMaterialSAM2 = (
				select fm.Nombre
				from SAM2.SAM.dbo.Acero a
				inner join SAM2.SAM.dbo.FamiliaAcero fa on a.FamiliaAceroID = fa.FAmiliaAceroID
				inner join SAM2.SAM.dbo.FamiliaMaterial fm on fa.FamiliaMaterialID = fm.FamiliaMaterialID
				where a.Nomenclatura = @nomenclatura
				)
	--traemos el nombre de la familia acero de sam2 de acuerdo a la nomenclatura
	set @nombreFamiliaAceroSAM2 = (
				select fa.Nombre
				from SAM2.SAM.dbo.Acero a
				inner join SAM2.SAM.dbo.FamiliaAcero fa on a.FamiliaAceroID = fa.FAmiliaAceroID
				where a.Nomenclatura = @nomenclatura
				)

	set @familiaMaterialID = (Select FamiliaMaterialID
			from Sam3_FamiliaMaterial where Nombre = @nombreFamiliaMaterialSAM2)

	set @familiaAceroID = (select FamiliaAceroID from Sam3_FamiliaAcero
			where Nombre = @nombreFamiliaAceroSAM2)

	--Actualizamos la familia de acero con el id correcto de la familia material
	update Sam3_FamiliaAcero set FamiliaMaterialID = @familiaMaterialID
		where FamiliaAceroID = @familiaAceroID
	
	--Actualizamos el acero con el id correcto de la familia de acero 
	update Sam3_Acero set FamiliaAceroID = @familiaAceroID where AceroID = @id

	FETCH NEXT FROM ActualizarAceros INTO @id
END
CLOSE ActualizarAceros;
DEALLOCATE ActualizarAceros;