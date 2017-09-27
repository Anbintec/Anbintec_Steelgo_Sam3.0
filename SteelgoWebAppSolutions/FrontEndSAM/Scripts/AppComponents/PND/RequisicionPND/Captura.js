var endRangeDate;
var ItemSeleccionado;
var pruebasID, ProyectoID;
var requisicionID, pruebaProyID = 0;
var ProyectoNombre = "";
var PruebaNombre = "";
var EstatusID = 1; // Capturada segun tabla Sam3_Estatus

if ($("#idField").val() != null || $("#idField").val() != undefined)
    requisicionID = $("#idField").val();
else
    requisicionID = 0;

IniciarCaptura();

function IniciarCaptura() {
    //AltaFecha();
    SuscribirEventos();
};

function changeLanguageCall() {
    //endRangeDate.data("kendoDatePicker").setOptions({
    //    format: _dictionary.FormatoFecha[$("#language").data("kendoDropDownList").value()]
    //});

    SiguienteProceso("");
    CargarGrid();
    $("#inputTipoPrueba").data("kendoComboBox").value("");
    $("#inputTipoPrueba").data("kendoComboBox").value("");
    $("#inputProyecto").data("kendoComboBox").enable(true);
    $("#inputTipoPrueba").data("kendoComboBox").enable(true);
    $('#containerDiv').css('display', 'none');
    $('#grid').data('kendoGrid').dataSource.read();
    //ajaxRequisicion();

    //ajaxObtenerProyectos();
    setTimeout(function () { AjaxCargarCamposPredeterminados() }, 1000);
    document.title = _dictionary.ServiciosTecnicosRequisicionPND[$("#language").data("kendoDropDownList").value()];
};

function SiguienteProceso(paramReq) {
    var url = "";
    if (paramReq == 0 || paramReq == undefined || paramReq == "") {
        url = "/PND/AsignacionRequisicion?leng=" + $("#language").data("kendoDropDownList").value();
    } else {
        url = "/PND/AsignacionRequisicion?leng=" + $("#language").data("kendoDropDownList").value()
            + "&requisicion=" + paramReq;
    }

    $("#GenerarRequisicionASignarRequisicionSup").attr("href", url);
    $("#GenerarRequisicionASignarRequisicionInf").attr("href", url);
}

function cargaInicialRequisicionEditar() {
    if (requisicionID != 0) {
        $("#inputProyecto").data("kendoComboBox").value(ProyectoID);
        ajaxObtenerTipoPruebasRequisicionEdicion();
    }
}

function CargarGrid() {
    $("#grid").kendoGrid({
        autoBind: true,
        autoSync: true,
        edit: function (e) {
            setTimeout(function () {
                var inputName = e.container.find('input');

                inputName.select();
            });
            if ($('#botonGuardar').text() != _dictionary.MensajeGuardar[$("#language").data("kendoDropDownList").value()])
                this.closeCell();
        },
        dataSource: {
            data: '',
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
                        DiametroPlano: { type: "number", editable: false },
                        Espesor: { type: "number", editable: false },
                        Cedula: { type: "string", editable: false },
                        Codigo: { type: "string", editable: false },

                        ElementoPorClasificacionPNDID: { type: "int", editable: false },
                        RequisicionID: { type: "int", editable: false },
                        ProyectoID: { type: "int", editable: false },
                        SpoolID: { type: "int", editable: false },
                        JuntaSpoolID: { type: "int", editable: false },
                        OrdenTrabajoSpoolID: { type: "int", editable: false },
                        TipoPruebaID: { type: "int", editable: false },
                        Especificacion: { type: "string", editable: false },
                        Agregar: { type: "boolean", editable: false },
                        ConfiguracionManual: { type: "int", editable: false },
                        OrdenTrabajoID: { type: "int", editable: false }
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
            { field: "NumeroControl", title: _dictionary.columnNumeroControl[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "100px" },
            { field: "EtiquetaJunta", title: _dictionary.columnJunta[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "65px" },
            { field: "TipoJunta", title: _dictionary.columnTipoJta[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "70px" },
            { field: "NombreRequisicion", title: _dictionary.columnRequisicion[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "125px" },
            { field: "Cuadrante", title: _dictionary.columnCuadrante[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "114px" },
            { field: "Prioridad", title: _dictionary.columnPrioridad[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellNumberMaftec(), width: "80px", attributes: { style: "text-align:right;" } },
            { field: "Clasificacion", title: _dictionary.columnClasificacion[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "72px" },
            { field: "DiametroPlano", title: _dictionary.columnDiametro[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellNumberMaftec(), width: "80px", attributes: { style: "text-align:right;" } },
            { field: "Espesor", title: _dictionary.columnEspesor[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellNumberMaftec(), width: "90px", attributes: { style: "text-align:right;" } },
            { field: "Cedula", title: _dictionary.columnCedula[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "90px" },
            { field: "Especificacion", title: _dictionary.columnEspec[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "85px" },
            { field: "Codigo", title: _dictionary.columnCodigoAsme[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "125px" },
            {
                field: "Agregar", title: _dictionary.columnAgregar[$("#language").data("kendoDropDownList").value()], filterable: {
                    multi: true,
                    messages: {
                        isTrue: _dictionary.lblVerdadero[$("#language").data("kendoDropDownList").value()],
                        isFalse: _dictionary.lblFalso[$("#language").data("kendoDropDownList").value()],
                        style: "max-width:100px;"
                    },
                    dataSource: [{ Etiquetado: true }, { Etiquetado: false }]
                }, template: "<input name='fullyPaid' class='ob-paid' type='checkbox' #= (Agregar && (RequisicionID != 0)) ? 'hidden=true':(Agregar ? 'checked=checked':'') # />", width: "100px", attributes: { style: "text-align:center;" }
            },
        ],
        dataBound: function (a) {
            $(".ob-paid").bind("change", function (e) {
                if ($('#botonGuardar').text() == _dictionary.MensajeGuardar[$("#language").data("kendoDropDownList").value()]) {
                    var grid = $("#grid").data("kendoGrid"),
                        dataItem = grid.dataItem($(e.target).closest("tr"));
                    if (dataItem.RequisicionID == 0 && e.target.checked == true)
                        dataItem.Agregar = true;
                    else
                        dataItem.Agregar = false;
                }
                else {
                    if (e.target.checked) {
                        e.target.checked = false;   
                        $("#grid").data("kendoGrid").dataItem($(e.target).closest("tr")).Agregar = false;
                    }
                    else {
                        e.target.checked = true;
                        $("#grid").data("kendoGrid").dataItem($(e.target).closest("tr")).Agregar = true;
                    }
                        
                }

                    $("#grid").data("kendoGrid").closeCell();
                $("#grid").data("kendoGrid").dataSource.sync();
            });
        }
    });

    $("#grid").on("change", ":checkbox", function (e) {
        if ($('#Guardar').text() == _dictionary.MensajeGuardar[$("#language").data("kendoDropDownList").value()]) {
            var grid = $("#grid").data("kendoGrid"),
            dataItem = grid.dataItem($(e.target).closest("tr"));
            if (dataItem.RequisicionID != 0 && dataItem.RequisicionID != undefined) {
                $(this)[0].checked = false;
                displayNotify("", "Este elemento no puede ser asignado a otra requisición", '1');
            }

            //$("#grid").data("kendoGrid").dataSource.sync();
        }
        //else {
        //    if ($(this)[0].checked) {
        //        $(this)[0].checked = false;
        //    }
        //    else {
        //        $(this)[0].checked = true;
        //    }
        //}
    });

    CustomisaGrid($("#grid"));
};

function AltaFecha() {
    endRangeDate = $("#Fecha").kendoDatePicker({
        max: new Date(),
    });
    $("#Fecha").data("kendoDatePicker").enable(false);
}

function ExisteSpool() {
    var jsonGrid = $("#grid").data("kendoGrid").dataSource._data;

    for (var i = 0; i < jsonGrid.length; i++) {
        if ( jsonGrid[i].OrdenTrabajoSpoolID ==  $("#InputID").data("kendoComboBox").value()) {
          
            return true;
        }
    }
    return false;
}

function ExisteJunta() {
    var jsonGrid = $("#grid").data("kendoGrid").dataSource._data;

    for (var i = 0; i < jsonGrid.length; i++) {
        if (jsonGrid[i].OrdenTrabajoSpoolID == $("#InputID").data("kendoComboBox").value() && jsonGrid[i].JuntaSpoolID == $("#Junta").data("kendoComboBox").value()) {
            return true;
        }
    }
    return false;
}

function AgregarJuntaNueva() {
    if ($("#inputTipoPrueba").data("kendoComboBox").dataItem($("#inputTipoPrueba").data("kendoComboBox").select()).TipoPruebaPorSpool == 1) {
        if (!ExisteSpool()) {
            AjaxObtenerSpool();
        }
        else
            displayNotify("GenerarRequisicionMensajeExisteSpool", "", '1');
    }
    else {
        if (!ExisteJunta()) {
            AjaxObtenerJunta();
        }
        else
            displayNotify("GenerarRequisicionMensajeExisteJunta", "", '1');
    }
}

function ValidaFormatoFecha(FechaValidar, Idioma) {

    //Valida que el formato de la fecha sea correcto (2-2-4)
    var bool;
    var RegExPattern = /^\d{1,2}\/\d{1,2}\/\d{2,4}$/;
    if ((String(FechaValidar).trim().match(RegExPattern)) && (FechaValidar != '')) {

        if (Idioma == 'es-MX') {
            if (existeFechaMexicoFormato(FechaValidar) && existeFechaMexico(FechaValidar)) {
                bool = true;
            }
            else {
                bool = false
            }
        }
        else if (Idioma == 'en-US') {

            if (existeFechaEUFormato(FechaValidar) && existeFechaEU(FechaValidar))
                bool = true;
            else
                bool = false
        }
    } else
        bool = false;
    return bool;

}

function existeFechaMexicoFormato(fecha) {
    var fechaf = fecha.split("/");
    var d = fechaf[0];
    var m = fechaf[1];
    var y = fechaf[2];

    return m > 0 && m < 13 && y > 0 && y < 32768 && d > 0 && d <= (new Date(y, m, 0)).getDate();
}

function existeFechaMexico(FechaValidar) {
    var fechaf = FechaValidar.split("/");
    var day = FechaValidar[0];
    var month = FechaValidar[1];
    var year = FechaValidar[2];
    var date = new Date(year, month, '0');
    if ((day - 0) > (date.getDate() - 0))
        return false;
    return true;
}

function existeFechaEUFormato(fecha) {
    var fechaf = fecha.split("/");
    var d = fechaf[1];
    var m = fechaf[0];
    var y = fechaf[2];

    return m > 0 && m < 13 && y > 0 && y < 32768 && d > 0 && d <= (new Date(y, m, 0)).getDate();
}

function existeFechaEU(FechaValidar) {
    var fechaf = FechaValidar.split("/");
    var day = FechaValidar[1];
    var month = FechaValidar[0];
    var year = FechaValidar[2];
    var date = new Date(year, month, '0');
    if ((day - 0) > (date.getDate() - 0))
        return false;
    return true;
}

function tieneClase(item) {
    for (var i = 0; i < item.classList.length; i++) {
        if (item.classList[i] == "k-state-border-up" || item.classList[i] == "k-state-border-down")
            return true;
    }
    return false;
}

function existenCambios(arregloCaptura) {
    for (index = 0; index < arregloCaptura.length; index++) {
        if (arregloCaptura[index].Agregar == true && arregloCaptura[index].RequisicionID == 0)
            return true;
    }
    return false;
}



function PlanchaAgregar(Agregar) {

    var dataSource = $("#grid").data("kendoGrid").dataSource;
    var filters = dataSource.filter();
    var allData = dataSource.data();
    var query = new kendo.data.Query(allData);
    var data = query.filter(filters).data;

    for (var i = 0; i < data.length; i++) {
        if ($('input:radio[name=LLena]:checked').val() === "Todos") {
            if (Agregar == "Si" && data[i].RequisicionID == 0) {
                data[i].Agregar = true;
                


            } else if (Agregar == "No" && data[i].RequisicionID == 0) {
                data[i].Agregar = false;

            }
        } else if ($('input:radio[name=LLena]:checked').val() === "Vacios") {
            if (!data[i].Agregar && data[i].RequisicionID == 0) {
                if (Agregar == "Si") {
                    data[i].Agregar = true;
                }
            }
        }
    }
    $("#grid").data("kendoGrid").refresh();
}