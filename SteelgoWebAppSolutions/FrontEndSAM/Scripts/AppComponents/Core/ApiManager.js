/********************************************/
/********************************************/
/*********                          *********/
/*********    API Manager           *********/
/*********                          *********/
/********************************************/
/********************************************/

/****************************/
/*    Global Variables      */
/****************************/

//LOCALHOST BASE URL's

//var $BackEndSAMUri = 'http://10.15.2.12:8080';
//var $SecuritySAMUri = 'http://10.15.2.12:8081';
//var $MessagesSAMUri = 'http://10.15.2.12:8082';
//var $SearchSAMUri = 'http://192.168.1.7:60921';
//var $FilesSAMUri = 'http://192.168.1.7:60921';

//DEVELOPMENT Steelgo BASE URL's
//var $BackEndSAMUri = 'http://192.168.1.7:60960';
//var $SecuritySAMUri = 'http://192.168.1.7:60921';
//var $MessagesSAMUri = 'http://192.168.1.7:60921';
//var $SearchSAMUri = 'http://192.168.1.7:60921';
//var $FilesSAMUri = 'http://192.168.1.7:60921';

//Local host
var $BackEndSAMUri = 'http://localhost:60960';
var $SecuritySAMUri = 'http://localhost:60921';
var $MessagesSAMUri = 'http://localhost:60966';
var $SearchSAMUri = 'http://localhost:60921';
var $SearchSAMUri2 = 'http://10.15.10.185:8089';
var $FilesSAMUri = 'http://localhost:60921';

//Combos Base URL
var $DocumentoAvisoLlegadaUploadFiles = $BackEndSAMUri+"/backendsam/api/DocumentoAvisoLlegada?folioAvisoLlegadaID=";
var $DocumentoPermisoAduana = $BackEndSAMUri + "/backendsam/api/DocumentoPermisoAduana";
var $DocumentoPaseSalidaUploadFiles = $BackEndSAMUri + "/backendsam/api/DocumentoPaseSalida?folioAvisoLlegada=";
var $DocumentoLlegadaMaterialUploadFiles = $BackEndSAMUri + "/backendsam/api/DocumentoFolioAvisoEntrada?folioAvisoEntradaID=";
var $DocumentoIncidenciasUploadFiles = $BackEndSAMUri + "/backendsam/api/DocumentoIncidencia?incidenciaID=";
var $URLItemCode = $BackEndSAMUri + '/backendsam/api/ItemCode?';
var $URLColada = $BackEndSAMUri + '/backendsam/api/Colada?';
var $URLItemCodeSteelgo = $BackEndSAMUri + '/backendsam/api/ObtenerRelacionItemCodeSteelgo?';
var $UrlTipoUso = $BackEndSAMUri + '/backendsam/api/TipoUso?';
var $UrlDummyDespacho = $BackEndSAMUri + '/backendsam/api/DummyDespacho?';
var $UrlNumerosUnicos = $BackEndSAMUri + '/backendsam/api/NumerosUnicos?';
var $UrlNumerosUnicosDespacho = $BackEndSAMUri + '/backendsam/api/NumeroUnico?';
var $UrlImpresionDocumental = $BackEndSAMUri + '/backendsam/api/ImpresionDocumental?';
var $UrlNoPickingTicket = $BackEndSAMUri + '/backendsam/api/PickingTicket?';
var $UrlNoEmpleado= $BackEndSAMUri + '/backendsam/api/Entrega?';
var $UrlReportingServices = "http://mtysqldev-v01:8082/Reports_Sam3/";
var $UrlOrdenTrabajoSpool = $BackEndSAMUri + '/backendsam/api/OrdenTrabajoSpool?';
var $UrlNumerosUnicosPreDespacho = $BackEndSAMUri + '/backendsam/api/PreDespacho?';
var $DiametrosCatalogos = $BackEndSAMUri + '/backendsam/api/Diametros?';
var $UrlOrdenTrabajo = $BackEndSAMUri + '/backendsam/api/OrdenTrabajo?';
var $UrlMaterialSpool = $BackEndSAMUri + '/backendsam/api/MaterialSpool?';
var $CatalogoEspesor = $BackEndSAMUri + '/backendsam/api/Espesor?';
var $MTR = $BackEndSAMUri + '/backendsam/api/MTR?';
var $URLCorte = $BackEndSAMUri + '/backendsam/api/Corte?';

//Base API's
var $BackEndSAM = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
//var $FileManager = new $.RestClient($FilesSAMUri + '/filemanager/api/');
var $MessagesManager = new $.RestClient($MessagesSAMUri + '/messagesmanager/api/');
//var $SearchManager = new $.RestClient($SearchSAMUri + '/searchmanager/api/');
var $SecurityManager = new $.RestClient($SecuritySAMUri + '/securitymanager/api/');
//DetalleAvisoLlegada
var $Plana = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $Transportista = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $Patio = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $Proveedor = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $AvisoLlegada = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $Chofer = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $Proyecto = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $Camion = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $Contacto = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $folioEntradaMaterial = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $FiltrosListadoEntradaMaterial = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $FiltrosListadoLlegadaMaterial = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $TipoVehiculo = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $Cliente = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $TipoAviso = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $PermisoAutorizado = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $PermisoTramite = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $TipoArchivo = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $DocumentoAvisoLlegada = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $ValidacionFolioConPermisoAduana = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $DummyIncidencias = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $DocumentoPaseSalida = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $PaseSalida= new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $DocumentoFolioAvisoEntrada = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $FolioAvisoEntrada = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $OrdenDescarga = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $ObtenerDatosFolioAvisoEntrada = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $ObtenerDatosFolioLlegadaMaterial = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $TipoPackingList = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $TipoUso = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $ListadoCuantificacion = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $ValidarItemCodeNuCuantificacion = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $DummyObtenerColadasPorProyecto = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $Familia = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $ItemCode = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $ItemCodeSteelgo = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $Fabricantes = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $TipoAcero = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $DummyColada = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $DummyCambiarEstatusCuantificacion = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $DummyOrdenRecepcionFoliosAvisoEntrada = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $DummyOrdenRecepcionItemCode = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $DummyGridOrdenRecepcion = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $OrdenRecepcion = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $NumeroUnico = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $OrdenAlmacenaje = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $GenerarOrdenAlmacenaje = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $FoliosCuantificacion = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $CargarGridCuantificacion = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $Colada = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $GuardarFolioLlegadaCuantificacion = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $CambiarEstatusCuantificacion = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $ComplementarRecepcion = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $ListadoMateriales = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $Almacenaje = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $DetalleNumeroUnico = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $ListadoMaterialesPorItemCode = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $Despacho = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $Maquina = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $Cortador = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $NumerosUnicos = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $Catalogos = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $Corte = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $AsociacionICS = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $Cedulas = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $Clasificacion = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $TipoIncidencia = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $CatalogoICSteelgo = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $Incidencia = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $DocumentoIncidencia = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $DocumentosCatalogos = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $ImpresionDocumental = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $Grupo = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $MaterialPendiente = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $Entrega = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $NotificacionDeficit = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $Usuario = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $RevisionDeficit = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $IncidenciaPaseSalida = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $Busqueda = new $.RestClient($SearchSAMUri2 + '/solr/Steelgo_Search/');
var $PreDespacho = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $GuardarPermisoAduana = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $Diametros = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $ConvertirSpoolAGranel = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $Spool = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $FamiliaAcero = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $Proxy = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $Reporte = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $Espesor = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $ValidarCedulas = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $ComplementoRecepcionAlt = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $PickingTicket = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $UbicacionNumeroUnico = new $.RestClient($BackEndSAMUri + '/backendsam/api/');

//INICIA MATERIALES
var $PlanchadoMaterial = new $.RestClient($BackEndSAMUri + '/backendsam/api/');


//INICIA ORDEN DE COMPRA

var $Dynasol = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $PackingList = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $DynasolCamion = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $RecepcionOC = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $CompraPagoOC = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $CobroOC = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $PagoOC = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $CreacionOC = new $.RestClient($BackEndSAMUri + '/backendsam/api/');


//Steelgo - Maftec
var $Dashboard = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $Cuadrante = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $Zona = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $Patios = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $Periodo = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $Cedula = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $Acero = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $TipoJunta = new $.RestClient($BackEndSAMUri + '/backendsam/api/');

//Fabricación
var $Armado = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $Defectos = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $Obrero = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $InspeccionDimensional = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $InspeccionVisualDimensional = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $CapturasRapidas = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $Soldadura = new $.RestClient($BackEndSAMUri + '/backendsam/api/');

/*General OK*/
var $OK = new $.RestClient($BackEndSAMUri + '/backendsam/api/');

//ServiciosTecnicos
var $CamposPredeterminados = new $.RestClient($BackEndSAMUri + '/backendsam/api/');

var $Proyectos = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $ServiciosTecnicosGeneral = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $RequisicionPND = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $EntregaPlacasGraficas = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $EditarRequisicion = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $ValidacionRT = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $OKPND = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $FechasSpool = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $OkDocumental = new $.RestClient($BackEndSAMUri + '/backendsam/api/');

//Condiciones Climatologicas
var $CondicionesClimatologicas = new $.RestClient($BackEndSAMUri + '/backendsam/api/');

//Pintura
var $PinturaGeneral = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $SistemaPintura = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $CargaCarro = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $CapturaAvance = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $SistemaPinturaAplicable = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $ListadoSistemaPintura = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $AvanceCuadrante = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $AdminComponentes = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $AdminReductores = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $RevisionPintura = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $DescargarCarro = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $PruebasPorLote = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $OkPintura = new $.RestClient($BackEndSAMUri + '/backendsam/api/');

//Embarque
var $Etiquetado = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $CargaPlana = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $EmbarqueGeneral = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $Proveedores = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $Encintado = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $Empaquetado = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $PreparacionEmbarque = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $RevisionEmbarque = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $ListadoEmbarque = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $AsignacionEnvio = new $.RestClient($BackEndSAMUri + '/backendsam/api/');

//Ingeneria
var $BuscaSpool = new $.RestClient($BackEndSAMUri + '/backendsam/api/');

//Planeacion Y Control
var $EmisionOT = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $Ciclos = new $.RestClient($BackEndSAMUri + '/backendsam/api/');



//#region Catalogos
var $TrabajoAdicional = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $TipoTrabajoAdicional = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $TipoObrero = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $Obrero = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $ObreroUbicacion = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $Defectos = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $ListadoCamposPredeterminados = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $Taller = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $Pruebas = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
var $Secuencias = new $.RestClient($BackEndSAMUri + '/backendsam/api/');
//#endregion


/****************************/
/*    Document Ready        */
/****************************/

//Method to be called on the document ready and contains all the pertinent code for a partial view
function apiManagerToBeExecutedOnDocumentReady() {
    //CODE
}

/****************************/
/*    Global Functions      */
/****************************/

$BackEndSAM.add('DummyListadoAvisoLlegada');
$BackEndSAM.add('perfil');
$SecurityManager.add('authentication');

//Detalle Aviso Llegada
$Plana.add("Plana");
$Transportista.add("Transportista");
$Patio.add("Patio");
$Proveedor.add("Proveedor");
$AvisoLlegada.add("AvisoLlegada");
$Chofer.add("Chofer");
$Proyecto.add("Proyecto");
$Camion.add("Tracto");
$Contacto.add("Contacto");
$folioEntradaMaterial.add("Listado");
$FiltrosListadoEntradaMaterial.add("AvisoLlegada");
$FiltrosListadoLlegadaMaterial.add("FolioAvisoEntrada");
$TipoVehiculo.add("TipoVehiculo");
$Cliente.add("Cliente");
$TipoAviso.add("TipoAviso");
$PermisoAutorizado.add("PermisoAutorizado");
$TipoArchivo.add("TipoArchivo");
$PermisoTramite.add("PermisoAduana");
$DocumentoAvisoLlegada.add("DocumentoAvisoLlegada");
$ValidacionFolioConPermisoAduana.add("ValidacionFolioConPermisoAduana");
$DummyIncidencias.add("DummyIncidencias")
$DocumentoPaseSalida.add("DocumentoPaseSalida");
$PaseSalida.add("PaseSalida");
$DocumentoFolioAvisoEntrada.add("DocumentoFolioAvisoEntrada");
$FolioAvisoEntrada.add("FolioAvisoEntrada");
$OrdenDescarga.add("OrdenDescarga");
$MessagesManager.add("MessageManager");
$ObtenerDatosFolioAvisoEntrada.add("ObtenerDatosFolioAvisoEntrada");
$ObtenerDatosFolioLlegadaMaterial.add("ObtenerDatosFolioLlegadaMaterial");
$TipoPackingList.add("TipoPackingList");
$TipoUso.add("TipoUso");
$ListadoCuantificacion.add("ListadoCuantificacion");
$ValidarItemCodeNuCuantificacion.add("ValidarItemCodeNuCuantificacion");
$DummyObtenerColadasPorProyecto.add("DummyColadasPorProyecto");
$Familia.add("Familia");
$ItemCode.add("ItemCode");
$ItemCodeSteelgo.add("ItemCodeSteelgo");
$Fabricantes.add("Fabricantes");
$TipoAcero.add("TipoAcero");
$DummyColada.add("DummyColada");
$DummyCambiarEstatusCuantificacion.add("DummyCambiarEstatusCuantificacion");
$DummyOrdenRecepcionFoliosAvisoEntrada.add("DummyOrdenRecepcionFoliosAvisoEntrada");
$DummyOrdenRecepcionItemCode.add("DummyOrdenRecepcionItemCode");
$DummyGridOrdenRecepcion.add("DummyGridOrdenRecepcion");
$OrdenRecepcion.add("OrdenRecepcion");
$NumeroUnico.add("NumeroUnico");
$OrdenAlmacenaje.add("OrdenAlmacenaje");
$GenerarOrdenAlmacenaje.add("GenerarOrdenAlmacenaje");
$FoliosCuantificacion.add("FoliosCuantificacion");
$CargarGridCuantificacion.add("CargarGridCuantificacion");
$Colada.add("Colada");
$GuardarFolioLlegadaCuantificacion.add("GuardarFolioLlegadaCuantificacion");
$CambiarEstatusCuantificacion.add("CambiarEstatusCuantificacion");
$ComplementarRecepcion.add("ComplementarRecepcion");
$ListadoMateriales.add("ListadoMateriales")
$Almacenaje.add("Almacenaje");
$DetalleNumeroUnico.add("DetalleNumeroUnico");
$ListadoMaterialesPorItemCode.add("ListadoMaterialesPorItemCode");
$Despacho.add("Despacho");
$Maquina.add("Maquina");
$Cortador.add("Cortador");
$NumerosUnicos.add("NumerosUnicos");
$Catalogos.add("AdministracionCatalogos");
$Corte.add("Corte")
$AsociacionICS.add("AsociacionICS");
$Cedulas.add("Cedulas");
$Clasificacion.add("Clasificacion");
$TipoIncidencia.add("TipoIncidencia");
$CatalogoICSteelgo.add("CatalogoICSteelgo");
$Incidencia.add("Incidencia");
$DocumentoIncidencia.add("DocumentoIncidencia");
$DocumentosCatalogos.add("DocumentosCatalogos");
$ImpresionDocumental.add("ImpresionDocumental");
$Grupo.add("Grupo");
$MaterialPendiente.add("MaterialPendiente");
$Entrega.add("Entrega");
$NotificacionDeficit.add("NotificacionDeficit");
$Usuario.add("Usuario");
$RevisionDeficit.add("RevisionDeficit");
$IncidenciaPaseSalida.add("IncidenciaPaseSalida");
$Busqueda.add("select");
$PreDespacho.add("PreDespacho");
$GuardarPermisoAduana.add("DocumentoPermisoAduana");
$Diametros.add("Diametros");
$ConvertirSpoolAGranel.add("ConvertirSpoolAGranel");
$Spool.add("Spool");
$FamiliaAcero.add("FamiliaAcero");
$Proxy.add("Proxy");
$Reporte.add("Reporte");
$Espesor.add("Espesor");
$ValidarCedulas.add("ValidarCedulas");
$ComplementoRecepcionAlt.add("Listado");
$PickingTicket.add("PickingTicket");
$UbicacionNumeroUnico.add("UbicacionNumeroUnico");


//INICIA MATERIALES
$PlanchadoMaterial.add("PlanchadoMaterial");

//INICIA ORDENES DE COMPRA

$Dynasol.add("Dynasol");
$PackingList.add("PackingList");
$DynasolCamion.add("Camion");
$RecepcionOC.add("RecepcionOC");
$CompraPagoOC.add("CompraPagoOC");
$CobroOC.add("CobroOC");
$PagoOC.add("PagoOC");
$CreacionOC.add("CreacionOC");