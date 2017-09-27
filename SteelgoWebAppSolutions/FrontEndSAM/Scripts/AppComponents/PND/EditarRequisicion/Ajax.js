function AjaxCargarCamposPredeterminados() {
    var campoPredeterminado = 3052;
    $CamposPredeterminados.CamposPredeterminados.read({ token: Cookies.get("token"), lenguaje: $("#language").val(), id: campoPredeterminado }).done(function (data) {
        if (data == "sin captura") {
            $('input:radio[name=Muestra]:nth(0)').trigger("click");
        }
        else if (data == "Todos") {
            $('input:radio[name=Muestra]:nth(1)').trigger("click");
        }
    });
}


var dataSpoolArray = null;
function AjaxObtenerSpoolID() {
    var OrdenTrabajoOrigianl = $("#InputOrdenTrabajo").val();
    $Armado.Armado.read({ ordenTrabajo: $("#InputOrdenTrabajo").val(), tipo: '1', token: Cookies.get("token"), lenguaje: $("#language").val() }).done(function (data) {
        dataSpoolArray = data;
        if (Error(data)) {
            if (data.OrdenTrabajo != "") {
                $("#InputOrdenTrabajo").val(data.OrdenTrabajo);
            }
            else {
                $("#InputOrdenTrabajo").val(OrdenTrabajoOrigianl);
                displayNotify("CapturaArmadoMensajeOrdenTrabajoNoEncontrada", "", '1');
            }

            $("#InputID").data("kendoComboBox").dataSource.data(data.idStatus);
            Cookies.set("LetraProyecto", data.OrdenTrabajo.substring(0, 1), { path: '/' });
            $("#InputID").data("kendoComboBox").enable(true);
            $("#InputID").data("kendoComboBox").input.focus();
        }
    });
}

function AjaxObtenerElementoRequisicion(paramReq) {

    $EditarRequisicion.EditarRequisicion.read({ token: Cookies.get("token"), lenguaje: $("#language").val(), RequisicionID: paramReq }).done(function (data) {
        if (data != null) {
            $("#inputProyecto").data("kendoComboBox").dataSource.data([]);
            $("#inputProyecto").data("kendoComboBox").dataSource.data(data.listaProyecto);
            $("#inputProyecto").data("kendoComboBox").value(data.ProyectoID);

            $("#inputTipoPrueba").data("kendoComboBox").dataSource.data([]);
            $("#inputTipoPrueba").data("kendoComboBox").dataSource.data(data.listaTipoPrueba);
            $("#inputTipoPrueba").data("kendoComboBox").value(data.TipoPruebaID);

            $("#inputRequisicion").data("kendoComboBox").dataSource.data([]);
            $("#inputRequisicion").data("kendoComboBox").dataSource.data(data.listaRequisicion);
            $("#inputRequisicion").data("kendoComboBox").value(data.RequisicionID);
            $("#inputRequisicion").data("kendoComboBox").trigger("change");

        }
    });
}

function AjaxCargaListaProyecto() {
    var proyectoId = 0;
    $Proyectos.Proyectos.read({ token: Cookies.get("token") }).done(function (data) {
        $("#inputProyecto").data("kendoComboBox").dataSource.data([]);
        $("#inputProyecto").data("kendoComboBox").dataSource.data(data);
        if (data.length < 3) {
            for (var i = 0; i < data.length; i++) {
                if (data[i].ProyectoID != 0) {
                    proyectoId = data[i].ProyectoID;
                }
            }
        }

        $("#inputProyecto").data("kendoComboBox").value(proyectoId);
        $("#inputProyecto").data("kendoComboBox").trigger("change");
    });
}

function AjaxCargaListaTipoPrueba() {
    var tipoPruebaId = 0;
    $ServiciosTecnicosGeneral.ServiciosTecnicosGeneral.read({ token: Cookies.get("token"), lenguaje: $("#language").val() }).done(function (data) {
        $("#inputTipoPrueba").data("kendoComboBox").dataSource.data([]);
        $("#inputTipoPrueba").data("kendoComboBox").dataSource.data(data);
        if (data.length < 3) {
            for (var i = 0; i < data.length; i++) {
                if (data[i].TipoPruebaID != 0) {
                    tipoPruebaId = data[i].TipoPruebaID;
                }
            }
        }

        $("#inputTipoPrueba").data("kendoComboBox").value(tipoPruebaId);
        $("#inputTipoPrueba").data("kendoComboBox").trigger('change');
    });
}

function AjaxCargaListaRequisicion(tipoPruebaID, proyectoID) {
    var requisicionId = 0;
    //$ServiciosTecnicosGeneral.ServiciosTecnicosGeneral.read({ token: Cookies.get("token"), ProyectoID: proyectoID, TipoPruebaID: tipoPruebaID, estatusID: 1 }).done(function (data) {    
    $ServiciosTecnicosGeneral.ServiciosTecnicosGeneral.read({ token: Cookies.get("token"), ProyectoID: proyectoID, TipoPruebaID: tipoPruebaID, estatusID: 1, lenguaje: $("#language").val() }).done(function (data) {
        $("#inputRequisicion").data("kendoComboBox").dataSource.data([]);
        $("#inputRequisicion").data("kendoComboBox").dataSource.data(data);

        if (data.length < 3) {
            for (var i = 0; i < data.length; i++) {
                if (data[i].RequisicionID != 0) {
                    requisicionId = data[i].RequisicionID;
                }
            }
        }
        $("#inputRequisicion").data("kendoComboBox").value(requisicionId);
        $("#inputRequisicion").data("kendoComboBox").trigger("change");


    });
}


function AjaxCargaDetalleRequisicion(requisicionID, tipoPruebaID, proyectoID, muestra) {
    loadingStart();
    $RequisicionPND.RequisicionPND.read({ token: Cookies.get("token"), lenguaje: $("#language").val(), RequisicionID: requisicionID, TipoPruebaID: tipoPruebaID, ProyectoID: proyectoID, Muestra: muestra }).done(function (data) {
        $("#grid").data("kendoGrid").dataSource.data([]);
        var ds = $("#grid").data("kendoGrid").dataSource;
        if (data.length > 0) {
            for (var i = 0; i < data.length; i++) {
                ds.add(data[i]);
                tipoPrueba = data[i].TipoPruebaID;
            }
            ds.page(1);
        } else {
            ds.page(0);
        }
        ds.sync();
        loadingStop();
    });
}


function AjaxGuardaCaptura(arregloCaptura, tipoGuardar) {
    
    Captura = [];
    Captura[0] = {
        RequisicionID: 0,
        Requisicion: "",
        ProyectoID: 0,
        TipoPruebaID: 0,
        FechaRequisicion: "",
        //CodigoAsme: "",
        Observacion: "",
        Lenguaje: "",
        ListaDetalle: ""
    };
    ListaCaptura = [];

    var cont = 0;
    for (index = 0; index < arregloCaptura.length; index++) {
        if (arregloCaptura[index].Agregar == true || arregloCaptura[index].Accion == 3) {

            ListaCaptura[cont] = {
                RequisicionID: 0,
                ElementoPorClasificacionPNDID: 0,
                Accion: 0,
                ClasificacionPNDID: 0,
                OrdenTrabajoID: 0,
                SpoolID: 0,
                JuntaSpoolID: 0,
                ClasificacionManual: 0
            };

            ListaCaptura[cont].RequisicionID = $("#inputRequisicion").data("kendoComboBox").value() == "" ? 0 : $("#inputRequisicion").data("kendoComboBox").value();
            ListaCaptura[cont].ElementoPorClasificacionPNDID = arregloCaptura[index].ElementoPorClasificacionPNDID;
            ListaCaptura[cont].Accion = arregloCaptura[index].RequisicionID == 0? 1: arregloCaptura[index].Accion;
            ListaCaptura[cont].OrdenTrabajoID = arregloCaptura[index].OrdenTrabajoID;
            ListaCaptura[cont].ClasificacionPNDID = arregloCaptura[index].ClasificacionPNDID;
            ListaCaptura[cont].SpoolID = arregloCaptura[index].SpoolID;
            ListaCaptura[cont].JuntaSpoolID = arregloCaptura[index].JuntaSpoolID;
            ListaCaptura[cont].ClasificacionManual = arregloCaptura[index].ClasificacionManual;

            cont++;
        }

    }

    if (ListaCaptura.length != 0) {
        var Requisicion = $("#inputRequisicion").data("kendoComboBox").dataItem($("#inputRequisicion").data("kendoComboBox").select());
        Captura[0].RequisicionID = Requisicion.RequisicionID;
        Captura[0].Requisicion = Requisicion.NombreRequisicion;
        Captura[0].ProyectoID = $("#inputProyecto").data("kendoComboBox").value();
        Captura[0].TipoPruebaID = $("#inputTipoPrueba").data("kendoComboBox").value();
        Captura[0].FolioCliente = Requisicion.FolioCliente == null ? "" : Requisicion.FolioCliente;
        Captura[0].Observacion = Requisicion.Observacion;
        Captura[0].Lenguaje = $("#language").val();
        Captura[0].FechaRequisicion = Requisicion.FechaRequisicion;

        Captura[0].ListaDetalle = ListaCaptura;

        

        $RequisicionPND.RequisicionPND.create(Captura[0], { token: Cookies.get("token") }).done(function (data) {
            if (Error(data)) {
                if (data.ReturnMessage.length > 0 && data.ReturnMessage[0] == "Ok") {
                    if (data.ReturnMessage[1] != undefined) {
                        if (tipoGuardar) {
                            Limpiar();
                            opcionHabilitarView(false, "FieldSetView");
                        }
                        else {
                            //$('input[name="Muestra"][value="SinCaptura"]').prop('checked', true);
                            AjaxCargaDetalleRequisicion(Captura[0].RequisicionID, Captura[0].TipoPruebaID, Captura[0].ProyectoID,1);
                            opcionHabilitarView(true, "FieldSetView");
                        }

                        displayNotify("EntregaPlacasGraficasMensajeGuardadoExistoso", "", "0");
                    }
                }
                else {
                    opcionHabilitarView(false, "FieldSetView");
                    mensaje = "La requisición: " + Captura[0].Requisicion + " ya existe, por favor asigne otro nombre";
                    displayNotify("", mensaje, '1');
                }
            }
        });
    }
}

function AjaxEliminaRequisicion(itemRequisicion) {
    var Requisicion = {
        RequisicionID: itemRequisicion.RequisicionID,
        TipoPruebaID: itemRequisicion.TipoPruebaID,
        ProyectoID: itemRequisicion.ProyectoID,
        NombreRequisicion: itemRequisicion.NombreRequisicion,
        CodigoAsme: itemRequisicion.FolioCliente,
        FechaRequisicion: itemRequisicion.FechaRequisicion,
        Observacion: itemRequisicion.Observacion
    }

    $EditarRequisicion.EditarRequisicion.update(Requisicion, { token: Cookies.get("token") }).done(function (data) {
        if (data.ReturnMessage.length > 0 && data.ReturnMessage[0] == "OK") {
            Limpiar();
            displayNotify("EditarRequisicionMensajeEliminadoCorrecto", "", "0");
        }
        else {
            displayNotify("MensajeGuardadoErroneo", "", '2');
        }
    });
}

function AjaxObtenerSpool() {
    loadingStart();
    $RequisicionPND.RequisicionPND.read({ token: Cookies.get("token"), lenguaje: $("#language").val(), IdOrdenTrabajo: $("#InputOrdenTrabajo").val(), OrdenTrabajoSpoolID: $("#InputID").val(), TipoPruebaID: $("#inputTipoPrueba").data("kendoComboBox").value() == "" ? 0 : $("#inputTipoPrueba").data("kendoComboBox").value(), ProyectoID: $("#inputProyecto").data("kendoComboBox").value() == "" ? 0 : $("#inputProyecto").data("kendoComboBox").value() }).done(function (data) {
        if (Error(data)) {
            if (data[0].ProyectoID == parseInt($("#inputProyecto").data("kendoComboBox").value())) {

                var ds = $("#grid").data("kendoGrid").dataSource;
                if (data.length > 0) {
                    $("#InputID").data("kendoComboBox").value("");
                    for (var i = 0; i < data.length; i++) {
                        ds.insert(0, data[i]);
                    }
                    ds.page(1);
                }
                ds.sync();
            }
            else {
                displayNotify("", "El elemento manual, debe ser del mismo proyecto", 1);
            }
            loadingStop();
        }
    });
};

function AjaxObtenerJunta() {
    loadingStart();
    $RequisicionPND.RequisicionPND.read({ token: Cookies.get("token"), lenguaje: $("#language").val(), IdOrdenTrabajo: $("#InputOrdenTrabajo").val(), OrdenTrabajoSpoolID: $("#InputID").val(), TipoPruebaID: $("#inputTipoPrueba").data("kendoComboBox").value() == "" ? 0 : $("#inputTipoPrueba").data("kendoComboBox").value(), ProyectoID: $("#inputProyecto").data("kendoComboBox").value() == "" ? 0 : $("#inputProyecto").data("kendoComboBox").value(), JuntaSpoolID: $("#Junta").val() }).done(function (data) {
        if (Error(data)) {
            if (data[0].ProyectoID == parseInt($("#inputProyecto").data("kendoComboBox").value())) {
                var ds = $("#grid").data("kendoGrid").dataSource;
                if (data.length > 0) {
                    $("#Junta").data("kendoComboBox").value("");
                    for (var i = 0; i < data.length; i++) {
                        ds.insert(0, data[i]);

                    }
                    ds.page(1);
                }
                ds.sync();
            }
            else {
                displayNotify("", "El elemento manual, debe ser del mismo proyecto", 1);
            }
            loadingStop();
        }
    });
};

function AjaxJunta(spoolID) {
    loadingStart();
    $EditarRequisicion.EditarRequisicion.read({ ordenTrabajo: $("#InputOrdenTrabajo").val(), id: spoolID, sinCaptura: $('input:radio[name=Muestra]:checked').val(), token: Cookies.get("token") }).done(function (data) {
        if (Error(data)) {
            $("#Junta").data("kendoComboBox").value("");
            $("#Junta").data("kendoComboBox").dataSource.data(data);
            loadingStop();
        }
    });
};