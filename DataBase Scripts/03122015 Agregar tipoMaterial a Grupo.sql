  ALTER TABLE Sam3_Grupo
  ADD TipoMaterialID int 

  ALTER TABLE Sam3_Grupo
ADD FOREIGN KEY (TipoMaterialID)
REFERENCES Sam3_TipoMaterial(TipoMaterialID)

ALTER TABLE Sam3_Grupo
SET TipoMaterialID = 1 
WHERE GrupoID = 1

ALTER TABLE Sam3_Grupo
SET TipoMaterialID = 2 
WHERE GrupoID > 1