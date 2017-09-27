var OrdenCompraAnterior;

function SuscribirEventos() {
    suscribirEventoGuardar();
    suscribirEventoComboOrdenCompra();
    SuscribirEventoMostrar();
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
