function AjaxDetalleGridComponentes() {
    loadingStart();

    $AdminComponentes.AdminComponentes.read({ token: Cookies.get("token"), lenguaje: $("#language").val() }).done(function (array) {
        if (Error(array)) {
            var ds = $("#grid").data("kendoGrid").dataSource;
            for (var i = 0; i < array.length; i++) {
                ds.add(array[i]);
            }
            $("#grid").data("kendoGrid").dataSource.sync();
        }
        loadingStop();
    });
};

function AjaxObtenerCatalogoComponentes() {
    loadingStart();

    $AdminComponentes.AdminComponentes.read({ token: Cookies.get("token"), lenguaje: $("#language").val(), bandera: 1 }).done(function (array) {
        if (Error(array)) {
            ListaCatalogoComponentes = array;
        }
        loadingStop();
    });
};

function AjaxGuardar(arregloCaptura, tipoGuardar) {
    loadingStart();

    // var arregloCaptura= $("#grid").data("kendoGrid").dataSource._data;
    Captura = [];
    Captura[0] = { Detalles: "" };
    ListaDetalles = [];
    for (index = 0; index < arregloCaptura.length; index++) {
        ListaDetalles[index] = { ComponenteID: "", Lote: "", Cantidad: "", Accion: "", AdminComponentesID: "" };
        ListaDetalles[index].ComponenteID = arregloCaptura[index].ComponenteID;
        ListaDetalles[index].Lote = arregloCaptura[index].Lote;
        ListaDetalles[index].Cantidad = arregloCaptura[index].Cantidad;
        ListaDetalles[index].Accion = arregloCaptura[index].Accion;
        ListaDetalles[index].AdminComponentesID = arregloCaptura[index].AdminComponentesID;
    }
    Captura[0].Detalles = ListaDetalles;

    $AdminComponentes.AdminComponentes.create(Captura[0], { token: Cookies.get("token"), lenguaje: $("#language").val() }).done(function (data) {
        if (Error(data)) {
            if (data.ReturnMessage.length > 0 && data.ReturnMessage[0] == "Ok") {
                $("#grid").data("kendoGrid").dataSource.data([]);
                displayNotify("MensajeGuardadoExistoso", "", '0');
                opcionHabilitarView(true, "FieldSetView");
                AjaxDetalleGridComponentes();
                loadingStop();
            }
            else {
                //mensaje = "No se guardo la informacion el error es: " + data.ReturnMessage[0] + "-2";
                displayNotify("MensajeGuardadoErroneo", "", '2');
                loadingStop();
            }
        }
        loadingStop();
    });

}

function AjaxValidarComponenteAsignado(ComponenteID, Lote, Cantidad, Accion, dataitem) {
    if (Accion != 0) {
        $AdminComponentes.AdminComponentes.read({ token: Cookies.get("token"), ComponenteID: ComponenteID, Lote: Lote, Cantidad: Cantidad }).done(function (data) {
            if (data.ReturnMessage.length > 0 && data.ReturnMessage[1] == "No") {
                var dataSource = $("#grid").data("kendoGrid").dataSource;
                if (Accion == 0 || Accion == undefined)
                    dataSource.remove(dataitem);
                else
                    dataitem.Accion = 3;
                dataSource.sync();
            }
            else {
                displayNotify("AdminComponenteComponenteAsignado", "", '2');
            }
        });
    }
    else {
        var dataSource = $("#grid").data("kendoGrid").dataSource;
        dataSource.remove(dataitem);
    }
}
