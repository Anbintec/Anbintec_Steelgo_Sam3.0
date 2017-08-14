CREATE TABLE Sam3_DeficitMateriales
(
	DeficitID int IDENTITY(1,1) Primary key,
	OrdenTrabajoID int not null,
	ItemCodeID int not null,
	SpoolID int null,
	Deficit int not null,
	Solucionado bit not null,
	Activo bit not null,
	UsuarioModificacion int null,
	FechaModificacion datetime null
)

ALTER TABLE Sam3_DeficitMateriales
ADD FOREIGN KEY (ItemCodeID)
REFERENCES Sam3_ItemCode(ItemCodeID)
