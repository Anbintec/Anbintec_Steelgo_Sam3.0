var TipoObrero = "Inspector Visual Dimensional";
var TipoConsultaObrero = 2;
var TipoPrueba = "Inspección dimensional";
var CampoFechaPredeterminada = 24;
var CampoResultadoPredetrminado = 25;
var CampoLlenadoPredeterminado = 26;

function AjaxObtenerListaInspector() {
    loadingStart();
    $Obrero.Obrero.read({ idProyecto: 0, tipo: TipoConsultaObrero, token: Cookies.get("token"), TipoObrero: TipoObrero }).done(function (data) {
        if (Error(data)) {
            $("#inputInspector").data("kendoComboBox").value("");
            $("#inputInspector").data("kendoComboBox").dataSource.data(data);
            loadingStop();
        }
    });

}
function AjaxObtenerListaDefectos() {
    loadingStart();
    $Defectos.Defectos.read({ lenguaje: $("#language").val(), TipoPrueba: TipoPrueba, token: Cookies.get("token") }).done(function (data) {
        if (Error(data)) {
            $("#inputDefecto").data("kendoComboBox").value("");
            $("#inputDefecto").data("kendoComboBox").dataSource.data(data);
            loadingStop();
        }
    });
}
function AjaxCargaCamposPredetrminados() {
    $InspeccionDimensional.InspeccionDimensional.read({ token: Cookies.get("token"), lenguaje: $("#language").val() }).done(function (data) {
        if (Error(data)) {
            var NewDate = kendo.toString(data.Fecha, _dictionary.FormatoFecha[$("#language").data("kendoDropDownList").value()]);
            endRangeDate.val(NewDate);

            if (data.Resultado == "Aprobado") {
                $('input#ResultadoDimensionalAprobado[name="ResultadoDimensional"]').click();
            }
            else if (data.Resultado == "Rechazado") {

                $('input#ResultadoDimensionalRechazado[name="ResultadoDimensional"]').click();

            }
            else if (data.Resultado == "Ninguno") {
                $('input#ResultadoDimensionalNinguno[name="ResultadoDimensional"]').click();
            }

            if (data.Llena == "Todos") {
                $('input:radio[name=LLena]:nth(0)').trigger("click");
                //$('input:radio[name=LLena]:nth(1)').attr('checked', false);
                $("input:radio[name=LLena]:checked").change();

            }
            else if (data.Llena == "Vacios") {
                //$('input:radio[name=LLena]:nth(0)').attr('checked', false);
                $('input:radio[name=LLena]:nth(1)').trigger("click");
                $("input:radio[name=LLena]:checked").change();

            }
            else {
                //$('input:radio[name=LLena]:nth(0)').attr('checked', false);
                $('input:radio[name=LLena]:nth(1)').trigger("click");
                $("input:radio[name=LLena]:checked").change();

            }

            if (data.Muestra == "Todos") {
                $('input:radio[name=Muestra]:nth(1)').trigger("click");
            }
            else if (data.Muestra == "sin captura") {
                $('input:radio[name=Muestra]:nth(0)').trigger("click");
            }

        }
    });
}
function AjaxObtenerSpoolID() {
    try {

        loadingStart();
        $CapturasRapidas.CapturasRapidas.read({ ordenTrabajo: $("#InputOrdenTrabajo").val(), tipo: '1', token: Cookies.get("token"), lenguaje: $("#language").val() }).done(function (data) {
            if (Error(data)) {
                $("#InputOrdenTrabajo").val(data.OrdenTrabajo);
                $("#InputID").data("kendoComboBox").dataSource.data(data.idStatus)
                Cookies.set("LetraProyecto", data.OrdenTrabajo.substring(0, 1), { path: '/' });
                loadingStop();
            }
        });
    } catch (e) {
        displayNotify("Mensajes_error", e.message, '2');
    }
}
function AjaxobtenerDetalleDimensional(spoolID) {
    loadingStart();
    $CapturasRapidas.CapturasRapidas.read({ id: spoolID, sinCaptura: $('input:radio[name=Muestra]:checked').val(), token: Cookies.get("token"), lenguaje: $("#language").val() }).done(function (data) {
        if (Error(data)) {
            if (data.ListaDetalleDimensional.length == 0) {
                $("#InspeccionDimensionalID").val("0")
            }
            else {
                $("#InspeccionDimensionalID").val(data.ListaDetalleDimensional[0].InspeccionDimensionalID);

            }
        }
        loadingStop();
    });
}

function AjaxObtenerJSonGrid() {


    try {
        loadingStart();
        $InspeccionDimensional.InspeccionDimensional.read({ JsonCaptura: JSON.stringify(ArregloListadoCaptura()), token: Cookies.get("token"), Lenguaje: $("#language").val() }).done(function (data) {
            if (Error(data)) {
                var ds = $("#grid").data("kendoGrid").dataSource;
                var array = JSON.parse(data);
                if (array.length > 0) {//1.- ahora solo trae un registro por spool porque en una sola llamada trae la junta.
                    if ($('input:radio[name=Muestra]:checked').val() == "Todos" || ($('input:radio[name=Muestra]:checked').val() == "Sin Capturar" && array[0].Accion == 1)) {
                        for (var i = 0; i < array.length; i++) {

                            if (!ExisteSpool(array[i])) {
                                array[i].NumeroUnico1 = array[i].NumeroUnico1 == "" ? DatoDefaultNumeroUnico1() : array[i].NumeroUnico1;
                                array[i].NumeroUnico2 = array[i].NumeroUnico2 == "" ? DatoDefaultNumeroUnico2() : array[i].NumeroUnico2;
                                if (array[i].FechaInspeccion != null) {
                                    array[i].FechaInspeccion = new Date(ObtenerDato(array[i].FechaInspeccion, 1), ObtenerDato(array[i].FechaInspeccion, 2), ObtenerDato(array[i].FechaInspeccion, 3));//año, mes, dia
                                }
                                ds.insert(0, array[i]);
                                displayNotify('', _dictionary.DimensionalSpool[$("#language").data("kendoDropDownList").value()] + " " + array[i].OrdenTrabajoSpool, '0')
                                //como trae solo un registro se sincroniza se explica en el punto del comentario 1
                                $("#grid").data("kendoGrid").dataSource.sync();
                                $("#InputID").data("kendoComboBox").value("");
                            }
                            else {
                                displayNotify("SpoolIDExistente", '', '1');
                                $("#InputID").data("kendoComboBox").value("");
                            }
                        }
                    }
                    else {
                        displayNotify("CapturaInspeccionDimensionalNoExisteLista", '', '1');
                        loadingStop();
                    }
                }
                else {
                    //mensaje que no existe el spool.
                    displayNotify("CapturaInspeccionDimensionalResultAjaxEmpty", '', '2');
                }
            }
            loadingStop();

        });

    } catch (e) {
        displayNotify("Mensajes_error", e.message, '2');

    }




}

function ExisteSpool(row) {
    //var jsonGridArmado = $("#grid").data("kendoGrid").dataSource._data;

    var dataSource = $("#grid").data("kendoGrid").dataSource;
    var filters = dataSource.filter();
    var allData = dataSource.data();
    var query = new kendo.data.Query(allData);
    var jsonGridArmado = query.filter(filters).data;

    for (var i = 0; i < jsonGridArmado.length; i++) {
        if (jsonGridArmado[i].OrdenTrabajoSpool == row.OrdenTrabajoSpool) {
            return true
        }
    }
    return false;

}

function ExisteJunta() {
    var jsonGridArmado = $("#grid").data("kendoGrid").dataSource._data;

    for (var i = 0; i < jsonGridArmado.length; i++) {
        if (jsonGridArmado[i].OrdenTrabajoSpool == ($("#InputOrdenTrabajo").val() + '-' + $("#InputID").data("kendoComboBox").text())) {
            if (jsonGridArmado[i].Accion == 3) {
                jsonGridArmado[i].Accion = 2;
                $("#grid").data("kendoGrid").dataSource.sync();
            }
            else {
                displayNotify("", _dictionary.DimensionalSpoolExist[$("#language").data("kendoDropDownList").value()] + jsonGridArmado[i].OrdenTrabajoSpool, "1");
            }
            return false;
        }
    }
    return true;
}


function AjaxGuardar(jSonCaptura, tipoGuardado) {
    loadingStart();

    Captura = [];
    Captura[0] = { Detalles: "" };
    $("#grid").data("kendoGrid").dataSource.sync();
    var mensaje = '';
    inspeccionDimensional = [];
    Juntas = [];
    //if (InspectorCorrecto(jSonCaptura)) {
    for (index = 0; index < jSonCaptura.length; index++) {
        $("#grid").data("kendoGrid").dataSource._data[index].RowOk = true;
        inspeccionDimensional[index] = { Accion: "", InspeccionDimensionalID: "", FechaInspeccion: "", ResultadoID: "", Resultado: "", InspectorID: "", Inspector: "", DefectosID: "", Defectos: "", OrdenTrabajoSpoolID: "", ListaJuntas: [], Estatus: 1 }
        inspeccionDimensional[index].Accion = jSonCaptura[index].Accion;
        inspeccionDimensional[index].InspeccionDimensionalID = jSonCaptura[index].InspeccionDimensionalID;
        inspeccionDimensional[index].OrdenTrabajoSpoolID = jSonCaptura[index].OrdenTrabajoSpoolID;
        inspeccionDimensional[index].ResultadoID = jSonCaptura[index].ResultadoID;
        inspeccionDimensional[index].DefectosID = jSonCaptura[index].DefectosID == "" ? 0 : jSonCaptura[index].DefectosID;
        inspeccionDimensional[index].InspectorID = jSonCaptura[index].InspectorID;
        inspeccionDimensional[index].FechaInspeccion = jSonCaptura[index].FechaInspeccion == null ? "" : kendo.toString(jSonCaptura[index].FechaInspeccion, String(_dictionary.FormatoFecha[$("#language").data("kendoDropDownList").value()].replace('{', '').replace('}', '').replace("0:", ""))).trim();

        //................................juntas

        var listaFinalJuntas = [];

        if (jSonCaptura[index].Defectos == jSonCaptura[index].DefectoInicial || jSonCaptura[index].DefectoInicial == "") {
            if (jSonCaptura[index].ListaJuntasSeleccionadasInicial != null && jSonCaptura[index].ListaJuntasSeleccionadas != null) {
                for (var i = 0; i < jSonCaptura[index].ListaJuntasSeleccionadasInicial.length; i++) {
                    var bandera = false;
                    for (var j = 0 ; j < jSonCaptura[index].ListaJuntasSeleccionadas.length ; j++) {
                        if (jSonCaptura[index].ListaJuntasSeleccionadasInicial[i].JuntaID == jSonCaptura[index].ListaJuntasSeleccionadas[j].JuntaID) {
                            listaFinalJuntas.push(jSonCaptura[index].ListaJuntasSeleccionadasInicial[i]);
                            bandera = true;
                        }
                        if ((jSonCaptura[index].ListaJuntasSeleccionadas.length - 1) == j && bandera == false) {
                            jSonCaptura[index].ListaJuntasSeleccionadasInicial[i].Accion = 3;
                            listaFinalJuntas.push(jSonCaptura[index].ListaJuntasSeleccionadasInicial[i]);
                        }
                    }
                }
                if (jSonCaptura[index].ListaJuntasSeleccionadasInicial.length == 0 || jSonCaptura[index].ListaJuntasSeleccionadasInicial == null) {
                    listaFinalJuntas = jSonCaptura[index].ListaJuntasSeleccionadas;
                }
                else {
                    for (var i = 0; i < jSonCaptura[index].ListaJuntasSeleccionadas.length; i++) {
                        var bandera = false;
                        for (var j = 0 ; j < jSonCaptura[index].ListaJuntasSeleccionadasInicial.length ; j++) {
                            if (jSonCaptura[index].ListaJuntasSeleccionadas[i].JuntaID != jSonCaptura[index].ListaJuntasSeleccionadasInicial[j].JuntaID) {
                                bandera = true;
                            }
                            if ((jSonCaptura[index].ListaJuntasSeleccionadasInicial.length - 1) == j && bandera == true && jSonCaptura[index].ListaJuntasSeleccionadas[i].Accion == 1) {
                                listaFinalJuntas.push(jSonCaptura[index].ListaJuntasSeleccionadas[i]);
                            }

                        }
                    }
                }
            }
            //.........................................
            if (jSonCaptura[index].ListaJuntasSeleccionadasInicial == null) {
                listaFinalJuntas = jSonCaptura[index].ListaJuntasSeleccionadas;
            }
            if (jSonCaptura[index].Accion == 4)
                listaFinalJuntas = [];

        }
        else {
            var cont = 0;
            for (var i = 0; i < jSonCaptura[index].ListaJuntasSeleccionadas.length; i++) {
                listaFinalJuntas[cont] = jSonCaptura[index].ListaJuntasSeleccionadas[i];
                cont++;
            }
            for (var i = 0; i < jSonCaptura[index].ListaJuntasSeleccionadasInicial.length; i++) {
                jSonCaptura[index].ListaJuntasSeleccionadasInicial[i].Accion = 3;
                listaFinalJuntas[cont] = jSonCaptura[index].ListaJuntasSeleccionadasInicial[i];
                cont++;
            }
            if (jSonCaptura[index].Accion == 4)
                listaFinalJuntas = [];
        }


        if (listaFinalJuntas != null && listaFinalJuntas.length > 0) {
            inspeccionDimensional[index].ListaJuntas = [];
            Juntas = [];
            for (var r = 0; r < listaFinalJuntas.length; r++) {
                Juntas[r] = { Accion: "", OrdenTrabajoSpoolID: "", DefectoID: "", JuntaID: "" }
                Juntas[r].DefectoID = jSonCaptura[index].DefectosID == "" ? 0 : jSonCaptura[index].DefectosID;
                Juntas[r].JuntaID = listaFinalJuntas[r].JuntaID;
                Juntas[r].Accion = listaFinalJuntas[r].Accion
                Juntas[r].OrdenTrabajoSpoolID = jSonCaptura[index].OrdenTrabajoSpoolID
            }
            inspeccionDimensional[index].ListaJuntas = Juntas;
        }
        else {
            if (jSonCaptura[index].ListaJuntasSeleccionadasInicial != null && jSonCaptura[index].ListaJuntasSeleccionadasInicial.length > 0) {
                Juntas = [];
                for (var r = 0; r < jSonCaptura[index].ListaJuntasSeleccionadasInicial.length; r++) {
                    Juntas[r] = { Accion: "", OrdenTrabajoSpoolID: "", DefectoID: "", JuntaID: "" }
                    Juntas[r].DefectoID = jSonCaptura[index].DefectosID == "" ? 0 : jSonCaptura[index].DefectosID;
                    Juntas[r].JuntaID = jSonCaptura[index].ListaJuntasSeleccionadasInicial[r].JuntaID;
                    Juntas[r].Accion = 3;//automaticamente es tres porque al inicio se tenia y al final se elimino todo.
                    Juntas[r].OrdenTrabajoSpoolID = jSonCaptura[index].OrdenTrabajoSpoolID;
                }
                inspeccionDimensional[index].ListaJuntas = Juntas;
            }
            else
                inspeccionDimensional[index].ListaJuntas = undefined;
        }


        if (inspeccionDimensional[index].Accion == 4) {
            if (!((inspeccionDimensional[index].InspectorID == "" || inspeccionDimensional[index].InspectorID == "0" || inspeccionDimensional[index].InspectorID == 0) &&
                inspeccionDimensional[index].FechaInspeccion == "" &&
                (inspeccionDimensional[index].DefectosID == "" || inspeccionDimensional[index].DefectosID == "0" || inspeccionDimensional[index].DefectosID == 0) &&
                (inspeccionDimensional[index].ResultadoID == "" || inspeccionDimensional[index].ResultadoID == "0" || inspeccionDimensional[index].ResultadoID == 0) &&
                (inspeccionDimensional[index].ListaJuntas == undefined || JuntasEliminadasGuardar(inspeccionDimensional[index].ListaJuntas)))
                ) {
                inspeccionDimensional[index].Estatus = 0;
                $("#grid").data("kendoGrid").dataSource._data[index].RowOk = false;
            }
        }
        else {

            if ((inspeccionDimensional[index].FechaInspeccion == "" ||
                (inspeccionDimensional[index].ResultadoID == "" || inspeccionDimensional[index].ResultadoID == "0" || inspeccionDimensional[index].ResultadoID == 0) ||
                (inspeccionDimensional[index].InspectorID == "" || inspeccionDimensional[index].InspectorID == "0" || inspeccionDimensional[index].InspectorID == 0)) &&
                (inspeccionDimensional[index].Accion == 2 || inspeccionDimensional[index].Accion == 1)
                ) {
                inspeccionDimensional[index].Estatus = 0;
                $("#grid").data("kendoGrid").dataSource._data[index].RowOk = false;
            }
            else {
                if (inspeccionDimensional[index].ResultadoID == "1") {
                    if (!((inspeccionDimensional[index].DefectosID == "" || inspeccionDimensional[index].DefectosID == "0" || inspeccionDimensional[index].DefectosID == 0) &&
                        inspeccionDimensional[index].ListaJuntas == undefined || JuntasEliminadasGuardar(inspeccionDimensional[index].ListaJuntas))
                        ) {
                        inspeccionDimensional[index].Estatus = 0;
                        $("#grid").data("kendoGrid").dataSource._data[index].RowOk = false;
                    }
                }
                else if (inspeccionDimensional[index].ResultadoID == "2") {
                    if ((inspeccionDimensional[index].DefectosID == "" || inspeccionDimensional[index].DefectosID == "0" || inspeccionDimensional[index].DefectosID == 0) && !JuntasCorrectasGuardar(inspeccionDimensional[index].ListaJuntas)) {
                        inspeccionDimensional[index].Estatus = 0;
                        $("#grid").data("kendoGrid").dataSource._data[index].RowOk = false;
                    }
                    else {
                        if (jSonCaptura[index].TIPO != "NoEspecificarJunta") {
                           
                            if (!JuntasCorrectasGuardar(inspeccionDimensional[index].ListaJuntas)) {
                                inspeccionDimensional[index].Estatus = 0;
                                $("#grid").data("kendoGrid").dataSource._data[index].RowOk = false;
                            }
                        }
                    }
                }
            }
        }


    }
    Captura[0].Detalles = inspeccionDimensional;

    if (!ExistRowEmptyAble(inspeccionDimensional)) {
        if (Captura[0].Detalles.length > 0) {
            $InspeccionDimensional.InspeccionDimensional.create(Captura[0], { token: Cookies.get("token"), lenguaje: $("#language").val() }).done(function (data) {
                if (Error(data)) {
                    if (data.ReturnMessage.length > 0 && data.ReturnMessage[0] == "Ok") {
                        if (tipoGuardado == 0) {
                            opcionHabilitarView(true, "FieldSetView");
                            AjaxGetSpoolGrid();
                        }
                        else if (tipoGuardado == 1) {
                            opcionHabilitarView(false, "FieldSetView");
                            limpiar();
                            AjaxCargaCamposPredetrminados();
                        }
                        displayNotify("CapturaMensajeGuardadoExitoso", "", '0');
                    }
                    else {
                        displayNotify("CapturaMensajeGuardadoErroneo", "", '2');
                        loadingStop();
                    }
                   
                }
            });
           
        }
        else {
            loadingStop();
        }
    }
    else {
        loadingStop();
        $("#grid").data("kendoGrid").dataSource.sync();
        ventanaConfirm = $("#ventanaConfirm").kendoWindow({
            iframe: true,
            title: _dictionary.TituloPopUpError[$("#language").data("kendoDropDownList").value()],
            visible: false, //the window will not appear before its .open method is called
            width: "auto",
            height: "auto",
            modal: true,
            actions: [
            ],
            animation: {
                close: false,
                open: false
            }
        }).data("kendoWindow");

        ventanaConfirm.content(_dictionary.MensajeConfirmacionGuardadoGeneral[$("#language").data("kendoDropDownList").value()] +
            "</br><center><button class='btn btn-blue' id='yesButton'>" + _dictionary.lblSi[$("#language").data("kendoDropDownList").value()] + "</button><button class='btn btn-blue' id='noButton'>" + _dictionary.lblNo[$("#language").data("kendoDropDownList").value()] + "</button></center>");

        ventanaConfirm.open().center();



        $("#yesButton").click(function () {
            loadingStart();

            ArregloGuardado = [];
            var indice = 0;
            for (var i = 0; i < Captura[0].Detalles.length; i++) {
                if (Captura[0].Detalles[i].Estatus == 1) {
                    ArregloGuardado[indice] = inspeccionDimensional[i];
                    indice++;
                }
            }

            Captura[0].Detalles = [];
            Captura[0].Detalles = ArregloGuardado;


            if (ArregloGuardado.length > 0) {
                $InspeccionDimensional.InspeccionDimensional.create(Captura[0], { token: Cookies.get("token"), lenguaje: $("#language").val() }).done(function (data) {
                    if (Error(data)) {
                        if (data.ReturnMessage.length > 0 && data.ReturnMessage[0] == "Ok") {
                            if (tipoGuardado == 0) {
                                opcionHabilitarView(true, "FieldSetView");
                                AjaxGetSpoolGrid();
                            }
                            else if (tipoGuardado == 1) {
                                opcionHabilitarView(false, "FieldSetView");
                                limpiar();
                                AjaxCargaCamposPredetrminados();
                            }
                            displayNotify("CapturaMensajeGuardadoExitoso", "", '0');
                        }
                        else {
                            displayNotify("CapturaMensajeGuardadoErroneo", "", '2');
                        }
                    }
                });
                loadingStop();
            }
            else {
                loadingStop();
                displayNotify("AdverteciaExcepcionGuardado", "", '1');
            }
            opcionHabilitarView(false, "FieldSetView");
            ventanaConfirm.close();
        });
        $("#noButton").click(function () {
            ventanaConfirm.close();
            opcionHabilitarView(false, "FieldSetView");
        });


    }

    
}

function InspectorCorrecto(array) {
    for (var i = 0 ; i < array.length ; i++) {
        if (array[i].InspectorID == "" || array[i].InspectorID == undefined || array[i].InspectorID == null) {
            return false;
        }
    }
    return true;
}


function AjaxGetSpoolGrid() {
    try {
        loadingStart();
        var listadogrid = $("#grid").data("kendoGrid").dataSource._data;
        $("#grid").data("kendoGrid").dataSource.data([]);

        for (var j = 0; j < listadogrid.length; j++) {
            $InspeccionDimensional.InspeccionDimensional.read({ OrdenTrabajoSpoolID: listadogrid[j].OrdenTrabajoSpoolID, OrdenTrabajoSpool: listadogrid[j].OrdenTrabajoSpool, ProyectoID: listadogrid[j].ProyectoID, token: Cookies.get("token"), Lenguaje: $("#language").val() }).done(function (data) {
                if (Error(data)) {
                    var ds = $("#grid").data("kendoGrid").dataSource;
                    var array = JSON.parse(data);
                    if (array.length > 0) {//1.- ahora solo trae un registro por spool porque en una sola llamada trae la junta.
                        for (var i = 0; i < array.length; i++) {
                            if (!ExisteSpool(array[i])) {
                                array[i].NumeroUnico1 = array[i].NumeroUnico1 == "" ? DatoDefaultNumeroUnico1() : array[i].NumeroUnico1;
                                array[i].NumeroUnico2 = array[i].NumeroUnico2 == "" ? DatoDefaultNumeroUnico2() : array[i].NumeroUnico2;
                                if (array[i].FechaInspeccion != null) {
                                    array[i].FechaInspeccion = new Date(ObtenerDato(array[i].FechaInspeccion, 1), ObtenerDato(array[i].FechaInspeccion, 2), ObtenerDato(array[i].FechaInspeccion, 3));//año, mes, dia
                                }
                                ds.add(array[i]);
                                //displayNotify('', array[i].OrdenTrabajoSpool, '0');

                                //como trae solo un registro se sincroniza se explica en el punto del comentario 1
                                if (i < listadogrid.length - 1) {
                                    $("#grid").data("kendoGrid").dataSource.sync();
                                    $("#InputID").data("kendoComboBox").value("");
                                }
                            }
                            else {
                                //MensajesSteelGO("SpoolIDExistente", '');
                            }
                        }
                    }
                    else {
                        //mensaje que no existe el spool.
                        displayNotify("CapturaInspeccionDimensionalResultAjaxEmpty", '', '2');
                    }
                }
            });

            //loadingStop();
        }

    } catch (e) {
        loadingStop();
        displayNotify("Mensajes_error", e.message, '2');

    }
}