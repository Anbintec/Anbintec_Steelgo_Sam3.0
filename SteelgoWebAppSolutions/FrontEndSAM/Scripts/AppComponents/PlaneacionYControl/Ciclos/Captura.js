function changeLanguageCall() {
    SuscribirEventos();
    AjaxObtenerProyectos();
    AjaxObtnerEtapas();
    AjaxObtenerFamiliaMateriales();
};

function CargarGrid() {
    $("#grid").kendoGrid({
        edit: function (e) {
            //    this.closeCell();
        },
        autoBind: true,
        //dataSource: {
        //    data: [],
        //    schema: {
        //        model: {
        //            modelo
        //        }
        //    },
        //    pageSize: 20,
        //    serverPaging: false,
        //    serverFiltering: false,
        //    serverSorting: false
        //},

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

        colums: [

            columnas
         // { field: "Etapa", title: "Etapas", filterable: true, width: "10px" },
        ],

    });
    //OcultarTodasColumnas();
    CustomisaGrid($("#grid"));

    GridColumns();
}

function OcultarColumnas(columnasVisibles) {
    var grid = $("#grid").data("kendoGrid");
    var columnas = grid.columns;

    for (var i = 1; i < columnas.length; i++) {
        for (var j = 0; j < columnasVisibles.length; j++) {
            if (i == columnasVisibles[j]) {
                grid.hideColumn(i);
                break;
            }
        }
    }
}

function OcultarTodasColumnas() {
    var grid = $("#grid").data("kendoGrid");
    var columnas = grid.columns;

    for (var i = 1; i < columnas.length; i++) {
        grid.hideColumn(i);
    }
}

function MostrarColumnas(columnasVisibles) {
    var grid = $("#grid").data("kendoGrid");
    var columnas = grid.columns;

    for (var i = 1; i < columnas.length; i++) {
        for (var j = 0; j < columnasVisibles.length; j++) {
            if (i == columnasVisibles[j]) {
                grid.showColumn(i);
                break;
            }
        }
    }

}

function MostrarColumnasPWHT(columnasVisibles) {
    var grid = $("#grid").data("kendoGrid");
    var columnas = grid.columns;

    for (var i = 1; i < columnas.length; i++) {
        for (var j = 0; j < columnasVisibles.length; j++) {
            var columna = columnasVisibles[j];

            if (i == columnasVisibles[j]) {
                columna++
                grid.showColumn(columna);
                break;
            }
        }
    }

}


