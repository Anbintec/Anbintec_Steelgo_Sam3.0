function AjaxObtenerJSONGrid() {
    $SoldadorCertificacion.SoldadorCertificacion.read({ TipoDato: 1, token: Cookies.get("token"), Lenguaje: $("#language").val(), proyectoID: 31, patioID: 1 }).done(function (data) {
        if (Error(data)) {
            resultadoJson = data;
            if (resultadoJson.length > 0) {
                $("#grid").data("kendoGrid").dataSource.data([]);
                $("#grid").data("kendoGrid").dataSource.data(resultadoJson);
            } else {
                $("#grid").data("kendoGrid").dataSource.data([]);
            };
        }
    });
}


function AjaxGuardarInformacion(detalle) {
    loadingStart();
    $SoldadorCertificacion.SoldadorCertificacion.create(detalle, { token: Cookies.get("token"), Lenguaje: $("#language").val(), TipoCaptura: 2 }).done(function (data) {
        if (Error(data)) {
            if (data.ReturnMessage.length > 0 && data.ReturnMessage[0] == "Ok") {
                displayNotify("CapturaMensajeGuardadoExitoso", "", '0');
                AjaxObtenerJSONGrid();
                opcionHabilitarView(true, "FieldSetView")
                loadingStop();
            }
            else {
                displayNotify("CapturaMensajeGuardadoErroneo", "", '2');
                loadingStop();
            }

        }
        loadingStop();
    });
}