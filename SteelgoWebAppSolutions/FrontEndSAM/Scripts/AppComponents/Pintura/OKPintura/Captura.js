IniciarCapturaOkPintura();

function IniciarCapturaOkPintura() {
    SuscribirEventos();
}

function changeLanguageCall() {
    CargarGrid();
    CargarGridPopUp();
    document.title = _dictionary.menuOkPintura[$("#language").val()]
    AjaxGetListaProyectos();
    $('input:radio[name=Muestra]:nth(1)').trigger("click");
}

function CargarGrid() {
    $("#grid").kendoGrid({
        autoBind: true,
        autoSync: true,
        edit: function (e) {
            var inputName = e.container.find('input');
            inputName.select();
            if ($('#BotonGuardar').text() != _dictionary.MensajeGuardar[$("#language").data("kendoDropDownList").value()])
                this.closeCell();
        },        
        dataSource: {
            schema: {
                model: {
                    fields: {
                        Accion: { type: "int", editable: false },
                        SpoolWorkStatusID: { type: "int", editable: false },
                        NumeroControl: { type: "string", editable: false },
                        Cuadrante: { type: "string", editable: false },
                        Prioridad: { type: "number", editable: false },
                        SpoolID: { type: "int", editable: false },
                        OrdenTrabajoSpoolID: { type: "int", editable: false },
                        ProyectoID: { type: "int", editable: false },
                        SHOTID: { type: "int", editable: false },
                        PRIMID: { type: "int", editable: false },
                        INTERID: { type: "int", editable: false },
                        ACABID: { type: "int", editable: false },
                        FechaProcesoSHOT: { type: "Date", editable: false },
                        ShotBlastPrueba: { type: "string", editable: false },
                        FechaProcesoPRIM: { type: "Date", editable: false },
                        PrimarioPrueba: { type: "string", editable: false },
                        FechaProcesoINTER: { type: "Date", editable: false },
                        IntermedioPrueba: { type: "string", editable: false },
                        FechaProcesoACAB: { type: "Date", editable: false },
                        AcabadoPrueba: { type: "string", editable: false },
                        OkPintura: { type: "boolean", editable: false }                        
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
            { field: "Cuadrante", title: _dictionary.columnCuadrante[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "127px" },
            { field: "Prioridad", title: _dictionary.columnPrioridad[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellNumberMaftec(), width: "80px", attributes: { style: "text-align:right;" } },
            { field: "FechaProcesoSHOT", title: _dictionary.columnFechaPintura[$("#language").data("kendoDropDownList").value()], filterable: getKendoGridFilterableDateMaftec(), width: "150px", format: _dictionary.FormatoFecha[$("#language").data("kendoDropDownList").value()], attributes: { style: "text-align:center;" } },
            { field: "ShotBlastPrueba", title: _dictionary.columnPrueba[$("#language").data("kendoDropDownList").value()], template: "<div class='EnlaceDetallePruebas shot' style='text-align:center;'><a href='\\#'  > <span>#=Detalle#</span></a></div>", filterable: false, width: "110px" },
            { field: "FechaProcesoPRIM", title: _dictionary.columnFechaPintura[$("#language").data("kendoDropDownList").value()], filterable: getKendoGridFilterableDateMaftec(), width: "150px", format: _dictionary.FormatoFecha[$("#language").data("kendoDropDownList").value()], attributes: { style: "text-align:center;" } },
            { field: "PrimarioPrueba", title: _dictionary.columnPrueba[$("#language").data("kendoDropDownList").value()], template: "<div class='EnlaceDetallePruebas prim' style='text-align:center;'><a href='\\#'  > <span>#=Detalle#</span></a></div>", filterable: false, width: "110px" },
            { field: "FechaProcesoINTER", title: _dictionary.columnFechaPintura[$("#language").data("kendoDropDownList").value()], filterable: getKendoGridFilterableDateMaftec(), width: "150px", format: _dictionary.FormatoFecha[$("#language").data("kendoDropDownList").value()], attributes: { style: "text-align:center;" } },
            { field: "IntermedioPrueba", title: _dictionary.columnPrueba[$("#language").data("kendoDropDownList").value()], template: "<div class='EnlaceDetallePruebas inter' style='text-align:center;'><a href='\\#'  > <span>#=Detalle#</span></a></div>", filterable: false, width: "110px" },
            { field: "FechaProcesoACAB", title: _dictionary.columnFechaPintura[$("#language").data("kendoDropDownList").value()], filterable: getKendoGridFilterableDateMaftec(), width: "150px", format: _dictionary.FormatoFecha[$("#language").data("kendoDropDownList").value()], attributes: { style: "text-align:center;" } },
            { field: "AcabadoPrueba", title: _dictionary.columnPrueba[$("#language").data("kendoDropDownList").value()], template: "<div class='EnlaceDetallePruebas acab' style='text-align:center;'><a href='\\#'  > <span>#=Detalle#</span></a></div>", filterable: false, width: "110px" },
            {
                field: "OkPintura", title: _dictionary.columnOkPintura[$("#language").data("kendoDropDownList").value()], filterable: {
                    multi: true,
                    messages: {
                        isTrue: _dictionary.lblVerdadero[$("#language").data("kendoDropDownList").value()],
                        isFalse: _dictionary.lblFalso[$("#language").data("kendoDropDownList").value()],
                        style: "max-width:100px;"
                    },
                    dataSource: [{ Etiquetado: true }, { Etiquetado: false }]
                }, template: "<input name='fullyPaid' class='ob-paid' type='checkbox' #= OkPintura ? 'checked=checked':'' #/>", width: "100px", attributes: { style: "text-align:center;" }
            }
        ],
        dataBound: function (a) {
            var myElem = document.getElementById('trParentHeader');
            if (myElem === null) {
                $("#grid").find("th.k-header").parent().before("<tr id='trParentHeader'> " +
                    "<th scope='col' colspan='3' class='k-header'></th> <th width='auto'  colspan='2' class='k-header' style='text-align: center;'><span id=''>" + _dictionary.lblShotblast[$("#language").data("kendoDropDownList").value()] + "</span></th>" +
                    "<th width='auto'  colspan='2' class='k-header' style='text-align: center;'><span>" + _dictionary.lblPrimario[$("#language").data("kendoDropDownList").value()] + "</span></th>" +
                    "<th width='auto'  colspan='2' class='k-header' style='text-align: center;'><span id=''>" + _dictionary.lblIntermedio[$("#language").data("kendoDropDownList").value()] + "</span></th>" +
                    "<th width='auto'  colspan='2' class='k-header' style='text-align: center;'><span id=''>" + _dictionary.lblAcabado[$("#language").data("kendoDropDownList").value()] + "</span></th>" +
                    "<th width='auto'  colspan='1' class='k-header' style='text-align: center;'><span id=''></span></th>" +
                    "</tr>");
            }


            $(".ob-paid").bind("change", function (e) {
                if ($('#BotonGuardar').text() == _dictionary.MensajeGuardar[$("#language").data("kendoDropDownList").value()]) {
                    var grid = $("#grid").data("kendoGrid");
                    dataItem = grid.dataItem($(e.target).closest("tr"));
                    if (dataItem != null) {
                        if (e.target.checked == true)                            
                            dataItem.OkPintura = true;
                        else                           
                            dataItem.OkPintura = false;
                    }                    
                }
                else
                    $("#grid").data("kendoGrid").closeCell();                
            });
        }
    });
    CustomisaGrid($("#grid"));
    /*GRID PARA CARGA MASIVA*/
    $("#gridMasivo").kendoGrid({
        autoBind: true,
        autoSync: true,
        edit: function (e) {
            if ($('#BotonGuardar').text() != _dictionary.MensajeGuardar[$("#language").data("kendoDropDownList").value()])
                this.closeCell();
        },
        dataSource: {
            data: '',
            schema: {
                model: {
                    fields: {
                        SPOOLID: { type: "string", editable: false },
                        OKPINTURA: { type: "boolean", editable: false }
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
            { field: "SPOOLID", title: _dictionary.columnNumeroControl[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "130px" },
            {
                field: "OKPINTURA", title: _dictionary.columnOkPintura[$("#language").data("kendoDropDownList").value()], filterable: {
                    multi: true,
                    messages: {
                        isTrue: _dictionary.lblVerdadero[$("#language").data("kendoDropDownList").value()],
                        isFalse: _dictionary.lblFalso[$("#language").data("kendoDropDownList").value()],
                        style: "max-width:100px;"
                    },
                    dataSource: [{ Etiquetado: true }, { Etiquetado: false }]                    
                }, template: "<input name='fullyPaid' class='ob-paid' type='checkbox' #= OKPINTURA ? 'checked=checked':'' #/>", width: "50px", attributes: { style: "text-align:center;" }
            },
        ]
    });
    $("#gridMasivo").hide();
    CustomisaGrid($("#gridMasivo"));
};

function CargarGridPopUp() {
    $("#gridPopUp").kendoGrid({        
        dataSource: {
            data: [],
            schema: {
                model: {
                    fields: {
                        Accion: { type: "number", editable: false },
                        ProcesoID: { type: "int", editable: false },
                        Prueba: { type: "string", editable: false },
                        UnidadMedida: { type: "string", editable: false },
                        UnidadMinima: { type: "double", editable: false },
                        UnidadMaxima: { type: "double", editable: false },
                        Color: { type: "string", editable: false }
                    }
                }
            },
            pageSize: 10,
            serverPaging: false,
            serverFiltering: false,
            serverSorting: false,            
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
        editable: false,
        navigatable: true,
        scrollable: false,      
        columns: [
                { field: "Prueba", title: _dictionary.lblPrueba[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "20px" },
                { field: "UnidadMedida", title: "U. Medida", filterable: getGridFilterableCellMaftec(), width: "20px" },
                { field: "UnidadMinima", title: _dictionary.columnUnidadMinima[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellNumberMaftec(), format: "{0:n2}", width: "20px", attributes: { style: "text-align:right;" } },
                { field: "UnidadMaxima", title: _dictionary.columnUnidadMaxima[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellNumberMaftec(), format: "{0:n2}", width: "20px", attributes: { style: "text-align:right;" } },
                { field: "Color", title: _dictionary.lblColor[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec, width: "20px" }
        ]        

    });
    CustomisaGrid($("#gridPopUp"));
};

function LlenarGridPopUp(data) {
    $("#gridPopUp").data('kendoGrid').dataSource.data([]);
    var ds = $("#gridPopUp").data("kendoGrid").dataSource;
    var grid = $("#gridPopUp").data("kendoGrid");
    //grid.hideColumn("Name");
    var array = data;
    for (var i = 0; i < array.length; i++) {
        if (data[0].ProcesoID != 4) {
            grid.hideColumn("Color");
        } else {
            grid.showColumn("Color");
        }
        ds.add(array[i]);
    }
    $("#gridPopUp").data("kendoGrid").refresh();
    VentanaModal();
}

function VentanaModal() {
    //var modalTitle = "Detalle";
    var modalTitle = $("#language").data("kendoDropDownList").value() == 'es-MX' ? 'Detalles' : 'Details';    
    var window = $("#windowGrid");
    var win = window.kendoWindow({
        modal: true,
        title: modalTitle,
        resizable: false,
        visible: true,
        width: "60%",
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
    //window.data("kendoWindow").title(modalTitle);
    window.data("kendoWindow").center().open();

};

function aplicarPlanchado(arregloCaptura, value) {
    var dataSource = $("#grid").data("kendoGrid").dataSource;
    var filters = dataSource.filter();
    var allData = dataSource.data();
    var query = new kendo.data.Query(allData);
    var data = query.filter(filters).data;
    for (index = 0; index < data.length; index++) {
        data[index].OkPintura = value;
    }
}