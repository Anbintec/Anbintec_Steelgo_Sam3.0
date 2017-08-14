USE [steelgo-sam3]
GO

/****** Object:  Table [dbo].[Sam3_Rel_Catalogos_Documento]    Script Date: 10/2/2015 7:15:25 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Sam3_Rel_Catalogos_Documento](
	[Rel_Catalogos_DocumentoID] [int] IDENTITY(1,1) NOT NULL,
	[CatalogoID] [int] NULL,
	[TipoArchivoID] [int] NULL,
	[DocumentoID] [int] NOT NULL,
	[Nombre] [varchar](200) NOT NULL,
	[Extension] [varchar](5) NOT NULL,
	[DocGuid] [uniqueidentifier] NULL,
	[Url] [varchar](max) NULL,
	[ContentType] [varchar](50) NULL,
	[Activo] [bit] NOT NULL,
	[UsuarioModificacion] [int] NULL,
	[FechaModificacion] [datetime] NULL,
	[ElementoCatalogoID] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Rel_Catalogos_DocumentoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[Sam3_Rel_Catalogos_Documento]  WITH CHECK ADD FOREIGN KEY([CatalogoID])
REFERENCES [dbo].[Sam3_Catalogos] ([CatalogoID])
GO

ALTER TABLE [dbo].[Sam3_Rel_Catalogos_Documento]  WITH CHECK ADD FOREIGN KEY([TipoArchivoID])
REFERENCES [dbo].[Sam3_TipoArchivo_Catalogo] ([TipoArchivoID])
GO

