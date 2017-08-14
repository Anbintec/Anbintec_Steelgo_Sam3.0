begin tran
begin try
	insert into Sam3_Perfil(
		Nombre,
		Activo
	)
	values(
		'Sin Permisos',
		1
	)

	commit tran
end try
begin catch
	rollback tran
end catch

select * from Sam3_Perfil
