function changeLanguageCall() {
    CargarGrid();
    $("#Area").data("kendoComboBox").value("");
    $("#Cuadrante").data("kendoComboBox").value("");
    //AjaxCargarArea();
    document.title = "Consulta";
};


function CargarGrid() {
    $("#grid").kendoGrid({
        autoBind: true,
        autoSync: true,

        dataSource: {
            data: [],
            schema: {
                model: {
                    fields: {
                        SpoolID: { type: "string", editable: false },
                        Paso: { type: "string", editable: false },
                        Cuadrante: { type: "string", editable: false },
                        Dibujo: { type: "String", editable: false },
                        Detalle: { type: "string", editable: false }
                    }
                }
            },
            filter: {
                logic: "or",
                filters: []
            },
            pageSize: 10,
            serverPaging: false,
            serverFiltering: false,
            serverSorting: false
        },
        pageable: {
            refresh: false,
            pageSizes: [10, 25, 50, 20],
            info: false,
            input: false,
            numeric: true,
           // buttonCount: 2
        },
        navigatable: true,
        editable: true,
        autoHeight: true,
        sortable: true,
        scrollable: true,
        selectable: true,
        filterable: getGridFilterableMaftec(),
        columns: [
            { field: "SpoolID", title: _dictionary.columnSpoolIDEmbarque[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "160px" },
            { field: "Paso", title: _dictionary.columnPasoEmbarque[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "160px" },
            { field: "Cuadrante", title: _dictionary.columnCuadranteEmbarque[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "170px" },
            { field: "Dibujo", title: _dictionary.columnDibujoEmbarque[$("#language").data("kendoDropDownList").value()], filterable: false, template: "<a>" + "Ver" + "</a>", width: "270px" },
            //{ command: { text: _dictionary.EmbarqueConsultaTraveler[$("#language").data("kendoDropDownList").value()]/*, click: eliminarCaptura*/ }, template: "<a>" + _dictionary.EmbarqueConsultaVer[$("#language").data("kendoDropDownList").value()] + "</a>", width: "150px" },
            { field: "Detalle", title: "Detalle",filterable: false, template: "<a>" + "link shop" + "</a>", width: "270px" }
        ],
        //dataBound: function (e) {
        //    quickHeadFilter2($("#grid").data("kendoGrid"));
        //},
    });
    CustomisaGrid($("#grid"));
};


function quickHeadFilter2(g) {
    var itera = 0;
    var id = "#" + g.wrapper[0].id;
    var gr = '$("' + id + '").data("kendoGrid")';
    if ($(id + " .filter-row").length === 0) {
        g.thead.append(function () {
            var init = "<tr class='filter-row' tabindex='0'>"
            $(id + " thead th:visible").each(function () {
                if (itera < 3) {
                    if (this.hasAttribute("data-field") && $(this).attr("data-field") !== "") {
                        if (modelType(g, $(this).attr("data-field")) === "number") {
                            init += "<th tabindex='0'><input class='k-textbox' data-filter='" + $(this).attr("data-field") + "' type='number' onkeyup='quickFilter(" + gr + ",this,event)'/></th>";
                        } else {
                            init += "<th tabindex='0'><input class='k-textbox' data-filter='" + $(this).attr("data-field") + "' type='text' onkeyup='quickFilter(" + gr + ",this,event)'/></th>";
                        }
                    } else {
                        init += "<th></th>";
                    }
                }
                else
                    init += "<th></th>";
                itera++;
            })
            return init += "</tr>"
        })
    }
}