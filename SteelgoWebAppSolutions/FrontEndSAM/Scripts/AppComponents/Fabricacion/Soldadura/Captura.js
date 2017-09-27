var modeloRenglon;
var editado = false;
var endRangeDate;
var esNormal;


IniciarCapturaSoldadura();
function IniciarCapturaSoldadura() {
    SuscribirFechaSoldadura();
    asignarProyecto();
    setTimeout(function () { SuscribirEventos() }, 100);
}

function changeLanguageCall() {
    editado = false;
    endRangeDate.data("kendoDatePicker").setOptions({
        format: _dictionary.FormatoFecha2[$("#language").data("kendoDropDownList").value()]
    });
    AjaxCargarCamposPredeterminados();
    CargarGrid();
    CargarGridPopUp();
    CargarGridPopupSoldadoresRaizCapturados();
    CargarGridPopupSoldadoresRellenoCapturados();
    suscribirEventoWindowsConfirmaCaptura();
    Limpiar();
    opcionHabilitarView(false, "FieldSetView")
    document.title = _dictionary.CapturaSoldaduraSoldaduraSpool[$("#language").data("kendoDropDownList").value()];

};



function asignarProyecto() {
    //$("#InputOrdenTrabajo").val(Cookies.get('LetraProyecto') == undefined ? '' : Cookies.get('LetraProyecto'));
    //$("#LabelProyecto").text('Proyecto :' + (Cookies.get('Proyecto') == undefined ? 'No hay ningun proyecto' : Cookies.get('Proyecto')));
}


function CargarGrid() {

    $("#grid").kendoGrid({
        autoBind: false,
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

            var inputName = e.container.find('input');
            inputName.select();
            editado = true;
            if ($('#botonGuardar').text() != _dictionary.lblGuardar[$("#language").data("kendoDropDownList").value()]) {
                this.closeCell();
            }

            if ($(".k-grid-content td").css("white-space") == "normal") {
                esNormal = true;
            }
            else {
                esNormal = false;
            }
        },
        dataSource: {
            data: '',
            schema: {
                model: {
                    fields: {
                        SpoolID: { type: "string", editable: false },
                        Junta: { type: "string", editable: false },
                        DetalleJunta: { type: "string", editable: false },
                        Taller: { type: "string", editable: true },
                        Diametro: { type: "number", editable: false },
                        FechaSoldadura: { type: "date", editable: true },
                        ProcesoSoldaduraRaiz: { type: "string", editable: true },
                        TemplateSoldadoresRaiz: { type: "string", editable: false },
                        ProcesoSoldaduraRelleno: { type: "string", editable: true },
                        TemplateSoldadoresRelleno: { type: "string", editable: false },
                        WPSNombre: { type: "string", editable: true },
                        DetalleAdicional: { type: "string", editable: false },
                        RequierePwht: { type: "boolean", editable: false }
                    }
                }
            },
            filter: {
                logic: "or",
                filters: [
                  { field: "Accion", operator: "eq", value: 1 },
                  { field: "Accion", operator: "eq", value: 2 },
                  { field: "Accion", operator: "eq", value: 4 },
                  { field: "Accion", operator: "eq", value: undefined }
                ]
            },
            pageSize: 10,
            serverPaging: false,
            serverFiltering: false,
            serverSorting: false
        },
        //groupable: true,
        navigatable: true,
        editable: true,
        filterable: getGridFilterableMaftec(),
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
        columns: [
            { field: "SpoolID", title: _dictionary.columnNumeroControl[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "100px" },
            { field: "Junta", title: _dictionary.columnJunta[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "70px" },
            { field: "DetalleJunta", title: _dictionary.columnDetalleJunta[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "180px" },
            {
                field: "RequierePwht", title: _dictionary.columnPWHT[$("#language").data("kendoDropDownList").value()], filterable: {
                    multi: true,
                    messages: {
                        isTrue: _dictionary.lblVerdadero[$("#language").data("kendoDropDownList").value()],
                        isFalse: _dictionary.lblFalso[$("#language").data("kendoDropDownList").value()],
                        style: "max-width:120px;"
                    },
                }, width: "90px", template: "<input  readonly disabled type='checkbox'   #= RequierePwht  ? checked='checked' : '' #/>", attributes: { style: "text-align:center;" }
            },
            { field: "Diametro", title: _dictionary.columnDiametro[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellNumberMaftec(), width: "80px", attributes: { style: "text-align:right;" } },
            { field: "FechaSoldadura", title: _dictionary.columnFecha[$("#language").data("kendoDropDownList").value()], filterable: getKendoGridFilterableDateMaftec(), editor: RenderDatePicker, width: "110px", format: _dictionary.FormatoFecha[$("#language").data("kendoDropDownList").value()] },
            { field: "Taller", title: _dictionary.columnTaller[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), editor: RenderComboBoxTaller, width: "130px" },
            { field: "ProcesoSoldaduraRaiz", title: _dictionary.columnProcesoRaiz[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "140px", editor: RenderComboBoxProcesoSoldaduraRaiz },
            { field: "ProcesoSoldaduraRelleno", title: _dictionary.columnProcesoRelleno[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "140px", editor: RenderComboBoxProcesoSoldaduraRelleno },
            { field: "WPSNombre", title: _dictionary.columnWPS[$("#language").data("kendoDropDownList").value()], editor: RenderComboBoxWPS, filterable: getGridFilterableCellMaftec(), width: "130px" },
            { field: "TemplateSoldadoresRaiz", title: _dictionary.columnSoldadoresRaiz[$("#language").data("kendoDropDownList").value()], filterable: false, width: "150px", template: "<div class='botonSoldadoresRaiz'><a href='\\#'  > <span>#=TemplateSoldadoresRaiz#</span></a></div>" },
            { field: "TemplateSoldadoresRelleno", title: _dictionary.columnSoldadoresRelleno[$("#language").data("kendoDropDownList").value()], filterable: false, width: "150px", template: "<div class='botonSoldadoresRelleno'><a href='\\#' > <span>#=TemplateSoldadoresRelleno#</span></a></div>" },
            { field: "DetalleAdicional", title: _dictionary.columnDetalleAdicional[$("#language").data("kendoDropDownList").value()], filterable: false, width: "150px", template: "<div class='botonAdicionales'><a href='\\#' > <span>#=TemplateTrabajosAdicionales#</span></a></div>" },
            { command: { text: _dictionary.botonCancelar[$("#language").data("kendoDropDownList").value()], click: cancelarCaptura }, filterable: false, title: _dictionary.columnELM[$("#language").data("kendoDropDownList").value()], width: "50px", attributes: { style: "text-align:center;" } },
            { command: { text: _dictionary.botonLimpiar[$("#language").data("kendoDropDownList").value()], click: limpiarRenglon }, filterable: false, title: _dictionary.columnLimpiar[$("#language").data("kendoDropDownList").value()], width: "50px", attributes: { style: "text-align:center;" } }
        ],
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

            if (esNormal) {
                $(".k-grid-content td").css("white-space", "normal");
            }
            else {
                $(".k-grid-content td").css("white-space", "nowrap");
            }

        }
    });

    CustomisaGrid($("#grid"));
};


function opcionHabilitarView(valor, name) {

    if (valor) {
        $('#FieldSetView').find('*').attr('disabled', true);
        $('#planchadoZone').find('*').attr('disabled', true);
        $("#InputID").data("kendoComboBox").enable(false);
        $("#Junta").data("kendoComboBox").enable(false);
        $("#inputTaller").data("kendoComboBox").enable(false);
        $('#InputOrdenTrabajo').css('opacity', '0.8');

        $("#FechaSoldadura").data("kendoDatePicker").enable(false);
        $('#botonGuardar').text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);
        $("#DetalleAvisoLlegada0017").text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);
        $('#ButtonAplicar').attr("disabled", true);
        $('#botonGuardar2').text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);
        $("#botonGuardar1").text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);
        $('#botonGuardar3').text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);

    }
    else {
        $('#InputOrdenTrabajo').css('opacity', '1');
        $('#FieldSetView').find('*').attr('disabled', false);
        $('#planchadoZone').find('*').attr('disabled', false);
        $("#InputID").data("kendoComboBox").enable(true);
        $("#inputTaller").data("kendoComboBox").enable(true);
        $("#FechaSoldadura").data("kendoDatePicker").enable(true);
        $("#Junta").data("kendoComboBox").enable(true);
        $('#botonGuardar').text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
        $("#DetalleAvisoLlegada0017").text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
        $('#ButtonAplicar').attr("disabled", false);
        $('#btnGuardarPiePagina').text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
        $('#botonGuardar2').text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
        $('#botonGuardar3').text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
        $("#botonGuardar1").text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
    }
}

function Limpiar() {
    $("#InputOrdenTrabajo").val("");
    $("#InputID").data("kendoComboBox").value("");
    $("#Junta").data("kendoComboBox").value("");//.dataSource.data([]);
    $("#FechaSoldadura").data("kendoDatePicker").value("");
    $("#inputTaller").data("kendoComboBox").value("");
    $("#grid").data('kendoGrid').dataSource.data([]);


    $('#InputOrdenTrabajo').css('opacity', '1');
    $('#FieldSetView').find('*').attr('disabled', false);
    $('#planchadoZone').find('*').attr('disabled', false);
    $("#InputID").data("kendoComboBox").enable(true);
    $("#inputTaller").data("kendoComboBox").enable(true);
    $("#FechaSoldadura").data("kendoDatePicker").enable(true);
    $("#Junta").data("kendoComboBox").enable(true);
    $('#botonGuardar').text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
    $("#DetalleAvisoLlegada0017").text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
    $('#ButtonAplicar').attr("disabled", false);
    $('#btnGuardarPiePagina').text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
    $('#botonGuardar2').text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
    $('#botonGuardar3').text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
    $("#botonGuardar1").text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);

}


function cancelarCaptura(e) {
    e.preventDefault();
    if ($('#botonGuardar').text() == _dictionary.MensajeGuardar[$("#language").data("kendoDropDownList").value()]) {

        e.preventDefault();
        var dataItem = $("#grid").data("kendoGrid").dataItem($(e.currentTarget).closest("tr"));
        var spoolIDRegistro = dataItem.SpoolID;

        var dataSource = $("#grid").data("kendoGrid").dataSource;

        dataSource.remove(dataItem);
        $("#grid").data("kendoGrid").dataSource.sync();

    }

};


function limpiarRenglon(e) {
    e.preventDefault();
    if ($('#botonGuardar').text() == _dictionary.DetalleAvisoLlegada0017[$("#language").data("kendoDropDownList").value()]) {
        var itemToClean = $("#grid").data("kendoGrid").dataItem($(e.currentTarget).closest("tr"));
        itemToClean.Raiz = [];
        itemToClean.Taller = "";
        itemToClean.WPSNombre = "";
        itemToClean.Relleno = [];
        itemToClean.TallerID = 0;
        itemToClean.WPSID = 0;
        itemToClean.FechaSoldadura = "";
        itemToClean.ProcesoSoldaduraRaiz = "";
        itemToClean.ProcesoSoldaduraRaizID = 0;
        itemToClean.ProcesoSoldaduraRelleno = "";
        itemToClean.ProcesoSoldaduraRellenoID = 0;
        itemToClean.ListaDetalleTrabajoAdicional = [];
        itemToClean.ListaSoldadoresRellenoCapturados = [];
        itemToClean.ListaSoldadoresRaizCapturados = [];
        itemToClean.TemplateSoldadoresRaiz = _dictionary.CapturaArmadoTemplateNoHaySoldadoresRaiz[$("#language").data("kendoDropDownList").value()];
        itemToClean.TemplateSoldadoresRelleno = _dictionary.CapturaArmadoTemplateNoHaySoldadoresRelleno[$("#language").data("kendoDropDownList").value()];
        itemToClean.TemplateTrabajosAdicionales = _dictionary.CapturaArmadoTemplateNoHayTrabajosAdicionales[$("#language").data("kendoDropDownList").value()];
        var dataSource = $("#grid").data("kendoGrid").dataSource;
        dataSource.sync();
    }

}

function CambioTipoListado() {
    if ($('input:radio[name=TipoAgregado]:checked').val() == "Reporte") {
        $("#JuntaDiv").css('display', 'none');
        $("#MuestraDiv").css('display', 'block');

        AjaxObtenerListaTaller();
    }
    else if ($('input:radio[name=TipoAgregado]:checked').val() == "Listado") {
        $("#JuntaDiv").css('display', 'block');
        $("#MuestraDiv").css('display', 'block');

        AjaxObtenerListaTaller();
    }
}


function FiltroMostrar(mostrar) {
    var ds = $("#grid").data("kendoGrid").dataSource;

    if (mostrar == 0) {
        var curr_filters = ds.filter().filters;
        if (curr_filters[0].filters != undefined)
            ds.filter(curr_filters[0].filters[0])
        else
            ds.filter(curr_filters[0])
        ds.sync();
    }
    else {

        var curr_filters = ds.filter().filters;
        ds.filter(curr_filters[0])
        ds.sync();
        var filters = ds.filter();
        filters.logic = "or"

        filters.filters.push({ field: "Accion", operator: "eq", value: 2 });
        filters.filters.push({ field: "Accion", operator: "eq", value: 4 });
        ds.sync();
    }
}

function tieneClase(item) {
    for (var i = 0; i < item.classList.length; i++) {
        if (item.classList[i] == "k-state-border-up" || item.classList[i] == "k-state-border-down") {
            return true;
        }
    }
    return false
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


function ArregloListadoReporte() {
    JsonCaptura = [];
    var lista = $("#Junta").data("kendoComboBox").dataSource._data;

    for (var i = 0; i < lista.length ; i++) {
        JsonCaptura[i] = { IDProyecto: "", Proyecto: "", IdOrdenTrabajo: "", OrdenTrabajo: "", idVal: "", idText: "", SpoolID: "", JuntaID: "", Junta: "", FechaSoldadura: "", tallerID: "", Taller: "", sinCaptura: "" };
        JsonCaptura[i].IDProyecto = $("#InputID").data("kendoComboBox").dataItem($("#InputID").data("kendoComboBox").select()).ProyectoID == undefined ? 0 : $("#InputID").data("kendoComboBox").dataItem($("#InputID").data("kendoComboBox").select()).ProyectoID;
        JsonCaptura[i].Proyecto = $("#InputID").data("kendoComboBox").dataItem($("#InputID").data("kendoComboBox").select()).Proyecto;
        JsonCaptura[i].IdOrdenTrabajo = $("#InputOrdenTrabajo").val();
        JsonCaptura[i].OrdenTrabajo = $("#InputOrdenTrabajo").val();
        JsonCaptura[i].idVal = $("#InputID").val();
        JsonCaptura[i].idText = $("#InputID").data("kendoComboBox").text()
        JsonCaptura[i].SpoolID = $("#InputOrdenTrabajo").val() + '-' + $("#InputID").val();
        JsonCaptura[i].JuntaID = lista[i].JuntaSpoolID;
        JsonCaptura[i].Junta = lista[i].Etiqueta;
        JsonCaptura[i].FechaSoldadura = $("#FechaSoldadura").val();
        JsonCaptura[i].sinCaptura = "Todos";
    }
    return JsonCaptura;
};

function ExisteJuntaEnSpool(Row) {

    var jsonGridArmado = $("#grid").data("kendoGrid").dataSource._data;

    for (var i = 0; i < jsonGridArmado.length; i++) {
        if (jsonGridArmado[i].IdOrdenTrabajo + '-' + jsonGridArmado[i].IdVal == (Row.IdOrdenTrabajo + '-' + Row.IdVal) && jsonGridArmado[i].JuntaID === Row.JuntaID) {
            return true;
        }
    }
    return false;
}


function CargarGridPopupSoldadoresRaizCapturados() {


    $("#inputSoldadoresRaiz").kendoGrid({
        dataSource: {
            data: [],
            schema: {
                model: {
                    fields: {
                        Accion: { type: "int", editable: false },
                        ObreroID: { type: "int", editable: false },
                        Soldador: { type: "string", editable: true },
                        ColadaID: { type: "int", editable: false },
                        Colada: { type: "string", editable: true },
                        Observaciones: { type: "string", editable: true }
                    }
                }
            }, filter: {
                logic: "or",
                filters: [
                  { field: "Accion", operator: "eq", value: 1 },
                  { field: "Accion", operator: "eq", value: 2 },
                    { field: "Accion", operator: "eq", value: 0 },
                    { field: "Accion", operator: "eq", value: undefined }
                ]
            }
        },
        selectable: true,
        filterable: getGridFilterableMaftec(),
        columns: [
          { field: "Soldador", title: _dictionary.CapturaSoldaduraHeaderSoldadores[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftecpopUp(), editor: RenderComboBoxSoldadorRaiz, width: "100px" },
          { field: "Colada", title: _dictionary.ListadoCatalogos0046[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftecpopUp(), editor: RenderComboBoxColada, width: "100px" },
          //{ field: "Observaciones", title: _dictionary.CapturaSoldaduraHeaderObservacion[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftecpopUp(), width: "100px" },
        {
            command: {
                name: "",
                title: _dictionary.columnELM[$("#language").data("kendoDropDownList").value()],
                text: _dictionary.botonCancelar[$("#language").data("kendoDropDownList").value()],
                click: function (e) {
                    e.preventDefault();
                    //modeloRenglon.DetalleAdicional.length;
                    var dataSource = this.dataSource;
                    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
                    if (buscarSoldadoresTrabajoAdicional(dataItem.ObreroID, modeloRenglon.ListaDetalleTrabajoAdicional)) {
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

                        ventanaConfirm.content(_dictionary.CapturaSoldaduraTrabajoAdicionalSeraBorrado[$("#language").data("kendoDropDownList").value()] +
                                     "</br><center><button class='confirm_yes btn btn-blue' id='yesButton'>Si</button><button class='confirm_yes btn btn-blue' id='noButton'> No</button></center>");

                        ventanaConfirm.open().center();

                        $("#yesButton").click(function () {

                            if (dataItem != null) {
                                if (dataItem.Accion == 1 || dataItem.Accion == undefined)
                                    dataSource.remove(dataItem);
                                else
                                    dataItem.Accion = 3;
                            }
                            eliminaSoldadoresTrabajoAdicional(dataItem.ObreroID, modeloRenglon.ListaDetalleTrabajoAdicional);

                            var cont = 0;
                            for (var i = 0; i < modeloRenglon.ListaDetalleTrabajoAdicional; i++) {
                                if (modeloRenglon.ListaDetalleTrabajoAdicional[i].Accion == 1 || modeloRenglon.ListaDetalleTrabajoAdicional[i].Accion == 2)
                                    cont++;
                            }

                            actuallongitudTrabajosAdicionales = cont;
                            if (actuallongitudTrabajosAdicionales == 0)
                                modeloRenglon.TemplateTrabajosAdicionales = _dictionary.CapturaArmadoTemplateNoHayTrabajosAdicionales[$("#language").data("kendoDropDownList").value()];
                            else
                                modeloRenglon.TemplateTrabajosAdicionales = _dictionary.CapturaSoldaduraMensajeCambioLongitud[$("#language").data("kendoDropDownList").value()] + actuallongitudTrabajosAdicionales + _dictionary.CapturaSoldaduraMensajeCambioTrabajosAdicionales[$("#language").data("kendoDropDownList").value()];


                            dataSource.sync();

                            ventanaConfirm.close();
                        });
                        $("#noButton").click(function () {
                            ventanaConfirm.close();
                        });
                    }
                    else {
                        if (dataItem != null) {
                            if (dataItem.Accion == 1 || dataItem.Accion == undefined)
                                dataSource.remove(dataItem);
                            else
                                dataItem.Accion = 3;
                        }
                        dataSource.sync();
                    }
                }
            }, width: "50px", attributes: { style: "text-align:center;" }, title: _dictionary.columnELM[$("#language").data("kendoDropDownList").value()]
        },
        ],
        editable: true,
        navigatable: true,
        toolbar: [{ name: "create" }]
    });
    CustomisaGrid($("#inputSoldadoresRaiz"));


    $("#inputSoldadoresRaiz table").on("keydown", function (e) {
        if (e.keyCode == 13) {
            e.preventDefault();
            //var grid = $("#gridPopUp").data("kendoGrid");
            //var length = grid.dataSource.view().length;
            //var currentIndexRow = $("#gridPopUp").data("kendoGrid").items().index($("#gridPopUp").data("kendoGrid").select());
            //if (currentIndexRow == length - 1) {
            var dataSource = $("#inputSoldadoresRaiz").data("kendoGrid").dataSource;
            var total = $("#inputSoldadoresRaiz").data("kendoGrid").dataSource.data().length;
            $("#inputSoldadoresRaiz").data("kendoGrid").dataSource.insert(total, {});
            $("#inputSoldadoresRaiz").data("kendoGrid").dataSource.page(dataSource.totalPages());
            $("#inputSoldadoresRaiz").data("kendoGrid").editRow($("#inputSoldadoresRaiz").data("kendoGrid").tbody.children().last());

            //}
        }
    });

};


function VentanaModalSoldadoresRaiz() {

    var modalTitle = "";
    modalTitle = _dictionary.CapturaSoldaduraSoldadoresRaiz[$("#language").data("kendoDropDownList").value()];
    var windowRaiz = $("#windowGridSoldadorRaiz");
    var win = windowRaiz.kendoWindow({
        modal: true,
        title: modalTitle,
        resizable: false,
        visible: true,
        width: "50%",
        minWidth: 30,
        position: {
            top: "1%",
            left: "1%"
        },
        actions: []
    }).data("kendoWindow");
    windowRaiz.data("kendoWindow").title(modalTitle);
    windowRaiz.data("kendoWindow").center().open();

};




function CargarGridPopupSoldadoresRellenoCapturados() {


    $("#inputSoldadoresRelleno").kendoGrid({

        dataSource: {
            data: [],
            schema: {
                model: {
                    fields: {
                        Accion: { type: "int", editable: false },
                        Soldador: { type: "string", editable: true },
                        Colada: { type: "string", editable: true },
                        //Observaciones: { type: "string", editable: true }
                    }
                }
            }, filter: {
                logic: "or",
                filters: [
                  { field: "Accion", operator: "eq", value: 1 },
                  { field: "Accion", operator: "eq", value: 2 },
                    { field: "Accion", operator: "eq", value: 0 },
                    { field: "Accion", operator: "eq", value: undefined }
                ]
            }
        },
        selectable: true,
        filterable: getGridFilterableMaftec(),
        columns: [
          { field: "Soldador", title: _dictionary.CapturaSoldaduraHeaderSoldadores[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftecpopUp(), editor: RenderComboBoxSoldadorRelleno, width: "100px" },
          { field: "Colada", title: _dictionary.ListadoCatalogos0046[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftecpopUp(), editor: RenderComboBoxColada, width: "100px" },
          //{ field: "Observaciones", title: _dictionary.CapturaSoldaduraHeaderObservacion[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftecpopUp(), width: "100px" },
        {
            command: {
                name: "",
                title: _dictionary.columnELM[$("#language").data("kendoDropDownList").value()],
                text: _dictionary.botonCancelar[$("#language").data("kendoDropDownList").value()],
                click: function (e) {
                    e.preventDefault();
                    //modeloRenglon.ListaDetalleTrabajoAdicional;
                    var dataSource = this.dataSource;
                    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
                    if (buscarSoldadoresTrabajoAdicional(dataItem.ObreroID, modeloRenglon.ListaDetalleTrabajoAdicional)) {
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

                        ventanaConfirm.content(_dictionary.CapturaSoldaduraTrabajoAdicionalSeraBorrado[$("#language").data("kendoDropDownList").value()] +
                                     "</br><center><button class='confirm_yes btn btn-blue' id='yesButton'>Si</button><button class='confirm_yes btn btn-blue' id='noButton'> No</button></center>");

                        ventanaConfirm.open().center();

                        $("#yesButton").click(function () {

                            if (dataItem != null) {
                                if (dataItem.Accion == 1 || dataItem.Accion == undefined)
                                    dataSource.remove(dataItem);
                                else
                                    dataItem.Accion = 3;
                            }
                            eliminaSoldadoresTrabajoAdicional(dataItem.ObreroID, modeloRenglon.ListaDetalleTrabajoAdicional);


                            var cont = 0;
                            for (var i = 0; i < modeloRenglon.ListaDetalleTrabajoAdicional; i++) {
                                if (modeloRenglon.ListaDetalleTrabajoAdicional[i].Accion == 1 || modeloRenglon.ListaDetalleTrabajoAdicional[i].Accion == 2)
                                    cont++;
                            }

                            actuallongitudTrabajosAdicionales = cont;
                            if (actuallongitudTrabajosAdicionales == 0)
                                modeloRenglon.TemplateTrabajosAdicionales = _dictionary.CapturaArmadoTemplateNoHayTrabajosAdicionales[$("#language").data("kendoDropDownList").value()];
                            else
                                modeloRenglon.TemplateTrabajosAdicionales = _dictionary.CapturaSoldaduraMensajeCambioLongitud[$("#language").data("kendoDropDownList").value()] + actuallongitudTrabajosAdicionales + _dictionary.CapturaSoldaduraMensajeCambioTrabajosAdicionales[$("#language").data("kendoDropDownList").value()];



                            dataSource.sync();

                            ventanaConfirm.close();
                        });
                        $("#noButton").click(function () {
                            ventanaConfirm.close();
                        });
                    }
                    else {
                        if (dataItem != null) {
                            if (dataItem.Accion == 1 || dataItem.Accion == undefined)
                                dataSource.remove(dataItem);
                            else
                                dataItem.Accion = 3;
                        }

                        dataSource.sync();
                    }
                }
            }, width: "50px", attributes: { style: "text-align:center;" }, title: _dictionary.columnELM[$("#language").data("kendoDropDownList").value()]
        },
        ],
        editable: true,
        navigatable: true,
        toolbar: [{ name: "create" }]
    });
    CustomisaGrid($("#inputSoldadoresRelleno"));


    $("#inputSoldadoresRelleno table").on("keydown", function (e) {
        if (e.keyCode == 13) {
            e.preventDefault();
            //var grid = $("#gridPopUp").data("kendoGrid");
            //var length = grid.dataSource.view().length;
            //var currentIndexRow = $("#gridPopUp").data("kendoGrid").items().index($("#gridPopUp").data("kendoGrid").select());
            //if (currentIndexRow == length - 1) {
            var dataSource = $("#inputSoldadoresRelleno").data("kendoGrid").dataSource;
            var total = $("#inputSoldadoresRelleno").data("kendoGrid").dataSource.data().length;
            $("#inputSoldadoresRelleno").data("kendoGrid").dataSource.insert(total, {});
            $("#inputSoldadoresRelleno").data("kendoGrid").dataSource.page(dataSource.totalPages());
            $("#inputSoldadoresRelleno").data("kendoGrid").editRow($("#inputSoldadoresRelleno").data("kendoGrid").tbody.children().last());

            //}
        }
    });


};


function VentanaModalSoldadoresRelleno() {

    var modalTitle = "";
    modalTitle = _dictionary.CapturaSoldaduraSoldadoresRelleno[$("#language").data("kendoDropDownList").value()];
    var windowRelleno = $("#windowMultiselectSoldadorRelleno");
    var win = windowRelleno.kendoWindow({
        modal: true,
        title: modalTitle,
        resizable: false,
        visible: true,
        width: "50%",
        minWidth: 30,
        position: {
            top: "1%",
            left: "1%"
        },
        actions: []
    }).data("kendoWindow");
    windowRelleno.data("kendoWindow").title(modalTitle);
    windowRelleno.data("kendoWindow").center().open();

};

function ArregloListadoJuntasCapturadas() {
    var dataSource = $("#grid").data("kendoGrid").dataSource;
    var data = dataSource._data

    JsonCaptura = [];


    for (var i = 0; i < data.length ; i++) {
        JsonCaptura[i] = { IDProyecto: "", Proyecto: "", IdOrdenTrabajo: "", OrdenTrabajo: "", idVal: "", idText: "", SpoolID: "", JuntaID: "", Junta: "", FechaArmado: "", TuberoID: "", Tubero: "", TallerID: "", Taller: "", sinCaptura: "" };
        JsonCaptura[i].IDProyecto = data[i].IDProyecto;
        JsonCaptura[i].Proyecto = data[i].Proyecto;
        JsonCaptura[i].IdOrdenTrabajo = data[i].IdOrdenTrabajo;
        JsonCaptura[i].OrdenTrabajo = data[i].OrdenTrabajo;
        JsonCaptura[i].idVal = data[i].idVal;
        JsonCaptura[i].idText = data[i].idText;
        JsonCaptura[i].SpoolID = data[i].SpoolID;
        JsonCaptura[i].JuntaID = data[i].JuntaID;
        JsonCaptura[i].Junta = data[i].Junta;
        JsonCaptura[i].FechaSoldadura = kendo.toString(data[i].FechaSoldadura, _dictionary.FormatoFecha[$("#language").data("kendoDropDownList").value()]);;
        JsonCaptura[i].tallerID = data[i].TallerID
        JsonCaptura[i].Taller = data[i].Taller;
        JsonCaptura[i].ColadaID = data[i].ColadaID;
        JsonCaptura[i].NumeroColada = data[i].NumeroColada;
        JsonCaptura[i].sinCaptura = "Todos";

    }
    return JsonCaptura;
}

function ArregloListadoCaptura() {
    JsonCaptura = [];
    JsonCaptura[0] = { IDProyecto: "", Proyecto: "", IdOrdenTrabajo: "", OrdenTrabajo: "", idVal: "", idText: "", SpoolID: "", JuntaID: "", Junta: "", FechaSoldadura: "", tallerID: "", Taller: "", sinCaptura: "", IDProyecto: "" };
    JsonCaptura[0].IDProyecto = $("#InputID").data("kendoComboBox").dataItem($("#InputID").data("kendoComboBox").select()).ProyectoID;
    JsonCaptura[0].Proyecto = $("#InputID").data("kendoComboBox").dataItem($("#InputID").data("kendoComboBox").select()).Proyecto;
    JsonCaptura[0].IdOrdenTrabajo = $("#InputOrdenTrabajo").val();
    JsonCaptura[0].OrdenTrabajo = $("#InputOrdenTrabajo").val();
    JsonCaptura[0].idVal = $("#InputID").val();
    JsonCaptura[0].idText = $("#InputID").data("kendoComboBox").text()
    JsonCaptura[0].SpoolID = $("#InputOrdenTrabajo").val() + '-' + $("#InputID").val();
    JsonCaptura[0].JuntaID = $("#Junta").val();
    JsonCaptura[0].Junta = $("#Junta").data("kendoComboBox").text();
    JsonCaptura[0].FechaSoldadura = $("#FechaSoldadura").val();
    JsonCaptura[0].sinCaptura = $('input:radio[name=Muestra]:checked').val();

    return JsonCaptura[0];
};

function ExisteJunta(Row) {
    var jsonGridSoldadura = $("#grid").data("kendoGrid").dataSource._data;
    if ($('input:radio[name=TipoAgregado]:checked').val() == "Listado") {
        for (var i = 0; i < jsonGridSoldadura.length; i++) {
            if (jsonGridSoldadura[i].IdOrdenTrabajo + '-' + jsonGridSoldadura[i].IdVal == (Row.IdOrdenTrabajo + '-' + Row.IdVal) && jsonGridSoldadura[i].JuntaID === Row.JuntaID) {
                return true;
            }
        }
    }
    return false;
}



function CargarGridPopUp() {

    $("#gridPopUp").kendoGrid({
        dataSource: {
            data: [],
            schema: {
                model: {
                    fields: {
                        juntaSpoolID: { type: "int", editable: true },
                        juntaSoldaduraID: { type: "int", editable: true },
                        TrabajoAdicional: { type: "string", editable: true },
                        ObreroID: { type: "int", editable: true },
                        Soldador: { type: "string", editable: true },
                        Observacion: { type: "string", editable: true }
                    }
                }
            }, filter: {
                logic: "or",
                filters: [
                  { field: "Accion", operator: "eq", value: 1 },
                  { field: "Accion", operator: "eq", value: 2 },
                    { field: "Accion", operator: "eq", value: 0 },
                    { field: "Accion", operator: "eq", value: undefined }
                ]
            },
        },
        selectable: true,
        filterable: getGridFilterableMaftec(),

        change: function (e) {

            //ItemSeleccionadoAnidado = this.dataSource.view()[this.select().index()];

            //var dataSource = this.dataSource;
            //var filters = dataSource.filter();
            //var allData = dataSource.data();
            //var query = new kendo.data.Query(allData);
            //var data = query.filter(filters).data;
            //actuallongitudTrabajosAdicionales = data.length;
            //if (actuallongitudTrabajosAdicionales == 0 || actuallongitudTrabajosAdicionales == undefined)
            //    modeloRenglon.TrabajosAdicionales = _dictionary.CapturaArmadoTemplateNoHayTrabajosAdicionales[$("#language").data("kendoDropDownList").value()];
            //else
            //    modeloRenglon.TrabajosAdicionales = _dictionary.CapturaSoldaduraMensajeCambioLongitud[$("#language").data("kendoDropDownList").value()] + actuallongitudTrabajosAdicionales + _dictionary.CapturaSoldaduraMensajeCambioTrabajosAdicionales[$("#language").data("kendoDropDownList").value()];
            //if (modeloRenglon.JuntaSoldaduraID != 0 && modeloRenglon.JuntaSoldaduraID != undefined)
            //    modeloRenglon.Accion = 2;

        },
        columns: [
          { field: "TrabajoAdicional", title: _dictionary.CapturaSoldaduraHeaderTrabajosAdicionalesAnidado[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftecpopUp(), width: "80px", editor: RenderComboBoxTrabajos },
          { field: "Soldador", title: _dictionary.CapturaSoldaduraHeaderSoldador[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftecpopUp(), width: "80px", editor: RenderComboBoxObrerosAdicionales },
          { field: "Observacion", title: _dictionary.CapturaSoldaduraHeaderObservacion[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftecpopUp(), width: "100px" },

         {
             command: {
                 name: "",
                 title: "",
                 text: _dictionary.botonCancelar[$("#language").data("kendoDropDownList").value()],
                 click: function (e) {
                     e.preventDefault();
                     dataItem = this.dataItem($(e.currentTarget).closest("tr"));

                     var dataSource = this.dataSource;

                     if (dataItem.Accion == 1 || dataItem.Accion == undefined)
                         dataSource.remove(dataItem);
                     else
                         dataItem.Accion = 3;

                     var filters = dataSource.filter();
                     var allData = dataSource.data();
                     var query = new kendo.data.Query(allData);
                     var data = query.filter(filters).data;

                     actuallongitudTrabajosAdicionales = data.length;

                     if (actuallongitudTrabajosAdicionales == 0 || actuallongitudTrabajosAdicionales == undefined)
                         modeloRenglon.TrabajosAdicionales = _dictionary.CapturaArmadoTemplateNoHayTrabajosAdicionales[$("#language").data("kendoDropDownList").value()];
                     else
                         modeloRenglon.TrabajosAdicionales = _dictionary.CapturaSoldaduraMensajeCambioLongitud[$("#language").data("kendoDropDownList").value()] + actuallongitudTrabajosAdicionales + _dictionary.CapturaSoldaduraMensajeCambioTrabajosAdicionales[$("#language").data("kendoDropDownList").value()];

                     dataSource.sync();

                 }
             }, width: "50px", title: _dictionary.tituloEliminar[$("#language").data("kendoDropDownList").value()], attributes: { style: "text-align:center;" }
         },
         {
             command: {
                 name: "",
                 title: "",
                 text: _dictionary.botonLimpiar[$("#language").data("kendoDropDownList").value()],
                 click: function (e) {
                     var itemToClean = $("#gridPopUp").data("kendoGrid").dataItem($(e.currentTarget).closest("tr"));
                     itemToClean.TrabajoAdicional = "";
                     itemToClean.Soldador = "";
                     itemToClean.Observacion = "";
                     var dataSource = $("#gridPopUp").data("kendoGrid").dataSource;
                     dataSource.sync();

                 }
             }, width: "50px", title: _dictionary.tituloLimpiar[$("#language").data("kendoDropDownList").value()], attributes: { style: "text-align:center;" }
         }
        ],
        editable: true,
        navigatable: true,
        toolbar: [{ name: "create" }]

    });

    CustomisaGrid($("#gridPopUp"));

    $("#gridPopUp table").on("keydown", function (e) {
        if (e.keyCode == 13) {
            e.preventDefault();
            //var grid = $("#gridPopUp").data("kendoGrid");
            //var length = grid.dataSource.view().length;
            //var currentIndexRow = $("#gridPopUp").data("kendoGrid").items().index($("#gridPopUp").data("kendoGrid").select());
            //if (currentIndexRow == length - 1) {
            var dataSource = $("#gridPopUp").data("kendoGrid").dataSource;
            var total = $("#gridPopUp").data("kendoGrid").dataSource.data().length;
            $("#gridPopUp").data("kendoGrid").dataSource.insert(total, {});
            $("#gridPopUp").data("kendoGrid").dataSource.page(dataSource.totalPages());
            $("#gridPopUp").data("kendoGrid").editRow($("#gridPopUp").data("kendoGrid").tbody.children().last());

            //}
        }
    });

};

function GridPopUpTrabajosAdicionales(data) {
    modeloRenglon = data;

    $("#gridPopUp").data('kendoGrid').dataSource.data([]);
    var ds = $("#gridPopUp").data("kendoGrid").dataSource;
    var array = [];
    array = data.ListaDetalleTrabajoAdicional;
    for (var i = 0; i < array.length; i++) {
        ds.add(array[i]);
    }
    ds.sync();
    VentanaModal();
}

function GridPopupSoldadoresRaizCapturados(data) {
    modeloRenglon = data;

    $("#inputSoldadoresRaiz").data('kendoGrid').dataSource.data([]);
    var ds = $("#inputSoldadoresRaiz").data("kendoGrid").dataSource;
    var array = [];
    array = data.ListaSoldadoresRaizCapturados;
    for (var i = 0; i < array.length; i++) {
        ds.add(array[i]);
    }
    ds.sync();
    VentanaModalSoldadoresRaiz();
}

function GridPopupSoldadoresRellenoCapturados(data) {
    modeloRenglon = data;

    $("#inputSoldadoresRelleno").data('kendoGrid').dataSource.data([]);
    var ds = $("#inputSoldadoresRelleno").data("kendoGrid").dataSource;
    var array = [];
    array = data.ListaSoldadoresRellenoCapturados;
    for (var i = 0; i < array.length; i++) {
        ds.add(array[i]);
    }
    ds.sync();
    VentanaModalSoldadoresRelleno();
}

var actuallongitudTrabajosAdicionales = 0;
var actuallongitudSoldadoresRaiz = 0;
var actuallongitudSoldadoresRelleno = 0;

function VentanaModal() {

    var modalTitle = "";
    modalTitle = _dictionary.CapturaSoldaduraHeaderAdicionales[$("#language").data("kendoDropDownList").value()];
    var window = $("#windowGrid");
    var win = window.kendoWindow({
        modal: true,
        title: modalTitle,
        resizable: false,
        visible: true,
        width: "50%",
        minWidth: 30,
        position: {
            top: "1%",
            left: "1%"
        },
        actions: [],
    }).data("kendoWindow");
    window.data("kendoWindow").title(modalTitle);
    window.data("kendoWindow").center().open();

};


function ObtenerListadoObreros(arregloObrerosRaiz, arregloObrerosRelleno) {
    var arregloRaiz = new Array(arregloObrerosRaiz.length);
    var arregloRelleno = new Array(arregloObrerosRelleno.length);

    //for (var i = 0; i < arregloRaiz.length; i++) {
    //    arregloRaiz[i] = new Array(2);
    //}

    for (var i = 0; i < arregloRaiz.length; i++) {
        arregloRaiz[i] = arregloObrerosRaiz[i].ObreroID;
        //arregloRaiz[i][2] = arregloObrerosRaiz[i].Soldador;
    }

    //for (var i = 0; i < arregloRelleno.length; i++) {
    //    arregloRelleno[i] = new Array(2);
    //}
    for (var i = 0; i < arregloRelleno.length; i++) {
        arregloRelleno[i] = arregloObrerosRelleno[i].ObreroID;
        //arregloRelleno[i][2] = arregloObrerosRelleno[i].Soldador;
    }



    var arreglo = arregloRaiz.concat(arregloRelleno)
    var arregloSinRepetir = arreglo.filter(onlyUnique);
    var array = [];

    for (var i = 0; i < arregloSinRepetir.length; i++) {
        array[i] = { ObreroID: "", Soldador: "" };
        for (var j = 0; j < arregloObrerosRaiz.length; j++) {
            if (arregloSinRepetir[i] == arregloObrerosRaiz[j].ObreroID) {
                array[i].ObreroID = arregloObrerosRaiz[j].ObreroID;
                array[i].Soldador = arregloObrerosRaiz[j].Soldador;
            }
        }
        if (array[i].Soldador == "") {
            for (var j = 0; j < arregloObrerosRelleno.length; j++) {
                if (arregloSinRepetir[i] == arregloObrerosRelleno[j].ObreroID) {
                    array[i].ObreroID = arregloObrerosRelleno[j].ObreroID;
                    array[i].Soldador = arregloObrerosRelleno[j].Soldador;
                }
            }
        }

    }

    return array;
}

function onlyUnique(value, index, self) {
    return self.indexOf(value) === index;
}



function PlanchaTaller() {
    var dataSource = $("#grid").data("kendoGrid").dataSource;
    var filters = dataSource.filter();
    var allData = dataSource.data();
    var query = new kendo.data.Query(allData);
    var data = query.filter(filters).data;

    for (var i = 0; i < data.length; i++) {
        if ($("#inputTaller").data("kendoComboBox").text() != "") {
            if ($('input:radio[name=LLena]:checked').val() === "Todos") {
                data[i].TallerID = $("#inputTaller").val();
                data[i].Taller = $("#inputTaller").data("kendoComboBox").text();
            }
            else {
                if (data[i].Taller == "" || data[i].Taller == null || data[i].Taller == undefined) {
                    data[i].TallerID = $("#inputTaller").val();
                    data[i].Taller = $("#inputTaller").data("kendoComboBox").text();
                }
            }
        }
    }
    $("#grid").data("kendoGrid").dataSource.sync();
};
function PlanchaFecha() {
    var dataSource = $("#grid").data("kendoGrid").dataSource;
    var filters = dataSource.filter();
    var allData = dataSource.data();
    var query = new kendo.data.Query(allData);
    var data = query.filter(filters).data;

    for (var i = 0; i < data.length; i++) {
        if (String(endRangeDate.val()).trim() != "") {
            if ($('input:radio[name=LLena]:checked').val() === "Todos") {
                data[i].FechaSoldadura = String(endRangeDate.val()).trim();
            }
            else {
                if (data[i].FechaSoldadura == "" || data[i].FechaSoldadura == null || data[i].FechaSoldadura == undefined) {
                    data[i].FechaSoldadura = String(endRangeDate.val()).trim();
                }
            }
        }
    }
    $("#grid").data("kendoGrid").dataSource.sync();
};


function desplegarNotificacion() {
    var tipoCapturaSpool = $('input:radio[name=TipoAgregado]:checked').val() == "Listado" ? false : true;
    var spoolIDDefinido = $("#InputID").data("kendoComboBox").dataItem($("#InputID").data("kendoComboBox").select()) == undefined ? false : true;
    var tipoMostrarTodos = $('input:radio[name=Muestra]:checked').val() == "Todos" ? true : false;
    var InputIDVacio = $("#InputID").val() == "" ? true : false;
    var InputJuntaVacia = $("#Junta").val() == "" ? true : false;
    var JuntaDefinida = $("#Junta").data("kendoComboBox").dataItem($("#Junta").data("kendoComboBox").select()) == undefined ? false : true;


    if (InputIDVacio) {
        displayNotify("CapturaSoldaduraSpoolNoCapturado", "", '1');
    }
    else if (InputJuntaVacia) {
        displayNotify("JuntaSinSeleccionar", "", '1');
    }
    else if (!spoolIDDefinido) {
        displayNotify("NoExisteSpoolID", "", '1');
    }
    else if (tipoCapturaSpool) {
        displayNotify("NoExisteSpoolID", "", '1');
    }
    else if (!tipoCapturaSpool) {
        if (JuntaDefinida) {
            if (!tipoMostrarTodos) {
                displayNotify("CapturaArmadoNoExisteLista", "", '1');
            }
        }
        else if (!JuntaDefinida) {
            if (tipoMostrarTodos) {
                displayNotify("CapturaArmadoNoExisteSpool", "", '1');
            }
            else {
                displayNotify("CapturaArmadoNoExisteLista", "", '1');
            }
        }
    }

}


function buscarSoldadoresTrabajoAdicional(ObreroID, listaTrabajoAdicional) {

    for (var i = 0; i < listaTrabajoAdicional.length; i++) {
        if (listaTrabajoAdicional[i].Accion != 3 || listaTrabajoAdicional[i].Accion != 4) {
            if (listaTrabajoAdicional[i].ObreroID == ObreroID) {
                return true;
            }
        }
    }
    return false;
}

function eliminaSoldadoresTrabajoAdicional(ObreroID, listaTrabajoAdicional) {

    for (var i = 0; i < listaTrabajoAdicional.length; i++) {
        if (listaTrabajoAdicional[i].Accion != 3 || listaTrabajoAdicional[i].Accion != 4) {
            if (listaTrabajoAdicional[i].ObreroID == ObreroID) {
                listaTrabajoAdicional[i].Accion = 3;
            }
        }
    }

}

function SuscribirFechaSoldadura() {

    endRangeDate = $("#FechaSoldadura").kendoDatePicker({
        max: new Date(),
        //format: _dictionary.FormatoFecha[$("#language").data("kendoDropDownList").value()],
        change: function (e) {
            ValidarFecha(e.sender._value)
        }
    });

    endRangeDate.on("keydown", function (e) {
        if (e.keyCode == 9) {
            ValidarFecha($("#FechaSoldadura").data("kendoDatePicker").value());
        }
    });

    $("#FechaSoldadura").blur(function (e) {
        ValidarFecha($("#FechaSoldadura").data("kendoDatePicker").value());
    });
}

function ValidarFecha(valor) {
    var fecha = kendo.toString(valor, String(_dictionary.FormatoFecha[$("#language").data("kendoDropDownList").value()].replace('{', '').replace('}', '').replace("0:", "")));
    if (fecha == null) {
        $("#FechaArmado").data("kendoDatePicker").value('');
    }
}