IniciarDashboardPintura();
function IniciarDashboardPintura() {
    SuscribirEventos();
    ConvertirInputs();
}

function changeLanguageCall() {
    //AjaxCargarHeaderDashboard();
    //CargarGrid();
    //CargarGridPopUp();
    //document.title = _dictionary.PinturaDashboardBreadcrumb[$("#language").data("kendoDropDownList").value()];

}

function ConvertirInputs() {
    $("#inputCantidadPeriodo").kendoNumericTextBox({
        format: "#",
        decimals: 0,
        min: 0
    });

    $("#inputTipoPeriodo").kendoComboBox({
        dataSource: {
            data: ["", "Dias", "Meses", "Años"]
        }
    });
}

function CargarGrid() {
    $("#grid").kendoGrid({
        edit: function (e) {

                var inputName = e.container.find('input');
                inputName.select();

            if ($('#Guardar').text() == _dictionary.MensajeGuardar[$("#language").data("kendoDropDownList").value()]) {
            } else
                this.closeCell();
        },
        dataBound: function () {
            var myElem = document.getElementById('trParentHeader');
            if (myElem == null) {
                $("#grid").find("th.k-header").parent().before("<tr id='trParentHeader'> " +
                    "<th width='auto'  colspan='3' class='k-header' style='text-align: center;'><span> SHOTBLAST </span></th>" +
                    "<th width='auto'  colspan='3' class='k-header' style='text-align: center;'><span id=''> PRIMARIO </span></th>" +
                    "<th width='auto'  colspan='3' class='k-header' style='text-align: center;'><span> INTERMEDIO </span></th>" +
                    "<th width='auto'  colspan='3' class='k-header' style='text-align: center;'><span id=''> ACABADO </span></th>" +
                    "<th width='auto'  colspan='3' class='k-header' style='text-align: center;'><span id=''> OK PINTURA </span></th>" +
                    "</tr>"+
                    "<tr>" +
                    "<th width='auto'  colspan='1' class='k-header' style='text-align: center;'><span> (0) </span></th>" +
                    "<th width='auto'  colspan='1' class='k-header' style='text-align: center;'><span> (0) </span></th>" +
                    "<th width='auto'  colspan='1' class='k-header' style='text-align: center;'><span> (0) </span></th>" +
                    "<th width='auto'  colspan='1' class='k-header' style='text-align: center;'><span> (0) </span></th>" +
                    "<th width='auto'  colspan='1' class='k-header' style='text-align: center;'><span> (0) </span></th>" +
                    "<th width='auto'  colspan='1' class='k-header' style='text-align: center;'><span> (0) </span></th>" +
                    "<th width='auto'  colspan='1' class='k-header' style='text-align: center;'><span> (0) </span></th>" +
                    "<th width='auto'  colspan='1' class='k-header' style='text-align: center;'><span> (0) </span></th>" +
                    "<th width='auto'  colspan='1' class='k-header' style='text-align: center;'><span> (0) </span></th>" +
                    "<th width='auto'  colspan='1' class='k-header' style='text-align: center;'><span> (0) </span></th>" +
                    "<th width='auto'  colspan='1' class='k-header' style='text-align: center;'><span> (0) </span></th>" +
                    "<th width='auto'  colspan='1' class='k-header' style='text-align: center;'><span> (0) </span></th>" +
                    "<th width='auto'  colspan='1' class='k-header' style='text-align: center;'><span> (0) </span></th>" +
                    "<th width='auto'  colspan='1' class='k-header' style='text-align: center;'><span> (0) </span></th>" +
                    "</tr>");
            }
        },
        dataSource: {
            data: [],
            schema: {
                model: {
                    fields: {
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
             { width: "140px", field: "PorCargarShotblast", title: "POR CARGAR", filterable: getGridFilterableCellNumberMaftec()},
             { width: "160px", field: "PorCapturarShotblast", title: "POR CAPTURAR", filterable: getGridFilterableCellNumberMaftec() },
             { width: "140px", field: "PorProbarShotblast", title: "POR PROBAR", filterable: getGridFilterableCellNumberMaftec() },
             { width: "140px", field: "PorCargarPrimario", title: "POR CARGAR", filterable: getGridFilterableCellNumberMaftec() },
             { width: "160px", field: "PorCapturarPrimario", title: "POR CAPTURAR", filterable: getGridFilterableCellNumberMaftec() },
             { width: "140px", field: "PorProbarPrimario", title: "POR PROBAR", filterable: getGridFilterableCellNumberMaftec() },
             { width: "140px", field: "PorCargarIntermedio", title: "POR CARGAR", filterable: getGridFilterableCellNumberMaftec() },
             { width: "160px", field: "PorCapturarIntermedio", title: "POR CAPTURAR", filterable: getGridFilterableCellNumberMaftec() },
             { width: "140px", field: "PorProbarIntermedio", title: "POR PROBAR", filterable: getGridFilterableCellNumberMaftec() },
             { width: "140px", field: "PorCargarAcabado", title: "POR CARGAR", filterable: getGridFilterableCellNumberMaftec() },
             { width: "160px", field: "PorCapturarAcabado", title: "POR CAPTURAR", filterable: getGridFilterableCellNumberMaftec() },
             { width: "140px", field: "PorProbarAcabado", title: "POR PROBAR", filterable: getGridFilterableCellNumberMaftec() },
             { width: "200px", field: "", title: "", filterable: getGridFilterableCellNumberMaftec() },
             //{ width: "200px", field: "Especificacion", title: _dictionary.columnCodigo[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec()},
             //{ command: { text: _dictionary.botonCancelar[$("#language").data("kendoDropDownList").value()], click: cancelarCaptura }, width: "50px", title: _dictionary.columnELM[$("#language").data("kendoDropDownList").value()] }
        ]
    });
    CustomisaGrid($("#grid"));
};