var columns = [];
var model = {};
var fields = {};
var etapasCiclos;

function AjaxObtenerProyectos() {
    $Ciclos.Ciclos.read({ token: Cookies.get("token"), idCatalogo: 1 }).done(function (data) {
        $("#inputProyecto").data("kendoComboBox").value("");

        if (data.length > 0) {
            $("#inputProyecto").data("kendoComboBox").dataSource.data(data);
        }

        loadingStop();
    });
}

function AjaxObtnerEtapas() {
    $Ciclos.Ciclos.read({ token: Cookies.get("token") }).done(function (data) {
        etapasCiclos = data;
        GenerarModelo(data[0], model)
        GenerarColumnas(data[0], columns)
        loadingStop();
    });
}

function AjaxObtenerFamiliaMateriales() {
    $Ciclos.Ciclos.read({ token: Cookies.get("token"), idFamiliaMaterial: 0 }).done(function (data) {

        $("#grid").kendoGrid({
            dataSource: {
                data: etapasCiclos,
                schema: {
                    model: GenerarModelo(data[0], model),
                }
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

            columns: GenerarColumnas(data[0], columns),
        });
        loadingStop();
    });
}

function AjaxObtenerFamiliaAceros() {
    $Ciclos.Ciclos.read({ token: Cookies.get("token"), idFamiliaAcero: 0 }).done(function (data) {
        alert(JSON.stringify(data));
        loadingStop();
    });

}

//gernerar modelo schema para kendo grid
function GenerarModelo(gridData, model) {
    //modelo
    // model.id = "ID";
    for (var property in gridData) {
        var st = property;
        var replaced = st.split('+').join('_');
        var replacedMaybe = replaced.split('-').join('_');
        var replaceAlmost = replacedMaybe.split('.').join('_');
        var replacedEnd = replaceAlmost.split(' ').join('_');

        property = replacedEnd;

        var propType = typeof gridData[property];

        if (propType == "number") {
            fields[property] = {
                type: "number",
                editable: true,
                validation: {
                    required: false
                }
            };
        } else if (propType == "boolean") {
            fields[property] = {
                type: "boolean",
                editable: true,
                validation: {
                    required: false
                }
            };
        } else if (propType == "string") {
            var parsedDate = kendo.parseDate(gridData[property]);
            if (parsedDate) {
                fields[property] = {
                    type: "date",
                    editable: true,
                    validation: {
                        required: false
                    }
                };
                dateFields.push(property);
            } else {
                fields[property] = {
                    validation: {
                        required: false
                    }
                };
            }
        } else {
            fields[property] = {
                validation: {
                    required: false
                }
            };
        }

    }
    model.fields = fields;
    //alert(JSON.stringify(model.fields));
    return model;
}

//generar coumnas para kendo grid
function GenerarColumnas(gridData, columns) {
    for (var property in gridData) {
        var st = property;
        var replaced = st.split('+').join('_');
        var replacedMaybe = replaced.split('-').join('_');
        var replaceAlmost = replacedMaybe.split('.').join('_');
        var replacedEnd = replaceAlmost.split(' ').join('_');

        if (property == "EtapaId") {
            columns.push({
                field: replacedEnd, title: property, filterable: true, hidden: true, width: "100px"
            });
        }
        else {
            columns.push({
                field: replacedEnd, title: property, filterable: true, width: "135px"
            });
        }
    }
    return columns;
}

//obtener ciclos capturados
function GuardarCliclos(tipoGuardado) {

    var ciclos = [];
    ciclos = {
        ProyectoID: "",
        EtapaID: "",
        TipoFamilia: "",
        PWHT: "",
    };

    var entityGrid = $("#grid").data("kendoGrid");
    var columns = $("#grid").data("kendoGrid").columns;
    var data = entityGrid.dataSource.data();
    var totalNumber = data.length;
    var currentDataItem;
    var dts = [];
    var cols = [];

    for (var i = 0; i < totalNumber; i++) {
        currentDataItem = data[i];
        dts.push(JSON.stringify(currentDataItem));

        //for (var p in data[i]) {
        //    if (data[i][p] != undefined) {
        //        console.log(JSON.stringify(p));
        //    }
        //}
        //console.log(JSON.stringify(currentDataItem) + " Length grid: " + totalNumber);

    }


    var ts = []
    for (var x = 0; x < dts.length; i++) {
        ts = dts[x].split(',');
    }
    console.log(ts);


    //Agregar error en fila
    //var $grid = $("#grid");
    //$("tr", $grid).each(function (indexTR, tr) {
    //    var $row = $(tr);
    //    $("td", $row).each(function (indexTD, td) {
    //        var valor = $(td).text();
    //        if (valor != "" && valor != undefined) {
    //           //var th = $(td).closest('tbody').prev('thead').find('> tr > th a:eq(' + indexTD + ')');
    //           // var th = $(this).closest('table').find('th > a').html();
    //           // alert(th);
    //            dts.push(valor);
    //        }
    //        else {

    //        }


    //    });
    //});
}





