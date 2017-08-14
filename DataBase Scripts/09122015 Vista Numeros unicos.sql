USE [steelgo-sam3]
GO

/****** Object:  View [dbo].[VW_NumerosUnicos]    Script Date: 12/9/2015 11:47:16 AM ******/
DROP VIEW [dbo].[VW_NumerosUnicos]
GO

/****** Object:  View [dbo].[VW_NumerosUnicos]    Script Date: 12/9/2015 11:47:16 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

--Numero unico
CREATE VIEW [dbo].[VW_NumerosUnicos] AS
SELECT 
	DISTINCT(nu.NumeroUnicoID),
	nu.Prefijo +'-' + REPLACE(STR(nu.Consecutivo,CASE 
		WHEN LEN(nu.Consecutivo)> pc.DigitosNumeroUnico THEN LEN(nu.Consecutivo) 
		ELSE pc.DigitosNumeroUnico END), SPACE(1), '0') as NumeroUnico,
	p.Nombre as Proyecto, 
	ic.ItemCodeID, 
	ic.Codigo as ItemCodeCodigo, 
	ics.ItemCodeSteelgoID, 
	ics.Codigo as ItemCodeSteelgoCodigo, 
	fc.FolioCuantificacionID, 
	ave.FolioAvisoLlegadaID, 
	fc.FolioAvisoEntradaID, 
	c.Nombre as Cliente, 
	nu.FechaModificacion,
	p.PatioID as Patio
FROM Sam3_NumeroUnico nu
	INNER JOIN Sam3_ItemCode ic on nu.ItemCodeID = ic.ItemCodeID
	INNER JOIN Sam3_Rel_ItemCode_Diametro icd on ic.ItemCodeID = icd.ItemCodeID
	INNER JOIN Sam3_Rel_FolioCuantificacion_ItemCode rfc on icd.Rel_ItemCode_Diametro_ID = rfc.Rel_ItemCode_Diametro_ID
	INNER JOIN Sam3_FolioCuantificacion fc on rfc.FolioCuantificacionID = fc.FolioCuantificacionID
	INNER JOIN Sam3_FolioAvisoEntrada ave on fc.FolioAvisoEntradaID = ave.FolioAvisoEntradaID
	INNER JOIN Sam3_FolioAvisoLlegada avll on ave.FolioAvisoLlegadaID = avll.FolioAvisoLlegadaID
	INNER JOIN Sam3_Rel_ItemCode_ItemCodeSteelgo ric on ic.ItemCodeID = ric.ItemCodeID
	INNER JOIN Sam3_ItemCodeSteelgo ics on ric.ItemCodeSteelgoID = ics.ItemCodeSteelgoID
	INNER JOIN Sam3_Cliente c on avll.ClienteID = c.ClienteID
	INNER JOIN Sam3_Proyecto p on nu.ProyectoID = p.ProyectoID
	INNER JOIN Sam3_ProyectoConfiguracion pc on nu.ProyectoID = pc.ProyectoID
	INNER JOIN Sam3_ProyectoConsecutivo pcn on nu.ProyectoID = pcn.ProyectoID
UNION
SELECT 
	DISTINCT(nu.NumeroUnicoID),
	nu.Prefijo +'-' + REPLACE(STR(nu.Consecutivo,CASE 
		WHEN LEN(nu.Consecutivo)> pc.DigitosNumeroUnico THEN LEN(nu.Consecutivo) 
		ELSE pc.DigitosNumeroUnico END), SPACE(1), '0') as NumeroUnico,
	p.Nombre as Proyecto, 
	ic.ItemCodeID, 
	ic.Codigo as ItemCodeCodigo, 
	ics.ItemCodeSteelgoID, 
	ics.Codigo as ItemCodeSteelgoCodigo, 
	fc.FolioCuantificacionID, 
	ave.FolioAvisoLlegadaID, 
	fc.FolioAvisoEntradaID, 
	c.Nombre as Cliente, 
	nu.FechaModificacion,
	p.PatioID as Patio
FROM Sam3_NumeroUnico nu
	INNER JOIN Sam3_ItemCode ic on nu.ItemCodeID = ic.ItemCodeID
	INNER JOIN Sam3_Rel_ItemCode_Diametro icd on ic.ItemCodeID = icd.ItemCodeID
	INNER JOIN Sam3_Rel_Bulto_ItemCode rb on icd.Rel_ItemCode_Diametro_ID = rb.Rel_ItemCode_Diametro_ID
	INNER JOIN Sam3_Bulto b on rb.BultoID = b.BultoID
	INNER JOIN Sam3_FolioCuantificacion fc on b.FolioCuantificacionID = fc.FolioCuantificacionID
	INNER JOIN Sam3_FolioAvisoEntrada ave on fc.FolioAvisoEntradaID = ave.FolioAvisoEntradaID
	INNER JOIN Sam3_FolioAvisoLlegada avll on ave.FolioAvisoLlegadaID = avll.FolioAvisoLlegadaID
	INNER JOIN Sam3_Rel_ItemCode_ItemCodeSteelgo ric on ic.ItemCodeID = ric.ItemCodeID
	INNER JOIN Sam3_ItemCodeSteelgo ics on ric.ItemCodeSteelgoID = ics.ItemCodeSteelgoID
	INNER JOIN Sam3_Cliente c on avll.ClienteID = c.ClienteID
	INNER JOIN Sam3_Proyecto p on nu.ProyectoID = p.ProyectoID
	INNER JOIN Sam3_ProyectoConfiguracion pc on nu.ProyectoID = pc.ProyectoID
	INNER JOIN Sam3_ProyectoConsecutivo pcn on nu.ProyectoID = pcn.ProyectoID






GO


