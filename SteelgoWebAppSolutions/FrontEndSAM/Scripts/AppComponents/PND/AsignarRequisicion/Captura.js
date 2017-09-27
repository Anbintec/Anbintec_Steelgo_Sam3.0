var editado = false;

function changeLanguageCall() {
    editado = false;
    var paramReq = getParameterByName('requisicion');
    var requisicionID = getParameterByName('requisicion');
    if (requisicionID == null) {
        AjaxCargarCamposPredeterminados(true);

    } else {
        AjaxCargarCamposPredeterminados(false);
        AjaxObtenerElementoRequisicion(requisicionID)
    }

    SiguienteProceso(paramReq);
    CargarGrid();
    CargarGridPopUp();
    document.title = _dictionary.ServiciosTecnicosAsignarRequisicionBreadcrumb[$("#language").data("kendoDropDownList").value()];
};

IniciarAsignarRequisicion();

function IniciarAsignarRequisicion() {


    SuscribirEventos();


};

function getParameterByName(name, url) {

    if (!url) url = window.location.href;
    name = name.replace(/[\[\]]/g, "\\$&");
    var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
        results = regex.exec(url);
    if (!results) return null;
    if (!results[2]) return '';
    return decodeURIComponent(results[2].replace(/\+/g, " "));
}

function SiguienteProceso(paramReq) {
    var url = "";
    if (paramReq == null) {
        url = "/PND/CapturaReporteRT?leng=" + $("#language").data("kendoDropDownList").value();
    } else {
        url = "/PND/CapturaReporteRT?leng=" + $("#language").data("kendoDropDownList").value()
            + "&requisicion=" + paramReq;
    }

    $("#EntregaresultadosSup").attr("href", url);
    $("#EntregaresultadosINF").attr("href", url);
}

function CargarGrid() {
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
    $("#grid").kendoGrid({
        autoBind: true,
        autoSync: true,
        edit: function (e) {
            setTimeout(function () {
                var inputName = e.container.find('input');

                inputName.select();
            });
            if ($('#botonGuardar').text() != _dictionary.lblGuardar[$("#language").data("kendoDropDownList").value()]) {//|| e.model.Accion == 2) {
                this.closeCell();
            }
        },

        dataSource: {
            schema: {
                model: {
                    fields: {
                        Clave: { type: "string", editable: false },
                        Nombre: { type: "string", editable: false },
                        Observacion: { type: "string", editable: false },
                        Fecha: { type: "date", editable: false },
                        RequisicionID: { type: "int", editable: false },
                        Requisicion: { type: "string", editable: false },
                        CantidadJuntas: { type: "number", editable: false },
                        Proveedor: { type: "string", editable: true },
                        ProveedorID: { type: "int", editable: true },
                        Equipo: {  hidden: true, type: "string", editable: true, },
                        TurnoLaboral: { type: "string", editable: true },
                        Capacidad: { type: "string", editable: false },
                        JuntasAsignadas: { type: "string", editable: false }
                    }
                }
            },
            filter: {
                logic: "or",
                filters: [
                  { field: "Accion", operator: "eq", value: 1 },
                  { field: "Accion", operator: "eq", value: 2 },
                  { field: "Accion", operator: "eq", value: 4 }
                ]
            },
            pageSize: 10,
            serverPaging: false,
            serverFiltering: false,
            serverSorting: false
        },
        navigatable: true,
        editable: true,
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
            { field: "Nombre", title: _dictionary.columnTipoPrueba[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "130px" },
            { field: "Requisicion", title: _dictionary.columnRequisicion[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "130px" },
            { field: "Observacion", title: _dictionary.columnObservacion[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "150px" },
            { field: "Fecha", title: _dictionary.columnFecha[$("#language").data("kendoDropDownList").value()], filterable: { cell: { showOperators: false } }, format: _dictionary.FormatoFecha[$("#language").data("kendoDropDownList").value()], width: "110px" },
            { field: "CantidadJuntas", title: _dictionary.columnJuntas[$("#language").data("kendoDropDownList").value()], template: "<div class='EnlaceDetalleJuntas' style='text-align:center;'><a href='\\#'  > <span>#=CantidadJuntas#</span></a></div>", filterable: getGridFilterableCellNumberMaftec(), width: "90px" },
            { field: "Proveedor", title: _dictionary.columnProveedor[$("#language").data("kendoDropDownList").value()], editor: RenderComboBoxProveedor, filterable: getGridFilterableCellMaftec(), width: "140px" },
            { field: "Equipo", title: _dictionary.columnEquipo[$("#language").data("kendoDropDownList").value()], editor: RenderComboBoxHerramientaPrueba, filterable: getGridFilterableCellMaftec(), width: "130px" },
            { field: "TurnoLaboral", title: _dictionary.columnTurnoLaboral[$("#language").data("kendoDropDownList").value()], editor: RenderComboBoxTurnoLaboral, filterable: getGridFilterableCellMaftec(), width: "120px" },
            { field: "Capacidad", title: _dictionary.columnCapacidad[$("#language").data("kendoDropDownList").value()], editor: RenderComboBoxTurnoLaboral, filterable: getGridFilterableCellMaftec(), width: "80px" },
            { field: "JuntasAsignadas", title: _dictionary.columnJuntasAsignadas[$("#language").data("kendoDropDownList").value()], editor: RenderComboBoxTurnoLaboral, template: "<div class='EnlaceDetalleElementosAsignados' style='text-align:center;'><a href='\\#'  > <span>#=JuntasAsignadas#</span></a></div>", filterable: getGridFilterableCellMaftec(), width: "110px" },
        //{ command: { text: _dictionary.botonCancelar[$("#language").data("kendoDropDownList").value()], click: cancelarCaptura }, title: _dictionary.tituloEliminar[$("#language").data("kendoDropDownList").value()], width: "50px" },
            { command: { text: _dictionary.botonLimpiar[$("#language").data("kendoDropDownList").value()], click: limpiarRenglon }, title: _dictionary.columnLimpiar[$("#language").data("kendoDropDownList").value()], width: "50px", attributes: { style: "text-align:center;" } }
        ],
        beforeEdit: function (e) {
            var columnIndex = this.cellIndex(e.container);
            var fieldName = this.thead.find("th").eq(columnIndex).data("field");
            if (!isEditable(fieldName, e.model)) {
                e.preventDefault();
            }
        },
    });
    CustomisaGrid($("#grid"));
};

function isEditable(fieldName, model) {
    if (fieldName === "Equipo") {
        var str = model.Nombre;
        var respuesta = model.RequiereEquipo;
        return respuesta;
    }

    else if (fieldName === "TurnoLaboral") {
        if (model.Proveedor == "") {
            return false;
        }
    }
    return true; // default to editable
}


function cancelarCaptura(e) {
    e.preventDefault();
    var filterValue = $(e.currentTarget).val();
    var dataItem = $("#grid").data("kendoGrid").dataItem($(e.currentTarget).closest("tr"));
    if ($("#language").val() == "es-MX") {
        if ($('#Guardar').text().trim() != "Editar") {
            windowTemplate = kendo.template($("#windowTemplate").html());

            ventanaConfirm = $("#ventanaConfirm").kendoWindow({
                iframe: true,
                title: _dictionary.WarningTitle[$("#language").data("kendoDropDownList").value()],
                visible: false, //the window will not appear before its .open method is called
                width: "auto",
                height: "auto",
                modal: true,
                animation: {
                    close: false,
                    open: false
                }
            }).data("kendoWindow");

            ventanaConfirm.content(_dictionary.CapturaReporteModificarNoPlacas[$("#language").data("kendoDropDownList").value()] +
                        "</br><center><button class='btn btn-blue' id='yesButton'>Si</button><button class='btn btn-blue' id='noButton'> No</button></center>");

            ventanaConfirm.open().center();

            $("#yesButton").click(function () {

                var dataSource = $("#grid").data("kendoGrid").dataSource;

                if (dataItem.Accion == 1) {
                    dataSource.remove(dataItem);
                }
                else {
                    dataItem.Accion = 3;
                }

                $("#grid").data("kendoGrid").dataSource.sync();

                ventanaConfirm.close();
            });
            $("#noButton").click(function () {
                ventanaConfirm.close();
            });

        }
    }
    else {
        if ($('#Guardar').text().trim() != "Edit") {
            windowTemplate = kendo.template($("#windowTemplate").html());

            ventanaConfirm = $("#ventanaConfirm").kendoWindow({
                iframe: true,
                title: _dictionary.WarningTitle[$("#language").data("kendoDropDownList").value()],
                visible: false, //the window will not appear before its .open method is called
                width: "auto",
                height: "auto",
                modal: true,
                animation: {
                    close: false,
                    open: false
                }
            }).data("kendoWindow");

            ventanaConfirm.content(_dictionary.CapturaInspeccionPreguntaBorradoCaptura[$("#language").data("kendoDropDownList").value()] +
                        "</br><center><button class='btn btn-blue' id='yesButton'>Si</button><button class='btn btn-blue' id='noButton'> No</button></center>");

            ventanaConfirm.open().center();

            $("#yesButton").click(function () {
                var dataSource = $("#grid").data("kendoGrid").dataSource;

                if (dataItem.Accion == 1)
                    dataSource.remove(dataItem);
                else
                    dataItem.Accion == 2;
                dataSource.remove(dataItem);
                $("#grid").data("kendoGrid").dataSource.sync();

                ventanaConfirm.close();
            });
            $("#noButton").click(function () {
                ventanaConfirm.close();
            });
        }
    }

}

function limpiarRenglon(e) {
    e.preventDefault();
    if ($('#Guardar').text() == _dictionary.lblGuardar[$("#language").data("kendoDropDownList").value()]) {

        var itemToClean = $("#grid").data("kendoGrid").dataItem($(e.currentTarget).closest("tr"));

        if (itemToClean.Accion == 2)
            itemToClean.Accion = 4;


        if (itemToClean.TurnoLaboral != "") {
            var JuntasAsignadasFinal = parseInt(itemToClean.JuntasAsignadas) - parseInt(itemToClean.CantidadJuntas);

            if (!itemToClean.RequiereEquipo) {
                setJuntasAsignatdasCapacidadTurnoProveedor(parseInt(itemToClean.JuntasAsignadas) - parseInt(itemToClean.CantidadJuntas), itemToClean.CapacidadTurnoProveedorAnteriorID);
                itemToClean.CapacidadTurnoProveedorID = 0;
                itemToClean.CapacidadTurnoProveedorAnteriorID = 0;
            }
            else {
                setJuntasAsignatdasCapacidadTurnoEquipo(parseInt(itemToClean.JuntasAsignadas) - parseInt(itemToClean.CantidadJuntas), itemToClean.CapacidadTurnoEquipoAnteriorID);
                itemToClean.CapacidadTurnoEquipoID = 0;
                itemToClean.CapacidadTurnoEquipoAnteriorID = 0;
            }

        }

        itemToClean.TipoPruebaProveedorID = 0;
        itemToClean.HerramientadePruebaID = 0;
        itemToClean.TurnoLaboral = "";
        itemToClean.Proveedor = "";
        itemToClean.Equipo = "";
        itemToClean.JuntasAsignadas = "";
        itemToClean.Capacidad = "";
        var dataSource = $("#grid").data("kendoGrid").dataSource;
        dataSource.sync();
        //alert(itemToClean);
    }


}


function CargarGridPopUp() {

    $("#gridPopUp").kendoGrid({
        dataSource: {
            data: [],
            schema: {
                model: {
                    fields: {
                        NumeroControl: { type: "string", editable: false },
                        EtiquetaJunta: { type: "string", editable: false },
                        TipoJunta: { type: "string", editable: false },
                        NombreRequisicion: { type: "string", editable: false },
                        Cuadrante: { type: "string", editable: false },
                        Prioridad: { type: "number", editable: false },
                        Clasificacion: { type: "string", editable: false },
                        Diametro: { type: "string", editable: false },
                        Espesor: { type: "string", editable: false },
                        Cedula: { type: "string", editable: false },
                        RequisicionID: { type: "int", editable: false },
                        ProyectoID: { type: "int", editable: false },
                        SpoolID: { type: "int", editable: false },
                        OrdenTrabajoSpoolID: { type: "int", editable: false },
                        JuntaSpoolID: { type: "int", editable: false },
                        TipoPruebaID: { type: "int", editable: false },
                        Especificacion: { type: "number", editable: false },
                        Agregar: { type: "boolean", editable: false },
                    }
                }
            },
            pageSize: 10,
            serverPaging: false,
            serverFiltering: false,
            serverSorting: false

        },
        navigatable: true,
        filterable: {
            extra: false
        },
        editable: false,
        autoHeight: true,
        sortable: true,
        scrollable: true,
        pageable: {
            refresh: false,
            pageSizes: [10, 25, 50, 100],
            info: false,
            input: false,
            numeric: true,
        },
        selectable: true,
        filterable: getGridFilterableMaftec(),

        columns: [
            { field: "NumeroControl", title: _dictionary.columnNumeroControl[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "110px" },
            { field: "EtiquetaJunta", title: _dictionary.columnJunta[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "60px", attributes: { style: "text-align:right;" } },
            { field: "TipoJunta", title: _dictionary.columnTipoJta[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "122px" },
            { field: "NombreRequisicion", title: _dictionary.columnRequisicion[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "145px" },
            { field: "Cuadrante", title: _dictionary.columnCuadrante[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "130px" },
            { field: "Prioridad", title: _dictionary.columnPrioridad[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellNumberMaftec(), width: "110px", attributes: { style: "text-align:right;" } },
            { field: "Clasificacion", title: _dictionary.columnClasificacion[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "130px" },
            { field: "Diametro", title: _dictionary.columnDiametro[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellNumberMaftec(), width: "130px", attributes: { style: "text-align:right;" } },
            { field: "Espesor", title: _dictionary.columnEspesor[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellNumberMaftec(), width: "122px", attributes: { style: "text-align:right;" } },
            { field: "Cedula", title: _dictionary.columnCedula[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "135px" },
            {
                field: "Agregar", title: _dictionary.columnAgregar[$("#language").data("kendoDropDownList").value()], filterable: false,
                //filterable: {
                //    multi: true,
                //    messages: {
                //        isTrue: _dictionary.lblVerdadero[$("#language").data("kendoDropDownList").value()],
                //        isFalse: _dictionary.lblFalso[$("#language").data("kendoDropDownList").value()],
                //        style: "max-width:100px;"
                //    },
                //    dataSource: [{ Etiquetado: true }, { Etiquetado: false }]
                //},
                template: "<input name='fullyPaid' class='ob-paid' type='checkbox'   #= (PermiteSeparar == 0) ? 'hidden=true': '' # />", width: "100px",minResizableWidth: "0px", attributes: { style: "text-align:center;" }
            },
        ],
        editable: false,
        navigatable: true,
        dataBound: function (a) {
            $(".ob-paid").bind("change", function (e) {

                var grid = $("#gridPopUp").data("kendoGrid"),
                    dataItem = grid.dataItem($(e.target).closest("tr"));
                if (e.target.checked == true)
                    dataItem.Agregar = true;
                else
                    dataItem.Agregar = false;


                $("#gridPopUp").data("kendoGrid").closeCell();

            });
        }
    });
    CustomisaGrid($("#gridPopUp"));
};

function LlenarGridPopUp(data) {
    modeloRenglon = data;
    $("#gridPopUp").data('kendoGrid').dataSource.data([]);
    var ds = $("#gridPopUp").data("kendoGrid").dataSource;
    var array = data.ListaElementosRequisicion;
    for (var i = 0; i < array.length; i++) {
        ds.add(array[i]);
    }
    VentanaModal();
}

function LlenarGridPopUpElementosAsignados(data) {
    modeloRenglon = data;
    $("#gridPopUp").data('kendoGrid').dataSource.data([]);
    var ds = $("#gridPopUp").data("kendoGrid").dataSource;
    var array = data;
    for (var i = 0; i < array.length; i++) {
        ds.add(array[i]);
    }
    VentanaModal();
}


function VentanaModal() {

    var modalTitle = "";
    modalTitle = _dictionary.lblDetalleJuntas[$("#language").data("kendoDropDownList").value()];
    var window = $("#windowGrid");
    var win = window.kendoWindow({
        modal: true,
        title: modalTitle,
        resizable: false,
        visible: true,
        width: "95%",
        minWidth: 30,
        position: {
            top: "10px",
            left: "10px"
        },
        animation: {
            close: false,
            open: false
        },
        actions: [],

    }).data("kendoWindow");
    window.data("kendoWindow").title(modalTitle);
    window.data("kendoWindow").center().open();

};


function PlanchaProveedor() {
    var dataSource = $("#grid").data("kendoGrid").dataSource;
    var filters = dataSource.filter();
    var allData = dataSource.data();
    var query = new kendo.data.Query(allData);
    var data = query.filter(filters).data;

    for (var i = 0; i < data.length; i++) {
        if ($('input:radio[name=Muestra]:checked').val() === "Todos" && $("#inputProveedor").data("kendoComboBox").dataItem($("#inputProveedor").data("kendoComboBox").select()) != undefined && $("#inputPrueba").data("kendoComboBox").dataItem($("#inputPrueba").data("kendoComboBox").select()).Clave == data[i].Clave) {
            data[i].ProveedorID = $("#inputProveedor").val();
            data[i].Proveedor = $("#inputProveedor").data("kendoComboBox").text();
            var Proveedor = ObtenerProveedor($("#inputProveedor").val(), data[i].ListaProveedor);
            data[i].ListaHerramientaPrueba = Proveedor.ListaHerramientaPrueba;
            data[i].ListaTurnoLaboral = Proveedor.ListaTurnoLaboral;
        }
        else {
            if ((data[i].Proveedor == "" || data[i].Proveedor == null || data[i].Proveedor == undefined) && $("#inputProveedor").data("kendoComboBox").dataItem($("#inputProveedor").data("kendoComboBox").select()) != undefined && $("#inputPrueba").data("kendoComboBox").dataItem($("#inputPrueba").data("kendoComboBox").select()).Clave == data[i].Clave) {
                data[i].ProveedorID = $("#inputProveedor").val();
                data[i].Proveedor = $("#inputProveedor").data("kendoComboBox").text();
                var Proveedor = ObtenerProveedor($("#inputProveedor").val(), data[i].ListaProveedor);
                data[i].ListaHerramientaPrueba = Proveedor.ListaHerramientaPrueba;
                data[i].ListaTurnoLaboral = Proveedor.ListaTurnoLaboral;
            }
        }
    }
    $("#grid").data("kendoGrid").dataSource.sync();
};

function ObtenerProveedor(id, listaProveedores) {
    for (var i = 0; i < listaProveedores.length; i++) {
        if (id = listaProveedores[i].ProveedorID)
            return listaProveedores[i];
    }
    return null;
};
function replaceAll(str, find, replace) {
    return str.replace(new RegExp(escapeRegExp(find), 'g'), replace);
}
function escapeRegExp(str) {
    return str.replace(/([.*+?^=!:${}()|\[\]\/\\])/g, "\\$1");
}


function getNumeroJuntasAsignadasEquipo(CapacidadTurnoEquipoID) {
    var ds = $("#grid").data("kendoGrid").dataSource._data;

    for (var i = 0; i < ds.length; i++) {
        if (ds[i].JuntasAsignadas != "" && ds[i].CapacidadTurnoEquipoID == CapacidadTurnoEquipoID && ds[i].RequiereEquipo) {
            return ds[i].JuntasAsignadas;
        }
    }
    return 0;
}
function getNumeroJuntasAsignadasProveedor(CapacidadTurnoProveedorID) {
    var ds = $("#grid").data("kendoGrid").dataSource._data;

    for (var i = 0; i < ds.length; i++) {
        if (ds[i].JuntasAsignadas != "" && ds[i].CapacidadTurnoProveedorID == CapacidadTurnoProveedorID && !ds[i].RequiereEquipo) {
            return ds[i].JuntasAsignadas;
        }
    }
    return 0;
}


function setJuntasAsignatdasCapacidadTurnoProveedor(JuntasAsignadas, CapacidadTurnoProveedorID) {
    var ds = $("#grid").data("kendoGrid").dataSource._data;

    for (var i = 0; i < ds.length; i++) {
        if (ds[i].JuntasAsignadas != "" && ds[i].CapacidadTurnoProveedorID == CapacidadTurnoProveedorID && !ds[i].RequiereEquipo) {
            ds[i].JuntasAsignadas = JuntasAsignadas;
        }
    }

    $("#grid").data("kendoGrid").dataSource.sync();
}

function setJuntasAsignatdasCapacidadTurnoEquipo(JuntasAsignadas, CapacidadTurnoEquipoID) {
    var ds = $("#grid").data("kendoGrid").dataSource._data;

    for (var i = 0; i < ds.length; i++) {
        if (ds[i].JuntasAsignadas != "" && ds[i].CapacidadTurnoEquipoID == CapacidadTurnoEquipoID && ds[i].RequiereEquipo) {
            ds[i].JuntasAsignadas = JuntasAsignadas;
        }
    }

    $("#grid").data("kendoGrid").dataSource.sync();
}



function setListadojuntasAsignadasCapacidadTurnoEquipo(CapacidadTurnoEquipoID, JuntasAsignadas) {
    var ds = $("#grid").data("kendoGrid").dataSource._data;

    for (var i = 0; i < ds.length; i++) {
        for (var j = 0; j < ds[i].ListaTurnoLaboral.length; j++) {
            if (ds[i].ListaTurnoLaboral[j].CapacidadTurnoEquipoID == CapacidadTurnoEquipoID && ds[i].RequiereEquipo) {
                ds[i].ListaTurnoLaboral[j].JuntasAsignadas = JuntasAsignadas;
            }
        }
        for (var k = 0; k < ds[i].ListaTurnoLaboralTotal.length; k++) {
            if (ds[i].ListaTurnoLaboralTotal[k].CapacidadTurnoEquipoID == CapacidadTurnoEquipoID && ds[i].RequiereEquipo) {
                ds[i].ListaTurnoLaboralTotal[k].JuntasAsignadas = JuntasAsignadas;
            }
        }

    }
    $("#grid").data("kendoGrid").dataSource.sync();
}


function setListadojuntasAsignadasCapacidadTurnoProveedor(CapacidadTurnoProveedorID, JuntasAsignadas) {
    var ds = $("#grid").data("kendoGrid").dataSource._data;

    for (var i = 0; i < ds.length; i++) {
        for (var j = 0; j < ds[i].ListaTurnoLaboral.length; j++) {
            if (ds[i].ListaTurnoLaboral[j].CapacidadTurnoProveedorID == CapacidadTurnoProveedorID && !ds[i].RequiereEquipo) {
                ds[i].ListaTurnoLaboral[j].JuntasAsignadas = JuntasAsignadas;
            }
        }
        for (var k = 0; k < ds[i].ListaTurnoLaboralTotal.length; k++) {
            if (ds[i].ListaTurnoLaboralTotal[k].CapacidadTurnoProveedorID == CapacidadTurnoProveedorID && !ds[i].RequiereEquipo) {
                ds[i].ListaTurnoLaboralTotal[k].JuntasAsignadas = JuntasAsignadas;
            }
        }

    }
    $("#grid").data("kendoGrid").dataSource.sync();
}



function generarListadoCorrectoAsignacionEquipo(ListaElementosRequisicion, CapacidadTurnoEquipoID) {
    var ds = $("#grid").data("kendoGrid").dataSource._data;
    loadingStart();
    var listaAux = [];
    for (var i = ds.length - 1; i >= 0; i--) {
        if (ds[i].CapacidadTurnoEquipoID == CapacidadTurnoEquipoID && ds[i].RequiereEquipo) {
            if (!listaAux.length < ds[i].ListaElementosAsignadosTurno.length)
                listaAux = ds[i].ListaElementosAsignadosTurno;

        }
    }

    for (var k = 0; k < ListaElementosRequisicion.length; k++) {
        listaAux.push(ListaElementosRequisicion[k]);
    }


    for (var i = ds.length - 1; i >= 0; i--) {
        if (ds[i].CapacidadTurnoEquipoID == CapacidadTurnoEquipoID && ds[i].RequiereEquipo) {
            ds[i].ListaElementosAsignadosTurno = listaAux;
        }
    }

    $("#grid").data("kendoGrid").dataSource.sync();
    loadingStop();
}



function generaListadoCorrectoAsignacionProveedor(ListaElementosRequisicion, CapacidadTurnoProveedorID) {
    var ds = $("#grid").data("kendoGrid").dataSource._data;
    loadingStart();
    var listaAux = [];
    for (var i = ds.length - 1; i >= 0; i--) {
        if (ds[i].CapacidadTurnoProveedorID == CapacidadTurnoProveedorID && !ds[i].RequiereEquipo) {
            if (!listaAux.length < ds[i].ListaElementosAsignadosTurno.length)
                listaAux = ds[i].ListaElementosAsignadosTurno;


        }
    }

    for (var k = 0; k < ListaElementosRequisicion.length; k++) {
        listaAux.push(ListaElementosRequisicion[k]);
    }


    for (var i = ds.length - 1; i >= 0; i--) {
        if (ds[i].CapacidadTurnoProveedorID == CapacidadTurnoProveedorID && !ds[i].RequiereEquipo) {
            ds[i].ListaElementosAsignadosTurno = listaAux;
        }
    }

    $("#grid").data("kendoGrid").dataSource.sync();
    loadingStop();
}

function removerListadoCorrectoAsignacionEquipo(ListaElementosRequisicion, CapacidadTurnoEquipoID) {
    var ds = $("#grid").data("kendoGrid").dataSource._data;
    loadingStart();
    var listaAux = [];
    for (var i = ds.length - 1; i >= 0; i--) {
        if (ds[i].CapacidadTurnoEquipoID == CapacidadTurnoEquipoID && ds[i].RequiereEquipo) {
            if (!listaAux.length < ds[i].ListaElementosAsignadosTurno.length)
                listaAux = ds[i].ListaElementosAsignadosTurno;

        }
    }
    for (var i = listaAux.length - 1; i >= 0; i--) {
        for (var k = 0; k < ListaElementosRequisicion.length; k++) {
            if (ListaElementosRequisicion[k].SpoolID == listaAux[i].SpoolID && ListaElementosRequisicion[k].EtiquetaJunta == listaAux[i].EtiquetaJunta) {
                listaAux.splice(i, 1);
                break;
            }
        }
    }


    for (var i = ds.length - 1; i >= 0; i--) {
        if (ds[i].CapacidadTurnoEquipoID == CapacidadTurnoEquipoID && ds[i].RequiereEquipo) {
            ds[i].ListaElementosAsignadosTurno = listaAux;
        }
        for (var j = 0; j < ds[i].ListaTurnoLaboral.length; j++) {
            if (ds[i].ListaTurnoLaboral[j].CapacidadTurnoEquipoID == CapacidadTurnoEquipoID && ds[i].RequiereEquipo) {
                ds[i].ListaTurnoLaboral[j].ListaElementosAsignadosTurno = listaAux;
            }
        }
        for (var k = 0; k < ds[i].ListaTurnoLaboralTotal.length; k++) {
            if (ds[i].ListaTurnoLaboralTotal[k].CapacidadTurnoEquipoID == CapacidadTurnoEquipoID && ds[i].RequiereEquipo) {
                ds[i].ListaTurnoLaboralTotal[k].ListaElementosAsignadosTurno = listaAux;
            }
        }
    }

    $("#grid").data("kendoGrid").dataSource.sync();
    loadingStop();
}



function removerListadoCorrectoAsignacionProveedor(ListaElementosRequisicion, CapacidadTurnoProveedorID) {
    var ds = $("#grid").data("kendoGrid").dataSource._data;

    var listaAux = [];
    for (var i = ds.length - 1; i >= 0; i--) {
        if (ds[i].CapacidadTurnoProveedorID == CapacidadTurnoProveedorID && !ds[i].RequiereEquipo) {
            if (!listaAux.length < ds[i].ListaElementosAsignadosTurno.length)
                listaAux = ds[i].ListaElementosAsignadosTurno;

        }
    }
    for (var i = listaAux.length - 1; i >= 0; i--) {
        for (var k = 0; k < ListaElementosRequisicion.length; k++) {
            if (ListaElementosRequisicion[k].SpoolID == listaAux[i].SpoolID && ListaElementosRequisicion[k].EtiquetaJunta == listaAux[i].EtiquetaJunta) {
                listaAux.splice(i, 1);
                break;
            }
        }
    }

    for (var i = ds.length - 1; i >= 0; i--) {
        if (ds[i].CapacidadTurnoProveedorID == CapacidadTurnoProveedorID && !ds[i].RequiereEquipo) {
            ds[i].ListaElementosAsignadosTurno = listaAux;
        }
        for (var j = 0; j < ds[i].ListaTurnoLaboral.length; j++) {
            if (ds[i].ListaTurnoLaboral[j].CapacidadTurnoProveedorID == CapacidadTurnoProveedorID && !ds[i].RequiereEquipo) {
                ds[i].ListaTurnoLaboral[j].ListaElementosAsignadosTurno = listaAux;
            }
        }
        for (var k = 0; k < ds[i].ListaTurnoLaboralTotal.length; k++) {
            if (ds[i].ListaTurnoLaboralTotal[k].CapacidadTurnoProveedorID == CapacidadTurnoProveedorID && !ds[i].RequiereEquipo) {
                ds[i].ListaTurnoLaboralTotal[k].ListaElementosAsignadosTurno = listaAux;
            }
        }
    }

    $("#grid").data("kendoGrid").dataSource.sync();

}



