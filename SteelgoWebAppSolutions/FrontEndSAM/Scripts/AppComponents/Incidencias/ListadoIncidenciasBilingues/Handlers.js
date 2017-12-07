function SuscribirEventos() {
    SuscribirEventoProyecto();
    suscrubirEventoEtapa();
    SuscribirEventoPeriodo();
    SuscribirEventoFecha();
    SuscribirEventoMostrar();
    suscribirEventoAplicar();
    suscribirEventoGuardar();
}




function suscribirEventoAplicar() {
    $('#TraduceES').click(function (e) {

        if ($('input:radio[name=LLena]:checked').val() === "Todos") {

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
                },
                actions: []
            }).data("kendoWindow");

            ventanaConfirm.content(_dictionary.CapturaMensajeArmadoPlancharTodos[$("#language").data("kendoDropDownList").value()] +
                         "</br><center><button class='confirm_yes btn btn-blue' id='yesButton'>Si</button><button class='confirm_yes btn btn-blue' id='noButton'> No</button></center>");

            ventanaConfirm.open().center();

            $("#yesButton").click(function (handler) {

                obtenerRegistrosTraducir();
                ventanaConfirm.close();
            });
            $("#noButton").click(function (handler) {
                ventanaConfirm.close();
            });
        }
        else {
            obtenerRegistrosTraducir();

        }
    });
};

function SuscribirEventoMostrar() {
    $("#btnBuscar").click(function (e) {
        AjaxCargarListado();

        var dataItem = $("#EtapaID").data("kendoComboBox").value();

        if (dataItem == "Abierta") {
            $("#grid").data("kendoGrid").showColumn("Titulo");
            $("#grid").data("kendoGrid").showColumn("TituloIngles");
            $("#grid").data("kendoGrid").showColumn("Descripcion");
            $("#grid").data("kendoGrid").showColumn("DescripcionIngles");


            $("#grid").data("kendoGrid").hideColumn("Respuesta");
            $("#grid").data("kendoGrid").hideColumn("RespuestaIngles");
            $("#grid").data("kendoGrid").hideColumn("DetalleResolucion");
            $("#grid").data("kendoGrid").hideColumn("DetalleResolucionIngles");
            $("#grid").data("kendoGrid").hideColumn("MotivoCancelacion");
            $("#grid").data("kendoGrid").hideColumn("MotivoCancelacionIngles");

        }
        else if (dataItem == "Resuelto") {
            $("#grid").data("kendoGrid").hideColumn("Titulo");
            $("#grid").data("kendoGrid").hideColumn("TituloIngles");
            $("#grid").data("kendoGrid").hideColumn("Descripcion");
            $("#grid").data("kendoGrid").hideColumn("DescripcionIngles");


            $("#grid").data("kendoGrid").hideColumn("Respuesta");
            $("#grid").data("kendoGrid").hideColumn("RespuestaIngles");
            $("#grid").data("kendoGrid").showColumn("DetalleResolucion");
            $("#grid").data("kendoGrid").showColumn("DetalleResolucionIngles");
            $("#grid").data("kendoGrid").hideColumn("MotivoCancelacion");
            $("#grid").data("kendoGrid").hideColumn("MotivoCancelacionIngles");

        }
        else if (dataItem == "Respondido") {
            $("#grid").data("kendoGrid").hideColumn("Titulo");
            $("#grid").data("kendoGrid").hideColumn("TituloIngles");
            $("#grid").data("kendoGrid").hideColumn("Descripcion");
            $("#grid").data("kendoGrid").hideColumn("DescripcionIngles");


            $("#grid").data("kendoGrid").showColumn("Respuesta");
            $("#grid").data("kendoGrid").showColumn("RespuestaIngles");
            $("#grid").data("kendoGrid").hideColumn("DetalleResolucion");
            $("#grid").data("kendoGrid").hideColumn("DetalleResolucionIngles");
            $("#grid").data("kendoGrid").hideColumn("MotivoCancelacion");
            $("#grid").data("kendoGrid").hideColumn("MotivoCancelacionIngles");

        }
        else if (dataItem == "Cancelado") {
            $("#grid").data("kendoGrid").hideColumn("Titulo");
            $("#grid").data("kendoGrid").hideColumn("TituloIngles");
            $("#grid").data("kendoGrid").hideColumn("Descripcion");
            $("#grid").data("kendoGrid").hideColumn("DescripcionIngles");


            $("#grid").data("kendoGrid").hideColumn("Respuesta");
            $("#grid").data("kendoGrid").hideColumn("RespuestaIngles");
            $("#grid").data("kendoGrid").hideColumn("DetalleResolucion");
            $("#grid").data("kendoGrid").hideColumn("DetalleResolucionIngles");
            $("#grid").data("kendoGrid").showColumn("MotivoCancelacion");
            $("#grid").data("kendoGrid").showColumn("MotivoCancelacionIngles");

        }
        else {
            $("#grid").data("kendoGrid").hideColumn("Titulo");
            $("#grid").data("kendoGrid").hideColumn("TituloIngles");
            $("#grid").data("kendoGrid").hideColumn("Descripcion");
            $("#grid").data("kendoGrid").hideColumn("DescripcionIngles");


            $("#grid").data("kendoGrid").hideColumn("Respuesta");
            $("#grid").data("kendoGrid").hideColumn("RespuestaIngles");
            $("#grid").data("kendoGrid").hideColumn("DetalleResolucion");
            $("#grid").data("kendoGrid").hideColumn("DetalleResolucionIngles");
            $("#grid").data("kendoGrid").hideColumn("MotivoCancelacion");
            $("#grid").data("kendoGrid").hideColumn("MotivoCancelacionIngles");

        }

    });

}

function SuscribirEventoProyecto() {
    $('#ProyectoID').kendoComboBox({
        dataTextField: "Nombre",
        dataValueField: "ProyectoID",
        suggest: true,
        filter: "contains",
        index: 3,
        change: function (e) {
            var dataItem = this.dataItem(e.sender.selectedIndex);

            if (dataItem != undefined) {

            }
            else {
                $("#ProyectoID").data("kendoComboBox").value("");
            }
        }
    });

}
function suscrubirEventoEtapa() {
    $("#EtapaID").kendoComboBox({
        dataTextField: "Nombre",
        dataValueField: "EtapaID",
        dataSource:
            [
            { Nombre: "", EtapaID: "0" },
            { Nombre: "Incidencia", EtapaID: "Abierta" },
            { Nombre: "Resolver", EtapaID: "Resuelto" },
            { Nombre: "Responder", EtapaID: "Respondido" },
            { Nombre: "Cancelar", EtapaID: "Cancelado" }
            ],
        change: function (e) {
            var dataItem = this.dataItem(e.sender.selectedIndex);
            if (dataItem == undefined) {
                $("#EtapaID").data("kendoComboBox").value("");
            }
        },
        filter: "contains",
        suggest: true,
    });
}
function SuscribirEventoPeriodo() {
    $("#PeriodoTiempo").kendoComboBox({
        dataTextField: "Periodo",
        dataValueField: "PeriodoID",
        suggest: true,
        filter: "contains",
        index: 3,
        change: function (e) {
            var dataItem = this.dataItem(e.sender.selectedIndex);
            $("#FechaInicial").data("kendoDatePicker").value("");
            $("#FechaFinal").data("kendoDatePicker").value("");
            if (dataItem != undefined) {
                if (dataItem.PeriodoID != 0) {
                    AjaxCargarRangoFechas(dataItem);
                }
            }
            else {
                $("#PeriodoTiempo").data("kendoComboBox").value("");
            }
        }
    });
}

function SuscribirEventoFecha() {
    FechaInicio = $("#FechaInicial").kendoDatePicker({
        max: new Date(),
        change: function (e) {
            if (!ValidarFecha(e.sender._value)) {
                $("#FechaInicial").data("kendoDatePicker").value("");
            }
        }
    });

    FechaInicio.on("keydown", function (e) {
        if (e.keyCode == 13) {
            if (!ValidarFecha($("#FechaInicial").data("kendoDatePicker").value())) {
                $("#FechaInicial").data("kendoDatePicker").value("");
            }
        }

        if (e.keyCode == 9) {
            if (!ValidarFecha($("#FechaInicial").data("kendoDatePicker").value())) {
                $("#FechaInicial").data("kendoDatePicker").value("");
            }
        }
    });

    $("#FechaInicial").blur(function (e) {
        if (!ValidarFecha($("#FechaInicial").data("kendoDatePicker").value())) {
            $("#FechaInicial").data("kendoDatePicker").value("");
        }
    });

    FechaFin = $("#FechaFinal").kendoDatePicker({
        max: new Date(),
        change: function (e) {
            if (!ValidarFecha(e.sender._value)) {
                $("#FechaFinal").data("kendoDatePicker").value("");
            }
        }
    });

    FechaFin.on("keydown", function (e) {
        if (e.keyCode == 13) {
            if (!ValidarFecha($("#FechaFinal").data("kendoDatePicker").value())) {
                $("#FechaFinal").data("kendoDatePicker").value("");
            }
        }

        if (e.keyCode == 9) {
            if (!ValidarFecha($("#FechaFinal").data("kendoDatePicker").value())) {
                $("#FechaFinal").data("kendoDatePicker").value("");
            }
        }
    });

    $("#FechaFinal").blur(function (e) {
        if (!ValidarFecha($("#FechaFinal").data("kendoDatePicker").value())) {
            $("#FechaFinal").data("kendoDatePicker").value("");
        }
    });
}


function obtenerRegistrosTraducir() {
    var dataSource = $("#grid").data("kendoGrid").dataSource;
    var filters = dataSource.filter();
    var allData = dataSource.data();
    var query = new kendo.data.Query(allData);
    var data = query.filter(filters).data;

    for (var i = 0; i < data.length; i++) {

        if ($('input:radio[name=LLena]:checked').val() === "Todos") {


            var dataItem = $("#EtapaID").data("kendoComboBox").value();

            if (dataItem == "Abierta") {
                if (data[i].Titulo != "" || data[i].Descripcion != "") {

                    var _urlTraslateAPI = "https://translation.googleapis.com/language/translate/v2?key=AIzaSyDagWUPN1-bZlZqW9jfTshrvDTuUbOFYik";
                    _urlTraslateAPI += "&source=" + "es";
                    _urlTraslateAPI += "&target=" + "en";
                    _urlTraslateAPI += "&q=" + escape(data[i].FolioOriginalID + "*" + data[i].Titulo);

                    $.post(_urlTraslateAPI, function (dataT, status) {
                        var array = dataT.data.translations[0].translatedText.split("*");
                        var folioID = array[0]
                        for (var j = 0; j < data.length; j++) {
                            if (data[j].FolioOriginalID == folioID.trim()) {
                                data[j].TituloIngles = array[1];
                                data[j].Modificado = 1;
                                $("#grid").data("kendoGrid").dataSource.sync();
                            }
                        }
                    });


                    _urlTraslateAPI = "";
                    _urlTraslateAPI = "https://translation.googleapis.com/language/translate/v2?key=AIzaSyDagWUPN1-bZlZqW9jfTshrvDTuUbOFYik";
                    _urlTraslateAPI += "&source=" + "es";
                    _urlTraslateAPI += "&target=" + "en";
                    _urlTraslateAPI += "&q=" + escape(data[i].FolioOriginalID + "*" + data[i].Descripcion);

                    $.post(_urlTraslateAPI, function (dataT, status) {
                        var array = dataT.data.translations[0].translatedText.split("*");
                        var folioID = array[0]
                        for (var j = 0; j < data.length; j++) {
                            if (data[j].FolioOriginalID == folioID.trim()) {
                                data[j].DescripcionIngles = array[1];
                                data[j].Modificado = 1;
                                $("#grid").data("kendoGrid").dataSource.sync();
                            }
                        }
                    });

                }

            }
            else if (dataItem == "Resuelto") {
                if (data[i].DetalleResolucion != "") {
                    var _urlTraslateAPI = "https://translation.googleapis.com/language/translate/v2?key=AIzaSyDagWUPN1-bZlZqW9jfTshrvDTuUbOFYik";
                    _urlTraslateAPI += "&source=" + "es";
                    _urlTraslateAPI += "&target=" + "en";
                    _urlTraslateAPI += "&q=" + escape(data[i].FolioOriginalID + "*" + data[i].DetalleResolucion);

                    $.post(_urlTraslateAPI, function (dataT, status) {
                        var array = dataT.data.translations[0].translatedText.split("*");
                        var folioID = array[0]
                        for (var j = 0; j < data.length; j++) {
                            if (data[j].FolioOriginalID == folioID.trim()) {
                                data[j].DetalleResolucionIngles = array[1];
                                data[j].Modificado = 1;
                                $("#grid").data("kendoGrid").dataSource.sync();
                            }
                        }
                    });
                }

            }
            else if (dataItem == "Respondido") {
                if (data[i].Respuesta != "") {
                    var _urlTraslateAPI = "https://translation.googleapis.com/language/translate/v2?key=AIzaSyDagWUPN1-bZlZqW9jfTshrvDTuUbOFYik";
                    _urlTraslateAPI += "&source=" + "es";
                    _urlTraslateAPI += "&target=" + "en";
                    _urlTraslateAPI += "&q=" + escape(data[i].FolioOriginalID + "*" + data[i].Respuesta);

                    $.post(_urlTraslateAPI, function (dataT, status) {
                        var array = dataT.data.translations[0].translatedText.split("*");
                        var folioID = array[0]
                        for (var j = 0; j < data.length; j++) {
                            if (data[j].FolioOriginalID == folioID.trim()) {
                                data[j].RespuestaIngles = array[1];
                                data[j].Modificado = 1;
                                $("#grid").data("kendoGrid").dataSource.sync();
                            }
                        }
                    });
                }

            }
            else if (dataItem == "Cancelado") {
                if (data[i].MotivoCancelacion != "") {
                    var _urlTraslateAPI = "https://translation.googleapis.com/language/translate/v2?key=AIzaSyDagWUPN1-bZlZqW9jfTshrvDTuUbOFYik";
                    _urlTraslateAPI += "&source=" + "es";
                    _urlTraslateAPI += "&target=" + "en";
                    _urlTraslateAPI += "&q=" + escape(data[i].FolioOriginalID + "*" + data[i].MotivoCancelacion);


                    $.post(_urlTraslateAPI, function (dataT, status) {
                        var array = dataT.data.translations[0].translatedText.split("*");
                        var folioID = array[0]
                        for (var j = 0; j < data.length; j++) {
                            if (data[j].FolioOriginalID == folioID.trim()) {
                                data[j].MotivoCancelacionIngles = array[1];
                                data[j].Modificado = 1;
                                $("#grid").data("kendoGrid").dataSource.sync();
                            }
                        }
                    });
                }

            }

        }
        else {


            var dataItem = $("#EtapaID").data("kendoComboBox").value();

            if (dataItem == "Abierta") {
                if ((data[i].Titulo != "" && data[i].TituloIngles == null) || (data[i].Descripcion != "" && data[i].DescripcionIngles == null)) {
                    var _urlTraslateAPI = "https://translation.googleapis.com/language/translate/v2?key=AIzaSyDagWUPN1-bZlZqW9jfTshrvDTuUbOFYik";
                    _urlTraslateAPI += "&source=" + "es";
                    _urlTraslateAPI += "&target=" + "en";
                    _urlTraslateAPI += "&q=" + escape(data[i].FolioOriginalID + "*" + data[i].Titulo);

                    $.post(_urlTraslateAPI, function (dataT, status) {
                        var array = dataT.data.translations[0].translatedText.split("*");
                        var folioID = array[0]
                        for (var j = 0; j < data.length; j++) {
                            if (data[j].FolioOriginalID == folioID.trim()) {
                                data[j].TituloIngles = array[1];
                                data[j].Modificado = 1;
                                $("#grid").data("kendoGrid").dataSource.sync();
                            }
                        }
                    });


                    _urlTraslateAPI = "";
                    _urlTraslateAPI = "https://translation.googleapis.com/language/translate/v2?key=AIzaSyDagWUPN1-bZlZqW9jfTshrvDTuUbOFYik";
                    _urlTraslateAPI += "&source=" + "es";
                    _urlTraslateAPI += "&target=" + "en";
                    _urlTraslateAPI += "&q=" + escape(data[i].FolioOriginalID + "*" + data[i].Descripcion);

                    $.post(_urlTraslateAPI, function (dataT, status) {
                        var array = dataT.data.translations[0].translatedText.split("*");
                        var folioID = array[0]
                        for (var j = 0; j < data.length; j++) {
                            if (data[j].FolioOriginalID == folioID.trim()) {
                                data[j].DescripcionIngles = array[1];
                                data[j].Modificado = 1;
                                $("#grid").data("kendoGrid").dataSource.sync();
                            }
                        }
                    });

                }

            }
            else if (dataItem == "Resuelto") {
                if (data[i].DetalleResolucion != "" && data[i].DetalleResolucionIngles == null) {
                    var _urlTraslateAPI = "https://translation.googleapis.com/language/translate/v2?key=AIzaSyDagWUPN1-bZlZqW9jfTshrvDTuUbOFYik";
                    _urlTraslateAPI += "&source=" + "es";
                    _urlTraslateAPI += "&target=" + "en";
                    _urlTraslateAPI += "&q=" + escape(data[i].FolioOriginalID + "*" + data[i].DetalleResolucion);

                    $.post(_urlTraslateAPI, function (dataT, status) {
                        var array = dataT.data.translations[0].translatedText.split("*");
                        var folioID = array[0]
                        for (var j = 0; j < data.length; j++) {
                            if (data[j].FolioOriginalID == folioID.trim()) {
                                data[j].DetalleResolucionIngles = array[1];
                                data[j].Modificado = 1;
                                $("#grid").data("kendoGrid").dataSource.sync();
                            }
                        }
                    });
                }

            }
            else if (dataItem == "Respondido") {
                if (data[i].Respuesta != "" && data[i].RespuestaIngles == null) {
                    var _urlTraslateAPI = "https://translation.googleapis.com/language/translate/v2?key=AIzaSyDagWUPN1-bZlZqW9jfTshrvDTuUbOFYik";
                    _urlTraslateAPI += "&source=" + "es";
                    _urlTraslateAPI += "&target=" + "en";
                    _urlTraslateAPI += "&q=" + escape(data[i].FolioOriginalID + "*" + data[i].Respuesta);

                    $.post(_urlTraslateAPI, function (dataT, status) {
                        var array = dataT.data.translations[0].translatedText.split("*");
                        var folioID = array[0]
                        for (var j = 0; j < data.length; j++) {
                            if (data[j].FolioOriginalID == folioID.trim()) {
                                data[j].RespuestaIngles = array[1];
                                data[j].Modificado = 1;
                                $("#grid").data("kendoGrid").dataSource.sync();
                            }
                        }
                    });
                }

            }
            else if (dataItem == "Cancelado") {
                if (data[i].MotivoCancelacion != "" && data[i].MotivoCancelacionIngles == null) {
                    var _urlTraslateAPI = "https://translation.googleapis.com/language/translate/v2?key=AIzaSyDagWUPN1-bZlZqW9jfTshrvDTuUbOFYik";
                    _urlTraslateAPI += "&source=" + "es";
                    _urlTraslateAPI += "&target=" + "en";
                    _urlTraslateAPI += "&q=" + escape(data[i].FolioOriginalID + "*" + data[i].MotivoCancelacion);


                    $.post(_urlTraslateAPI, function (dataT, status) {
                        var array = dataT.data.translations[0].translatedText.split("*");
                        var folioID = array[0]
                        for (var j = 0; j < data.length; j++) {
                            if (data[j].FolioOriginalID == folioID.trim()) {
                                data[j].MotivoCancelacionIngles = array[1];
                                data[j].Modificado = 1;
                                $("#grid").data("kendoGrid").dataSource.sync();
                            }
                        }
                    });
                }

            }


        }

    }

}


function suscribirEventoGuardar() {
    $('.accionGuardar').click(function (e) {
        e.preventDefault();
        var ds = $("#grid").data("kendoGrid").dataSource;
        
            if ($('#botonGuardar').text() == "Guardar" || $('#botonGuardar').text() == "Save") {
                if (ds._data.length > 0) {
                    AjaxGuardarCaptura(ds._data, 0);
                }
                else
                    displayNotify("MensajeAdverteciaExcepcionGuardado", "", '1');
            }
            else if ($('#botonGuardar').text() == "Editar" || $('#botonGuardar').text() == "Edit")
                opcionHabilitarView(false, "FieldSetView")
        
    });

}

