IniciarEdicionRequisicion();
function IniciarEdicionRequisicion() {
    //SuscribirEventos();
    setTimeout(function () { SuscribirEventos() }, 100);
}

function changeLanguageCall() {
    var paramReq = getParameterByName('requisicion');
    SiguienteProceso(paramReq);
    if (paramReq != null) {
        AjaxObtenerElementoRequisicion(paramReq);
    } else {
        AjaxCargaListaProyecto();
    }
    AjaxCargarCamposPredeterminados();
    cargarGrid();
    document.title = _dictionary.ServiciosTecnicosEditarRequisicionBreadcrumb[$("#language").data("kendoDropDownList").value()];

}

function AgregarJuntaNueva() {
    if ($("#inputTipoPrueba").data("kendoComboBox").dataItem($("#inputTipoPrueba").data("kendoComboBox").select()).TipoPruebaPorSpool == 1) {
        if (!ExisteSpool()) {
            AjaxObtenerSpool();
        }
        else
            displayNotify("GenerarRequisicionMensajeExisteSpool", "", '1');
    }
    else {
        if (!ExisteJunta()) {
            AjaxObtenerJunta();
        }
        else
            displayNotify("GenerarRequisicionMensajeExisteJunta", "", '1');
    }
}

function ExisteJunta() {
    var jsonGrid = $("#grid").data("kendoGrid").dataSource._data;

    for (var i = 0; i < jsonGrid.length; i++) {
        if (jsonGrid[i].OrdenTrabajoSpoolID == $("#InputID").data("kendoComboBox").value() && jsonGrid[i].JuntaSpoolID == $("#Junta").data("kendoComboBox").value()) {
            return true;
        }
    }
    return false;
}

function ExisteSpool() {
    var jsonGrid = $("#grid").data("kendoGrid").dataSource._data;

    for (var i = 0; i < jsonGrid.length; i++) {
        if (jsonGrid[i].OrdenTrabajoSpoolID == $("#InputID").data("kendoComboBox").value()) {

            return true;
        }
    }
    return false;
}

function SiguienteProceso(paramReq) {
    var url = "";
    if (paramReq == null) {
        url = "/PND/AsignacionRequisicion?leng=" + $("#language").data("kendoDropDownList").value();
    } else {
        url = "/PND/AsignacionRequisicion?leng=" + $("#language").data("kendoDropDownList").value()
            + "&requisicion=" + paramReq;
    }
    $("#GenerarRequisicionASignarRequisicionSup").attr("href", url);
    $("#GenerarRequisicionASignarRequisicionInf").attr("href", url);


}

function getParameterByName(name, url) {

    if (!url) url = window.location.href;
    name = name.replace(/[\[\]]/g, "\\$&");
    var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
        results = regex.exec(url);
    if (!results) return null;
    if (!results[2]) return '';
    return decodeURIComponent(results[2].replace(/\+/g, " "));
}

function cargarGrid() {
    $("#grid").kendoGrid({
        autoBind: true,
        autoSync: true,
        edit: function (e) {
            setTimeout(function () {
                var inputName = e.container.find('input');

                inputName.select();
            });
            if ($('#botonGuardar').text() != _dictionary.MensajeGuardar[$("#language").data("kendoDropDownList").value()])
                this.closeCell();
        },
        dataSource: {
            data: [],
            schema: {
                model: {
                    fields: {
                        Accion: { type: "number", editable: false },
                        NumeroControl: { type: "string", editable: false },
                        EtiquetaJunta: { type: "string", editable: false },
                        TipoJunta: { type: "string", editable: false },
                        NombreRequisicion: { type: "string", editable: false },
                        Cuadrante: { type: "string", editable: false },
                        Prioridad: { type: "number", editable: false },
                        Clasificacion: { type: "string", editable: false },
                        Diametro: { type: "number", editable: false },
                        Espesor: { type: "number", editable: false },
                        Cedula: { type: "string", editable: false },
                        ElementoPorClasificacionPNDID: { type: "int", editable: false },
                        RequisicionID: { type: "int", editable: false },
                        ProyectoID: { type: "int", editable: false },
                        SpoolID: { type: "int", editable: false },
                        JuntaSpoolID: { type: "int", editable: false },
                        OrdenTrabajoSpoolID: { type: "int", editable: false },
                        JuntaSpoolID: { type: "int", editable: false },
                        TipoPruebaID: { type: "int", editable: false },
                        Especificacion: { type: "string", editable: false },
                        Agregar: { type: "boolean", editable: false },
                        EstatusCaptura: { type: "int", editable: false }
                    }
                }
            },
            pageSize: 10,
            serverPaging: false,
            serverFiltering: false,
            serverSorting: false
        },
        navigatable: true,
        autoHeight: true,
        sortable: true,
        scrollable: true,
        editable: true,
        selectable: true,
        pageable: {
            refresh: false,
            pageSizes: [10, 25, 50, 100],
            info: false,
            input: false,
            numeric: true,
        },
        filterable: getGridFilterableMaftec(),
        columns: [
            { field: "NumeroControl", title: _dictionary.columnNumeroControl[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "130px" },
            { field: "EtiquetaJunta", title: _dictionary.columnJunta[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "80px", attributes: { style: "text-align:right;" } },
            { field: "TipoJunta", title: _dictionary.columnTipoJta[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "112px" },
            { field: "NombreRequisicion", title: _dictionary.columnRequisicion[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "135px" },
            { field: "Cuadrante", title: _dictionary.columnCuadrante[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "127px" },
            { field: "Prioridad", title: _dictionary.columnPrioridad[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellNumberMaftec(), width: "90px", attributes: { style: "text-align:right;" } },
            { field: "Clasificacion", title: _dictionary.columnClasificacion[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "85px" },
            { field: "Diametro", title: _dictionary.columnDiametro[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellNumberMaftec(), width: "94px", attributes: { style: "text-align:right;" } },
            { field: "Espesor", title: _dictionary.columnEspesor[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellNumberMaftec(), width: "112px", attributes: { style: "text-align:right;" } },
            { field: "Cedula", title: _dictionary.columnCedula[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "105px" },
            {
                field: "Agregar", title: _dictionary.columnAgregar[$("#language").data("kendoDropDownList").value()], filterable: {
                    multi: true,
                    messages: {
                        isTrue: _dictionary.lblVerdadero[$("#language").data("kendoDropDownList").value()],
                        isFalse: _dictionary.lblFalso[$("#language").data("kendoDropDownList").value()],
                        style: "max-width:100px;"
                    },
                    dataSource: [{ Etiquetado: true }, { Etiquetado: false }]
                }, template: "<input name='fullyPaid' class='ob-paid' type='checkbox' data-bind='checked: Agregar' #= Agregar ? checked='checked' : '' #/>", width: "112px", attributes: { style: "text-align:center;" }
            },
        ],
        dataBound: function (a) {
            $(".ob-paid").bind("change", function (e) {
                if ($('#btnGuardar').text() == _dictionary.MensajeGuardar[$("#language").data("kendoDropDownList").value()]) {
                    var grid = $("#grid").data("kendoGrid"),
                    dataItem = grid.dataItem($(e.target).closest("tr"));

                    
                    if (dataItem.RequisicionID != 0) {
                        if (dataItem.Agregar && !e.target.checked) {
                            dataItem.Accion = 3;
                        }
                        else if(!dataItem.Agregar && e.target.checked) {
                            dataItem.Accion = 2;
                        }
                    }
                    if (e.target.checked == true)
                        dataItem.Agregar = true;
                    else
                        dataItem.Agregar = false;

                    
                }
                else {
                    if (e.target.checked) {
                        e.target.checked = false;
                        $("#grid").data("kendoGrid").dataItem($(e.target).closest("tr")).Agregar = false;
                    }
                    else {
                        e.target.checked = true;
                        $("#grid").data("kendoGrid").dataItem($(e.target).closest("tr")).Agregar = true;
                    }
                }
                $("#grid").data("kendoGrid").dataSource.sync();
            });
        }
    });

    $("#grid").on("change", ":checkbox", function (e) {
        if ($('#Guardar').text() == _dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]) {
            var grid = $("#grid").data("kendoGrid"),
            dataItem = grid.dataItem($(e.target).closest("tr"));

            if ($(this)[0].checked) {
                dataItem.Agregar = true;
                if (dataItem.Accion == 2) {
                    dataItem.EstatusCaptura = 0;
                }
            } else {
                dataItem.Agregar = false;
                if (dataItem.Accion == 2) {
                    dataItem.EstatusCaptura = 1;
                }
            }
        } else {
            if (e.target.checked) {
                e.target.checked = false;
                $("#grid").data("kendoGrid").dataItem($(e.target).closest("tr")).Agregar = false;
            }
            else {
                e.target.checked = true;
                $("#grid").data("kendoGrid").dataItem($(e.target).closest("tr")).Agregar = true;
            }
        }
        $("#grid").data("kendoGrid").dataSource.sync();
    });

    CustomisaGrid($("#grid"));
}

function FiltroMostrar(mostrar) {
    var ds = $("#grid").data("kendoGrid").dataSource;

    if (mostrar == 0) {
        var curr_filters = ds.filter().filters;
        if (curr_filters[0].filters != undefined)
            ds.filter(curr_filters[0].filters[0])
        else
            ds.filter(curr_filters[0])
        ds.sync();


    }
    else {

        var curr_filters = ds.filter().filters;
        ds.filter(curr_filters[0])
        ds.sync();
        var filters = ds.filter();
        filters.logic = "or"

        filters.filters.push({ field: "Accion", operator: "eq", value: 2 });
        ds.sync();
    }
}


function validaInformacionCapturada() {
    var ds = $("#grid").data("kendoGrid").dataSource;
    var filters = ds.filter();
    var allData = ds.data();
    var query = new kendo.data.Query(allData);
    var data = query.filter(filters).data;

    var contador = 0;
    if (data.length > 0) {
        for (var i = 0; i < data.length; i++) {
            if (data[i].EstatusCaptura == 1) {
                contador++;
            }
        }
        if (contador > 0) {
            return true;
        } else {
            return false;
        }
    } else {
        return false;
    }
}

function tieneClase(item) {
    for (var i = 0; i < item.classList.length; i++) {
        if (item.classList[i] == "k-state-border-up" || item.classList[i] == "k-state-border-down")
            return true;
    }
    return false;
}

function existenCambios(arregloCaptura) {
    for (index = 0; index < arregloCaptura.length; index++) {
        if (arregloCaptura[index].Agregar == true && arregloCaptura[index].RequisicionID == 0)
            return true;
    }
    return false;
}