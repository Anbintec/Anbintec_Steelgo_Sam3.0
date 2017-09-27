
function ObtenerListasPQR() {

    var TipoDato = 1;

    $PQR.PQR.read({ Proyecto: 16, PruebaID: 0, token: Cookies.get("token") }).done(function (data) {
        if (Error(data)) {
            if (data.length > 0)
            {
                $("#ProcesoSoldaduraRellenoID").data("kendoComboBox").dataSource.data(data[0].ListaProcesosSoldadura);
                $("#ProcesoSoldaduraRaizID").data("kendoComboBox").dataSource.data(data[0].ListaProcesosSoldadura);
                $("#GrupoPMaterialBase1ID").data("kendoComboBox").dataSource.data(data[0].ListaMaterialesBase);
                $("#GrupoPMaterialBase2ID").data("kendoComboBox").dataSource.data(data[0].ListaMaterialesBase);
                $("#CodigoID").data("kendoComboBox").dataSource.data(data[0].ListaCodigos);
                $("#TipoJunta").data("kendoComboBox").dataSource.data(data[0].listaTipoJunta);
                $("#TipoPrueba").data("kendoComboBox").dataSource.data(data[0].listaTipoPrueba);
            }
        }
    });
};

function AjaxGuardar(tipoGuardar) {
    Captura = [];
    Captura[0] = { Detalles: "" };
    ListaDetalles = [];
    var correcto = true;

    ListaDetalles[0] = {
        PQRID: "",
        Accion: "",
        Nombre: "",
        PREHEAT: "",
        PWHT: "",
        EspesorRelleno: "",
        EspesorRaiz: "",
        ProcesoSoldaduraRellenoID: "",
        ProcesoSoldaduraRaizID: "",
        NumeroP: "",
        GrupoPMaterialBase1: "",
        GrupoPMaterialBase2: "",
        Aporte: "",
        Mezcla: "",
        Respaldo: "",
        GrupoF: "",
        Codigo: ""
    };

    if ($('#NombreId').val() == "" || $('#NombreId').val() == undefined || $('#NombreId').val() == null) {
        correcto = false;
        displayNotify("lblPQRNombreMandatorio", "", '1');
    }
    else if ($('#EspesorRelleno').val() == "" || $('#EspesorRelleno').val() == undefined || $('#EspesorRelleno').val() == null) {
        correcto = false;
        displayNotify("lblPQREspRellMandatorio", "", '1');
    }
    else if ($('#EspesorRaiz').val() == "" || $('#EspesorRaiz').val() == undefined || $('#EspesorRaiz').val() == null) {
        correcto = false;
        displayNotify("lblPQREspRaizMandatorio", "", '1');
    }
    else if ($('#ProcesoSoldaduraRellenoID').data("kendoComboBox").dataItem($("#ProcesoSoldaduraRellenoID").data("kendoComboBox").select()) == undefined
          || $('#ProcesoSoldaduraRellenoID').data("kendoComboBox").dataItem($("#ProcesoSoldaduraRellenoID").data("kendoComboBox").select()).ProcesoSoldaduraID == 0) {
        correcto = false;
        displayNotify("lblPQRProcSoldMandatorio", "", '1');
    }
    else if ($('#ProcesoSoldaduraRaizID').data("kendoComboBox").dataItem($("#ProcesoSoldaduraRaizID").data("kendoComboBox").select()) == undefined
          || $('#ProcesoSoldaduraRaizID').data("kendoComboBox").dataItem($("#ProcesoSoldaduraRaizID").data("kendoComboBox").select()).ProcesoSoldaduraID == 0) {
        correcto = false;
        displayNotify("lblPQRProcRaizMandatorio", "", '1');
    }
    else if ($('#GrupoPMaterialBase1ID').data("kendoComboBox").dataItem($("#GrupoPMaterialBase1ID").data("kendoComboBox").select()) == undefined
          || $('#GrupoPMaterialBase1ID').data("kendoComboBox").dataItem($("#GrupoPMaterialBase1ID").data("kendoComboBox").select()).GrupoPID == 0) {
        correcto = false;
        displayNotify("lblPQRGrpB1Mandatorio", "", '1');
    }
    else if ($('#GrupoPMaterialBase2ID').data("kendoComboBox").dataItem($("#GrupoPMaterialBase2ID").data("kendoComboBox").select()) == undefined
          || $('#GrupoPMaterialBase2ID').data("kendoComboBox").dataItem($("#GrupoPMaterialBase2ID").data("kendoComboBox").select()).GrupoPID == 0) {
        correcto = false;
        displayNotify("lblPQRGrpB2Mandatorio", "", '1');
    }
    else if ($('#CodigoID').data("kendoComboBox").dataItem($("#CodigoID").data("kendoComboBox").select()) == undefined
          || $('#CodigoID').data("kendoComboBox").dataItem($("#CodigoID").data("kendoComboBox").select()).CodigoAsmeID == 0) {
        correcto = false;
        displayNotify("lblPQRCodigoMandatorio", "", '1');
    }
    else if ($('#TipoJunta').data("kendoComboBox").dataItem($("#TipoJunta").data("kendoComboBox").select()) == undefined
      || $('#TipoJunta').data("kendoComboBox").dataItem($("#TipoJunta").data("kendoComboBox").select()).CodigoAsmeID == 0) {
        correcto = false;
        displayNotify("lblPQRTipoJuntaMandatorio", "", '1');
    }
    else if ($('#TipoPrueba').data("kendoComboBox").dataItem($("#TipoPrueba").data("kendoComboBox").select()) == undefined
      || $('#TipoPrueba').data("kendoComboBox").dataItem($("#TipoPrueba").data("kendoComboBox").select()).CodigoAsmeID == 0) {
        correcto = false;
        displayNotify("lblPQRTipoPruebaMandatorio", "", '1');
    }
    else if ($('#ProcesoSoldaduraRellenoID').data("kendoComboBox").dataItem($("#ProcesoSoldaduraRellenoID").data("kendoComboBox").select()).Codigo == "N/A"
      && $('#ProcesoSoldaduraRaizID').data("kendoComboBox").dataItem($("#ProcesoSoldaduraRaizID").data("kendoComboBox").select()).Codigo == "N/A") {
        correcto = false;
        displayNotify("lblPQRDobleNA", "", '1');
    }
    else if ($('#ProcesoSoldaduraRellenoID').data("kendoComboBox").dataItem($("#ProcesoSoldaduraRellenoID").data("kendoComboBox").select()).Codigo != "N/A"
          && parseFloat($('#EspesorRelleno').val()) <= 0) {
        correcto = false;
        displayNotify("lblPQREspNARell", "", '1');
    }
    else if ($('#ProcesoSoldaduraRaizID').data("kendoComboBox").dataItem($("#ProcesoSoldaduraRaizID").data("kendoComboBox").select()).Codigo != "N/A"
          && parseFloat($('#EspesorRaiz').val()) <= 0) {
        correcto = false;
        displayNotify("lblPQREspNARaiz", "", '1');
    }


    ListaDetalles[0].PQRID = $("#PQRID").val() == "0" ? 0 : $("#PQRID").val();
    ListaDetalles[0].Accion = $("#PQRID").val() == "0" ? 1 : 2;
    ListaDetalles[0].Nombre = $('#NombreId').val();
    ListaDetalles[0].PREHEAT = $('#chkPreheat').is(':checked') ? 1 : 0;
    ListaDetalles[0].PWHT = $('#chkPwht').is(':checked') ? 1 : 0;
    ListaDetalles[0].CVN = $('#chkCVN').is(':checked') ? 1 : 0;
    ListaDetalles[0].FN = $('#chkFN').is(':checked') ? 1 : 0;
    ListaDetalles[0].MacroTest = $('#chkMacroTest').is(':checked') ? 1 : 0;
    ListaDetalles[0].TipoPruebaID = $("#TipoPrueba").data("kendoComboBox").value();
    ListaDetalles[0].TipoJuntaID = $("#TipoJunta").data("kendoComboBox").value();
    ListaDetalles[0].EspesorRelleno = $("#EspesorRelleno").val();
    ListaDetalles[0].EspesorRaiz = $("#EspesorRaiz").val();
    ListaDetalles[0].ProcesoSoldaduraRellenoID = $("#ProcesoSoldaduraRellenoID").data("kendoComboBox").value();
    ListaDetalles[0].ProcesoSoldaduraRaizID = $("#ProcesoSoldaduraRaizID").data("kendoComboBox").value();
    ListaDetalles[0].GrupoPMaterialBase1 = $("#GrupoPMaterialBase1ID").data("kendoComboBox").value();
    ListaDetalles[0].GrupoPMaterialBase2 = $("#GrupoPMaterialBase2ID").data("kendoComboBox").value();
    ListaDetalles[0].Aporte = $("#AporteID").val();
    ListaDetalles[0].AporteRelleno = $("#AporteRellenoID").val();
    ListaDetalles[0].Mezcla = $("#MezclaID").val();
    ListaDetalles[0].Respaldo = $("#RespaldoID").val();
    ListaDetalles[0].GrupoF = $("#GrupoFID").val();
    ListaDetalles[0].GrupoFRelleno = $("#GrupoFRellenoID").val();
    ListaDetalles[0].Codigo = $("#CodigoID").data("kendoComboBox").value();

    Captura[0].Detalles = ListaDetalles;

    if (Captura[0].Detalles.length > 0 && correcto) {
        loadingStart();
        $PQR.PQR.create(Captura[0], { token: Cookies.get("token"), accion: ListaDetalles[0].Accion }).done(function (data) {
            if (Error(data)) {
                if (data.ReturnMessage.length > 0 && data.ReturnMessage[0] == "OK") {
                    if (data.ReturnMessage[1] != undefined) {
                        $("#PQRID").val(data.ReturnMessage[1]);
                    }
                    
                    if (tipoGuardar == 1) {//guardar y nuevo
                        loadingStart();
                        Limpiar();
                    }
                    else {
                        opcionHabilitarView(true, "FieldSetView");
                        loadingStop();
                    }
                    displayNotify("CapturaMensajeGuardadoExitoso", "", '0');
                    
                }
                else  /*(data.ReturnMessage.length > 0 && data.ReturnMessage[0] != "Ok") */ {
                    //mensaje = "No se guardo la informacion el error es: " + data.ReturnMessage[0] + "-2";
                    displayNotify("CapturaMensajeGuardadoErroneo", "", '2');
                    loadingStop();

                }
            }
            
        });
    }
}

function AjaxExistePQR(tipo) {
    if ($('#NombreId').val() != "" && $('#NombreId').val() != undefined && $('#NombreId').val() != null
       && ($("#PQRID").val() == "" || $("#PQRID").val() == undefined || $("#PQRID").val() == null || $("#PQRID").val() == 0)) {
        loadingStart();
        $PQR.PQR.read({ token: Cookies.get("token"), nombre: $('#NombreId').val() }).done(function (data) {
            if (Error(data)) {
                if (data.ReturnMessage[0] != "OK") {
                    displayNotify("", "El Nombre del PQR ya existe", '2');
                    //$('#NombreId').focus();

                }
                else if (tipo != 3) {
                    AjaxGuardar(tipo);
                }
            }
            loadingStop();
        });
    } else if ($("#PQRID").val() != "" || $("#PQRID").val() != undefined || $("#PQRID").val() != null || $("#PQRID").val() != 0) {
        loadingStart();
        $PQR.PQR.read({ token: Cookies.get("token"), nombre: $('#NombreId').val() }).done(function (data) {
            if (Error(data)) {
                if (data.ReturnMessage[0] != "OK") {
                    if (data.ReturnMessage[0] == $("#PQRID").val()) {
                        if (tipo != 3) {
                            AjaxGuardar(tipo);
                        }
                    } else {
                        displayNotify("", "El Nombre del PQR ya existe", '2');
                    }
                }
                else if (tipo != 3) {
                    AjaxGuardar(tipo);
                }
            }
            loadingStop();
        });
    }
}