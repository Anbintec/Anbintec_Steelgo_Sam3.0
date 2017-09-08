function SuscribirEventos() {
    suscribirEventoComboOrdenCompra();
    SuscribirEventoMostrar();
    suscribirEventoGuardar();
    suscribirEventoListadoPackingList();
}

function suscribirEventoGuardar() {
    $(".accionGuardar").click(function (e) {
        e.preventDefault();


        if ($('#botonGuardar').text() == "Guardar" || $('#botonGuardar').text() == "Save") {
            var ds = $("#grid").data("kendoGrid").dataSource;
            if (ds._data.length > 0) {
                if ($('#InputCamion').val().trim() != "" && $("#inputOrdenCompra").data("kendoComboBox").text() != '' && $("#inputOrdenCompra").data("kendoComboBox").value() != undefined) {
                    AjaxGuardarCaptura(ds._data, 0);
                }
                else
                    displayNotify("DynasolOrdenPedidoPackingMandatorio", "", "1");
            }
            else {
                displayNotify("AdverteciaExcepcionGuardado", "", '1');
            }
        }
        else if ($('#Guardar').text() == "Editar" || $('#Guardar').text() == "Edit") {
            opcionHabilitarView(false)
        }

    });

    $('.accionGuardarNuevo').click(function (e) {
        e.preventDefault();
        var ds = $("#grid").data("kendoGrid").dataSource;
        if (ds._data.length > 0) {
            if ($('#InputCamion').val().trim() != "" && $("#inputOrdenCompra").data("kendoComboBox").text() != '' && $("#inputOrdenCompra").data("kendoComboBox").value() != undefined) {
                AjaxGuardarCaptura(ds._data, 1);
            }
            else
                displayNotify("DynasolOrdenPedidoPackingMandatorio", "", "1");
        }
        else {
            displayNotify("AdverteciaExcepcionGuardado", "", '1');
        }
    });

}

function SuscribirEventoMostrar() {
    $('#btnMostrar').click(function (e) {
        if ($('#InputCamion').val().trim() != "" && $("#inputOrdenCompra").data("kendoComboBox").text() != '' && $("#inputOrdenCompra").data("kendoComboBox").value() != undefined) {
            AjaxCargarRevision();
        }
        else {
            displayNotify('DynasolOrdenPedidoPackingMandatorio', '', '1');
        }
    });
    //$('#InputCamion').closest('.k-widget').keydown(function (e) {
    $('#InputCamion').keydown(function (e) {
        if (e.keyCode === 13) {
            if ($('#InputCamion').val().trim() != "" && $("#inputOrdenCompra").data("kendoComboBox").text() != '' && $("#inputOrdenCompra").data("kendoComboBox").value() != undefined) {
                AjaxCargarRevision();
            }
            else {
                displayNotify('DynasolOrdenPedidoPackingMandatorio', '', '1');
            }
        }
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
            $("#grid").data("kendoGrid").dataSource.data([]);
            if (dataItem == undefined) {
                $("#inputOrdenCompra").data("kendoComboBox").value("");
            }
        }
    });
}


function opcionHabilitarView(valor, name) {

    if (valor) {
        $('#FieldSetView').find('*').attr('disabled', true);

        $("#inputOrdenCompra").data("kendoComboBox").enable(false);


        $('#botonGuardar2').text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);
        $("#botonGuardar").text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);
        $("#botonGuardar3").text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);
        $('#botonGuardar4').text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);

    }
    else {
        $('#FieldSetView').find('*').attr('disabled', false);

        $("#inputOrdenCompra").data("kendoComboBox").enable(true);


        $('#botonGuardar2').text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
        $("#botonGuardar").text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
        $("#botonGuardar3").text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
        $('#botonGuardar4').text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);


    }
}


function suscribirEventoListadoPackingList() {

    $(document).on('click', '.EnlaceDetalleColada', function (e) {

        if ($('#Guardar').text() == _dictionary.MensajeGuardar[$("#language").data("kendoDropDownList").value()]) {
            var grid = $("#grid").data("kendoGrid");
            dataItem = grid.dataItem($(e.target).closest("tr"));
            if (dataItem.ListadoCamion != "")
                displayNotify("DynasolOrdenPedidoCamionCantidad", dataItem.ListadoCamion, '1');
        }
    });
}