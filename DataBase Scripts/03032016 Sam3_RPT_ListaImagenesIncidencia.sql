-- ================================================
-- Template generated from Template Explorer using:
-- Create Procedure (New Menu).SQL
--
-- Use the Specify Values for Template Parameters 
-- command (Ctrl-Shift-M) to fill in the parameter 
-- values below.
--
-- This block of comments will not be included in
-- the definition of the procedure.
-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Julian Hernandez
-- Create date: 03-03-2016
-- Description:	Imagenes relacionadas a una incidencia
-- =============================================
CREATE PROCEDURE Sam3_RPT_ListaImagenesIncidencia
	@incidenciaID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    select 
		Url,
		Descripcion,
		FechaModificacion
	from Sam3_Rel_Incidencia_Documento
	where IncidenciaID = @incidenciaID
	and ContentType = 'image/jpeg'
	order by FechaModificacion
END
GO
