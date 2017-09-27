var TipoMuestraPredeterminadoID = 3049;
var TipoMuestraPredeterminadoPlanchado = 3065;
var TipoMuestraPredeterminadoSelecTodos = 3062;

function AjaxCargarCamposPredeterminados() {
    $CamposPredeterminados.CamposPredeterminados.read({ token: Cookies.get("token"), lenguaje: $("#language").val(), id: TipoMuestraPredeterminadoID }).done(function (data) {
        if (Error(data)) {
            if (data == "sin captura") {
                $('input:radio[name=Muestra]:nth(0)').trigger("click");
            }
            else if (data == "todos") {
                $('input:radio[name=Muestra]:nth(1)').trigger("click");
            }
            loadingStop();
        }
    });
    $CamposPredeterminados.CamposPredeterminados.read({ token: Cookies.get("token"), lenguaje: $("#language").val(), id: TipoMuestraPredeterminadoSelecTodos }).done(function (data) {
        if (data == "Si") {
            $('input:radio[name=SelectTodos]:nth(0)').trigger("click");
        }
        else if (data == "No") {
            $('input:radio[name=SelectTodos]:nth(1)').trigger("click");
        }
        else if (data == "Ninguno") {
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

    AjaxGetListaProyectos();
};

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

function AjaxGetListaProyectos() {
    $Proyectos.Proyectos.read({ token: Cookies.get("token") }).done(function (data) {
        if (Error(data)) {
            //$("#divMostrar").css('display', 'block');
            $("#inputProyecto").data("kendoComboBox").dataSource.data(data);

            if ($("#inputProyecto").data("kendoComboBox").dataSource._data.length == 2) {
                $("#inputProyecto").data("kendoComboBox").select(1);
                $("#inputProyecto").data("kendoComboBox").trigger("change");
            }
        }
    });
}

function AjaxGetListaTiposDePrueba() {
    $ServiciosTecnicosGeneral.ServiciosTecnicosGeneral.read({ token: Cookies.get("token"), lenguaje: $("#language").val() }).done(function (data) {
        if (Error(data)) {
            //$("#divMostrar").css('display', 'block');
            $("#inputTipoPrueba").data("kendoComboBox").dataSource.data(data);

            if ($("#inputTipoPrueba").data("kendoComboBox").dataSource._data.length == 2) {
                $("#inputTipoPrueba").data("kendoComboBox").select(1);
                $("#inputTipoPrueba").data("kendoComboBox").trigger("change");
            }
        }
    });
}

function AjaxGetListaRequisiciones(proyectoID, tipoPruebaID) {
    $ServiciosTecnicosGeneral.ServiciosTecnicosGeneral.read({ token: Cookies.get("token"), ProyectoID: proyectoID, TipoPruebaID: tipoPruebaID, estatusID: 1, lenguaje: $("#language").val() }).done(function (data) {
        if (Error(data)) {

            $("#listaRequisiciones").data("kendoComboBox").dataSource.data(data);

            if ($("#listaRequisiciones").data("kendoComboBox").dataSource._data.length == 2) {
                $("#listaRequisiciones").data("kendoComboBox").select(1);
                $("#listaRequisiciones").data("kendoComboBox").trigger("change");
            }
        }
    });
}

function AjaxGetListaElementos(requisicionID, tipoPruebaID, proyectoID, muestra) {
    loadingStart();
    $RequisicionPND.RequisicionPND.read({ token: Cookies.get("token"), lenguaje: $("#language").val(), RequisicionID: requisicionID, TipoPruebaID: tipoPruebaID, ProyectoID: proyectoID, Muestra: muestra }).done(function (data) {
        if (Error(data)) {
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
        }
    });
}




function AjaxGuardarCaptura(arregloCaptura, tipoGuardar) {
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
        if (arregloCaptura[index].Agregar == true) {

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

            ListaCaptura[cont].RequisicionID = $("#listaRequisiciones").data("kendoComboBox").value() == "" ? 0 : $("#listaRequisiciones").data("kendoComboBox").value();
            ListaCaptura[cont].ElementoPorClasificacionPNDID = arregloCaptura[index].ElementoPorClasificacionPNDID;
            ListaCaptura[cont].Accion = arregloCaptura[index].RequisicionID > 0 ? 2 : 1;
            ListaCaptura[cont].OrdenTrabajoID = arregloCaptura[index].OrdenTrabajoID;
            ListaCaptura[cont].ClasificacionPNDID = arregloCaptura[index].ClasificacionPNDID;
            ListaCaptura[cont].SpoolID = arregloCaptura[index].SpoolID;
            ListaCaptura[cont].JuntaSpoolID = arregloCaptura[index].JuntaSpoolID;
            ListaCaptura[cont].ClasificacionManual = arregloCaptura[index].ClasificacionManual;

            cont++;
        }

    }

    if (ListaCaptura.length != 0) {
        Captura[0].RequisicionID = $("#listaRequisiciones").data("kendoComboBox").value() == "" ? 0 : $("#listaRequisiciones").data("kendoComboBox").value();
        Captura[0].Requisicion = "";
        Captura[0].ProyectoID = $("#inputProyecto").data("kendoComboBox").value();
        Captura[0].TipoPruebaID = $("#inputTipoPrueba").data("kendoComboBox").value() == "" ? 0 : $("#inputTipoPrueba").data("kendoComboBox").value();
        Captura[0].FolioCliente = "";
        Captura[0].Observacion = "";
        Captura[0].Lenguaje = $("#language").val();
        Captura[0].FechaRequisicion = "";

        Captura[0].ListaDetalle = ListaCaptura;

        loadingStart();

        var tipoPruebaID = $("#inputProyecto").data("kendoComboBox").value();

        loadingStop();

        var modalTitle = "";
        modalTitle = _dictionary.MensajeNuevaRequisicion[$("#language").data("kendoDropDownList").value()];
        var ventanaConfirm = $("#ventanaConfirm");
        var window = ventanaConfirm.kendoWindow({
            modal: true,
            title: modalTitle,
            resizable: false,
            visible: true,
            width: "50%",
            minWidth: 30,
            position: {
                top: "1%",
                left: "1%"
            },
            animation: false,
            actions: []

        }).data("kendoWindow");

        window.content('<div id="ventanaConfirm" z-index: inherit">' +
                            '<div class="col-sm-11 col-md-11 col-lg-11">' +
                                '<div class="form-group col-xs-12 col-sm-12 col-md-12 col-lg-12">' +
                                    '<label id=""><span>' + _dictionary.lblRequisicion1[$("#language").data("kendoDropDownList").value()] + '</span></label>' +
                                    '<input id="NombreRequisicion" class="form-control" />' +
                                '</div>' +
                                '<div class="form-group col-xs-12 col-sm-12 col-md-12 col-lg-12">' +
                                    '<label id=""><span>' + _dictionary.lblNumeroClienteRequisicion[$("#language").data("kendoDropDownList").value()] + '</span></label>' +
                                    '<input id="FolioCliente" class="form-control"  />' +
                                '</div>' +
                                '<div class="form-group col-xs-12 col-sm-12 col-md-12 col-lg-12">' +
                                    '<label id=""><span>' + _dictionary.lblFechaRequisicion[$("#language").data("kendoDropDownList").value()] + '</span></label>' +
                                    '<input id="FechaRequisicion" class="form-control"/>' +
                                '</div>' +

                                '<div class="form-group col-xs-12 col-sm-12 col-md-12 col-lg-12">' +
                                    '<label id=""><span>' + _dictionary.lblObservacion[$("#language").data("kendoDropDownList").value()] + '</span></label>' +
                                    '<input id="Observacion" class="form-control" />' +
                                '</div>' +
                                '<div class="form-group col-xs-12 col-sm-12 col-md-12 col-lg-12">' +
                                    '<center><button class="btn btn-blue" id="YesButton"> Guardar</button>&nbsp;<button class="btn btn-blue" id="NoButton"> Cancelar</button></center>' +
                                '</div>' +
                            '</div>' +
                        '</div>');

        ventanaConfirm.data("kendoWindow").title(modalTitle);

        if ($("#listaRequisiciones").data("kendoComboBox").value() != "" && $("#listaRequisiciones").data("kendoComboBox").value() != 0) {
            $("#NombreRequisicion").val($("#listaRequisiciones").data("kendoComboBox").dataItem($("#listaRequisiciones").data("kendoComboBox").select()).NombreRequisicion);
            $("#FechaRequisicion").val($("#listaRequisiciones").data("kendoComboBox").dataItem($("#listaRequisiciones").data("kendoComboBox").select()).FechaRequisicion);
            $("#Observacion").val($("#listaRequisiciones").data("kendoComboBox").dataItem($("#listaRequisiciones").data("kendoComboBox").select()).Observacion);
            $("#FolioCliente").val($("#listaRequisiciones").data("kendoComboBox").dataItem($("#listaRequisiciones").data("kendoComboBox").select()).FolioCliente);
        }
        else {
            var idFechaRequisicion = 2047;
            $CamposPredeterminados.CamposPredeterminados.read({ token: Cookies.get("token"), lenguaje: $("#language").val(), id: idFechaRequisicion }).done(function (data) {
                $("#FechaRequisicion").val(data);
            });
        }

        ventanaConfirm.data("kendoWindow").center().open();

        $("#YesButton").click(function (handler) {
            Captura[0].Requisicion = $("#NombreRequisicion").val();
            Captura[0].FolioCliente = $("#FolioCliente").val();
            Captura[0].Observacion = $("#Observacion").val();
            Captura[0].FechaRequisicion = $("#FechaRequisicion").val();

            $RequisicionPND.RequisicionPND.create(Captura[0], { token: Cookies.get("token") }).done(function (data) {
                if (Error(data)) {
                    if (data.ReturnMessage.length > 0 && data.ReturnMessage[0] == "Ok") {
                        if (data.ReturnMessage[1] != undefined) {
                            if (tipoGuardar == 1) {
                                Limpiar();
                                opcionHabilitarView(false, "FieldSetView");
                            }
                            else {
                                $('input[name="Muestra"][value="SinCaptura"]').prop('checked', true);
                                AjaxGetGuardado(data.ReturnMessage[1]);
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

            window.close();
        });
        $("#NoButton").click(function (handler) {
            window.close();
        });
    }
    else {
        displayNotify("", "Para guardar o modificar una requisición es necesario seleccionar al menos un elemento", "1");
    }
}

function AjaxGetGuardado(RequisicionID) {
    $ServiciosTecnicosGeneral.ServiciosTecnicosGeneral.read({ token: Cookies.get("token"), ProyectoID: $("#inputProyecto").data("kendoComboBox").value(), TipoPruebaID: $("#inputTipoPrueba").data("kendoComboBox").value(), estatusID: 1, lenguaje: $("#language").val() }).done(function (data) {
        if (Error(data)) {
            $("#listaRequisiciones").data("kendoComboBox").dataSource.data(data);

            $("#listaRequisiciones").data("kendoComboBox").value(RequisicionID);
            AjaxGetListaElementos(RequisicionID, $("#inputTipoPrueba").data("kendoComboBox").value(), $("#inputProyecto").data("kendoComboBox").value(), $('input:radio[name=Muestra]:checked').val());
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
            if (data[0].ProyectoID == parseInt($("#inputProyecto").data("kendoComboBox").value())){ 
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

    $RequisicionPND.RequisicionPND.read({ ordenTrabajo: $("#InputOrdenTrabajo").val(), id: spoolID, sinCaptura: $('input:radio[name=Muestra]:checked').val(), token: Cookies.get("token") }).done(function (data) {
        if (Error(data)) {

            $("#Junta").data("kendoComboBox").value("");
            $("#Junta").data("kendoComboBox").dataSource.data(data);

            loadingStop();
        }
    });
}