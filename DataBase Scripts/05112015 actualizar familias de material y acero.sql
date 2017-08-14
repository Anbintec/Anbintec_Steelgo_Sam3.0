begin try
begin tran

	update Sam3_FamiliaAcero set Activo = 0 where FamiliaAceroID in (1,2)
	update Sam3_FamiliaMaterial set Activo = 0 where FamiliaMaterialID in (1,2)

	update Sam3_EquivalenciaFamiliaAcero set Activo = 0 where Sam3_FamiliaAceroID in (1,2)
	update Sam3_EquivalenciaFamiliaMaterial set Activo = 0 where Sam3_FamiliaMaterialID in (1,2)

commit 
end try
begin catch
	rollback tran
end catch