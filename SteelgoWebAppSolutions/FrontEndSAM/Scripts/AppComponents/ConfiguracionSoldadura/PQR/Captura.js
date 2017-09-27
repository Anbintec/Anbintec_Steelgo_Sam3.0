var esNormal;

function changeLanguageCall() {
    document.title = _dictionary.PQRBreadcrumb[$("#language").data("kendoDropDownList").value()];
    CargarGrid();
    LlenaGridAjax();
};


function CargarGrid() {
    $("#grid").kendoGrid({
        save: function (e) {
            var focusedCellIndex = this.current()[0].cellIndex;
            var dataItem = e.model;
            var grid = this;
            nextDataItem = this.dataSource.at(this.dataSource.indexOf(dataItem) + 1);

            this.refresh();
            setTimeout(function () {
                return function () {
                    var focusedCell = $("#grid tr[data-uid='" + e.model.uid + "'] td:nth-child(" + (focusedCellIndex + 1) + ")");
                    grid.select(focusedCell);
                    grid.editCell(focusedCell);
                }
            }(), 200);
        },
        edit: function (e) {

            if ($('#Guardar').text() == _dictionary.MensajeGuardar[$("#language").data("kendoDropDownList").value()] && e.model.RegistrosWPS == 0) {

            } else {
                this.closeCell();
                if ($('#Guardar').text() != _dictionary.textoEditar[$("#language").data("kendoDropDownList").value()])
                    displayNotify("", _dictionary.lblPQRElementoPQR[$("#language").data("kendoDropDownList").value()] + e.model.Nombre + _dictionary.lblPQRYaAsignado[$("#language").data("kendoDropDownList").value()], 1);
            }

            if ($(".k-grid-content td").css("white-space") == "normal") {
                esNormal = true;
            }
            else {
                esNormal = false;
            }
        },
        dataBound: function () {
            var myElem = document.getElementById('trParentHeader');
            if (myElem == null) {
                $("#grid").find("th.k-header").parent().before("<tr id='trParentHeader'> " +
                    "<th scope='col' colspan='8' class='k-header'></th>  <th width='auto'  colspan='2' class='k-header' style='text-align: center;'><span id=''>" + _dictionary.columnEspesormm[$("#language").data("kendoDropDownList").value()] + "</span></th>" +
                    "<th width='auto'  colspan='2' class='k-header' style='text-align: center;'><span>" + _dictionary.columnProceso[$("#language").data("kendoDropDownList").value()] + "</span></th>" +
                    "<th width='auto'  colspan='2' class='k-header' style='text-align: center;'><span id=''>" + _dictionary.columnGrupoP[$("#language").data("kendoDropDownList").value()] + "</span></th>" +
                    "<th width='auto'  colspan='2' class='k-header' style='text-align: center;'><span id=''>" + _dictionary.columnAporte[$("#language").data("kendoDropDownList").value()] + "</span></th>" +
                    "<th width='auto'  colspan='2' class='k-header' style='text-align: center;'><span id=''>" + _dictionary.columnGrupoF[$("#language").data("kendoDropDownList").value()] + "</span></th>" +
                    "<th width='auto'  colspan='4' class='k-header' style='text-align: center;'><span id=''></span></th>" +
                    "</tr>");
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

        },
        autoBind: true,
        dataSource: {
            data: [],
            schema: {
                model: {
                    fields: {
                        PQRID: { type: "int" },
                        Nombre: { type: "string", editable: true },
                        PREHEAT: { type: "boolean", editable: false },
                        PWHT: { type: "boolean", editable: false },
                        CVN: { type: "boolean", editable: false },
                        FN: { type: "boolean", editable: false },
                        MacroTest: { type: "boolean", editable: false },
                        EspesorRelleno: { type: "number", editable: true },
                        EspesorRaiz: { type: "number", editable: true },
                        CodigoRelleno: { type: "string", editable: true },
                        CodigoRaiz: { type: "string", editable: true },
                        NumeroP: { type: "string", editable: true },
                        GrupoPMaterialesBase1: { type: "string", editable: true },
                        GrupoPMaterialesBase2: { type: "string", editable: true },
                        Aporte: { type: "string", editable: true },
                        Mezcla: { type: "string", editable: true },
                        Respaldo: { type: "string", editable: true },
                        GrupoF: { type: "string", editable: true },
                        CodigoASMEID: { type: "int", editable: false },
                        Especificacion: { type: "String", editable: true },

                        GrupoPMaterialesBase1ID: { type: "int" },
                        GrupoPMaterialesBase2ID: { type: "int" },
                        ProcesoSoldaduraRellenoID: { type: "int" },
                        ProcesoSoldaduraRaizID: { type: "int" },
                        NumeroPID: { type: "int" },
                        GrupoPID: { type: "int" },
                        Accion: { type: "int" },

                        RegistrosWPS: { type: "int" }
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
            serverSorting: false,
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
             { width: "110px", field: "Nombre", title: _dictionary.columnNombre[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec() },
             {
                 width: "120px", field: "PREHEAT", title: _dictionary.columnPreheat[$("#language").data("kendoDropDownList").value()], filterable: {
                     multi: true,
                     messages: {
                         isTrue: _dictionary.lblVerdadero[$("#language").data("kendoDropDownList").value()],
                         isFalse: _dictionary.lblFalso[$("#language").data("kendoDropDownList").value()],
                         style: "max-width:120px;"
                     },
                     dataSource: [{ Etiquetado: true }, { Etiquetado: false }]
                 }, template: "<input name='fullyPaid' class='chk-PREHEAT' type='checkbox' data-bind='checked: PREHEAT' #= PREHEAT ? checked='checked' : '' #/>", width: "120px", attributes: { style: "text-align:center;" }
             },
             {
                 width: "100px", field: "PWHT", title: _dictionary.columnPWHT[$("#language").data("kendoDropDownList").value()], filterable: {
                     multi: true,
                     messages: {
                         isTrue: _dictionary.lblVerdadero[$("#language").data("kendoDropDownList").value()],
                         isFalse: _dictionary.lblFalso[$("#language").data("kendoDropDownList").value()],
                         style: "max-width:100px;"
                     },
                     dataSource: [{ Etiquetado: true }, { Etiquetado: false }]
                 }, template: "<input name='fullyPaid' class='chk-PWHT' type='checkbox' data-bind='checked: PWHT' #= PWHT ? checked='checked' : '' #/>", width: "111px", attributes: { style: "text-align:center;" }
             },
             {
                 width: "100px", field: "CVN", title: _dictionary.columnCVN[$("#language").data("kendoDropDownList").value()], filterable: {
                     multi: true,
                     messages: {
                         isTrue: _dictionary.lblVerdadero[$("#language").data("kendoDropDownList").value()],
                         isFalse: _dictionary.lblFalso[$("#language").data("kendoDropDownList").value()],
                         style: "max-width:100px;"
                     },
                     dataSource: [{ Etiquetado: true }, { Etiquetado: false }]
                 }, template: "<input name='fullyPaid' class='chk-CVN' type='checkbox' data-bind='checked: CVN' #= CVN ? checked='checked' : '' #/>", width: "111px", attributes: { style: "text-align:center;" }
             },
             {
                 width: "100px", field: "FN", title: _dictionary.columnFN[$("#language").data("kendoDropDownList").value()], filterable: {
                     multi: true,
                     messages: {
                         isTrue: _dictionary.lblVerdadero[$("#language").data("kendoDropDownList").value()],
                         isFalse: _dictionary.lblFalso[$("#language").data("kendoDropDownList").value()],
                         style: "max-width:100px;"
                     },
                     dataSource: [{ Etiquetado: true }, { Etiquetado: false }]
                 }, template: "<input name='fullyPaid' class='chk-FN' type='checkbox' data-bind='checked: FN' #= FN ? checked='checked' : '' #/>", width: "111px", attributes: { style: "text-align:center;" }
             },
             {
                 width: "100px", field: "MacroTest", title: _dictionary.columnMacroTest[$("#language").data("kendoDropDownList").value()], filterable: {
                     multi: true,
                     messages: {
                         isTrue: _dictionary.lblVerdadero[$("#language").data("kendoDropDownList").value()],
                         isFalse: _dictionary.lblFalso[$("#language").data("kendoDropDownList").value()],
                         style: "max-width:100px;"
                     },
                     dataSource: [{ Etiquetado: true }, { Etiquetado: false }]
                 }, template: "<input name='fullyPaid' class='chk-MacroTest' type='checkbox' data-bind='checked: MacroTest' #= MacroTest ? checked='checked' : '' #/>", width: "111px", attributes: { style: "text-align:center;" }
             },
             { width: "120px", field: "TipoPrueba", title: _dictionary.columnTipoPrueba[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), editor: RenderComboBoxTipoPrueba },
             { width: "120px", field: "TipoJunta", title: _dictionary.DimensionalVisualHeaderTipoJunta[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), editor: RenderComboBoxTipoJunta },
             { width: "120px", field: "EspesorRelleno", title: _dictionary.columnRelleno[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellNumberMaftec(), format: "{0}", editor: RenderEspesorRelleno, attributes: { style: "text-align:right;" } },
             { width: "120px", field: "EspesorRaiz", title: _dictionary.columnRaiz[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellNumberMaftec(), format: "{0}", editor: RenderEspesorRaiz, attributes: { style: "text-align:right;" } },
             { width: "120px", field: "CodigoRelleno", title: _dictionary.columnRelleno[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "130px", editor: RenderComboBoxProcesoSoldaduraRelleno },
             { width: "120px", field: "CodigoRaiz", title: _dictionary.columnRaiz[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), editor: RenderComboBoxProcesoSoldaduraRaiz },
             { width: "120px", field: "GrupoPMaterialBase1Nombre", title: _dictionary.columnBase1[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), editor: RenderComboBoxMaterialesBase1 },
             { width: "120px", field: "GrupoPMaterialBase2Nombre", title: _dictionary.columnBase2[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), editor: RenderComboBoxMaterialesBase2 },
             { width: "120px", field: "Aporte", title: _dictionary.columnRaiz[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec() },
             { width: "120px", field: "AporteRelleno", title: _dictionary.columnRelleno[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec() },
             { width: "120px", field: "GrupoF", title: _dictionary.columnRaiz[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec() },
             { width: "120px", field: "GrupoFRelleno", title: _dictionary.columnRelleno[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec() },
             { width: "130px", field: "Mezcla", title: _dictionary.columnMezcla[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec() },
             { width: "130px", field: "Respaldo", title: _dictionary.columnRespaldo[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec() },
             { width: "200px", field: "Especificacion", title: _dictionary.columnCodigo[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), editor: RenderComboBoxEspecificacion },
             { command: { text: _dictionary.botonCancelar[$("#language").data("kendoDropDownList").value()], click: cancelarCaptura }, width: "50px", title: _dictionary.columnELM[$("#language").data("kendoDropDownList").value()], attributes: { style: "text-align:center;" } }
        ],
        editable: true,
        navigatable: true

    });
    

    $("#grid .k-grid-content").on("change", "input.chk-PWHT", function (e) {
        
        if ($('#Guardar').text() != _dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]) {
                var grid = $("#grid").data("kendoGrid")
                dataItem = grid.dataItem($(e.target).closest("tr"));
                if (dataItem.RegistrosWPS == 0) {
                    if ($(this)[0].checked) {
                        dataItem.PWHT = true;
                    }
                    else {
                        dataItem.PWHT = false;
                    }
                    $("#grid").data("kendoGrid").dataSource.sync();
                }
                else {
                    displayNotify("", _dictionary.lblPQRElementoPQR[$("#language").data("kendoDropDownList").value()] + dataItem.Nombre + _dictionary.lblPQRYaAsignado[$("#language").data("kendoDropDownList").value()], 1);
                    if ($(this)[0].checked) {
                        dataItem.PWHT = false;
                    }
                    else {
                        dataItem.PWHT = true;
                    }
                    $("#grid").data("kendoGrid").dataSource.sync();

                }
            }
            else {
                if ($(this)[0].checked) {
                    $(this)[0].checked = false;
                }
                else {
                    $(this)[0].checked = true;
                }
            }
    });

    $("#grid .k-grid-content").on("change", "input.chk-PREHEAT", function (e) {
            if ($('#Guardar').text() != _dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]) {
                var grid = $("#grid").data("kendoGrid")
                dataItem = grid.dataItem($(e.target).closest("tr"));
                if (dataItem.RegistrosWPS == 0) {
                    if ($(this)[0].checked) {
                        dataItem.PREHEAT = true;
                    }
                    else {
                        dataItem.PREHEAT = false;
                    }
                    $("#grid").data("kendoGrid").dataSource.sync();
                }
                else {
                    displayNotify("", _dictionary.lblPQRElementoPQR[$("#language").data("kendoDropDownList").value()] + dataItem.Nombre + _dictionary.lblPQRYaAsignado[$("#language").data("kendoDropDownList").value()], 1);
                    if ($(this)[0].checked) {
                        dataItem.PREHEAT = false;
                    }
                    else {
                        dataItem.PREHEAT = true;
                    }
                    $("#grid").data("kendoGrid").dataSource.sync();

                }
            }
            else {
                if ($(this)[0].checked) {
                    $(this)[0].checked = false;
                }
                else {
                    $(this)[0].checked = true;
                }
            }
      
    });
    $("#grid .k-grid-content").on("change", "input.chk-CVN", function (e) {

        if ($('#Guardar').text() != _dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]) {
            var grid = $("#grid").data("kendoGrid")
            dataItem = grid.dataItem($(e.target).closest("tr"));
            if (dataItem.RegistrosWPS == 0) {
                if ($(this)[0].checked) {
                    dataItem.CVN = true;
                }
                else {
                    dataItem.CVN = false;
                }
                $("#grid").data("kendoGrid").dataSource.sync();
            }
            else {
                displayNotify("", _dictionary.lblPQRElementoPQR[$("#language").data("kendoDropDownList").value()] + dataItem.Nombre + _dictionary.lblPQRYaAsignado[$("#language").data("kendoDropDownList").value()], 1);
                if ($(this)[0].checked) {
                    dataItem.CVN = false;
                }
                else {
                    dataItem.CVN = true;
                }
                $("#grid").data("kendoGrid").dataSource.sync();
            }
        }
        else {
            if ($(this)[0].checked) {
                $(this)[0].checked = false;
            }
            else {
                $(this)[0].checked = true;
            }
        }
    });

    $("#grid .k-grid-content").on("change", "input.chk-FN", function (e) {

        if ($('#Guardar').text() != _dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]) {
            var grid = $("#grid").data("kendoGrid")
            dataItem = grid.dataItem($(e.target).closest("tr"));
            if (dataItem.RegistrosWPS == 0) {
                if ($(this)[0].checked) {
                    dataItem.FN = true;
                }
                else {
                    dataItem.FN = false;
                }
                $("#grid").data("kendoGrid").dataSource.sync();
            }
            else {
                displayNotify("", _dictionary.lblPQRElementoPQR[$("#language").data("kendoDropDownList").value()] + dataItem.Nombre + _dictionary.lblPQRYaAsignado[$("#language").data("kendoDropDownList").value()], 1);
                if ($(this)[0].checked) {
                    dataItem.FN = false;
                }
                else {
                    dataItem.FN = true;
                }
                $("#grid").data("kendoGrid").dataSource.sync();
            }
        }
        else {
            if ($(this)[0].checked) {
                $(this)[0].checked = false;
            }
            else {
                $(this)[0].checked = true;
            }
        }
    });

    $("#grid .k-grid-content").on("change", "input.chk-MacroTest", function (e) {

        if ($('#Guardar').text() != _dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]) {
            var grid = $("#grid").data("kendoGrid")
            dataItem = grid.dataItem($(e.target).closest("tr"));
            if (dataItem.RegistrosWPS == 0) {
                if ($(this)[0].checked) {
                    dataItem.MacroTest = true;
                }
                else {
                    dataItem.MacroTest = false;
                }
                $("#grid").data("kendoGrid").dataSource.sync();
            }
            else {
                displayNotify("", _dictionary.lblPQRElementoPQR[$("#language").data("kendoDropDownList").value()] + dataItem.Nombre + _dictionary.lblPQRYaAsignado[$("#language").data("kendoDropDownList").value()], 1);
                if ($(this)[0].checked) {
                    dataItem.MacroTest = false;
                }
                else {
                    dataItem.MacroTest = true;
                }
                $("#grid").data("kendoGrid").dataSource.sync();
            }
        }
        else {
            if ($(this)[0].checked) {
                $(this)[0].checked = false;
            }
            else {
                $(this)[0].checked = true;
            }
        }
    });



    CustomisaGrid($("#grid"));
};

function NombreRepetido(listaDetalles) {
    var dataSource = $("#grid").data("kendoGrid").dataSource;
    var allData = dataSource.data();

    for (var i = 0; i < listaDetalles.length; i++) {
        for (var j = 0; j < listaDetalles.length; j++) {
            if (listaDetalles[i].Nombre.toLowerCase() == listaDetalles[j].Nombre.toLowerCase() && i != j) {
                listaDetalles[j].Estatus = -1;
                //$('tr[data-uid="' + allData[j].uid + '"] ').css("background-color", "#ffcccc");
                $("#grid").data("kendoGrid").dataSource._data[j].RowOk = false;
            }
        }
    }

    return ExistNombreRepetido(listaDetalles);
}

function ExistNombreRepetido(rows) {
    for (var i = 0; i < rows.length; i++) {
        if (rows[i].Estatus == -1)
            return true;
    }
    return false;
}

function VentanaModal() {
    var modalTitle = "";
    modalTitle = "PQR";
    var window = $("#windowPQR");
    var win = window.kendoWindow({
        actions: "",
        modal: true,
        title: modalTitle,
        resizable: false,
        visible: false,
        width: "50%%",
        minWidth: 660,
        position: {
            top: "10%",
            left: "20%"
        }
    }).data("kendoWindow");
    window.data("kendoWindow").title(modalTitle);
    window.data("kendoWindow").center().open();

};

//function LimpiaControles() {
//    $('#NombreId').val('');
//    $('#EspesorRelleno').val('');
//    $('#EspesorRaiz').val('');
//    $('#chkPreheat').prop('checked', false);
//    $('#chkPwht').prop('checked', false);

//    $('#ProcesoSoldaduraRellenoID').data("kendoComboBox").value("");
//    $('#ProcesoSoldaduraRaizID').data("kendoComboBox").value("");
//    $('#NumeroPID').data("kendoComboBox").value("");
//    $('#GrupoPMaterialBase1ID').data("kendoComboBox").value("");
//    $('#AporteID').data("kendoComboBox").value("");
//    $('#MezclaID').data("kendoComboBox").value("");
//    $('#RespaldoID').data("kendoComboBox").value("");
//    $('#GrupoFID').data("kendoComboBox").value("");
//    $('#CodigoID').data("kendoComboBox").value("");
//};

function cancelarCaptura(e) {
    e.preventDefault();
    if ($('#Guardar').text() == _dictionary.MensajeGuardar[$("#language").data("kendoDropDownList").value()]) {
        e.preventDefault();
        var dataItem = $("#grid").data("kendoGrid").dataItem($(e.currentTarget).closest("tr"));
        var PQRIDRegistro = dataItem.PQRID;

        if (dataItem.RegistrosWPS == 0) {
            windowTemplate = kendo.template($("#windowTemplate").html());

            ventanaConfirm = $("#ventanaConfirm").kendoWindow({
                iframe: true,
                title: _dictionary.CapturaAvanceTitulo[$("#language").data("kendoDropDownList").value()],
                visible: false, //the window will not appear before its .open method is called
                width: "auto",
                height: "auto",
                modal: true,
                animation: {
                    close: false,
                    open: false
                }
            }).data("kendoWindow");

            ventanaConfirm.content(_dictionary.lblPQRCapturaBorradoCaptura[$("#language").data("kendoDropDownList").value()] +
                         "</br><center><button class='confirm_yes btn btn-blue' id='yesButton'>Si</button><button class='confirm_yes btn btn-blue' id='noButton'> No</button></center>");

            ventanaConfirm.open().center();

            $("#yesButton").click(function () {

                var dataSource = $("#grid").data("kendoGrid").dataSource;
                dataItem.Accion = 3;
                $("#grid").data("kendoGrid").dataSource.sync();

                ventanaConfirm.close();
            });
            $("#noButton").click(function () {
                ventanaConfirm.close();
            });
        }
        else
            displayNotify("", _dictionary.lblPQRElementoPQR[$("#language").data("kendoDropDownList").value()] + dataItem.Nombre + _dictionary.lblPQRYaAsignado[$("#language").data("kendoDropDownList").value()], 1);
    }

};

function LLenaControles(e) {
    var dataItem = $("#grid").data("kendoGrid").dataItem($(e.currentTarget).closest("tr"));

    $("#IdPQR").val(dataItem.PQRID)

    $("#NombreId").val(dataItem.Nombre)

    var ChkPreheat = dataItem.PREHEAT;
    if (ChkPreheat == true) {

        var data = kendo.observable({
            optionCheck: true
        });
        kendo.bind($("#chkPreheat"), data);
    }
    else {

        var data = kendo.observable({
            optionCheck: false
        });
        kendo.bind($("#chkPreheat"), data);
    }

    $("#EspesorRelleno").val(dataItem.EspesorRelleno);
    $("#EspesorRaiz").val(dataItem.EspesorRaiz);

    var ChkPWHT = dataItem.PWHT;
    if (ChkPWHT == true) {

        var data = kendo.observable({
            optionCheckPWHT: true
        });
        kendo.bind($("#chkPwht"), data);
    }
    else {

        var data = kendo.observable({
            optionCheckPWHT: false
        });
        kendo.bind($("#chkPwht"), data);
    }

    var CMBProcesoSoldaduraRelleno = $("#ProcesoSoldaduraRellenoID").data("kendoComboBox");
    CMBProcesoSoldaduraRelleno.value(dataItem.ProcesoSoldaduraRellenoID);


    var CMBProcesoSoldaduraRAIZ = $("#ProcesoSoldaduraRaizID").data("kendoComboBox");
    CMBProcesoSoldaduraRAIZ.value(dataItem.ProcesoSoldaduraRaizID);


    //var CMBNumeroP = $("#NumeroPID").data("kendoComboBox");
    //CMBNumeroP.value(dataItem.NumeroPID);

    var GrupoPMaterialBase1ID = $("#GrupoPMaterialBase1ID").data("kendoComboBox");
    GrupoPMaterialBase1ID.value(dataItem.GrupoMaterialBase1PID);

    var GrupoPMaterialBase2ID = $("#GrupoPMaterialBase12D").data("kendoComboBox");
    GrupoPMaterialBase2ID.value(dataItem.GrupoMaterialBase2PID);

    var CMBAporte = $("#AporteID").data("kendoComboBox");
    CMBAporte.value(dataItem.AporteID);

    var CMBMezcla = $("#MezclaID").data("kendoComboBox");
    CMBMezcla.value(dataItem.MezclaID);

    var CMBRespaldo = $("#RespaldoID").data("kendoComboBox");
    CMBRespaldo.value(dataItem.RespaldoID);

    var CMBGrupoFID = $("#GrupoFID").data("kendoComboBox");
    CMBGrupoFID.value(dataItem.GrupoFID)

    var CMBCodigoID = $("#CodigoID").data("kendoComboBox");
    CMBCodigoID.value(dataItem.CodigoID)

};
