USE [steelgo-sam3]
GO

/****** Object:  Table [dbo].[Sam3_ItemCodeSteelgo]    Script Date: 10/2/2015 7:14:38 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Sam3_ItemCodeSteelgo](
	[ItemCodeSteelgoID] [int] IDENTITY(1,1) NOT NULL,
	[DescripcionEspanol] [nvarchar](max) NOT NULL,
	[DescripcionIngles] [nvarchar](max) NULL,
	[Peso] [decimal](7, 3) NOT NULL,
	[Diametro1] [decimal](7, 4) NOT NULL,
	[Diametro2] [decimal](7, 4) NOT NULL,
	[FamiliaAceroID] [int] NOT NULL,
	[Activo] [bit] NOT NULL,
	[UsuarioModificacion] [int] NULL,
	[FechaModificacion] [datetime] NULL,
	[Area] [int] NOT NULL,
	[CedulaID] [int] NULL,
	[Codigo] [varchar](100) NOT NULL,
	[GrupoID] [int] NULL,
	[DescripcionLargaEspanol] [varchar](250) NULL,
	[DescripcionLargaIngles] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ItemCodeSteelgoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[Sam3_ItemCodeSteelgo]  WITH CHECK ADD FOREIGN KEY([CedulaID])
REFERENCES [dbo].[Sam3_Cedula] ([CedulaID])
GO

ALTER TABLE [dbo].[Sam3_ItemCodeSteelgo]  WITH CHECK ADD FOREIGN KEY([GrupoID])
REFERENCES [dbo].[Sam3_Grupo] ([GrupoID])
GO

