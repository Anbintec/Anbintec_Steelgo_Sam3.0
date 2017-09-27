
function LlenaGridAjax() {

    var TipoDato = 1;

    $PQR.PQR.read({ TipoDato: TipoDato, Proyecto: 28, PruebaID: 2, Especificacion: null, Codigo: null, token: Cookies.get("token"),pantallaEnvia:1 }).done(function (data) {
        if (Error(data)) {
            resultadoJson = data;
            if (resultadoJson.length > 0) {
                $("#grid").data("kendoGrid").dataSource.data(resultadoJson);
            } else {
                $("#grid").data("kendoGrid").dataSource.data([]);
            };
            $("#grid").data("kendoGrid").dataSource.sync();
        }
    });
};

function AjaxGuardarListado() {
    var dataSource = $("#grid").data("kendoGrid").dataSource;
    var filters = dataSource.filter();
    var arregloCaptura = dataSource.data();
    //var query = new kendo.data.Query(allData);
    //var arregloCaptura = query.filter(filters).data;


    Captura = [];
    Captura[0] = { Detalles: "" };
    ListaDetalles = [];
    var correcto = true;
    var pqrModified = '';
    var desplegadoNA = false;
    var desplegadoRell = false;
    var desplegadoRaiz = false;
    for (index = 0; index < arregloCaptura.length; index++) {
        ListaDetalles[index] = {
            PQRID: "",
            Accion: "",
            Nombre: "",
            PWHT: "",
            CVN: "",
            FN: "",
            MacroTest: "",
            PREHEAT: "",
            TipoPruebaID: "",
            TipoJuntaID: "",
            EspesorRelleno: "",
            EspesorRaiz: "",
            ProcesoSoldaduraRellenoID: "",
            ProcesoSoldaduraRaizID: "",
            NumeroP: "",
            GrupoMaterialesBase1: "",
            GrupoMaterialesBase2: "",
            Aporte: "",
            AporteRelleno: "",
            Mezcla: "",
            Respaldo: "",
            GrupoF: "",
            GrupoFRelleno: "",
            Especificacion: "",
            Estatus: 1
        };

        $("#grid").data("kendoGrid").dataSource._data[index].RowOk = true;

        if (((arregloCaptura[index].Nombre == "" || arregloCaptura[index].Nombre == undefined || arregloCaptura[index].Nombre == null) ||
            
            (arregloCaptura[index].TipoJunta == "" || arregloCaptura[index].TipoJunta == "") ||
            (arregloCaptura[index].TipoPrueba == null || arregloCaptura[index].EspesorRelleno == null) ||
            (arregloCaptura[index].ProcesoSoldaduraRaizID == 0 || arregloCaptura[index].ProcesoSoldaduraRaizID == undefined || arregloCaptura[index].ProcesoSoldaduraRaizID == "" || arregloCaptura[index].ProcesoSoldaduraRaizID == null) ||
            (arregloCaptura[index].ProcesoSoldaduraRellenoID == 0 || arregloCaptura[index].ProcesoSoldaduraRellenoID == undefined || arregloCaptura[index].ProcesoSoldaduraRellenoID == "" || arregloCaptura[index].ProcesoSoldaduraRellenoID == null) ||
            (arregloCaptura[index].ProcesoSoldaduraRaizID == 6 && arregloCaptura[index].ProcesoSoldaduraRellenoID == 6) ||
            (arregloCaptura[index].ProcesoSoldaduraRellenoID != 6 && arregloCaptura[index].EspesorRelleno <= 0) ||
            (arregloCaptura[index].ProcesoSoldaduraRaizID != 6 && arregloCaptura[index].EspesorRaiz <= 0) ||
            (arregloCaptura[index].ProcesoSoldaduraRellenoID == 6 && arregloCaptura[index].EspesorRelleno > 0) ||
            (arregloCaptura[index].ProcesoSoldaduraRaizID == 6 && arregloCaptura[index].EspesorRaiz > 0) ||
            (arregloCaptura[index].GrupoPMaterialBase1 == 0 || arregloCaptura[index].GrupoPMaterialBase1 == undefined || arregloCaptura[index].GrupoPMaterialBase1 == "" || arregloCaptura[index].GrupoPMaterialBase1 == null) ||
            (arregloCaptura[index].GrupoPMaterialBase2 == 0 || arregloCaptura[index].GrupoPMaterialBase2 == undefined || arregloCaptura[index].GrupoPMaterialBase2 == "" || arregloCaptura[index].GrupoPMaterialBase2 == null) ||
            (arregloCaptura[index].CodigoASMEID == 0 || arregloCaptura[index].CodigoASMEID == undefined || arregloCaptura[index].CodigoASMEID == "" || arregloCaptura[index].CodigoASMEID == null)) && arregloCaptura[index].Accion != 0) {

            ListaDetalles[index].Estatus = 0;
            $("#grid").data("kendoGrid").dataSource._data[index].RowOk = false;

            if (arregloCaptura[index].ProcesoSoldaduraRaizID == 6 && arregloCaptura[index].ProcesoSoldaduraRellenoID == 6 && !desplegadoNA) {
                desplegadoNA = true;
                displayNotify("lblPQRDobleNA", "", "1");
            }
            else if (arregloCaptura[index].ProcesoSoldaduraRellenoID != 6 && arregloCaptura[index].EspesorRelleno <= 0 && !desplegadoRell) {
                desplegadoRell = true;
                displayNotify("lblPQREspNARell", "", "1");
            }
            else if (arregloCaptura[index].ProcesoSoldaduraRaizID != 6 && arregloCaptura[index].EspesorRaiz <= 0 && !desplegadoRaiz) {
                desplegadoRaiz = true;
                displayNotify("lblPQREspNARaiz", "", "1");
            }
            else if (arregloCaptura[index].ProcesoSoldaduraRellenoID == 6 && arregloCaptura[index].EspesorRelleno > 0) {
                desplegadoRaiz = true;
                displayNotify("lblPQRNACeroRell", "", "1");
            }
            else if (arregloCaptura[index].ProcesoSoldaduraRaizID == 6 && arregloCaptura[index].EspesorRaiz > 0) {
                desplegadoRaiz = true;
                displayNotify("lblPQRNACeroRaiz", "", "1");
            }
        }
        else {
            if (pqrModified.length == 0)
                pqrModified = arregloCaptura[index].Nombre.toLowerCase();
            else
                pqrModified += ", " + arregloCaptura[index].Nombre.toLowerCase();
        }

        ListaDetalles[index].PQRID = arregloCaptura[index].PQRID;
        ListaDetalles[index].Nombre = arregloCaptura[index].Nombre.toLowerCase();
        ListaDetalles[index].PWHT = arregloCaptura[index].PWHT ? 1 : 0;
        ListaDetalles[index].PREHEAT = arregloCaptura[index].PREHEAT ? 1 : 0;
        ListaDetalles[index].CVN = arregloCaptura[index].CVN ? 1 : 0;
        ListaDetalles[index].FN = arregloCaptura[index].FN ? 1 : 0;
        ListaDetalles[index].MacroTest = arregloCaptura[index].MacroTest ? 1 : 0;
        ListaDetalles[index].TipoPruebaID = arregloCaptura[index].TipoPruebaID;
        ListaDetalles[index].TipoJuntaID = arregloCaptura[index].TipoJuntaID;
        ListaDetalles[index].EspesorRelleno = arregloCaptura[index].EspesorRelleno;
        ListaDetalles[index].EspesorRaiz = arregloCaptura[index].EspesorRaiz;
        ListaDetalles[index].ProcesoSoldaduraRellenoID = arregloCaptura[index].ProcesoSoldaduraRellenoID;
        ListaDetalles[index].ProcesoSoldaduraRaizID = arregloCaptura[index].ProcesoSoldaduraRaizID;
        ListaDetalles[index].NumeroP = arregloCaptura[index].NumeroP;
        ListaDetalles[index].GrupoPMaterialBase1 = arregloCaptura[index].GrupoPMaterialBase1;
        ListaDetalles[index].GrupoPMaterialBase2 = arregloCaptura[index].GrupoPMaterialBase2;
        ListaDetalles[index].Aporte = arregloCaptura[index].Aporte;
        ListaDetalles[index].AporteRelleno = arregloCaptura[index].AporteRelleno;
        ListaDetalles[index].Mezcla = arregloCaptura[index].Mezcla;
        ListaDetalles[index].Respaldo = arregloCaptura[index].Respaldo;
        ListaDetalles[index].GrupoF = arregloCaptura[index].GrupoF;
        ListaDetalles[index].GrupoFRelleno = arregloCaptura[index].GrupoFRelleno;
        ListaDetalles[index].Codigo = arregloCaptura[index].CodigoASMEID;
        ListaDetalles[index].Accion = arregloCaptura[index].Accion;
        
    }
    Captura[0].Detalles = ListaDetalles;

    

    //if () {
        if (!ExistRowEmpty(ListaDetalles) && !NombreRepetido(ListaDetalles)) {
            $("#grid").data("kendoGrid").dataSource.sync();
            if (Captura[0].Detalles.length > 0 && correcto) {
                loadingStart();
                $PQR.PQR.create(Captura[0], { token: Cookies.get("token"), accion: 2 }).done(function (data) {
                    if (data.ReturnMessage.length > 0 && data.ReturnMessage[0] == "OK") {
                        displayNotify("CapturaMensajeGuardadoExitoso", "", '0');
                        opcionHabilitarView(true);
                        LlenaGridAjax();
                        loadingStop();
                    }
                    else  /*(data.ReturnMessage.length > 0 && data.ReturnMessage[0] != "Ok") */ {
                        //mensaje = "No se guardo la informacion el error es: " + data.ReturnMessage[0] + "-2";
                        displayNotify("CapturaMensajeGuardadoErroneo", "", '2');
                        loadingStop();

                    }
                });
            }
        }
        else {
            loadingStop();
            $("#grid").data("kendoGrid").dataSource.sync();
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
                var pqrModified = '';
                ArregloGuardado = [];
                var indice = 0;
                for (var i = 0; i < Captura[0].Detalles.length; i++) {
                    if (Captura[0].Detalles[i].Estatus == 1) {
                        ArregloGuardado[indice] = ListaDetalles[i];

                        if (pqrModified.length == 0)
                            pqrModified = ListaDetalles[i].Nombre;
                        else
                            pqrModified += "," + ListaDetalles[i].Nombre

                        indice++;
                    }
                }

                Captura[0].Detalles = [];
                Captura[0].Detalles = ArregloGuardado;


                if (Captura[0].Detalles.length > 0) {
                    loadingStart();
                    $PQR.PQR.create(Captura[0], { token: Cookies.get("token"), accion: 2 }).done(function (data) {
                        if (data.ReturnMessage.length > 0 && data.ReturnMessage[0] == "OK") {
                            displayNotify("CapturaMensajeGuardadoExitoso", "", '0');
                            LlenaGridAjax();
                            loadingStop();
                        }
                        else  /*(data.ReturnMessage.length > 0 && data.ReturnMessage[0] != "Ok") */ {
                            //mensaje = "No se guardo la informacion el error es: " + data.ReturnMessage[0] + "-2";
                            displayNotify("CapturaMensajeGuardadoErroneo", "", '2');
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
    //}
    //else {
    //    $("#grid").data("kendoGrid").dataSource.sync();
    //    displayNotify("lblPQRNoRepetidos", "", "2");
    //    opcionHabilitarView(false);
    //}
};