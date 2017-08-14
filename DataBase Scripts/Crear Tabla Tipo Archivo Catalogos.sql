USE [steelgo-sam3]
GO

/****** Object:  Table [dbo].[Sam3_TipoArchivo_Catalogo]    Script Date: 10/2/2015 7:15:04 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Sam3_TipoArchivo_Catalogo](
	[TipoArchivoID] [int] IDENTITY(1,1) NOT NULL,
	[CatalogoID] [int] NULL,
	[Nombre] [varchar](100) NOT NULL,
	[Activo] [bit] NOT NULL,
	[FechaModificacion] [datetime] NULL,
	[UsuarioModificacion] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[TipoArchivoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[Sam3_TipoArchivo_Catalogo]  WITH CHECK ADD FOREIGN KEY([CatalogoID])
REFERENCES [dbo].[Sam3_Catalogos] ([CatalogoID])
GO

