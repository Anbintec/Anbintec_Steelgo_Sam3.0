function AjaxGuardarCaptura(data, tipoGuardar) {
    var arregloCaptura = data;
    var Captura = { Detalle: "" };
    var Datos = [];
    var cont = 0;
    for (var i = 0; i < arregloCaptura.length; i++) {
       
        if (arregloCaptura[i].RowOk) {
            Datos[cont] =
       {
           Consecutivo: 0,
           OrdenCompraID: 0,
           Revision: "",
           Descripcion: "",
           MaterialNorma: "",
           Diametro1: 0,
           Diametro2: 0,
           Shedule: "",
           Rating: "",
           PrepExt: "",
           Cantidad: 0,
           PrecioCompra: 0,
           PrecioVenta: 0
       }
            Datos[cont].Consecutivo = arregloCaptura[i].Consecutivo;
            Datos[cont].Revision = arregloCaptura[i].Revision;
            Datos[cont].Descripcion = arregloCaptura[i].Descripcion;
            Datos[cont].MaterialNorma = arregloCaptura[i].MaterialNorma;
            Datos[cont].Diametro1 = arregloCaptura[i].Diametro1;
            Datos[cont].Diametro2 = arregloCaptura[i].Diametro2;
            Datos[cont].Shedule = arregloCaptura[i].Shedule;
            Datos[cont].Rating = arregloCaptura[i].Rating;
            Datos[cont].PrepExt = arregloCaptura[i].PrepExt;
            Datos[cont].Cantidad = arregloCaptura[i].Cantidad;
            Datos[cont].PrecioCompra = arregloCaptura[i].PrecioCompra;
            Datos[cont].PrecioVenta = arregloCaptura[i].PrecioVenta;

            cont++;
        }
    }
    Captura.Detalle = Datos;

    loadingStart();
    $CreacionOC.CreacionOC.read({ token: Cookies.get("token"), OrdenCompra: $('#inputOrdenCompra').val(), ClienteID: $("#inputCliente").data("kendoComboBox").value() }).done(function (data) {
        if (Error(data)) {
            if (data.ReturnMessage[1] == "nueva") {
                $CreacionOC.CreacionOC.create(Captura, {
                    ClienteID: $("#inputCliente").data("kendoComboBox").value(), OrdenCompra: $('#inputOrdenCompra').val(),
                    Moneda: $("#inputMonedaCobro").data("kendoComboBox").value(), MonedaPago: $("#inputMonedaPago").data("kendoComboBox").value(), token: Cookies.get("token")
                }).done(function (data) {
                    if (Error(data)) {
                        if (data.ReturnMessage.length > 0 && data.ReturnMessage[0] == "OK") {
                            Limpiar();
                            displayNotify("MensajeGuardadoExistoso", "", '0');

                        } else {
                            displayNotify("MensajeGuardadoErroneo", "", '2');
                        }
                    }
                });
            }
            else {
                displayNotify("", "La orden de compra, ya existe para el cliente seleccionado", "1")
            }
        }
        loadingStop();
    });


    
};


function AjaxGetClienteByOC() {
    loadingStart();
    $CreacionOC.CreacionOC.read({ token: Cookies.get("token"), OrdenCompraID: 0 }).done(function (data) {
        if (Error(data)) {
            $("#inputCliente").data("kendoComboBox").setDataSource([]);
            $("#inputCliente").data("kendoComboBox").text("");
            if (data.length == 1) {
                $("#inputCliente").data("kendoComboBox").dataSource.data(data);
                $("#inputCliente").data("kendoComboBox").value(data.ClienteID);
                $("#inputCliente").data("kendoComboBox").select(1);
            } else {
                $("#inputCliente").data("kendoComboBox").dataSource.data(data);
            }
            AjaxObtenerCatalogoMoneda();
        }
        loadingStop();
    });

}

function AjaxObtenerCatalogoMoneda() {
    loadingStart();
    $CreacionOC.CreacionOC.read({ token: Cookies.get("token") }).done(function (data) {
        if (Error(data)) {
            $("#inputMonedaCobro").data("kendoComboBox").setDataSource([]);
            $("#inputMonedaCobro").data("kendoComboBox").text("");
            $("#inputMonedaCobro").data("kendoComboBox").dataSource.data(data);

            $("#inputMonedaPago").data("kendoComboBox").setDataSource([]);
            $("#inputMonedaPago").data("kendoComboBox").text("");
            $("#inputMonedaPago").data("kendoComboBox").dataSource.data(data);
        }
        loadingStop();
    });

}