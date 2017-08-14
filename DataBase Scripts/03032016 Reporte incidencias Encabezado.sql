-- ================================================
-- Template generated from Template Explorer using:
-- Create Procedure (New Menu).SQL
--
-- Use the Specify Values for Template Parameters 
-- command (Ctrl-Shift-M) to fill in the parameter 
-- values below.
--
-- This block of comments will not be included in
-- the definition of the procedure.
-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Julian Hernandez	
-- Create date: 03-03-2016
-- Description:	Encabezado de incidencias
-- =============================================
CREATE PROCEDURE Sam3_RPT_EncabezadoReporteIncidencias
	@incidenciaID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	declare @tipoIncidenciaID int
set @tipoIncidenciaID = (
	select TipoIncidenciaID from Sam3_Incidencia 
	where IncidenciaID = @incidenciaID
)

--1 - folio aviso entrada
--2 - Entrada de material
--4 - Packing List
--5 - Orden de recepcion
--7 - ItemCode
--8 - Orden de almacenaje
--9 - Numero unico
--10 - Despacho
--11 - Corte
--12 - Pre despacho 

if @tipoIncidenciaID = 1 -- aviso de llegada
begin 
	select 
		(
			select 
				LTRIM(RTRIM(rel.PreFijoFolioIncidencias)) +
				REPLACE(
						STR(
							LTRIM(RTRIM(inc2.Consecutivo)),
							CASE
								WHEN LEN(inc2.Consecutivo) > rel.CantidadCerosFolioIncidencias 
								THEN LEN(inc2.Consecutivo)
								ELSE rel.CantidadCerosFolioIncidencias
							END
						), SPACE(1), '0'
				) +
				LTRIM(RTRIM(rel.PostFijoFolioIncidencias))
			from Sam3_Incidencia inc2 inner join Sam3_Rel_Proyecto_Entidad_Configuracion rel 
			on inc2.Rel_Proyecto_Entidad_Configuracion_ID = rel.Rel_Proyecto_Entidad_Configuracion_ID
			where inc2.IncidenciaID = @incidenciaID
		) as 'NoReporte',
		inc.FechaCreacion,
		(
			select cl.Nombre
			from Sam3_Rel_Incidencia_FolioAvisoLlegada relinc
			inner join Sam3_FolioAvisoLlegada fa on relinc.FolioAvisoLlegadaID = fa.FolioAvisoLlegadaID
			inner join Sam3_Cliente cl on fa.ClienteID = cl.ClienteID
			where IncidenciaID = @incidenciaID
		) as Cliente,
		tpi.Nombre as Clasificacion,
		(
			select p.Nombre
			from Sam3_Rel_Incidencia_FolioAvisoLlegada relinc
			inner join Sam3_FolioAvisoLlegada fa on relinc.FolioAvisoLlegadaID = fa.FolioAvisoLlegadaID
			inner join Sam3_Rel_FolioAvisoLlegada_Proyecto relp on fa.FolioAvisoLlegadaID = relp.FolioAvisoLlegadaID
			inner join Sam3_Proyecto p on relp.ProyectoID = p.ProyectoID
			where IncidenciaID = @incidenciaID
			for XML auto
		) as Proyecto,
		inc.Titulo,
		(
			select 
				LTRIM(RTRIM(relp.PreFijoFolioAvisoLlegada)) +
				REPLACE(
						STR(
							LTRIM(RTRIM(fa.Consecutivo)),
							CASE
								WHEN LEN(fa.Consecutivo) > relp.CantidadCerosFolioAvisoLlegada 
								THEN LEN(fa.Consecutivo)
								ELSE relp.CantidadCerosFolioAvisoLlegada
							END
						), SPACE(1), '0'
				) +
				LTRIM(RTRIM(relp.PostFijoFolioAvisoLlegada))
			from Sam3_Rel_Incidencia_FolioAvisoLlegada relfa 
			inner join Sam3_FolioAvisoLlegada fa on relfa.FolioAvisoLlegadaID = fa.FolioAvisoLlegadaID
			inner join Sam3_Rel_Proyecto_Entidad_Configuracion relp on fa.ProyectoNombrado = relp.Proyecto
			where relfa.IncidenciaID = @incidenciaID
			and fa.Entidad = relp.Entidad
		) as Referencia,
		inc.Descripcion
	from Sam3_Incidencia inc
	inner join Sam3_TipoIncidencia tpi on inc.TipoIncidenciaID = tpi.TipoIncidenciaID
	where IncidenciaID = @incidenciaID
end

if @tipoIncidenciaID = 2 -- entrada de material
begin 
	select 
		(
			select 
				LTRIM(RTRIM(rel.PreFijoFolioIncidencias)) +
				REPLACE(
						STR(
							LTRIM(RTRIM(inc2.Consecutivo)),
							CASE
								WHEN LEN(inc2.Consecutivo) > rel.CantidadCerosFolioIncidencias 
								THEN LEN(inc2.Consecutivo)
								ELSE rel.CantidadCerosFolioIncidencias
							END
						), SPACE(1), '0'
				) +
				LTRIM(RTRIM(rel.PostFijoFolioIncidencias))
			from Sam3_Incidencia inc2 inner join Sam3_Rel_Proyecto_Entidad_Configuracion rel 
			on inc2.Rel_Proyecto_Entidad_Configuracion_ID = rel.Rel_Proyecto_Entidad_Configuracion_ID
			where inc2.IncidenciaID = @incidenciaID
		) as 'NoReporte',
		inc.FechaCreacion,
		(
			select cl.Nombre
			from Sam3_Rel_Incidencia_FolioAvisoEntrada relinc
			inner join Sam3_FolioAvisoEntrada fe on relinc.FolioAvisoEntradaID = fe.FolioAvisoEntradaID
			inner join Sam3_FolioAvisoLlegada fa on fe.FolioAvisoLlegadaID = fa.FolioAvisoLlegadaID
			inner join Sam3_Cliente cl on fa.ClienteID = cl.ClienteID
			where relinc.IncidenciaID = @incidenciaID
		) as Cliente,
		tpi.Nombre as Clasificacion,
		(
			select p.Nombre
			from Sam3_Rel_Incidencia_FolioAvisoEntrada relinc
			inner join Sam3_FolioAvisoEntrada fe on relinc.FolioAvisoEntradaID = fe.FolioAvisoEntradaID
			inner join Sam3_FolioAvisoLlegada fa on fe.FolioAvisoLlegadaID = fa.FolioAvisoLlegadaID
			inner join Sam3_Rel_FolioAvisoLlegada_Proyecto relp on fa.FolioAvisoLlegadaID = relp.FolioAvisoLlegadaID
			inner join Sam3_Proyecto p on relp.ProyectoID = p.ProyectoID
			where relinc.IncidenciaID = @incidenciaID
		) as Proyecto,
		inc.Titulo,
		(
			select 
				LTRIM(RTRIM(relp.PreFijoFolioAvisoLlegada)) +
				REPLACE(
						STR(
							LTRIM(RTRIM(fa.Consecutivo)),
							CASE
								WHEN LEN(fa.Consecutivo) > relp.CantidadCerosFolioAvisoLlegada 
								THEN LEN(fa.Consecutivo)
								ELSE relp.CantidadCerosFolioAvisoLlegada
							END
						), SPACE(1), '0'
				) +
				LTRIM(RTRIM(relp.PostFijoFolioAvisoLlegada))
			from Sam3_Rel_Incidencia_FolioAvisoEntrada relinc
			inner join Sam3_FolioAvisoEntrada fe on relinc.FolioAvisoEntradaID = fe.FolioAvisoEntradaID
			inner join Sam3_FolioAvisoLlegada fa on fe.FolioAvisoLlegadaID = fa.FolioAvisoLlegadaID
			inner join Sam3_Rel_Proyecto_Entidad_Configuracion relp on fa.ProyectoNombrado = relp.Proyecto
			where relinc.IncidenciaID = @incidenciaID
			and fa.Entidad = relp.Entidad
		) as Referencia,
		inc.Descripcion
	from Sam3_Incidencia inc
	inner join Sam3_TipoIncidencia tpi on inc.TipoIncidenciaID = tpi.TipoIncidenciaID
	where IncidenciaID = @incidenciaID
end

if @tipoIncidenciaID = 4 -- packing list
begin
	select 
		(
			select 
				LTRIM(RTRIM(rel.PreFijoFolioIncidencias)) +
				REPLACE(
						STR(
							LTRIM(RTRIM(inc2.Consecutivo)),
							CASE
								WHEN LEN(inc2.Consecutivo) > rel.CantidadCerosFolioIncidencias 
								THEN LEN(inc2.Consecutivo)
								ELSE rel.CantidadCerosFolioIncidencias
							END
						), SPACE(1), '0'
				) +
				LTRIM(RTRIM(rel.PostFijoFolioIncidencias))
			from Sam3_Incidencia inc2 inner join Sam3_Rel_Proyecto_Entidad_Configuracion rel 
			on inc2.Rel_Proyecto_Entidad_Configuracion_ID = rel.Rel_Proyecto_Entidad_Configuracion_ID
			where inc2.IncidenciaID = @incidenciaID
		) as 'NoReporte',
		inc.FechaCreacion,
		(
			select cl.Nombre
			from Sam3_Rel_Incidencia_FolioCuantificacion relinc
			inner join Sam3_FolioCuantificacion fc on relinc.FolioCuantificacionID = fc.FolioAvisoEntradaID
			inner join Sam3_Proyecto p on fc.ProyectoID = p.ProyectoID
			inner join Sam3_Cliente cl on p.ClienteID = cl.ClienteID
			where relinc.IncidenciaID = @incidenciaID
		) as Cliente,
		tpi.Nombre as Clasificacion,
		(
			select p.Nombre
			from Sam3_Rel_Incidencia_FolioCuantificacion relinc
			inner join Sam3_FolioCuantificacion fc on relinc.FolioCuantificacionID = fc.FolioAvisoEntradaID
			inner join Sam3_Proyecto p on fc.ProyectoID = p.ProyectoID
			where relinc.IncidenciaID = @incidenciaID
		) as Proyecto,
		inc.Titulo,
		(
			select 
				LTRIM(RTRIM(relp.PreFijoFolioPackingList)) +
				REPLACE(
						STR(
							LTRIM(RTRIM(fc.Consecutivo)),
							CASE
								WHEN LEN(fc.Consecutivo) > relp.CantidadCerosFolioPackingList 
								THEN LEN(fc.Consecutivo)
								ELSE relp.CantidadCerosFolioPackingList
							END
						), SPACE(1), '0'
				) +
				LTRIM(RTRIM(relp.PostFijoFolioPackingList))
			from Sam3_Rel_Incidencia_FolioCuantificacion relinc
			inner join Sam3_FolioCuantificacion fc on relinc.FolioCuantificacionID = fc.FolioCuantificacionID
			inner join Sam3_Rel_Proyecto_Entidad_Configuracion relp 
				on fc.Rel_Proyecto_Entidad_Configuracion_ID = relp.Rel_Proyecto_Entidad_Configuracion_ID
			where relinc.IncidenciaID = @incidenciaID
		) as Referencia,
		inc.Descripcion
	from Sam3_Incidencia inc
	inner join Sam3_TipoIncidencia tpi on inc.TipoIncidenciaID = tpi.TipoIncidenciaID
	where IncidenciaID = @incidenciaID
end

if @tipoIncidenciaID = 5 -- orden de recepcion
begin
	select 
		(
			select 
				LTRIM(RTRIM(rel.PreFijoFolioIncidencias)) +
				REPLACE(
						STR(
							LTRIM(RTRIM(inc2.Consecutivo)),
							CASE
								WHEN LEN(inc2.Consecutivo) > rel.CantidadCerosFolioIncidencias 
								THEN LEN(inc2.Consecutivo)
								ELSE rel.CantidadCerosFolioIncidencias
							END
						), SPACE(1), '0'
				) +
				LTRIM(RTRIM(rel.PostFijoFolioIncidencias))
			from Sam3_Incidencia inc2 inner join Sam3_Rel_Proyecto_Entidad_Configuracion rel 
			on inc2.Rel_Proyecto_Entidad_Configuracion_ID = rel.Rel_Proyecto_Entidad_Configuracion_ID
			where inc2.IncidenciaID = @incidenciaID
		) as 'NoReporte',
		inc.FechaCreacion,
		(
			select distinct(cl.Nombre)
			from Sam3_Rel_Incidencia_OrdenRecepcion relinc
			inner join Sam3_OrdenRecepcion ord on relinc.OrdenRecepcionID = ord.OrdenRecepcionID
			inner join Sam3_Rel_NumeroUnico_RelFC_RelB relnu on ord.OrdenRecepcionID = relnu.OrdenRecepcionID
			inner join Sam3_NumeroUnico nu on nu.NumeroUnicoID = relnu.NumeroUnicoID
			inner join Sam3_Proyecto p on nu.ProyectoID = p.ProyectoID
			inner join Sam3_Cliente cl on p.ClienteID = cl.ClienteID
			where relinc.IncidenciaID = @incidenciaID
		) as Cliente,
		tpi.Nombre as Clasificacion,
		(
			select distinct(p.Nombre)
			from Sam3_Rel_Incidencia_OrdenRecepcion relinc
			inner join Sam3_OrdenRecepcion ord on relinc.OrdenRecepcionID = ord.OrdenRecepcionID
			inner join Sam3_Rel_NumeroUnico_RelFC_RelB relnu on ord.OrdenRecepcionID = relnu.OrdenRecepcionID
			inner join Sam3_NumeroUnico nu on nu.NumeroUnicoID = relnu.NumeroUnicoID
			inner join Sam3_Proyecto p on nu.ProyectoID = p.ProyectoID
			where relinc.IncidenciaID = @incidenciaID
		) as Proyecto,
		inc.Titulo,
		(
			select 
				LTRIM(RTRIM(relp.PreFijoFolioOrdenRecepcion)) +
				REPLACE(
						STR(
							LTRIM(RTRIM(ord.Consecutivo)),
							CASE
								WHEN LEN(ord.Consecutivo) > relp.CantidadCerosFolioOrdenRecepcion 
								THEN LEN(ord.Consecutivo)
								ELSE relp.CantidadCerosFolioOrdenRecepcion
							END
						), SPACE(1), '0'
				) +
				LTRIM(RTRIM(relp.PostFijoFolioOrdenRecepcion))
			from Sam3_Rel_Incidencia_OrdenRecepcion relinc
			inner join Sam3_OrdenRecepcion ord on relinc.OrdenRecepcionID = ord.OrdenRecepcionID
			inner join Sam3_Rel_Proyecto_Entidad_Configuracion relp 
			on relp.Rel_Proyecto_Entidad_Configuracion_ID = ord.Rel_Proyecto_Entidad_Configuracion_ID
			where relinc.IncidenciaID = @incidenciaID
		) as Referencia,
		inc.Descripcion
	from Sam3_Incidencia inc
	inner join Sam3_TipoIncidencia tpi on inc.TipoIncidenciaID = tpi.TipoIncidenciaID
	where IncidenciaID = @incidenciaID
end

if @tipoIncidenciaID = 7 -- itemcode
begin
	select 
		(
			select 
				LTRIM(RTRIM(rel.PreFijoFolioIncidencias)) +
				REPLACE(
						STR(
							LTRIM(RTRIM(inc2.Consecutivo)),
							CASE
								WHEN LEN(inc2.Consecutivo) > rel.CantidadCerosFolioIncidencias 
								THEN LEN(inc2.Consecutivo)
								ELSE rel.CantidadCerosFolioIncidencias
							END
						), SPACE(1), '0'
				) +
				LTRIM(RTRIM(rel.PostFijoFolioIncidencias))
			from Sam3_Incidencia inc2 inner join Sam3_Rel_Proyecto_Entidad_Configuracion rel 
			on inc2.Rel_Proyecto_Entidad_Configuracion_ID = rel.Rel_Proyecto_Entidad_Configuracion_ID
			where inc2.IncidenciaID = @incidenciaID
		) as 'NoReporte',
		inc.FechaCreacion,
		(
			select distinct(cl.Nombre)
			from Sam3_Rel_Incidencia_ItemCode relinc
			inner join Sam3_ItemCode it on relinc.ItemCodeID = it.ItemCodeID
			inner join Sam3_Proyecto p on it.ProyectoID = p.ProyectoID
			inner join Sam3_Cliente cl on p.ClienteID = cl.ClienteID
			where relinc.IncidenciaID = @incidenciaID
		) as Cliente,
		tpi.Nombre as Clasificacion,
		(
			select distinct(p.Nombre)
			from Sam3_Rel_Incidencia_ItemCode relinc
			inner join Sam3_ItemCode it on relinc.ItemCodeID = it.ItemCodeID
			inner join Sam3_Proyecto p on it.ProyectoID = p.ProyectoID
			where relinc.IncidenciaID = @incidenciaID
		) as Proyecto,
		inc.Titulo,
		(
			select it.Codigo
			from Sam3_Rel_Incidencia_ItemCode relinc
			inner join Sam3_ItemCode it on relinc.ItemCodeID = it.ItemCodeID
			where relinc.IncidenciaID = @incidenciaID
		) as Referencia,
		inc.Descripcion
	from Sam3_Incidencia inc
	inner join Sam3_TipoIncidencia tpi on inc.TipoIncidenciaID = tpi.TipoIncidenciaID
	where IncidenciaID = @incidenciaID
end

if @tipoIncidenciaID = 8 -- orden de almacenaje
begin
	select 
		(
			select 
				LTRIM(RTRIM(rel.PreFijoFolioIncidencias)) +
				REPLACE(
						STR(
							LTRIM(RTRIM(inc2.Consecutivo)),
							CASE
								WHEN LEN(inc2.Consecutivo) > rel.CantidadCerosFolioIncidencias 
								THEN LEN(inc2.Consecutivo)
								ELSE rel.CantidadCerosFolioIncidencias
							END
						), SPACE(1), '0'
				) +
				LTRIM(RTRIM(rel.PostFijoFolioIncidencias))
			from Sam3_Incidencia inc2 inner join Sam3_Rel_Proyecto_Entidad_Configuracion rel 
			on inc2.Rel_Proyecto_Entidad_Configuracion_ID = rel.Rel_Proyecto_Entidad_Configuracion_ID
			where inc2.IncidenciaID = @incidenciaID
		) as 'NoReporte',
		inc.FechaCreacion,
		(
			select distinct(cl.Nombre)
			from Sam3_Rel_Incidencia_OrdenAlmacenaje relinc
			inner join Sam3_Rel_OrdenAlmacenaje_NumeroUnico roa on relinc.OrdenalmacenajeID = roa.OrdenAlmacenajeID
			inner join Sam3_NumeroUnico nu on nu.NumeroUnicoID = roa.NumeroUnicoID
			inner join Sam3_Proyecto p on nu.ProyectoID = p.ProyectoID
			inner join Sam3_Cliente cl on p.ClienteID = cl.ClienteID
			where relinc.IncidenciaID = @incidenciaID
		) as Cliente,
		tpi.Nombre as Clasificacion,
		(
			select distinct(p.Nombre)
			from Sam3_Rel_Incidencia_OrdenAlmacenaje relinc
			inner join Sam3_Rel_OrdenAlmacenaje_NumeroUnico roa on relinc.OrdenalmacenajeID = roa.OrdenAlmacenajeID
			inner join Sam3_NumeroUnico nu on nu.NumeroUnicoID = roa.NumeroUnicoID
			inner join Sam3_Proyecto p on nu.ProyectoID = p.ProyectoID
			where relinc.IncidenciaID = @incidenciaID
		) as Proyecto,
		inc.Titulo,
		(
			select 
				LTRIM(RTRIM(relp.PreFijoFolioOrdenAlmacenaje)) +
				REPLACE(
						STR(
							LTRIM(RTRIM(oa.Consecutivo)),
							CASE
								WHEN LEN(oa.Consecutivo) > relp.CantidadCerosFolioOrdenAlmacenaje 
								THEN LEN(oa.Consecutivo)
								ELSE relp.CantidadCerosFolioOrdenAlmacenaje
							END
						), SPACE(1), '0'
				) +
				LTRIM(RTRIM(relp.PostFijoFolioOrdenAlmacenaje))
			from Sam3_Rel_Incidencia_OrdenAlmacenaje relinc
			inner join Sam3_OrdenAlmacenaje oa on relinc.OrdenalmacenajeID = oa.OrdenAlmacenajeID
			inner join Sam3_Rel_Proyecto_Entidad_Configuracion relp 
			on relp.Rel_Proyecto_Entidad_Configuracion_ID = oa.Rel_Proyecto_Entidad_Configuracion_ID
			where relinc.IncidenciaID = @incidenciaID
		) as Referencia,
		inc.Descripcion
	from Sam3_Incidencia inc
	inner join Sam3_TipoIncidencia tpi on inc.TipoIncidenciaID = tpi.TipoIncidenciaID
	where IncidenciaID = @incidenciaID
end

if @tipoIncidenciaID = 9 -- numero unico
begin
	select 
		(
			select 
				LTRIM(RTRIM(rel.PreFijoFolioIncidencias)) +
				REPLACE(
						STR(
							LTRIM(RTRIM(inc2.Consecutivo)),
							CASE
								WHEN LEN(inc2.Consecutivo) > rel.CantidadCerosFolioIncidencias 
								THEN LEN(inc2.Consecutivo)
								ELSE rel.CantidadCerosFolioIncidencias
							END
						), SPACE(1), '0'
				) +
				LTRIM(RTRIM(rel.PostFijoFolioIncidencias))
			from Sam3_Incidencia inc2 inner join Sam3_Rel_Proyecto_Entidad_Configuracion rel 
			on inc2.Rel_Proyecto_Entidad_Configuracion_ID = rel.Rel_Proyecto_Entidad_Configuracion_ID
			where inc2.IncidenciaID = @incidenciaID
		) as 'NoReporte',
		inc.FechaCreacion,
		(
			select distinct(cl.Nombre)
			from Sam3_Rel_Incidencia_NumeroUnico relinc
			inner join Sam3_NumeroUnico nu on nu.NumeroUnicoID = relinc.NumeroUnicoID
			inner join Sam3_Proyecto p on nu.ProyectoID = p.ProyectoID
			inner join Sam3_Cliente cl on p.ClienteID = cl.ClienteID
			where relinc.IncidenciaID = @incidenciaID
		) as Cliente,
		tpi.Nombre as Clasificacion,
		(
			select distinct(p.Nombre)
			from Sam3_Rel_Incidencia_NumeroUnico relinc
			inner join Sam3_NumeroUnico nu on nu.NumeroUnicoID = relinc.NumeroUnicoID
			inner join Sam3_Proyecto p on nu.ProyectoID = p.ProyectoID
			where relinc.IncidenciaID = @incidenciaID
		) as Proyecto,
		inc.Titulo,
		(
			select 
				nu2.Prefijo +'-' + 
				REPLACE(
					STR(
						nu2.Consecutivo,
						CASE 
							WHEN LEN(nu2.Consecutivo)> pc.DigitosNumeroUnico THEN LEN(nu2.Consecutivo) 
							ELSE pc.DigitosNumeroUnico 
							END
						), SPACE(1), '0')
			from Sam3_Rel_Incidencia_NumeroUnico relinc
			inner join Sam3_NumeroUnico nu2 on nu2.NumeroUnicoID = relinc.NumeroUnicoID
			inner join Sam3_ProyectoConfiguracion pc on nu2.ProyectoID = pc.ProyectoID
			where relinc.IncidenciaID = @incidenciaID
		) as Referencia,
		inc.Descripcion
	from Sam3_Incidencia inc
	inner join Sam3_TipoIncidencia tpi on inc.TipoIncidenciaID = tpi.TipoIncidenciaID
	where IncidenciaID = @incidenciaID
end

if @tipoIncidenciaID = 10 -- despacho
begin
	select 
		(
			select 
				LTRIM(RTRIM(rel.PreFijoFolioIncidencias)) +
				REPLACE(
						STR(
							LTRIM(RTRIM(inc2.Consecutivo)),
							CASE
								WHEN LEN(inc2.Consecutivo) > rel.CantidadCerosFolioIncidencias 
								THEN LEN(inc2.Consecutivo)
								ELSE rel.CantidadCerosFolioIncidencias
							END
						), SPACE(1), '0'
				) +
				LTRIM(RTRIM(rel.PostFijoFolioIncidencias))
			from Sam3_Incidencia inc2 inner join Sam3_Rel_Proyecto_Entidad_Configuracion rel 
			on inc2.Rel_Proyecto_Entidad_Configuracion_ID = rel.Rel_Proyecto_Entidad_Configuracion_ID
			where inc2.IncidenciaID = @incidenciaID
		) as 'NoReporte',
		inc.FechaCreacion,
		(
			select distinct(cl.Nombre)
			from Sam3_Rel_Incidencia_Despacho relinc
			inner join Sam3_Despacho d on relinc.DespachoID = d.DespachoID
			inner join Sam3_Proyecto p on d.ProyectoID = p.ProyectoID
			inner join Sam3_Cliente cl on p.ClienteID = cl.ClienteID
			where relinc.IncidenciaID = @incidenciaID
		) as Cliente,
		tpi.Nombre as Clasificacion,
		(
			select distinct(p.Nombre)
			from Sam3_Rel_Incidencia_Despacho relinc
			inner join Sam3_Despacho d on relinc.DespachoID = d.DespachoID
			inner join Sam3_Proyecto p on d.ProyectoID = p.ProyectoID
			where relinc.IncidenciaID = @incidenciaID
		) as Proyecto,
		inc.Titulo,
		(
			select
			(
				select odts.NumeroControl + '-' + ms.Etiqueta
				from sam2.sam.dbo.OrdenTrabajoSpool odts
				inner join sam2.sam.dbo.OrdenTrabajoMaterial odtm 
					on odts.OrdenTrabajoSpoolID = odtm.OrdenTrabajoSpoolID
				inner join sam2.sam.dbo.MaterialSpool ms 
					on odtm.MaterialSpoolID = ms.MaterialSpoolID
				where odts.OrdenTrabajoSpoolID = d.OrdenTrabajoSpoolID
				and odtm.MaterialSpoolID = d.MaterialSpoolID
			)
			from  Sam3_Rel_Incidencia_Despacho relinc
			inner join Sam3_Despacho d on relinc.DespachoID = d.DespachoID
			where relinc.IncidenciaID = @incidenciaID
		) as Referencia,
		inc.Descripcion
	from Sam3_Incidencia inc
	inner join Sam3_TipoIncidencia tpi on inc.TipoIncidenciaID = tpi.TipoIncidenciaID
	where IncidenciaID = @incidenciaID
end

if @tipoIncidenciaID = 11 -- corte
begin
	select 
		(
			select 
				LTRIM(RTRIM(rel.PreFijoFolioIncidencias)) +
				REPLACE(
						STR(
							LTRIM(RTRIM(inc2.Consecutivo)),
							CASE
								WHEN LEN(inc2.Consecutivo) > rel.CantidadCerosFolioIncidencias 
								THEN LEN(inc2.Consecutivo)
								ELSE rel.CantidadCerosFolioIncidencias
							END
						), SPACE(1), '0'
				) +
				LTRIM(RTRIM(rel.PostFijoFolioIncidencias))
			from Sam3_Incidencia inc2 inner join Sam3_Rel_Proyecto_Entidad_Configuracion rel 
			on inc2.Rel_Proyecto_Entidad_Configuracion_ID = rel.Rel_Proyecto_Entidad_Configuracion_ID
			where inc2.IncidenciaID = @incidenciaID
		) as 'NoReporte',
		inc.FechaCreacion,
		(
			select distinct(cl.Nombre)
			from Sam3_Rel_Incidencia_Corte relinc
			inner join Sam3_Corte c on relinc.CorteID = c.CorteID
			inner join Sam3_Proyecto p on c.ProyectoID = p.ProyectoID
			inner join Sam3_Cliente cl on p.ClienteID = cl.ClienteID
			where relinc.IncidenciaID = @incidenciaID
		) as Cliente,
		tpi.Nombre as Clasificacion,
		(
			select distinct(p.Nombre)
			from Sam3_Rel_Incidencia_Corte relinc
			inner join Sam3_Corte c on relinc.CorteID = c.CorteID
			inner join Sam3_Proyecto p on c.ProyectoID = p.ProyectoID
			where relinc.IncidenciaID = @incidenciaID
		) as Proyecto,
		inc.Titulo,
		(
			select c.CorteID
			from  Sam3_Rel_Incidencia_Corte relinc
			inner join Sam3_Corte c on relinc.CorteID = c.CorteID
			where relinc.IncidenciaID = @incidenciaID
		) as Referencia,
		inc.Descripcion
	from Sam3_Incidencia inc
	inner join Sam3_TipoIncidencia tpi on inc.TipoIncidenciaID = tpi.TipoIncidenciaID
	where IncidenciaID = @incidenciaID
end

--if @tipoIncidenciaID = 12 -- predespacho
--begin
--	select 
--		(
--			select 
--				LTRIM(RTRIM(rel.PreFijoFolioIncidencias)) +
--				REPLACE(
--						STR(
--							LTRIM(RTRIM(inc2.Consecutivo)),
--							CASE
--								WHEN LEN(inc2.Consecutivo) > rel.CantidadCerosFolioIncidencias 
--								THEN LEN(inc2.Consecutivo)
--								ELSE rel.CantidadCerosFolioIncidencias
--							END
--						), SPACE(1), '0'
--				) +
--				LTRIM(RTRIM(rel.PostFijoFolioIncidencias))
--			from Sam3_Incidencia inc2 inner join Sam3_Rel_Proyecto_Entidad_Configuracion rel 
--			on inc2.Rel_Proyecto_Entidad_Configuracion_ID = rel.Rel_Proyecto_Entidad_Configuracion_ID
--			where inc2.IncidenciaID = @incidenciaID
--		) as 'NoReporte',
--		inc.FechaCreacion,
--		(
--			select distinct(cl.Nombre)
--			from Sam3_Rel_Incidencia_Corte relinc
--			inner join Sam3_Corte c on relinc.CorteID = c.CorteID
--			inner join Sam3_Proyecto p on c.ProyectoID = p.ProyectoID
--			inner join Sam3_Cliente cl on p.ClienteID = cl.ClienteID
--			where relinc.IncidenciaID = @incidenciaID
--		) as Cliente,
--		tpi.Nombre as Clasificacion,
--		(
--			select distinct(p.Nombre)
--			from Sam3_Rel_Incidencia_Corte relinc
--			inner join Sam3_Corte c on relinc.CorteID = c.CorteID
--			inner join Sam3_Proyecto p on c.ProyectoID = p.ProyectoID
--			where relinc.IncidenciaID = @incidenciaID
--		) as Proyecto,
--		inc.Titulo,
--		(
--			select c.CorteID
--			from  Sam3_Rel_Incidencia_Corte relinc
--			inner join Sam3_Corte c on relinc.CorteID = c.CorteID
--			where relinc.IncidenciaID = @incidenciaID
--		) as Referencia,
--		inc.Descripcion
--	from Sam3_Incidencia inc
--	inner join Sam3_TipoIncidencia tpi on inc.TipoIncidenciaID = tpi.TipoIncidenciaID
--	where IncidenciaID = @incidenciaID
--end
END
GO
