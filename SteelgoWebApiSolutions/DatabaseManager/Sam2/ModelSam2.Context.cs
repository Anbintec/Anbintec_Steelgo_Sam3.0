﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DatabaseManager.Sam2
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class Sam2Context : DbContext
    {
        public Sam2Context()
            : base("name=Sam2Context")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Acero> Acero { get; set; }
        public virtual DbSet<AgrupadoresPND> AgrupadoresPND { get; set; }
        public virtual DbSet<AgrupadoresPorJunta> AgrupadoresPorJunta { get; set; }
        public virtual DbSet<AgrupadoresReparaciones> AgrupadoresReparaciones { get; set; }
        public virtual DbSet<AgrupadoresSoportes> AgrupadoresSoportes { get; set; }
        public virtual DbSet<AgrupadoresSpoolPND> AgrupadoresSpoolPND { get; set; }
        public virtual DbSet<aplicaPND> aplicaPND { get; set; }
        public virtual DbSet<aspnet_Applications> aspnet_Applications { get; set; }
        public virtual DbSet<aspnet_Membership> aspnet_Membership { get; set; }
        public virtual DbSet<aspnet_SchemaVersions> aspnet_SchemaVersions { get; set; }
        public virtual DbSet<aspnet_Users> aspnet_Users { get; set; }
        public virtual DbSet<BastonSpool> BastonSpool { get; set; }
        public virtual DbSet<BastonSpoolJunta> BastonSpoolJunta { get; set; }
        public virtual DbSet<BuscaArchivo> BuscaArchivo { get; set; }
        public virtual DbSet<CampoSeguimientoJunta> CampoSeguimientoJunta { get; set; }
        public virtual DbSet<CampoSeguimientoSpool> CampoSeguimientoSpool { get; set; }
        public virtual DbSet<CategoriaPendiente> CategoriaPendiente { get; set; }
        public virtual DbSet<Cedula> Cedula { get; set; }
        public virtual DbSet<Cliente> Cliente { get; set; }
        public virtual DbSet<Colada> Colada { get; set; }
        public virtual DbSet<ColadaMTR> ColadaMTR { get; set; }
        public virtual DbSet<Color> Color { get; set; }
        public virtual DbSet<ConceptoEstimacion> ConceptoEstimacion { get; set; }
        public virtual DbSet<ConfiguracionArmadoMaterial> ConfiguracionArmadoMaterial { get; set; }
        public virtual DbSet<ConfiguracionDestajoArmado> ConfiguracionDestajoArmado { get; set; }
        public virtual DbSet<ConfiguracionDestajoSoldadorRaiz> ConfiguracionDestajoSoldadorRaiz { get; set; }
        public virtual DbSet<ConfiguracionDestajoSoldadorRelleno> ConfiguracionDestajoSoldadorRelleno { get; set; }
        public virtual DbSet<CongeladoParcial> CongeladoParcial { get; set; }
        public virtual DbSet<Consumible> Consumible { get; set; }
        public virtual DbSet<Contacto> Contacto { get; set; }
        public virtual DbSet<ContactoCliente> ContactoCliente { get; set; }
        public virtual DbSet<Cortador> Cortador { get; set; }
        public virtual DbSet<Corte> Corte { get; set; }
        public virtual DbSet<CorteDetalle> CorteDetalle { get; set; }
        public virtual DbSet<CorteSpool> CorteSpool { get; set; }
        public virtual DbSet<CorteSpoolDeleted> CorteSpoolDeleted { get; set; }
        public virtual DbSet<CorteSpoolHistorico> CorteSpoolHistorico { get; set; }
        public virtual DbSet<CorteSpoolPendiente> CorteSpoolPendiente { get; set; }
        public virtual DbSet<CostoArmado> CostoArmado { get; set; }
        public virtual DbSet<CostoProcesoRaiz> CostoProcesoRaiz { get; set; }
        public virtual DbSet<CostoProcesoRelleno> CostoProcesoRelleno { get; set; }
        public virtual DbSet<Cuadrante> Cuadrante { get; set; }
        public virtual DbSet<CuadranteHistorico> CuadranteHistorico { get; set; }
        public virtual DbSet<Defecto> Defecto { get; set; }
        public virtual DbSet<Despachador> Despachador { get; set; }
        public virtual DbSet<Despacho> Despacho { get; set; }
        public virtual DbSet<DestajoSoldador> DestajoSoldador { get; set; }
        public virtual DbSet<DestajoSoldadorDetalle> DestajoSoldadorDetalle { get; set; }
        public virtual DbSet<DestajoSoldadorRellenadorFaltante> DestajoSoldadorRellenadorFaltante { get; set; }
        public virtual DbSet<DestajoSoldaduraRellenador> DestajoSoldaduraRellenador { get; set; }
        public virtual DbSet<DestajoTubero> DestajoTubero { get; set; }
        public virtual DbSet<DestajoTuberoDetalle> DestajoTuberoDetalle { get; set; }
        public virtual DbSet<Destino> Destino { get; set; }
        public virtual DbSet<DetallePersonalizacionSeguimientoJunta> DetallePersonalizacionSeguimientoJunta { get; set; }
        public virtual DbSet<DetallePersonalizacionSeguimientoSpool> DetallePersonalizacionSeguimientoSpool { get; set; }
        public virtual DbSet<Diametro> Diametro { get; set; }
        public virtual DbSet<DocIsoLog> DocIsoLog { get; set; }
        public virtual DbSet<DTSErrorLog> DTSErrorLog { get; set; }
        public virtual DbSet<DTSExecLog> DTSExecLog { get; set; }
        public virtual DbSet<DTSSummaryLog> DTSSummaryLog { get; set; }
        public virtual DbSet<DupontSpool> DupontSpool { get; set; }
        public virtual DbSet<Embarque> Embarque { get; set; }
        public virtual DbSet<EmbarqueInventario> EmbarqueInventario { get; set; }
        public virtual DbSet<EmbarqueSpool> EmbarqueSpool { get; set; }
        public virtual DbSet<Espesor> Espesor { get; set; }
        public virtual DbSet<EspesorComercial> EspesorComercial { get; set; }
        public virtual DbSet<Estacion> Estacion { get; set; }
        public virtual DbSet<EstatusOrden> EstatusOrden { get; set; }
        public virtual DbSet<Estimacion> Estimacion { get; set; }
        public virtual DbSet<EstimacionJunta> EstimacionJunta { get; set; }
        public virtual DbSet<EstimacionSpool> EstimacionSpool { get; set; }
        public virtual DbSet<FabArea> FabArea { get; set; }
        public virtual DbSet<Fabricante> Fabricante { get; set; }
        public virtual DbSet<FabricanteProyecto> FabricanteProyecto { get; set; }
        public virtual DbSet<FactorDiametro> FactorDiametro { get; set; }
        public virtual DbSet<FactorMaterial> FactorMaterial { get; set; }
        public virtual DbSet<FactorTipoJunta> FactorTipoJunta { get; set; }
        public virtual DbSet<FamiliaAcero> FamiliaAcero { get; set; }
        public virtual DbSet<FamiliaMaterial> FamiliaMaterial { get; set; }
        public virtual DbSet<FrentePintura> FrentePintura { get; set; }
        public virtual DbSet<HistoricoWorkstatus> HistoricoWorkstatus { get; set; }
        public virtual DbSet<InspeccionDimensionalPatio> InspeccionDimensionalPatio { get; set; }
        public virtual DbSet<InspeccionVisual> InspeccionVisual { get; set; }
        public virtual DbSet<InspeccionVisualCampo> InspeccionVisualCampo { get; set; }
        public virtual DbSet<InspeccionVisualPatio> InspeccionVisualPatio { get; set; }
        public virtual DbSet<InspeccionVisualPatioDefecto> InspeccionVisualPatioDefecto { get; set; }
        public virtual DbSet<Inspector> Inspector { get; set; }
        public virtual DbSet<Isolog> Isolog { get; set; }
        public virtual DbSet<IsoPrioridadEtileno> IsoPrioridadEtileno { get; set; }
        public virtual DbSet<ItemCode> ItemCode { get; set; }
        public virtual DbSet<ItemCodeEquivalente> ItemCodeEquivalente { get; set; }
        public virtual DbSet<JuntaArmado> JuntaArmado { get; set; }
        public virtual DbSet<JuntaArmadoPagado> JuntaArmadoPagado { get; set; }
        public virtual DbSet<JuntaCampo> JuntaCampo { get; set; }
        public virtual DbSet<JuntaCampoArmado> JuntaCampoArmado { get; set; }
        public virtual DbSet<JuntaCampoInspeccionVisual> JuntaCampoInspeccionVisual { get; set; }
        public virtual DbSet<JuntaCampoInspeccionVisualDefecto> JuntaCampoInspeccionVisualDefecto { get; set; }
        public virtual DbSet<JuntaCampoReportePND> JuntaCampoReportePND { get; set; }
        public virtual DbSet<JuntaCampoReportePNDCuadrante> JuntaCampoReportePNDCuadrante { get; set; }
        public virtual DbSet<JuntaCampoReportePNDSector> JuntaCampoReportePNDSector { get; set; }
        public virtual DbSet<JuntaCampoReporteTT> JuntaCampoReporteTT { get; set; }
        public virtual DbSet<JuntaCampoRequisicion> JuntaCampoRequisicion { get; set; }
        public virtual DbSet<JuntaCampoSoldadura> JuntaCampoSoldadura { get; set; }
        public virtual DbSet<JuntaCampoSoldaduraDetalle> JuntaCampoSoldaduraDetalle { get; set; }
        public virtual DbSet<JuntaInspeccionVisual> JuntaInspeccionVisual { get; set; }
        public virtual DbSet<JuntaInspeccionVisualDefecto> JuntaInspeccionVisualDefecto { get; set; }
        public virtual DbSet<JuntaPruebaDig> JuntaPruebaDig { get; set; }
        public virtual DbSet<JuntaReporteDiarioSoldadura> JuntaReporteDiarioSoldadura { get; set; }
        public virtual DbSet<JuntaReportePnd> JuntaReportePnd { get; set; }
        public virtual DbSet<JuntaReportePndCuadrante> JuntaReportePndCuadrante { get; set; }
        public virtual DbSet<JuntaReportePndSector> JuntaReportePndSector { get; set; }
        public virtual DbSet<JuntaReporteTt> JuntaReporteTt { get; set; }
        public virtual DbSet<JuntaRequisicion> JuntaRequisicion { get; set; }
        public virtual DbSet<JuntaSoldadura> JuntaSoldadura { get; set; }
        public virtual DbSet<JuntaSoldaduraDetalle> JuntaSoldaduraDetalle { get; set; }
        public virtual DbSet<JuntaSoldaduraPagado> JuntaSoldaduraPagado { get; set; }
        public virtual DbSet<JuntaSpool> JuntaSpool { get; set; }
        public virtual DbSet<JuntaSpoolDeleted> JuntaSpoolDeleted { get; set; }
        public virtual DbSet<JuntaSpoolHistorico> JuntaSpoolHistorico { get; set; }
        public virtual DbSet<JuntaSpoolPendiente> JuntaSpoolPendiente { get; set; }
        public virtual DbSet<JuntaWorkstatus> JuntaWorkstatus { get; set; }
        public virtual DbSet<KgTeorico> KgTeorico { get; set; }
        public virtual DbSet<LotePintura> LotePintura { get; set; }
        public virtual DbSet<Maquina> Maquina { get; set; }
        public virtual DbSet<MaterialSpool> MaterialSpool { get; set; }
        public virtual DbSet<MaterialSpoolDeleted> MaterialSpoolDeleted { get; set; }
        public virtual DbSet<MaterialSpoolHistorico> MaterialSpoolHistorico { get; set; }
        public virtual DbSet<MaterialSpoolPendiente> MaterialSpoolPendiente { get; set; }
        public virtual DbSet<MivDragados> MivDragados { get; set; }
        public virtual DbSet<Modulo> Modulo { get; set; }
        public virtual DbSet<ModuloSeguimientoJunta> ModuloSeguimientoJunta { get; set; }
        public virtual DbSet<ModuloSeguimientoSpool> ModuloSeguimientoSpool { get; set; }
        public virtual DbSet<NumeroUnico> NumeroUnico { get; set; }
        public virtual DbSet<NumeroUnicoCorte> NumeroUnicoCorte { get; set; }
        public virtual DbSet<NumeroUnicoInventario> NumeroUnicoInventario { get; set; }
        public virtual DbSet<NumeroUnicoMovimiento> NumeroUnicoMovimiento { get; set; }
        public virtual DbSet<NumeroUnicoSegmento> NumeroUnicoSegmento { get; set; }
        public virtual DbSet<OptEntregaOrdenesTrabajo> OptEntregaOrdenesTrabajo { get; set; }
        public virtual DbSet<OptInfoArmadoSoldadura> OptInfoArmadoSoldadura { get; set; }
        public virtual DbSet<OptProgramacionOT> OptProgramacionOT { get; set; }
        public virtual DbSet<OptReporteFaltantes> OptReporteFaltantes { get; set; }
        public virtual DbSet<OptReportes> OptReportes { get; set; }
        public virtual DbSet<OrdenTrabajo> OrdenTrabajo { get; set; }
        public virtual DbSet<OrdenTrabajoDetallesEntrega> OrdenTrabajoDetallesEntrega { get; set; }
        public virtual DbSet<OrdenTrabajoFechaPrevSoldadura> OrdenTrabajoFechaPrevSoldadura { get; set; }
        public virtual DbSet<OrdenTrabajoJunta> OrdenTrabajoJunta { get; set; }
        public virtual DbSet<OrdenTrabajoMaterial> OrdenTrabajoMaterial { get; set; }
        public virtual DbSet<OrdenTrabajoSpool> OrdenTrabajoSpool { get; set; }
        public virtual DbSet<Pagina> Pagina { get; set; }
        public virtual DbSet<PathOT> PathOT { get; set; }
        public virtual DbSet<Patio> Patio { get; set; }
        public virtual DbSet<Pendiente> Pendiente { get; set; }
        public virtual DbSet<PendienteDetalle> PendienteDetalle { get; set; }
        public virtual DbSet<Peq> Peq { get; set; }
        public virtual DbSet<Perfil> Perfil { get; set; }
        public virtual DbSet<PerfilPermiso> PerfilPermiso { get; set; }
        public virtual DbSet<PeriodoDestajo> PeriodoDestajo { get; set; }
        public virtual DbSet<PeriodoPrograma> PeriodoPrograma { get; set; }
        public virtual DbSet<Permiso> Permiso { get; set; }
        public virtual DbSet<PersonalizacionSeguimientoJunta> PersonalizacionSeguimientoJunta { get; set; }
        public virtual DbSet<PersonalizacionSeguimientoSpool> PersonalizacionSeguimientoSpool { get; set; }
        public virtual DbSet<PinturaAvance> PinturaAvance { get; set; }
        public virtual DbSet<PinturaCondicionesAmbientales> PinturaCondicionesAmbientales { get; set; }
        public virtual DbSet<PinturaNumeroUnico> PinturaNumeroUnico { get; set; }
        public virtual DbSet<PinturaReporteInspeccionDetalle> PinturaReporteInspeccionDetalle { get; set; }
        public virtual DbSet<PinturaSpool> PinturaSpool { get; set; }
        public virtual DbSet<PreList> PreList { get; set; }
        public virtual DbSet<PrioridadDragados> PrioridadDragados { get; set; }
        public virtual DbSet<ProcesoPinturaDetalle> ProcesoPinturaDetalle { get; set; }
        public virtual DbSet<ProcesoRaiz> ProcesoRaiz { get; set; }
        public virtual DbSet<ProcesoRelleno> ProcesoRelleno { get; set; }
        public virtual DbSet<Proveedor> Proveedor { get; set; }
        public virtual DbSet<ProveedorPintura> ProveedorPintura { get; set; }
        public virtual DbSet<ProveedorProyecto> ProveedorProyecto { get; set; }
        public virtual DbSet<Proyecto> Proyecto { get; set; }
        public virtual DbSet<ProyectoCamposRecepcion> ProyectoCamposRecepcion { get; set; }
        public virtual DbSet<ProyectoConfiguracion> ProyectoConfiguracion { get; set; }
        public virtual DbSet<ProyectoConsecutivo> ProyectoConsecutivo { get; set; }
        public virtual DbSet<ProyectoDossier> ProyectoDossier { get; set; }
        public virtual DbSet<ProyectoNomenclaturaSpool> ProyectoNomenclaturaSpool { get; set; }
        public virtual DbSet<ProyectoPendiente> ProyectoPendiente { get; set; }
        public virtual DbSet<ProyectoPrograma> ProyectoPrograma { get; set; }
        public virtual DbSet<ProyectoReporte> ProyectoReporte { get; set; }
        public virtual DbSet<Recepcion> Recepcion { get; set; }
        public virtual DbSet<RecepcionNumeroUnico> RecepcionNumeroUnico { get; set; }
        public virtual DbSet<ReporteCampoPND> ReporteCampoPND { get; set; }
        public virtual DbSet<ReporteCampoTT> ReporteCampoTT { get; set; }
        public virtual DbSet<ReporteDiarioSoldadura> ReporteDiarioSoldadura { get; set; }
        public virtual DbSet<ReporteDimensional> ReporteDimensional { get; set; }
        public virtual DbSet<ReporteDimensionalDetalle> ReporteDimensionalDetalle { get; set; }
        public virtual DbSet<ReporteDimensionalDetalleDeleted> ReporteDimensionalDetalleDeleted { get; set; }
        public virtual DbSet<ReportePnd> ReportePnd { get; set; }
        public virtual DbSet<ReporteSpoolPnd> ReporteSpoolPnd { get; set; }
        public virtual DbSet<ReporteTt> ReporteTt { get; set; }
        public virtual DbSet<Requisicion> Requisicion { get; set; }
        public virtual DbSet<RequisicionCampo> RequisicionCampo { get; set; }
        public virtual DbSet<RequisicionNumeroUnico> RequisicionNumeroUnico { get; set; }
        public virtual DbSet<RequisicionNumeroUnicoDetalle> RequisicionNumeroUnicoDetalle { get; set; }
        public virtual DbSet<RequisicionPintura> RequisicionPintura { get; set; }
        public virtual DbSet<RequisicionPinturaDetalle> RequisicionPinturaDetalle { get; set; }
        public virtual DbSet<RequisicionSpool> RequisicionSpool { get; set; }
        public virtual DbSet<Reserva> Reserva { get; set; }
        public virtual DbSet<ReservaItemcode> ReservaItemcode { get; set; }
        public virtual DbSet<ResultadoPND> ResultadoPND { get; set; }
        public virtual DbSet<RSF_Settings> RSF_Settings { get; set; }
        public virtual DbSet<SAC_Temp_area> SAC_Temp_area { get; set; }
        public virtual DbSet<SAC_Temp_BHTecnip> SAC_Temp_BHTecnip { get; set; }
        public virtual DbSet<SAC_Temp_BHTInventario> SAC_Temp_BHTInventario { get; set; }
        public virtual DbSet<SAC_Temp_WPS> SAC_Temp_WPS { get; set; }
        public virtual DbSet<SistemaPintura> SistemaPintura { get; set; }
        public virtual DbSet<SistemaPinturaConfig> SistemaPinturaConfig { get; set; }
        public virtual DbSet<Soldador> Soldador { get; set; }
        public virtual DbSet<Spool> Spool { get; set; }
        public virtual DbSet<SpoolDimensionPintura> SpoolDimensionPintura { get; set; }
        public virtual DbSet<SpoolHistorico> SpoolHistorico { get; set; }
        public virtual DbSet<SpoolHold> SpoolHold { get; set; }
        public virtual DbSet<SpoolHoldHistorial> SpoolHoldHistorial { get; set; }
        public virtual DbSet<SpoolHoldUpdate> SpoolHoldUpdate { get; set; }
        public virtual DbSet<SpoolPendiente> SpoolPendiente { get; set; }
        public virtual DbSet<SpoolReportePnd> SpoolReportePnd { get; set; }
        public virtual DbSet<SpoolRequisicion> SpoolRequisicion { get; set; }
        public virtual DbSet<SpoolsDeleted> SpoolsDeleted { get; set; }
        public virtual DbSet<StatusEmbarque> StatusEmbarque { get; set; }
        public virtual DbSet<Subparte_Dragados> Subparte_Dragados { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<Taller> Taller { get; set; }
        public virtual DbSet<TecnicaSoldador> TecnicaSoldador { get; set; }
        public virtual DbSet<TempCodigo> TempCodigo { get; set; }
        public virtual DbSet<TipoCorte> TipoCorte { get; set; }
        public virtual DbSet<TipoJunta> TipoJunta { get; set; }
        public virtual DbSet<TipoMaterial> TipoMaterial { get; set; }
        public virtual DbSet<TipoMovimiento> TipoMovimiento { get; set; }
        public virtual DbSet<TipoPendiente> TipoPendiente { get; set; }
        public virtual DbSet<TipoPrueba> TipoPrueba { get; set; }
        public virtual DbSet<TipoPruebaSpool> TipoPruebaSpool { get; set; }
        public virtual DbSet<TipoRechazo> TipoRechazo { get; set; }
        public virtual DbSet<TipoReporteDimensional> TipoReporteDimensional { get; set; }
        public virtual DbSet<TipoReporteProyecto> TipoReporteProyecto { get; set; }
        public virtual DbSet<Transferencia> Transferencia { get; set; }
        public virtual DbSet<TransferenciaSpool> TransferenciaSpool { get; set; }
        public virtual DbSet<Transportista> Transportista { get; set; }
        public virtual DbSet<TransportistaProyecto> TransportistaProyecto { get; set; }
        public virtual DbSet<Tubero> Tubero { get; set; }
        public virtual DbSet<UbicacionFisica> UbicacionFisica { get; set; }
        public virtual DbSet<UltimoProceso> UltimoProceso { get; set; }
        public virtual DbSet<UrlProyecto> UrlProyecto { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }
        public virtual DbSet<UsuarioProyecto> UsuarioProyecto { get; set; }
        public virtual DbSet<WorkstatusSpool> WorkstatusSpool { get; set; }
        public virtual DbSet<Wpq> Wpq { get; set; }
        public virtual DbSet<Wps> Wps { get; set; }
        public virtual DbSet<WpsProyecto> WpsProyecto { get; set; }
        public virtual DbSet<BI_Join_MaterialSeguimiento> BI_Join_MaterialSeguimiento { get; set; }
        public virtual DbSet<DestajoArmado> DestajoArmado { get; set; }
        public virtual DbSet<FactorCedula> FactorCedula { get; set; }
        public virtual DbSet<FechasDestajo> FechasDestajo { get; set; }
        public virtual DbSet<JuntaInspeccionDig> JuntaInspeccionDig { get; set; }
        public virtual DbSet<PinturaReporteInspeccion> PinturaReporteInspeccion { get; set; }
        public virtual DbSet<ProcesoPintura> ProcesoPintura { get; set; }
        public virtual DbSet<RespaldoTransferencias> RespaldoTransferencias { get; set; }
        public virtual DbSet<RespaldoTransferenciasSpool> RespaldoTransferenciasSpool { get; set; }
        public virtual DbSet<RevisadosAsignacionMateriales> RevisadosAsignacionMateriales { get; set; }
        public virtual DbSet<rid_dragados> rid_dragados { get; set; }
        public virtual DbSet<TempCargaMasiva> TempCargaMasiva { get; set; }
        public virtual DbSet<tempDespachos20150205> tempDespachos20150205 { get; set; }
        public virtual DbSet<tempMovimientosInventario> tempMovimientosInventario { get; set; }
        public virtual DbSet<TempNumerosTransferencias> TempNumerosTransferencias { get; set; }
        public virtual DbSet<TempNumeroUnico> TempNumeroUnico { get; set; }
        public virtual DbSet<tempNumeroUnicoInventario0210> tempNumeroUnicoInventario0210 { get; set; }
        public virtual DbSet<tempNumeroUnicoInventarioRespaldo> tempNumeroUnicoInventarioRespaldo { get; set; }
        public virtual DbSet<tempNumeroUnicoMovimiento0210> tempNumeroUnicoMovimiento0210 { get; set; }
        public virtual DbSet<tempNumeroUnicoSegmento0210> tempNumeroUnicoSegmento0210 { get; set; }
        public virtual DbSet<TempSpoolCargaJuntasPND> TempSpoolCargaJuntasPND { get; set; }
        public virtual DbSet<TempTransferenciaModificado> TempTransferenciaModificado { get; set; }
        public virtual DbSet<TempTransferenciaSpoolModificado> TempTransferenciaSpoolModificado { get; set; }
        public virtual DbSet<tempWksSpool> tempWksSpool { get; set; }
        public virtual DbSet<TipoFabClass> TipoFabClass { get; set; }
        public virtual DbSet<TipoFabLine> TipoFabLine { get; set; }
        public virtual DbSet<TMP_UpdateSpool> TMP_UpdateSpool { get; set; }
        public virtual DbSet<TMP_UpdateSpoolID> TMP_UpdateSpoolID { get; set; }
    }
}
