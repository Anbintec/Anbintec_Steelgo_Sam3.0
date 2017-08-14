
 ALTER TABLE Sam3_FolioAvisoEntrada 
 ADD IdentificadorCliente varchar(50) NULL


DBCC CHECKIDENT ('Sam3_Propiedad', NORESEED) 
DBCC CHECKIDENT ('Sam3_Propiedad', RESEED, 224);

INSERT INTO Sam3_Propiedad VALUES(14, 'identificadorcliente', 1, NULL,NULL) --14: Llegada Material

INSERT INTO Sam3_Rel_Perfil_Propiedad_Pagina VALUES(1, ,7,1,1,0,1,NULL,NULL) --7: DetalleLlegadaMaterial

