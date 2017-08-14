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
-- Create date: 04-03-2016
-- Description:	Reporte de pase de salida
-- =============================================
CREATE PROCEDURE Sam3_RPT_PaseSalidaTransportista
	-- Add the parameters for the stored procedure here
	@FolioAvisoLlegadaID int, @idUsuario int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

declare @i int, 
		@strPlacas varchar(max),
		@tieneIncidencias bit
		

	--set @FolioAvisoLlegadaID = 10339
	--set @idUsuario = 1
	set @i=1;
	set @strPlacas='';


	create table #Placas(
	idx int identity(1,1),
	Placas varchar(20)
	)

	insert into #Placas
	select Placas from Sam3_Rel_FolioAvisoLlegada_Vehiculo a
	inner join Sam3_Vehiculo b on a.VehiculoID=b.VehiculoID
	inner join Sam3_TipoVehiculo c on b.TipoVehiculoID=c.TipoVehiculoID
	where a.Activo=1 and b.Activo=1 and c.Activo=1
	and FolioAvisoLlegadaID=@FolioAvisoLlegadaID

	declare @FechaInicioDescarga varchar(12),@FechaFinDescarga varchar(12),
			@cantidadPackingFirmado int,  @cantidadPlanas int

	select @FechaInicioDescarga=isnull(LTRIM(RIGHT(CONVERT(CHAR(20), FechainicioDescarga, 22), 11)),''),
		   @FechaFinDescarga=isnull(LTRIM(RIGHT(CONVERT(CHAR(20), FechaFinDescarga, 22), 11)),'')
	from Sam3_FolioAvisoEntrada where FolioAvisoLlegadaID=@FolioAvisoLlegadaID

	set @cantidadPlanas=(select COUNT(*) from #Placas)

	create table #Plana(
		FolioAvisoLlegadaID int,
		Plana varchar(max),
		ValorPlana varchar(max)
	)

	while(@i<= @cantidadPlanas)
	begin
		declare @Plana varchar(max), @Placas varchar(max)
		set @Plana= @i --'Plana '+convert(varchar(max),@i);

		select @Placas=Placas from #Placas where idx=@i

		insert into #Plana values(@FolioAvisoLlegadaID, @Plana,@Placas )
		set @i+=1;
	end

	declare @string varchar(max)
	SET @string='';

	set @tieneIncidencias = (
		select case 
				when (
					select count(DocumentoID) 
					from Sam3_Rel_FolioAvisoLlegada_Documento
					where FolioAvisoLlegadaID = @FolioAvisoLlegadaID
					and TipoArchivoID = (
						select TipoArchivoID 
						from Sam3_TipoArchivo
						where Nombre = 'Incidencias firmadas'
					)
				) > 0 
				then
					1
				else
					0
				end
	)

	select  convert(varchar(10),getdate(),103) as Fecha,
			@FechaInicioDescarga  as FechaInicioDescarga,
			@FechaFinDescarga as FechaFinDescarga,
			 LTRIM(RIGHT(CONVERT(CHAR(20), FechaRecepcion, 22), 11)) AS [FechaLlegada],
			(select Nombre from Sam3_Chofer where ChoferID= a.ChoferID) as [NombreOperador],
			@tieneIncidencias as TieneIncidenciaFirmada,
			(
				select case @tieneIncidencias
					when 1 then(
						select 
							LTRIM(RTRIM(rel.PreFijoFolioIncidencias)) +
								REPLACE(
								STR(
									LTRIM(RTRIM(inc.Consecutivo)),
									CASE
									WHEN LEN(inc.Consecutivo) > rel.CantidadCerosFolioIncidencias 
									THEN LEN(inc.Consecutivo)
									ELSE rel.CantidadCerosFolioIncidencias
									END
								), SPACE(1), '0'
							) +
							LTRIM(RTRIM(rel.PostFijoFolioIncidencias)) as NumeroIncidencia
						from Sam3_Rel_Incidencia_FolioAvisoLlegada incidencia
						inner join Sam3_Incidencia inc on Incidencia.IncidenciaID = inc.IncidenciaID
						inner join Sam3_Rel_Proyecto_Entidad_Configuracion rel
							on inc.Rel_Proyecto_Entidad_Configuracion_ID = rel.Rel_Proyecto_Entidad_Configuracion_ID
						where Incidencia.FolioAvisoLlegadaID = a.FolioAvisoLlegadaID
						for xml auto
					)
					when 0 then ''
					end
			) as Incidencias, 
			p.Nombre as Proyecto,
			(select Placas from Sam3_Vehiculo where VehiculoID= a.VehiculoID) as Tracto, 
			fae.OrdenCompra as Pedimento, 
			fae.Factura as Factura, 
			'N/A'  as Remision,
			convert(varchar(max),@cantidadPlanas)  as [NumeroPlanas],
			(
				select 
					plana as 'id',
					ValorPlana
				from #Plana Planas
				where FolioAvisoLlegadaID = a.FolioAvisoLlegadaID
				for xml auto
			) as Planas,
			cl.Nombre,
			(
				select 
				LTRIM(RTRIM(relp.PreFijoFolioAvisoLlegada)) +
				REPLACE(
						STR(
							LTRIM(RTRIM(fa.Consecutivo)),
							CASE
								WHEN LEN(fa.Consecutivo) > relp.CantidadCerosFolioAvisoLlegada 
								THEN LEN(fa.Consecutivo)
								ELSE relp.CantidadCerosFolioAvisoLlegada
							END
						), SPACE(1), '0'
				) +
				LTRIM(RTRIM(relp.PostFijoFolioAvisoLlegada))
				from Sam3_FolioAvisoLlegada fa
				inner join Sam3_Rel_Proyecto_Entidad_Configuracion relp on fa.ProyectoNombrado = relp.Proyecto
				where fa.Entidad = relp.Entidad and fa.FolioAvisoLlegadaID = a.FolioAvisoLlegadaID
			) as NoAvisoEntrada,
			(
				select
					documento.Nombre
				from Sam3_Rel_FolioAvisoLlegada_Documento Documento
				where documento.FolioAvisoLlegadaID = a.FolioAvisoLlegadaID 
				for xml auto
			) as Documentos,
			(
				select 
					Nombre
				from Sam3_Usuario 
				where UsuarioID = @idUsuario 
			) as AutorizadoPor
	from Sam3_FolioAvisoLlegada a 
	inner join Sam3_Rel_FolioAvisoLlegada_Proyecto relp 
		on a.FolioAvisoLlegadaID=relp.FolioAvisoLlegadaID 
	inner join Sam3_Proyecto  p 
		on  relp.ProyectoID=p.ProyectoID 
	inner join Sam3_FolioAvisoEntrada  fae 
		on  fae.FolioAvisoLlegadaID=a.FolioAvisoLlegadaID 
	--left join #Plana pl
	--	on pl.FolioAvisoLlegadaID=a.FolioAvisoLlegadaID
	inner join Sam3_Cliente cl on a.ClienteID = cl.ClienteID
	where a.FolioAvisoLlegadaID=@FolioAvisoLlegadaID

	drop table #Placas
	drop table #Plana
END
GO
