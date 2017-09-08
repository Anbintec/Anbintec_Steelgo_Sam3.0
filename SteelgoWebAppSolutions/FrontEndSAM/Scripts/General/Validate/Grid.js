function getGridFilterableMaftec(val) {
    return {
        mode: "menu, row",
        extra: false,
        operators: {
            string: {
                startswith: _dictionary.KendoGridFilterable0001[$("#language").data("kendoDropDownList").value()],
                eq: _dictionary.KendoGridFilterable0002[$("#language").data("kendoDropDownList").value()],
                neq: _dictionary.KendoGridFilterable0003[$("#language").data("kendoDropDownList").value()],
            },
        },
        cell: {
            showOperators: false,
            operator: "contains"
        }
    }
}

function getGridFilterableCellMaftec() {
    return {
        cell: {
            operator: "contains",
            template: function (args) {
                args.css("width", "95%").addClass("general-input").keydown(function (e) {
                    setTimeout(function () {
                        $(e.target).trigger("change");
                    });
                });
            },
            showOperators: false
        }
    }
}

function getGridFilterableCellNumberMaftec() {
    return {
        extra: true, 
        cell: {
            operator: "equals",
            template: function (args) {
                $(args).prop('type', 'number');
                args.css("width", "95%").addClass("general-input").keydown(function (e) {
                    setTimeout(function () {
                        $(e.target).trigger("change");
                    });
                });
            },
            showOperators: false
        }
    }
}
function getGridFilterableCellNumberMaftecPopup() {
    return {
        extra: true,
        cell: {
            operator: "equals",
            template: function (args) {
                $(args).prop('type', 'number');
                args.css("width", "95%").addClass("inputtext-popup").keydown(function (e) {
                    setTimeout(function () {
                        $(e.target).trigger("change");
                    });
                });
            },
            showOperators: false
        }
    }
}

function getGridFilterableCellMaftecpopUp() {
    return {
        cell: {
            operator: "contains",
            template: function (args) {
                args.css("width", "95%").addClass("general-input").keydown(function (e) {
                    setTimeout(function () {
                        $(e.target).trigger("change");
                    });
                });
            },
            showOperators: false
        }
    }
}

function getKendoGridFilterableDateMaftec() {
    return {
        cell: {
            template: function (args) {
                args.kendoDatePicker({
                    format: "dd/MM/yyyy"
                });
            },
            showOperators: false
        }
    }
}

function RowEmpty($grid) {
    $("tr", $grid).each(function (index) {
        var $row = $(this);
        $row.css("background-color", "");
        $("td", $(this)).each(function (index) {
            if ($(this).text() == "") {
                $row.css("background-color", "#ffcccc");
            }
        });
    });
}

function ExistRowEmpty(rows) {
    for (var i = 0; i < rows.length; i++) {
        if (rows[i].Estatus == 0) {
            $("#grid").data("kendoGrid").dataSource._data[i].RowOk = false;
            return true;
        }
    }
    
    return false;
}

function ExistRowErrors(rows) {
    for (var i = 0; i < rows.length; i++) {
        if (rows[i].RowOk == false) {
            return true;
        }
    }
    return false;
}

function ExistRowEmptyAble(rows) {
    for (var i = 0; i < rows.length; i++) {
        if (rows[i].Estatus == 0 && (rows[i].Accion != 3)) {
            $("#grid").data("kendoGrid").dataSource._data[i].RowOk = false;
            return true;
        }
    }

    return false;
}

function ExistRowWithErrors(rows) {
    for (var i = 0; i < rows.length; i++) {
        if (rows[i].Estatus == 0) {
            return true;
        }
    }

    return false;
}

function ContarElementosAsignados(elementoID, array, rowitem) {

    var numeroVecesAsignado = 0;
    for (var i = 0; i < array.length; i++) {
        if (array[i].IdOrdenTrabajo + '-' + array[i].IdVal == (rowitem.IdOrdenTrabajo + '-' + rowitem.IdVal)) {
            if (array[i].NumeroUnico1ID == elementoID || array[i].NumeroUnico2ID == elementoID)
                numeroVecesAsignado++;
        }
    }
    return numeroVecesAsignado;
}

function ContarElementosConMismaLocalizacion(array, localizacion, rowitem) {
    var numeroVecesMismaLocalizacion = 0;
    for (var i = 0; i < array.length; i++) {
        if (array[i].IdOrdenTrabajo + '-' + array[i].IdVal == (rowitem.IdOrdenTrabajo + '-' + rowitem.IdVal)) {
            if (array[i].Localizacion.split("-")[0] == localizacion || array[i].Localizacion.split("-")[1] == localizacion)
                numeroVecesMismaLocalizacion++;
        }
    }
    return numeroVecesMismaLocalizacion;
}