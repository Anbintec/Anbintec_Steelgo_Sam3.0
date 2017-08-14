use SAM
/****** Object:  Index [IX_NumeroUnico_ProyectoID_NumeroUnicoID_ItemCodeID_ColadaID_Codigo]    Script Date: 24/10/2016 08:13:14 p. m. ******/
DROP INDEX [IX_NumeroUnico_ProyectoID_NumeroUnicoID_ItemCodeID_ColadaID_Codigo] ON [dbo].[NumeroUnico]
GO

/****** Object:  Index [IX_NumeroUnico_ProyectoID_NumeroUnicoID_ItemCodeID_ColadaID_Codigo]    Script Date: 24/10/2016 08:13:14 p. m. ******/
CREATE NONCLUSTERED INDEX [IX_NumeroUnico_ProyectoID_NumeroUnicoID_ItemCodeID_ColadaID_Codigo] ON [dbo].[NumeroUnico]
(
	[ProyectoID] ASC,
	[NumeroUnicoID] ASC,
	[ItemCodeID] ASC,
	[ColadaID] ASC,
	[Codigo] ASC
)
INCLUDE ( 	[ProveedorID],
	[FabricanteID],
	[TipoCorte1ID],
	[TipoCorte2ID],
	[Estatus],
	[PartidaFactura],
	[PartidaOrdenDeCompra],
	[Diametro1],
	[Diametro2],
	[Cedula],
	[MarcadoAsme],
	[MarcadoGolpe],
	[MarcadoPintura],
	[PruebasHidrostaticas]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO