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
    $Dynasol.Dynasol.read({ token: Cookies.get("token"), OrdenCompraID: $("#inputOrdenCompra").data("kendoComboBox").value(), Lenguaje: $("#language").val() }).done(function (data) {
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
    try {
        var bandera = true, banderaProcesoRaiz = true, banderaProcesoRelleno = true;
        loadingStart();
        Captura = [];
        Captura[0] = { Detalles: "" };
        ListaDetalles = [];

        for (index = 0; index < arregloCaptura.length; index++) {
            $("#grid").data("kendoGrid").dataSource._data[index].RowOk = true;
            ListaDetalles[index] = {
                Accion: "", RevisionID: "", Partida: "", ListaColadas: "", ListaDetalleInspeccion: "", Estatus: 1
            };

            var labelFecha = String(_dictionary.FormatoFecha[$("#language").data("kendoDropDownList").value()].replace('{', '').replace('}', '').replace("0:", ""));

            ListaDetalles[index].Accion = arregloCaptura[index].Accion;
            ListaDetalles[index].RevisionID = arregloCaptura[index].RevisionID;
            ListaDetalles[index].Partida = arregloCaptura[index].Partida;


            //-----------------------------------Lista Coladas--------------------------------------------

            ListaColadas = [];
            ListaInspeccion = [];
            if (arregloCaptura[index].ListaDetalleColadas != undefined) {
                var cont = 0;
                for (j = 0; j < arregloCaptura[index].ListaDetalleColadas.length; j++) {
                    ListaColadas[j] = {
                        Accion: "", RevisionID: "", ColadaID: "", Nombre: "", CantidadC: "", CantidadG: "", FechaRecibido: "",
                        Camion: "", FacturaProveedor: "", FechaFactura: "", Acuerdo: "", FechaEnvio: "", Pedimento: "",
                        ShippingDate: "", CantidadS: "", FechaRecibidoSteelgo: "", InspeccionSteelgo: "", MedidaCeciliaID: "",
                        MedidaGerezID: "", MedidaSteelgoID: ""
                    }

                    ListaColadas[j].Accion = arregloCaptura[index].ListaDetalleColadas[j].Accion;
                    ListaColadas[j].RevisionID = arregloCaptura[index].RevisionID;
                    ListaColadas[j].ColadaID = arregloCaptura[index].ListaDetalleColadas[j].ColadaID;
                    ListaColadas[j].Nombre = arregloCaptura[index].ListaDetalleColadas[j].Colada;
                    ListaColadas[j].CantidadC = arregloCaptura[index].ListaDetalleColadas[j].Cant;
                    ListaColadas[j].CantidadG = arregloCaptura[index].ListaDetalleColadas[j].CantG;
                    ListaColadas[j].FechaRecibido = kendo.toString(arregloCaptura[index].ListaDetalleColadas[j].FechaRecibidoG, labelFecha) == null ? "" : kendo.toString(arregloCaptura[index].ListaDetalleColadas[j].FechaRecibidoG, labelFecha).trim();
                    ListaColadas[j].Camion = arregloCaptura[index].ListaDetalleColadas[j].Camion;
                    ListaColadas[j].FacturaProveedor = arregloCaptura[index].ListaDetalleColadas[j].FacturaProveedor;
                    ListaColadas[j].FechaFactura = kendo.toString(arregloCaptura[index].ListaDetalleColadas[j].FechaFactura, labelFecha) == null ? "" : kendo.toString(arregloCaptura[index].ListaDetalleColadas[j].FechaFactura, labelFecha).trim();
                    ListaColadas[j].Acuerdo = arregloCaptura[index].ListaDetalleColadas[j].Acuerdo;
                    ListaColadas[j].FechaEnvio = kendo.toString(arregloCaptura[index].ListaDetalleColadas[j].FechaEnvio, labelFecha) == null ? "" : kendo.toString(arregloCaptura[index].ListaDetalleColadas[j].FechaEnvio, labelFecha).trim();;
                    ListaColadas[j].Pedimento = arregloCaptura[index].ListaDetalleColadas[j].Pedimento;
                    ListaColadas[j].ShippingDate = kendo.toString(arregloCaptura[index].ListaDetalleColadas[j].ShippingDate, labelFecha) == null ? "" : kendo.toString(arregloCaptura[index].ListaDetalleColadas[j].ShippingDate, labelFecha).trim();
                    ListaColadas[j].CantidadS = arregloCaptura[index].ListaDetalleColadas[j].CantS;
                    ListaColadas[j].FechaRecibidoSteelgo = kendo.toString(arregloCaptura[index].ListaDetalleColadas[j].FechaRecibidoS, labelFecha) == null ? "" : kendo.toString(arregloCaptura[index].ListaDetalleColadas[j].FechaRecibidoS, labelFecha).trim();;
                    ListaColadas[j].MedidaCeciliaID = arregloCaptura[index].ListaDetalleColadas[j].MedidaCeciliaID;
                    ListaColadas[j].MedidaGerezID = arregloCaptura[index].ListaDetalleColadas[j].MedidaGerezID;
                    ListaColadas[j].MedidaSteelgoID = arregloCaptura[index].ListaDetalleColadas[j].MedidaSteelgoID;



                    //-----------------------------------Lista Inspeccion--------------------------------------------
                    if (arregloCaptura[index].ListaDetalleColadas[j].ListaDetalleInspeccion != undefined) {
                        for (i = 0; i < arregloCaptura[index].ListaDetalleColadas[j].ListaDetalleInspeccion.length; i++) {
                            ListaInspeccion[cont] = {
                                Accion: "", RevisionID: "", DetalleInspeccion: "", InspeccionID: "", NombreColada: "", Comentario: ""

                            }

                            ListaInspeccion[cont].Accion = arregloCaptura[index].ListaDetalleColadas[j].ListaDetalleInspeccion[i].Accion;
                            ListaInspeccion[cont].RevisionID = arregloCaptura[index].RevisionID;
                            ListaInspeccion[cont].DetalleInspeccion = arregloCaptura[index].ListaDetalleColadas[j].ListaDetalleInspeccion[i].DetalleInspeccionID;
                            ListaInspeccion[cont].InspeccionID = arregloCaptura[index].ListaDetalleColadas[j].ListaDetalleInspeccion[i].InspeccionID;
                            ListaInspeccion[cont].NombreColada = arregloCaptura[index].ListaDetalleColadas[j].Colada;
                            ListaInspeccion[cont].Comentario = arregloCaptura[index].ListaDetalleColadas[j].ListaDetalleInspeccion[i].Comentario;
                            cont++;
                        }
                    }


                }
            }
            ListaDetalles[index].ListaColadas = ListaColadas;
            ListaDetalles[index].ListaDetalleInspeccion = ListaInspeccion;



            for (var j = 0; j < arregloCaptura.length; j++) {
                if (arregloCaptura[index].Partida.trim() == arregloCaptura[j].Partida.trim() && index != j && arregloCaptura[index].Partida != "" && arregloCaptura[j].Partida != "" ) {
                    ListaDetalles[index].Estatus = 0;
                    $("#grid").data("kendoGrid").dataSource._data[index].RowOk = false;
                    
                    $("#grid").data("kendoGrid").dataSource.sync();
                }
            }



            if (arregloCaptura[index].Partida == "") {
                ListaDetalles[index].Estatus = 0;
                $("#grid").data("kendoGrid").dataSource._data[index].RowOk = false;
                $("#grid").data("kendoGrid").dataSource.sync();
                //$('tr[data-uid="' + arregloCaptura[index].uid + '"] ').css("background-color", "#ffcccc");
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
    $Dynasol.Dynasol.create(Captura, { token: Cookies.get("token"), lenguaje: $("#language").val() }).done(function (data) {
        if (Error(data)) {
            if (data.ReturnMessage.length > 0 && data.ReturnMessage[0] == "OK") {
                displayNotify("CapturaMensajeGuardadoExitoso", "", '0');

                if (tipoGuardar == 1) {
                    //editado = false;

                    $("#grid").data('kendoGrid').dataSource.data([]);
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