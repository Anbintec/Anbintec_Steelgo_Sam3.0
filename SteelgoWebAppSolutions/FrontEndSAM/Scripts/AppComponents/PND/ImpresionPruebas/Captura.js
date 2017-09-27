var ReporteIDConsecutivo;
var editado = false;
function changeLanguageCall() {
    CargarGrid();
    $('#grid').data('kendoGrid').dataSource.read();
    AjaxCargarCamposPredeterminados();
    document.title = _dictionary.lblImpresionPruebas[$("#language").data("kendoDropDownList").value()];
    //AjaxCargarCamposPredeterminados();
};

IniciarImpresionPruebas();
function IniciarImpresionPruebas() {
    SuscribirEventos();
    setTimeout(function () { AjaxObtenerProyectos(); }, 500);
}

function JuntasSeleccionadasConIDUnico(arregloJuntas) {
    //arregloJuntas[index].Seleccionado
    //arregloJuntas[index].ReporteID
    var cantidadJuntasSeleccionadasDiferentes = 0, cantidadJuntasSeleccionadas, IdReporteMaximo = 0;

    for (index = 0; index < arregloJuntas.length; index++) {

        if (arregloJuntas[index].Seleccionado) {

            for (var j = 0; j < arregloJuntas.length; j++) {
                if (arregloJuntas[index].ReporteID != null && arregloJuntas[j].Seleccionado && arregloJuntas[index].ReporteID != arregloJuntas[j].ReporteID && arregloJuntas[j].ReporteID != null) {
                    cantidadJuntasSeleccionadasDiferentes++;
                }
            }
        }
    }

    if (cantidadJuntasSeleccionadasDiferentes == 0) {

        return true;

    }
    return false;
}



function AsignarIDUnicoXJuntaSeleccionada(arregloJuntas) {
    var IDReporteSeleccionado;
    var cantidadJuntasSeleccionadasVacias = 0, cantidadJuntasSeleccionadasAsignadas = 0;

    for (a = 0; a < arregloJuntas.length; a++) {
        if (arregloJuntas[a].Seleccionado && (arregloJuntas[a].ReporteID == null || arregloJuntas[a].ReporteID == undefined)) {
            cantidadJuntasSeleccionadasVacias++;
        }
    }

    for (a = 0; a < arregloJuntas.length; a++) {
        if (arregloJuntas[a].Seleccionado && (arregloJuntas[a].ReporteID != null || arregloJuntas[a].ReporteID != undefined)) {
            cantidadJuntasSeleccionadasAsignadas++;
        }
    }

    if (cantidadJuntasSeleccionadasAsignadas == 0) {
        //caso 1 solo es un registro y es nuevo

        if (cantidadJuntasSeleccionadasVacias == 1) {
            for (a = 0; a < arregloJuntas.length; a++) {
                if (arregloJuntas[a].Seleccionado && (arregloJuntas[a].ReporteID == null || arregloJuntas[a].ReporteID == undefined)) {
                    arregloJuntas[a].Status = 'N'
                }
            }
        } else if (cantidadJuntasSeleccionadasVacias > 1) {
            for (a = 0; a < arregloJuntas.length; a++) {
                if (arregloJuntas[a].Seleccionado && (arregloJuntas[a].ReporteID == null || arregloJuntas[a].ReporteID == undefined)) {
                    arregloJuntas[a].Status = 'NR'
                }
            }

        }
    }
    else {
        for (a = 0; a < arregloJuntas.length; a++) {
            if (arregloJuntas[a].Seleccionado && (arregloJuntas[a].ReporteID != null || arregloJuntas[a].ReporteID != undefined)) {
                for (var i = 0; i < arregloJuntas.length; i++) {
                    if (arregloJuntas[i].Seleccionado && (arregloJuntas[i].ReporteID == null || arregloJuntas[i].ReporteID == undefined)) {
                        arregloJuntas[i].ReporteID = arregloJuntas[a].ReporteID;
                    }
                }
            }
        }
    }

    return true;
}

function CargarGrid() {
    $("#grid").kendoGrid({
        autoBind: true,
        dataSource: {
            schema: {
                model: {
                    fields: {
                        Requisicion: { type: "int", editable: false },
                        Spool: { type: "int", editable: false },
                        Junta: { type: "string", editable: false },
                        Clasificacion: { type: "string", editable: false },
                        Diametro: { type: "string", editable: false },
                        Espesor: { type: "string", editable: false },
                        Cedula: { type: "string", editable: false },
                        TipoJunta: { type: "string", editable: false },
                        Reporte: { type: "string", editable: false },
                        Version: { type: "string", editable: false },
                        Seleccionado: { type: "boolean", editable: false }
                    }
                }
            },
            filter: {
                logic: "or",
                filters: [
                  { field: "Accion", operator: "eq", value: 1 },
                  { field: "Accion", operator: "eq", value: 2 },
                  { field: "Accion", operator: "eq", value: 4 }

                ]
            },
            pageSize: 10,
            serverPaging: false,
            serverFiltering: false,
            serverSorting: false
        },
        filterable: getGridFilterableMaftec(),
        navigatable: true,
        editable: true,
        autoHeight: true,
        sortable: true,
        scrollable: true,
        pageable: {
            refresh: false,
            pageSizes: [10, 25, 50, 100],
            info: false,
            input: false,
            numeric: true,
        },
        columns: [
            { field: "Requisicion", title: _dictionary.columnRequisicion[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "120px " },
            { field: "Spool", title: _dictionary.columnNumeroControl[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "100px" },
            { field: "Junta", title: _dictionary.columnJunta[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "90px" },
            { field: "Clasificacion", title: _dictionary.columnClasificacion[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "90px" },
            { field: "Diametro", title: _dictionary.columnDiametro[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "100px", attributes: { style: "text-align:right;" } },
            { field: "Espesor", title: _dictionary.columnEspesor[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "100px", attributes: { style: "text-align:right;" } },
            { field: "Cedula", title: _dictionary.columnCedula[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "100px" },
            { field: "TipoJunta", title: _dictionary.columnFirmadoTipoJunta[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "100px" },
            { field: "Reporte", title: _dictionary.columnReporte[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "130px" },
            { field: "Version", title: _dictionary.columnVersion[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "100px", attributes: { style: "text-align:right;" } },
            {
                field: "Seleccionado", title: _dictionary.columnSeleccionado[$("#language").data("kendoDropDownList").value()],
                filterable: {
                    multi: true,
                    messages: {
                        isTrue: _dictionary.lblVerdadero[$("#language").data("kendoDropDownList").value()],
                        isFalse: _dictionary.lblFalso[$("#language").data("kendoDropDownList").value()],
                        style: "max-width:100px;"
                    },
                    dataSource: [{ Seleccionado: true }, { Seleccionado: false }]
                }
                , template: '<input type="checkbox" #= Seleccionado ? "checked=checked" : "" # class="chkbx"  ></input>  ', width: "90px", attributes: { style: "text-align:center;" }
            },
            { command: { text: _dictionary.botonCancelar[$("#language").data("kendoDropDownList").value()] }, title: _dictionary.columnELM[$("#language").data("kendoDropDownList").value()], width: "50px" },
        ]
    });
    CustomisaGrid($("#grid"));

    $("#grid .k-grid-content").on("change", "input.chkbx", function (e) {
        var grid = $("#grid").data("kendoGrid"),
        dataItem = grid.dataItem($(e.target).closest("tr"));

        var ds = grid.dataSource._data;
        var correcto = true;
        for (var i = 0 ; i < ds.length; i++) {
            if (dataItem.Reporte != "")
                if (ds[i].Reporte != "" && ds[i].Reporte != dataItem.Reporte && ds[i].Seleccionado)
                    correcto = false;

        }
        if (correcto) {
            if ($(this)[0].checked) {
                dataItem.Seleccionado = true;
            }
            else {
                dataItem.Seleccionado = false;
            }
        }
        else {
            displayNotify("mensajeImpresionPruebasElementoNoSeleccionable", "", "1");
            dataItem.Seleccionado = false;
            $(this)[0].checked = false;

        }

        //$("#grid").data("kendoGrid").dataSource.sync();


    });
};


function seleccionarTodo(valorPlanchado) {
    var ds = $("#grid").data("kendoGrid").dataSource._data;

    for (var i = 0; i < ds.length; i++) {
        ds[i].Seleccionado = valorPlanchado;
    }
    $("#grid").data("kendoGrid").dataSource.sync();
}