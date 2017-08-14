USE [steelgo-sam3]
GO

/****** Object:  View [dbo].[VW_AvisoEntrada]    Script Date: 12/9/2015 11:35:59 AM ******/
DROP VIEW [dbo].[VW_AvisoEntrada]
GO

/****** Object:  View [dbo].[VW_AvisoEntrada]    Script Date: 12/9/2015 11:35:59 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



--AViso de entrada
CREATE VIEW [dbo].[VW_AvisoEntrada] AS
SELECT ave.FolioAvisoEntradaID, 
	avll.FolioAvisoLlegadaID, 
	cli.Nombre as Cliente, 
	p.Nombre as Proyecto, 
	ave.FechaCreacion,
	pa.PatioID as Patio
FROM Sam3_FolioAvisoEntrada ave
INNER JOIN Sam3_FolioAvisoLlegada avll on ave.FolioAvisoLlegadaID = avll.FolioAvisoLlegadaID
INNER JOIN Sam3_Rel_FolioAvisoLlegada_Proyecto avllp on avll.FolioAvisoLlegadaID = avllp.FolioAvisoLlegadaID
INNER JOIN Sam3_Proyecto p on avllp.ProyectoID = p.ProyectoID
INNER JOIN Sam3_Patio pa on p.PatioID = pa.PatioID
LEFT JOIN Sam3_Cliente cli on avll.ClienteID = cli.ClienteID


GO


