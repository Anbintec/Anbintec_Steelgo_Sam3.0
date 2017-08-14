CREATE TABLE Sam3_MTR(
MTRID int not null identity(1,1) PRIMARY KEY,
NumeroMTR varchar(200) not null,
ItemCodeID int not null FOREIGN KEY REFERENCES Sam3_ItemCode(ItemCodeID),
ColadaID int not null FOREIGN KEY REFERENCES Sam3_Colada(ColadaID),
CantidadPiezasAmparadas int not null,
Activo bit not null,
UsuarioModificacion int null,
FechaModificacion datetime null
)

INSERT INTO Sam3_Catalogos VALUES ('MTR', 1, NULL, NULL)

SELECT * FROM Sam3_MTR

ALTER TABLE Sam3_NumeroUnico
 ADD MTRID int null 
 
ALTER TABLE Sam3_NumeroUnico
ADD FOREIGN KEY (MTRID)
REFERENCES Sam3_MTR(MTRID)