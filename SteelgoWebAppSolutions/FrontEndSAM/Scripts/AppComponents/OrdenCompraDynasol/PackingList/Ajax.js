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
    $PackingList.PackingList.read({ token: Cookies.get("token"), OrdenCompraID: $("#inputOrdenCompra").data("kendoComboBox").value(), PackingList: $("#InputPackingList").val().trim() }).done(function (data) {
        if (Error(data)) {
            $("#grid").data("kendoGrid").dataSource.data([]);

            var ds = $("#grid").data("kendoGrid").dataSource;
            var array = data;
            if (data.length > 0) {
                ds.data(data);
            }
        }
        ds.sync();
        loadingStop();


    });
}




function AjaxGuardarCaptura(arregloCaptura, tipoGuardar) {
    loadingStart();
    try {
        var bandera = true, banderaProcesoRaiz = true, banderaProcesoRelleno = true;
        loadingStart();
        Captura = [];
        Captura[0] = { Detalles: "" };
        ListaDetalles = [];
        var cont = 0;
        for (index = 0; index < arregloCaptura.length; index++) {
            if (arregloCaptura[index].Agregar || arregloCaptura[index].Accion == 3) {
                $("#grid").data("kendoGrid").dataSource._data[index].RowOk = true;
                ListaDetalles[cont] = {
                    Accion: "", DetallePackingListID: "", OrdenCompraID: "", ColadaID: "", Cant: "", PackingListID: "", Estatus: 1
                };

                ListaDetalles[cont].Accion = arregloCaptura[index].Accion;
                ListaDetalles[cont].DetallePackingListID = arregloCaptura[index].DetallePackingListID;
                ListaDetalles[cont].OrdenCompraID = arregloCaptura[index].OrdenCompraID;
                ListaDetalles[cont].ColadaID = arregloCaptura[index].ColadaID;
                ListaDetalles[cont].PackingListID = arregloCaptura[index].PackingListID;
                ListaDetalles[cont].Cant = arregloCaptura[index].CantPL;


                if ((arregloCaptura[index].CantPL == "" || arregloCaptura[index].CantPL == 0) && arregloCaptura[index].Accion != 3) {
                    ListaDetalles[cont].Estatus = 0;
                    $("#grid").data("kendoGrid").dataSource._data[index].RowOk = false;
                    $("#grid").data("kendoGrid").dataSource.sync();
                    //$('tr[data-uid="' + arregloCaptura[index].uid + '"] ').css("background-color", "#ffcccc");
                }
                cont++;
            }
        }

        Captura[0].Detalles = ListaDetalles;

        if (!ExistRowEmpty(ListaDetalles)) {
            if (Captura[0].Detalles.length > 0) {
                AjaxEjecutarGuardado(Captura[0], tipoGuardar);
            }

        }
        else {
            loadingStop();
            windowTemplate = kendo.template($("#windowTemplate").html());

            ventanaConfirm = $("#ventanaConfirm").kendoWindow({
                iframe: true,
                title: _dictionary.CapturaAvanceIntAcabadoMensajeErrorGuardado[$("#language").data("kendoDropDownList").value()],
                visible: false, //the window will not appear before its .open method is called
                width: "auto",
                height: "auto",
                actions: [],
                modal: true,
                animation: {
                    close: false,
                    open: false
                }
            }).data("kendoWindow");

            ventanaConfirm.content(_dictionary.CapturaAvanceIntAcabadoMensajePreguntaGuardado[$("#language").data("kendoDropDownList").value()] +
                "</br><center><button class='btn btn-blue' id='yesButton'>Si</button><button class='btn btn-blue' id='noButton'> No</button></center>");

            ventanaConfirm.open().center();

            $("#yesButton").click(function () {
                loadingStart();

                ArregloGuardado = [];
                var indice = 0;
                for (var i = 0; i < Captura[0].Detalles.length; i++) {
                    if (Captura[0].Detalles[i].Estatus == 1) {
                        ArregloGuardado[indice] = ListaDetalles[i];
                        indice++;
                    }
                }

                Captura[0].Detalles = [];
                Captura[0].Detalles = ArregloGuardado;


                if (ArregloGuardado.length > 0) {
                    AjaxEjecutarGuardado(Captura[0], tipoGuardar);
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

        loadingStop();
    } catch (e) {
        loadingStop();
        displayNotify("", e.message, '2');
    }

}


function AjaxEjecutarGuardado(Captura, tipoGuardar) {
    $PackingList.PackingList.create(Captura, { OrdenCompraID: $("#inputOrdenCompra").data("kendoComboBox").value(), NombrePL: $("#InputPackingList").val().trim(), token: Cookies.get("token"), lenguaje: $("#language").val() }).done(function (data) {
        if (Error(data)) {
            if (data.ReturnMessage.length > 0 && data.ReturnMessage[0] == "OK") {
                displayNotify("CapturaMensajeGuardadoExitoso", "", '0');

                if (tipoGuardar == 1) {
                    //editado = false;

                    $("#grid").data('kendoGrid').dataSource.data([]);
                    $('#InputPackingList').val("")
                    AjaxObtenerOrdenesCompra();
                    opcionHabilitarView(false, "FieldSetView");
                    loadingStop();

                }
                else {
                    opcionHabilitarView(true, "FieldSetView");
                    loadingStop();
                    AjaxCargarRevision();
                }
            }
            else {
                loadingStop();
                displayNotify("CapturaMensajeGuardadoErroneo", "", '2');
            }
        }
    });
}
