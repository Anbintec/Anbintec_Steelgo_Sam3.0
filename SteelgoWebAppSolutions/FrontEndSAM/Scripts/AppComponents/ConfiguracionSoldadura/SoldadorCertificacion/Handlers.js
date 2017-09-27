function SuscribirEventos() {
    suscribirEventoGuardar();
}


function suscribirEventoGuardar() {

    $('.accionGuardar').click(function (e) {
        var ds = $("#grid").data("kendoGrid").dataSource;
        if (ds._data.length > 0) {
            if ($('#Guardar').text() == _dictionary.DetalleAvisoLlegada0017[$("#language").data("kendoDropDownList").value()]) {
                ValidarInformacionEnviada();
            }
            else {
                opcionHabilitarView(false, "FieldSetView");
            }
        }
    });
}




function opcionHabilitarView(valor, name) {
    if (valor) {
        $('#buttonNuevoSoldador').find('*').attr('disabled', true);
        $('#Guardar1').text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);
        $("#Guardar").text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);
        $("#botonGuardar4").text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);
        $("#botonGuardar2").text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);
    }
    else {
        $('#buttonNuevoSoldador').find('*').attr('disabled', false);
        $('#Guardar1').text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
        $("#Guardar").text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
        $("#botonGuardar4").text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
        $("#botonGuardar2").text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
    }
}