USE [steelgo-sam3]
GO

EXEC sys.sp_dropextendedproperty @name=N'MS_DiagramPaneCount' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'VW_ItemCodes'

GO

EXEC sys.sp_dropextendedproperty @name=N'MS_DiagramPane2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'VW_ItemCodes'

GO

EXEC sys.sp_dropextendedproperty @name=N'MS_DiagramPane1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'VW_ItemCodes'

GO

/****** Object:  View [dbo].[VW_ItemCodes]    Script Date: 12/9/2015 11:43:25 AM ******/
DROP VIEW [dbo].[VW_ItemCodes]
GO

/****** Object:  View [dbo].[VW_ItemCodes]    Script Date: 12/9/2015 11:43:25 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*Item Code*/
CREATE VIEW [dbo].[VW_ItemCodes]
AS

SELECT DISTINCT 
               ic.ItemCodeID, 
			   p.Nombre AS Proyecto, 
			   p.PatioID AS Patio, 
			   ic.Codigo AS ItemCodeCodigo, 
			   ics.ItemCodeSteelgoID, 
			   ics.Codigo AS ItemCodeSteelgoCodigo, 
               fc.FolioCuantificacionID, 
			   ave.FolioAvisoLlegadaID, 
			   fc.FolioAvisoEntradaID, 
			   c.Nombre AS Cliente, 
			   ic.FechaModificacion
			   FROM  dbo.Sam3_ItemCode AS ic
			   INNER JOIN Sam3_Rel_ItemCode_Diametro icd on ic.ItemCodeID = icd.ItemCodeID  
			   INNER JOIN dbo.Sam3_Rel_FolioCuantificacion_ItemCode AS rfc ON icd.Rel_ItemCode_Diametro_ID = rfc.Rel_ItemCode_Diametro_ID 
			   INNER JOIN dbo.Sam3_FolioCuantificacion AS fc ON rfc.FolioCuantificacionID = fc.FolioCuantificacionID 
			   INNER JOIN dbo.Sam3_FolioAvisoEntrada AS ave ON fc.FolioAvisoEntradaID = ave.FolioAvisoEntradaID 
			   INNER JOIN dbo.Sam3_FolioAvisoLlegada AS avll ON ave.FolioAvisoLlegadaID = avll.FolioAvisoLlegadaID 
			   INNER JOIN dbo.Sam3_Rel_ItemCode_ItemCodeSteelgo AS ric ON ic.ItemCodeID = ric.ItemCodeID 
			   INNER JOIN dbo.Sam3_ItemCodeSteelgo AS ics ON ric.ItemCodeSteelgoID = ics.ItemCodeSteelgoID 
			   INNER JOIN Sam3_Cliente AS c ON avll.ClienteID = c.ClienteID 
			   INNER JOIN dbo.Sam3_Proyecto AS p ON ic.ProyectoID = p.ProyectoID
UNION
SELECT DISTINCT 
               ic.ItemCodeID, 
			   p.Nombre AS Proyecto, 
			   p.PatioID AS Patio, 
			   ic.Codigo AS ItemCodeCodigo, 
			   ics.ItemCodeSteelgoID, 
			   ics.Codigo AS ItemCodeSteelgoCodigo, 
               fc.FolioCuantificacionID, 
			   ave.FolioAvisoLlegadaID, 
			   fc.FolioAvisoEntradaID, 
			   c.Nombre AS Cliente, 
			   ic.FechaModificacion
               FROM  dbo.Sam3_ItemCode AS ic
			   INNER JOIN Sam3_Rel_ItemCode_Diametro icd on ic.ItemCodeID = icd.ItemCodeID 
			   INNER JOIN dbo.Sam3_Rel_Bulto_ItemCode AS rb ON icd.Rel_ItemCode_Diametro_ID = rb.Rel_ItemCode_Diametro_ID 
			   INNER JOIN Sam3_Bulto b on rb.BultoID = b.BultoID
			   INNER JOIN dbo.Sam3_FolioCuantificacion AS fc ON b.FolioCuantificacionID  = fc.FolioCuantificacionID 
			   INNER JOIN dbo.Sam3_FolioAvisoEntrada AS ave ON fc.FolioAvisoEntradaID = ave.FolioAvisoEntradaID 
			   INNER JOIN dbo.Sam3_FolioAvisoLlegada AS avll ON ave.FolioAvisoLlegadaID = avll.FolioAvisoLlegadaID 
			   INNER JOIN dbo.Sam3_Rel_ItemCode_ItemCodeSteelgo AS ric ON ic.ItemCodeID = ric.ItemCodeID 
			   INNER JOIN dbo.Sam3_ItemCodeSteelgo AS ics ON ric.ItemCodeSteelgoID = ics.ItemCodeSteelgoID 
			   INNER JOIN Sam3_Cliente AS c ON avll.ClienteID = c.ClienteID 
			   INNER JOIN dbo.Sam3_Proyecto AS p ON ic.ProyectoID = p.ProyectoID


GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "ic"
            Begin Extent = 
               Top = 7
               Left = 48
               Bottom = 135
               Right = 301
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "rfc"
            Begin Extent = 
               Top = 140
               Left = 48
               Bottom = 268
               Right = 344
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "fc"
            Begin Extent = 
               Top = 273
               Left = 48
               Bottom = 401
               Right = 250
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "ave"
            Begin Extent = 
               Top = 406
               Left = 48
               Bottom = 534
               Right = 248
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "avll"
            Begin Extent = 
               Top = 539
               Left = 48
               Bottom = 667
               Right = 245
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "ric"
            Begin Extent = 
               Top = 672
               Left = 48
               Bottom = 800
               Right = 317
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "ics"
            Begin Extent = 
               Top = 805
               Left = 48
               Bottom = 933
               Right = 273
            End
            DisplayFlags = 280
            TopColumn = 0
   ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'VW_ItemCodes'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'      End
         Begin Table = "p"
            Begin Extent = 
               Top = 1071
               Left = 48
               Bottom = 1199
               Right = 245
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "c"
            Begin Extent = 
               Top = 199
               Left = 798
               Bottom = 327
               Right = 986
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'VW_ItemCodes'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'VW_ItemCodes'
GO


