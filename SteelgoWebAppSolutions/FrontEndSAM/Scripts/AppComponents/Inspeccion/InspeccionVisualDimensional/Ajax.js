var TipoObrero = "Inspector Visual Dimensional";
var TipoConsultaObrero = 2;
var TipoPruebaDimensional = "Inspección dimensional";
var TipoPruebaVisual = "Inspección visual";
var CampoFechaDimensionalPredeterminada = 4;
var CampoFechaVisualPredeterminada = 16;
var CampoResultadoDimensionalPredeterminada = 18;
var CampoResultadoVisualPredeterminada = 20;
var CampoLlenadoPredeterminada = 22;
var idMostrarInformacion = 1047;
var listadoJuntasInicialGlobal = [];


function AjaxValidarNumerosUnicos(arregloCaptura, tipoGuardar) {
    Captura = [];
    Captura[0] = { Detalles: "" };
    ListaDetalles = [];
    var RequiereValidarNU;
    var numUnicosIncorrectosCliente = false;
    for (index = 0; index < arregloCaptura.length; index++) {
        ListaDetalles[index] = { JuntaSpoolID: "", NumeroUnico1ID: "", NumeroUnico2ID: "", Localizacion1: "", Localizacion2: "" };
        if (RequiereValidarNU == undefined) {
            RequiereValidarNU = arregloCaptura[index].RevisionNU;
        }

        $("#grid").data("kendoGrid").dataSource._data[index].NUOk = true;
        ListaDetalles[index].JuntaSpoolID = arregloCaptura[index].JuntaID;
        ListaDetalles[index].NumeroUnico1ID = arregloCaptura[index].NumeroUnico1ID;
        ListaDetalles[index].NumeroUnico2ID = arregloCaptura[index].NumeroUnico2ID;
        ListaDetalles[index].Localizacion1 = arregloCaptura[index].Localizacion.split("-")[0];
        ListaDetalles[index].Localizacion2 = arregloCaptura[index].Localizacion.split("-")[1];




    }
    Captura[0].Detalles = ListaDetalles;


    $Armado.Armado.update(Captura[0], { token: Cookies.get("token") }).done(function (data) {
        if (RequiereValidarNU) {
            if (data.length == 0) {
                ajaxGuardado(arregloCaptura, tipoGuardar)
            }
            else {

                for (var j = 0; j < data.length; j++) {
                    for (var i = 0; i < arregloCaptura.length; i++) {
                        if (arregloCaptura[i].JuntaID == data[j] && arregloCaptura[i].NumeroUnico1ID != "0" && arregloCaptura[i].NumeroUnico2ID != "0") {
                            $("#grid").data("kendoGrid").dataSource._data[i].NUOk = false;
                        }
                    }
                }



                ValidarCaptura(arregloCaptura);

                displayNotify("CapturaArmadoMensajeJuntaIncorrectaPorNU", "", '1');
                $("#grid").data("kendoGrid").dataSource.sync();
            }
        }
        else {
            ajaxGuardado(arregloCaptura, tipoGuardar);
        }
    });

}

function ValidarCaptura(jSonCaptura) {
    Captura = [];
    Captura[0] = { Detalles: "" };
    var existRowEmpty = false;
    var capturaSinDimensional = false;
    var capturaSinVisual = false;


    // Elementos del grid
    ListaDetalleGuardarInspeccionVisual = []



    // Comienza captura Visual
    for (index = 0; index < jSonCaptura.length; index++) {
        //$('tr[data-uid="' + jSonCaptura[index].uid + '"] ').css("background-color", "#ffffff");
        $("#grid").data("kendoGrid").dataSource._data[index].RowOk = true;

        ListaDetalleGuardarInspeccionVisual[index] = { Accion: "", DefectosID: "", ObreroID: "", FechaInspeccion: "", ResultadoID: "", TallerID: "", JuntaSpoolID: "", ListaNumeroUnicoAsignado: "" };
        ListaDetalleGuardarInspeccionVisual[index].Accion = jSonCaptura[index].Accion;
        ListaDetalleGuardarInspeccionVisual[index].DefectosID = jSonCaptura[index].DefectosID == null ? 0 : jSonCaptura[index].DefectosID;
        ListaDetalleGuardarInspeccionVisual[index].ObreroID = (jSonCaptura[index].InspectorID == null || jSonCaptura[index].InspectorID == "") ? 0 : jSonCaptura[index].InspectorID;
        ListaDetalleGuardarInspeccionVisual[index].FechaInspeccion = jSonCaptura[index].FechaInspeccion == null ? " " : kendo.toString(jSonCaptura[index].FechaInspeccion, String(_dictionary.FormatoFecha[$("#language").data("kendoDropDownList").value()].replace('{', '').replace('}', '').replace("0:", ""))).trim();
        ListaDetalleGuardarInspeccionVisual[index].ResultadoID = jSonCaptura[index].ResultadoID == null ? 0 : jSonCaptura[index].ResultadoID;
        ListaDetalleGuardarInspeccionVisual[index].TallerID = jSonCaptura[index].TallerID == null ? 0 : jSonCaptura[index].TallerID;
        ListaDetalleGuardarInspeccionVisual[index].JuntaSpoolID = jSonCaptura[index].JuntaID == null ? 0 : jSonCaptura[index].JuntaID;

        ObjetoNumeroUnicoAsignado = []
        ObjetoNumeroUnicoAsignado[0] = { Accion: "", JuntaID: "", NumeroUnico1ID: "", NumeroUnico2ID: "" }
        ObjetoNumeroUnicoAsignado[0].Accion = jSonCaptura[index].AccionNumeroUnico;//jSonCaptura[index].RevisionNU ? jSonCaptura[index].AccionNumeroUnico : 0;//(jSonCaptura[index].Accion == 3 || jSonCaptura[index].Accion == 4) ? jSonCaptura[index].Accion : jSonCaptura[index].AccionNumeroUnico;
        ObjetoNumeroUnicoAsignado[0].JuntaID = jSonCaptura[index].JuntaID;
        ObjetoNumeroUnicoAsignado[0].NumeroUnico1ID = jSonCaptura[index].NumeroUnico1ID;
        ObjetoNumeroUnicoAsignado[0].NumeroUnico2ID = jSonCaptura[index].NumeroUnico2ID;
        ListaDetalleGuardarInspeccionVisual[index].ListaNumeroUnicoAsignado = ObjetoNumeroUnicoAsignado;


        if (jSonCaptura[index].RevisionNU) {
            if ((ListaDetalleGuardarInspeccionVisual[index].TallerID == "" || ListaDetalleGuardarInspeccionVisual[index].TallerID == "0" ||
				ListaDetalleGuardarInspeccionVisual[index].ResultadoID == "" || ListaDetalleGuardarInspeccionVisual[index].ResultadoID == "0" ||
				((ListaDetalleGuardarInspeccionVisual[index].DefectosID == "" || ListaDetalleGuardarInspeccionVisual[index].DefectosID == "0" || ListaDetalleGuardarInspeccionVisual[index].DefectosID == null) && ListaDetalleGuardarInspeccionVisual[index].ResultadoID == 2) ||
				ListaDetalleGuardarInspeccionVisual[index].ObreroID == "" || ListaDetalleGuardarInspeccionVisual[index].ObreroID == "0" ||
				ListaDetalleGuardarInspeccionVisual[index].FechaInspeccion == " " || ListaDetalleGuardarInspeccionVisual[index].FechaInspeccion == "0" ||
				ListaDetalleGuardarInspeccionVisual[index].ListaNumeroUnicoAsignado[0].NumeroUnico1ID == "" || ListaDetalleGuardarInspeccionVisual[index].ListaNumeroUnicoAsignado[0].NumeroUnico1ID == "0" || ListaDetalleGuardarInspeccionVisual[index].ListaNumeroUnicoAsignado[0].NumeroUnico1ID == null ||
				ListaDetalleGuardarInspeccionVisual[index].ListaNumeroUnicoAsignado[0].NumeroUnico2ID == "" || ListaDetalleGuardarInspeccionVisual[index].ListaNumeroUnicoAsignado[0].NumeroUnico2ID == "0" || ListaDetalleGuardarInspeccionVisual[index].ListaNumeroUnicoAsignado[0].NumeroUnico2ID == null
			) && (ListaDetalleGuardarInspeccionVisual[index].Accion != 4 && ListaDetalleGuardarInspeccionVisual[index].Accion != 3)) {

                if (ListaDetalleGuardarInspeccionVisual[index].Accion == 2) ListaDetalleGuardarInspeccionVisual[index].Accion = 4;

                ListaDetalleGuardarInspeccionVisual[index].Estatus = 0;
                $("#grid").data("kendoGrid").dataSource._data[index].RowOk = false;
                existRowEmpty = true;
            }
            else if (ListaDetalleGuardarInspeccionVisual[index].Accion == 4) {
                if (!(ListaDetalleGuardarInspeccionVisual[index].TallerID == "" || ListaDetalleGuardarInspeccionVisual[index].TallerID == "0" ||
					ListaDetalleGuardarInspeccionVisual[index].ResultadoID == "" || ListaDetalleGuardarInspeccionVisual[index].ResultadoID == "0" ||
					((ListaDetalleGuardarInspeccionVisual[index].DefectosID == "" || ListaDetalleGuardarInspeccionVisual[index].DefectosID == "0" || ListaDetalleGuardarInspeccionVisual[index].DefectosID == null) && ListaDetalleGuardarInspeccionVisual[index].ResultadoID == 2) ||
					ListaDetalleGuardarInspeccionVisual[index].ObreroID == "" || ListaDetalleGuardarInspeccionVisual[index].ObreroID == "0" ||
					ListaDetalleGuardarInspeccionVisual[index].FechaInspeccion == " " || ListaDetalleGuardarInspeccionVisual[index].FechaInspeccion == "0" ||
					ListaDetalleGuardarInspeccionVisual[index].ListaNumeroUnicoAsignado[0].NumeroUnico1ID == "" || ListaDetalleGuardarInspeccionVisual[index].ListaNumeroUnicoAsignado[0].NumeroUnico1ID == "0" || ListaDetalleGuardarInspeccionVisual[index].ListaNumeroUnicoAsignado[0].NumeroUnico1ID == null ||
					ListaDetalleGuardarInspeccionVisual[index].ListaNumeroUnicoAsignado[0].NumeroUnico2ID == "" || ListaDetalleGuardarInspeccionVisual[index].ListaNumeroUnicoAsignado[0].NumeroUnico2ID == "0" || ListaDetalleGuardarInspeccionVisual[index].ListaNumeroUnicoAsignado[0].NumeroUnico2ID == null)) {

                    ListaDetalleGuardarInspeccionVisual[index].Estatus = 0;
                    $("#grid").data("kendoGrid").dataSource._data[index].RowOk = false;
                    existRowEmpty = true;
                }
            }
        }
        else {
            if ((ListaDetalleGuardarInspeccionVisual[index].TallerID == "" || ListaDetalleGuardarInspeccionVisual[index].TallerID == "0" ||
				ListaDetalleGuardarInspeccionVisual[index].ResultadoID == "" || ListaDetalleGuardarInspeccionVisual[index].ResultadoID == "0" ||
				((ListaDetalleGuardarInspeccionVisual[index].DefectosID == "" || ListaDetalleGuardarInspeccionVisual[index].DefectosID == "0" || ListaDetalleGuardarInspeccionVisual[index].DefectosID == null) && ListaDetalleGuardarInspeccionVisual[index].ResultadoID == 2) ||
				ListaDetalleGuardarInspeccionVisual[index].ObreroID == "" || ListaDetalleGuardarInspeccionVisual[index].ObreroID == "0" ||
				ListaDetalleGuardarInspeccionVisual[index].FechaInspeccion == " " || ListaDetalleGuardarInspeccionVisual[index].FechaInspeccion == "0"
			) && (ListaDetalleGuardarInspeccionVisual[index].Accion != 4 && ListaDetalleGuardarInspeccionVisual[index].Accion != 3)) {

                if (ListaDetalleGuardarInspeccionVisual[index].Accion == 2) ListaDetalleGuardarInspeccionVisual[index].Accion = 4;

                ListaDetalleGuardarInspeccionVisual[index].Estatus = 0;
                $("#grid").data("kendoGrid").dataSource._data[index].RowOk = false;
                existRowEmpty = true;
            }
            else if (ListaDetalleGuardarInspeccionVisual[index].Accion == 4) {
                if (!(ListaDetalleGuardarInspeccionVisual[index].TallerID == "" || ListaDetalleGuardarInspeccionVisual[index].TallerID == "0" ||
					ListaDetalleGuardarInspeccionVisual[index].ResultadoID == "" || ListaDetalleGuardarInspeccionVisual[index].ResultadoID == "0" ||
					((ListaDetalleGuardarInspeccionVisual[index].DefectosID == "" || ListaDetalleGuardarInspeccionVisual[index].DefectosID == "0" || ListaDetalleGuardarInspeccionVisual[index].DefectosID == null) && ListaDetalleGuardarInspeccionVisual[index].ResultadoID == 2) ||
					ListaDetalleGuardarInspeccionVisual[index].ObreroID == "" || ListaDetalleGuardarInspeccionVisual[index].ObreroID == "0" ||
					ListaDetalleGuardarInspeccionVisual[index].FechaInspeccion == " " || ListaDetalleGuardarInspeccionVisual[index].FechaInspeccion == "0")) {

                    ListaDetalleGuardarInspeccionVisual[index].Estatus = 0;
                    $("#grid").data("kendoGrid").dataSource._data[index].RowOk = false;
                    existRowEmpty = true;
                }
            }
        }
    }

}

function ajaxObtenerListaInspector() {
    loadingStart();
    $Obrero.Obrero.read({ idProyecto: 0, tipo: TipoConsultaObrero, token: Cookies.get("token"), TipoObrero: TipoObrero }).done(function (data) {
        $("#inputInspector").data("kendoComboBox").value("");
        $("#inputInspector").data("kendoComboBox").dataSource.data(data);
        loadingStop();
    });
}

function ajaxObtenerListaTaller() {
    try {
        $Armado.Armado.read({ idProyecto: Cookies.get("Proyecto").split('°')[0], token: Cookies.get("token") }).done(function (data) {
            $("#inputTaller").data("kendoComboBox").value("");
            $("#inputTaller").data("kendoComboBox").dataSource.data(data);
        });
    }
    catch (e) {
    }
}

function ajaxJunta(spoolID) {
    $InspeccionVisualDimensional.VisualDimensional.read({ ordenTrabajo: $("#InputOrdenTrabajo").val(), id: spoolID, sinCaptura: 1, token: Cookies.get("token"), todos: 1 }).done(function (data) {
        if (Error(data)) {
            $("#ListaJuntas").data("kendoMultiSelect").dataSource.data(data);
        }
    });
}

function ajaxObtenerListaInspectorVisual() {
    $Obrero.Obrero.read({ idProyecto: 0, tipo: TipoConsultaObrero, token: Cookies.get("token"), TipoObrero: TipoObrero }).done(function (data) {
        $("#inputInspectorVisual").data("kendoComboBox").value("");
        $("#inputInspectorVisual").data("kendoComboBox").dataSource.data(data)

    });
}

function ajaxObtenerListaDefectosDimensionales() {

    $Defectos.Defectos.read({ lenguaje: $("#language").val(), TipoPrueba: TipoPruebaDimensional, token: Cookies.get("token") }).done(function (data) {
        $("#inputDefecto").data("kendoComboBox").value("");
        $("#inputDefecto").data("kendoComboBox").dataSource.data(data)
    });
}

function ajaxObtenerListaDefectosVisuales() {

    $Defectos.Defectos.read({ lenguaje: $("#language").val(), TipoPrueba: TipoPruebaVisual, token: Cookies.get("token") }).done(function (data) {
        $("#inputDefectosVisual").data("kendoComboBox").value("");
        $("#inputDefectosVisual").data("kendoComboBox").dataSource.data(data)

    });
}

function ajaxCargaCamposPredeterminados() {

    $CamposPredeterminados.CamposPredeterminados.read({ token: Cookies.get("token"), lenguaje: $("#language").val(), id: CampoFechaDimensionalPredeterminada }).done(function (data) {
        var NewDate = kendo.toString(data, _dictionary.FormatoFecha[$("#language").data("kendoDropDownList").value()]);
        endRangeDate.val(NewDate);
    });

    $CamposPredeterminados.CamposPredeterminados.read({ token: Cookies.get("token"), lenguaje: $("#language").val(), id: CampoFechaVisualPredeterminada }).done(function (data) {
        var NewDate2 = kendo.toString(data, _dictionary.FormatoFecha[$("#language").data("kendoDropDownList").value()]);
        endRangeDateV.val(NewDate2);
    });
    $CamposPredeterminados.CamposPredeterminados.read({ token: Cookies.get("token"), lenguaje: $("#language").val(), id: CampoResultadoVisualPredeterminada }).done(function (data) {
        if (data == "Aprobado") {
            $('input:radio[name=ResultadoVisual]:nth(0)').trigger("click");
            //$('input:radio[name=ResultadoVisual]:nth(1)').attr('checked', false);
            $("input:radio[name=ResultadoVisual]:checked").change();

        }
        else if (data == "Rechazado") {
            //$('input:radio[name=ResultadoVisual]:nth(0)').attr('checked', false);
            $('input:radio[name=ResultadoVisual]:nth(1)').trigger("click");
            $("input:radio[name=ResultadoVisual]:checked").change();
        }
        else if (data == "Ninguno") {
            $('input:radio[name=ResultadoVisual]:nth(0)').attr('checked', false);
            $('input:radio[name=ResultadoVisual]:nth(1)').attr('checked', false);
            $('input:radio[name=ResultadoVisual]:nth(2)').trigger("click");
            $("input:radio[name=ResultadoVisual]:checked").change();
        }

    });
    $CamposPredeterminados.CamposPredeterminados.read({ token: Cookies.get("token"), lenguaje: $("#language").val(), id: CampoLlenadoPredeterminada }).done(function (data) {
        if (data == "Todos") {
            $('input:radio[name=LLena]:nth(0)').trigger("click");
            //$('input:radio[name=LLena]:nth(1)').attr('checked', false);
            $("input:radio[name=LLena]:checked").change();

        }
        else if (data == "Vacios") {
            //$('input:radio[name=LLena]:nth(0)').attr('checked', false);
            $('input:radio[name=LLena]:nth(1)').trigger("click");
            $("input:radio[name=LLena]:checked").change();

        }

    });

}

function ajaxObtenerSpoolID() {
    try {
        $CapturasRapidas.CapturasRapidas.read({ ordenTrabajo: $("#InputOrdenTrabajo").val(), tipo: '1', token: Cookies.get("token"), lenguaje: $("#language").val() }).done(function (data) {
            loadingStart();


            if (OrdenTrabajoSpoolAnterior == undefined) {
                OrdenTrabajoSpoolAnterior = data;
                OrdenTrabajoSpoolActual = data;
            }
            else {
                OrdenTrabajoSpoolActual = data;
            }

            $("#InputOrdenTrabajo").val(data.OrdenTrabajo);
            $("#InputID").data("kendoComboBox").dataSource.data(data.idStatus)
            Cookies.set("LetraProyecto", data.OrdenTrabajo.substring(0, 1), { path: '/' });
            loadingStop();
        });
    } catch (e) {
        displayNotify("", e.message, '2');
    }
}

function ajaxobtenerDetalleDimensional(spoolID) {

    $InspeccionVisualDimensional.VisualDimensional.read({ id: spoolID, sinCaptura: 1, token: Cookies.get("token"), lenguaje: $("#language").val() }).done(function (data) {
        if (data.ListaDetalleDimensional[0].ResultadoID != 0) {
            if (data.ListaDetalleDimensional[0].ResultadoID == 1) {//aprobado
                $('input:radio[name=ResultadoDimensional]:nth(0)').trigger("click");
                //$('input:radio[name=ResultadoDimensional]:nth(1)').attr('checked', false);
                $("input:radio[name=ResultadoDimensional]:checked").change();
            }
            else if (data.ListaDetalleDimensional[0].ResultadoID == 2) {//rechazado
                //$('input:radio[name=ResultadoDimensional]:nth(0)').attr('checked', false);
                $('input:radio[name=ResultadoDimensional]:nth(1)').trigger("click");
                $("input:radio[name=ResultadoDimensional]:checked").change();
            }

            $("#inputDefecto").data("kendoComboBox").value(data.ListaDetalleDimensional[0].DefectoID == 0 ? "" : data.ListaDetalleDimensional[0].DefectoID);
            $("#inputInspector").data("kendoComboBox").value(data.ListaDetalleDimensional[0].ObreroID);
            $("#ListaJuntas").data("kendoMultiSelect").dataSource.data([]);
            $("#ListaJuntas").data("kendoMultiSelect").dataSource.data(data.ListaDetalleDimensional[0].ListaJuntas);
            $("#InspeccionDimensionalID").val(data.ListaDetalleDimensional[0].InspeccionDimensionalID);
            endRangeDate.val(data.ListaDetalleDimensional[0].FechaInspeccion);








            listadoJuntasInicialGlobal = data.ListaDetalleDimensional[0].ListaJuntasSeleccionadas.length > 0 ? data.ListaDetalleDimensional[0].ListaJuntasSeleccionadas : [];
            if (data.ListaDetalleDimensional[0].ListaJuntasSeleccionadas.length > 0) {
                editado = true;
                var valores = [];
                for (var i = 0; i < data.ListaDetalleDimensional[0].ListaJuntasSeleccionadas.length; i++) {
                    valores[i] = data.ListaDetalleDimensional[0].ListaJuntasSeleccionadas[i].JuntaID;
                };

                $("#ListaJuntas").data("kendoMultiSelect").value(valores);
            }

            if ($("#inputDefecto").data("kendoComboBox").select() > 0) {
                if ($("#inputDefecto").data("kendoComboBox").dataItem().TIPO == "NoEspecificarJunta")
                    $("#ListaJuntas").data("kendoMultiSelect").enable(false);
                else {
                    if ($('#Guardar').text() == "Guardar" || $('#Guardar').text() == "Save") {
                        $("#ListaJuntas").data("kendoMultiSelect").enable(true);
                    }
                    else
                        $("#ListaJuntas").data("kendoMultiSelect").enable(false);
                }
            }
            else
                $("#ListaJuntas").data("kendoMultiSelect").enable(false);


        }
        else {
            $("#ListaJuntas").data("kendoMultiSelect").value("");
            $("#ListaJuntas").data("kendoMultiSelect").dataSource.data(data.ListaDetalleDimensional[0].ListaJuntas);
            if ($("#inputDefecto").data("kendoComboBox").select() > 0) {
                if ($("#inputDefecto").data("kendoComboBox").dataItem().TIPO == "NoEspecificarJunta")
                    $("#ListaJuntas").data("kendoMultiSelect").enable(false);
                else
                    $("#ListaJuntas").data("kendoMultiSelect").enable(true);
            }
            else
                $("#ListaJuntas").data("kendoMultiSelect").enable(false);
            $("#inputDefecto").data("kendoComboBox").value("");
            $("#inputDefecto").data("kendoComboBox").enable(false);
            $("#inputInspector").data("kendoComboBox").value("");
            $('input:radio[name=ResultadoDimensional]:nth(1)').attr('checked', false);
            $('input:radio[name=ResultadoDimensional]:nth(0)').attr('checked', false);
            $CamposPredeterminados.CamposPredeterminados.read({ token: Cookies.get("token"), lenguaje: $("#language").val(), id: CampoFechaDimensionalPredeterminada }).done(function (data) {
                var NewDate = kendo.toString(data, _dictionary.FormatoFecha[$("#language").data("kendoDropDownList").value()]);
                endRangeDate.val(NewDate);
            });
        }
    });
}



function ajaxObtenerJSonGridDespuesGuardar(rowsVisualDimensional) {
    loadingStart();
    try {
        $InspeccionVisualDimensional.VisualDimensional.read({ JsonCaptura: JSON.stringify(ArregloListadoCaptura()), token: Cookies.get("token"), Lenguaje: $("#language").val(), juntasSeleccionadas: $("#ListaJuntas").data("kendoMultiSelect").value().toString() }).done(function (data) {
            var desplegado = false;

            var ds = $("#grid").data("kendoGrid").dataSource;
            var array = JSON.parse(data);


            var ExistenJuntasGrid = [];
            var JuntasAgregadas = [];
            for (var j = 0; j < rowsVisualDimensional.length; j++) {
                for (var i = 0; i < array.length; i++) {
                    if (array[i].JuntaID == rowsVisualDimensional[j].JuntaID) {
                        JuntasAgregadas.push(array[i].Junta);
                        array[i].NumeroUnico1 = array[i].NumeroUnico1 == "" ? DatoDefaultNumeroUnico1() : array[i].NumeroUnico1;
                        array[i].NumeroUnico2 = array[i].NumeroUnico2 == "" ? DatoDefaultNumeroUnico2() : array[i].NumeroUnico2;
                        array[i].NumeroUnico1ID = array[i].NumeroUnico1ID == "" ? 0 : array[i].NumeroUnico1ID;
                        array[i].NumeroUnico2ID = array[i].NumeroUnico2ID == "" ? 0 : array[i].NumeroUnico2ID;
                        if (array[i].FechaInspeccion != null) {
                            array[i].FechaInspeccion = new Date(ObtenerDato(array[i].FechaInspeccion, 1), ObtenerDato(array[i].FechaInspeccion, 2), ObtenerDato(array[i].FechaInspeccion, 3));//año, mes, dia
                        }
                        ds.add(array[i]);
                    }
                }
            }


            //deshabilitaSpool();
            loadingStop();
        });
    } catch (e) {
        displayNotify("", e.message, '2');
    }
}

function ajaxObtenerJSonGrid() {
    loadingStart();
    try {
        $InspeccionVisualDimensional.VisualDimensional.read({ JsonCaptura: JSON.stringify(ArregloListadoCaptura()), token: Cookies.get("token"), Lenguaje: $("#language").val(), juntasSeleccionadas: $("#ListaJuntas").data("kendoMultiSelect").value().toString() }).done(function (data) {
            var desplegado = false;

            var ds = $("#grid").data("kendoGrid").dataSource;
            var array = JSON.parse(data);



            var ExistenJuntasGrid = [];
            var JuntasAgregadas = [];

            if (array.length > 0)
                editado = true;
            else
                editado = false;

            for (var i = 0; i < array.length; i++) {
                if (ExisteJunta(array[i].JuntaID)) {
                    JuntasAgregadas.push(array[i].Junta);
                    array[i].NumeroUnico1 = array[i].NumeroUnico1 == "" ? DatoDefaultNumeroUnico1() : array[i].NumeroUnico1;
                    array[i].NumeroUnico2 = array[i].NumeroUnico2 == "" ? DatoDefaultNumeroUnico2() : array[i].NumeroUnico2;
                    array[i].NumeroUnico1ID = array[i].NumeroUnico1ID == "" ? 0 : array[i].NumeroUnico1ID;
                    array[i].NumeroUnico2ID = array[i].NumeroUnico2ID == "" ? 0 : array[i].NumeroUnico2ID;
                    if (array[i].FechaInspeccion != null) {
                        array[i].FechaInspeccion = new Date(ObtenerDato(array[i].FechaInspeccion, 1), ObtenerDato(array[i].FechaInspeccion, 2), ObtenerDato(array[i].FechaInspeccion, 3));//año, mes, dia
                    }
                    ds.add(array[i]);
                }
                else
                    ExistenJuntasGrid.push(array[i].Junta)

            }
            if (array.length > 0 && !($('#Guardar').text() == "Editar" || $('#Guardar').text() == "Edit")) {
                if (!desplegado) {
                    desplegado = true;

                    if (ExistenJuntasGrid.length > 0) {
                        displayNotify("", _dictionary.DimensionalVisualMensajeJuntasRepetidas[$("#language").data("kendoDropDownList").value()].replace("?1", ExistenJuntasGrid), '2');
                    }
                    if (JuntasAgregadas.length > 0) {
                        //$("#InputID").data("kendoComboBox").value("");
                        displayNotify("", _dictionary.DimensionalVisualMensajeJuntasAgregadas[$("#language").data("kendoDropDownList").value()] + JuntasAgregadas, '0');
                    }

                }



            }

            //deshabilitaSpool();
            loadingStop();
        });
    } catch (e) {
        displayNotify("", e.message, '2');
    }
}

function ajaxGuardado(jSonCaptura, tipoGuardar) {
    if ($("#InputID").data("kendoComboBox").dataItem($("#InputID").data("kendoComboBox").select()) != undefined && $("#InputID").data("kendoComboBox").dataItem($("#InputID").data("kendoComboBox").select()).Valor != "0" && $("#InputID").data("kendoComboBox").dataItem($("#InputID").data("kendoComboBox").select()).Valor != "") {
        Captura = [];
        Captura[0] = { Detalles: "" };
        var existRowEmpty = false;
        var capturaSinDimensional = false;
        var capturaSinVisual = false;

        inspeccionDimensional = [];

        Juntas = [];

        // Elementos del grid
        ListaDetalleGuardarInspeccionVisual = []

        // Elementos Inspección Dimensional
        inspeccionDimensional[0] = { Accion: "", Lenguaje: "", InspeccionDimensionalID: "", OrdenTrabajoSpoolID: "", FechaInspeccion: "", ResultadoID: "", ObreroID: "", DefectoID: "", ListaDetalleGuardarInspeccionVisual: "", ListaJuntas: "" }
        inspeccionDimensional[0].Lenguaje = $("#language").val();
        inspeccionDimensional[0].InspeccionDimensionalID = $("#InspeccionDimensionalID").val();
        inspeccionDimensional[0].OrdenTrabajoSpoolID = ordentrabajoSpoolID.Valor;
        inspeccionDimensional[0].FechaInspeccion = $("#FechaInspeccion").val().trim() == null ? "" : kendo.toString($("#FechaInspeccion").val().trim(), String(_dictionary.FormatoFecha[$("#language").data("kendoDropDownList").value()].replace('{', '').replace('}', '').replace("0:", ""))).trim()

        var ResultadoDimensional = $('input:radio[name=ResultadoDimensional]:checked').val();
        var Defecto = $("#inputDefecto").data("kendoComboBox").select();
        var tipoDefecto = Defecto == -1 ? 0 : $("#inputDefecto").data("kendoComboBox").dataItem(Defecto).TIPO;
        var Inspector = $("#inputInspector").data("kendoComboBox").select();

        if (ResultadoDimensional == "Aprobado") inspeccionDimensional[0].ResultadoID = 1;
        else if (ResultadoDimensional == "Rechazado") inspeccionDimensional[0].ResultadoID = 2;
        else {
            inspeccionDimensional[0].ResultadoID = 0;
            capturaSinDimensional = true;
        }

        if (inspeccionDimensional[0].InspeccionDimensionalID == 0) inspeccionDimensional[0].Accion = 1;
        else inspeccionDimensional[0].Accion = 2;


        if (Defecto > 0) inspeccionDimensional[0].DefectoID = $("#inputDefecto").data("kendoComboBox").dataItem(Defecto).DefectoID;
        else if (ResultadoDimensional == "Rechazado") capturaSinDimensional = true;

        if (Inspector > 0) inspeccionDimensional[0].ObreroID = $("#inputInspector").data("kendoComboBox").dataItem(Inspector).ObreroID;
        else capturaSinDimensional = true;

        if (inspeccionDimensional[0].FechaInspeccion == "") capturaSinDimensional = true;

        // Proceso Juntas Dimensional
        var juntasListado = $("#ListaJuntas").data("kendoMultiSelect")._dataItems;
        if (juntasListado.length > 0) {
            for (var r = 0; r < juntasListado.length; r++) {
                Juntas[r] = { Accion: "", OrdenTrabajoSpoolID: "", DefectoID: "", JuntaID: "" }
                Juntas[r].DefectoID = Defecto > 0 ? $("#inputDefecto").data("kendoComboBox").dataItem($("#inputDefecto").data("kendoComboBox").select()).DefectoID : null;
                Juntas[r].JuntaID = $("#ListaJuntas").data("kendoMultiSelect")._dataItems[r].JuntaID;

                if (listadoJuntasInicialGlobal.length == 0) Juntas[r].Accion = 1;
                else {
                    var existeJunta = false;
                    for (var indexLista = 0; indexLista < listadoJuntasInicialGlobal.length; indexLista++)
                        if (listadoJuntasInicialGlobal[indexLista].JuntaID == $("#ListaJuntas").data("kendoMultiSelect")._dataItems[r].JuntaID) existeJunta = true;

                    if (existeJunta) Juntas[r].Accion = 2;
                    else Juntas[r].Accion = 1;
                }

                Juntas[r].OrdenTrabajoSpoolID = ordentrabajoSpoolID.Valor;
            }
        }

        for (var indexAccion3 = 0; indexAccion3 < listadoJuntasInicialGlobal.length; indexAccion3++) {
            var existeJuntaGlobal = false;
            for (var indexMultiSelect = 0; indexMultiSelect < juntasListado.length; indexMultiSelect++)
                if (listadoJuntasInicialGlobal[indexAccion3].JuntaID == juntasListado[indexMultiSelect].JuntaID) existeJuntaGlobal = true;
            if (!existeJuntaGlobal)
                Juntas.push({
                    Accion: 3,
                    OrdenTrabajoSpoolID: ordentrabajoSpoolID.Valor,
                    DefectoID: Defecto > 0 ? $("#inputDefecto").data("kendoComboBox").dataItem($("#inputDefecto").data("kendoComboBox").select()).DefectoID : null,
                    JuntaID: listadoJuntasInicialGlobal[indexAccion3].JuntaID
                });
        }

        inspeccionDimensional[0].ListaJuntas = Juntas.length > 0 ? Juntas : undefined;
        Captura[0].Detalles[0] = inspeccionDimensional;

        if (Defecto <= 0 && Inspector <= 0 && juntasListado.length == 0 && inspeccionDimensional[0].InspeccionDimensionalID != 0) {
            capturaSinDimensional = false;
            inspeccionDimensional[0].Accion = 3;
        }

        // Finaliza captura datos Dimensional

        // Comienza captura Visual
        for (index = 0; index < jSonCaptura.length; index++) {
            //$('tr[data-uid="' + jSonCaptura[index].uid + '"] ').css("background-color", "#ffffff");
            $("#grid").data("kendoGrid").dataSource._data[index].RowOk = true;
            $("#grid").data("kendoGrid").dataSource._data[index].NUOk = true;

            ListaDetalleGuardarInspeccionVisual[index] = { Accion: "", DefectosID: "", ObreroID: "", FechaInspeccion: "", ResultadoID: "", TallerID: "", JuntaSpoolID: "", ListaNumeroUnicoAsignado: "" };
            ListaDetalleGuardarInspeccionVisual[index].Accion = jSonCaptura[index].Accion;
            ListaDetalleGuardarInspeccionVisual[index].DefectosID = jSonCaptura[index].DefectosID == null ? 0 : jSonCaptura[index].DefectosID;
            ListaDetalleGuardarInspeccionVisual[index].ObreroID = (jSonCaptura[index].InspectorID == null || jSonCaptura[index].InspectorID == "") ? 0 : jSonCaptura[index].InspectorID;
            ListaDetalleGuardarInspeccionVisual[index].FechaInspeccion = jSonCaptura[index].FechaInspeccion == null ? " " : kendo.toString(jSonCaptura[index].FechaInspeccion, String(_dictionary.FormatoFecha[$("#language").data("kendoDropDownList").value()].replace('{', '').replace('}', '').replace("0:", ""))).trim();
            ListaDetalleGuardarInspeccionVisual[index].ResultadoID = jSonCaptura[index].ResultadoID == null ? 0 : jSonCaptura[index].ResultadoID;
            ListaDetalleGuardarInspeccionVisual[index].TallerID = jSonCaptura[index].TallerID == null ? 0 : jSonCaptura[index].TallerID;
            ListaDetalleGuardarInspeccionVisual[index].JuntaSpoolID = jSonCaptura[index].JuntaID == null ? 0 : jSonCaptura[index].JuntaID;

            ObjetoNumeroUnicoAsignado = []
            ObjetoNumeroUnicoAsignado[0] = { Accion: "", JuntaID: "", NumeroUnico1ID: "", NumeroUnico2ID: "" }
            ObjetoNumeroUnicoAsignado[0].Accion = jSonCaptura[index].AccionNumeroUnico;// jSonCaptura[index].RevisionNU ? jSonCaptura[index].AccionNumeroUnico : 0;//(jSonCaptura[index].Accion == 3 || jSonCaptura[index].Accion == 4) ? jSonCaptura[index].Accion : jSonCaptura[index].AccionNumeroUnico;
            ObjetoNumeroUnicoAsignado[0].JuntaID = jSonCaptura[index].JuntaID;
            ObjetoNumeroUnicoAsignado[0].NumeroUnico1ID = jSonCaptura[index].NumeroUnico1ID;
            ObjetoNumeroUnicoAsignado[0].NumeroUnico2ID = jSonCaptura[index].NumeroUnico2ID;
            ListaDetalleGuardarInspeccionVisual[index].ListaNumeroUnicoAsignado = ObjetoNumeroUnicoAsignado;

            if (jSonCaptura[index].RevisionNU) {
                if ((ListaDetalleGuardarInspeccionVisual[index].TallerID == "" || ListaDetalleGuardarInspeccionVisual[index].TallerID == "0" ||
                    ListaDetalleGuardarInspeccionVisual[index].ResultadoID == "" || ListaDetalleGuardarInspeccionVisual[index].ResultadoID == "0" ||
                    ((ListaDetalleGuardarInspeccionVisual[index].DefectosID == "" || ListaDetalleGuardarInspeccionVisual[index].DefectosID == "0" || ListaDetalleGuardarInspeccionVisual[index].DefectosID == null) && ListaDetalleGuardarInspeccionVisual[index].ResultadoID == 2) ||
                    ListaDetalleGuardarInspeccionVisual[index].ObreroID == "" || ListaDetalleGuardarInspeccionVisual[index].ObreroID == "0" ||
                    ListaDetalleGuardarInspeccionVisual[index].FechaInspeccion == " " || ListaDetalleGuardarInspeccionVisual[index].FechaInspeccion == "0" ||
                    ListaDetalleGuardarInspeccionVisual[index].ListaNumeroUnicoAsignado[0].NumeroUnico1ID == "" || ListaDetalleGuardarInspeccionVisual[index].ListaNumeroUnicoAsignado[0].NumeroUnico1ID == "0" || ListaDetalleGuardarInspeccionVisual[index].ListaNumeroUnicoAsignado[0].NumeroUnico1ID == null ||
                    ListaDetalleGuardarInspeccionVisual[index].ListaNumeroUnicoAsignado[0].NumeroUnico2ID == "" || ListaDetalleGuardarInspeccionVisual[index].ListaNumeroUnicoAsignado[0].NumeroUnico2ID == "0" || ListaDetalleGuardarInspeccionVisual[index].ListaNumeroUnicoAsignado[0].NumeroUnico2ID == null
                ) && (ListaDetalleGuardarInspeccionVisual[index].Accion != 4 && ListaDetalleGuardarInspeccionVisual[index].Accion != 3)) {

                    if (ListaDetalleGuardarInspeccionVisual[index].Accion == 2) ListaDetalleGuardarInspeccionVisual[index].Accion = 4;

                    ListaDetalleGuardarInspeccionVisual[index].Estatus = 0;
                    $("#grid").data("kendoGrid").dataSource._data[index].RowOk = false;
                    existRowEmpty = true;
                }
                else if (ListaDetalleGuardarInspeccionVisual[index].Accion == 4) {
                    if (!(ListaDetalleGuardarInspeccionVisual[index].TallerID == "" || ListaDetalleGuardarInspeccionVisual[index].TallerID == "0" ||
                        ListaDetalleGuardarInspeccionVisual[index].ResultadoID == "" || ListaDetalleGuardarInspeccionVisual[index].ResultadoID == "0" ||
                        ((ListaDetalleGuardarInspeccionVisual[index].DefectosID == "" || ListaDetalleGuardarInspeccionVisual[index].DefectosID == "0" || ListaDetalleGuardarInspeccionVisual[index].DefectosID == null) && ListaDetalleGuardarInspeccionVisual[index].ResultadoID == 2) ||
                        ListaDetalleGuardarInspeccionVisual[index].ObreroID == "" || ListaDetalleGuardarInspeccionVisual[index].ObreroID == "0" ||
                        ListaDetalleGuardarInspeccionVisual[index].FechaInspeccion == " " || ListaDetalleGuardarInspeccionVisual[index].FechaInspeccion == "0" ||
                        ListaDetalleGuardarInspeccionVisual[index].ListaNumeroUnicoAsignado[0].NumeroUnico1ID == "" || ListaDetalleGuardarInspeccionVisual[index].ListaNumeroUnicoAsignado[0].NumeroUnico1ID == "0" || ListaDetalleGuardarInspeccionVisual[index].ListaNumeroUnicoAsignado[0].NumeroUnico1ID == null ||
                        ListaDetalleGuardarInspeccionVisual[index].ListaNumeroUnicoAsignado[0].NumeroUnico2ID == "" || ListaDetalleGuardarInspeccionVisual[index].ListaNumeroUnicoAsignado[0].NumeroUnico2ID == "0" || ListaDetalleGuardarInspeccionVisual[index].ListaNumeroUnicoAsignado[0].NumeroUnico2ID == null)) {

                        ListaDetalleGuardarInspeccionVisual[index].Estatus = 0;
                        $("#grid").data("kendoGrid").dataSource._data[index].RowOk = false;
                        existRowEmpty = true;
                    }
                }
            }
            else {
                if ((ListaDetalleGuardarInspeccionVisual[index].TallerID == "" || ListaDetalleGuardarInspeccionVisual[index].TallerID == "0" ||
                    ListaDetalleGuardarInspeccionVisual[index].ResultadoID == "" || ListaDetalleGuardarInspeccionVisual[index].ResultadoID == "0" ||
                    ((ListaDetalleGuardarInspeccionVisual[index].DefectosID == "" || ListaDetalleGuardarInspeccionVisual[index].DefectosID == "0" || ListaDetalleGuardarInspeccionVisual[index].DefectosID == null) && ListaDetalleGuardarInspeccionVisual[index].ResultadoID == 2) ||
                    ListaDetalleGuardarInspeccionVisual[index].ObreroID == "" || ListaDetalleGuardarInspeccionVisual[index].ObreroID == "0" ||
                    ListaDetalleGuardarInspeccionVisual[index].FechaInspeccion == " " || ListaDetalleGuardarInspeccionVisual[index].FechaInspeccion == "0"
                ) && (ListaDetalleGuardarInspeccionVisual[index].Accion != 4 && ListaDetalleGuardarInspeccionVisual[index].Accion != 3)) {

                    if (ListaDetalleGuardarInspeccionVisual[index].Accion == 2) ListaDetalleGuardarInspeccionVisual[index].Accion = 4;

                    ListaDetalleGuardarInspeccionVisual[index].Estatus = 0;
                    $("#grid").data("kendoGrid").dataSource._data[index].RowOk = false;
                    existRowEmpty = true;
                }
                else if (ListaDetalleGuardarInspeccionVisual[index].Accion == 4) {
                    if (!(ListaDetalleGuardarInspeccionVisual[index].TallerID == "" || ListaDetalleGuardarInspeccionVisual[index].TallerID == "0" ||
                        ListaDetalleGuardarInspeccionVisual[index].ResultadoID == "" || ListaDetalleGuardarInspeccionVisual[index].ResultadoID == "0" ||
                        ((ListaDetalleGuardarInspeccionVisual[index].DefectosID == "" || ListaDetalleGuardarInspeccionVisual[index].DefectosID == "0" || ListaDetalleGuardarInspeccionVisual[index].DefectosID == null) && ListaDetalleGuardarInspeccionVisual[index].ResultadoID == 2) ||
                        ListaDetalleGuardarInspeccionVisual[index].ObreroID == "" || ListaDetalleGuardarInspeccionVisual[index].ObreroID == "0" ||
                        ListaDetalleGuardarInspeccionVisual[index].FechaInspeccion == " " || ListaDetalleGuardarInspeccionVisual[index].FechaInspeccion == "0")) {

                        ListaDetalleGuardarInspeccionVisual[index].Estatus = 0;
                        $("#grid").data("kendoGrid").dataSource._data[index].RowOk = false;
                        existRowEmpty = true;
                    }
                }
            }
        }

        // Finaliza captura Visual

        // Filtro grid por status
        var ListaDetalleGuardarFiltroStatus = [];
        for (var x = 0; x < ListaDetalleGuardarInspeccionVisual.length; x++)
            if (ListaDetalleGuardarInspeccionVisual[x].Estatus != 0) ListaDetalleGuardarFiltroStatus.push(ListaDetalleGuardarInspeccionVisual[x]);

        if (ListaDetalleGuardarFiltroStatus.length <= 0) capturaSinVisual = true;

        if (inspeccionDimensional.length > 0) inspeccionDimensional[0].ListaDetalleGuardarInspeccionVisual = ListaDetalleGuardarFiltroStatus;
        else capturaSinVisual = true;

        if (ResultadoDimensional == undefined) {
            existRowEmpty = true;
            $("#FechaInspeccion").css({ 'background-color': '#ffcccc' });
            $("#inputDefecto").data("kendoComboBox").input.css("background-color", "#ffcccc");
            $("#inputInspector").data("kendoComboBox").input.css("background-color", "#ffcccc");
            $("#ListaJuntas").data("kendoMultiSelect").input.css("background-color", "#ffcccc");

            $("#FechaInspeccionDiv span").css({ 'background-color': '#ffcccc' });
            $("#inputDefectoDiv span").css({ 'background-color': '#ffcccc' });
            $("#inputInspectorDiv span").css({ 'background-color': '#ffcccc' });
            $("#listaJuntasDiv .k-multiselect").css({ 'background-color': '#ffcccc' });

            $("#lblRechazado").css({ 'color': '#EC4F50' });
            $("#lblAprobado").css({ 'color': '#EC4F50' });


        }
        else if (ResultadoDimensional == "Aprobado" && ((Inspector == -1 || Inspector == 0))) {
            existRowEmpty = true;
            $("#FechaInspeccion").css({ 'background-color': '#ffcccc' });
            $("#inputDefecto").data("kendoComboBox").input.css("background-color", "#ffcccc");
            $("#inputInspector").data("kendoComboBox").input.css("background-color", "#ffcccc");
            $("#ListaJuntas").data("kendoMultiSelect").input.css("background-color", "#ffcccc");

            $("#FechaInspeccionDiv span").css({ 'background-color': '#ffcccc' });
            $("#inputDefectoDiv span").css({ 'background-color': '#ffcccc' });
            $("#inputInspectorDiv span").css({ 'background-color': '#ffcccc' });
            $("#listaJuntasDiv .k-multiselect").css({ 'background-color': '#ffcccc' });

            $("#lblRechazado").css({ 'color': '#EC4F50' });
            $("#lblAprobado").css({ 'color': '#EC4F50' });

        }
        else if (ResultadoDimensional == "Rechazado" && (inspeccionDimensional[0].DefectoID == "" || inspeccionDimensional[0].FechaInspeccion == "" || inspeccionDimensional[0].ObreroID == "" || (tipoDefecto == "NoEspecificarJunta" ? false : (inspeccionDimensional[0].ListaJuntas == undefined || JuntasValidas(inspeccionDimensional[0].ListaJuntas) == 0)))) {


            existRowEmpty = true;
            $("#FechaInspeccion").css({ 'background-color': '#ffcccc' });
            $("#inputDefecto").data("kendoComboBox").input.css("background-color", "#ffcccc");
            $("#inputInspector").data("kendoComboBox").input.css("background-color", "#ffcccc");
            $("#ListaJuntas").data("kendoMultiSelect").input.css("background-color", "#ffcccc");

            $("#FechaInspeccionDiv span").css({ 'background-color': '#ffcccc' });
            $("#inputDefectoDiv span").css({ 'background-color': '#ffcccc' });
            $("#inputInspectorDiv span").css({ 'background-color': '#ffcccc' });
            $("#listaJuntasDiv .k-multiselect").css({ 'background-color': '#ffcccc' });

            $("#lblRechazado").css({ 'color': '#EC4F50' });
            $("#lblAprobado").css({ 'color': '#EC4F50' });

            inspeccionDimensional[0].ListaJuntas = [];
            inspeccionDimensional[0].ListaJuntas.push({ Accion: 0, OrdenTrabajoSpoolID: "", DefectoID: "", JuntaID: "" });
            if ($("#inputDefecto").data("kendoComboBox").dataItem(Defecto) != undefined)
                if ($("#inputDefecto").data("kendoComboBox").dataItem(Defecto).TIPO != "NoEspecificarJunta")
                    capturaSinDimensional = true;
        }
        else {
            aplicarColorBlancoCapturaDimensional();
        }

        // Asignación de elementos vacios de prueba
        if (capturaSinDimensional)
            Captura[0].Detalles = [{ Lenguaje: "", InspeccionDimensionalID: 0, OrdenTrabajoSpoolID: 0, FechaInspeccion: "", ResultadoID: 0, ObreroID: 0, DefectoID: 0, ListaDetalleGuardarInspeccionVisual: undefined, ListaJuntas: undefined }];
        else Captura[0].Detalles = inspeccionDimensional;

        if (capturaSinVisual)
            ListaDetalleGuardarFiltroStatus[0] = { Accion: 0, OrdenTrabajoSpoolID: 0, TipoJuntaID: "", EtiquetaJunta: "", EtiquetaMaterial1: "", EtiquetaMaterial2: "", DefectosID: 0, ObreroID: 0, FechaInspeccion: "", JuntaTrabajoID: 0, ResultadoID: 0, TallerID: 0, NumeroUnico1ID: 0, NumeroUnico2ID: 0, InspeccionVisualID: 0 };
        else Captura[0].Detalles[0].ListaDetalleGuardarInspeccionVisual = ListaDetalleGuardarFiltroStatus;

        if (capturaSinDimensional && !capturaSinVisual) Captura[0].Detalles[0].ListaDetalleGuardarInspeccionVisual = ListaDetalleGuardarFiltroStatus;

        Captura[0].Detalles[0].ListaJuntas = inspeccionDimensional[0].ListaJuntas;

        $("#grid").data("kendoGrid").dataSource.sync();




        if (existRowEmpty) {
            //windowTemplate = kendo.template($("#windowTemplate").html());
            ventanaConfirmGuardar = $("#ventanaConfirmGuardar").kendoWindow({
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


            ventanaConfirmGuardar.content(_dictionary.MensajeConfirmacionGuardadoGeneral[$("#language").data("kendoDropDownList").value()] +
                "</br><center><button class='btn btn-blue' id='yesButtonConfirGuardar'>" + _dictionary.EntregaPlacasGraficasbotonSi[$("#language").data("kendoDropDownList").value()] + "</button><button class='btn btn-blue' id='noButtonConfirGuardar'> " + _dictionary.EntregaPlacasGraficasbotonNo[$("#language").data("kendoDropDownList").value()] + "</button></center>");

            ventanaConfirmGuardar.open().center();

            $("#yesButtonConfirGuardar").click(function () {
                ejecutaGuardado(Captura, capturaSinDimensional, capturaSinVisual, tipoGuardar);
                ventanaConfirmGuardar.close();
            });

            $("#noButtonConfirGuardar").click(function () {
                ventanaConfirmGuardar.close();
                //opcionHabilitarView(false, "FieldSetView");
            });
        }
        else {

            ejecutaGuardado(Captura, capturaSinDimensional, capturaSinVisual, tipoGuardar);
        }
    }
    else {
        displayNotify("AdverteciaExcepcionGuardado", "", '1');
    }
}


function JuntasValidas(data) {
    var cantidadJuntasValidas = 0;
    for (var i = 0; i < data.length; i++) {
        if (data[i].Accion != 3)
            cantidadJuntasValidas++;
    }
    return cantidadJuntasValidas;
}

function aplicarColorBlancoCapturaDimensional() {
    $("#FechaInspeccion").css({ 'background-color': '#ffffff' });
    $("#inputDefecto").data("kendoComboBox").input.css("background-color", "#ffffff");
    $("#inputInspector").data("kendoComboBox").input.css("background-color", "#ffffff");
    $("#ListaJuntas").data("kendoMultiSelect").input.css("background-color", "#ffffff");

    $("#FechaInspeccionDiv span").css({ 'background-color': '#ffffff' });
    $("#inputDefectoDiv span").css({ 'background-color': '#ffffff' });
    $("#inputInspectorDiv span").css({ 'background-color': '#ffffff' });
    $("#listaJuntasDiv .k-multiselect").css({ 'background-color': '#ffffff' });
    $("#lblRechazado").css({ 'color': '#000' });
    $("#lblAprobado").css({ 'color': '#000' });
}

function ejecutaGuardado(Captura, guardadoSinInspeccionDimensional, guardadoSinInspeccionVisual, tipoGuardar) {
    loadingStart();
    aplicarColorBlancoCapturaDimensional();
    if (guardadoSinInspeccionDimensional && guardadoSinInspeccionVisual) {
        displayNotify("DimensionalVisualMensajeNoHayDatosPorGuardar", "", '1');
        loadingStop();
    }
    else $InspeccionVisualDimensional.VisualDimensional.create(Captura[0], { token: Cookies.get("token"), lenguaje: $("#language").val() }).done(function (data) {
        if (data.ReturnMessage.length > 0 && data.ReturnMessage[0] == "Ok") {
            var datos = $("#grid").data('kendoGrid').dataSource._data
            $("#grid").data('kendoGrid').dataSource.data([]);
            if (tipoGuardar == 1) {
                opcionHabilitarView(false, "FieldSetView");
                limpiar();
                LimpiarPlanchado();
                habilitaSpool();
                editado = false;
            }
            else {
                opcionHabilitarView(true, "FieldSetSetView");
                ajaxobtenerDetalleDimensional(ordentrabajoSpoolID.Valor);
                ajaxObtenerJSonGridDespuesGuardar(datos);
            }
            displayNotify("CapturaMensajeGuardadoExitoso", "", '0');
        }
        else if (data.ReturnMessage.length > 0 && data.ReturnMessage[0] != "Ok") {
            displayNotify("MensajeGuardadoErroneo", "", '2');
            opcionHabilitarView(false, "FieldSetView");
            //deshabilitaSpool();
            loadingStop();
        }

    });
}

function ObtenerDato(fecha, tipoDatoObtener) {
    var cultura = $("#language").val();

    switch (tipoDatoObtener) {
        case 1://anho
            return fecha.split('/')[2]
            break;
        case 2://mes
            if (cultura = 'es-MX')
                return fecha.split('/')[1]
            else
                return fecha.split('/')[0]
            break;
        case 3://dia
            if (cultura = 'es-MX')
                return fecha.split('/')[0]
            else
                return fecha.split('/')[1]
            break;
    }
}

function validaTaller(ListaDetalleGuardar) {
    for (var i = 0; i < ListaDetalleGuardar.length; i++) {
        if (ListaDetalleGuardar[i] == undefined || ListaDetalleGuardar[i].TallerID == null || ListaDetalleGuardar[i].TallerID == 0) {
            return false;
        }
    }
    return true;
}

function validaDefectos(ListaDetalleGuardar) {
    for (var i = 0; i < ListaDetalleGuardar.length; i++) {
        if (ListaDetalleGuardar[i].DefectosID == null && ListaDetalleGuardar[i].ResultadoID == 2) {
            return false;
        }
    }
    return true;
}

function validaResultado(ListaDetalleGuardar) {
    for (var i = 0; i < ListaDetalleGuardar.length; i++) {
        if (ListaDetalleGuardar[i].ResultadoID == null) {
            return false;
        }
    }
    return true;
}

function validaInspector(ListaDetalleGuardar) {
    for (var i = 0; i < ListaDetalleGuardar.length; i++) {
        if (ListaDetalleGuardar[i].ObreroID == "") {
            return false;
        }
    }
    return true;
}

function validaNumeroUnico1(ListaDetalleGuardar) {
    for (var i = 0; i < ListaDetalleGuardar.length; i++) {
        if (ListaDetalleGuardar[i].NumeroUnico1ID == 0) {
            return false;
        }
    }
    return true;
}

function validaNumeroUnico2(ListaDetalleGuardar) {
    for (var i = 0; i < ListaDetalleGuardar.length; i++) {
        if (ListaDetalleGuardar[i].NumeroUnico2ID == 0) {
            return false;
        }
    }
    return true;
}