function suscribirEventos() {
    suscribirEventoGuardar();
}

suscribirEventos();

function suscribirEventoGuardar() {
    $('.accionGuardar').click(function (e) {
        if ($('#Guardar').text() == "Guardar" || $('#Guardar').text() == "Save") {
            AjaxExistePQR(0);
        }
        else if ($('#Guardar').text() == "Editar" || $('#Guardar').text() == "Edit") {
            opcionHabilitarView(false);
        }
    });

    $('#btnGuardarYNuevo').click(function (e) {
        opcionHabilitarView(false);
        AjaxExistePQR(1);

    });

    $('#btnGuardarYNuevo1').click(function (e) {
        opcionHabilitarView(false);
        AjaxExistePQR(1);

    });
}

function opcionHabilitarView(valor, name) {
    if (valor) {
        $('#FieldSetView').find('*').attr('disabled', true);
        $("#NombreId").attr('disabled', true);
        $("#chkPreheat").prop('disabled', true);
        $("#chkPwht").prop('disabled', true);
        $("#EspesorRelleno").data("kendoNumericTextBox").readonly(true);
        $("#EspesorRelleno").data("kendoNumericTextBox").enable(false);
        $("#EspesorRelleno").data("kendoNumericTextBox").wrapper.find(".k-numeric-wrap").find(".k-select").hide();
        $("#EspesorRaiz").data("kendoNumericTextBox").enable(false);
        $("#EspesorRaiz").data("kendoNumericTextBox").wrapper.find(".k-numeric-wrap").find(".k-select").hide();
        $("#ProcesoSoldaduraRellenoID").data("kendoComboBox").enable(false);
        $("#ProcesoSoldaduraRaizID").data("kendoComboBox").enable(false);
        $("#GrupoPMaterialBase1ID").data("kendoComboBox").enable(false);
        $("#GrupoPMaterialBase2ID").data("kendoComboBox").enable(false);
        $("#AporteID").attr('disabled', true);
        $("#MezclaID").attr('disabled', true);
        $("#RespaldoID").attr('disabled', true);
        $("#GrupoFID").attr('disabled', true);
        $("#CodigoID").data("kendoComboBox").enable(false);
        $('#Guardar1').text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);
        $("#Guardar").text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);
        $("#botonGuardar4").text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);
        $("#botonGuardar2").text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);
    }
    else {
        $('#FieldSetView').find('*').attr('disabled', false);
        $("#NombreId").attr('disabled', false);
        $("#chkPreheat").prop('disabled', false);
        $("#chkPwht").prop('disabled', false);
        $("#EspesorRelleno").data("kendoNumericTextBox").readonly(false);
        $("#EspesorRelleno").data("kendoNumericTextBox").enable(true);
        $("#EspesorRelleno").data("kendoNumericTextBox").wrapper.find(".k-numeric-wrap").find(".k-select").show();
        $("#EspesorRaiz").data("kendoNumericTextBox").readonly(false);
        $("#EspesorRaiz").data("kendoNumericTextBox").enable(true);
        $("#EspesorRaiz").data("kendoNumericTextBox").wrapper.find(".k-numeric-wrap").find(".k-select").show();
        $("#ProcesoSoldaduraRellenoID").data("kendoComboBox").enable(true);
        $("#ProcesoSoldaduraRaizID").data("kendoComboBox").enable(true);
        $("#GrupoPMaterialBase1ID").data("kendoComboBox").enable(true);
        $("#GrupoPMaterialBase2ID").data("kendoComboBox").enable(true);
        $("#AporteID").attr('disabled', false);
        $("#MezclaID").attr('disabled', false);
        $("#RespaldoID").attr('disabled', false);
        $("#GrupoFID").attr('disabled', false);
        $("#CodigoID").data("kendoComboBox").enable(true);
        $('#Guardar1').text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
        $("#Guardar").text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
        $("#botonGuardar4").text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
        $("#botonGuardar2").text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
    }
}