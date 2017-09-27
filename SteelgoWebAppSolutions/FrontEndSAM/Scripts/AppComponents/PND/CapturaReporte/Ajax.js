var TipoMuestraPredeterminadoID = 3049;
var TipoMuestraPredeterminadoPlanchado = 3065;
var TipoMuestraPredeterminadoSelecTodos = 3062;

function AjaxCargarCamposPredeterminados() {
    $CamposPredeterminados.CamposPredeterminados.read({ token: Cookies.get("token"), lenguaje: $("#language").val(), id: TipoMuestraPredeterminadoID }).done(function (data) {
        if (Error(data)) {
            if (data == "sin captura") {
                $('input:radio[name=Muestra]:nth(0)').trigger("click");
            }
            else if (data == "Todos") {
                $('input:radio[name=Muestra]:nth(1)').trigger("click");
            }
            loadingStop();
        }
    });
    $CamposPredeterminados.CamposPredeterminados.read({ token: Cookies.get("token"), lenguaje: $("#language").val(), id: TipoMuestraPredeterminadoSelecTodos }).done(function (data) {
        if (data == "Si") {
            $('input:radio[name=SelectSector]:nth(0)').trigger("click");
            $('input:radio[name=SelectTodos]:nth(0)').trigger("click");
        }
        else if (data == "No") {
            $('input:radio[name=SelectSector]:nth(1)').trigger("click");
            $('input:radio[name=SelectTodos]:nth(1)').trigger("click");
        }
        else if (data == "Ninguno") {
            $('input:radio[name=SelectSector]:nth(2)').trigger("click");
            $('input:radio[name=SelectTodos]:nth(2)').trigger("click");
        }
        loadingStop();
    });

    $CamposPredeterminados.CamposPredeterminados.read({ token: Cookies.get("token"), lenguaje: $("#language").val(), id: TipoMuestraPredeterminadoPlanchado }).done(function (data) {
        if (data == "Todos") {
            $('input:radio[name=LLena]:nth(0)').trigger("click");
        }
        else if (data == "Vacios") {
            $('input:radio[name=LLena]:nth(1)').trigger("click");
        }
        loadingStop();
    });

    AjaxProyecto();
};


function AjaxProyecto() {
    loadingStart();
    $ReporteRT.ReporteRT.read({ token: Cookies.get("token") }).done(function (data) {
        if (Error(data)) {
            $("#inputFuente").data("kendoComboBox").setDataSource();
            $("#inputFuente").data("kendoComboBox").text("");
            $("#inputTurno").data("kendoComboBox").setDataSource();
            $("#inputTurno").data("kendoComboBox").text("");

            $("#inputPrueba").data("kendoComboBox").setDataSource();
            $("#inputPrueba").data("kendoComboBox").text("");
            $("#inputProveedor").data("kendoComboBox").setDataSource();
            $("#inputProveedor").data("kendoComboBox").text("");
            $("#inputRequisicion").data("kendoComboBox").setDataSource();
            $("#inputRequisicion").data("kendoComboBox").text("");

            $("#lblTurno").text("");

            $("#inputProyecto").data("kendoComboBox").text("");

            $("#inputPrueba").data("kendoComboBox").dataSource.data([]);
            $("#inputProveedor").data("kendoComboBox").dataSource.data([]);
            $("#inputRequisicion").data("kendoComboBox").dataSource.data([]);

            $("#inputProyecto").data("kendoComboBox").dataSource.data(data);

            if ($("#inputProyecto").data("kendoComboBox").dataSource._data.length == 2) {
                $("#inputProyecto").data("kendoComboBox").select(1);
                AjaxPruebas($("#inputProyecto").data("kendoComboBox").dataSource._data[1].ProyectoID);
            }

            loadingStop();
        }
    });

}

function AjaxProveedor(proyectoID, patioID) {
    loadingStart();
    $ReporteRT.ReporteRT.read({ token: Cookies.get("token"), proyectoID: proyectoID, tipoPruebaID: $("#inputPrueba").data("kendoComboBox").value(), patioID: patioID }).done(function (data) {
        if (Error(data)) {
            $("#inputFuente").data("kendoComboBox").setDataSource();
            $("#inputFuente").data("kendoComboBox").text("");
            $("#inputTurno").data("kendoComboBox").setDataSource();
            $("#inputTurno").data("kendoComboBox").text("");
            $("#inputRequisicion").data("kendoComboBox").setDataSource();
            $("#inputRequisicion").data("kendoComboBox").text("");



            $("#lblTurno").text("");
            if (Error(data)) {
                $("#inputRequisicion").data("kendoComboBox").value("");
                $("#inputProveedor").data("kendoComboBox").dataSource.data([]);
                $("#inputProveedor").data("kendoComboBox").dataSource.data(data);
                if ($("#inputProveedor").data("kendoComboBox").dataSource._data.length == 2) {
                    $("#inputProveedor").data("kendoComboBox").select(1);
                    AjaxRequisicion($("#inputProyecto").data("kendoComboBox").value(), $("#inputProveedor").data("kendoComboBox").dataSource._data[1].ProveedorID);
                }

            }
        }
        loadingStop();
    });

}

function AjaxPruebas(ProyectoID) {
    loadingStart();
    $ReporteRT.ReporteRT.read({ token: Cookies.get("token"), proyectoID: ProyectoID, x: $("#language").val(), y: "" }).done(function (data) {
        if (Error(data)) {
            $("#inputFuente").data("kendoComboBox").setDataSource();
            $("#inputFuente").data("kendoComboBox").text("");
            $("#inputTurno").data("kendoComboBox").setDataSource();
            $("#inputTurno").data("kendoComboBox").text("");

            $("#inputProveedor").data("kendoComboBox").setDataSource();
            $("#inputProveedor").data("kendoComboBox").text("");
            $("#inputProveedor").data("kendoComboBox").dataSource.data([]);

            $("#inputRequisicion").data("kendoComboBox").setDataSource();
            $("#inputRequisicion").data("kendoComboBox").text("");

            var tipoPruebaID = 0;
            $("#lblTurno").text("");
            $("#inputPrueba").data("kendoComboBox").text("");
            $("#inputPrueba").data("kendoComboBox").dataSource.data([]);
            $("#inputPrueba").data("kendoComboBox").dataSource.data(data);


            if ($("#inputPrueba").data("kendoComboBox").dataSource._data.length == 2) {
                $("#inputPrueba").data("kendoComboBox").select(1);
                AjaxProveedor($("#inputProyecto").data("kendoComboBox").dataSource._data[1].ProyectoID, $("#inputProyecto").data("kendoComboBox").dataSource._data[1].PatioID);
            }
        }
    });
};

function AjaxRequisicion(proyectoID, proveedorID) {
    loadingStart();

    $ReporteRT.ReporteRT.read({ token: Cookies.get("token"), proyectoID: proyectoID, proveedorID: proveedorID }).done(function (data) {
        if (Error(data)) {
            $("#inputFuente").data("kendoComboBox").setDataSource();
            $("#inputFuente").data("kendoComboBox").text("");
            $("#inputTurno").data("kendoComboBox").setDataSource();
            $("#inputTurno").data("kendoComboBox").text("");

            $("#inputRequisicion").data("kendoComboBox").dataSource.data([]);
            $("#inputRequisicion").data("kendoComboBox").dataSource.data(data);
            $("#lblTurno").text("");
            if ($("#inputRequisicion").data("kendoComboBox").dataSource._data.length == 2) {
                $("#inputRequisicion").data("kendoComboBox").select(1);
                $("#lblTurno").text($("#inputRequisicion").data("kendoComboBox").dataItem($("#inputRequisicion").data("kendoComboBox").select()).TurnoLaboral);

                if ($("#inputPrueba").data("kendoComboBox").dataItem($("#inputPrueba").data("kendoComboBox").select()).RequiereEquipoCaptura) {
                    AjaxFuente();
                    $("#inputFuente").data("kendoComboBox").enable(true);
                }
                else {
                    AjaxTurno();
                    $("#inputFuente").data("kendoComboBox").enable(false);
                }
            }
        }
        loadingStop();
    });

}

function AjaxFuente() {//Equipo
    loadingStart();

    $ReporteRT.ReporteRT.read({
        token: Cookies.get("token"), TipoPruebaID: $("#inputPrueba").data("kendoComboBox").value(),
        ProveedorID: $("#inputProveedor").data("kendoComboBox").value(), lenguaje: "es-MX",
    }).done(function (data) {
        if (Error(data)) {
            $("#inputFuente").data("kendoComboBox").value("");
            $("#inputFuente").data("kendoComboBox").dataSource.data(data);
            $("#inputTurno").data("kendoComboBox").text("");
            $("#inputTurno").data("kendoComboBox").dataSource.data([]);
            listadoTurno = data;
        }
        loadingStop();
    });

}

function AjaxTurno() {

    if ($("#inputFuente").data("kendoComboBox").text() != "" || $("#inputPrueba").data("kendoComboBox").text().indexOf("RT") === -1) {
        loadingStart();
        $ReporteRT.ReporteRT.read({
            token: Cookies.get("token"),
            TipoPruebaID: $("#inputPrueba").data("kendoComboBox").value(),
            ProveedorID: $("#inputProveedor").data("kendoComboBox").value(),
            EquipoID: $("#inputFuente").data("kendoComboBox").value() == "" ? 0 : $("#inputFuente").data("kendoComboBox").value(),
            lenguaje: "es-MX"
        }).done(function (data) {
            if (Error(data)) {
                $("#inputTurno").data("kendoComboBox").value("");
                $("#inputTurno").data("kendoComboBox").dataSource.data(data);
            }
            loadingStop();
        });
    }
}

function ajaxResultadosDetalle(proyectoID, proveedorID, requisicionID) {
    loadingStart();

    $ReporteRT.ReporteRT.read({
        token: Cookies.get("token"),
        proyectoID: (($("#inputProyecto").data("kendoComboBox").value() != "") ? ($("#inputProyecto").data("kendoComboBox").value()) : (0)),
        tipoPruebaID: (($("#inputPrueba").data("kendoComboBox").value() != "") ? ($("#inputPrueba").data("kendoComboBox").value()) : (0)),
        proveedorID: (($("#inputProveedor").data("kendoComboBox").value() != "") ? ($("#inputProveedor").data("kendoComboBox").value()) : (0)),
        requisicionID: (($("#inputRequisicion").data("kendoComboBox").value() != "") ? ($("#inputRequisicion").data("kendoComboBox").value()) : (0)),
        equipoID: (($("#inputFuente").data("kendoComboBox").value() != "") ? ($("#inputFuente").data("kendoComboBox").value()) : (0)),
        turnoID: (($("#inputTurno").data("kendoComboBox").value() != "") ? ($("#inputTurno").data("kendoComboBox").value()) : (0)),
        lenguaje: $("#language").val()
    }).done(function (data) {
        if (Error(data)) {
            $("#grid").data('kendoGrid').dataSource.data([]);
            var ds = $("#grid").data("kendoGrid").dataSource;
            for (var i = 0; i < data.length; i++) {
                ds.add(data[i]);
            }
        }
        $("#grid").data('kendoGrid').dataSource.sync()
        loadingStop();
    });
}

function AjaxGuardarCaptura(ds, TipoGuardar) {
    if (ds.length > 0) {
        var RequisicionID = 0;
        var Captura = [];
        Captura[0] = { Detalles: "" }
        var listaDetalles = [];
        var listaErrores = "";
        var cont = 0;
        for (var i = 0; i < ds.length; i++) {
            $("#grid").data("kendoGrid").dataSource._data[i].RowOk = true;
            $("#grid").data("kendoGrid").dataSource.sync();
            //valida que tenga numero de placas y se ha capturado tamaño y densidad

            listaDetalles[cont] = {
                CapturaResultadoID: "", RequisicionID: "", ElementoClasificacionPNDID: "", ProveedorEquipoID: "",
                CapacidadTurnoEquipoID: "", CapacidadTurnoProveedorID: "", Status: "", Evaluacion: "", NoPlacas: "",
                ResultadoID: "", Accion: "",NombreGrafica: "", DefectoID: "", ListaDetallePorPlacas: "", listaDetalleIndicacion: "", Estatus: 1
            };

            listaDetalles[cont].CapturaResultadoID = ds[i].CapturaResultadoID;
            listaDetalles[cont].RequisicionID = ds[i].RequisicionID;
            listaDetalles[cont].ElementoClasificacionPNDID = ds[i].ElementoPorClasificacionPNDID;
            listaDetalles[cont].ProveedorEquipoID = ds[i].ProveedorEquipoID;
            listaDetalles[cont].CapacidadTurnoProveedorID = ds[i].CapacidadTurnoProveedorID;
            listaDetalles[cont].CapacidadTurnoEquipoID = ds[i].CapacidadTurnoEquipoID;
            listaDetalles[cont].Status = 3;
            listaDetalles[cont].Evaluacion = ds[i].Evaluacion;
            listaDetalles[cont].NoPlacas = ds[i].NoPlacas;
            listaDetalles[cont].ResultadoID = ds[i].ResultadoID;
            listaDetalles[cont].Accion = ds[i].Accion;
            listaDetalles[cont].NombreGrafica = ds[i].NombreGrafica;
            listaDetalles[cont].DefectoID = ds[i].DefectoID;


            var informacion = [];
            var detallesDefectos = [];
            for (var j = 0; j < ds[i].ListaDetallePorPlacas.length; j++) {
                informacion[j] = {
                    CapturaResultadoPlacaID: "", ElementoClasificacionPNDID: "", CapturaResultadoID: "",
                    Ubicacion: "", ResultadoID: "", Accion: ""
                }

                informacion[j].CapturaResultadoPlacaID = ds[i].ListaDetallePorPlacas[j].CapturaResultadoPlacaID;
                informacion[j].ElementoClasificacionPNDID = ds[i].ListaDetallePorPlacas[j].ElementoPorClasificacionPNDID;
                informacion[j].CapturaResultadoID = ds[i].CapturaResultadoID;
                informacion[j].Ubicacion = ds[i].ListaDetallePorPlacas[j].Ubicacion;
                informacion[j].ResultadoID = ds[i].ListaDetallePorPlacas[j].ResultadoID;
                informacion[j].Accion = ds[i].ListaDetallePorPlacas[j].Accion;



                for (var k = 0; k < ds[i].ListaDetallePorPlacas[j].ListaDetalleDefectos.length; k++) {

                    detallesDefectos[k] = {
                        CapturaResultadoPlacaDefectoID: 0, CapturaResultadoPlacaID: 0, ElementoClasificacionPNDID: 0,
                        Ubicacion: "", ResultadoID: 0, DefectoID: 0, InicioMM: 0, FinMM: 0, RangoCuadranteID: 0, Accion: 0, RangoCuadranteID: 0
                    }
                    detallesDefectos[k].CapturaResultadoPlacaDefectoID = ds[i].ListaDetallePorPlacas[j].ListaDetalleDefectos[k].CapturaResultadoPlacaDefectoID;
                    detallesDefectos[k].CapturaResultadoPlacaID = ds[i].ListaDetallePorPlacas[j].CapturaResultadoPlacaID;
                    detallesDefectos[k].ElementoClasificacionPNDID = ds[i].ElementoPorClasificacionPNDID;
                    detallesDefectos[k].Ubicacion = ds[i].ListaDetallePorPlacas[j].ListaDetalleDefectos[k].Ubicacion;
                    detallesDefectos[k].ResultadoID = ds[i].ListaDetallePorPlacas[j].ListaDetalleDefectos[k].ResultadoID;
                    detallesDefectos[k].DefectoID = ds[i].ListaDetallePorPlacas[j].ListaDetalleDefectos[k].DefectoID;
                    detallesDefectos[k].InicioMM = ds[i].ListaDetallePorPlacas[j].ListaDetalleDefectos[k].InicioMM;
                    detallesDefectos[k].FinMM = ds[i].ListaDetallePorPlacas[j].ListaDetalleDefectos[k].FinMM;
                    detallesDefectos[k].RangoCuadranteID = ds[i].ListaDetallePorPlacas[j].ListaDetalleDefectos[k].RangoCuadranteID;
                    detallesDefectos[k].Accion = ds[i].ListaDetallePorPlacas[j].ListaDetalleDefectos[k].Accion;
                    detallesDefectos[k].RangoCuadranteID = ds[i].ListaDetallePorPlacas[j].ListaDetalleDefectos[k].RangoCuadranteID;


                }
                listaDetalles[cont].ListaDetallePorPlacas = informacion;
                listaDetalles[cont].listaDetalleIndicacion = detallesDefectos;
            }






            if ($("#inputPrueba").data("kendoComboBox").dataItem($("#inputPrueba").data("kendoComboBox").select()).RequiereEquipoCaptura) {
                if (ds[i].Equipo == "" || ds[i].Turno == "") {
                    listaDetalles[cont].Estatus = 0; //el elemento esta mal.
                    $("#grid").data("kendoGrid").dataSource._data[i].RowOk = false;
                    $("#grid").data("kendoGrid").dataSource.sync();
                }
                
            }
            else {
                if (ds[i].Turno == "") {
                    listaDetalles[cont].Estatus = 0; //el elemento esta mal.
                    $("#grid").data("kendoGrid").dataSource._data[i].RowOk = false;
                    $("#grid").data("kendoGrid").dataSource.sync();
                }
            }

            if (ds[i].ResultadoID == 0) {
                listaDetalles[cont].Estatus = 0; //el elemento esta mal.
                $("#grid").data("kendoGrid").dataSource._data[i].RowOk = false;
                $("#grid").data("kendoGrid").dataSource.sync();
            }

            if (!($("#inputPrueba").data("kendoComboBox").text().indexOf("RT") === -1)) {
                if (ds[i].NumeroPlacas <= 0 || ds[i].NumeroPlacas == null || ds[i].NumeroPlacas == undefined) {
                    listaDetalles[cont].Estatus = 0 //el elemento esta mal.
                    $("#grid").data("kendoGrid").dataSource._data[i].RowOk = false;
                    $("#grid").data("kendoGrid").dataSource.sync();
                }
                
            }
            else {
                if (ds[i].NombreGrafica == "") {
                    listaDetalles[cont].Estatus = 0 //el elemento esta mal.
                    $("#grid").data("kendoGrid").dataSource._data[i].RowOk = false;
                    $("#grid").data("kendoGrid").dataSource.sync();
                }
                if (ds[i].Resultado == _dictionary.ValidacionResultadosComboRechazado[$("#language").data("kendoDropDownList").value()]) {
                    if (ds[i].Indicacion == "") {
                        listaDetalles[cont].Estatus = 0 //el elemento esta mal.
                        $("#grid").data("kendoGrid").dataSource._data[i].RowOk = false;
                        $("#grid").data("kendoGrid").dataSource.sync();
                    }
                }
            }
            cont++;

        }
        Captura[0].Detalles = listaDetalles;

        if (!ExistRowEmpty(listaDetalles)) {

            loadingStart();
            //setTimeout(function () {
            //    disableEnableView(true);
            //    displayNotify("EntregaPlacasGraficasMensajeGuardadoExistoso", "", '0');
            //    loadingStop();
            //}, 700);

            $ReporteRT.ReporteRT.create(Captura[0], { token: Cookies.get("token"), lenguaje: $("#language").val(), RequisicionID: $("#inputRequisicion").data("kendoComboBox").value() }).done(function (data) {
                if (Error(data)) {
                    if (data.ReturnMessage.length > 0 && data.ReturnMessage[0] == "Ok") {

                        if (TipoGuardar) {
                            cleanView();
                        } else {
                            $("#grid").data('kendoGrid').dataSource.data([]);
                            ajaxResultadosDetalle($("#inputProyecto").data("kendoComboBox").value(), $("#inputProveedor").data("kendoComboBox").value(), $("#inputRequisicion").data("kendoComboBox").value());
                            disableEnableView(true);
                        }

                        displayNotify("MensajeGuardadoExistoso", "", '0');
                    } else {
                        displayNotify("MensajeGuardadoErroneo", "", '2');
                    }
                }
            });
        } else {

            ventanaConfirm = $("#ventanaConfirm").kendoWindow({
                iframe: true,
                title: _dictionary.WarningTitle[$("#language").data("kendoDropDownList").value()],
                visible: false,
                width: "auto",
                height: "auto",
                modal: true,
                animation: {
                    close: false,
                    open: false
                }
            }).data("kendoWindow");

            ventanaConfirm.content(_dictionary.ListadoCatalogos0011[$("#language").data("kendoDropDownList").value()] +
                "</br><center><button class='btn btn-blue' id='yesButton'>Si</button><button class='btn btn-blue' id='noButton'>No</button></center>");

            ventanaConfirm.open().center();
            $("#yesButton").click(function () {

                var elemento = 0;

                while (elemento < Captura[0].Detalles.length && Captura[0].Detalles.length != 0) {
                    if (Captura[0].Detalles[elemento].Estatus == 0) {
                        Captura[0].Detalles.pop(Captura[0].Detalles[elemento]);
                    }
                    else
                        elemento++;
                }

                if (Captura[0].Detalles.length > 0) {

                    loadingStart();
                    ventanaConfirm.close();

                    $ReporteRT.ReporteRT.create(Captura[0], { token: Cookies.get("token"), lenguaje: $("#language").val(), RequisicionID: $("#inputRequisicion").data("kendoComboBox").value() }).done(function (data) {
                        if (Error(data)) {
                            if (data.ReturnMessage.length > 0 && data.ReturnMessage[0] == "Ok") {

                                if (TipoGuardar) {
                                    cleanView();
                                } else {
                                    $("#grid").data('kendoGrid').dataSource.data([]);
                                    ajaxResultadosDetalle($("#inputProyecto").data("kendoComboBox").value(), $("#inputProveedor").data("kendoComboBox").value(), $("#inputRequisicion").data("kendoComboBox").value());
                                    disableEnableView(true);
                                }

                                displayNotify("MensajeGuardadoExistoso", "", '0');
                            } else {
                                displayNotify("MensajeGuardadoErroneo", "", '2');
                            }
                        }
                    });
                } else {
                    ventanaConfirm.close();
                    displayNotify("EditarRequisicionExcepcionGuardado", "", '1');   
                }

            });
            $("#noButton").click(function () {
                ventanaConfirm.close();
            });
        }
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