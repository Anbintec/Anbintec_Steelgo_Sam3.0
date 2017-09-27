var esNormal;

function CargarGrid() {

    $("#grid").kendoGrid({
        edit: function (e) {
            if ($('#Guardar').text() != _dictionary.MensajeGuardar[$("#language").data("kendoDropDownList").value()])
                this.closeCell();

            if ($(".k-grid-content td").css("white-space") == "normal") {
                esNormal = true;
            }
            else {
                esNormal = false;
            }
        },
        dataSource: {
            data: [],
            schema: {
                model: {
                    fields: {
                        CodigoObrero: { type: "string", editable: true },
                        NombreWPS: { type: "string", editable: false },
                        ProcesoSoldadura: { type: "string", editable: true },
                        FechaInicioCertificado: { type: "date", editable: true },
                        FechaFinCertificado: { type: "date", editable: true },
                        PasosSoldadura: { type: "number", editable: true },
                        CedulaTuboCalificado: { type: "string", editable: true },
                        EspesorMinimo: { type: "number", editable: true },
                        EspesorMaximo: { type: "number", editable: false },
                        DiametroCalificado: { type: "number", editable: true },
                        DiametroMinimo: { type: "number", editable: false },
                        TipoDePrueba: { type: "string" },
                        Posicion: { type: "number" }

                    }
                }
            },
            filter: {
                logic: "or",
                filters: [
                  { field: "Accion", operator: "eq", value: 1 },
                  { field: "Accion", operator: "eq", value: 2 }
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
            numeric: true
        },
        filterable: getGridFilterableMaftec(),
        columns: [
            { field: "CodigoObrero", title: _dictionary.columnSoldador[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "145px", editor: RenderComboBoxSoldador },
            { field: "NombreWPS", title: _dictionary.columnNombreWPS[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "120px" },
            { field: "ProcesoSoldadura", title: _dictionary.columnProcesoSoldadura[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "120px", editor: RenderComboBoxProcesoSoldadura },
            { field: "FechaInicioCertificado", title: _dictionary.columnDesde[$("#language").data("kendoDropDownList").value()], filterable: getKendoGridFilterableDateMaftec(), width: "100px", format: _dictionary.FormatoFecha[$("#language").data("kendoDropDownList").value()], editor: RenderFechaInicio },
            { field: "FechaFinCertificado", title: _dictionary.columnHasta[$("#language").data("kendoDropDownList").value()], filterable: getKendoGridFilterableDateMaftec(), width: "100px", format: _dictionary.FormatoFecha[$("#language").data("kendoDropDownList").value()], editor: RenderFechaFin },
            { field: "PasosSoldadura", title: _dictionary.columnNumeroPasos[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellNumberMaftec(), width: "90px", editor: renderNoPasos, attributes: { style: "text-align:right;" } },
            { field: "CedulaTuboCalificado", title: _dictionary.columnCedulaTubo[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "90px", editor: RenderComboBoxCedulaTuboCalificado },
            { field: "EspesorMinimo", title: _dictionary.columnEspesorMin[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellNumberMaftec(), width: "110px", editor: renderEmin, attributes: { style: "text-align:right;" } },
            { field: "EspesorMaximo", title: _dictionary.columnEspesorMax[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellNumberMaftec(), width: "110px", attributes: { style: "text-align:right;" } },
            { field: "DiametroCalificado", title: _dictionary.columnDiametroCalificado[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellNumberMaftec(), width: "130px", attributes: { style: "text-align:right;" }, editor: renderDiametro },
            { field: "DiametroMinimo", title: _dictionary.columnDiametroMinimo[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellNumberMaftec(), width: "130px", attributes: { style: "text-align:right;" }, },
            { field: "TipoDePrueba", title: _dictionary.columnTipoPrueba[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "100px", editor: RenderComboBoxTipoPrueba },
            { field: "Posicion", title: _dictionary.columnPosicion[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellNumberMaftec(), width: "120px", editor: renderPosicion, format: "{0} °", attributes: { style: "text-align:right;" } },
            { command: { text: _dictionary.botonCancelar[$("#language").data("kendoDropDownList").value()], click: EliminaSoldadorCertificacion }, title: _dictionary.columnELM[$("#language").data("kendoDropDownList").value()], width: "50px" }
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


function EliminaSoldadorCertificacion(e) {
    e.preventDefault();
    if ($('#Guardar').text() == _dictionary.MensajeGuardar[$("#language").data("kendoDropDownList").value()]) {

        e.preventDefault();


        var dataItem = $("#grid").data("kendoGrid").dataItem($(e.currentTarget).closest("tr"));
        var WPSIDRegistro = dataItem.WPSID;
        //if (dataItem.RegistrosWPS == 0) {

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

        ventanaConfirm.content(_dictionary.SoldadorCertificacionPreguntaBorradoCaptura[$("#language").data("kendoDropDownList").value()] +
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

}


function VentanaModal() {
    var modalTitle = "";
    modalTitle = "Soldador Certificacion";
    var window = $("#windowSoldadorCertificacion");

    if (win == undefined) {

        win = window.kendoWindow({
            
            modal: true,
            title: modalTitle,
            resizable: false,
            visible: false,
            width: "50%",
            minWidth: 660,
            position: {
                top: "10%",
                left: "20%"
            },
            animation: {
                close: false,
                open: false
            },
            actions: []
        }).data("kendoWindow");

    }

    window.data("kendoWindow").title(modalTitle);
    window.data("kendoWindow").center().open();

};


function AbrirVentanaModalVista() {
    VentanaModal();
    $("#windowSoldadorCertificacion").show();
};

function changeLanguageCall() {
    document.title = _dictionary.SoldadorCertificacionBreadcrumb[$("#language").data("kendoDropDownList").value()];
    CargarGrid();
    SuscribirEventos();
    setTimeout(function () { AjaxObtenerJSONGrid(); }, 100)
    opcionHabilitarView(false, "FieldSetView");
};

function tieneClase(item) {
    for (var i = 0; i < item.classList.length; i++) {
        if (item.classList[i] == "k-state-border-up" || item.classList[i] == "k-state-border-down") {
            return true;
        }
    }
    return false;
}

function ObtenerDato(fecha, tipoDatoObtener) {
    var cultura = $("#language").val();

    switch (tipoDatoObtener) {
        case 1://anho
            return fecha.split('/')[2]
            break;
        case 2://mes
            if (cultura == 'es-MX')
                return fecha.split('/')[1] - 1
            else
                return fecha.split('/')[0] - 1
            break;
        case 3://dia
            if (cultura == 'es-MX')
                return fecha.split('/')[0]
            else
                return fecha.split('/')[1]
            break;
    }
}

function ValidarInformacionEnviada() {
    var desplegadoPasos = false;
    var desplegadoEspesor = false;
    var desplegadoDiametro = false;
    var desplegadoPosicion = false;


    Captura = [];
    Captura[0] = { Detalles: "" };
    ListaDetalles = [];
    var arregloCaptura = $("#grid").data("kendoGrid").dataSource._data;
    for (index = 0; index < arregloCaptura.length; index++) {
        $("#grid").data("kendoGrid").dataSource._data[index].RowOk = true;
        ListaDetalles[index] = {
            SoldadorCertificacionID: "",
            Accion: "",
            ObreroID: "",
            WPSID: "",
            ProcesoSoldaduraID: "",
            TipoDePruebaID: "",
            Posicion: "",
            FechaInicioCertificado: "",
            FechaFinCertificado: "",
            CedulaTuboCalificadoID: "",
            DiametroCalificado: "",
            EspesorMinimo: "",
            EspesorMaximo: "",
            PasosSoldadura: "",
            Estatus: 1
        };

        if (
            (arregloCaptura[index].CodigoObrero == "" || arregloCaptura[index].CodigoObrero == undefined || arregloCaptura[index].CodigoObrero == null) ||
            (arregloCaptura[index].NombreWPS == "" || arregloCaptura[index].NombreWPS == undefined || arregloCaptura[index].NombreWPS == null) ||
            (arregloCaptura[index].ProcesoSoldadura == "" || arregloCaptura[index].ProcesoSoldadura == undefined || arregloCaptura[index].ProcesoSoldadura == null) ||
            (arregloCaptura[index].FechaInicioCertificado == "" || arregloCaptura[index].FechaInicioCertificado == undefined || arregloCaptura[index].FechaInicioCertificado == null) ||
            (arregloCaptura[index].FechaFinCertificado == "" || arregloCaptura[index].FechaFinCertificado == undefined || arregloCaptura[index].FechaFinCertificado == null) ||
            (arregloCaptura[index].PasosSoldadura == "" || arregloCaptura[index].PasosSoldadura == undefined || arregloCaptura[index].PasosSoldadura == null) ||
            (arregloCaptura[index].CedulaTuboCalificado == "" || arregloCaptura[index].CedulaTuboCalificado == undefined || arregloCaptura[index].CedulaTuboCalificado == null) ||
            (arregloCaptura[index].EspesorMinimo == "" || arregloCaptura[index].EspesorMinimo == undefined || arregloCaptura[index].EspesorMinimo == null) ||
            (arregloCaptura[index].EspesorMaximo == "" || arregloCaptura[index].EspesorMaximo == undefined || arregloCaptura[index].EspesorMaximo == null) ||
            (arregloCaptura[index].DiametroCalificado == "" || arregloCaptura[index].DiametroCalificado == undefined || arregloCaptura[index].DiametroCalificado == null) ||
            (arregloCaptura[index].TipoDePrueba == "" || arregloCaptura[index].TipoDePrueba == undefined || arregloCaptura[index].TipoDePrueba == null) ||
            (arregloCaptura[index].Posicion == "" || arregloCaptura[index].Posicion == undefined || arregloCaptura[index].Posicion == null)
           ) {

            if (arregloCaptura[index].PasosSoldadura == "" || arregloCaptura[index].PasosSoldadura <= 0) { //Pasos soladura
                desplegadoPasos = true;

                displayNotify("CapturaSoldadorCertificacionNoPasosMsg", "", '1');
            }
            else if (arregloCaptura[index].EspesorMinimo == "" || arregloCaptura[index].EspesorMinimo <= 0) { //Pasos soladura
                desplegadoEspesor = true;
                displayNotify("SoldadorCertificacionMensajeEspesorMinimo", '1');

            }
            else if (arregloCaptura[index].DiametroCalificado == "" || arregloCaptura[index].DiametroCalificado <= 0) { //Pasos soladura
                desplegadoDiametro = true;
                displayNotify("SoldadorCertificacionMensajeDiametroCalificado", "", '1');

            }
            else if (arregloCaptura[index].Posicion == "" || arregloCaptura[index].Posicion <= 0) { //Pasos soladura
                desplegadoPosicion = true;
                displayNotify("CapturaSoldadorCertificacionPosicionMsg", "", '1');

            }

            ListaDetalles[index].Estatus = 0;
            $("#grid").data("kendoGrid").dataSource._data[index].RowOk = false;
            //$('tr[data-uid="' + arregloCaptura[index].uid + '"] ').css("background-color", "#ffcccc");
        }

        //el registro esta bien.
        ListaDetalles[index].SoldadorCertificacionID = arregloCaptura[index].SoldadorCertificacionID;
        ListaDetalles[index].Accion = arregloCaptura[index].Accion;
        ListaDetalles[index].ObreroID = arregloCaptura[index].ObreroID;
        ListaDetalles[index].WPSID = arregloCaptura[index].WPSID;
        ListaDetalles[index].ProcesoSoldaduraID = arregloCaptura[index].ProcesoSoldaduraID;
        ListaDetalles[index].TipoDePruebaID = arregloCaptura[index].TipoDePruebaID;
        ListaDetalles[index].Posicion = arregloCaptura[index].Posicion;
        ListaDetalles[index].FechaInicioCertificado = arregloCaptura[index].FechaInicioCertificado == null ? "" : kendo.toString(arregloCaptura[index].FechaInicioCertificado, String(_dictionary.FormatoFecha[$("#language").data("kendoDropDownList").value()].replace('{', '').replace('}', '').replace("0:", ""))).trim();
        ListaDetalles[index].FechaFinCertificado = arregloCaptura[index].FechaFinCertificado == null ? "" : kendo.toString(arregloCaptura[index].FechaFinCertificado, String(_dictionary.FormatoFecha[$("#language").data("kendoDropDownList").value()].replace('{', '').replace('}', '').replace("0:", ""))).trim();
        ListaDetalles[index].CedulaTuboCalificadoID = arregloCaptura[index].CedulaTuboCalificadoID;
        ListaDetalles[index].DiametroCalificado = arregloCaptura[index].DiametroCalificado;
        ListaDetalles[index].DiametroMinimo = arregloCaptura[index].DiametroMinimo;
        ListaDetalles[index].EspesorMinimo = arregloCaptura[index].EspesorMinimo;
        ListaDetalles[index].EspesorMaximo = arregloCaptura[index].EspesorMaximo;
        ListaDetalles[index].PasosSoldadura = arregloCaptura[index].PasosSoldadura;


    }
    Captura[0].Detalles = ListaDetalles;




    if (NombreRepetido(ListaDetalles)) {
        displayNotify("SoldadorNombreRepetido", "", "2");
        $("#grid").data("kendoGrid").dataSource.sync();
    }
    else if (!ExistRowEmpty(ListaDetalles)) {
        if (Captura[0].Detalles.length > 0) {
            AjaxGuardarInformacion(Captura[0]);
        }
    }
    else {
        loadingStop();
        windowTemplate = kendo.template($("#windowTemplate").html());
        $("#grid").data("kendoGrid").dataSource.sync();
        ventanaConfirm = $("#ventanaConfirm").kendoWindow({
            iframe: true,
            title: _dictionary.CapturaAvanceIntAcabadoMensajeErrorGuardado[$("#language").data("kendoDropDownList").value()],
            visible: false, //the window will not appear before its .open method is called
            width: "auto",
            height: "auto",
            modal: true,
            animation: {
                close: false,
                open: false
            },
            actions: []
        }).data("kendoWindow");

        ventanaConfirm.content(_dictionary.CapturaAvanceIntAcabadoMensajePreguntaGuardado[$("#language").data("kendoDropDownList").value()] +
            "</br><center><button class='btn btn-blue' id='yesButton'>" + _dictionary.EntregaPlacasGraficasbotonSi[$("#language").data("kendoDropDownList").value()] + "</button><button class='btn btn-blue' id='noButton'>" + _dictionary.EntregaPlacasGraficasbotonNo[$("#language").data("kendoDropDownList").value()] + "</button></center>");

        ventanaConfirm.open().center();


        //RowEmpty($("#grid"));

        $("#yesButton").click(function () {

            ArregloGuardado = [];
            var indice = 0;
            for (var i = 0; i < Captura[0].Detalles.length; i++) {
                if (Captura[0].Detalles[i].Estatus == 1) {
                    ArregloGuardado[indice] = ListaDetalles[i];
                    indice++;
                }
            }

            Captura[0].Detalles = [];
            Captura[0].Detalles = ArregloGuardado;


            if (Captura[0].Detalles.length > 0) {
                AjaxGuardarInformacion(Captura[0]);
            }
            else {
                displayNotify("AdverteciaExcepcionGuardado", "", '1');
            }

            ventanaConfirm.close();
        });
        $("#noButton").click(function () {
            opcionHabilitarView(false, "FieldSetView");
            ventanaConfirm.close();
        });

    }


}

function startChange(row) {

    var startDate = $("#inputFechaInicioCertificado").data("kendoDatePicker").value(),
     endDate = $("#inputFechaFinCertificado").data("kendoDatePicker").value(),
    end = $("#inputFechaFinCertificado").data("kendoDatePicker"),
    start = $("#inputFechaInicioCertificado").data("kendoDatePicker");

    if (startDate) {
        startDate = new Date(startDate);
        startDate.setDate(startDate.getDate());
        end.min(startDate);
    }
    else if (endDate) {
        start.max(new Date(endDate));
    } else {
        endDate = new Date();
        start.max(endDate);
        end.min(endDate);
    }

    
}

function endChange(row) {

    var endDate = $("#inputFechaFinCertificado").data("kendoDatePicker").value(),
    startDate = $("#inputFechaInicioCertificado").data("kendoDatePicker").value(),
    end = $("#inputFechaFinCertificado").data("kendoDatePicker"),
    start = $("#inputFechaInicioCertificado").data("kendoDatePicker");

    if (endDate) {
        endDate = new Date(endDate);
        endDate.setDate(endDate.getDate());
        start.max(endDate);
    }
    else if (startDate) {
        end.min(new Date(startDate));
    } else {
        endDate = new Date();
        start.max(endDate);
        end.min(endDate);
    }
}

function NombreRepetido(listaDetalles) {
    var dataSource = $("#grid").data("kendoGrid").dataSource;
    var allData = dataSource.data();

    for (var i = 0; i < listaDetalles.length; i++) {
        for (var j = 0; j < listaDetalles.length; j++) {
            if (listaDetalles[i].ObreroID == listaDetalles[j].ObreroID && listaDetalles[i].WPSID == listaDetalles[j].WPSID && listaDetalles[i].ProcesoSoldaduraID == listaDetalles[j].ProcesoSoldaduraID && i != j) {
                listaDetalles[j].Estatus = -4;
                $("#grid").data("kendoGrid").dataSource._data[j].RowOk = false;
            }
        }
    }

    return ValidaNombreRepetido(listaDetalles);
}



function ValidaNombreRepetido(rows) {
    for (var i = 0; i < rows.length; i++) {
        if (rows[i].Estatus == -4) {
            $("#grid").data("kendoGrid").dataSource._data[i].RowOk = false;
            return true;
        }
    }
    return false;
}

