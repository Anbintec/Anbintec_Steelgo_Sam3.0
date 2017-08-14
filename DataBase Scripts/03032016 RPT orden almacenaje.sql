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

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Sam3_RPT_OrdenAlmacenaje]') AND type in (N'P', N'PC'))
        DROP PROCEDURE [dbo].[Sam3_RPT_OrdenAlmacenaje]
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
CREATE PROCEDURE Sam3_RPT_OrdenAlmacenaje
	-- Add the parameters for the stored procedure here
	@ordenAlmacenajeID int, @usuarioID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	declare @relfcID int, @relbID int, @fcID int

--set @orID = 7188


declare @result table(
	NumeroUnico varchar(max),
	OrdenAlmacenaje varchar(max),
	Cliente varchar(max),
	Proyecto varchar(max),
	ItemCode varchar(max),
	Descripcion varchar(max),
	Diametro1 varchar(max),
	Diametro2 varchar(max),
	Espesor varchar(max),
	TipoMaterial varchar(max),
	Cantidad varchar(max),
	Responsable varchar(max)
)


	insert into @result
	Select
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
			from Sam3_NumeroUnico nu2 inner join Sam3_ProyectoConfiguracion pc
			on nu2.ProyectoID = pc.ProyectoID
			where nu2.NumeroUnicoID = nu.NumeroUnicoID
		), 
		(
			select 
				LTRIM(RTRIM(rel.PreFijoFolioOrdenAlmacenaje)) +
				REPLACE(
						STR(
							LTRIM(RTRIM(ord.Folio)),
							CASE
								WHEN LEN(ord.Folio) > rel.CantidadCerosFolioOrdenAlmacenaje THEN LEN(ord.Folio)
								ELSE rel.CantidadCerosFolioOrdenAlmacenaje
							END
						), SPACE(1), '0'
				) +
				LTRIM(RTRIM(rel.PostFijoFolioOrdenAlmacenaje))
			from Sam3_OrdenAlmacenaje oa inner join Sam3_Rel_Proyecto_Entidad_Configuracion rel 
			on oa.Rel_Proyecto_Entidad_Configuracion_ID = rel.Rel_Proyecto_Entidad_Configuracion_ID
			where OrdenAlmacenajeID = @ordenAlmacenajeID
		),
		cli.Nombre,
		(
			select Nombre
			from Sam3_Proyecto
			where ProyectoID = nu.ProyectoID
		),
		it.Codigo,
		it.DescripcionEspanol,
		d1.Valor,
		d2.Valor,
		'0.00' as Espesor, --No tengo claro de donde saldra este dato
		tpm.Nombre,
		(
			select CantidadRecibida
			from Sam3_NumeroUnicoInventario
			where NumeroUnicoID = nu.NumeroUnicoID
		),
		(
			select Nombre
			from Sam3_Usuario
			where UsuarioID = @usuarioID
		)
	from Sam3_OrdenAlmacenaje ord
	inner join Sam3_Rel_OrdenAlmacenaje_NumeroUnico reloa
		on ord.OrdenAlmacenajeID = reloa.OrdenAlmacenajeID
	inner join Sam3_NumeroUnico nu
		on reloa.NumeroUnicoID = nu.NumeroUnicoID
	inner join Sam3_ItemCode it
		on nu.ItemCodeID = it.ItemCodeID
	inner join Sam3_Rel_ItemCode_Diametro relitd 
		on it.ItemCodeID = relitd.ItemCodeID
	inner join Sam3_Diametro d1 
		on relitd.Diametro1ID = d1.DiametroID
	inner join Sam3_Diametro d2
		on relitd.Diametro2ID = d2.DiametroID
	inner join Sam3_TipoMaterial tpm 
		on it.TipoMaterialID = tpm.TipoMaterialID
	inner join Sam3_Colada c
		on nu.ColadaID = c.ColadaID
	inner join Sam3_Proyecto p 
		on nu.ProyectoID = p.ProyectoID
	inner join Sam3_Cliente cli 
	 on p.ClienteID = cli.ClienteID
	where ord.OrdenAlmacenajeID = @ordenAlmacenajeID

select * from @result
END
GO
