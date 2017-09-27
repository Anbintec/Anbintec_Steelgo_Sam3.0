function AjaxObtenerProyectos() {
    loadingStart();

    $Proyectos.Proyectos.read({ token: Cookies.get("token") }).done(function (data) {
        if (Error(data)) {

            $("#inputProyecto").data("kendoComboBox").text("");
            $("#inputProyecto").data("kendoComboBox").dataSource.data(data);

            if ($("#inputProyecto").data("kendoComboBox").dataSource._data.length == 2) {
                $("#inputProyecto").data("kendoComboBox").select(1);
                AjaxJuntas();
            }
            else {
                $("#inputProyecto").data("kendoComboBox").select(0);
                loadingStop();
            }

        }

    });
}


var data = [{
    SpoolID: "X001-001",
    Junta: "1",
    Descripcion: "STD - Espesor: 8.1820 - Acero: CS, Loc: 4-2 - Tipo: BW",
    Diametro: "8",
    DefectosAsignados: false,
    TemplateDetalleElemento: "Detalle"
},
            {
                SpoolID: "X001-001",
                Junta: "2",
                Descripcion: "STD - Espesor: 9.5290 - Acero: CS - Loc: 2-1 - Tipo: BW",
                Diametro: "14",
                DefectosAsignados: false,
                TemplateDetalleElemento: "Detalle"
            },
            {
                SpoolID: "X001-001",
                Junta: "3",
                Descripcion: "STD - Espesor: 9.5290 - Acero: CS, Loc: 1-3 - Tipo: BW",
                Diametro: "14",
                DefectosAsignados: false,
                TemplateDetalleElemento: "Detalle"
            },
            {
                SpoolID: "X001-002",
                Junta: "5",
                Descripcion: "STD - Espesor: 9.5290 - Acero: CS, Loc: 1-3 - Tipo: BW",
                Diametro: "12",
                DefectosAsignados: false,
                TemplateDetalleElemento: "Detalle"
            },
            {
                SpoolID: "X001-002",
                Junta: "6",
                Descripcion: "STD - Espesor: 9.5290 - Acero: CS, Loc: 3-4 - Tipo: BW",
                Diametro: "9",
                DefectosAsignados: false,
                TemplateDetalleElemento: "Detalle"
            }
];

function AjaxJuntas(ProyectoID) {

    loadingStart();
    setTimeout(function () {
        $("#grid").data("kendoGrid").dataSource.data([]);
        var ds = $("#grid").data("kendoGrid").dataSource;
        if (ProyectoID == 16) {
            
            var array = data;
            if (data.length > 0) {
                ds.data(data);
            }
        }
        ds.sync();
        loadingStop();
        editado = false;
    }, 700);
}



function AjaxGuardarCaptura() {
    loadingStart();
    setTimeout(function () {
        opcionHabilitarView(true);
        editado = false;
        displayNotify("EntregaPlacasGraficasMensajeGuardadoExistoso", "", '0');
        loadingStop();
    }, 700);
}