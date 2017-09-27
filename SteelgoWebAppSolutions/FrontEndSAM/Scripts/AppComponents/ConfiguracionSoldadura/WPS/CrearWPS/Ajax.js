var NombreCorrecto = false;

function obtenerPQRAjax() {
    loadingStart();
    $PQR.PQR.read({ token: Cookies.get("token"), TipoAccion: 1, pantallaEnvia: 2 }).done(function (data) {
        if (Error(data)) {
            $("#PQRRaizNombre").data("kendoComboBox").dataSource.data(data);
            $("#PQRRellenoNombre").data("kendoComboBox").dataSource.data(data);
        }
        loadingStop();
    });
}

function AjaxExisteWPS(varGuardar) {
    if ($('#NomnreWPS').val() != "" && $('#NomnreWPS').val() != undefined && $('#NomnreWPS').val() != null) {
        var WPSID = $("#WPSID").val() == "0" ? 0 : $("#WPSID").val();
        loadingStart();
        $WPS.WPS.read({ WPSID: WPSID, NombreWPSValida: $('#NomnreWPS').val(), token: Cookies.get("token") }).done(function (data) {
            if (Error(data)) {
                if (data.ReturnMessage[0] != "OK") {
                    //displayNotify("", "El Nombre del WPS ya existe", '2');
                    NombreCorrecto = false;
                }
                else {
                    NombreCorrecto = true;
                }
                AjaxGuardar(varGuardar);
            }
            loadingStop();
        });
    }
    else {
        displayNotify("WPSMensajeErrorNombreMandatorio", "", '1');
    }
}


function AjaxGuardar(tipoGuardar) {
    Captura = [];
    Captura[0] = { Detalles: "" };
    ListaDetalles = [];
    var correcto = true;

    ListaDetalles[0] = {
        Accion: "",
        WPSId: "",
        WPSNombre: "",
        PQRRaizId: "",
        PQRRellenoId: "",
        GrupoPId: "",
        PWHTId: "",
        PREHEAT: "",
        EspesorMaximoRaiz: "",
        EspesorMinimoRaiz: "",
        EspesorMaximoRelleno: "",
        EspesorMinimoRelleno: "",
        ProyectoID: "",
        GrupoP1: "",
        GrupoP2: "",
        gruposCorrectos: "",
        QuitarCVN: "",
    };

    if ($('#NomnreWPS').val() == "" || $('#NomnreWPS').val() == undefined || $('#NomnreWPS').val() == null) {
        correcto = false;
        displayNotify("WPSMensajeErrorNombreMandatorio", "", '1');
    }
    else if ($("#PQRRaizNombre").data("kendoComboBox").dataItem($("#PQRRaizNombre").data("kendoComboBox").select()) == undefined) {
        correcto = false;
        displayNotify("WPSMensajeErrorPQRRaiz", "", '1');
    }
    else if ($("#PQRRellenoNombre").data("kendoComboBox").dataItem($("#PQRRellenoNombre").data("kendoComboBox").select()) == undefined) {
        correcto = false;
        displayNotify("WPSMensajeErrorPQRRelleno", "", '1');
    }
    else if (!ContieneGruposMaterialBase($("#PQRRaizNombre").data("kendoComboBox").dataItem($("#PQRRaizNombre").data("kendoComboBox").select()).GrupoPMaterialBase1, $("#PQRRaizNombre").data("kendoComboBox").dataItem($("#PQRRaizNombre").data("kendoComboBox").select()).GrupoPMaterialBase2, $("#PQRRellenoNombre").data("kendoComboBox").dataItem($("#PQRRellenoNombre").data("kendoComboBox").select()).GrupoPMaterialBase1, $("#PQRRellenoNombre").data("kendoComboBox").dataItem($("#PQRRellenoNombre").data("kendoComboBox").select()).GrupoPMaterialBase2)) {
        correcto = false;
        displayNotify("WPSMensajeErrorGrupoP", "", '1');
    }
    else if ($('#PWHRelleno').is(':checked') != $('#PWHRaiz').is(':checked')) {
        correcto = false;
        displayNotify("WPSMensajeErrorPWHT", "", '1');
    }
    else if ($('#PREHEATRelleno').is(':checked') != $('#PREHEATRaiz').is(':checked')) {
        correcto = false;
        displayNotify("WPSMensajeErrorPREHEAT", "", '1');
    }
    else if (!NombreCorrecto && $("#WPSID").val() == "0") {
        correcto = false;
        displayNotify("WPSMensajeErrorNombreRepetido", "", "2");
    }


    if (correcto) {
        ListaDetalles[0].Accion = $("#WPSID").val() == "0" ? 1 : 2;
        ListaDetalles[0].WPSId = $("#WPSID").val() == "0" ? 0 : $("#WPSID").val();
        ListaDetalles[0].WPSNombre = $('#NomnreWPS').val();
        ListaDetalles[0].PQRRaizId = $('#PQRRaizNombre').data("kendoComboBox").value();
        ListaDetalles[0].PQRRellenoId = $('#PQRRellenoNombre').data("kendoComboBox").value();
        ListaDetalles[0].PWHTId = $('#PREHEATRelleno').is(':checked') ? 1 : 0;
        ListaDetalles[0].PREHEAT = $('#PWHRelleno').is(':checked') ? 1 : 0;
        ListaDetalles[0].EspesorMaximoRaiz = $('#EspesorMaximoRaiz').text();
        ListaDetalles[0].EspesorMinimoRaiz = $('#EspesorMinimoRaiz').text();
        ListaDetalles[0].EspesorMaximoRelleno = $('#EspesorMaximoRelleno').text();
        ListaDetalles[0].EspesorMinimoRelleno = $('#EspesorMinimoRelleno').text();
        ListaDetalles[0].QuitarCVN = $('#QuitarCVN').is(':checked') ? 1 : 0;
        var arregloGrupos = obtenerGruposPLiberar($("#PQRRaizNombre").data("kendoComboBox").dataItem($("#PQRRaizNombre").data("kendoComboBox").select()).GrupoPMaterialBase1, $("#PQRRaizNombre").data("kendoComboBox").dataItem($("#PQRRaizNombre").data("kendoComboBox").select()).GrupoPMaterialBase2, $("#PQRRellenoNombre").data("kendoComboBox").dataItem($("#PQRRellenoNombre").data("kendoComboBox").select()).GrupoPMaterialBase1, $("#PQRRellenoNombre").data("kendoComboBox").dataItem($("#PQRRellenoNombre").data("kendoComboBox").select()).GrupoPMaterialBase2);

        ListaDetalles[0].GrupoP1 = arregloGrupos[0];
        ListaDetalles[0].GrupoP2 = arregloGrupos[1];

        ListaDetalles[0].GrupoP1 = arregloGrupos[0];
        ListaDetalles[0].GrupoP2 = arregloGrupos[1];
        ListaDetalles[0].gruposCorrectos = obtenerGruposP($("#WPSID").val(), arregloGrupos[0], arregloGrupos[1], ListaDetalles[0].Accion);



        Captura[0].Detalles = ListaDetalles;


        if (Captura[0].Detalles.length > 0) {

            loadingStart();
            $WPS.WPS.create(Captura[0], { token: Cookies.get("token") }).done(function (data) {
                if (Error(data)) {
                    if (data.ReturnMessage.length > 0 && data.ReturnMessage[0] == "OK") {
                        if (data.ReturnMessage[1] != undefined) {
                            $("#WPSID").val(data.ReturnMessage[1]);
                        }

                        if (tipoGuardar == 1) {
                            Limpiar();
                            opcionHabilitarView(false, "FieldSetView");

                        }
                        else {
                            opcionHabilitarView(true, "FieldSetView");

                        }
                        displayNotify("CapturaMensajeGuardadoExitoso", "", '0');

                    }
                    else  /*(data.ReturnMessage.length > 0 && data.ReturnMessage[0] != "Ok") */ {
                        //mensaje = "No se guardo la informacion el error es: " + data.ReturnMessage[0] + "-2";
                        displayNotify("CapturaMensajeGuardadoErroneo", "", '2');

                    }
                }
                loadingStop();
            });
        }
    }

}