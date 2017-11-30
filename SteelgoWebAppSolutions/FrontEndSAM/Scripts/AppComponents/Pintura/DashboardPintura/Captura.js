IniciarDashboardPintura();
function IniciarDashboardPintura() {
    SuscribirEventos();
    ConvertirInputs();
    mostrarFiltro();
    $("#inputFechaInicial").val("");
    $("#inputFechaFinal").val("");
}

function changeLanguageCall() {
    AjaxCargarPeriodos();
    AjaxObtenerProyectos();
    AjaxGetClientes();

    document.title = _dictionary.menuServiciosTecnicosDashboardPND[$("#language").data("kendoDropDownList").value()];
    $("#inputFechaInicial").data("kendoDatePicker").setOptions({
        format: _dictionary.FormatoFecha2[$("#language").data("kendoDropDownList").value()]
    });
    $("#inputFechaFinal").data("kendoDatePicker").setOptions({
        format: _dictionary.FormatoFecha2[$("#language").data("kendoDropDownList").value()]
    });
    OcultarCampos(true);


    //AjaxCargarHeaderDashboard();
    CargarGrid();
    //CargarGridPopUp();
    //document.title = _dictionary.PinturaDashboardBreadcrumb[$("#language").data("kendoDropDownList").value()];

}

function ConvertirInputs() {
    $("#inputCantidadPeriodo").kendoNumericTextBox({
        format: "#",
        decimals: 0,
        min: 0
    });

    $("#inputTipoPeriodo").kendoComboBox({
        dataSource: {
            data: ["", "Dias", "Meses", "Años"]
        }
    });
}

function CargarGrid() {
    $("#grid").kendoGrid({
        edit: function (e) {

                var inputName = e.container.find('input');
                inputName.select();

            if ($('#Guardar').text() == _dictionary.MensajeGuardar[$("#language").data("kendoDropDownList").value()]) {
            } else
                this.closeCell();
        },
        dataBound: function () {
           
        },
        dataSource: {
            data: [],
            schema: {
                model: {
                    fields: {
                        NumeroControl: { type: "string", editable: false },
                        Cuadrante: { type: "string", editable: false },
                        Prioridad: { type: "number", editable: false },
                        Area: { type: "number", editable: false },
                        Peso: { type: "number", editable: false },
                        SistemaPintura: { type: "string", editable: false },
                        Proyecto: { type: "string", editable: false },
                        Carro: { type: "string", editable: false },
                        Color: { type: "string", editable: false },
                        CantidadSpools: { type: "number", editable: false },
                        Prueba: { type: "string", editable: false },

                    }
                }
            },
            pageSize: 10,
            serverPaging: false,
            serverFiltering: false,
            serverSorting: false
        },
        navigatable: true,
        autoHeight: true,
        sortable: true,
        scrollable: true,
        editable: true,
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
             
             { field: "NumeroControl", title: _dictionary.columnNumeroControl[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(),},
             { field: "Prioridad", title: _dictionary.columnPrioridad[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellNumberMaftec(),  attributes: { style: "text-align:right;" } },

             { field: "SistemaPintura", title: _dictionary.columnSistemaPintura[$("#language").data("kendoDropDownList").value()],  filterable: getGridFilterableCellMaftec(),},
             { field: "Proyecto", title: _dictionary.columnProyecto[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), },
             { field: "Carro", title: "Carro", filterable: getGridFilterableCellMaftec() },
             { field: "Color", title: _dictionary.columnColor[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), attributes: { style: "text-align:center;" } },

             { field: "Cuadrante", title: _dictionary.columnCuadrante[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(),},
             { field: "M2", title: _dictionary.columnM2[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellNumberMaftec(), attributes: { style: "text-align:right;" }, },
             { field: "Peso", title: _dictionary.columnPeso[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellNumberMaftec(), attributes: { style: "text-align:right;" }, },

             { field: "CantidadSpools", title: "Cantidad Spools", filterable: getGridFilterableCellNumberMaftec(),  },

             { field: "Prueba", title: _dictionary.columnPrueba[$("#language").data("kendoDropDownList").value()], template: "<div class='EnlaceDetallePruebas shot' style='text-align:center;'><a href='\\#'  > <span>Detalle</span></a></div>", filterable: false,  },
        ]
    });
    CustomisaGrid($("#grid"));
};





function OcultarCampos(Visible) {
    if (!Visible) {
        $("#tabEstatus").css("display", "block");
        $("#tabEstatus2").css("display", "block");
        $("#ContenedorGrid").css("display", "block");
        $("#contenidoDashboard").css("display", "none");
        $("#contenidoDashboardTab").css("display", "none");
    } else {
        $("#tabEstatus").css("display", "none");
        $("#tabEstatus2").css("display", "none");
        $("#ContenedorGrid").css("display", "none");
    }
}

function ActivarRefrescarGrid(idBoton) {
    $("#contenidoDashboard").css('display', 'block');
    $("#contenidoDashboardTab").css("display", "block");
    //$("#tabEstatus").css("display", "none");
    $("#tabEstatus2").css("display", "none");
    tabActivo(idBoton);


    // POR CARGAR
    if (idBoton == 1) {
        $("#grid").data("kendoGrid").showColumn("NumeroControl");
        $("#grid").data("kendoGrid").showColumn("Prioridad");
        $("#grid").data("kendoGrid").showColumn("Cuadrante");
        $("#grid").data("kendoGrid").showColumn("Area");
        $("#grid").data("kendoGrid").showColumn("Peso");
        
        $("#grid").data("kendoGrid").hideColumn("SistemaPintura");
        $("#grid").data("kendoGrid").hideColumn("Proyecto");
        $("#grid").data("kendoGrid").hideColumn("Carro");
        $("#grid").data("kendoGrid").hideColumn("Color");
        $("#grid").data("kendoGrid").hideColumn("CantidadSpools");
        $("#grid").data("kendoGrid").hideColumn("Prueba");
    }
        // POR CAPTURAR PRIMARIO Y SHOTBLAST
    else if (idBoton == 2 || idBoton == 4) {

        $("#grid").data("kendoGrid").hideColumn("NumeroControl");
        $("#grid").data("kendoGrid").hideColumn("Prioridad");
        $("#grid").data("kendoGrid").hideColumn("Cuadrante");
        $("#grid").data("kendoGrid").hideColumn("Area");
        $("#grid").data("kendoGrid").hideColumn("Peso");

        
        $("#grid").data("kendoGrid").showColumn("Proyecto");
        $("#grid").data("kendoGrid").showColumn("SistemaPintura");
        $("#grid").data("kendoGrid").showColumn("Carro");
        $("#grid").data("kendoGrid").hideColumn("Color");
        $("#grid").data("kendoGrid").hideColumn("Prueba");
        $("#grid").data("kendoGrid").showColumn("CantidadSpools");
        
    }
        // POR PROBAR PRIMARIO Y SHOTBLAST
    else if (idBoton == 3 || idBoton == 6) {

        $("#grid").data("kendoGrid").showColumn("NumeroControl");
        $("#grid").data("kendoGrid").showColumn("Prioridad");
        $("#grid").data("kendoGrid").showColumn("SistemaPintura");
        $("#grid").data("kendoGrid").showColumn("Area");
        $("#grid").data("kendoGrid").showColumn("Peso");

        $("#grid").data("kendoGrid").hideColumn("Cuadrante");
        $("#grid").data("kendoGrid").hideColumn("Proyecto");
        $("#grid").data("kendoGrid").hideColumn("Carro");
        $("#grid").data("kendoGrid").hideColumn("Color");
        $("#grid").data("kendoGrid").hideColumn("CantidadSpools");
        $("#grid").data("kendoGrid").hideColumn("Prueba");

    }


        // POR DESCARGAR PRIMARIO Y SHOTBLAST
    else if (idBoton == 6 || idBoton == 12) {

        $("#grid").data("kendoGrid").showColumn("NumeroControl");
        $("#grid").data("kendoGrid").hideColumn("Prioridad");
        $("#grid").data("kendoGrid").hideColumn("Cuadrante");
        $("#grid").data("kendoGrid").hideColumn("Area");
        $("#grid").data("kendoGrid").hideColumn("Peso");


        $("#grid").data("kendoGrid").showColumn("Proyecto");
        $("#grid").data("kendoGrid").showColumn("SistemaPintura");
        $("#grid").data("kendoGrid").showColumn("Carro");
        $("#grid").data("kendoGrid").hideColumn("Color");
        $("#grid").data("kendoGrid").hideColumn("Prueba");
        $("#grid").data("kendoGrid").hideColumn("CantidadSpools");

    }
        // POR CAPTURAR INTERMEDIO Y ACABADO
    else if (idBoton == 7 || idBoton == 9) {

        $("#grid").data("kendoGrid").hideColumn("NumeroControl");
        $("#grid").data("kendoGrid").hideColumn("Prioridad");
        $("#grid").data("kendoGrid").showColumn("Cuadrante");
        $("#grid").data("kendoGrid").hideColumn("Area");
        $("#grid").data("kendoGrid").hideColumn("Peso");


        $("#grid").data("kendoGrid").showColumn("Proyecto");
        $("#grid").data("kendoGrid").showColumn("SistemaPintura");
        $("#grid").data("kendoGrid").hideColumn("Carro");
        $("#grid").data("kendoGrid").showColumn("Color");
        $("#grid").data("kendoGrid").hideColumn("Prueba");
        $("#grid").data("kendoGrid").showColumn("CantidadSpools");

    }
        // POR PROBAR INTERMEDIO Y ACABADO
    else if (idBoton == 8 || idBoton == 10) {

        $("#grid").data("kendoGrid").showColumn("NumeroControl");
        $("#grid").data("kendoGrid").showColumn("Prioridad");
        $("#grid").data("kendoGrid").showColumn("SistemaPintura");
        $("#grid").data("kendoGrid").showColumn("Area");
        $("#grid").data("kendoGrid").showColumn("Peso");
        $("#grid").data("kendoGrid").showColumn("Color");

        $("#grid").data("kendoGrid").hideColumn("Cuadrante");
        $("#grid").data("kendoGrid").hideColumn("Proyecto");
        $("#grid").data("kendoGrid").hideColumn("Carro");
        $("#grid").data("kendoGrid").hideColumn("CantidadSpools");
        $("#grid").data("kendoGrid").hideColumn("Prueba");

    }

        // OK PINTURA
    else if (idBoton == 11) {

        $("#grid").data("kendoGrid").showColumn("NumeroControl");
        $("#grid").data("kendoGrid").showColumn("Prioridad");
        $("#grid").data("kendoGrid").showColumn("Cuadrante");
        $("#grid").data("kendoGrid").hideColumn("Area");
        $("#grid").data("kendoGrid").hideColumn("Peso");


        $("#grid").data("kendoGrid").hideColumn("Proyecto");
        $("#grid").data("kendoGrid").hideColumn("SistemaPintura");
        $("#grid").data("kendoGrid").hideColumn("Carro");
        $("#grid").data("kendoGrid").hideColumn("Color");
        $("#grid").data("kendoGrid").showColumn("Prueba");
        $("#grid").data("kendoGrid").hideColumn("CantidadSpools");

    }


        

    AjaxAccionesListado(idBoton);
}



function mostrarFiltro() {
    if ($("input:radio[name=Muestra]:nth(0)").is(":checked")) {
        $(".filtroSpool").css("display", "block");
        $(".filtroM2").css("display", "none");
        $(".filtroTon").css("display", "none");
    }
    else if ($("input:radio[name=Muestra]:nth(1)").is(":checked")) {
        $(".filtroSpool").css("display", "none");
        $(".filtroM2").css("display", "block");
        $(".filtroTon").css("display", "none");
    }
    else if ($("input:radio[name=Muestra]:nth(2)").is(":checked")) {
        $(".filtroSpool").css("display", "none");
        $(".filtroM2").css("display", "none");
        $(".filtroTon").css("display", "block");
    }
}


function tabActivo(idButton) {
    $(".btn-tabList").removeClass("active");
    var list = document.getElementById("contenidoDashboardTab").getElementsByTagName("button");

    for (var i = 0; i < list.length; i++) {
        if (list[i].id == idButton) {
            $("#" + idButton).addClass("active");
            break;
        }
    }
};