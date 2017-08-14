/* 
33 PopUp Proveedor --> PaginaID

18	Proveedor --> EntidadID

24	18	contacto ---> Propiedad ID
*/

UPDATE [Sam3_Rel_Perfil_Propiedad_Pagina]
SET Requerido = 0
WHERE PaginaID=/*33*/ and PerfilID=1 and PropiedadID=/*24*/