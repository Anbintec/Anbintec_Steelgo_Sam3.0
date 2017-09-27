function AjaxCargarCamposPredeterminados() {
    var campoPredeterminado = 3070;
    $CamposPredeterminados.CamposPredeterminados.read({ token: Cookies.get("token"), lenguaje: $("#language").val(), id: campoPredeterminado }).done(function (data) {
        if (data == "Pendientes") {
            $('#btnPendientes').trigger("click");
        }
        else if (data == "Transitados") {
            $('#btnTransito').trigger("click");
        }
    });
}

function AjaxObtenerContadorPorEstatus() {
    loadingStart();
    $ListadoEmbarque.ListadoEmbarque.read({ token: Cookies.get("token"), lenguaje: $("#language").val() }).done(function (data) {
        $("#nPendientes").text(" 0");
        $("#nTransito").text(" 0");

        if (data.length > 0) {
            $("#nPendientes").text(" "+data[0].Pendientes);
            $("#nTransito").text(" "+data[0].Transito);
        }
        loadingStop();
    });
}

function AjaxObtenerDetalleListadoEmbarque(estatus) {
    loadingStart();
    $ListadoEmbarque.ListadoEmbarque.read({ token: Cookies.get("token"), Lenguaje: $("#language").val(), EstatusEmbarque: estatus }).done(function (data) {
        $("#grid").data("kendoGrid").dataSource.data([]);
        var ds = $("#grid").data("kendoGrid").dataSource;

        if (data.length > 0) {
            if (estatus == 1) {
                for (var x = 0; x < data.length; x++) {
                    if (data[x].listaDestino.length == 2 && data[x].DestinoID == 0) {
                        data[x].DestinoID = data[x].listaDestino[1].DestinoID;
                        data[x].Destino = data[x].listaDestino[1].Destino;
                    }

                }
                $("#grid").data("kendoGrid").showColumn("Enviar");
            } else {
                $("#grid").data("kendoGrid").hideColumn("Enviar");
            }
            ds.data(data);
            ds.page(1);
        } else
            ds.page(0);

        ds.sync();
        AjaxObtenerContadorPorEstatus();
    });
}

function AjaxGuardarCaptura(ds, tipoGuardado) {
    loadingStart();

    var Captura = [];
    var ListaDetalle = [];
    var index = 0;
    Captura[0] = { listaDetalle: "" };

    for (var i = 0; i < ds.length; i++) {
        ListaDetalle[i] = {
            EmbarqueID: 0, DestinoID: 0, SolicitudPermiso: "", FechaPermiso: "", AprobadoAduana: 0,
            OkCliente: 0, OkEmbarque: 0, EstatusCaptura: 0, BitacoraAduana: 0
        }

        ListaDetalle[i].EmbarqueID = ds[i].EmbarqueID;
        ListaDetalle[i].DestinoID = ds[i].DestinoID;
        ListaDetalle[i].SolicitudPermiso = ds[i].FolioSolicitudPermiso;
        ListaDetalle[i].FechaPermiso = ds[i].FechaSolicitudPermiso == null ? "" : kendo.toString(ds[i].FechaSolicitudPermiso, String(_dictionary.FormatoFecha[$("#language").data("kendoDropDownList").value()].replace('{', '').replace('}', '').replace("0:", ""))).trim();
        ListaDetalle[i].AprobadoAduana = ds[i].RequierePermisoAduana?ds[i].AprobadoAduana:0;
        ListaDetalle[i].OkCliente = ds[i].OkCliente;
        ListaDetalle[i].OkEmbarque = ds[i].OkEmbarque;

        if (ds[i].DestinoAntID != ds[i].DestinoID || ds[i].FolioSolicitudPermiso != ds[i].SolicitudPermisoAnt ||
            ListaDetalle[i].FechaPermiso != ds[i].FechaSolicitudAnt || ds[i].OkCliente != ds[i].OkClienteAnt
            || ds[i].AprobadoAduana != ds[i].AprobadoAduanaAnt || ds[i].OkEmbarque != ds[i].OkEmbarqueAnt)
            ListaDetalle[i].EstatusCaptura = 1;

        if (ds[i].AprobadoAduana != ds[i].AprobadoAduanaAnt && ds[i].RequierePermisoAduana)
            ListaDetalle[i].BitacoraAduana = 1;

    }

    Captura[0].listaDetalle = ListaDetalle;
    $ListadoEmbarque.ListadoEmbarque.create(Captura[0], { token: Cookies.get("token"), lenguaje: $("#language").val()}).done(function (data) {
        if (data.ReturnMessage.length > 0 && data.ReturnMessage[0] == "Ok") {
            if (tipoGuardado != "1") {
                AjaxObtenerDetalleListadoEmbarque(estatus);
                opcionHabilitarView(false, "FieldSetView");
            } else {
                var estatus = $("#btnPendientes").hasClass("active") ? 1 : 2;
                opcionHabilitarView(true, "FieldSetView");
                AjaxObtenerDetalleListadoEmbarque(estatus);

            }
            displayNotify("MensajeGuardadoExistoso", "", '0');
        } else if (data.ReturnMessage.length > 0 && data.ReturnMessage[0] != "Ok") {
            displayNotify("MensajeGuardadoErroneo", "", '2');
            loadingStop();
        }
    });
}

function AjaxEnviarEmbarque(dataItem, numEmb, numEmbCliente, fechaEnvio) {
    loadingStart();

    var DetalleJson = {
        EmbarqueID: "", DestinoID: "", ProyectoID: "", SolicitudPermiso: "", FechaPermiso: "", AprobadoAduana: "", OkCliente: "",
        OkEmbarque: "", BitacoraAduana:""
    }
    if (dataItem != undefined) {
        DetalleJson.EmbarqueID = dataItem.EmbarqueID;
        DetalleJson.DestinoID = dataItem.DestinoID;
        DetalleJson.ProyectoID = dataItem.ProyectoID;
        DetalleJson.SolicitudPermiso = dataItem.FolioSolicitudPermiso;
        DetalleJson.FechaPermiso = dataItem.FechaSolicitudPermiso != null?kendo.toString(dataItem.FechaSolicitudPermiso, String(_dictionary.FormatoFecha[$("#language").data("kendoDropDownList").value()].replace('{', '').replace('}', '').replace("0:", ""))).trim() : "";
        DetalleJson.AprobadoAduana = dataItem.RequierePermisoAduana ? dataItem.AprobadoAduana : 0;
        DetalleJson.OkCliente = dataItem.OkCliente;
        DetalleJson.OkEmbarque = dataItem.OkEmbarque;
        DetalleJson.BitacoraAduana = dataItem.AprobadoAduana != dataItem.AprobadoAduanaAnt && dataItem.RequierePermisoAduana ? 1 : 0;
    }

    $ListadoEmbarque.ListadoEmbarque.create( DetalleJson, {
        token: Cookies.get("token"), Lenguaje: $("#language").val(), NumeroEmbarque: numEmb,
        NumeroEmbarqueCliente: numEmbCliente, FechaEnvio: fechaEnvio, ProyectoID: dataItem.ProyectoID
    }).done(function (data) {
        if (data.ReturnCode == 200) {
            if (data.ReturnMessage.length > 0 && data.ReturnMessage[0] == "Ok") {
                AjaxObtenerDetalleListadoEmbarque(1);
                displayNotify("EmbarqueListadoMsjExitoEnviar", "", '0');
            } else if (data.ReturnMessage.length > 0 && data.ReturnMessage[0] != "Ok") {
                if (data.ReturnMessage[0] == "NE EXISTE")
                    displayNotify("EmbarqueListadoErrorEnvioNumeroEmbarqueExiste", "", '2');
                else if (data.ReturnMessage[0] == "NEC EXISTE")
                    displayNotify("EmbarqueListadoErrorEnvioNumeroEmbarqueClienteExiste", "", '2');
                else
                    displayNotify("EmbarqueListadoErrorEnvioNumeroEmbarquesExisten", "", '2');
            }
        } else if (data.ReturnMessage.length > 0 && data.ReturnMessage[0] != "Ok") {
            displayNotify("EmbarqueListadoMsjErrorEnviar", "", '2');
            loadingStop();
        }
        var fecha = kendo.toString(new Date(), String(_dictionary.FormatoFecha[$("#language").data("kendoDropDownList").value()].replace('{', '').replace('}', '').replace("0:", ""))).trim();
        $("#NumEmb").val("");
        $("#NumEmbCliente").val("");
        $("#Fecha").val(fecha);
    });

}