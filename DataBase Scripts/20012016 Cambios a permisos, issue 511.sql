/* 
33 PopUp Proveedor --> PaginaID

18	Proveedor --> EntidadID

26	18	descripcion
27	18	direccion --> PropiedadID
28	18	telefono
*/

UPDATE [Sam3_Rel_Perfil_Propiedad_Pagina]
SET Requerido = 0
WHERE PaginaID=/*33*/ and PerfilID=1 and PropiedadID=/*26*/

UPDATE [Sam3_Rel_Perfil_Propiedad_Pagina]
SET Requerido = 0
WHERE PaginaID=/*33*/ and PerfilID=1 and PropiedadID=/*27*/

UPDATE [Sam3_Rel_Perfil_Propiedad_Pagina]
SET Requerido = 0
WHERE PaginaID=/*33*/ and PerfilID=1 and PropiedadID=/*28*/