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

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Sam3_RPT_EtiquetasDeMaterial]') AND type in (N'P', N'PC'))
        DROP PROCEDURE [dbo].[Sam3_RPT_EtiquetasDeMaterial]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Julian Hernandez
-- Create date: 03-03-2016
-- Description:	Etiquetas de material
-- =============================================
CREATE PROCEDURE Sam3_RPT_EtiquetasDeMaterial
	-- Add the parameters for the stored procedure here
	@ordenRecepcionID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	declare @result table(
	NumeroUnicoID int,
	NumeroUnico varchar(max),
	Segmento varchar(5),
	Itemcode varchar(max),
	Diametro1 varchar(max),
	Diametro2 varchar(max),
	Colada varchar(max),
	TotalKg varchar(max),
	Cedula varchar(max),
	Longitud varchar(max),
	KgmL varchar(max),
	Descripcion varchar(max),
	Proyecto varchar(max),
	Cliente varchar(max)
)

--Insertar Tubos
insert into @result
select 
	nu.NumeroUnicoID,
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
	nus.Segmento,
	it.Codigo,
	d1.Valor,
	d2.Valor,
	colada.NumeroColada,
	(
		(select
			its.Peso
		from Sam3_ItemCodeSteelgo its inner join Sam3_Rel_ItemCodeSteelgo_Diametro itsd
			on its.ItemCodeSteelgoID = itsd.ItemCodeSteelgoID
		inner join Sam3_Rel_ItemCode_ItemCodeSteelgo itis 
			on itsd.Rel_ItemCodeSteelgo_Diametro_ID = itis.Rel_ItemCodeSteelgo_Diametro_ID
		inner join Sam3_Rel_ItemCode_Diametro itd 
			on itis.Rel_ItemCode_Diametro_ID = itd.Rel_ItemCode_Diametro_ID
		inner join Sam3_ItemCode it2 on itd.ItemCodeID = it2.ItemCodeID
		where it2.ItemCodeID = it.ItemCodeID
		)
	), --total kgs
	nu.Cedula,
	nus.InventarioFisico,
	(
		convert(varchar,it.Peso) + '/' + convert(varchar,(nus.InventarioFisico /1000))
	),
	it.DescripcionEspanol,
	p.Nombre,
	cli.Nombre
from Sam3_OrdenRecepcion ord inner join Sam3_Rel_NumeroUnico_RelFC_RelB relnu
	on ord.OrdenRecepcionID = relnu.OrdenRecepcionID
inner join Sam3_NumeroUnico nu on relnu.NumeroUnicoID = nu.NumeroUnicoID
inner join Sam3_ItemCode it on nu.ItemCodeID = it.ItemCodeID
left join Sam3_Colada colada on nu.ColadaID = colada.ColadaID
inner join Sam3_Proyecto p on nu.ProyectoID = p.ProyectoID
inner join Sam3_Cliente cli on p.ClienteID = cli.ClienteID
left join Sam3_Diametro d1 on nu.Diametro1 = d1.DiametroID
left join Sam3_Diametro d2 on nu.Diametro2 = d2.DiametroID
inner join Sam3_NumeroUnicoInventario nui on nu.NumeroUnicoID = nui.NumeroUnicoID
inner join Sam3_NumeroUnicoSegmento nus on nu.NumeroUnicoID = nus.NumeroUnicoID
where it.TipoMaterialID = 1
and ord.OrdenRecepcionID = @ordenRecepcionID

--Insertar Accesorios
insert into @result
select 
	nu.NumeroUnicoID,
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
	'N/A',
	it.Codigo,
	d1.Valor,
	d2.Valor,
	colada.NumeroColada,
	(
		it.Peso * nui.InventarioFisico
	),
	nu.Cedula,
	nui.InventarioFisico,
	(
		'N/A'
	),
	it.DescripcionEspanol,
	p.Nombre,
	cli.Nombre
from Sam3_OrdenRecepcion ord inner join Sam3_Rel_NumeroUnico_RelFC_RelB relnu
	on ord.OrdenRecepcionID = relnu.OrdenRecepcionID
inner join Sam3_NumeroUnico nu on relnu.NumeroUnicoID = nu.NumeroUnicoID
inner join Sam3_ItemCode it on nu.ItemCodeID = it.ItemCodeID
left join Sam3_Colada colada on nu.ColadaID = colada.ColadaID
inner join Sam3_Proyecto p on nu.ProyectoID = p.ProyectoID
inner join Sam3_Cliente cli on p.ClienteID = cli.ClienteID
left join Sam3_Diametro d1 on nu.Diametro1 = d1.DiametroID
left join Sam3_Diametro d2 on nu.Diametro2 = d2.DiametroID
inner join Sam3_NumeroUnicoInventario nui on nu.NumeroUnicoID = nui.NumeroUnicoID
where it.TipoMaterialID = 2
and ord.OrdenRecepcionID = @ordenRecepcionID

select * from @result
END
GO
