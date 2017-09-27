function suscribirEventos() {
    suscribirEventoGuardar();
}

suscribirEventos();

function suscribirEventoGuardar() {
    $('.accionGuardar').click(function (e) {
        var ds = $("#grid").data("kendoGrid").dataSource;
        if (ds._data.length > 0) {
            if ($('#Guardar').text() == "Guardar" || $('#Guardar').text() == "Save") {
                
                AjaxGuardarCaptura();
            }
            else if ($('#Guardar').text() == "Editar" || $('#Guardar').text() == "Edit") {
                opcionHabilitarView(false)
            }
        }
    });
}

function opcionHabilitarView(valor, name) {

    if (valor) {
        $('#FieldSetView').find('*').attr('disabled', true);
        $('#AgregarWPS').prop('disabled', true);
        $('#Guardar1').text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);
        $("#Guardar").text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);
        $("#botonGuardar4").text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);
        $("#botonGuardar2").text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);

    }
    else {
        $('#FieldSetView').find('*').attr('disabled', false);
        $('#AgregarWPS').prop('disabled', false);
        $('#Guardar1').text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
        $("#Guardar").text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
        $("#botonGuardar4").text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
        $("#botonGuardar2").text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
    }
}