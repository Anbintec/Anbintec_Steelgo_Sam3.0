function LlenarGridPopUp() {

    $("#gridPopUp").data('kendoGrid').dataSource.data([]);
    var ds = $("#gridPopUp").data("kendoGrid").dataSource;
    ListaPruebas = window.parent.registroCompleto.listadoPruebasProceso;
    ListaUnidadMedida = window.parent.registroCompleto.listadoUnidadesMedida;
    var array = window.parent.registroCompleto.listadoPruebasDetalle;
    for (var i = 0; i < array.length; i++) {
        ds.add(array[i]);
    }
    //VentanaModal();
}

function changeLanguageCall() {
   // $("#ContenedorGridPopUp").remove();
   // $("#ContenedorGridPopUp").appendTo("<div id='gridPopUp' data-role='grid' class='k-grid k-widget'></div>")
    hideElements();
    CargarGridPopUp();
   // 
}

SuscribirEventos()



function hideElements() {

    $(".sidebar").hide();
    $(".logo").hide();
    $(".search-bar").hide();
    $(".notifications").hide();
    $(".logged-user").hide();
    $(".content-container").removeClass("topbar").addClass("printView");
    $(".breadcrumb-container").hide();
    $(".languageSelector").hide();
    $(".pull-right").hide();
    $("header").hide();
    $(".content-frame").removeClass("content-frame");
    $("body").css("background", "#FFFFFF");
};


function CargarGridPopUp() {
    if ($("#gridPopUp").data('kendoGrid') == undefined) {
        $("#gridPopUp").kendoGrid({
            edit: function (e) {
                //setTimeout(function () {
                //    var inputName = e.container.find('input');
                //    inputName.select();
                //});
                if (window.parent.tieneAvance) {
                    this.closeCell();
                }
            },
            dataSource: {
                data: [],
                schema: {
                    model: {
                        fields: {
                            Accion: { type: "int", editable: false },
                            ConfiguracionLote: { type: "boolean", editable: false },
                            Cantidad: { type: "number", editable: true },
                            NumeroPruebas: { type: "number", editable: true },
                            Prueba: { type: "string", editable: true },
                            UnidadMedida: { type: "string", editable: false },
                            UnidadMinima: { type: "number", editable: true },
                            UnidadMaxima: { type: "number", editable: true }

                        }
                    }
                },
                filter: {
                    logic: "or",
                    filters: [
                      { field: "Accion", operator: "eq", value: 1 },
                      { field: "Accion", operator: "eq", value: undefined },
                      { field: "Accion", operator: "eq", value: 2 },
                      { field: "Accion", operator: "eq", value: 4 }
                    ]
                },
                pageSize: 10,
                serverPaging: false,
                serverFiltering: false,
                serverSorting: false

            },
            pageable: false,
            selectable: true,
            filterable: getGridFilterableMaftec(),

            columns: [
                {
                    field: "ConfiguracionLote", title: _dictionary.ConfiguracionLote[$("#language").data("kendoDropDownList").value()], filterable: {
                        multi: true,
                        messages: {
                            isTrue: _dictionary.lblVerdadero[$("#language").data("kendoDropDownList").value()],
                            isFalse: _dictionary.lblFalso[$("#language").data("kendoDropDownList").value()],
                            style: "max-width:100px;"
                        },
                        dataSource: [{ ConfiguracionLote: true }, { ConfiguracionLote: false }]
                    },

                    width: "260px", attributes: { style: "text-align:center;" }
                },
                { field: "Cantidad", title: _dictionary.Cantidad[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellNumberMaftec(), width: "100%", attributes: { style: "text-align:right;" } },
                { field: "NumeroPruebas", title: _dictionary.columnPruebasLote[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellNumberMaftec(), width: "100%", editor: RenderNumeroPruebas, attributes: { style: "text-align:right;" }, format: "{0: }" },
                { field: "ProyectoProcesoPrueba", title: _dictionary.lblPrueba[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftecpopUp(), width: "100%", editor: comboBoxPruebas },
                { field: "UnidadMedida", title: _dictionary.columnUnidadMedida[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftecpopUp(), width: "100%" },
                { field: "UnidadMinima", title: _dictionary.columnUnidadMinima[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellNumberMaftec(), width: "80px", editor: RenderUnidadMinima, attributes: { style: "text-align:right;" }, format: "{0: }" },
                { field: "UnidadMaxima", title: _dictionary.columnUnidadMaxima[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellNumberMaftec(), width: "80px", editor: RenderUnidadMaxima, attributes: { style: "text-align:right;" }, format: "{0: }" },
                { command: { text: _dictionary.botonCancelar[$("#language").data("kendoDropDownList").value()], click: cancelarCaptura }, title: _dictionary.columnELM[$("#language").data("kendoDropDownList").value()], width: "100%", attributes: { style: "text-align:center;" } },
                { command: { text: _dictionary.botonLimpiar[$("#language").data("kendoDropDownList").value()], click: limpiarRenglon }, title: _dictionary.columnLimpiar[$("#language").data("kendoDropDownList").value()], width: "100%", attributes: { style: "text-align:center;" } }
            ],
            editable: true,
            navigatable: true,
            toolbar: [{ name: "create" }],
            dataBound: function (e) {
                var grid = $("#gridPopUp").data("kendoGrid");
                var gridData = grid.dataSource.view();

                if (gridData.length > 0) {
                    for (var i = 0; i < gridData.length; i++) {
                        var currentUid = gridData[i].uid;
                        var currenRow = grid.table.find("tr[data-uid='" + currentUid + "']");

                        if (gridData[i].ConfiguracionLote == true) {
                            currenRow[0].childNodes[0].outerHTML =
                          "<td role='gridcell' style='text-align: center;'>" +
                          "<table><tr><td>" + "<input class='RadioSector' style=' display:inline-block; width:33% !important;'   type='radio' name='" + i + "' checked=checked > Spool"
                        + "<input class='RadioSector' style=' display:inline-block; width:33% !important;'   type='radio' name='" + i + "' >M.Lote"

                        + "</td>" + "</td></tr></table>";
                        } else if (gridData[i].ConfiguracionLote == false) {
                            currenRow[0].childNodes[0].outerHTML =
                          "<td role='gridcell' style='text-align: center;'>" +
                         "<table><tr><td>" + "<input class='RadioSector' style=' display:inline-block; width:33% !important;'   type='radio' name='" + i + "'  >Spool"
                        + "<input class='RadioSector' style=' display:inline-block; width:33% !important;'  type='radio' name='" + i + "' checked=checked >M.Lote"

                        + "</td>" + "</td></tr></table>";
                        }


                    }
                }
            }

        });

        $("#gridPopUp table").on("keydown", function (e) {
            if (e.keyCode == 13) {
                e.preventDefault();
                var dataSource = $("#gridPopUp").data("kendoGrid").dataSource;
                var total = $("#gridPopUp").data("kendoGrid").dataSource.data().length;
                $("#gridPopUp").data("kendoGrid").dataSource.insert(total, {});
                $("#gridPopUp").data("kendoGrid").dataSource.page(dataSource.totalPages());
                $("#gridPopUp").data("kendoGrid").editRow($("#gridPopUp").data("kendoGrid").tbody.children().last());

                //}
            }
        });

        $("#gridPopUp").on("change", ":radio", function (e) {
            var grid = $("#gridPopUp").data("kendoGrid"),
            dataItem = grid.dataItem($(e.target).closest("tr"));

            if ($('#Guardar').text() == _dictionary.MensajeGuardar[$("#language").data("kendoDropDownList").value()]) {

                dataItem.ConfiguracionLote = e.target.checked;
            }
            else {
                dataItem.ConfiguracionLote = !e.target.checked;
                // $("#grid").data("kendoGrid").dataSource.sync();
                grid.dataSource.sync();
            }

        });
        
        CustomisaGrid($("#gridPopUp"));
    }

    LlenarGridPopUp();
};

function cancelarCaptura(e) {
    if (!window.parent.tieneAvance) {
        e.preventDefault();
        var filterValue = $(e.currentTarget).val();
        var dataItem = $("#gridPopUp").data("kendoGrid").dataItem($(e.currentTarget).closest("tr"));
        var dataSource = $("#gridPopUp").data("kendoGrid").dataSource;
        //if (dataItem.Accion == 2) {
        //	dataItem.Accion = 3;
        //}
        //else {
        //	dataSource.remove(dataItem);
        //}
        dataSource.remove(dataItem);
        $("#gridPopUp").data("kendoGrid").dataSource.sync();
    }

}


function limpiarRenglon(e) {
    if (!tieneAvance) {

        e.preventDefault();
        if ($('#Guardar').text() == _dictionary.lblGuardar[$("#language").data("kendoDropDownList").value()]) {

            var itemToClean = $("#gridPopUp").data("kendoGrid").dataItem($(e.currentTarget).closest("tr"));

            if (itemToClean.Accion == 2)
                itemToClean.Accion = 4;

            itemToClean.ProyectoProcesoPrueba = "";
            itemToClean.ProyectoProcesoPruebaID = 0;
            itemToClean.UnidadMedida = "";
            itemToClean.UnidadMedidaID = 0;
            itemToClean.UnidadMinima = 0;
            itemToClean.UnidadMaxima = 0;

            var dataSource = $("#gridPopUp").data("kendoGrid").dataSource;
            dataSource.sync();
            //alert(itemToClean);
        }

    }
}