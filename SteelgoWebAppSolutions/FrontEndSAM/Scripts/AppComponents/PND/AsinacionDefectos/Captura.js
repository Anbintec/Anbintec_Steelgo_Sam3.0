var modeloRenglon;

function changeLanguageCall() {
    CargarGrid();
    CargarGridPopUpDetallePartida();
    inicio();
};


function inicio() {
    SuscribirEventos();
    AjaxObtenerProyectos();
}



function CargarGrid() {

    $("#grid").kendoGrid({
        dataSource: {
            data: [],
            schema: {
                model: {
                    fields: {

                        SpoolID: { type: "string", editable: false },
                        Junta: { type: "string", editable: false },
                        Descripcion: { type: "string", editable: false },
                        Diametro: { type: "number", editable: false },
                        JtaSeg1: { type: "string", editable: true },
                        JtaSeg2: { type: "string", editable: true },
                    }
                }
            },
        },
        edit: function (e) {
            setTimeout(function () {
                var inputName = e.container.find('input');

                inputName.select();
            });
            if ($('#Guardar').text() == _dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]) {
                this.closeCell();
            };
        },
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
            { field: "SpoolID", title: "Spool ID", filterable: getGridFilterableCellMaftec(), width: "100px" },
            { field: "Junta", title: "Jta", filterable: getGridFilterableCellMaftec(), width: "100px" },
            { field: "Descripcion", title: _dictionary.columnDetalleJunta[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "200px" },
            { field: "Diametro", title: "Diam", filterable: getGridFilterableCellNumberMaftec(), width: "70px" },
            { field: "JtaSeg1", title: "Jta seg 1", filterable: getGridFilterableCellMaftec(), width: "100px", editor: RenderComboBoxJunta },
            { field: "JtaSeg2", title: "Jta seg 2", filterable: getGridFilterableCellMaftec(), width: "100px", editor: RenderComboBoxJunta2 },
            { command: { text: _dictionary.botonLimpiar[$("#language").data("kendoDropDownList").value()], click: limpiarRenglon }, filterable: false, title: _dictionary.columnLimpiar[$("#language").data("kendoDropDownList").value()], width: "50px", attributes: { style: "text-align:center;" } }
            //{ field: "TemplateDetalleElemento", title: "Detalle", filterable: false, width: "105px", template: "<div class='EnlacePorPlaca' style='text-align:center;'><a href='\\#'  > <span>Detalle</span></a></div> " },
           ],

            editable: true,
            navigatable: true
        });
        CustomisaGrid($("#grid")); 5
};

function CargarGridPopUpDetallePartida() {

    kendo.ui.Grid.fn.editCell = (function (editCell) {
        return function (cell) {
            cell = $(cell);

            var that = this,
                column = that.columns[that.cellIndex(cell)],
                model = that._modelForContainer(cell),
                event = {
                    container: cell,
                    model: model,
                    preventDefault: function () {
                        this.isDefaultPrevented = true;
                    }
                };

            if (model && typeof this.options.beforeEdit === "function") {
                this.options.beforeEdit.call(this, event);
                if (event.isDefaultPrevented) return;
            }

            editCell.call(this, cell);
        };
    })(kendo.ui.Grid.fn.editCell);

    $("#gridPopUp").kendoGrid({
        ////autoBind: true,
        dataSource: {
            data: [{
                Ubicacion: "0-1",
                Defecto: "Falta Fusión",
                InicioMM: 1,
                FinMM: 3,
                Soldador: "",
                JtaSeg1: "",
                JtaSeg2: "",
                listaJuntas: [],
                EsRepetido: false
            },
            {
                Ubicacion: "1-2",
                Defecto: "Poros",
                InicioMM: 10,
                FinMM: 18,
                Soldador: "",
                JtaSeg1: "",
                JtaSeg2: "",
                listaJuntas: [],
                EsRepetido: false
            },
            {
                Ubicacion: "4-5",
                Defecto: "Poros",
                InicioMM: 5,
                FinMM: 8,
                Soldador: "",
                JtaSeg1: "",
                JtaSeg2: "",
                listaJuntas: [],
                EsRepetido: false
            }
            ],
            schema: {
                model: {
                    fields: {
                        Ubicacion: { type: "string", editable: false },
                        Defecto: { type: "string", editable: false },
                        InicioMM: { type: "number", editable: false },
                        FinMM: { type: "number", editable: false },
                        Soldador: { type: "string", editable: true },
                        
                    }
                }
            },
        },
        selectable: true,
        filterable: getGridFilterableMaftec(),
        columns: [
          { field: "Ubicacion", title: "Ubicacion", filterable: getGridFilterableCellMaftec(), width: "100px" },
          { field: "Defecto", title: "Defecto", filterable: getGridFilterableCellMaftec(), width: "100px" },
          { field: "InicioMM", title: _dictionary.CapturaReportePruebasHeaderInicio[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellNumberMaftec(), width: "80px", editor: RenderInicioMM, attributes: { style: "text-align:right;" } },
          { field: "FinMM", title: _dictionary.CapturaReportePruebasHeaderFin[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellNumberMaftec(), width: "80px", editor: RenderFinMM, attributes: { style: "text-align:right;" } },
          { field: "Soldador", title: "Soldador", filterable: getGridFilterableCellMaftec(), width: "100px", editor: RenderComboBoxSoldador },
          
        ],
        editable: true,
        //toolbar: [{ name: "create" }],
        navigatable: true,
        dataBound: function (a) {

        },
        beforeEdit: function (e) {
            var columnIndex = this.cellIndex(e.container);
            var fieldName = this.thead.find("th").eq(columnIndex).data("field");
            if (!isEditable(fieldName, e.model)) {
                e.preventDefault();
            }
        },

    });
    CustomisaGrid($("#gridPopUp"));

};


function limpiarRenglon(e) {
    e.preventDefault();
    if ($('#botonGuardar').text() == _dictionary.DetalleAvisoLlegada0017[$("#language").data("kendoDropDownList").value()]) {
        var itemToClean = $("#gridPopUp").data("kendoGrid").dataItem($(e.currentTarget).closest("tr"));
        
        itemToClean.JtaSeg1 = "";
        itemToClean.JuntaSpoolID = 0;
        itemToClean.JtaSeg2 = "";
        itemToClean.JuntaSpool2ID = 0;
        var dataSource = $("#gridPopUp").data("kendoGrid").dataSource;
        dataSource.sync();
    }

}


function isEditable(fieldName, model) {

    if (fieldName === "JtaSeg1") {
        if (model.Soldador == "" || model.EsRepetido) {
            return false;
        }
     }
    if (fieldName === "JtaSeg2") {
        if (model.Soldador == "" || model.EsRepetido) {
             return false;
         }
     }

    return true; // default to editable
}




var currentPlaca = null;
function LlenarGridPopUp(data) {
    modeloRenglon = data;
    currentPlaca = data;
    var ds = $("#gridPopUp").data("kendoGrid").dataSource;
    VentanaModalDetallePlaca();
}



function VentanaModalDetallePlaca() {

    var modalTitle = "";
    modalTitle = "Partidas";
    var window = $("#windowGrid");
    var win = window.kendoWindow({
        modal: true,
        title: "DetallePlaca",
        resizable: false,
        visible: true,
        width: "60%",
        minWidth: 30,
        position: {
            top: "10px",
            left: "10px"
        },
        animation: {
            close: false,
            open: false
        },

    }).data("kendoWindow");
    window.data("kendoWindow").title(modalTitle);
    window.data("kendoWindow").center().open();

};
