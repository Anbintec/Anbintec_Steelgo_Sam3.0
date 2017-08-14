USE [steelgo-sam3]
GO

/****** Object:  View [dbo].[VW_FolioCuantificacion]    Script Date: 12/9/2015 11:42:11 AM ******/
DROP VIEW [dbo].[VW_FolioCuantificacion]
GO

/****** Object:  View [dbo].[VW_FolioCuantificacion]    Script Date: 12/9/2015 11:42:11 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



--Folio Cuantificacion
CREATE VIEW [dbo].[VW_FolioCuantificacion] AS
SELECT fc.FolioCuantificacionID, 
p.Nombre as Proyecto, 
ave.FolioAvisoEntradaID, 
avll.FolioAvisoLlegadaID, 
cli.Nombre as Cliente, 
fc.FechaCreacion,
pa.PatioID as Patio
FROM Sam3_FolioCuantificacion fc
INNER JOIN Sam3_Proyecto p on fc.ProyectoID = p.ProyectoID
INNER JOIN Sam3_FolioAvisoEntrada ave on fc.FolioAvisoEntradaID = ave.FolioAvisoEntradaID
INNER JOIN Sam3_FolioAvisoLlegada avll on ave.FolioAvisoLlegadaID = avll.FolioAvisoLlegadaID
INNER JOIN Sam3_Cliente cli on avll.ClienteID = cli.ClienteID
INNER JOIN Sam3_Patio pa on p.PatioID = pa.PatioID


GO


