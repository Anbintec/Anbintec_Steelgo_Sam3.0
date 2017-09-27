var proyectoInicial = 0;
var pruebaOriginal = 0;
var requisicionOriginal = 0;
var editado = false;

function SuscribirEventos() {
    SuscribirEventoComboPrueba();
    suscribirEventoGuardar();
    SuscribirEventoComboRequisicion();
    SuscribirEventoComboProyecto();
};

function suscribirEventoGuardar() {

}


function SuscribirEventoComboPrueba() {
    $('#inputPrueba').kendoComboBox({
        dataTextField: "Nombre",
        dataValueField: "TipoPruebaID",
        suggest: true,
        filter: "contains",
        index: 3,
        change: function (e) {
            dataItem = this.dataItem(e.sender.selectedIndex);
            if (!editado) {
                if (dataItem != undefined) {
                    $("#inputRequisicion").data("kendoComboBox").setDataSource();
                    $("#inputRequisicion").data("kendoComboBox").value("");
                    $("#inputRequisicion").data("kendoComboBox").dataSource.data([]);
                    AjaxRequisicion();
                    pruebaOriginal = $("#inputPrueba").data("kendoComboBox").value();
                    if (dataItem.Nombre == "") {
                        $("#grid").data("kendoGrid").dataSource.data([]);
                    }
                }
                else {
                    $("#inputRequisicion").data("kendoComboBox").setDataSource();
                    $("#inputRequisicion").data("kendoComboBox").value("");
                    $("#inputRequisicion").data("kendoComboBox").dataSource.data([]);
                    $("#grid").data("kendoGrid").dataSource.data([]);
                }
            }
            else {
                var ventanaConfirm = $("#ventanaConfirmCaptura").kendoWindow({
                    iframe: true,
                    title: _dictionary.EntregaPlacasGraficasTituloPopup[$("#language").data("kendoDropDownList").value()],
                    visible: false,
                    width: "auto",
                    height: "auto",
                    modal: true,
                    animation: {
                        close: false,
                        open: false
                    },
                    actions: [],

                }).data("kendoWindow");

                ventanaConfirm.content(_dictionary.EntregaPlacasGraficasMensajeDatosCapturadosNoGuardados[$("#language").data("kendoDropDownList").value()] +
                    "</br><center><button class='btn btn-blue' id='yesButtonProy'>" + _dictionary.EntregaPlacasGraficasbotonSi[$("#language").data("kendoDropDownList").value()] + "</button><button class='btn btn-blue' id='noButtonProy'>" + _dictionary.EntregaPlacasGraficasbotonNo[$("#language").data("kendoDropDownList").value()] + "</button></center>");

                ventanaConfirm.open().center();
                $("#yesButtonProy").click(function () {
                    if (dataItem != undefined) {
                        $("#inputRequisicion").data("kendoComboBox").setDataSource();
                        $("#inputRequisicion").data("kendoComboBox").value("");
                        $("#inputRequisicion").data("kendoComboBox").dataSource.data([]);
                        AjaxRequisicion();
                        pruebaOriginal = $("#inputPrueba").data("kendoComboBox").value();
                        if (dataItem.Nombre == "") {
                            $("#grid").data("kendoGrid").dataSource.data([]);
                        }
                    }
                    else {
                        $("#inputPrueba").data("kendoComboBox").text("");
                        $("#inputRequisicion").data("kendoComboBox").setDataSource();
                        $("#inputRequisicion").data("kendoComboBox").value("");
                        $("#inputRequisicion").data("kendoComboBox").dataSource.data([]);
                        $("#grid").data("kendoGrid").dataSource.data([]);
                    }
                    ventanaConfirm.close();

                });
                $("#noButtonProy").click(function () {
                    $("#inputPrueba").data("kendoComboBox").value(pruebaOriginal);
                    ventanaConfirm.close();
                });
            }
        }
    });

};




function SuscribirEventoComboProyecto() {
    $('#inputProyecto').kendoComboBox({
        dataTextField: "Nombre",
        dataValueField: "ProyectoID",
        suggest: true,
        filter: "contains",
        index: 3,
        change: function (e) {
            dataItem = this.dataItem(e.sender.selectedIndex);
            if (dataItem != undefined || dataItem.Nombre != "") {
                if (!editado) {
                    AjaxPruebas();
                    $("#inputRequisicion").data("kendoComboBox").setDataSource();
                    $("#inputRequisicion").data("kendoComboBox").value("");
                    $("#inputRequisicion").data("kendoComboBox").dataSource.data([]);
                    $("#inputPrueba").data("kendoComboBox").setDataSource();
                    $("#inputPrueba").data("kendoComboBox").value("");
                    $("#inputPrueba").data("kendoComboBox").dataSource.data([]);
                    proyectoInicial = $("#inputProyecto").data("kendoComboBox").value();

                }
                else {
                    var ventanaConfirm = $("#ventanaConfirmCaptura").kendoWindow({
                        iframe: true,
                        title: _dictionary.EntregaPlacasGraficasTituloPopup[$("#language").data("kendoDropDownList").value()],
                        visible: false,
                        width: "auto",
                        height: "auto",
                        modal: true,
                        animation: {
                            close: false,
                            open: false
                        },
                        actions: [],
                    }).data("kendoWindow");

                    ventanaConfirm.content(_dictionary.EntregaPlacasGraficasMensajeDatosCapturadosNoGuardados[$("#language").data("kendoDropDownList").value()] +
                        "</br><center><button class='btn btn-blue' id='yesButtonProy'>" + _dictionary.EntregaPlacasGraficasbotonSi[$("#language").data("kendoDropDownList").value()] + "</button><button class='btn btn-blue' id='noButtonProy'>" + _dictionary.EntregaPlacasGraficasbotonNo[$("#language").data("kendoDropDownList").value()] + "</button></center>");

                    ventanaConfirm.open().center();
                    $("#yesButtonProy").click(function () {
                        AjaxPruebas();
                        $("#inputRequisicion").data("kendoComboBox").setDataSource();
                        $("#inputRequisicion").data("kendoComboBox").value("");
                        $("#inputRequisicion").data("kendoComboBox").dataSource.data([]);
                        $("#inputPrueba").data("kendoComboBox").setDataSource();
                        $("#inputPrueba").data("kendoComboBox").value("");
                        $("#inputPrueba").data("kendoComboBox").dataSource.data([]);
                        proyectoInicial = $("#inputProyecto").data("kendoComboBox").value();
                        ventanaConfirm.close();
                    });
                    $("#noButtonProy").click(function () {
                        $("#inputProyecto").data("kendoComboBox").value(proyectoInicial);
                        ventanaConfirm.close();
                    });
                }
            }
            else {
                $("#inputProyecto").data("kendoComboBox").value("");

            }
        }
    });

};


function SuscribirEventoComboRequisicion() {
    $('#inputRequisicion').kendoComboBox({
        suggest: true,
        delay: 10,
        filter: "contains",
        autoBind: false,
        dataTextField: "NombreRequisicion",
        dataValueField: "RequisicionID",
        change: function (e) {
            dataItem = this.dataItem(e.sender.selectedIndex);
            if (dataItem != undefined) {
                if (!editado) {

                    requisicionOriginal = $("#inputRequisicion").data("kendoComboBox").value();

                    AjaxCargarElementosRequisicion();
                }
                else {
                    var ventanaConfirm = $("#ventanaConfirmCaptura").kendoWindow({
                        iframe: true,
                        title: _dictionary.EntregaPlacasGraficasTituloPopup[$("#language").data("kendoDropDownList").value()],
                        visible: false,
                        width: "auto",
                        height: "auto",
                        modal: true,
                        animation: {
                            close: false,
                            open: false
                        },
                        actions: []
                    }).data("kendoWindow");

                    ventanaConfirm.content(_dictionary.EntregaPlacasGraficasMensajeDatosCapturadosNoGuardados[$("#language").data("kendoDropDownList").value()] +
                        "</br><center><button class='btn btn-blue' id='yesButtonProy'>" + _dictionary.EntregaPlacasGraficasbotonSi[$("#language").data("kendoDropDownList").value()] + "</button><button class='btn btn-blue' id='noButtonProy'>" + _dictionary.EntregaPlacasGraficasbotonNo[$("#language").data("kendoDropDownList").value()] + "</button></center>");

                    ventanaConfirm.open().center();
                    $("#yesButtonProy").click(function () {
                        requisicionOriginal = $("#inputRequisicion").data("kendoComboBox").value();
                        AjaxCargarElementosRequisicion();
                        ventanaConfirm.close();
                    });
                    $("#noButtonProy").click(function () {
                        $("#inputRequisicion").data("kendoComboBox").value(requisicionOriginal);
                        ventanaConfirm.close();
                    });
                }
            }
            else {
                $("#inputRequisicion").data("kendoComboBox").value("");

            }
        }
    });

};



function suscribirEventoChangeRadio() {
    $('input:radio[name=Muestra]:nth(0)').change(function () {
        AjaxCargarElementosRequisicion();
    });
    $('input:radio[name=Muestra]:nth(1)').change(function () {
        AjaxCargarElementosRequisicion();
    });
}
