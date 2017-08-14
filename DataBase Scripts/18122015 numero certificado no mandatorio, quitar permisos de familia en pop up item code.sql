---------------------------------------------------------------------------------------------------------------------
---------------------------------------------------------------------------------------------------------------------
--ELIMINAR PERMISO DE FAMILIA EN POPUP ITEM CODE

UPDATE Sam3_Propiedad 
SET Activo = 0 
WHERE PropiedadID = 90
--Entidad = 25 / Item Code
--Propiedad = familia


UPDATE Sam3_Rel_Perfil_Propiedad_Pagina
SET Activo = 0
WHERE PropiedadID = 90 AND PaginaID = 12
--paginaID = propiedad 90 en ... 12 Cuantificacion y 35 Item code pop up

---------------------------------------------------------------------------------------------------------------------
---------------------------------------------------------------------------------------------------------------------
--NO MANDATORIO NUMERO DE CERTIFICADO


UPDATE Sam3_Rel_Perfil_Propiedad_Pagina
SET Requerido = 0 
WHERE PropiedadID = 103 AND PaginaID = 12 

--PaginaID: 12 Cuantificacion
--EntidadID: 27 Colada
--PropiedadID: 103 numerocertificado