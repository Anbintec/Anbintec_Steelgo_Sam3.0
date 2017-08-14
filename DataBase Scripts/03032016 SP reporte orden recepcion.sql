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

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Sam3_RPT_OrdenRecepcion]') AND type in (N'P', N'PC'))
        DROP PROCEDURE [dbo].[Sam3_RPT_OrdenRecepcion]
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Julian Hernandez
-- Create date: 01-03-2016
-- Description:	Obtener datos para reporte de Orden de recepcion
-- =============================================
CREATE PROCEDURE Sam3_RPT_OrdenRecepcion
	-- Add the parameters for the stored procedure here
	@ordenRecepcionID int, @usuarioID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	declare @relfcID int, @relbID int, @fcID int

--set @orID = 7188

declare @temp table(
	OrdenRecepcionID int,
	RelFCID int,
	RELBID int
)

declare @result table(
	NumeroUnico varchar(max),
	OrdenRecepcion varchar(max),
	Cliente varchar(max),
	Proyecto varchar(max),
	AvisoEntrada varchar(max),
	PackingList varchar(max),
	ItemCode varchar(max),
	Descripcion varchar(max),
	Diametro1 varchar(max),
	Diametro2 varchar(max),
	Espesor varchar(max),
	TipoMaterial varchar(max),
	Cantidad varchar(max),
	Colada varchar(max),
	Responsable varchar(max)
)

insert into @temp (OrdenRecepcionID, RelFCID, RELBID)
	select OrdenRecepcionID, Rel_FolioCuantificacion_ItemCode_ID, Rel_Bulto_ItemCode_ID
	from Sam3_Rel_NumeroUnico_RelFC_RelB
	where OrdenRecepcionID = @ordenRecepcionID

	insert into @result
	Select
		(
			select 
				nu.Prefijo +'-' + 
				REPLACE(
					STR(
						nu.Consecutivo,
						CASE 
							WHEN LEN(nu.Consecutivo)> pc.DigitosNumeroUnico THEN LEN(nu.Consecutivo) 
							ELSE pc.DigitosNumeroUnico 
							END
						), SPACE(1), '0')
			from Sam3_NumeroUnico nu inner join Sam3_ProyectoConfiguracion pc
			on nu.ProyectoID = pc.ProyectoID
			where nu.NumeroUnicoID = relnu.NumeroUnicoID
		), 
		(
			select 
				LTRIM(RTRIM(rel.PreFijoFolioOrdenRecepcion)) +
				REPLACE(
						STR(
							LTRIM(RTRIM(ord.Folio)),
							CASE
								WHEN LEN(ord.Folio) > rel.CantidadCerosFolioOrdenRecepcion THEN LEN(ord.Folio)
								ELSE rel.CantidadCerosFolioOrdenRecepcion
							END
						), SPACE(1), '0'
				) +
				LTRIM(RTRIM(rel.PostFijoFolioOrdenRecepcion))
			from Sam3_OrdenRecepcion ord inner join Sam3_Rel_Proyecto_Entidad_Configuracion rel 
			on ord.Rel_Proyecto_Entidad_Configuracion_ID = rel.Rel_Proyecto_Entidad_Configuracion_ID
			where OrdenRecepcionID = @ordenRecepcionID
		),
		cli.Nombre,
		(
			select Nombre
			from Sam3_Proyecto
			where ProyectoID = (
				select ProyectoID
				from Sam3_NumeroUnico
				where NumeroUnicoID = relnu.NumeroUnicoID
			)
		),
		(
			select 
				LTRIM(RTRIM(rel.PreFijoFolioAvisoLlegada)) +
				REPLACE(
					STR(
					LTRIM(RTRIM(fa2.Consecutivo)),
						CASE
							WHEN LEN(fa2.Consecutivo) > rel.CantidadCerosFolioAvisoLlegada THEN LEN(fa2.Consecutivo)
							ELSE rel.CantidadCerosFolioAvisoLlegada
						END
					), SPACE(1), '0'
				) +
				LTRIM(RTRIM(rel.PostFijoFolioAvisoLlegada))
				from Sam3_FolioAvisoLlegada fa2 inner join Sam3_Rel_Proyecto_Entidad_Configuracion rel 
				on fa2.ProyectoNombrado = rel.Proyecto
				where fa2.FolioAvisoLlegadaID = fa.FolioAvisoLlegadaID
				and fa2.Entidad = rel.Entidad
		),
		(
			select 
				LTRIM(RTRIM(rel.PreFijoFolioPackingList)) +
				REPLACE(
					STR(
						LTRIM(RTRIM(fc2.ConsecutivoConfiguracion)),
						CASE
							WHEN LEN(fc2.ConsecutivoConfiguracion) > rel.CantidadCerosFolioPackingList 
							THEN LEN(fc2.ConsecutivoConfiguracion)
							ELSE rel.CantidadCerosFolioPackingList
						END
					), SPACE(1), '0'
				) +
				LTRIM(RTRIM(rel.PostFijoFolioPackingList))
			from Sam3_FolioCuantificacion fc2 inner join Sam3_Rel_Proyecto_Entidad_Configuracion rel 
			on fc2.Rel_Proyecto_Entidad_Configuracion_ID = rel.Rel_Proyecto_Entidad_Configuracion_ID
			where fc2.FolioCuantificacionID = fc.FolioCuantificacionID
		),
		it.Codigo,
		it.DescripcionEspanol,
		d1.Valor,
		d2.Valor,
		'0.00', --No tengo claro de donde saldra este dato
		tpm.Nombre,
		relfc.Cantidad,
		c.NumeroColada,
		(
			select Nombre
			from Sam3_Usuario
			where UsuarioID = @usuarioID
		)
	from Sam3_OrdenRecepcion ord
	inner join Sam3_Rel_NumeroUnico_RelFC_RelB relnu 
		on ord.OrdenRecepcionID = relnu.OrdenRecepcionID
	inner join Sam3_Rel_FolioCuantificacion_ItemCode relfc
		on relnu.Rel_FolioCuantificacion_ItemCode_ID = relfc.Rel_FolioCuantificacion_ItemCode_ID
	inner join Sam3_FolioCuantificacion fc 
		on relfc.FolioCuantificacionID = fc.FolioCuantificacionID
	inner join Sam3_FolioAvisoEntrada fe 
		on fc.FolioAvisoEntradaID = fe.FolioAvisoEntradaID
	inner join Sam3_FolioAvisoLlegada fa 
		on fe.FolioAvisoLlegadaID = fa.FolioAvisoLlegadaID
	inner join Sam3_Rel_ItemCode_Diametro relitd 
		on relfc.Rel_ItemCode_Diametro_ID = relitd.Rel_ItemCode_Diametro_ID
	inner join Sam3_ItemCode it
		on relitd.ItemCodeID = it.ItemCodeID
	inner join Sam3_Diametro d1 
		on relitd.Diametro1ID = d1.DiametroID
	inner join Sam3_Diametro d2
		on relitd.Diametro2ID = d2.DiametroID
	inner join Sam3_TipoMaterial tpm 
		on it.TipoMaterialID = tpm.TipoMaterialID
	inner join Sam3_Colada c
		on relfc.ColadaID = c.ColadaID
	inner join Sam3_Cliente cli 
	 on fe.ClienteID = cli.ClienteID
	where relfc.Rel_FolioCuantificacion_ItemCode_ID in (
		select RelFCID from @temp where RelFCID is not null
		and ord.OrdenRecepcionID = @ordenRecepcionID
	)
	and ord.OrdenRecepcionID = @ordenRecepcionID
---------------------------------------------------------------------------------------------------	
	insert into @result
	Select
		(
			select 
				nu.Prefijo +'-' + 
				REPLACE(
					STR(
						nu.Consecutivo,
						CASE 
							WHEN LEN(nu.Consecutivo)> pc.DigitosNumeroUnico THEN LEN(nu.Consecutivo) 
							ELSE pc.DigitosNumeroUnico 
							END
						), SPACE(1), '0')
			from Sam3_NumeroUnico nu inner join Sam3_ProyectoConfiguracion pc
			on nu.ProyectoID = pc.ProyectoID
			where nu.NumeroUnicoID = relnu.NumeroUnicoID
		), 
		(
			select 
				LTRIM(RTRIM(rel.PreFijoFolioOrdenRecepcion)) +
				REPLACE(
						STR(
							LTRIM(RTRIM(ord.Folio)),
							CASE
								WHEN LEN(ord.Folio) > rel.CantidadCerosFolioOrdenRecepcion THEN LEN(ord.Folio)
								ELSE rel.CantidadCerosFolioOrdenRecepcion
							END
						), SPACE(1), '0'
				) +
				LTRIM(RTRIM(rel.PostFijoFolioOrdenRecepcion))
			from Sam3_OrdenRecepcion ord inner join Sam3_Rel_Proyecto_Entidad_Configuracion rel 
			on ord.Rel_Proyecto_Entidad_Configuracion_ID = rel.Rel_Proyecto_Entidad_Configuracion_ID
			where OrdenRecepcionID = @ordenRecepcionID
		),
		cli.Nombre,
		(
			select Nombre
			from Sam3_Proyecto
			where ProyectoID = (
				select ProyectoID
				from Sam3_NumeroUnico
				where NumeroUnicoID = relnu.NumeroUnicoID
			)
		),
		(
			select 
				LTRIM(RTRIM(rel.PreFijoFolioAvisoLlegada)) +
				REPLACE(
					STR(
					LTRIM(RTRIM(fa2.Consecutivo)),
						CASE
							WHEN LEN(fa2.Consecutivo) > rel.CantidadCerosFolioAvisoLlegada THEN LEN(fa2.Consecutivo)
							ELSE rel.CantidadCerosFolioAvisoLlegada
						END
					), SPACE(1), '0'
				) +
				LTRIM(RTRIM(rel.PostFijoFolioAvisoLlegada))
				from Sam3_FolioAvisoLlegada fa2 inner join Sam3_Rel_Proyecto_Entidad_Configuracion rel 
				on fa2.ProyectoNombrado = rel.Proyecto
				where fa2.FolioAvisoLlegadaID = fa.FolioAvisoLlegadaID
				and fa2.Entidad = rel.Entidad
		),
		(
			select 
				LTRIM(RTRIM(rel.PreFijoFolioPackingList)) +
				REPLACE(
					STR(
						LTRIM(RTRIM(fc2.ConsecutivoConfiguracion)),
						CASE
							WHEN LEN(fc2.ConsecutivoConfiguracion) > rel.CantidadCerosFolioPackingList 
							THEN LEN(fc2.ConsecutivoConfiguracion)
							ELSE rel.CantidadCerosFolioPackingList
						END
					), SPACE(1), '0'
				) +
				LTRIM(RTRIM(rel.PostFijoFolioPackingList))
			from Sam3_FolioCuantificacion fc2 inner join Sam3_Rel_Proyecto_Entidad_Configuracion rel 
			on fc2.Rel_Proyecto_Entidad_Configuracion_ID = rel.Rel_Proyecto_Entidad_Configuracion_ID
			where fc2.FolioCuantificacionID = fc.FolioCuantificacionID
		),
		it.Codigo,
		it.DescripcionEspanol,
		d1.Valor,
		d2.Valor,
		'0.00', --No tengo claro de donde saldra este dato
		tpm.Nombre,
		relb.Cantidad,
		c.NumeroColada,
		(
			select Nombre
			from Sam3_Usuario
			where UsuarioID = @usuarioID
		)
	from Sam3_OrdenRecepcion ord
	inner join Sam3_Rel_NumeroUnico_RelFC_RelB relnu 
		on ord.OrdenRecepcionID = relnu.OrdenRecepcionID
	inner join Sam3_Rel_Bulto_ItemCode relb
		on relnu.Rel_Bulto_ItemCode_ID = relb.Rel_Bulto_ItemCode_ID
	inner join Sam3_Bulto b
		on relb.BultoID = b.BultoID
	inner join Sam3_FolioCuantificacion fc 
		on b.FolioCuantificacionID = fc.FolioCuantificacionID
	inner join Sam3_FolioAvisoEntrada fe 
		on fc.FolioAvisoEntradaID = fe.FolioAvisoEntradaID
	inner join Sam3_FolioAvisoLlegada fa 
		on fe.FolioAvisoLlegadaID = fa.FolioAvisoLlegadaID
	inner join Sam3_Rel_ItemCode_Diametro relitd 
		on relb.Rel_ItemCode_Diametro_ID = relitd.Rel_ItemCode_Diametro_ID
	inner join Sam3_ItemCode it
		on relitd.ItemCodeID = it.ItemCodeID
	inner join Sam3_Diametro d1 
		on relitd.Diametro1ID = d1.DiametroID
	inner join Sam3_Diametro d2
		on relitd.Diametro2ID = d2.DiametroID
	inner join Sam3_TipoMaterial tpm 
		on it.TipoMaterialID = tpm.TipoMaterialID
	inner join Sam3_Colada c
		on relb.ColadaID = c.ColadaID
	inner join Sam3_Cliente cli 
		on fe.ClienteID = cli.ClienteID
	where relb.Rel_Bulto_ItemCode_ID in (
		select RELBID from @temp where RELBID is not null
		and ord.OrdenRecepcionID = @ordenRecepcionID
	)
	and ord.OrdenRecepcionID = @ordenRecepcionID

select * from @result
END
GO
