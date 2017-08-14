
CREATE TABLE Sam3_Despacho(
	[DespachoID] [int] Primary key IDENTITY(1,1) NOT NULL,
	[ProyectoID] [int] NOT NULL,
	[OrdenTrabajoSpoolID] [int] NOT NULL,
	[MaterialSpoolID] [int] NOT NULL,
	[NumeroUnicoID] [int] NOT NULL,
	[SalidaInventarioID] [int] NOT NULL,
	[Segmento] [nchar](1) NULL,
	[EsEquivalente] [bit] NOT NULL,
	[Cantidad] [int] NOT NULL,
	[Cancelado] [bit] NOT NULL,
	[FechaDespacho] [datetime] NOT NULL,
	[UsuarioModificacion] [INT] NULL,
	[FechaModificacion] [datetime] NULL,
	[Activo] [BIT] NOT NULL DEFAULT(1),
	[DespachadorID] [int] NULL,
	foreign key (ProyectoID) references Sam3_Proyecto(ProyectoID),
	foreign key (NumeroUnicoID) references Sam3_NumeroUnico(NumeroUnicoID),
	foreign key (SalidaInventarioID) references Sam3_NumeroUnicoMovimiento(NumeroUnicoMovimientoID)
)