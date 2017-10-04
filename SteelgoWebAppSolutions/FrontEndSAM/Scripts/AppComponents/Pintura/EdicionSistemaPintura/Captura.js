IniciarEdicionSistemaPintura();
var esNormal;

function IniciarEdicionSistemaPintura() {
    SuscribirEventos();
}

function changeLanguageCall() {
    CargarGrid();
    CargarGridDetalle();
    document.title = _dictionary.PinturaEdicionSPBreadcrumb[$("#language").data("kendoDropDownList").value()];
    AjaxCargaDetalleSistemaPintura();
}

function CargarGrid() {
    $("#grid").kendoGrid({
        edit: function (e) {

            var inputName = e.container.find('input');
            inputName.select();
            if ($('#Guardar').text() != _dictionary.lblGuardar[$("#language").data("kendoDropDownList").value()]) {
                this.closeCell();
            }

            if ($(".k-grid-content td").css("white-space") == "normal") {
                esNormal = true;
            }
            else {
                esNormal = false;
            }

        },
        dataBound: function () {
            var myElem = document.getElementById('trParentHeader');
            if (myElem == null) {
                $("#grid").find("th.k-header").parent().before("<tr id='trParentHeader'> " +
                    "<th scope='col' colspan='4' class='k-header'>" +
                    "</th><th width='auto'  colspan='6' class='k-header' style='text-align: center;'><span id=''>" + _dictionary.lblShotblast[$("#language").data("kendoDropDownList").value()] + "</span></th>" +
                    "<th width='auto'  colspan='6' class='k-header' style='text-align: center;'><span>" + _dictionary.lblPrimario[$("#language").data("kendoDropDownList").value()] + "</span></th>" +
                    "<th width='auto'  colspan='6' class='k-header' style='text-align: center;'><span id=''>" + _dictionary.lblIntermedio[$("#language").data("kendoDropDownList").value()] + "</span></th>" +
                    "<th width='auto'  colspan='6' class='k-header' style='text-align: center;'><span id=''>" + _dictionary.lblAcabado[$("#language").data("kendoDropDownList").value()] + "</span></th>" +
                    "<th width='auto'  colspan='1' class='k-header' style='text-align: center;'><span id=''></span></th>" +
                    "</tr>");
            }

            if (esNormal) {
                $(".k-grid-content td").css("white-space", "normal");
            }
            else {
                $(".k-grid-content td").css("white-space", "nowrap");
            }
        },
        autoBind: true,
        dataSource: {
            data: [],
            schema: {
                model: {
                    fields: {
                        Accion: { type: "number", editable: false },
                        SistemaPinturaID: { type: "number", editable: false },
                        SistemaPintura: { type: "string", editable: false },
                        ProyectoID: { type: "number", editable: false },
                        Proyecto: { type: "string", editable: false },
                        Color: { type: "string", editable: false },

                        PruebaPorLoteShotblast: { type: "number", editable: false },
                        MetrosPorLoteShotblast: { type: "number", editable: false },
                        PruebaShotblast: { type: "string", editable: false },

                        NumeroComponentesShootblast: { type: "number", editable: false },
                        TemplateComponentesShotBlast: { type: "string", editable: false },
                        TemplateReductoresShotblast: { type: "string", editable: false },

                        PruebaPorLotePrimario: { type: "number", editable: false },
                        MetrosPorLotePrimario: { type: "number", editable: false },
                        PruebaPrimario: { type: "string", editable: false },

                        NumeroComponentesPrimario: { type: "number", editable: false },
                        TemplateComponentesPrimario: { type: "string", editable: false },
                        TemplateReductoresPrimario: { type: "string", editable: false },

                        PruebaPorLoteIntermedio: { type: "number", editable: false },
                        MetrosPorLoteIntermedio: { type: "number", editable: false },
                        PruebaIntermedio: { type: "string", editable: false },

                        NumeroComponentesIntermedio: { type: "number", editable: false },
                        TemplateComponentesIntermedio: { type: "string", editable: false },
                        TemplateReductoresIntermedio: { type: "string", editable: false },

                        PruebaPorLoteAcabado: { type: "number", editable: false },
                        MetrosPorLoteAcabado: { type: "number", editable: false },
                        PruebaAcabado: { type: "string", editable: false },

                        NumeroComponentesAcabado: { type: "number", editable: false },
                        TemplateComponentesAcabado: { type: "string", editable: false },
                        TemplateReductoresAcabado: { type: "string", editable: false }
                    }
                }
            },
            filter: {
                logic: "or",
                filters: [
                  { field: "Accion", operator: "eq", value: 1 }
                ]
            },
            pageSize: 10,
            serverPaging: false,
            serverFiltering: false,
            serverSorting: false,
        },
        navigatable: true,
        editable: false,
        autoHeight: true,
        sortable: true,
        scrollable: true,
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
            {
                command: { text: _dictionary.botonCancelar[$("#language").data("kendoDropDownList").value()], click: eliminarCaptura }, title: _dictionary.columnELM[$("#language").data("kendoDropDownList").value()], width: "50px",
                attributes: { style: "text-align:center;" },
            },
            { field: "SistemaPintura", title: _dictionary.columnSistemaPintura[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "135px", template: "<div style='text-align:left;'><a href='/Pintura/SistemaPintura?SistemaPinturaID=#=SistemaPintura#&ProyectoID=#=ProyectoID#'><span>#=SistemaPintura#</span></a></div> " },
            { field: "Proyecto", title: _dictionary.columnProyecto[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "135px" },
            { field: "Color", title: _dictionary.columnColor[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "160px" },
            { field: "PruebaShotblast", title: _dictionary.columnPrueba[$("#language").data("kendoDropDownList").value()], filterable: false, width: "120px", template: "<div class='DetalleShotblast' style='text-align:left;'><a href='\\#'  > <span >#=PruebaShotblast#</span></a></div> " },
            { field: "PruebaPorLoteShotblast", title: _dictionary.columnPruebasPorLote[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellNumberMaftec(), width: "130px", attributes: { style: "text-align:right;" }, template: "<span >#=PruebaPorLoteShotblast!=0?PruebaPorLoteShotblast:'N/A' #</span></a></div>" },
            { field: "MetrosPorLoteShotblast", title: _dictionary.columnMetrosPorLote[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellNumberMaftec(), format: "{0:n2}", width: "120px", attributes: { style: "text-align:right;" }, template: "<span >#=MetrosPorLoteShotblast!=0?MetrosPorLoteShotblast:'N/A' #</span></a></div>" },

            { field: "NumeroComponentesShootblast", title: _dictionary.columnNumeroComponentes[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellNumberMaftec(), width: "150px", attributes: { style: "text-align:right;" }, template: "<span >#=NumeroComponentesShootblast!=undefined?NumeroComponentesShootblast:'N/A' #</span></a></div>" },
            { field: "TemplateComponentesShotBlast", title: _dictionary.columnTemplateComponentes[$("#language").data("kendoDropDownList").value()], filterable: false, width: "130px", attributes: { style: "text-align:center;" }, template: "<span >#=TemplateComponentesShotBlast!=undefined?TemplateComponentesShotBlast:'N/A' #</span></a></div>" },
            { field: "TemplateReductoresShotblast", title: _dictionary.columnTemplateReductores[$("#language").data("kendoDropDownList").value()], filterable: false, width: "120px", attributes: { style: "text-align:center;" }, template: "<span >#=TemplateReductoresShotblast!=undefined?TemplateReductoresShotblast:'N/A' #</span></a></div>" },

            { field: "PruebaPrimario", title: _dictionary.columnPrueba1[$("#language").data("kendoDropDownList").value()], filterable: false, width: "120px", template: "<div class='DetallePrimario' style='text-align:left;'><a href='\\#'  > <span>#=PruebaPrimario#</span></a></div> " },
            { field: "PruebaPorLotePrimario", title: _dictionary.columnPruebasPorLote1[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellNumberMaftec(), width: "130px", attributes: { style: "text-align:right;" }, template: "<span >#=PruebaPorLotePrimario!=0?PruebaPorLotePrimario:'N/A' #</span></a></div>" },
            { field: "MetrosPorLotePrimario", title: _dictionary.columnMetrosPorLote1[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellNumberMaftec(), format: "{0:n2}", width: "120px", attributes: { style: "text-align:right;" }, template: "<span >#=MetrosPorLotePrimario!=0?MetrosPorLotePrimario:'N/A' #</span></a></div>" },

            { field: "NumeroComponentesPrimario", title: _dictionary.columnNumeroComponentes[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellNumberMaftec(), width: "120px", attributes: { style: "text-align:right;" }, template: "<span >#=NumeroComponentesShootblast!=undefined?NumeroComponentesShootblast:'N/A' #</span></a></div>" },
            { field: "TemplateComponentesPrimario", title: _dictionary.columnTemplateComponentes[$("#language").data("kendoDropDownList").value()], filterable: false, width: "130px", attributes: { style: "text-align:center;" }, template: "<span >#=TemplateComponentesPrimario!=undefined?TemplateComponentesPrimario:'N/A' #</span></a></div>" },
            { field: "TemplateReductoresPrimario", title: _dictionary.columnTemplateReductores[$("#language").data("kendoDropDownList").value()], filterable: false, width: "120px", attributes: { style: "text-align:center;" }, template: "<span >#=TemplateReductoresPrimario!=undefined?TemplateReductoresPrimario:'N/A' #</span></a></div>" },

            { field: "PruebaIntermedio", title: _dictionary.columnPrueba2[$("#language").data("kendoDropDownList").value()], filterable: false, width: "120px", template: "<div class='DetalleItermedio' style='text-align:left;'><a href='\\#'  > <span>#=PruebaIntermedio#</span></a></div> " },
            { field: "PruebaPorLoteIntermedio", title: _dictionary.columnPruebasPorLote2[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellNumberMaftec(), width: "130px", attributes: { style: "text-align:right;" }, template: "<span >#=PruebaPorLoteIntermedio!=0?PruebaPorLoteIntermedio:'N/A' #</span></a></div>" },
            { field: "MetrosPorLoteIntermedio", title: _dictionary.columnMetrosPorLote2[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellNumberMaftec(), format: "{0:n2}", width: "120px", attributes: { style: "text-align:right;" }, template: "<span >#=MetrosPorLoteIntermedio!=0?MetrosPorLoteIntermedio:'N/A' #</span></a></div>" },

            { field: "NumeroComponentesIntermedio", title: _dictionary.columnNumeroComponentes[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellNumberMaftec(), width: "120px", attributes: { style: "text-align:right;" }, template: "<span >#=NumeroComponentesIntermedio!=undefined?NumeroComponentesIntermedio:'N/A' #</span></a></div>" },
            { field: "TemplateComponentesIntermedio", title: _dictionary.columnTemplateComponentes[$("#language").data("kendoDropDownList").value()], filterable: false, width: "130px", attributes: { style: "text-align:center;" }, template: "<span >#=TemplateComponentesIntermedio!=undefined?TemplateComponentesIntermedio:'N/A' #</span></a></div>" },
            { field: "TemplateReductoresIntermedio", title: _dictionary.columnTemplateReductores[$("#language").data("kendoDropDownList").value()], filterable: false, width: "120px", attributes: { style: "text-align:center;" }, template: "<span >#=TemplateReductoresIntermedio!=undefined?TemplateReductoresIntermedio:'N/A' #</span></a></div>" },

            { field: "PruebaAcabado", title: _dictionary.columnPrueba3[$("#language").data("kendoDropDownList").value()], filterable: false, width: "120px", template: "<div class='DetalleAcabado' style='text-align:left;'><a href='\\#'  > <span>#=PruebaAcabado#</span></a></div> " },
            { field: "PruebaPorLoteAcabado", title: _dictionary.columnPruebasPorLote3[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellNumberMaftec(), width: "130px", attributes: { style: "text-align:right;" }, template: "<span >#=PruebaPorLoteAcabado!=0?PruebaPorLoteAcabado:'N/A' #</span></a></div>" },
            { field: "MetrosPorLoteAcabado", title: _dictionary.columnMetrosPorLote3[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellNumberMaftec(), format: "{0:n2}", width: "120px", attributes: { style: "text-align:right;" }, template: "<span >#=MetrosPorLoteAcabado!=0?MetrosPorLoteAcabado:'N/A' #</span></a></div>" },

            { field: "NumeroComponentesAcabado", title: _dictionary.columnNumeroComponentes[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellNumberMaftec(), width: "120px", attributes: { style: "text-align:right;" }, template: "<span >#=NumeroComponentesAcabado!=undefined?NumeroComponentesAcabado:'N/A' #</span></a></div>" },
            { field: "TemplateComponentesAcabado", title: _dictionary.columnTemplateComponentes[$("#language").data("kendoDropDownList").value()], filterable: false, width: "130px", attributes: { style: "text-align:center;" }, template: "<span >#=TemplateComponentesAcabado!=undefined?TemplateComponentesAcabado:'N/A' #</span></a></div>" },
            { field: "TemplateReductoresAcabado", title: _dictionary.columnTemplateReductores[$("#language").data("kendoDropDownList").value()], filterable: false, width: "120px", attributes: { style: "text-align:center;" }, template: "<span >#=TemplateReductoresAcabado!=undefined?TemplateReductoresAcabado:'N/A' #</span></a></div>" },
        ],
        editable: true,
        navigatable: true
    });

    CustomisaGrid($("#grid"));
}

function eliminarCaptura(e) {
    e.preventDefault();
    var dataItem = $("#grid").data("kendoGrid").dataItem($(e.currentTarget).closest("tr"));

    ventanaConfirm = $("#ventanaConfirm").kendoWindow({
        iframe: true,
        title: _dictionary.PinturaCargaTitulo[$("#language").data("kendoDropDownList").value()],
        visible: false, //the window will not appear before its .open method is called
        width: "23%",
        height: "auto",
        draggable: false,
        modal: true,
        animation: {
            close: false,
            open: false
        },
        actions: []
       
    }).data("kendoWindow");

    ventanaConfirm.content('<center>' + _dictionary.SistemaPinturaMensajeConfirmaEliminar[$("#language").data("kendoDropDownList").value()] + '</center>' +
                 "</br><center><button class='confirm_yes btn btn-blue' id='yesButton'>Si</button>  <button class='confirm_yes btn btn-blue' id='noButton'> No</button></center>");

    ventanaConfirm.open().center();

    $("#yesButton").click(function () {
        if (!dataItem.AsignadoSpool) {
            AjaxEliminaSistemaPintura(dataItem.SistemaPinturaID,dataItem.ProyectoID);

        } else {
            displayNotify("SistemaPinturaErrorEliminado", "", '2');
        }
        ventanaConfirm.close();
    });
    $("#noButton").click(function () {
        ventanaConfirm.close();
    });
}
function editaSistemaPintura(dataItem) {

    if ($('#Guardar').text() == _dictionary.lblGuardar[$("#language").data("kendoDropDownList").value()]) {
        var SistemaPinturaID = dataItem.SistemaPinturaID;
        var url = '/Pintura/SistemaPintura?SistemaPinturaID=' + SistemaPinturaID
        window.open(url, '_blank');
    }
}

function CargarGridDetalle() {
    $("#gridPopUp").kendoGrid({
        autoBind: true,
        dataSource: {
            data: [],
            schema: {
                model: {
                    fields: {
                        Accion: { type: "number", editable: false },
                        Prueba: { type: "string", editable: false },
                        UnidadMedida: { type: "string", editable: false },
                        UnidadMin: { type: "double", editable: false },
                        UnidadMax: { type: "double", editable: false },
                    }
                }
            }, filter: {
                logic: "or",
                filters: [
                  { field: "Accion", operator: "eq", value: 1 }
                ]
            },
        },
        navigatable: true,
        filterable: {
            extra: false
        },
        click: function (e) {
        },
        editable: true,
        autoHeight: true,
        sortable: true,
        scrollable: true,
        columns: [
                { field: "Prueba", title: _dictionary.lblPrueba[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "20px" },
                { field: "UnidadMedida", title: "U. Medida", filterable: getGridFilterableCellMaftec(), width: "20px" },
                { field: "UnidadMin", title: _dictionary.columnUnidadMinima[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellNumberMaftec(), format: "{0:n2}", width: "20px", attributes: { style: "text-align:right;" } },
                { field: "UnidadMax", title: _dictionary.columnUnidadMaxima[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellNumberMaftec(), format: "{0:n2}", width: "20px", attributes: { style: "text-align:right;" } }
        ]

    });
    CustomisaGrid($("#gridPopUp"));
}

