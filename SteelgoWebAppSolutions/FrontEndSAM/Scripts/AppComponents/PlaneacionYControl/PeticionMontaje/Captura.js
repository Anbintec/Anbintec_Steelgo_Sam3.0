function changeLanguageCall() {
    CargarGrid();
    AltaFecha();
    $("#Proyecto").data("kendoComboBox").value("");
    $("#Nombre").val("");
    AjaxObtenerProyectos();
};


function AltaFecha() {
    endRangeDate = $("#FechaDeseable").kendoDatePicker({
        max: new Date(),
        date: new Date()
    });
}

function CargarGrid() {
    $("#grid").kendoGrid({
        autoBind: true,
        dataSource: {
            data: [{
                Spool: "x",
                SpoolID: "X1-002",
                GrupoEmbarque: "PND",
                Emitido: "SÍ",
                Junta: "2",
                m2: "10",
                Peqs: "2",
                Kg: "30",
                Area: "2",
                TipoMateriales: "",
                Peticion: "Petición 1",
                FechaPeticion: "18/02/2016",
                Check: true,
                Peso: "350",
                Area: "623",
                M2: "623",
                Juntas: "4",
                Peqs: "162"
            },
            {
                Spool: "x",
                SpoolID: "X2-016",
                GrupoEmbarque: "PND",
                Emitido: "NO",
                Junta: "3",
                m2: "4",
                Peqs: "30",
                Kg: "20",
                Area: "3",
                TipoMateriales: "",
                Peticion: "Petición 1",
                FechaPeticion: "18/02/2016",
                Seleccionado: false,
                Peso: "350",
                Area: "623",
                M2: "623",
                Juntas: "4",
                Peqs: "162"
            },
            {
                Spool: "x",
                SpoolID: "X2-004",
                GrupoEmbarque: "PND",
                Emitido: "NO",
                Junta: "1",
                m2: "20",
                Peqs: "23.2",
                Kg: "15",
                Area: "2",
                TipoMateriales: "",
                Peticion: "",
                FechaPeticion: "",
                Seleccionado: false,
                Peso: "350",
                Area: "623",
                M2: "623",
                Juntas: "4",
                Peqs: "162"
            }, ],
            schema: {
                model: {
                    fields: {
                        Spool: { type: "string", editable: false },
                        SpoolID: { type: "string", editable: false },
                        GrupoEmbarque: { type: "string", editable: false },
                        Emitido: { type: "string", editable: false },
                        Junta: { type: "string", editable: false },
                        m2: { type: "string", editable: false },
                        Peqs: { type: "string", editable: false },
                        Kg: { type: "string", editable: false },
                        Area: { type: "string", editable: false },
                        TipoMateriales: { type: "string", editable: false },
                        Peticion: { type: "string", editable: false },
                        FechaPeticion: { type: "date", editable: false },
                        Seleccionado: { type: "boolean", editable: false },
                        Peso: { type: "string", editable: false },
                        Area: { type: "string", editable: false },
                        M2: { type: "string", editable: false },
                        Juntas: { type: "string", editable: false },
                        Peqs: { type: "string", editable: false }

                    }
                }
            },
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
            { field: "Seleccionado", title: "", filterable: false, template: "<input name='fullyPaid' class='chk-seleccion' type='checkbox' data-bind='checked: Seleccionado' #= Seleccionado ? checked='checked' : '' #/>", width: "120px" },
            { field: "Spool", title: _dictionary.PeticionMotajeHeaderSpool[$("#language").data("kendoDropDownList").value()], filterable: true, width: "100px" },
            { field: "SpoolID", title: _dictionary.PeticionMotajeHeaderSpoolID[$("#language").data("kendoDropDownList").value()], filterable: true, width: "100px" },
            { field: "GrupoEmbarque", title: _dictionary.PeticionMotajeHeaderGrupoEmbarque[$("#language").data("kendoDropDownList").value()], filterable: true, width: "180px" },
            { field: "Emitido", title: _dictionary.PeticionMotajeHeaderEmitido[$("#language").data("kendoDropDownList").value()], filterable: true, width: "100px" },
            { field: "Junta", title: _dictionary.JuntaGrid[$("#language").data("kendoDropDownList").value()], filterable: false, width: "55px" },
            { field: "m2", title: _dictionary.PeticionMotajeHeaderM2[$("#language").data("kendoDropDownList").value()], filterable: false, width: "100px" },
            { field: "Peqs", title: _dictionary.PeticionMotajeHeaderPeqs[$("#language").data("kendoDropDownList").value()], filterable: false, width: "100px" },
            { field: "Kg", title: _dictionary.PeticionMotajeHeaderKg[$("#language").data("kendoDropDownList").value()], filterable: false, width: "100px" },
            { field: "Area", title: _dictionary.PeticionMotajeHeaderArea[$("#language").data("kendoDropDownList").value()], filterable: false, width: "100px" },
            { field: "TipoMateriales", title: _dictionary.PeticionMotajeHeaderTipoMateriales[$("#language").data("kendoDropDownList").value()], filterable: false, width: "120px" },
            { field: "Peticion", title: _dictionary.PeticionMotajeHeaderPeticion[$("#language").data("kendoDropDownList").value()], filterable: false, width: "100px" },
            { field: "FechaPeticion", title: _dictionary.PeticionMotajeHeaderFechaPeticion[$("#language").data("kendoDropDownList").value()], filterable: false, format: _dictionary.FormatoFecha[$("#language").data("kendoDropDownList").value()], width: "130px" },

        ]
    });

    $("#grid .k-grid-content").on("change", "input.chk-seleccion", function (e) {
        if ($("#language").val() == "es-MX") {
            if ($('#Guardar').text() != "Editar") {
                var grid = $("#grid").data("kendoGrid")
                dataItem = grid.dataItem($(e.target).closest("tr"));
                if ($(this)[0].checked) {
                    dataItem.Seleccionado = true;
                }
                else {
                    dataItem.Seleccionado = false;
                }
                $("#grid").data("kendoGrid").dataSource.sync();
            }
            else {
                if ($(this)[0].checked) {
                    $(this)[0].checked = false;
                }
                else {
                    $(this)[0].checked = true;
                }
            }
        }
        else {
            if ($('#Guardar').text() != "Edit") {
                var grid = $("#grid").data("kendoGrid")
                dataItem = grid.dataItem($(e.target).closest("tr"));
                if ($(this)[0].checked) {
                    dataItem.Seleccionado = true;
                }
                else {
                    dataItem.Seleccionado = false;
                }
                $("#grid").data("kendoGrid").dataSource.sync();
            }
            else {
                if ($(this)[0].checked) {
                    $(this)[0].checked = false;
                }
                else {
                    $(this)[0].checked = true;
                }
            }
        }
    });


    CustomisaGrid($("#grid"));
};