begin tran
	update Sam3_Rel_Perfil_Propiedad_Pagina 
		set Requerido = 0
	where PaginaID = (select PaginaID from Sam3_Pagina where Accion = 'ComplementoRecepcion') 
	and PropiedadID = (select PropiedadID from Sam3_Propiedad where Nombre = 'numerocertificado')
rollback tran