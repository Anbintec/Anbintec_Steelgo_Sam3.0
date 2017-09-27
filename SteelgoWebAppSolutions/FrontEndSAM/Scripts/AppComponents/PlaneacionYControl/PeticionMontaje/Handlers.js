function SuscribirEventos() {
    suscribirEventoProyecto();
    suscribirEventoGuardar();
}

SuscribirEventos();


function suscribirEventoGuardar() {

    $('.accionGuardar').click(function (e) {
        var ds = $("#grid").data("kendoGrid").dataSource;
        if (ds._data.length > 0) {
            if ($('#Guardar').text() == "Guardar" || $('#Guardar').text() == "Save") {
                //opcionHabilitarView(true, "FieldSetView");
                AjaxGuardarCaptura(ds._data);
            }
            else if ($('#Guardar').text() == "Editar" || $('#Guardar').text() == "Edit") {
                //opcionHabilitarView(false, "FieldSetView")
            }
        }
    });


}

function suscribirEventoProyecto() {
    $("#Proyecto").kendoComboBox({
        dataTextField: "Proyecto",
        dataValueField: "ProyectoID",
        suggest: true,
        filter: "contains",
        index: 3,
        change: function (e) {
            if ($("#Proyecto").data("kendoComboBox").dataItem($("#Proyecto").data("kendoComboBox").select()) != undefined) {

            }
            else {
                $("#Proyecto").data("kendoComboBox").value("");
            }

        }
    });
}


