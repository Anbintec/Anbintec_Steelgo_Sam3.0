SELECT * FROM Sam3_ItemCode
SELECT * FROM Sam3_Rel_ItemCode_Diametro
SELECT * FROM Sam3_Rel_FolioCuantificacion_ItemCode

ALTER TABLE Sam3_Rel_FolioCuantificacion_ItemCode
ADD DimensionPromedio decimal(14,4) null

