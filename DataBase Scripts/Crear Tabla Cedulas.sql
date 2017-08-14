USE [steelgo-sam3]
GO

/****** Object:  Table [dbo].[Sam3_Cedula]    Script Date: 10/2/2015 7:14:12 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Sam3_Cedula](
	[CedulaID] [int] IDENTITY(1,1) NOT NULL,
	[Diametro] [decimal](7, 4) NULL,
	[CedulaA] [varchar](50) NULL,
	[CedulaB] [varchar](50) NULL,
	[CedulaC] [varchar](50) NULL,
	[CedulaIn] [decimal](7, 4) NOT NULL,
	[CedulaMM] [decimal](7, 4) NOT NULL,
	[Espesor] [decimal](7, 4) NOT NULL,
	[Activo] [bit] NOT NULL,
	[UsuarioModificacion] [int] NOT NULL,
	[FechaModificacion] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[CedulaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Espesor] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

