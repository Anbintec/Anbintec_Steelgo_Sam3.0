function RenderGridNivelDos(e) {

    var detailRow = e.detailRow;

    var model = e.data;
    kendo.bind(detailRow, model);
    //model.bind('change', function (e) {
    //    var tr = $('tr[data-uid=' + model.uid + ']');
    //    $('#grid').data().kendoGrid.expandRow(tr);
    //})

    detailRow.find(".nivel2").kendoGrid({
        edit: function (e) {
            this.closeCell();
        },
        autoBind: true,
        dataSource: {
            data: model.ListaSpools,
            schema: {
                model: {
                    fields: {
                        TipoID: { type: "number", editable: false },
                        SpoolID: { type: "number", editable: false },
                        Seleccionado: { type: "number", editable: false },
                        Proyectado: { type: "number", editable: false },
                        SpoolNombre: { type: "string", editable: false },
                        Dibujo: { type: "string", editable: false },
                        DiametroMaximo: { type: "number", editable: false },
                        DiametroPromedio: { type: "number", editable: false },
                        Peso: { type: "number", editable: false },
                        Area: { type: "number", editable: false }
                    }
                }
            },
            pageSize: 20,
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
            pageSizes: [10, 15, 20],
            info: false,
            input: false,
            numeric: true,
        },
        detailTemplate: kendo.template($("#templateGridNivelTres").html()),
        detailInit: RenderGridNivelTres,
        columns: [
            { field: "Seleccionado", title: " ", filterable: false, template: '<input type="checkbox" #= Proyectado ? "disabled=disabled" : "" # class="chkbx"  ></input>', width: "50px" },
            { command: { text: _dictionary.botonCancelar[$("#language").data("kendoDropDownList").value()], click: eliminarSpoolDeProyeccion }, title: "", width: "99px" },
            { field: "Proyeccion", title: "Proyección", filterable: false },
            { field: "SpoolNombre", title: "Spool", filterable: true },
            { field: "Dibujo", title: "Dibujo", filterable: true },
            { field: "DiametroMaximo", title: "Diametro Máximo", filterable: true },
            { field: "DiametroPromedio", title: "Diametro Promedio", filterable: true },
            { field: "Peso", title: "Kgs", filterable: true, width: "100px" },
            { field: "Area", title: "M2", filterable: true, width: "100px" },
            { field: "Juntas", title: "Juntas", filterable: true, width: "110px" },
            { field: "Peqs", title: "Peqs", filterable: true, width: "100px" },

        ]
    });

    dataBound(e);

    $("td[role='gridcell']").on("change", ":checkbox", function (e) {
        var grid = $(e.target).closest(".nivel2").data("kendoGrid"),
        dataItem = grid.dataItem($(e.target).closest("tr"));

        if ($(this)[0].checked) {
            dataItem.Seleccionado = true;
        }
        else {
            dataItem.Seleccionado = false;

        }

    });
}

function search(nameKey, myArray) {
    for (var i = 0; i < myArray.length; i++) {
        if (myArray[i].name === nameKey) {
            return myArray[i];
        }
    }
}

function dataBound(e) {

    var grid = $(".nivel2").data("kendoGrid");
    var gridData = grid.dataSource.view();
    for (var i = 0; i < gridData.length; i++) {
        var currentUid = gridData[i].uid;

        if (gridData[i].Proyectado != 1) {
            var currenRow = grid.table.find("tr[data-uid='" + currentUid + "']");
            var editButton = $(currenRow).find(".k-grid-Cancelar");

            editButton.hide();
        }
    }
}

function eliminarSpoolDeProyeccion(e) {
    e.preventDefault();

    var filterValue = $(e.currentTarget).val();
    var dataItem = $(".nivel2").data("kendoGrid").dataItem($(e.currentTarget).closest("tr"));

    windowTemplate = kendo.template($("#windowTemplate").html());

    ventanaConfirm = $("#ventanaConfirm").kendoWindow({
        iframe: true,
        title: _dictionary.WarningTitle[$("#language").data("kendoDropDownList").value()],
        visible: false, //the window will not appear before its .open method is called
        width: "auto",
        height: "auto",
        modal: true
    }).data("kendoWindow");

    ventanaConfirm.content(_dictionary.CapturaAvanceIntAcabadoPreguntaBorradoCaptura[$("#language").data("kendoDropDownList").value()] +
                "</br><center><button class='btn btn-blue' id='yesButton'>Si</button><button class='btn btn-blue' id='noButton'> No</button></center>");

    ventanaConfirm.open().center();

    $("#yesButton").click(function () {
        var dataSource = $(".nivel2").data("kendoGrid").dataSource;
        for (var i = 0; i < Talleres.length; i++) {
            for (var j = 0; j < Talleres[i].taller[0].Proyecciones.length ; j++) {
                if (Talleres[i].taller[0].Proyecciones[j].Nombre == dataItem.Proyeccion) {
                    var totalAutomatico = 0;
                    var totalManual = 0;

                    for (var k = 0; k < dataItem.ListaJuntas.length; k++) {
                        if (dataItem.ListaJuntas[k].FabclasID == k) {
                            totalAutomatico += dataItem.ListaJuntas[k].Peqs;
                        }
                        else if (dataItem.ListaJuntas[k].FabclasID == 2) {
                            totalManual += dataItem.ListaJuntas[k].Peqs;
                        }
                    }

                    Talleres[i].taller[0].Proyecciones[j].Automatico -= (totalAutomatico * 0.8);
                    Talleres[i].taller[0].Proyecciones[j].Automan -= (totalAutomatico * 0.2);
                    Talleres[i].taller[0].Proyecciones[j].NumeroSpools -= totalManual;

                    for (var m = 0; m < Talleres[i].taller[0].Proyecciones[j].SpoolDetalle.length; m++) {
                        if (Talleres[i].taller[0].Proyecciones[j].SpoolDetalle[m].SpoolID == dataItem.SpoolID) {
                            Talleres[i].taller[0].Proyecciones[j].SpoolDetalle.splice(m, 1);
                        }
                    }

                    dataItem.Proyectado = 0;
                    dataItem.Seleccionado = 0;
                    dataItem.Proyeccion = "";
                    $("#grid").data("kendoGrid").dataSource.sync();
                    EditarEliminarSpoolDeContenedorProyecciones(dataItem);

                    break;
                }
            }
        }
        dataItem.Proyectado = 0;
        dataItem.Proyeccion = "";

        dataSource.sync();
        ventanaConfirm.close();
    });
    $("#noButton").click(function () {
        ventanaConfirm.close();
    });

}

function RenderGridNivelTres(e) {
    var detailRow = e.detailRow;

    var model = e.data;
    kendo.bind(detailRow, model);
    //model.bind('change', function (e) {
    //    var tr = $('tr[data-uid=' + model.uid + ']');
    //    $('#grid').data().kendoGrid.expandRow(tr);
    //})

    detailRow.find(".nivel3").kendoGrid({
        edit: function (e) {
            this.closeCell();
        },
        autoBind: true,
        dataSource: {
            data: model.ListaJuntas,
            schema: {
                model: {
                    fields: {
                        Junta: { type: "number", editable: false },
                        Fabclas: { type: "string", editable: false },
                        TipoJunta: { type: "string", editable: false },
                        Peqs: { type: "number", editable: false }
                    }
                }
            },
            pageSize: 20,
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
            pageSizes: [10, 15, 20],
            info: false,
            input: false,
            numeric: true,
        },
        columns: [
            { field: "Junta", title: "Tipo", filterable: false },
            { field: "Fabclas", title: "Fabclas", filterable: false },
            { field: "TipoJunta", title: "Tipo Junta", filterable: false },
            { field: "Peqs", title: "Peqs", filterable: false, width: "100px" }
        ]
    });

    //Nivel 3
    $(".nivel3").data('kendoGrid').dataSource.data([]);
    var dsNivel3 = $(".nivel3").data("kendoGrid").dataSource;
    for (var i = 0; i < model.ListaJuntas.length; i++) {
        dsNivel3.add(model.ListaJuntas[i]);
    }
}