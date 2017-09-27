var FechaInicio;
var FechaFin;
var esNormal;

function IniciarListadoEmbarquesCerrados() {
    SuscribirEventos();
}
function changeLanguageCall() {    
    CargarGrid();
    IniciarListadoEmbarquesCerrados();
    AjaxCargaProyecto();
    AjaxCargarPeriodos();
    document.title = _dictionary.lblListadoEmbarquesCerrados[$("#language").data("kendoDropDownList").value()];
    FechaInicio.data("kendoDatePicker").setOptions({
        format: _dictionary.FormatoFecha2[$("#language").data("kendoDropDownList").value()]
    });

    FechaFin.data("kendoDatePicker").setOptions({
        format: _dictionary.FormatoFecha2[$("#language").data("kendoDropDownList").value()]
    });
}

function CargarGrid() {
    $("#grid").kendoGrid({
        autoBind: true,
        edit: function (e) {
            if ($(".k-grid-content td").css("white-space") == "normal") {
                esNormal = true;
            }
            else {
                esNormal = false;
            }
        },
        dataSource: {
            schema: {
                model: {
                    fields: {
                        Embarque: { type: "string", editable: false },
                        Planas: { type: "string", editable: false },
                        Proyecto: { type: "string", editable: false },
                        Destino: { type: "string", editable: false },
                        PapelesCliente: { type: "boolean", editable: false },
                        PapelesAduana: { type: "boolean", editable: false },
                        FolioSolicitudPermiso: { type: "string", editable: false },
                        FechaSolicitudPermiso: { type: "string", editable: false },
                        AprobadoAduanaDesc: { type: "string", editable: false },
                        OkCliente: { type: "boolean", editable: false },
                        OkEmbarque: { type: "boolean", editable: false },
                        RequierePermisoAduana: { type: "boolean", editable: false },
                        RequierePapCliente: { type: "boolean", editable: false }
                    }
                }
            },
            pageSize: 10,
            serverPaging: false,
            serverFiltering: false,
            serverSorting: false
        },
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
        filterable: getGridFilterableMaftec(),
        columns: [
            { field: "Embarque", title: _dictionary.columnEmbarque[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "130px" },
            { field: "Proyecto", title: _dictionary.columnProyecto[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "130px" },
            { field: "Planas", title: _dictionary.columnPlanasEmb[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "160px" },
            { field: "Destino", title: _dictionary.columnDestinoEmb[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "145px" },            
            {
                field: "PapelesCliente", title: _dictionary.columnPapCliente[$("#language").data("kendoDropDownList").value()], filterable: false, template: "<center><button  type='button' class='btn btn-blue imprimirPapelesCliente' #= RequierePapCliente == false ? 'disabled' : '' #> <span>" +
                  _dictionary.lblImprimir[$("#language").data("kendoDropDownList").value()] + "</span></button></center>", width: "140px"
            },
            {
                field: "PapelesAduana", title: _dictionary.columnPapAduana[$("#language").data("kendoDropDownList").value()], filterable: false, template: "<center><button  type='button' class='btn btn-blue imprimirPapelesAduana' #= RequierePermisoAduana == false ?  'disabled': '' # > <span>" +
                    _dictionary.lblImprimir[$("#language").data("kendoDropDownList").value()] + "</span></button></center>", width: "140px"
            },
            { field: "FolioSolicitudPermiso", title: _dictionary.columnSolicitudPermiso[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "140px" },
            { field: "FechaSolicitudPermiso", title: _dictionary.columnFechaPermiso[$("#language").data("kendoDropDownList").value()], filterable: getKendoGridFilterableDateMaftec(), width: "150px" },
            { field: "AprobadoAduanaDesc", title: _dictionary.columnAprobadoAduana[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "130px" },            
            {
                field: "OkCliente", title: _dictionary.columnAprobadoCliente[$("#language").data("kendoDropDownList").value()], filterable: {
                    multi: true,
                    messages: {
                        isTrue: _dictionary.lblVerdadero[$("#language").data("kendoDropDownList").value()],
                        isFalse: _dictionary.lblFalso[$("#language").data("kendoDropDownList").value()],
                        style: "max-width:100px;"
                    },
                    dataSource: [{ OkCliente: true }, { OkCliente: false }]
                }, template: '<input type="checkbox" class="chk-OkCliente" #= OkCliente ? "checked=checked" : "" # class="chkbx" disabled></input>', width: "130px", attributes: { style: "text-align:center;" }
            },
            {
                field: "OkEmbarque", title: _dictionary.columnOkEmbarque[$("#language").data("kendoDropDownList").value()], filterable: {
                    multi: true,
                    messages: {
                        isTrue: _dictionary.lblVerdadero[$("#language").data("kendoDropDownList").value()],
                        isFalse: _dictionary.lblFalso[$("#language").data("kendoDropDownList").value()],
                        style: "max-width:100px;"
                    },
                    dataSource: [{ OkEmbarque: true }, { OkEmbarque: false }]
                }, template: '<input type="checkbox" class="chk-OkEmbarque" #= OkEmbarque ? "checked=checked" : "" # class="chkbx" disabled></input>', width: "150px", attributes: { style: "text-align:center;" }
            }
        ],
        dataBound: function (e) {
            if (esNormal) {
                $(".k-grid-content td").css("white-space", "normal");
            }
            else {
                $(".k-grid-content td").css("white-space", "nowrap");
            }
        }
    });
    CustomisaGrid($("#grid"));
};

function ValidarFecha(valor) {
    var fecha = kendo.toString(valor, String(_dictionary.FormatoFecha[$("#language").data("kendoDropDownList").value()].replace('{', '').replace('}', '').replace("0:", "")));
    if (fecha == null) {
        return false;
    }
    return true;
}
