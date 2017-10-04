var SpoolContiene = "";
var ProyectoIDAnterior = 0;
var TipoMuestraPredeterminadoID = 3052;

function AjaxCargarCamposPredeterminados() {
    $CamposPredeterminados.CamposPredeterminados.read({ token: Cookies.get("token"), lenguaje: $("#language").val(), id: TipoMuestraPredeterminadoID }).done(function (data) {
        if (Error(data)) {
            if (data == "sin captura") {
                $('input[name="Muestra"][value="SinCaptura"]').prop('checked', true);
            }
            else if (data == "Todos") {
                $('input[name="Muestra"][value="Todos"]').prop('checked', true);
            }
        }
        loadingStop();
    });
    AjaxGetListaProyectos();
};

function AjaxGetListaProyectos() {
    $Proyectos.Proyectos.read({ token: Cookies.get("token") }).done(function (data) {
        if (Error(data)) {
            $("#inputProyecto").data("kendoComboBox").dataSource.data(data);
            if ($("#inputProyecto").data("kendoComboBox").dataSource._data.length == 2) {
                $("#inputProyecto").data("kendoComboBox").select(1);
                $("#inputProyecto").data("kendoComboBox").trigger("change");
                ProyectoIDAnterior = data[1].ProyectoID;
            }
            else {
                ProyectoIDAnterior = 0;
            }
        }
    });
}

function AjaxGetNumeroElementos(ProyectoID, NumControl) {
    $OkPintura.OKPintura.read({ token: Cookies.get("token"), ProyectoID: ProyectoID, NumControl: NumControl, Muestra: $('input:radio[name=Muestra]:checked').val() }).done(function (data) {
        //if (Error(data)) {            
        if (data > 100) {
            var ventanaConfirmBusqueda = $("#ventanaConfirm").kendoWindow({
                iframe: true,
                title: _dictionary.TituloPopupCancelar[$("#language").data("kendoDropDownList").value()],
                visible: false,
                width: "45%",
                height: "auto",
                draggable: false,
                actions: [],
                modal: true,
                animation: {
                    close: true,
                    open: false
                }
            }).data("kendoWindow");

            ventanaConfirmBusqueda.content('<center>' + _dictionary.EmbarqueAlertaCantidadRegistros[$("#language").data("kendoDropDownList").value()] + '</center>' +
                "</br><center><button class='btn btn-blue' id='btnContinuarBusqueda'>" + _dictionary.lblSi[$("#language").data("kendoDropDownList").value()] + "</button> <button class='btn btn-blue' id='btnCancelarBusqueda'>" + _dictionary.lblNo[$("#language").data("kendoDropDownList").value()] + "</button></center>");

            ventanaConfirmBusqueda.open().center();
            $("#btnContinuarBusqueda").click(function (e) {
                ventanaConfirmBusqueda.close();
                AjaxGetListaElementos(ProyectoID, NumControl);
            });

            $("#btnCancelarBusqueda").click(function () {
                ventanaConfirmBusqueda.close();
                loadingStop();
            });
        } else {
            AjaxGetListaElementos(ProyectoID, NumControl);
        }
        //}        
    });
}

function AjaxCheckNumControl(ProyectoID, NumControl) {
    loadingStart();    
    $OkPintura.OKPintura.read({ token: Cookies.get("token"), ProyectoID: ProyectoID, NumControl: NumControl }).done(function (data) {        
        if (data === 1) {
            //displayNotify("", "Numero de control valido", "0");
            AjaxGetNumeroElementos(ProyectoID, NumControl);
        } else {            
            displayNotify("EmbarqueCargaMsjErrorSpoolAgregarProyectoIncorrecto", "", "1");
            $("#grid").data("kendoGrid").dataSource.data([]);
        }         
    });
}

function AjaxGetListaElementos(ProyectoID, NumControl) {
    loadingStart();
    SpoolContiene = NumControl;
    var totalRegistros = 0;
    $OkPintura.OKPintura.read({ token: Cookies.get("token"), Lenguaje: $("#language").val(), ProyectoID: ProyectoID, NumControl: NumControl, Muestra: $('input:radio[name=Muestra]:checked').val()}).done(function (data) {
        try {
            $("#grid").data("kendoGrid").dataSource.data([]);
            var dataSource = $("#grid").data("kendoGrid").dataSource;
            if (Error(data)) {
                $("#InputNumeroControl").val(SpoolContiene.toString().toUpperCase());
                if (data.length > 0) {
                    for (var i = 0; i < data.length; i++) {
                        dataSource.add(data[i]);
                    }
                } else {
                    displayNotify("NODATA", "", "1");
                    $("#grid").data("kendoGrid").dataSource.data([]);
                }                
            }
            loadingStop();
        } catch (e) {
            displayNotify("", "Error: " + e.message, "2");
        }
    });
}

function AjaxGuardarCaptura(arregloCaptura, tipoGuardado) {
    var proyectoID = $("#inputProyecto").data("kendoComboBox").value();
    Captura = [];
    Captura[0] = { Detalle: "" };
    ListaCaptura = [];
    var cont = 0;
    for (var i = 0; i < arregloCaptura.length; i++) {
        ListaCaptura[cont] = {            
            SpoolID: 0,
            OrdenTrabajoSpoolID: 0,
            OkPintura: 0,
        }        
        ListaCaptura[cont].SpoolID = arregloCaptura[i].SpoolID;
        ListaCaptura[cont].OrdenTrabajoSpoolID = arregloCaptura[i].OrdenTrabajoSpoolID;
        ListaCaptura[cont].OkPintura = arregloCaptura[i].OkPintura;
        cont++;
    }

    Captura[0].Detalle = ListaCaptura;
    var ds = $("#grid").data("kendoGrid").dataSource;
    if (ds._data.length > 0) {
        if (Captura[0].Detalle.length > 0) {
            $("#InputNumeroControl").val(SpoolContiene);
            $OkPintura.OKPintura.create(Captura[0], { token: Cookies.get("token"), Lenguaje: $("#language").val(), ProyectoID: proyectoID }).done(function (data) {
                if (Error(data)) {
                    if (data.ReturnMessage.length > 0 && data.ReturnMessage[0] == "OK") {
                        if (data.ReturnMessage[0] != undefined) {
                            if (tipoGuardado == 1) {
                                Limpiar();
                                opcionHabilitarView(false, "FieldSetView");
                                $("#inputProyecto").data("kendoComboBox").value("");
                            }
                            else {                                
                                AjaxGetListaElementos($("#inputProyecto").data("kendoComboBox").value(), $("#InputNumeroControl").val(), $('input:radio[name=Muestra]:checked').val());
                                opcionHabilitarView(true, "FieldSetView");
                            }
                            displayNotify("MensajeGuardadoExistoso", "", "0");
                        }
                    } else {
                        opcionHabilitarView(false, "FieldSetView");
                    }
                }
            });
        } else {
            displayNotify("EditarRequisicionExcepcionGuardado", "", "1");
        }
    } else {
        displayNotify("EditarRequisicionExcepcionGuardado", "", "1");
    }
}

function AjaxGuardadoMasivo(data) {
    var Correcto = false;
    CapturaMasiva = [];
    CapturaMasiva[0] = { Detalle: "" };
    CapturaMasiva[0].Detalle = JSON.stringify(data);
    var proyectoID = $("#inputProyecto").data("kendoComboBox").value();    
    $OkPintura.OKPintura.create(CapturaMasiva[0], { Lenguaje: $("#language").val(), token: Cookies.get("token"), ProyectoID: proyectoID, Flag: true }).done(function (data) {
        if (Error(data)) {
            download(data, "ResultadoCargaMasiva.csv", "text/csv");
            if (!$("#InputNumeroControl").val().length > 0 && !$("#grid").data("kendoGrid").dataSource.data().length > 0) {
                $("#grid").data("kendoGrid").dataSource.data([]);
                $("#InputNumeroControl").val("");
            }
            displayNotify("MensajeGuardadoExistoso", "", "0");
            Correcto = true;
        }
    });
    return Correcto;
};

function AjaxObtenerPruebas(SpoolID, ProcesoID) {
    try {        
        if (SpoolID != 0 && ProcesoID != 0) {
            $OkPintura.OKPintura.read({ token: Cookies.get("token"), Lenguaje: $("#language").val(), SpoolID: SpoolID, ProcesoID: ProcesoID }).done(function (data) {
                if (Error) {                    
                    LlenarGridPopUp(data);
                }
            });
        }
    } catch (e) {
        displayNotify("", "Error: " + e.message, "2");
        return;
    }
}