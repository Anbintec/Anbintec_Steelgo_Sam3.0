var tiposCSV = ["application/csv", "application/excel", "application/lotus123", "application/msexcel", "application/vnd.lotus-1-2-3", "application/vnd.ms-excel", "application/vnd.ms-works", "application/vnd.msexcel", "application/wk1", "application/wks", "application/x-123", "application/x-dos_ms_excel", "application/x-excel", "application/x-lotus123", "application/x-ms-excel", "application/x-msexcel", "application/x-msworks", "application/x-wks", "application/x-xls", "application/xlc", "application/xls", "text/anytext", "text/comma-separated-values", "text/csv", "zz-application/zz-winassoc-wk1"];

var OrdenCompraAnterior;

function SuscribirEventos() {
    SuscribirEventoComboMoneda();
    SuscribirEventoComboCliente();
    SuscribirEventoGuardarCliente();
    SuscribirEventoCerrarPopupCliente();
    suscribirEventoGuardar();
    SuscribirEventoCargarCSV();
    suscribirEventoDescarGaCSV();
    suscribirEventoComboOrdenCompra();
    SuscribirEventoMostrar();
}

function SuscribirEventoComboMoneda() {
    $("#inputMoneda").kendoComboBox({        
        dataValueField: "MonedaID",
        dataTextField: "Moneda",
        dataSource: [],
        suggest: true,
        filter: "contains",
        index: 3,
    });
}

function SuscribirEventoGuardarCliente() {
    $("#GuardarCliente").click(function () {
        if ($('#btnGuardarSup1').text() == _dictionary.lblGuardar[$("#language").data("kendoDropDownList").value()]) {
            var Check = $("#CheckCerrar")[0].checked;
            if (!Check) {
                AjaxGuardarCliente();
            } else {
                displayNotify("lblRevisionOCCerrada", "", "1");
            }
        }
    });    
}

function SuscribirEventoCerrarPopupCliente() {
    $("#btnCerrarPopup").click(function (e) {
        e.preventDefault();
        $("#windowGrid").data("kendoWindow").close();
    });
}

function SuscribirEventoCargarCSV() {
    $("#btnCargaCsv, #btnCargaCsv1").click(function () {
        if ($('#btnGuardarSup1').text() == _dictionary.lblGuardar[$("#language").data("kendoDropDownList").value()]) {
            var OrdenCompraID = $("#inputOrdenCompra").data("kendoComboBox").value();
            if (OrdenCompraID != 0) {
                var Check = $("#CheckCerrar")[0].checked;
                $("#inputCliente").data("kendoComboBox").value("");
                if (!Check) {
                    $("#files").val("");
                    $("#files").click();
                } else {
                    displayNotify("lblRevisionOCCerrada", "", "1");
                }
            } else {
                displayNotify("msgFaltaOrdenCompra", "", "1");
            }            
        }
    });

    document.getElementById("files").addEventListener("change", function (evt) {
        if (!(window.File && window.FileReader && window.FileList && window.Blob)) {
            displayNotify("ListadoCatalogos0007", "", '2');
        } else {
            var data = [];
            var file = evt.target.files[0];
            var Cliente = "";
            try {
                if (tiposCSV.indexOf(file.type.toLowerCase()) == -1) {
                    this.value = null;
                    displayNotify("ListadoCatalogos0008", "", '2');
                } else {
                    var reader = new FileReader();
                    reader.readAsText(file);
                    reader.onload = function (event) {
                        var csvData = event.target.result;                        
                        Cliente = GetClienteByCSV(csvData);
                        var dataCliente = $("#inputCliente").data("kendoComboBox").dataSource._data;
                        var existe = 0;
                        var tmpClienteID = 0;
                        if (Cliente != "") {
                            for (var i = 0; i < dataCliente.length; i++) {
                                if (Cliente == dataCliente[i].Cliente) {
                                    existe++;
                                    tmpClienteID = dataCliente[i].ClienteID;
                                }
                            }
                            if (existe > 0) {
                                $("#inputCliente").data("kendoComboBox").value(tmpClienteID);
                            } else {
                                displayNotify("lblNoCoincideCliente", "", "1");
                                setTimeout(function () {
                                    VentanaModal(Cliente);
                                }, 900);

                            }
                        }                                            
                    };
                    reader.onerror = function () {
                        alert('Unable to read ' + file.fileName);
                    };
                }
            } catch (e) { }
        }
    });
}

function GetClienteByCSV(data) {
    var lineas = data.split(/\r?\n|\r/);
    var encabezado = lineas[0].split(",");
    var Cliente = "";
    if (encabezado[0] == "Cliente") {        
        Cliente = lineas[1] = lineas[1].split(",")[0];
    }
    return Cliente;
}


function suscribirEventoDescarGaCSV() {
    $("#btnDescargaCsv, #btnDescargaCsv1").click(function (e) {
        window.location.href = "/TemplateCliente.csv";
    });
}

function SuscribirEventoComboCliente() {
    $("#inputCliente").kendoComboBox({
        dataValueField: "ClienteID",
        dataTextField: "Cliente",
        dataSource: [],
        suggest: true,
        filter: "contains",
        index: 3,
    });
}


function suscribirEventoComboOrdenCompra() {
    $('#inputOrdenCompra').kendoComboBox({
        dataTextField: "Nombre",
        dataValueField: "OrdenCompraID",
        dataSource: [],
        suggest: true,
        filter: "contains",
        index: 3,
        change: function (e) {
            var dataItem = this.dataItem(e.sender.selectedIndex);
            var ds = $("#grid").data("kendoGrid").dataSource._data;

            if (cambiosCheckOK > 0) {
                var ventanaConfirm = $("#ventanaConfirmCaptura").kendoWindow({
                    iframe: true,
                    title: _dictionary.EntregaPlacasGraficasTituloPopup[$("#language").data("kendoDropDownList").value()],
                    visible: false,
                    animation: false,
                    width: "auto",
                    height: "auto",
                    actions: [],
                    modal: true
                }).data("kendoWindow");

                ventanaConfirm.content(_dictionary.EntregaPlacasGraficasMensajeDatosCapturadosNoGuardados[$("#language").data("kendoDropDownList").value()] +
                    "</br><center><button class='btn btn-blue' id='yesButtonProy'>" + _dictionary.EntregaPlacasGraficasbotonSi[$("#language").data("kendoDropDownList").value()] + "</button><button class='btn btn-blue' id='noButtonProy'>" + _dictionary.EntregaPlacasGraficasbotonNo[$("#language").data("kendoDropDownList").value()] + "</button></center>");

                ventanaConfirm.open().center();
                $("#yesButtonProy").click(function () {
                    if (dataItem.CerradaPago == 1) {
                        $('#inputCerrar').prop('checked', true);
                    }
                    else {
                        $('#inputCerrar').prop('checked', false);
                    }
                    ventanaConfirm.close();
                    OrdenCompraAnterior = $("#inputOrdenCompra").data("kendoComboBox").value();
                    $("#inputOrdenCompra").data("kendoComboBox").value(OrdenCompraAnterior);
                    $("#grid").data("kendoGrid").dataSource.data([]);
                    cambiosCheckOK = 0;
                });

                $("#noButtonProy").click(function () {
                    $("#inputOrdenCompra").data("kendoComboBox").value(OrdenCompraAnterior);
                    ventanaConfirm.close();
                });
            } else {
                OrdenCompraAnterior = $("#inputOrdenCompra").data("kendoComboBox").value();
                if (dataItem != null) {
                    if (dataItem.CerradaPago == 1) {
                        $('#inputCerrar').prop('checked', true);
                    }
                    else {
                        $('#inputCerrar').prop('checked', false);
                    }
                }                
            }
        }
    });
}


function SuscribirEventoMostrar() {
    $("#btnMostrar").click(function (e) {
        if (cambiosCheckOK > 0) {
            var ventanaConfirm = $("#ventanaConfirmCaptura").kendoWindow({
                iframe: true,
                title: _dictionary.EntregaPlacasGraficasTituloPopup[$("#language").data("kendoDropDownList").value()],
                visible: false,
                animation: false,
                width: "auto",
                height: "auto",
                actions: [],
                modal: true
            }).data("kendoWindow");

            ventanaConfirm.content(_dictionary.EntregaPlacasGraficasMensajeDatosCapturadosNoGuardados[$("#language").data("kendoDropDownList").value()] +
                "</br><center><button class='btn btn-blue' id='yesButtonProy'>" + _dictionary.EntregaPlacasGraficasbotonSi[$("#language").data("kendoDropDownList").value()] + "</button><button class='btn btn-blue' id='noButtonProy'>" + _dictionary.EntregaPlacasGraficasbotonNo[$("#language").data("kendoDropDownList").value()] + "</button></center>");

            ventanaConfirm.open().center();
            $("#yesButtonProy").click(function () {
                ventanaConfirm.close();
                OrdenCompraAnterior = $("#inputOrdenCompra").data("kendoComboBox").value();
                //Limpiar();
                if ($("#btnGuardarSup1").text() === _dictionary.lblGuardar[$("#language").val()]) {
                    var OrdenCompraID = $("#inputOrdenCompra").data("kendoComboBox").value();
                    if (OrdenCompraID != 0) {
                        AjaxCargarRevision();
                    } else {
                        displayNotify("msgFaltaOrdenCompra", "", '1');
                    }
                } else {
                    opcionHabilitarView(false);
                }
                cambiosCheckOK = 0;
            });

            $("#noButtonProy").click(function () {
                $("#inputOrdenCompra").data("kendoComboBox").value(OrdenCompraAnterior);
                ventanaConfirm.close();
            });
        } else {
            OrdenCompraAnterior = $("#inputOrdenCompra").data("kendoComboBox").value();
            if ($("#btnGuardarSup1").text() === _dictionary.lblGuardar[$("#language").val()]) {
                var OrdenCompraID = $("#inputOrdenCompra").data("kendoComboBox").value();
                if (OrdenCompraID != 0) {
                    AjaxCargarRevision();
                } else {
                    displayNotify("msgFaltaOrdenCompra", "", '1');
                }
            } else {
                opcionHabilitarView(false);
            }
        }
    });
}


function suscribirEventoGuardar() {
    $('#btnGuardarSup1, #btnGuardarInf1, #btnGuardarSup2, #btnGuardarInf2 ').click(function (e) {
        if ($("#btnGuardarSup1").text() === _dictionary.lblGuardar[$("#language").val()]) {
            var dataGrid = $("#grid").data("kendoGrid").dataSource._data;
            var cont = 0;
            if (dataGrid.length > 0) {
                AjaxGuardarCaptura(dataGrid, 0);
            } else {
                displayNotify("AdverteciaExcepcionGuardado", "", "1");
            }
        } else {
            opcionHabilitarView(false);
        }
    });

    $('#btnGuardarYNuevoSup, #btnGuardarYNuevoInf').click(function (e) {
        if ($("#btnGuardarSup1").text() === _dictionary.lblGuardar[$("#language").val()]) {
            var ds = $("#grid").data("kendoGrid").dataSource;
            if (ds._data.length > 0) {
                AjaxGuardarCaptura(ds._data, 1);
            }
            else {
                displayNotify("AdverteciaExcepcionGuardado", "", '1');
            }
        } else {
            opcionHabilitarView(false);
        }
    });
}



function opcionHabilitarView(valor, name) {
    if (valor) {
        $('#FieldSetView').find('*').attr('disabled', true);
        $("#inputOrdenCompra").data("kendoComboBox").enable(false);
        $('#btnGuardarSup1').text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);
        $('#btnGuardarInf1').text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);
        $('#btnGuardarSup2').text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);
        $('#btnGuardarInf2').text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);
        $('#btnGuardarYNuevoSup').text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);
        $('#btnGuardarYNuevoInf').text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);
    } else {
        $('#FieldSetView').find('*').attr('disabled', false);
        $("#inputOrdenCompra").data("kendoComboBox").enable(true);
        $('#btnGuardarSup1').text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
        $('#btnGuardarInf1').text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
        $('#btnGuardarSup2').text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
        $('#btnGuardarInf2').text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
        $('#btnGuardarYNuevoSup').text(_dictionary.botonGuardarYNuevo[$("#language").data("kendoDropDownList").value()]);
        $('#btnGuardarYNuevoInf').text(_dictionary.botonGuardarYNuevo[$("#language").data("kendoDropDownList").value()]);
    }
}

function SuscribirEventoAgregarCliente() {
    //$("#inputNombreCliente").data("kendoTextBox");
    $('#inputNombreCliente').kendoTextBox();        
}