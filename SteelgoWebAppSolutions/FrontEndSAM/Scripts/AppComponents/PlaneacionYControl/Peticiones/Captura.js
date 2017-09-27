function changeLanguageCall() {
    SuscribirEventos();
    CargarGrid();
    document.title = _dictionary.lblGrupoEmbarque[$("#language").data("kendoDropDownList").value()];
};


function CargarGrid() {
    var acciones = [
        { AccionID: "1", Accion: "Aprobar" },
        { AccionID: "2", Accion: "Enviar notificación" },
        { AccionID: "3", Accion: "Enviar notificación para aprobación" },
        { AccionID: "4", Accion: "Cerrar petición" },
    ];
    $("#grid").kendoGrid({
        edit: function (e) {
            //   this.closeCell();
        },
        autoBind: true,
        dataSource: {
            data: [
                {
                    Spool: "x",
                    SpoolID: "x1",
                    Paso: "PND",
                    Ciclo: "Si",
                    Fase: "Si",
                    Peso: "350",
                    Area: "623",
                    M2: "623",
                    Juntas: "4",
                    Peqs: "162",
                    GrupoEmision: "29-01-2016",
                    Emitido: "Si",
                    Embarque: "15-01-2016",
                    FechaPeticion: "06-02-2016",
                    TipoMaterial: "Acero",
                    ListaAcciones: acciones
                },
                {
                    Spool: "x",
                    SpoolID: "x2",
                    Paso: "LD",
                    Ciclo: "Si",
                    Fase: "Si",
                    Peso: "350",
                    Area: "623",
                    M2: "623",
                    Juntas: "4",
                    Peqs: "162",
                    GrupoEmision: "29-01-2016",
                    Emitido: "Si",
                    Embarque: "21-01-2016",
                    FechaPeticion: "06-02-2016",
                    TipoMaterial: "Acero",
                    ListaAcciones: acciones
                },
                {
                    Spool: "y",
                    SpoolID: "x3",
                    Paso: "Soldadura",
                    Ciclo: "Si",
                    Fase: "Si",
                    Peso: "350",
                    Area: "623",
                    M2: "623",
                    Juntas: "4",
                    Peqs: "162",
                    GrupoEmision: "29-01-2016",
                    Emitido: "Si",
                    Embarque: "15-06-2015",
                    FechaPeticion: "06-06-2015",
                    TipoMaterial: "Acero",
                    ListaAcciones: acciones
                },
                {
                    Spool: "w",
                    SpoolID: "",
                    Paso: "Armado",
                    Ciclo: "Si",
                    Fase: "Si",
                    Peso: "350",
                    Area: "623",
                    M2: "623",
                    Juntas: "4",
                    Peqs: "162",
                    GrupoEmision: "29-01-2015",
                    Emitido: "No",
                    Embarque: "",
                    FechaPeticion: "",
                    TipoMaterial: "Acero",
                    ListaAcciones: acciones
                },
            ],
            schema: {
                model: {
                    fields: {
                        Spool: { type: "string", editable: false },
                        SpoolID: { type: "string", editable: false },
                        Paso: { type: "string", editable: false },
                        Ciclo: { type: "string", editable: false },
                        Fase: { type: "string", editable: false },
                        Peso: { type: "string", editable: false },
                        Area: { type: "string", editable: false },
                        M2: { type: "string", editable: false },
                        Juntas: { type: "string", editable: false },
                        Peqs: { type: "string", editable: false },
                        GrupoEmision: { type: "string", editable: false },
                        Emitido: { type: "string", editable: false },
                        Embarque: { type: "string", editable: false },
                        FechaPeticion: { type: "string", editable: false },
                        TipoMaterial: { type: "string", editable: false },

                    }
                }
            },
            //filter: {
            //    logic: "or",
            //    filters: [
            //      { field: "Accion", operator: "eq", value: 1 },
            //      { field: "Accion", operator: "eq", value: 2 }
            //    ]
            //},
            pageSize: 20,
            serverPaging: false,
            serverFiltering: false,
            serverSorting: false
        },
        navigatable: true,
        filterable: {
            extra: false
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
        columns: [
            { field: "Acciones", title: _dictionary.PeticionHeaderResolucion[$("#language").data("kendoDropDownList").value()], filterable: false, editor: RenderComboBoxAcciones, width: "100px" },
            { field: "Spool", title: _dictionary.PeticionHeaderNombre[$("#language").data("kendoDropDownList").value()], filterable: true, width: "100px" },
            { field: "SpoolID", title: _dictionary.PeticionHeaderSpoolID[$("#language").data("kendoDropDownList").value()], filterable: true, width: "100px" },
            { field: "Paso", title: _dictionary.PeticionHeaderPaso[$("#language").data("kendoDropDownList").value()], filterable: true, width: "100px" },
            { field: "Ciclo", title: _dictionary.PeticionHeaderCiclo[$("#language").data("kendoDropDownList").value()], filterable: true, width: "100px" },
            { field: "Fase", title: _dictionary.PeticionHeaderFase[$("#language").data("kendoDropDownList").value()], filterable: true, width: "100px" },
            { field: "Peso", title: _dictionary.PeticionHeaderKg[$("#language").data("kendoDropDownList").value()], filterable: true, width: "100px" },
            { field: "Area", title: _dictionary.PeticionHeaderArea[$("#language").data("kendoDropDownList").value()], filterable: true, width: "100px" },
            { field: "M2", title: _dictionary.PeticionHeaderM2[$("#language").data("kendoDropDownList").value()], filterable: true, width: "100px" },
            { field: "Juntas", title: _dictionary.PeticionHeaderJuntas[$("#language").data("kendoDropDownList").value()], filterable: true, width: "110px" },
            { field: "Peqs", title: _dictionary.PeticionHeaderPeqs[$("#language").data("kendoDropDownList").value()], filterable: true, width: "100px" },
            { field: "GrupoEmision", title: _dictionary.PeticionHeaderGrupoEmision[$("#language").data("kendoDropDownList").value()], filterable: true, width: "150px" },
            { field: "Emitido", title: _dictionary.PeticionHeaderEmitido[$("#language").data("kendoDropDownList").value()], filterable: true, width: "100px" },
            { field: "Embarque", title: _dictionary.PeticionHeaderEmbarque[$("#language").data("kendoDropDownList").value()], filterable: true, width: "120px" },
            { field: "FechaPeticion", title: _dictionary.PeticionHeaderFechaPeticion[$("#language").data("kendoDropDownList").value()], filterable: true, width: "150px" },
            { field: "TipoMaterial", title: _dictionary.PeticionHeaderTipoMaterial[$("#language").data("kendoDropDownList").value()], filterable: true, width: "150px" },


            //{ command: { text: _dictionary.botonCancelar[$("#language").data("kendoDropDownList").value()], click: eliminarCaptura }, title: "", width: "99px" }
        ]
    });

    CustomisaGrid($("#grid"));
}

function eliminarCapturaDetalleSpool(e) {
    e.preventDefault();
    if ($('#botonGuardar').text() == _dictionary.MensajeGuardar[$("#language").data("kendoDropDownList").value()]) {

        var filterValue = $(e.currentTarget).val();
        var dataItem = $("#grid").data("kendoGrid").dataItem($(e.currentTarget).closest("tr"));
        var spoolIDRegistro = dataItem.SpoolID;


        windowTemplate = kendo.template($("#windowTemplate").html());

        ventanaConfirm = $("#ventanaConfirm").kendoWindow({
            iframe: true,
            title: _dictionary.WarningTitle[$("#language").data("kendoDropDownList").value()],
            visible: false, //the window will not appear before its .open method is called
            width: "auto",
            height: "auto",
            modal: true
        }).data("kendoWindow");

        ventanaConfirm.content(_dictionary.ProyeccionBorradoCaptura[$("#language").data("kendoDropDownList").value()] +
                    "</br><center><button class='btn btn-blue' id='yesButton'>Si</button><button class='btn btn-blue' id='noButton'> No</button></center>");

        ventanaConfirm.open().center();

        $("#yesButton").click(function () {
            var dataSource = $("#grid").data("kendoGrid").dataSource;
            dataItem.Accion = 3;

            if (dataItem.PinturaSpoolID === 0)
            { dataSource.remove(dataItem); }

            dataSource.sync();
            ventanaConfirm.close();
        });
        $("#noButton").click(function () {
            ventanaConfirm.close();
        });
    }
}