var procesoPinturaSeleccionadoAnterior = "";
var editado = false;
var ventanaConfirmEdicionCambioProcesoPintura;
var proyectoActualSeleccionado;
var esNormal;
var ComponentesDinamicos;
var ReductorDinamico;
var ComponentesDinamicosJSON = [];
var ReductoresDinamicosJSON = [];
var ventanaConfirmEdicionCaptura;
var elementoEjecutoChange;
var endRangeDate;
var LineaCaptura = { proyectoIDSeleccionado: "", zonaIDSeleccionado: "", cuadranteIDSeleccionado: "", sistemaPinturaIDSeleccionado: "", ColorIDSeleccionado: "" }
var nextCell = null;
function BloquearElementosDinamicos(bloqueado) {
    for (var i = 0; i < ComponentesDinamicosJSON.length; i++) {
        $('#' + ComponentesDinamicosJSON[i].NombreColumna).data("kendoComboBox").enable(bloqueado);
    }

    for (var i = 0; i < ReductoresDinamicosJSON.length; i++) {
        $('#' + ReductoresDinamicosJSON[i].NombreColumna).data("kendoComboBox").enable(bloqueado);
    }
}

function TryParseInt(str, defaultValue) {
    var retValue = defaultValue;
    if (str !== null) {
        if (str.length > 0) {
            if (!isNaN(str)) {
                retValue = parseInt(str);
            }
        }
    }
    return retValue;
}

function plancharTodo() {
    if ($("#inputPintor").data("kendoMultiSelect")._dataItems.length > 0) {
        PlancharTrabajadores($("#grid").data("kendoGrid").dataSource._data);
    }
    if ($("#inputFechaProceso").val() != "") {
        PlanchaFechaProceso();
    }

    PlanchaElementosDinamicos();
}

function PlanchaElementosDinamicos() {
    var dataSource = $("#grid").data("kendoGrid").dataSource;
    var filters = dataSource.filter();
    var allData = dataSource.data();
    var query = new kendo.data.Query(allData);
    var data = query.filter(filters).data;


    if ($('input:radio[name=LLena]:checked').val() === "Todos") {
        for (var j = 0; j < ComponentesDinamicosJSON.length; j++) {
            for (var i = 0; i < data.length; i++) {
                if ($("#" + ComponentesDinamicosJSON[j].NombreColumna).val() != undefined && $("#" + ComponentesDinamicosJSON[j].NombreColumna).val() != "")
                    data[i][ComponentesDinamicosJSON[j].NombreColumna] = $("#" + ComponentesDinamicosJSON[j].NombreColumna).val();
            }
        }

        for (var j = 0; j < ReductoresDinamicosJSON.length; j++) {
            for (var i = 0; i < data.length; i++) {
                if ($("#" + ReductoresDinamicosJSON[j].NombreColumna).val() != undefined && $("#" + ReductoresDinamicosJSON[j].NombreColumna).val() != "")
                    data[i][ReductoresDinamicosJSON[j].NombreColumna] = $("#" + ReductoresDinamicosJSON[j].NombreColumna).val();
            }
        }
    }
    else {
        for (var j = 0; j < ComponentesDinamicosJSON.length; j++) {
            for (var i = 0; i < data.length; i++) {
                if ($("#" + ComponentesDinamicosJSON[j].NombreColumna).val() != undefined && $("#" + ComponentesDinamicosJSON[j].NombreColumna).val() != "") {
                    if (data[i][ComponentesDinamicosJSON[j].NombreColumna] == null || data[i][ComponentesDinamicosJSON[j].NombreColumna].trim() == "" || data[i][ComponentesDinamicosJSON[j].NombreColumna] == undefined)
                        data[i][ComponentesDinamicosJSON[j].NombreColumna] = $("#" + ComponentesDinamicosJSON[j].NombreColumna).val();
                }
            }
        }

        for (var j = 0; j < ReductoresDinamicosJSON.length; j++) {
            for (var i = 0; i < data.length; i++) {
                if ($("#" + ReductoresDinamicosJSON[j].NombreColumna).val() != undefined && $("#" + ReductoresDinamicosJSON[j].NombreColumna).val() != "") {
                    if (data[i][ReductoresDinamicosJSON[j].NombreColumna] == null || data[i][ReductoresDinamicosJSON[j].NombreColumna].trim() == "" || data[i][ReductoresDinamicosJSON[j].NombreColumna] == undefined)
                        data[i][ReductoresDinamicosJSON[j].NombreColumna] = $("#" + ReductoresDinamicosJSON[j].NombreColumna).val();
                }
            }
        }
    }

    $("#grid").data("kendoGrid").refresh();
}

function PlanchaFechaProceso() {
    var dataSource = $("#grid").data("kendoGrid").dataSource;
    var filters = dataSource.filter();
    var allData = dataSource.data();
    var query = new kendo.data.Query(allData);
    var data = query.filter(filters).data;

    if ($('input:radio[name=LLena]:checked').val() === "Todos") {
        for (var i = 0; i < data.length; i++) {
            data[i].FechaProceso = String(endRangeDate.val()).trim();
        }
    }
    else {
        for (var i = 0; i < data.length; i++) {
            if (data[i].FechaProceso === "" || data[i].FechaProceso === null || data[i].FechaProceso === undefined) {
                data[i].FechaProceso = String(endRangeDate.val()).trim();
            }
        }
    }
    $("#grid").data("kendoGrid").refresh();
}

function PlancharTrabajadores(arregloCaptura) {

    var dataShotBlast = $("#inputPintor").data("kendoMultiSelect")._dataItems;
    var dataSource = $("#grid").data("kendoGrid").dataSource;
    var filters = dataSource.filter();
    var allData = dataSource.data();
    var query = new kendo.data.Query(allData);
    var data = query.filter(filters).data;

    //obtenermos los obreros a planchar
    if ($('input:radio[name=LLena]:checked').val() === _dictionary.CapturaTodos[$("#language").data("kendoDropDownList").value()]) {
        for (var i = 0; i < data.length; i++) {
            data[i].ListaObreros = $("#inputPintor").data("kendoMultiSelect").dataSource._data;
            data[i].ListaObrerosSeleccionados = dataShotBlast;
            data[i].plantillaObrero = _dictionary.CapturaAvancePintoresShotblastExistentes[$("#language").data("kendoDropDownList").value()] + arregloCaptura[i].ListaObrerosSeleccionados.length;
        }

    }
    else {
        for (var i = 0; i < data.length; i++) {
            if (data[i].ListaObrerosSeleccionados.length == 0) {
                data[i].ListaObreros = $("#inputPintor").data("kendoMultiSelect").dataSource._data;
                data[i].ListaObrerosSeleccionados = dataShotBlast;
                data[i].plantillaObrero = _dictionary.CapturaAvancePintoresShotblastExistentes[$("#language").data("kendoDropDownList").value()] + arregloCaptura[i].ListaObrerosSeleccionados.length;
            }
        }

    }
    $("#grid").data("kendoGrid").dataSource.sync();
}

function existeSpool(spool, array) {
    for (var index = 0; index < array._data.length; index++) {
        if (array._data[index].Spool == spool) {
            return true;
        }
    }
    return false;
}


function Limpiar() {
    $("#InputCuadrante").val("");
    $("#InputColor").val("");
    $("#InputFechaCapturaAvanceIntAcabado").val("");
    $("#InputPintor").val("");
    $("#InputSistemaPintura").val("");
    $("#InputPinturaComponenteComposicion").val("");
    $("#grid").data('kendoGrid').dataSource.data([]);


    for (var i = 0; i < ComponentesDinamicosJSON.length; i++) {
        $('#' + ComponentesDinamicosJSON[i].NombreColumna).data("kendoComboBox").value("");
    }

    for (var i = 0; i < ReductoresDinamicosJSON.length; i++) {
        $('#' + ReductoresDinamicosJSON[i].NombreColumna).data("kendoComboBox").value("");
    }

}

function tieneClase(item) {

    var tieneClass = $(item).hasClass("k-state-border-up") || $(item).hasClass("k-state-border-down");
    return tieneClass;
}

function CrearControlesDinamicos() {
    if ($('#Guardar').text() == _dictionary.MensajeGuardar[$("#language").data("kendoDropDownList").value()]) {
        for (var i = 0; i < ComponentesDinamicosJSON.length; i++) {
            $("#divAgregarComponentesReductoresDinamicos").append('<div class="col-xs-3" style="display: inline-block;"><label>' + ComponentesDinamicosJSON[i].NombreColumna + '</label><input id="' + ComponentesDinamicosJSON[i].NombreColumna + '" /></div>');

            $('#' + ComponentesDinamicosJSON[i].NombreColumna).kendoComboBox({
                dataTextField: "NombreLote",
                dataValueField: "NombreLote",
                dataSource: ComponentesDinamicosJSON[i].ListadoLotes,
                suggest: true,
                delay: 10,
                filter: "contains",
                index: 3
            });
        }

        for (var i = 0; i < ReductoresDinamicosJSON.length; i++) {
            $("#divAgregarComponentesReductoresDinamicos").append('<div class="col-xs-3" style="display: inline-block;"><label>' + ReductoresDinamicosJSON[i].NombreColumna + '</label><input id="' + ReductoresDinamicosJSON[i].NombreColumna + '" /></div>');

            $('#' + ReductoresDinamicosJSON[i].NombreColumna).kendoComboBox({
                dataTextField: "NombreLote",
                dataValueField: "NombreLote",
                dataSource: ReductoresDinamicosJSON[i].ListadoLotes,
                suggest: true,
                delay: 10,
                filter: "contains",
                index: 3
            });
        }
    }

}

function changeLanguageCall() {
    SuscribirEventos();
    AjaxCargarCamposPredeterminados();

}

function ValidarFechaPrimario(valor) {
    var fecha = kendo.toString(valor, String(_dictionary.FormatoFecha[$("#language").data("kendoDropDownList").value()].replace('{', '').replace('}', '').replace("0:", "")));
    if (fecha == null) {
        $("#inputFechaProceso").data("kendoDatePicker").value('');
    }
}

function CambiarProcesoPintura() {
    if (procesoPinturaSeleccionadoAnterior == "")
        procesoPinturaSeleccionadoAnterior = $('input:radio[name=ProcesoPintura]:checked').val();

    if (!editado) {
        LimpiarDespuesCambioProcesoPintura();
        procesoPinturaSeleccionadoAnterior = $('input:radio[name=ProcesoPintura]:checked').val();
        AjaxCargarProyecto();
        if (procesoPinturaSeleccionadoAnterior == 4)
            $('#divColor').show();
        else
            $('#divColor').hide();

        if ($('input:radio[name=ProcesoPintura]:checked').val() != 4) {
            $("#lblTrabajador").text(_dictionary.columnShotblastero[$("#language").data("kendoDropDownList").value()])
        }
        else {

            $("#lblTrabajador").text(_dictionary.columnPintor[$("#language").data("kendoDropDownList").value()])
        }
    }
    else {
        ventanaConfirmEdicionCambioProcesoPintura.open().center();
    }
}

function LimpiarDespuesCambioProcesoPintura() {
    $("#inputProyecto").data("kendoComboBox").dataSource.data([]);
    $("#inputZona").data("kendoComboBox").dataSource.data([]);
    $("#inputCuadrante").data("kendoComboBox").dataSource.data([]);
    $("#inputSistemaPintura").data("kendoComboBox").dataSource.data([]);
    $("#InputID").data("kendoComboBox").dataSource.data([]);
    $("#inputColor").data("kendoComboBox").dataSource.data([]);

    $("#inputProyecto").data("kendoComboBox").value("");
    $("#inputZona").data("kendoComboBox").value("");
    $("#inputCuadrante").data("kendoComboBox").value("");
    $("#inputSistemaPintura").data("kendoComboBox").value("");
    $("#InputID").data("kendoComboBox").value("");
    $("#inputColor").data("kendoComboBox").value("");



    $("#grid").empty();
    CrearGrid();
    CustomisaGrid($("#grid"));
    document.getElementById('divAgregarComponentesReductoresDinamicos').innerHTML = '';
    $("#inputPintor").data("kendoMultiSelect").value("");
}

function BuscarDetalle() {
    if (!editado) {
        LimpiarDespuesCambioCaptura();
        var dataItemCuadrante = $("#inputCuadrante").data("kendoComboBox").dataItem($("#inputCuadrante").data("kendoComboBox").select());
        var dataItemSP = $("#inputSistemaPintura").data("kendoComboBox").dataItem($("#inputSistemaPintura").data("kendoComboBox").select());
        var dataItemColor = $("#inputColor").data("kendoComboBox").dataItem($("#inputColor").data("kendoComboBox").select());
        if (LineaCaptura.proyectoIDSeleccionado != "" && LineaCaptura.proyectoIDSeleccionado != undefined && LineaCaptura.proyectoIDSeleccionado != null && LineaCaptura.proyectoIDSeleccionado > 0) {
            if ($("#inputZona").data("kendoComboBox").select() > 0) {
                if ($("#inputCuadrante").data("kendoComboBox").select() > 0) {
                    if ($("#inputSistemaPintura").data("kendoComboBox").select() > 0) {
                        if (!($('input:radio[name=ProcesoPintura]:checked').val() == "4" && $("#inputColor").data("kendoComboBox").select() <= 0)) {
                            AjaxCargarLayoutGrid(dataItemCuadrante.CuadranteID, dataItemSP.SistemaPinturaProyectoID, $('input:radio[name=ProcesoPintura]:checked').val() == "4" ? dataItemColor.SistemaPinturaColorID : 0, $("#language").val(), $('input:radio[name=ProcesoPintura]:checked').val(), $('input:radio[name=Muestra]:checked').val());
                        }
                        else {
                            displayNotify("CapturaAvanceCuadranteNoColor", "", '1');
                        }
                    }
                    else {
                        displayNotify("CapturaAvanceCuadranteNoSistemaPintura", "", '1');
                    }
                }
                else {
                    displayNotify("CapturaAvanceCuadranteNoCuadrante", "", '1');
                }
            }
            else {
                displayNotify("CapturaAvanceCuadranteNoZona", "", '1');
            }
        }
        else {
            displayNotify("CapturaAvanceCuadranteNoProyecto", "", '1');
        }
    }
    else {
        ventanaConfirmEdicionCaptura.open().center();
    }
}

function LimpiarDespuesCambioCaptura() {
    $("#InputOrdenTrabajo").val("");
    $("#InputID").data("kendoComboBox").value("");
    $("#grid").empty();
    CrearGrid();
    CustomisaGrid($("#grid"));
    document.getElementById('divAgregarComponentesReductoresDinamicos').innerHTML = '';
    $("#inputPintor").data("kendoMultiSelect").value("");
}

function CrearGrid() {
    kendo.ui.Grid.fn.editCell = (function (editCell) {
        return function (cell) {
            cell = $(cell);

            var that = this,
                column = that.columns[that.cellIndex(cell)],
                model = that._modelForContainer(cell),
                event = {
                    container: cell,
                    model: model,
                    preventDefault: function () {
                        this.isDefaultPrevented = true;
                    }
                };

            if (model && typeof this.options.beforeEdit === "function") {
                this.options.beforeEdit.call(this, event);
                if (event.isDefaultPrevented) return;
            }

            editCell.call(this, cell);
        };
    })(kendo.ui.Grid.fn.editCell);

    $("#grid").kendoGrid({
        edit: function (e) {
            var inputName = e.container.find('input');
            inputName.select();
            if ($('#Guardar').text() == _dictionary.lblGuardar[$("#language").data("kendoDropDownList").value()]) {
                //if ((e.model.ListaObrerosSeleccionados == undefined || e.model.ListaObrerosSeleccionados == "" || e.model.ListaObrerosSeleccionados == null) && (e.model.ListaObrerosGuargados == undefined || e.model.ListaObrerosGuargados == null || e.model.ListaObrerosGuargados == "")) {
                //    ajaxObtenerListadoObrerosGuardados(e.model, e.model.SpoolID, $('input:radio[name=ProcesoPintura]:checked').val())
                //}
            }
            else {
                this.closeCell();
            }
            nextCell = {
                cellIndex: e.container[0].cellIndex,
                uid: e.model.uid,
            };
        },
        dataBound: function (e) {

            var grid = $("#grid").data("kendoGrid");
            var gridData = grid.dataSource.view();

            if (gridData.length > 0) {
                for (var i = 0; i < gridData.length; i++) {
                    var currentUid = gridData[i].uid;
                    var currenRow = grid.table.find("tr[data-uid='" + currentUid + "']");
                    // ajaxObtenerListadoObrerosGuardados(gridData[i], gridData[i].SpoolID, $('input:radio[name=ProcesoPintura]:checked').val())
                    if (gridData[i].RowOk == false) {
                        grid.table.find("tr[data-uid='" + currentUid + "']").removeClass("k-alt");
                        grid.table.find("tr[data-uid='" + currentUid + "']").addClass("kRowError");
                    }
                    else if (gridData[i].RowOk) {
                        if (i % 2 == 0)
                            grid.table.find("tr[data-uid='" + currentUid + "']").removeClass("k-alt");
                        grid.table.find("tr[data-uid='" + currentUid + "']").removeClass("kRowError");
                    }
                }
            }



            if (esNormal) {
                $(".k-grid-content td").css("white-space", "normal");
            }
            else {
                $(".k-grid-content td").css("white-space", "nowrap");
            }

            if (nextCell != null) {
                var grid = e.sender;
                console.log("set focus to next cell", nextCell);
                var nextCellIndex = nextCell.cellIndex;
                var row = grid.items().filter("tr[data-uid=" + nextCell.uid + "]");
                var cell = row.find("td:eq(" + nextCellIndex + ")");
                grid.current(cell);
            }
        },
        beforeEdit: function (e) {
            var columnIndex = this.cellIndex(e.container);
            var fieldName = this.thead.find("th").eq(columnIndex).data("field");
            //if (fieldName == "ListaObrerosSeleccionados")
            //e.preventDefault();
            //   setTimeout(
            //function () {
            //    if ($('#Guardar').text() == _dictionary.lblGuardar[$("#language").data("kendoDropDownList").value()]) {
            //        if ((e.model.ListaObrerosSeleccionados == undefined || e.model.ListaObrerosSeleccionados == "" || e.model.ListaObrerosSeleccionados == null) && (e.model.ListaObrerosGuargados == undefined || e.model.ListaObrerosGuargados == null || e.model.ListaObrerosGuargados == "")) {
            //            ajaxObtenerListadoObrerosGuardados(e.model, e.model.SpoolID, $('input:radio[name=ProcesoPintura]:checked').val())
            //        }
            //    }
            //}, 1000);


        },
        dataBinding: function (e) {
            console.log("Evento dataBinding");
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
            if (cultura == 'es-MX')
                return fecha.split('/')[1] - 1
            else
                return fecha.split('/')[0] - 1
            break;
        case 3://dia
            if (cultura == 'es-MX')
                return fecha.split('/')[0]
            else
                return fecha.split('/')[1]
            break;
    }
}

function limpiarFila(e) {
    e.preventDefault();
    if ($('#btnGuardar').text() == _dictionary.DetalleAvisoLlegada0017[$("#language").data("kendoDropDownList").value()]) {
        var itemToClean;
        itemToClean = this.dataItem($(e.currentTarget).closest("tr"));

        if (itemToClean.Accion == 2)
            itemToClean.Accion = 4;

        itemToClean.FechaProceso = "";
        itemToClean.ListaObrerosSeleccionados = [];
        itemToClean.LoteID = "";
        for (var j = 0; j < ComponentesDinamicosJSON.length; j++) {
            itemToClean[ComponentesDinamicosJSON[j].NombreColumna] = "";
        }

        for (var j = 0; j < ReductoresDinamicosJSON.length; j++) {
            itemToClean[ReductoresDinamicosJSON[j].NombreColumna] = "";
        }

        itemToClean.plantillaObrero = _dictionary.CapturaAvancePintoresShotblastExistentes[$("#language").data("kendoDropDownList").value()] + "0";
        $("#grid").data("kendoGrid").refresh();
    }
}