var ventanaConfirm;

function suscribirEventos() {
    suscribirEventoGuardar();
    mostrarConfirmacionVentanaModal();
};

function mostrarConfirmacionVentanaModal() {
    ventanaConfirm = $("#ventanaConfirm").kendoWindow({
        iframe: true,
        visible: false, //the window will not appear before its .open method is called
        width: "auto",
        height: "auto",
        modal: true,
        actions: [],
        animation: {
            close: false,
            open: false
        }
    }).data("kendoWindow");

    ventanaConfirm.content(_dictionary.MensajeConfirmacionGuardadoGeneral[$("#language").data("kendoDropDownList").value()] +
        "</br><center><button class='btn btn-blue' id='yesButton'>" + _dictionary.lblSi[$("#language").data("kendoDropDownList").value()] + "</button><button class='btn btn-blue' id='noButton'> " + _dictionary.lblNo[$("#language").data("kendoDropDownList").value()] + "</button></center>");

    $("#yesButton").click(function () {
        loadingStart();
        var ds = $("#grid").data("kendoGrid").dataSource;
        ArregloGuardado = [];
        var indice = 0;
        for (var i = 0; i < ds._data.length; i++) {
            if (ds._data[i].RowOk == false) {
                ArregloGuardado[indice] = ds._data[i];
                indice++;
            }
        }

        if (ArregloGuardado.length > 0) {
            AjaxGuardar(ArregloGuardado, 0);
        }
        else {
            loadingStop();
            displayNotify("AdverteciaExcepcionGuardado", "", '1');
        }

        ventanaConfirm.close();
    });


    $("#noButton").click(function () {
        ventanaConfirm.close();
    });
}
function suscribirEventoGuardar() {
    $("#Guardar,#btnGuardar, #GuardarPie, #btnGuardarPie").click(function () {
        if ($('#Guardar').text() == "Guardar" || $('#Guardar').text() == "Save") {
            var ds = $("#grid").data("kendoGrid").dataSource;
            if (ds._data.length > 0) {
                if (!ValidarValoresRepetidos(ds._data) && !ValoresBlanco(ds._data))
                    // alert("exito");
                    AjaxGuardar(ds._data, 0);
                else {

                    ventanaConfirm.setOptions({
                        title: _dictionary.TituloPopUpError[$("#language").data("kendoDropDownList").value()]
                    });

                    ventanaConfirm.open().center();
                }
            }
            else {
                displayNotify("AdverteciaExcepcionGuardado", "", '2');
            }
        }
        else if ($('#Guardar').text() == "Editar" || $('#Guardar').text() == "Edit") {
            opcionHabilitarView(false, "FieldSetView")
        }
    });


};


function eliminarCaptura(e) {
    e.preventDefault();
    if ($('#botonGuardar').text() == _dictionary.DetalleAvisoLlegada0017[$("#language").data("kendoDropDownList").value()]) {

        var filterValue = $(e.currentTarget).val();
        var dataItem = $("#grid").data("kendoGrid").dataItem($(e.currentTarget).closest("tr"));

        AjaxValidarReductorAsignado(dataItem.ReductorID, dataItem.Lote,dataItem.Cantidad, dataItem.Accion, dataItem);

      

       
    }
};

function limpiarCaptura(e) {
    e.preventDefault();

    if ($('#botonGuardar').text() == _dictionary.DetalleAvisoLlegada0017[$("#language").data("kendoDropDownList").value()]) {
        var itemToClean = $("#grid").data("kendoGrid").dataItem($(e.currentTarget).closest("tr"));
        itemToClean.Reductor = "";
        itemToClean.ReductorID = 0;
        itemToClean.Lote = "";
        itemToClean.Cantidad = 0;
        itemToClean.Unidad = "";
        if (itemToClean.Accion == 2)
            itemToClean.Accion = 4;

        var dataSource = $("#grid").data("kendoGrid").dataSource;
        dataSource.sync();
    }
};