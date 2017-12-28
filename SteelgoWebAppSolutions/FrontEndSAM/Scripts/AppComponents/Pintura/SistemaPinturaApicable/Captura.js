 
var esNormal;
var editado = false;
var lineaCaptura = { ProyectoID: "", dato: "" }
var nextCell = null;

function IniciarSistemaPinturaAplicable() {
    SuscribirEventos();
}

function changeLanguageCall() {
    IniciarSistemaPinturaAplicable();
    $('input[value="spool"]').prop("checked", true);
    CargarGrid();
    AjaxCargarCamposPredeterminados();
    AjaxCargaProyecto();
    document.title = _dictionary.SistemaPinturaAplicableHeader[$("#language").data("kendoDropDownList").value()];
}

function CargarGrid() {
    //BeforeEdit
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
        edit: function (e) {

            if ($('#Guardar').text() != _dictionary.lblGuardar[$("#language").data("kendoDropDownList").value()]) {
                this.closeCell();
            }
            setTimeout(function () {
                var inputName = e.container.find('input');

                inputName.select();
            });
            if ($(".k-grid-content td").css("white-space") == "normal") {
                esNormal = true;
            }
            else {
                esNormal = false;
            }
            nextCell = {
                cellIndex: e.container[0].cellIndex,
                uid: e.model.uid,
            };
        },
        autoBind: true,
        dataSource: {
            data: [],
            schema: {
                model: {
                    fields: {
                        Accion: { type: "number", editable: false },
                        SpoolAplicableID: { type: "number", editable: false },
                        OrdenTrabajoID: { type: "number", editable: false },
                        SpoolID: { type: "number", editable: true },
                        Spool: { type: "string", editable: false },
                        NumeroControl: { type: "string", editable: false },
                        Diametro: { type: "number", editable: false },
                        SistemaPinturaColorID: { type: "number", editable: true },
                        SistemaPinturaID: { type: "number", editable: true },
                        SistemaPintura: { type: "string", editable: true },
                        ColorPinturaID: { type: "number", editable: true },
                        Color: { type: "string", editable: true },
                        EstatusCaptura: { type: "number", editable: true },
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
        filterable: getGridFilterableMaftec(),
        editable: true,
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
        columns: [
			{ field: "Spool", title: _dictionary.SistemaPinturaAplicableSpool[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "140px" },
			{ field: "NumeroControl", title: _dictionary.columnNumeroControl2[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "130px" },
			{ field: "Diametro", title: _dictionary.columnDiametro[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellNumberMaftec(), editor: Diametro, width: "100px", width: "95px", attributes: { style: "text-align:right;" } },
			{ field: "SistemaPintura", title: _dictionary.columnSistemaPintura[$("#language").data("kendoDropDownList").value()], editor: comboBoxSistemaPintura, filterable: getGridFilterableCellMaftec(), width: "120px" },
			{ field: "Color", title: _dictionary.columnColor[$("#language").data("kendoDropDownList").value()], editor: comboBoxColor, filterable: getGridFilterableCellMaftec(), width: "110px", template: "#=Color#" },
			{ command: { text: _dictionary.botonLimpiar[$("#language").data("kendoDropDownList").value()], click: eliminaCaptura }, title: _dictionary.columnLimpiar[$("#language").data("kendoDropDownList").value()], width: "99px", attributes: { style: "text-align:center;" } }

        ],
        beforeEdit: function (e) {
            var columnIndex = this.cellIndex(e.container);
            var fieldName = this.thead.find("th").eq(columnIndex).data("field");
            if (!isEditable(fieldName, e.model)) {
                e.preventDefault();
            }
        },
        dataBound: function (e) {
            if (nextCell != null) {
                var grid = e.sender;
                console.log("set focus to next cell", nextCell);
                var nextCellIndex = nextCell.cellIndex;
                var row = grid.items().filter("tr[data-uid=" + nextCell.uid + "]");
                var cell = row.find("td:eq(" + nextCellIndex + ")");
                grid.current(cell);



            }


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

            if (esNormal) {
                $(".k-grid-content td").css("white-space", "normal");
            }
            else {
                $(".k-grid-content td").css("white-space", "nowrap");
            }

        }
    });
    CustomisaGrid($("#grid"));
}

function isEditable(fieldName, model) {
    if (fieldName === "Color") {
        var sistemaPinturaID = model.SistemaPinturaID
        if (model.ListaColorPintura.length <= 1) {
            return false;
        }
    }
    if (fieldName == 'SistemaPintura' || fieldName === "Color") {
        var AsignadoSpool = model.AsignadoSpool
        if (model.AsignadoSpool) {
            displayNotify("", _dictionary.PinturaSpoolCargadoEnCarro[$("#language").data("kendoDropDownList").value()].replace('?', model.NombreCarro), '1');
            return false;
        }
    }
    return true;
}
function eliminaCaptura(e) {
    if ($('#Guardar').text() == _dictionary.lblGuardar[$("#language").data("kendoDropDownList").value()]) {
        e.preventDefault();
        var itemToClean = $("#grid").data("kendoGrid").dataItem($(e.currentTarget).closest("tr"));
        if (itemToClean.AsignadoSpool) {
            displayNotify("", _dictionary.PinturaSpoolCargadoEnCarro[$("#language").data("kendoDropDownList").value()].replace('?', itemToClean.NombreCarro), '1');
        }
        else {
            itemToClean.SistemaPinturaColorID = 0;
            itemToClean.SistemaPinturaID = 0
            itemToClean.SistemaPintura = "";
            itemToClean.ColorPinturaID = 0;
            itemToClean.Color = "";

            var dataSource = $("#grid").data("kendoGrid").dataSource;
            dataSource.sync();
        }
    }
}

function plancharTodo(tipoLlenado) {

    var itemSistemaPintura = $("#inputSistemaPintura").data("kendoComboBox").dataItem($("#inputSistemaPintura").data("kendoComboBox").select());
    var itemColor = $("#inputColorPintura").data("kendoComboBox").dataItem($("#inputColorPintura").data("kendoComboBox").select());

    if (itemSistemaPintura != undefined && itemSistemaPintura.SistemaPinturaID != 0) {
        PlanchadoSistemaPintura(tipoLlenado);
    }

}

function PlanchadoSistemaPintura(tipoLlenado) {
    var itemSistemaPintura = $("#inputSistemaPintura").data("kendoComboBox").dataItem($("#inputSistemaPintura").data("kendoComboBox").select());
    var ds = $("#grid").data("kendoGrid").dataSource;
    var filters = ds.filter();
    var allData = ds.data();
    var query = new kendo.data.Query(allData);
    var data = query.filter(filters).data;
    var itemColor = $("#inputColorPintura").data("kendoComboBox").dataItem($("#inputColorPintura").data("kendoComboBox").select());



    for (var i = 0; i < data.length; i++) {
        if (tipoLlenado === "Todos" && !data[i].AsignadoSpool) {
            data[i].SistemaPintura = itemSistemaPintura.Nombre;
            data[i].SistemaPinturaID = itemSistemaPintura.SistemaPinturaID;
            data[i].NoPintable = itemSistemaPintura.NoPintable;
            data[i].EstatusCaptura = 1;
            data[i].ListaColorPintura = $("#inputColorPintura").data("kendoComboBox").dataSource._data;

            if (data[i].ListaColorPintura.length < 1) {

                data[i].Color = "";
                data[i].ColorPinturaID = 0;
                data[i].SistemaPinturaColorID = 0;
            } else {
                data[i].Color = itemColor.Nombre;
                data[i].ColorPinturaID = itemColor.ColorPinturaID;
                data[i].SistemaPinturaColorID = itemColor.SistemaPinturaColorID;
            }
        }
        else if (tipoLlenado === "Vacios" && !data[i].AsignadoSpool) {
            if (data[i].SistemaPintura === "" || data[i].SistemaPintura === null || data[i].SistemaPintura === undefined) {
                data[i].SistemaPintura = itemSistemaPintura.Nombre;
                data[i].SistemaPinturaID = itemSistemaPintura.SistemaPinturaID;
                data[i].NoPintable = itemSistemaPintura.NoPintable;
                data[i].EstatusCaptura = 1;
                data[i].ListaColorPintura = $("#inputColorPintura").data("kendoComboBox").dataSource._data;

                if (data[i].ListaColorPintura.length < 1) {
                    data[i].Color = "";
                    data[i].ColorPinturaID = 0;
                    data[i].SistemaPinturaColorID = 0;
                } else {
                    data[i].Color = itemColor.Nombre;
                    data[i].ColorPinturaID = itemColor.ColorPinturaID;
                    data[i].SistemaPinturaColorID = itemColor.SistemaPinturaColorID;
                }
            }
            else if (data[i].SistemaPintura === itemSistemaPintura.Nombre) {
                if (data[i].ListaColorPintura.length < 1) {
                    data[i].Color = "";
                    data[i].ColorPinturaID = 0;
                    data[i].SistemaPinturaColorID = 0;
                } else {
                    if (itemColor != undefined && data[i].Color == "") {
                        data[i].Color = itemColor.Nombre;
                        data[i].ColorPinturaID = itemColor.ColorPinturaID;
                        data[i].SistemaPinturaColorID = itemColor.SistemaPinturaColorID;
                    }
                }
            }
        }
    }

    $("#grid").data("kendoGrid").dataSource.sync();
}

function PlanchadoColor(tipoLlenado) {
    var ds = $("#grid").data("kendoGrid").dataSource;
    var filters = ds.filter();
    var allData = ds.data();
    var query = new kendo.data.Query(allData);
    var data = query.filter(filters).data;
    var itemColor = $("#inputColorPintura").data("kendoComboBox").dataItem($("#inputColorPintura").data("kendoComboBox").select());
    var itemSistemaPintura = $("#inputSistemaPintura").data("kendoComboBox").dataItem($("#inputSistemaPintura").data("kendoComboBox").select());

    for (var i = 0; i < data.length; i++) {
        if (tipoLlenado === "Todos") {
            if (itemSistemaPintura.NoPintable) {
                data[i].Color = "";
                data[i].ColorPinturaID = 0;
                data[i].SistemaPinturaColorID = 0;
            } else {

                data[i].Color = itemColor.Nombre;
                data[i].ColorPinturaID = itemColor.ColorPinturaID;
                data[i].SistemaPinturaColorID = itemColor.SistemaPinturaColorID;
                data[i].EstatusCaptura = 1;
            }
        }
        else if (tipoLlenado === "Vacios") {
            if (data[i].Color === "" || data[i].Color === null || data[i].Color === undefined) {
                if (itemSistemaPintura.NoPintable) {
                    data[i].Color = "";
                    data[i].ColorPinturaID = 0;
                    data[i].SistemaPinturaColorID = 0;
                } else {

                    data[i].Color = itemColor.Nombre;
                    data[i].ColorPinturaID = itemColor.ColorPinturaID;
                    data[i].SistemaPinturaColorID = itemColor.SistemaPinturaColorID;
                    data[i].EstatusCaptura = 1;
                }
            }
        }
    }

    $("#grid").data("kendoGrid").dataSource.sync();
}

function ValidaInformacionCapturada() {
    var ds = $("#grid").data("kendoGrid").dataSource;
    var filters = ds.filter();
    var allData = ds.data();
    var query = new kendo.data.Query(allData);
    var data = query.filter(filters).data;

    var contador = 0;
    if (data.length > 0) {
        for (var i = 0; i < data.length; i++) {
            if (data[i].EstatusCaptura == 1) {
                contador++;
            }
        }
        if (contador > 0) {
            return true;
        } else {
            return false;
        }
    } else {
        return false;
    }
}

function GenerarBusqueda() {
    var Proyecto = $("#inputProyecto").data("kendoComboBox").dataItem($("#inputProyecto").data("kendoComboBox").select());
    var tipoBusqueda = 0;
    var cadena = "";

    if (Proyecto != undefined && Proyecto.ProyectoID != "" && Proyecto.ProyectoID != 0) {
        if ($("#styleSpool").hasClass("active")) {
            if (!editado) {
                lineaCaptura.dato = $("#inputSpool").val().trim();

                $("#inputSpool").attr("saAttr", $("#inputSpool").val());
                if ($("#inputSpool").val() != null && $("#inputSpool").val() != "") {
                    tipoBusqueda = 1;
                    cadena = $("#inputSpool").val().trim();
                    AjaxCargarNumeroElementosPorBusqueda(Proyecto.ProyectoID, tipoBusqueda, cadena);
                } else {
                    displayNotify("SPAMensajeIngresaSpool", "", '1');
                }
            } else {
                var ventanaConfirm = $("#ventanaConfirmCaptura").kendoWindow({
                    iframe: true,
                    title: _dictionary.EntregaPlacasGraficasTituloPopup[$("#language").data("kendoDropDownList").value()],
                    visible: false,
                    width: "40%",
                    height: "auto",
                    draggable: false,
                    modal: true,
                    actions: [],
                    animation: {
                        open: false,
                        close: false
                    },
                }).data("kendoWindow");

                ventanaConfirm.content('<center>' + _dictionary.EntregaPlacasGraficasMensajeDatosCapturadosNoGuardados[$("#language").data("kendoDropDownList").value()] + '</center>' +
					"</br><center><button class='btn btn-blue' id='yesButtonProy'>" + _dictionary.lblSi[$("#language").data("kendoDropDownList").value()] + "</button> <button class='btn btn-blue' id='noButtonProy'>" + _dictionary.lblNo[$("#language").data("kendoDropDownList").value()] + "</button></center>");

                ventanaConfirm.open().center();
                $("#yesButtonProy").click(function () {
                    $("#inputSpool").attr("saAttr", $("#inputSpool").val());
                    if ($("#inputSpool").val() != null && $("#inputSpool").val() != "") {
                        tipoBusqueda = 1;
                        cadena = $("#inputSpool").val().trim();
                        AjaxCargarNumeroElementosPorBusqueda(Proyecto.ProyectoID, tipoBusqueda, cadena);
                    } else {
                        displayNotify("SPAMensajeIngresaSpool", "", '1');
                    }
                    ventanaConfirm.close();
                });
                $("#noButtonProy").click(function () {
                    $("#inputSpool").val($("#inputSpool").attr("saAttr"));
                    ventanaConfirm.close();
                });
            }

        } else if ($("#styleNc").hasClass("active")) {
            if (!editado) {
                lineaCaptura.dato = $("#inputNc").val().trim();
                $("#inputNc").attr("ncaAttr", $("#inputNc").val());
                if ($("#inputNc").val() != null && $("#inputNc").val() != "") {
                    tipoBusqueda = 2;
                    cadena = $("#inputNc").val().trim();
                    AjaxCargarNumeroElementosPorBusqueda(Proyecto.ProyectoID, tipoBusqueda, cadena);
                } else {
                    displayNotify("SPAMensajeIngresaNc", "", '1');
                }
            } else {
                var ventanaConfirm = $("#ventanaConfirmCaptura").kendoWindow({
                    iframe: true,
                    title: _dictionary.EntregaPlacasGraficasTituloPopup[$("#language").data("kendoDropDownList").value()],
                    visible: false,
                    width: "40%",
                    height: "auto",
                    draggable: false,
                    modal: true,
                    actions: [],
                    animation: {
                        open: false,
                        close: false
                    },
                }).data("kendoWindow");

                ventanaConfirm.content('<center>' + _dictionary.EntregaPlacasGraficasMensajeDatosCapturadosNoGuardados[$("#language").data("kendoDropDownList").value()] + '</center>' +
					"</br><center><button class='btn btn-blue' id='yesButtonNc'>" + _dictionary.lblSi[$("#language").data("kendoDropDownList").value()] + "</button> <button class='btn btn-blue' id='noButtonProy'>" + _dictionary.lblNo[$("#language").data("kendoDropDownList").value()] + "</button></center>");

                ventanaConfirm.open().center();

                $("#yesButtonNc").click(function () {
                    $("#inputNc").attr("ncaAttr", $("#inputNc").val());
                    if ($("#inputNc").val() != null && $("#inputNc").val() != "") {
                        tipoBusqueda = 2;
                        cadena = $("#inputNc").val().trim();
                        AjaxCargarNumeroElementosPorBusqueda(Proyecto.ProyectoID, tipoBusqueda, cadena);
                    } else {
                        displayNotify("SPAMensajeIngresaNc", "", '1');
                    }
                    ventanaConfirm.close();
                });
                $("#noButtonProy").click(function () {
                    $("#inputNc").val($("#inputNc").attr("ncaAttr"));
                    ventanaConfirm.close();
                });
            }

        }

    } else {
        displayNotify("MensajeSeleccionaProyecto", "", '1');
    }
}