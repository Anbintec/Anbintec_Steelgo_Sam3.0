function AjaxObtenerOrdenesCompra() {
    loadingStart();
    $Dynasol.Dynasol.read({ token: Cookies.get("token") }).done(function (data) {
        if (Error(data)) {
            $("#inputOrdenCompra").data("kendoComboBox").setDataSource([]);
            $("#inputOrdenCompra").data("kendoComboBox").text("");
            $("#inputOrdenCompra").data("kendoComboBox").dataSource.data(data);
            if ($("#inputOrdenCompra").data("kendoComboBox").dataSource._data.length == 2) {
                $("#inputOrdenCompra").data("kendoComboBox").select(1);
                AjaxCargarRevision();
            }
            else {
                $("#inputOrdenCompra").data("kendoComboBox").select(0);
                loadingStop();
            }
        }
    });
}

function AjaxCargarRevision() {
    loadingStart();
    var OrdenCompra = $("#inputOrdenCompra").data("kendoComboBox").value() == undefined ? 0 : $("#inputOrdenCompra").data("kendoComboBox").value();
    $CompraPagoOC.CompraPagoOC.read({ token: Cookies.get("token"), OrdenCompraID: OrdenCompra }).done(function (data) {
        if (Error(data)) {
            $("#grid").data("kendoGrid").dataSource.data([]);
            var ds = $("#grid").data("kendoGrid").dataSource;
            var array = data;
            if (data.length > 0) {
                ds.data(data);
            }
            ds.sync();
        }
        loadingStop();
    });
}

function AjaxGuardarCaptura(arregloCaptura, tipoGuardar) {
    loadingStart();
    var Captura = { Detalle: "" };
    var Datos = [];
    for (var i = 0; i < arregloCaptura.length; i++) {
        Datos[i] =
        {
            ColadaID: 0,            
            CantC: 0,
            CantP: 0,
            ModificadoPorUsuario: false
        }
        Datos[i].ColadaID = arregloCaptura[i].ColadaID;
        Datos[i].CantC = arregloCaptura[i].CantC;
        Datos[i].CantP = arregloCaptura[i].CantP;
        Datos[i].ModificadoPorUsuario = arregloCaptura[i].ModificadoPorUsuario;
    }
    Captura.Detalle = Datos;

    $CompraPagoOC.CompraPagoOC.create(Captura, { token: Cookies.get("token") }).done(function (data) {
        if (data.ReturnMessage.length > 0 && data.ReturnMessage[0] == "OK") {
            if (tipoGuardar == 0) {
                opcionHabilitarView(true);
                AjaxCargarRevision();
            } else {
                Limpiar();
                opcionHabilitarView(false);                    
            }
            displayNotify("MensajeGuardadoExistoso", "", '0');
            cambiosCheckOK = 0;
        } else {
            displayNotify("MensajeGuardadoErroneo", "", '2');
        }
    });
    loadingStop();
}

function Limpiar() {    
    $("#inputOrdenCompra").data("kendoComboBox").value("");
    $("#grid").data("kendoGrid").dataSource.data([]);
}