var listadoTurno = [];

function AjaxProyecto() {
    loadingStart();
    $ReporteRT.ReporteRT.read({ token: Cookies.get("token") }).done(function (data) {
        if (Error(data)) {
            $("#inputProyecto").data("kendoComboBox").value("");
            $("#inputRequisicion").data("kendoComboBox").value("");
            $("#inputProveedor").data("kendoComboBox").value("");
            $("#inputProyecto").data("kendoComboBox").dataSource.data(data);

            if ($("#inputProyecto").data("kendoComboBox").dataSource._data.length == 2) {
                $("#inputProyecto").data("kendoComboBox").select(1);
                AjaxPruebas($("#inputProyecto").data("kendoComboBox").dataSource._data[1].ProyectoID);
            }
            else {
                $("#inputProyecto").data("kendoComboBox").select(0);
                loadingStop();
            }
        }
        loadingStop();
    });
}

function AjaxProveedor(proyectoID, patioID) {
    loadingStart();
    $ReporteRT.ReporteRT.read({ token: Cookies.get("token"), proyectoID: proyectoID, tipoPruebaID: $("#inputPrueba").data("kendoComboBox").value(), patioID: patioID }).done(function (data) {
        if (Error(data)) {
            $("#inputRequisicion").data("kendoComboBox").value("");
            $("#inputProveedor").data("kendoComboBox").dataSource.data([]);
            $("#inputProveedor").data("kendoComboBox").dataSource.data(data);
            if ($("#inputProveedor").data("kendoComboBox").dataSource._data.length == 2) {
                $("#inputProveedor").data("kendoComboBox").select(1);
                AjaxRequisicion($("#inputProyecto").data("kendoComboBox").value(), $("#inputProveedor").data("kendoComboBox").dataSource._data[1].ProveedorID);
            }
            else {
                $("#inputProveedor").data("kendoComboBox").select(0);
                loadingStop();
            }
        }
        loadingStop();
    });
}

function AjaxPruebas(ProyectoID) {
    loadingStart();
    $ReporteRT.ReporteRT.read({ token: Cookies.get("token"), proyectoID: ProyectoID, x: $("#language").val(), y: "" }).done(function (data) {
        var tipoPruebaID = 0;
        $("#inputRequisicion").data("kendoComboBox").value("");
        $("#inputProveedor").data("kendoComboBox").value("");
        $("#inputPrueba").data("kendoComboBox").dataSource.data([]);
        $("#inputPrueba").data("kendoComboBox").dataSource.data(data);
        if ($("#inputPrueba").data("kendoComboBox").dataSource._data.length == 2) {
            $("#inputPrueba").data("kendoComboBox").select(1);
            AjaxProveedor($("#inputProyecto").data("kendoComboBox").dataSource._data[1].ProyectoID, $("#inputProyecto").data("kendoComboBox").dataSource._data[1].PatioID);
        }
        else {
            $("#inputPrueba").data("kendoComboBox").select(0);
            loadingStop();
        }
    });
};

function AjaxRequisicion(proyectoID, proveedorID) {
    loadingStart();
    $ReporteRT.ReporteRT.read({ token: Cookies.get("token"), proyectoID: proyectoID, proveedorID: proveedorID }).done(function (data) {
        if (Error(data)) {
            $("#inputRequisicion").data("kendoComboBox").dataSource.data([]);
            $("#inputRequisicion").data("kendoComboBox").dataSource.data(data);
            if ($("#inputRequisicion").data("kendoComboBox").dataSource._data.length == 2) {
                $("#inputRequisicion").data("kendoComboBox").select(1);
                $("#btnAgregar").trigger("click");
            }
            else {
                $("#inputRequisicion").data("kendoComboBox").select(0);
                loadingStop();
            }
        }
        loadingStop();
    });
}

function ajaxResultadosDetalle(proyectoID, proveedorID, requisicionID) {
    loadingStart();
    var data = [];
    data = [
        {
            Accion: 1,
            ClasificacionPND: "RT",
            CodigoAsme: "ASME VIII Div 1 App 8",
            Densidad: null,
            Equipo: "Fuente Radiográfica",
            EquipoID: 1,
            EsSector: true,
            EstatusRequisicion: 0,
            Junta: 43,
            JuntaSpoolID: 206420,
            ListaDefectos: [{ DefectoID: 0, Defecto: "" }, { DefectoID: 2, Defecto: "Defecto Nivel Junta" }],
            ListaDetallePorPlacas: [
               {
                   Accion: 2,
                   JuntaSpoolID: 206420,
                   ListaDefectos: [{ DefectoID: 0, Defecto: "" }, { DefectoID: 2, Defecto: "Defecto Nivel Junta" }],                   
                   ListaDetalleDefectos: [
                       {
                           Defecto: "Defecto Nivel Junta",
                           InicioMM: 1,
                           FinMM: 2,
                           InicioCuadrante: "A",
                           FinCuadrante: "B",
                           Resultado: "Rechazado",                           
                       }
                   ],
                   ListaResultados: [{ ResultadosID: null, Resultado: "" }, { ResultadosID: 1, Resultado: "Rechazado" }, { ResultadosID: 2, Resultado: "Aprobado" }],
                   OrdenTrabajoID: 960,
                   Resultado: "Rechazado",                   
                   ResultadoConciliacion: "Rechazado",
                   RazonNoConciliacion: "RESULTADO CONCILACIÓN 1",
                   ResultadoID: 1,
                   SpoolID: 41788,
                   TemplateDetallePorPlaca: "Detalle",
                   Ubicacion: "0-1",
               },
               {
                   Accion: 2,
                   JuntaSpoolID: 206420,
                   ListaDefectos: [{ DefectoID: 0, Defecto: "" }, { DefectoID: 2, Defecto: "Defecto Nivel Junta" }],
                   ListaDetalleDefectos: [
                      {
                          Defecto: "Otro Defecto Nivel Junta 2",
                          InicioMM: 1,
                          FinMM: 2,
                          InicioCuadrante: "A",
                          FinCuadrante: "A",
                          Resultado: "Aprobado",
                      }
                   ],
                   ListaResultados: [{ ResultadosID: null, Resultado: "" }, { ResultadosID: 1, Resultado: "Rechazado" }, { ResultadosID: 2, Resultado: "Aprobado" }],
                   OrdenTrabajoID: 960,
                   Resultado: "Aprobado",
                   ResultadoConciliacion: "Aprobado",
                   RazonNoConciliacion: "",
                   ResultadoID: 1,
                   SpoolID: 41788,
                   TemplateDetallePorPlaca: "Detalle",
                   Ubicacion: "1-0",
               }
            ],
            ListaResultados: [{ ResultadosID: null, Resultado: "" }, { ResultadosID: 1, Resultado: "Rechazado" }, { ResultadosID: 2, Resultado: "Aprobado" }],
            NumeroControl: "X004-008",
            NumeroPlacas: 2,
            Observaciones: "OBSERVACION 1",
            OrdenTrabajoID: 960,
            ResultadoConciliacion: "Rechazado",
            RazonNoConciliacion: "RESULTADO CONCILACIÓN 1",
            RequisicionID: 26,            
            SpoolID: 41788,
            Tamano: null,
            TemplateDetalleElemento: "Ver detalle",
            TipoPrueba: "Reporte de RT",
            Turno: "Matutino",
            TurnoLaboralID: 1
        },
        {
            Accion: 1,
            ClasificacionPND: "RT",
            CodigoAsme: "ETIL X",
            Densidad: null,
            Equipo: "Bobina",
            EquipoID: 2,
            EsSector: true,
            EstatusRequisicion: 0,
            Junta: 42,
            JuntaSpoolID: 206420,
            ListaDefectos: [{ DefectoID: 0, Defecto: "" }, { DefectoID: 2, Defecto: "Defecto Nivel Junta" }],
            ListaDetallePorPlacas: [
                {
                    Accion: 2,
                    JuntaSpoolID: 206420,
                    ListaDefectos: [{ DefectoID: 0, Defecto: "" }, { DefectoID: 2, Defecto: "Defecto Nivel Junta" }],
                    ListaDetalleDefectos: [
                       {
                           Defecto: "Otro Defecto Nivel Junta",
                           InicioMM: 1,
                           FinMM: 2,
                           InicioCuadrante: "A",
                           FinCuadrante: "B",
                           Resultado: "Rechazado",
                       }
                    ],
                    ListaResultados: [{ ResultadosID: null, Resultado: "" }, { ResultadosID: 1, Resultado: "Rechazado" }, { ResultadosID: 2, Resultado: "Aprobado" }],
                    OrdenTrabajoID: 960,
                    Resultado: "Rechazado",
                    RazonNoConciliacion: "Rechazado",
                    ResultadoConciliacion: "RESULTADO CONCILACIÓN 2",
                    ResultadoID: 1,
                    SpoolID: 41788,
                    TemplateDetallePorPlaca: "Detalle",
                    Ubicacion: "0-0",
                }                
            ],
            ListaResultados: [{ ResultadosID: null, Resultado: "" }, { ResultadosID: 1, Resultado: "Rechazado" }, { ResultadosID: 2, Resultado: "Aprobado" }],
            NumeroControl: "X004-008",
            NumeroPlacas: 1,
            Observaciones: "OBSERVACION 2",
            OrdenTrabajoID: 960,
            ResultadoConciliacion: "Aprobado",
            RazonNoConciliacion: "",
            RequisicionID: 26,            
            SpoolID: 41788,
            Tamano: null,
            TemplateDetalleElemento: "Ver detalle",
            TipoPrueba: "Reporte de RT",
            Turno: "Vespertino",
            TurnoLaboralID: 2
        },
        {
            Accion: 1,
            ClasificacionPND: "RT",
            CodigoAsme: "ETIL X",
            Densidad: null,
            Equipo: "Fuente Radiográfica",
            EquipoID: 1,
            EsSector: true,
            EstatusRequisicion: 0,
            Junta: 44,
            JuntaSpoolID: 206420,
            ListaDefectos: [{ DefectoID: 0, Defecto: "" }, { DefectoID: 2, Defecto: "Defecto Nivel Junta" }],
            ListaDetallePorPlacas: [
                {
                    Accion: 2,
                    JuntaSpoolID: 206420,
                    ListaDefectos: [{ DefectoID: 0, Defecto: "" }, { DefectoID: 2, Defecto: "Defecto Nivel Junta" }],
                    ListaDetalleDefectos: [
                       {
                           Defecto: "Defecto Nivel Junta",
                           InicioMM: 1,
                           FinMM: 2,
                           InicioCuadrante: "A",
                           FinCuadrante: "B",
                           Resultado: "Rechazado",
                       }
                    ],
                    ListaResultados: [{ ResultadosID: null, Resultado: "" }, { ResultadosID: 1, Resultado: "Rechazado" }, { ResultadosID: 2, Resultado: "Aprobado" }],
                    OrdenTrabajoID: 960,
                    Resultado: "Rechazado",
                    RazonNoConciliacion: "Rechazado",
                    ResultadoConciliacion: "RESULTADO CONCILACIÓN 3",
                    ResultadoID: 1,
                    SpoolID: 41788,
                    TemplateDetallePorPlaca: "Detalle",
                    Ubicacion: "0-0",
                }                
            ],
            ListaResultados: [{ ResultadosID: null, Resultado: "" }, { ResultadosID: 1, Resultado: "Rechazado" }, { ResultadosID: 2, Resultado: "Aprobado" }],
            NumeroControl: "X004-008",
            NumeroPlacas: 1,
            Observaciones: "OBSERVACION 3",
            OrdenTrabajoID: 960,
            ResultadoConciliacion: "Rechazado",
            RazonNoConciliacion: "RESULTADO CONCILACIÓN 3",
            RequisicionID: 26,            
            SpoolID: 41788,
            Tamano: null,
            TemplateDetalleElemento: "Ver detalle",
            TipoPrueba: "Reporte de RT",
            Turno: "Vespertino",
            TurnoLaboralID: 2
        }];
    
    var tipoPrueba = $("#inputPrueba").data("kendoComboBox").value();
    var ds = $("#grid").data("kendoGrid").dataSource;
    if (tipoPrueba == 2) {                
        for (var i = 0; i < data.length; i++) {
            data[i].TipoPrueba = "Reporte de PT";
            ds.add(data[i]);
        }
    } else {                
        for (var i = 0; i < data.length; i++) {         
            ds.add(data[i]);
        }
    }       
    loadingStop();
    //$ReporteRT.ReporteRT.read({
    //    token: Cookies.get("token"),
    //    proyectoID: (($("#inputProyecto").data("kendoComboBox").value() != "") ? ($("#inputProyecto").data("kendoComboBox").value()) : (0)),
    //    tipoPruebaID: (($("#inputPrueba").data("kendoComboBox").value() != "") ? ($("#inputPrueba").data("kendoComboBox").value()) : (0)),
    //    proveedorID: (($("#inputProveedor").data("kendoComboBox").value() != "") ? ($("#inputProveedor").data("kendoComboBox").value()) : (0)),
    //    requisicionID: (($("#inputRequisicion").data("kendoComboBox").value() != "") ? ($("#inputRequisicion").data("kendoComboBox").value()) : (0)),
    //    equipoID: 0,
    //    turnoID: 0,        
    //    lenguaje: $("#language").val()
    //}).done(function (data) {
    //    if (Error(data)) {
    //        $("#grid").data('kendoGrid').dataSource.data([]);
    //        var ds = $("#grid").data("kendoGrid").dataSource;
    //        for (var i = 0; i < data.length; i++) {
    //            ds.add(data[i]);
    //        }
    //    }
    //    loadingStop();
    //});
}

function AjaxGuardarCaptura(ds, guardarYNuevo) {
    loadingStart();
    if (guardarYNuevo) {
        setTimeout(function (e) {
            loadingStop();
            cleanView();
            displayNotify("EntregaPlacasGraficasMensajeGuardadoExistoso", "", "0");            
        }, 700);
    } else {
        setTimeout(function (e) {
            loadingStop();
            disableEnableView(true);
            displayNotify("EntregaPlacasGraficasMensajeGuardadoExistoso", "", "0");            
        }, 700);
    }
}



function AjaxObtenerElementoRequisicion(paramReq) {
    $ReporteRT.ReporteRT.read({ lenguaje: $("#language").val(), token: Cookies.get("token"), RequisicionID: paramReq, Flag: true }).done(function (data) {
        if (data != null) {
            $("#inputProyecto").data("kendoComboBox").dataSource.data([]);
            $("#inputProyecto").data("kendoComboBox").dataSource.data(data.listaProyecto);
            $("#inputProyecto").data("kendoComboBox").value(data.ProyectoID);
            $("#inputProyecto").data("kendoComboBox").select(1);

            $("#inputPrueba").data("kendoComboBox").dataSource.data([]);
            $("#inputPrueba").data("kendoComboBox").dataSource.data(data.listaTipoPrueba);
            $("#inputPrueba").data("kendoComboBox").value(data.TipoPruebaID);
            $("#inputPrueba").data("kendoComboBox").select(1);

            $("#inputProveedor").data("kendoComboBox").dataSource.data([]);
            $("#inputProveedor").data("kendoComboBox").dataSource.data(data.listaProveedor);
            $("#inputProveedor").data("kendoComboBox").value(data.ProveedorID);
            $("#inputProveedor").data("kendoComboBox").select(1);

            $("#inputRequisicion").data("kendoComboBox").dataSource.data([]);
            $("#inputRequisicion").data("kendoComboBox").dataSource.data(data.listaRequisicion);
            $("#inputRequisicion").data("kendoComboBox").value(data.RequisicionID);
            $("#inputRequisicion").data("kendoComboBox").select(1);

            ajaxResultadosDetalle(0, 0, 0);
        }
    });
}
