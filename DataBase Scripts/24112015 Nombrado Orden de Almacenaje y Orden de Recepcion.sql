
--Orden Almacenaje
ALTER TABLE Sam3_OrdenAlmacenaje
ADD [Rel_Proyecto_Entidad_Configuracion_ID] int 
foreign key (Rel_Proyecto_Entidad_Configuracion_ID) references Sam3_Rel_Proyecto_Entidad_Configuracion(Rel_Proyecto_Entidad_Configuracion_ID)

ALTER TABLE Sam3_OrdenAlmacenaje
ADD Consecutivo int 

--Orden Recepcion
ALTER TABLE Sam3_OrdenRecepcion
ADD [Rel_Proyecto_Entidad_Configuracion_ID] int 
foreign key (Rel_Proyecto_Entidad_Configuracion_ID) references Sam3_Rel_Proyecto_Entidad_Configuracion(Rel_Proyecto_Entidad_Configuracion_ID)

ALTER TABLE Sam3_OrdenRecepcion
ADD Consecutivo int 

