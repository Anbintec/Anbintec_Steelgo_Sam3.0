function AjaxDetalleGridReductores() {
    loadingStart();

    $AdminReductores.AdminReductores.read({ token: Cookies.get("token"), lenguaje: $("#language").val() }).done(function (array) {
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

function AjaxObtenerCatalogoReductores() {
    loadingStart();

    $AdminReductores.AdminReductores.read({ token: Cookies.get("token"), lenguaje: $("#language").val(), bandera: 1 }).done(function (array) {
        if (Error(array)) {
            ListaCatalogoReductores = array;
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
        ListaDetalles[index] = { ReductorID: "", Lote: "", Cantidad: "", Accion: "", AdminReductoresID: "" };
        ListaDetalles[index].ReductorID = arregloCaptura[index].ReductorID;
        ListaDetalles[index].Lote = arregloCaptura[index].Lote;
        ListaDetalles[index].Cantidad = arregloCaptura[index].Cantidad;
        ListaDetalles[index].Accion = arregloCaptura[index].Accion;
        ListaDetalles[index].AdminReductoresID = arregloCaptura[index].AdminReductoresID;
    }
    Captura[0].Detalles = ListaDetalles;

    $AdminReductores.AdminReductores.create(Captura[0], { token: Cookies.get("token"), lenguaje: $("#language").val() }).done(function (data) {
        if (Error(data)) {
            if (data.ReturnMessage.length > 0 && data.ReturnMessage[0] == "Ok") {
                $("#grid").data("kendoGrid").dataSource.data([]);
                displayNotify("MensajeGuardadoExistoso", "", '0');
                opcionHabilitarView(true, "FieldSetView");
                AjaxDetalleGridReductores();
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

function AjaxValidarReductorAsignado(reductorID, Lote, Cantidad, Accion, dataItem) {
    if (Accion != 0) {
        $AdminReductores.AdminReductores.read({ token: Cookies.get("token"), ReductorID: reductorID, Lote: Lote, Cantidad: Cantidad }).done(function (data) {
            if (data.ReturnMessage.length > 0 && data.ReturnMessage[1] == "No") {
                var dataSource = $("#grid").data("kendoGrid").dataSource;

                if (dataItem.Accion == 0 || dataItem.Accion == undefined)
                    dataSource.remove(dataItem);
                else
                    dataItem.Accion = 3;

                dataSource.sync();
            }
            else {
                displayNotify("AdminReductorAsignado", "", '2');
            }
        });
    }
    else {
        var dataSource = $("#grid").data("kendoGrid").dataSource;
        dataSource.remove(dataItem);
    }

}