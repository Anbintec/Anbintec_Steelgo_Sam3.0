IniciarDashboardPND();
function IniciarDashboardPND() {
    SuscribirEventos();    
}
function changeLanguageCall() {    
    AjaxCargarHeaderDashboard();
    AjaxObtenerProyectos();
    AjaxCargarPeriodos();
    CargarGrid();
    CargarGridPopUp();
    document.title = _dictionary.menuServiciosTecnicosDashboardPND[$("#language").data("kendoDropDownList").value()];
    $("#inputFechaInicio").data("kendoDatePicker").setOptions({
        format: _dictionary.FormatoFecha2[$("#language").data("kendoDropDownList").value()]
    });
    $("#inputFechaFin").data("kendoDatePicker").setOptions({
        format: _dictionary.FormatoFecha2[$("#language").data("kendoDropDownList").value()]
    });
    OcultarCampos(true);
}

function AgregarStatusDinamicos(listaStatus) {
    document.getElementById("divStatusRequisiciones").innerHTML = "";

    for (var i = 0; i < listaStatus.length; i++) {
        $("#divStatusRequisiciones").append("<button  onclick='ActivarRefrescarGrid(" + listaStatus[i].Estatus_DashboardID + ")' id='"
            + listaStatus[i].Estatus_DashboardID + "' class='btn btn-tabList btn-listRequisicion'><span id='" + listaStatus[i].Estatus_DashboardID + "'>"
            + listaStatus[i].Descripcion + " </span><span  class='porElemento'  id='" + listaStatus[i].NumeroElementos + "'>" + listaStatus[i].NumeroElementos
            +" </span><span class='porRequisicion' style='display:none;'  id='" + listaStatus[i].NumeroRequisiciones + "'>" + listaStatus[i].NumeroRequisiciones + "</span></button>");
        if (i == 0) {
            //AjaxAccionesListado(listaStatus[i].Estatus_DashboardID);
            $("#" + listaStatus[i].Estatus_DashboardID).addClass("active");
        }
    }
}

function VerDetalle(e) {
    var dataItem = $("#grid").data("kendoGrid").dataItem($(e.currentTarget).closest("tr"));
    document.location.target = "_blank";
    document.location.href = "/serviciostecnicos/CapturaReportePruebas?id=" + dataItem.Folio;
};

function tabActivo(idButton) {
    $(".btn-tabList").removeClass("active");
    var list = document.getElementById("divStatusRequisiciones").getElementsByTagName("button");

    for (var i = 0; i < list.length; i++) {
        if (list[i].id == idButton) {
            $("#" + idButton).addClass("active");
            break;
        }
    }
};


function CargarGrid() {
    $("#grid").kendoGrid({
        autoBind: true,
        dataSource: {
            schema: {
                model: {
                    fields: {
                        TipoPrueba: { type: "string", editable: false },
                        Requisicion: { type: "string", editable: false },
                        FechaAsignacion: { type: "date", editable: false },
                        NumeroElementos: { type: "string", editable: false },
                        Proveedor: { type: "string", editable: false },
                        Equipo: { type: "string", editable: false },
                        Turno: { type: "string", editable: false },
                        Url: { type: "string", editable: false },
                    }
                }
            },
            pageSize: 10,
            serverPaging: false,
            serverFiltering: false,
            serverSorting: false
        },
        navigatable: true,
        filterable: {
            extra: false
        },
        editable: true,
        autoHeight: true,
        sortable: true,
        scrollable: true,
        pageable: {
            refresh: false,
            pageSizes: [10, 25, 50,100],
            info: false,
            input: false,
            numeric: true,
        },
        filterable: getGridFilterableMaftec(),
        columns: [
            { field: "TipoPrueba", title: _dictionary.columnTipoPrueba[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec() },
            { field: "Requisicion", title: _dictionary.columnRequisicion[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), template: " <div class='' style='text-align:center;'><a href='#=Url##=RequisicionID#'  > <span>#=Requisicion#</span></a></div> " },
            { field: "Fecha", title: _dictionary.lblFechaRequisicion[$("#language").data("kendoDropDownList").value()], filterable: { cell: { showOperators: false } },  format: _dictionary.FormatoFecha[$("#language").data("kendoDropDownList").value()] },
            { field: "NumeroElementos", title: _dictionary.columnJuntas[$("#language").data("kendoDropDownList").value()], template: "<div class='EnlaceDetalleElementos' style='text-align:center;'><a href='\\#'  > <span>#=NumeroElementos#</span></a></div>", filterable: getGridFilterableCellMaftec() },
            { field: "Proveedor", title: _dictionary.columnProveedor[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec() },
            { field: "Equipo", title: _dictionary.columnEquipo[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec() },
            { field: "Turno", title: _dictionary.columnTurnoLaboral[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec() },
            
        ]
    });
    CustomisaGrid($("#grid"));
};



function CargarGridPopUp() {

    $("#gridPopUp").kendoGrid({
        dataSource: {
            data: [],
            schema: {
                model: {
                    fields: {
                        NumeroControl: { type: "string", editable: false },
                        EtiquetaJunta: { type: "string", editable: false },
                        TipoJunta: { type: "string", editable: false },
                        NombreRequisicion: { type: "string", editable: false },
                        Cuadrante: { type: "string", editable: false },
                        Prioridad: { type: "number", editable: false },
                        Clasificacion: { type: "string", editable: false },
                        Diametro: { type: "string", editable: false },
                        Espesor: { type: "string", editable: false },
                        Cedula: { type: "string", editable: false },
                        RequisicionID: { type: "int", editable: false },
                        ProyectoID: { type: "int", editable: false },
                        SpoolID: { type: "int", editable: false },
                        OrdenTrabajoSpoolID: { type: "int", editable: false },
                        JuntaSpoolID: { type: "int", editable: false },
                        TipoPruebaID: { type: "int", editable: false },
                        Especificacion: { type: "number", editable: false },
                    }
                }
            },
            pageSize: 10,
            serverPaging: false,
            serverFiltering: false,
            serverSorting: false

        },
        pageable: {
            refresh: false,
            pageSizes: [10, 25, 50, 100],
            info: false,
            input: false,
            numeric: true,
        },
        selectable: true,
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
            { field: "Cedula", title: _dictionary.columnCedula[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "105px" }
        ],
        editable: false,
        navigatable: true
    });
    CustomisaGrid($("#gridPopUp"));
};



function LlenarGridPopUp(data) {
    modeloRenglon = data;
    $("#gridPopUp").data('kendoGrid').dataSource.data([]);
    var ds = $("#gridPopUp").data("kendoGrid").dataSource;
    var array = data.listaElementosRequisicion;
    for (var i = 0; i < array.length; i++) {
        ds.add(array[i]);
    }
    VentanaModal();
}


function VentanaModal() {

    var modalTitle = "";
    modalTitle = _dictionary.lblDetalleJuntas[$("#language").data("kendoDropDownList").value()];
    var window = $("#windowGrid");
    var win = window.kendoWindow({
        modal: true,
        title: modalTitle,
        resizable: false,
        visible: true,
        width: "95%",
        minWidth: 30,
        position: {
            top: "10px",
            left: "10px"
        },
        actions: [
            "Close"
        ],
        close: function onClose(e) {
            var gridDataSource = $("#gridPopUp").data("kendoGrid").dataSource;
            gridDataSource.filter([]);
        }
    }).data("kendoWindow");
    window.data("kendoWindow").title(modalTitle);
    window.data("kendoWindow").center().open();
};

function OcultarCampos(Visible) {
    if (!Visible) {
        $("#tabEstatus").css("display", "block");
        $("#ContenedorGrid").css("display", "block");
        $("#contenidoDashboard").css("display", "none");
    } else {
        $("#tabEstatus").css("display", "none");        
        $("#ContenedorGrid").css("display", "none");        
    }
}