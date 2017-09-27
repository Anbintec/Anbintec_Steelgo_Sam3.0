function ObtenerJSONParaGrid() {
    loadingStart();
    $WPS.WPS.read({ TipoDato: 1, token: Cookies.get("token") }).done(function (data) {
        if (Error(data)) {
            $("#grid").data('kendoGrid').dataSource.data([]);
            var ds = $("#grid").data("kendoGrid").dataSource;
            var array = data;
            for (var i = 0; i < array.length; i++) {
                ds.add(array[i]);
            }
        }
        loadingStop();
    });
};



function EliminaWPSAjax(dataItem) {

    if (confirm(_dictionary.WPSMensajeEliminar[$("#language").data("kendoDropDownList").value()])) {
        loadingStart();
        $WPS.WPS.update({}, { TipoDeDato: 4, WPSIdentificador: dataItem.WPSID, token: Cookies.get("token") }).done(function (data) {
            if (data.ReturnMessage == 'OK') {
                ObtenerJSONParaGrid();
            } else {

            };
            loadingStop();
        });
    }
};

function ExistEmptyWPS(listaDetalles) {

    var dataSource = $("#grid").data("kendoGrid").dataSource;
    var filters = dataSource.filter();
    var allData = dataSource.data();
    var query = new kendo.data.Query(allData);
    var arregloCaptura = query.filter(filters).data;



    for (var j = 0; j < listaDetalles.length; j++) {
        if (listaDetalles[j].Estatus == 0) {
            $("#grid").data("kendoGrid").dataSource._data[j].RowOk = false;
            //$('tr[data-uid="' + arregloCaptura[j].uid + '"] ').css("background-color", "#ffcccc");
        }
    }


    for (var i = 0; i < listaDetalles.length; i++) {
        if (listaDetalles[i].Estatus == 0)
            return true;
    }
    return false;
}
function AjaxGuardarCaptura() {

    Captura = [];
    Captura[0] = { Detalles: "" };
    ListaDetalles = [];
    var dataSource = $("#grid").data("kendoGrid").dataSource;
    var filters = dataSource.filter();
    var arregloCaptura = dataSource.data();
    //var query = new kendo.data.Query(allData);
    //var arregloCaptura = query.filter(filters).data;
    for (index = 0; index < arregloCaptura.length; index++) {
        $("#grid").data("kendoGrid").dataSource._data[index].RowOk = true;
        ListaDetalles[index] = {
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
            ModificadoUsuario: "",
            QuitarCVN: "",
            Estatus: 1
        };

        if ((arregloCaptura[index].WPSNombre == "" || arregloCaptura[index].WPSNombre == undefined || arregloCaptura[index].WPSNombre == null) ||
            (arregloCaptura[index].NombrePQRRaiz == "" || arregloCaptura[index].NombrePQRRaiz == undefined || arregloCaptura[index].NombrePQRRaiz == null) ||
            (arregloCaptura[index].NombrePQRRelleno == "" || arregloCaptura[index].NombrePQRRelleno == undefined || arregloCaptura[index].NombrePQRRelleno == null)
           ) {
            ListaDetalles[index].Estatus = 0;//Informacion Incompleta.
            $("#grid").data("kendoGrid").dataSource._data[index].RowOk = false;
            //$('tr[data-uid="' + arregloCaptura[index].uid + '"] ').css("background-color", "#ffcccc");
        }
        else if (!ContieneGruposMaterialBase(arregloCaptura[index].GrupoMaterialBase1RaizUID, arregloCaptura[index].GrupoMaterialBase1RaizDID, arregloCaptura[index].GrupoMaterialBase1RellenoUID, arregloCaptura[index].GrupoMaterialBase1RellenoDID)) {
            ListaDetalles[index].Estatus = -1;//se agrega esto para mostrar errores especificos
            $("#grid").data("kendoGrid").dataSource._data[index].RowOk = false;
            //$('tr[data-uid="' + arregloCaptura[index].uid + '"] ').css("background-color", "#ffcccc");
        }
        else if (arregloCaptura[index].PWHTRellenoId != arregloCaptura[index].PWHTRaizId) {
            ListaDetalles[index].Estatus = -2;//se agrega esto para mostrar errores especificos
            $("#grid").data("kendoGrid").dataSource._data[index].RowOk = false;
            //$('tr[data-uid="' + arregloCaptura[index].uid + '"] ').css("background-color", "#ffcccc");
        }
        else if (arregloCaptura[index].PREHEATRellenoId != arregloCaptura[index].PREHEATRaizId) {
            ListaDetalles[index].Estatus = -3;//se agrega esto para mostrar errores especificos
            $("#grid").data("kendoGrid").dataSource._data[index].RowOk = false;
            //$('tr[data-uid="' + arregloCaptura[index].uid + '"] ').css("background-color", "#ffcccc");
        }



        ListaDetalles[index].Accion = arregloCaptura[index].Accion;
        ListaDetalles[index].WPSId = arregloCaptura[index].WPSID;
        ListaDetalles[index].WPSNombre = arregloCaptura[index].WPSNombre;
        ListaDetalles[index].PQRRaizId = arregloCaptura[index].PQRRaizId;
        ListaDetalles[index].PQRRellenoId = arregloCaptura[index].PQRRellenoId;
        ListaDetalles[index].GrupoPId = arregloCaptura[index].GrupoMaterialBase1RaizUID;
        ListaDetalles[index].PWHTId = arregloCaptura[index].PWHTRellenoId;
        ListaDetalles[index].PREHEAT = arregloCaptura[index].PREHEATRellenoId;
        ListaDetalles[index].EspesorMaximoRaiz = arregloCaptura[index].EspesorMaximoRaiz;
        ListaDetalles[index].EspesorMinimoRaiz = arregloCaptura[index].EspesorMinimoRaiz;
        ListaDetalles[index].EspesorMinimoRelleno = arregloCaptura[index].EspesorMinimoRelleno;
        ListaDetalles[index].EspesorMaximoRelleno = arregloCaptura[index].EspesorMaximoRelleno;
        ListaDetalles[index].ProyectoID = arregloCaptura[index].ProyectoID;
        ListaDetalles[index].QuitarCVN = arregloCaptura[index].QuitarCVN;

        if (arregloCaptura[index].WPSNombre != arregloCaptura[index].WPSNombreOriginal)
            arregloCaptura[index].EditadoUsuario = true;
        
        ListaDetalles[index].ModificadoUsuario = arregloCaptura[index].EditadoUsuario;
        var arregloGrupos = obtenerGruposPLiberar(arregloCaptura[index].GrupoMaterialBase1RaizU, arregloCaptura[index].GrupoMaterialBase1RaizD, arregloCaptura[index].GrupoMaterialBase1RellenoU, arregloCaptura[index].GrupoMaterialBase1RellenoD);

        ListaDetalles[index].GrupoP1 = arregloGrupos[0];
        ListaDetalles[index].GrupoP2 = arregloGrupos[1];
        ListaDetalles[index].gruposCorrectos = obtenerGruposP(arregloCaptura[index].WPSID, arregloGrupos[0], arregloGrupos[1], ListaDetalles[index].Accion);

        
    }

    Captura[0].Detalles = ListaDetalles;

    var CapturaCorrecta = true;

    if (NombreRepetido(ListaDetalles)) {
        displayNotify("WPSMensajeErrorNombreRepetido", "", "2");
        opcionHabilitarView(false);
        CapturaCorrecta = false;
    }

    if (ExistEmptyWPS(ListaDetalles)) {
        //displayNotify("WPSMensajeErrorGruposMaterialBase", "", "2");
        opcionHabilitarView(false);
        CapturaCorrecta = false;
    }

    if (EsCorrectoGruposMaterialBase(ListaDetalles)) {
        displayNotify("WPSMensajeErrorGruposMaterialBase", "", "2");
        opcionHabilitarView(false);
        CapturaCorrecta = false;
    }

    if (EsCorrectoPWHTRELLENO(ListaDetalles)) {
        displayNotify("WPSMensajeErrorPWHRelleno", "", "2");
        opcionHabilitarView(false);
        CapturaCorrecta = false;
    }


    if (EsCorrectoPreHitRelleno(ListaDetalles)) {
        displayNotify("WPSMensajeErrorPreHitRelleno", "", "2");
        opcionHabilitarView(false);
        CapturaCorrecta = false;
    }


    $("#grid").data("kendoGrid").dataSource.sync();

    if (CapturaCorrecta) {
        ArregloGuardado = [];
        var indice = 0;
        for (var i = 0; i < Captura[0].Detalles.length; i++) {
            if ( Captura[0].Detalles[i].ModificadoUsuario) {
                ArregloGuardado[indice] = ListaDetalles[i];
                indice++;
            }
        }

        Captura[0].Detalles = [];
        Captura[0].Detalles = ArregloGuardado;

        if (Captura[0].Detalles.length > 0) {
            loadingStart();
            $WPS.WPS.create(Captura[0], { token: Cookies.get("token") }).done(function (data) {
                if (data.ReturnMessage.length > 0 && data.ReturnMessage[0] == "OK") {
                    displayNotify("CapturaMensajeGuardadoExitoso", "", '0');
                    ObtenerJSONParaGrid();
                    opcionHabilitarView(true);
                    loadingStop();
                }
                else  /*(data.ReturnMessage.length > 0 && data.ReturnMessage[0] != "Ok") */ {
                    //mensaje = "No se guardo la informacion el error es: " + data.ReturnMessage[0] + "-2";
                    displayNotify("CapturaMensajeGuardadoErroneo", "", '2');
                    loadingStop();

                }
            });
        }
        else
            displayNotify("MensajeAdverteciaExcepcionGuardado", "", '1');
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
            modal: true,
            animation: {
                close: false,
                open: false
            },
            actions: []
        }).data("kendoWindow");

        ventanaConfirm.content(_dictionary.CapturaAvanceIntAcabadoMensajePreguntaGuardado[$("#language").data("kendoDropDownList").value()] +
            "</br><center><button class='btn btn-blue' id='yesButton'>" + _dictionary.EntregaPlacasGraficasbotonSi[$("#language").data("kendoDropDownList").value()] + "</button><button class='btn btn-blue' id='noButton'>" + _dictionary.EntregaPlacasGraficasbotonNo[$("#language").data("kendoDropDownList").value()] + "</button></center>");

        ventanaConfirm.open().center();


        //RowEmpty($("#grid"));

        $("#yesButton").click(function () {
            loadingStart();
            ArregloGuardado = [];
            var indice = 0;
            for (var i = 0; i < Captura[0].Detalles.length; i++) {
                if (Captura[0].Detalles[i].Estatus == 1 && Captura[0].Detalles[i].ModificadoUsuario) {
                    ArregloGuardado[indice] = ListaDetalles[i];
                    indice++;
                }
            }

            Captura[0].Detalles = [];
            Captura[0].Detalles = ArregloGuardado;


            if (Captura[0].Detalles.length > 0) {
                loadingStart();
                $WPS.WPS.create(Captura[0], { token: Cookies.get("token") }).done(function (data) {
                    if (data.ReturnMessage.length > 0 && data.ReturnMessage[0] == "OK") {
                        displayNotify("CapturaMensajeGuardadoExitoso", "", '0');
                        ObtenerJSONParaGrid();
                        opcionHabilitarView(true);
                        loadingStop();
                    }
                    else  /*(data.ReturnMessage.length > 0 && data.ReturnMessage[0] != "Ok") */ {
                        //mensaje = "No se guardo la informacion el error es: " + data.ReturnMessage[0] + "-2";
                        displayNotify("CapturaMensajeGuardadoErroneo", "", '2');
                        opcionHabilitarView(false);
                        loadingStop();

                    }
                });
            }
            else {
                loadingStop();
                displayNotify("AdverteciaExcepcionGuardado", "", '1');
            }


            ventanaConfirm.close();
        });
        $("#noButton").click(function () {
            opcionHabilitarView(false);
            ventanaConfirm.close();
        });
    }

};