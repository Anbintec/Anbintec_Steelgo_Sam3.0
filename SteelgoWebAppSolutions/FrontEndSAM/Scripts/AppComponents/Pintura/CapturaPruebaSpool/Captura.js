var editado = false;
var ventanaConfirmEdicionSinTipoBusqueda;

var LineaCaptura = {OrdenTrabajoSpool:"", ProcesoIDSeleccionado: "", ColorIDSeleccionado: "", ProyectoProcesoPruebaIDSeleccionado: "", InputIDSeleccionado: "" }
var EjecutaChange = 0;
var CantidadLotes = 0;
var DatosLotes = '';

function changeLanguageCall() {
    SuscribirEventos();
    CargarGrid();
}

function TryParseInt(str, defaultValue) {
    var retValue = defaultValue;
    if (str !== null) {
        if (str.length > 0) {
            if (!isNaN(str)) {
                retValue = parseInt(str);
            }
        }
    }
    return retValue;
}

function Limpiar()
{
    $("#InputOrdenTrabajo").val("");
    $("#InputID").data("kendoComboBox").dataSource.data([]);
    $("#InputID").data("kendoComboBox").value("");

    $("#inputProceso").data("kendoComboBox").dataSource.data([]);
    $("#inputProceso").data("kendoComboBox").value("");

    $("#inputPrueba").data("kendoComboBox").dataSource.data([]);
    $("#inputPrueba").data("kendoComboBox").value("");

    $("#inputColor").data("kendoComboBox").dataSource.data([]);
    $("#inputColor").data("kendoComboBox").value("");

    $("#grid").data("kendoGrid").dataSource.data([]);
    $('#InformacionSpoolDiv').hide();
    
}

function CargarGrid() {
    $("#grid").kendoGrid({
        edit: function (e) {
            var inputName = e.container.find('input');
            inputName.select();
            if ($('#botonGuardar').text() != _dictionary.MensajeGuardar[$("#language").data("kendoDropDownList").value()])
                this.closeCell();

        },
        dataSource: {
            schema: {
                model: {
                    fields: {
                        PruebaLoteID: { type: "number", editable: false },
                        Accion: { type: "number", editable: false },
                        SpoolID: { type: "number", editable: false },
                        ProyectoProcesoPruebaID: { type: "number", editable: false },
                        UnidadMaxima: { type: "number", editable: false },
                        UnidadMinima: { type: "number", editable: false },
                        Medida: { type: "string", editable: false },


                        FechaPrueba: { type: "date", editable: true },
                        UnidadMedida: { type: "number", editable: true },
                        ResultadoEvaluacion: { type: "boolean", editable: false },

                        Lote: { type: "number", editable: false }
                    }
                }
            }, filter: {
                logic: "or",
                filters: [
                  { field: "Accion", operator: "eq", value: 1 },
                  { field: "Accion", operator: "eq", value: 2 },
                  { field: "Accion", operator: "eq", value: 0 },
                  { field: "Accion", operator: "eq", value: 4 },
                  { field: "Accion", operator: "eq", value: undefined }
                ]
            }


        },
        selectable: true,
        filterable: getGridFilterableMaftec(),
        columns: [
                  { field: "Lote", title: _dictionary.columnLote[$("#language").data("kendoDropDownList").value()], filterable: { cell: { showOperators: false } }, width: "20px" },
                  { field: "FechaPrueba", format: _dictionary.FormatoFecha[$("#language").data("kendoDropDownList").value()], editor: RenderDatePicker, title: _dictionary.columnFechaPrueba[$("#language").data("kendoDropDownList").value()], filterable: { cell: { showOperators: false } }, width: "20px" },
                  { field: "UnidadMedida", title: "Valor U. Medida", filterable: getGridFilterableCellNumberMaftec(), width: "20px", attributes: { style: "text-align:right;" }, editor: RenderMedida },
                   {
                       field: "ResultadoEvaluacion", title: "Aprobado", filterable: {
                           multi: true,
                           messages: {
                               isTrue: _dictionary.lblVerdadero[$("#language").data("kendoDropDownList").value()],
                               isFalse: _dictionary.lblFalso[$("#language").data("kendoDropDownList").value()],
                               style: "max-width:100px;"
                           },
                           dataSource: [{ OkPND: true }, { OkPND: false }]
                       }, template: "#= ResultadoEvaluacion ? 'Si' : 'No' #", width: "30px", attributes: { style: "text-align:center;" }
                   },
                  { command: { text: _dictionary.botonCancelar[$("#language").data("kendoDropDownList").value()], click: eliminarCaptura }, title: _dictionary.columnELM[$("#language").data("kendoDropDownList").value()], width: "10px", attributes: { style: "text-align:center;" } }
        ],
        editable: true,
        navigatable: true,
        toolbar: [{ name: "create" }],
        dataBound: function () {
            var grid = $("#grid").data("kendoGrid");
            var gridData = grid.dataSource.view();

            

          

            for (var i = 0; i < gridData.length; i++) {
                var currentUid = gridData[i].uid;
                if (gridData[i].RowOk == false) {
                    grid.table.find("tr[data-uid='" + currentUid + "']").removeClass("k-alt");
                    grid.table.find("tr[data-uid='" + currentUid + "']").addClass("kRowError");

                }
                else if (gridData[i].RowOk) {
                    if (i % 2 == 0)
                        grid.table.find("tr[data-uid='" + currentUid + "']").removeClass("k-alt");
                    grid.table.find("tr[data-uid='" + currentUid + "']").removeClass("kRowError");
                }
            }
        }
    });

    $(".k-button, .k-button-icontext, .k-grid-add").click(function (e) {
        if (CantidadLotes > 0) {
            for (var i = 0; i < CantidadLotes ; i++) {
                var dataSource = $("#grid").data("kendoGrid").dataSource;
                
                var total = $("#grid").data("kendoGrid").dataSource.data().length;
                var nuevoDato = { Lote: DatosLotes.split(',')[i], ResultadoEvaluacion :false}

                $("#grid").data("kendoGrid").dataSource.insert(total, nuevoDato);
                $("#grid").data("kendoGrid").dataSource.page(dataSource.totalPages());

            }
            return false;
        }
        else {
            displayNotify("SpoolSinLote", "", '1');
            return false;
        }
    });
    CustomisaGrid($("#grid"));
}



function eliminarCaptura(e) {
    e.preventDefault();
    if ($('#botonGuardar').text() == _dictionary.DetalleAvisoLlegada0017[$("#language").data("kendoDropDownList").value()]) {

        var filterValue = $(e.currentTarget).val();
        var dataItem = $("#grid").data("kendoGrid").dataItem($(e.currentTarget).closest("tr"));
        var dataSource = $("#grid").data("kendoGrid").dataSource;

        if (dataItem.Accion != 2)
            dataSource.remove(dataItem);
        else
            dataItem.Accion = 3

        dataSource.sync();
    }

}




function convertirImagen() {
    var file = document.querySelector('input[type=file]').files[0];
    var preview = document.querySelector('img');
    AjaxEnviarImagenBase64(document.querySelector('input[type=file]').value);
};