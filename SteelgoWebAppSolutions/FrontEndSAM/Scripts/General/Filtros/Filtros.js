
function FiltroMostrar(mostrar) {
    var ds = $("#grid").data("kendoGrid").dataSource;

    if (mostrar == 0) {
        var curr_filters = ds.filter().filters;
        if (curr_filters[0].filters != undefined)
            ds.filter(curr_filters[0].filters[0])
        else
            ds.filter(curr_filters[0])
        $("#grid").data("kendoGrid").refresh();
    }
    else {

        var curr_filters = ds.filter().filters;
       
        var filters = ds.filter();
        filters.logic = "or"
        filters.filters.push(curr_filters[0]);
        filters.filters.push({ field: "Accion", operator: "eq", value: 2 });
        filters.filters.push({ field: "Accion", operator: "eq", value: 4 });
        $("#grid").data("kendoGrid").refresh();
    }
}