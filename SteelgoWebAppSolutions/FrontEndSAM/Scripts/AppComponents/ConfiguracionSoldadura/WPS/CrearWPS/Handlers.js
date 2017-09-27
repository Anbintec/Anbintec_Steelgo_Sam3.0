function suscribirEventos() {
    suscribirEventoGuardar();
    suscribirEventoNombreWPS();
    suscribirEventoRaizNombre();
    suscribirEventoRellenoNombre();
    suscribirEventoChangeCVN();
}

function suscribirEventoGuardar() {
    $('.accionGuardar').click(function (e) {
        if ($('#Guardar').text() == _dictionary.DetalleAvisoLlegada0017[$("#language").data("kendoDropDownList").value()]) {
            AjaxExisteWPS(0);
        }
        else if ($('#Guardar').text() == _dictionary.DetalleAvisoLlegada0082[$("#language").data("kendoDropDownList").value()])
            opcionHabilitarView(false, "FieldSetView")
    });

    $('#btnGuardarYNuevo').click(function (e) {
        AjaxExisteWPS(1);
    });
    $('#btnGuardarYNuevo1').click(function (e) {
        AjaxExisteWPS(1);
    });

}

function suscribirEventoNombreWPS() {
    $("#NomnreWPS").keyup(function (e) {
        $('#NomnreWPS').val($('#NomnreWPS').val().toUpperCase());
    });
}

function suscribirEventoRaizNombre() {
    $("#PQRRaizNombre").kendoComboBox({
        dataTextField: "Nombre",
        dataValueField: "PQRID",
        suggest: true,
        delay: 10,
        filter: "contains",
        change: function (e) {
            dataItem = this.dataItem(e.sender.selectedIndex);
            if (dataItem != undefined && dataItem.Accion != 0) {
                if (dataItem.CodigoRaiz != "N/A") {
                    if (dataItem.PREHEAT) {
                        var data = kendo.observable({
                            optionCheck: true
                        });
                        kendo.bind($("#PREHEATRaiz"), data);
                    } else {
                        var data = kendo.observable({
                            optionCheck: false
                        });
                        kendo.bind($("#PREHEATRaiz"), data);
                    }
                    if (dataItem.PWHT) {
                        var data = kendo.observable({
                            optionCheck: true
                        });
                        kendo.bind($("#PWHRaiz"), data);
                    }
                    else {
                        var data = kendo.observable({
                            optionCheck: false
                        });
                        kendo.bind($("#PWHRaiz"), data);
                    }
                    if (dataItem.CVN) {
                        var data = kendo.observable({
                            optionCheck: true
                        });
                        kendo.bind($("#CVNRaiz"), data);
                    }
                    else {
                        var data = kendo.observable({
                            optionCheck: false
                        });
                        kendo.bind($("#CVNRaiz"), data);
                    }


                    $("#grupoPRaiz").val(dataItem.GrupoPMaterialBase1Nombre + " - " + dataItem.GrupoPMaterialBase2Nombre);
                    $("#RaizEspesorRaiz").text(parseFloat(dataItem.EspesorRaiz));
                    $("#RaizEspesorRelleno").text(parseFloat(dataItem.EspesorRelleno));

                    //var EspesoresRaiz = ObtenerEspesorCorrecto(parseFloat(dataItem.EspesorRaiz) + parseFloat(dataItem.EspesorRelleno), dataItem.PWHT, dataItem.CodigoRaiz, true);
                    var EspesoresRelleno, EspesorMaximoRaiz, EspesorMaximoRelleno;
                    if ($("#PQRRellenoNombre").data("kendoComboBox").dataItem($("#PQRRellenoNombre").data("kendoComboBox").select()) != undefined) {
                        EspesoresRelleno = ObtenerEspesorCorrecto(parseFloat($("#PQRRellenoNombre").data("kendoComboBox").dataItem($("#PQRRellenoNombre").data("kendoComboBox").select()).EspesorRelleno) +
                                                                  parseFloat(dataItem.EspesorRaiz),
                                                                  $('#PWHRelleno').is(':checked'), $('#CVNRelleno').is(':checked'), $("#PQRRellenoNombre").data("kendoComboBox").dataItem($("#PQRRellenoNombre").data("kendoComboBox").select()).CodigoRelleno,
                                                                  $('#QuitarCVN').is(':checked'));
                        $("#EspesorMaximoWPS").text(parseFloat(EspesoresRelleno[0].EspesorMaximo));
                        $("#EspesorMinimoWPS").text(parseFloat(EspesoresRelleno[0].EspesorMinimo));

                        EspesorMaximoRaiz = ObtenerEspesorCorrecto(parseFloat($("#PQRRaizNombre").data("kendoComboBox").dataItem($("#PQRRaizNombre").data("kendoComboBox").select()).EspesorRaiz),
                                                     $('#PWHRelleno').is(':checked'), $('#CVNRelleno').is(':checked'), $("#PQRRellenoNombre").data("kendoComboBox").dataItem($("#PQRRellenoNombre").data("kendoComboBox").select()).CodigoRelleno,
                                                     $('#QuitarCVN').is(':checked'));
                        $("#EspesorRaiz").text(parseFloat(EspesorMaximoRaiz[0].EspesorMaximo));


                        EspesorMaximoRelleno = ObtenerEspesorCorrecto(parseFloat($("#PQRRellenoNombre").data("kendoComboBox").dataItem($("#PQRRellenoNombre").data("kendoComboBox").select()).EspesorRelleno),
                                                      $('#PWHRelleno').is(':checked'), $('#CVNRelleno').is(':checked'), $("#PQRRellenoNombre").data("kendoComboBox").dataItem($("#PQRRellenoNombre").data("kendoComboBox").select()).CodigoRelleno,
                                                      $('#QuitarCVN').is(':checked'));
                        $("#EspesorRelleno").text(parseFloat(EspesorMaximoRelleno[0].EspesorMaximo));

                        if (dataItem.CVN == 1 && $("#PQRRellenoNombre").data("kendoComboBox").dataItem($("#PQRRellenoNombre").data("kendoComboBox").select()).CVN == 1) {
                            $('#QuitarCVN').prop("disabled", false);
                        }
                        else {
                            $('#QuitarCVN').prop("disabled", true);
                            var data = kendo.observable({
                                optionCheck: false
                            });
                            kendo.bind($("#QuitarCVN"), data);
                        }

                    }
                    

                }
                else {
                    displayNotify("WPSMensajeErrorPQRNoAplicaRaiz", "", "1");
                    $("#PQRRaizNombre").data("kendoComboBox").value("");
                    $("#grupoPRaiz").val("");
                    $("#RaizEspesorRaiz").text(0);
                    $("#RaizEspesorRelleno").text(0);
                    var data = kendo.observable({
                        optionCheck: false
                    });
                    kendo.bind($("#PREHEATRaiz"), data);
                    kendo.bind($("#PWHRaiz"), data);
                }
                if ($("#PQRRellenoNombre").data("kendoComboBox").value() == "") {
                    $("#EspesorMaximoWPS").text("0");
                    $("#EspesorMinimoWPS").text("0");
                }

            }
            else {
                $("#PQRRaizNombre").data("kendoComboBox").value("");
                $("#grupoPRaiz").val("");
                $("#RaizEspesorRaiz").text(0);
                $("#RaizEspesorRelleno").text(0);
                var data = kendo.observable({
                    optionCheck: false
                });
                kendo.bind($("#PREHEATRaiz"), data);
                kendo.bind($("#PWHRaiz"), data);

                if ($("#PQRRellenoNombre").text() == "") {
                    $("#EspesorMaximoWPS").text("0");
                    $("#EspesorMinimoWPS").text("0");
                }
            }
        }
    });

}

function suscribirEventoRellenoNombre() {
    $("#PQRRellenoNombre").kendoComboBox({
        dataTextField: "Nombre",
        dataValueField: "PQRID",
        suggest: true,
        delay: 10,
        filter: "contains",
        change: function (e) {
            dataItem = this.dataItem(e.sender.selectedIndex);

            if (dataItem != undefined && dataItem.Accion != 0) {
                if (dataItem.CodigoRelleno != "N/A") {
                    if (dataItem.PREHEAT) {
                        var data = kendo.observable({
                            optionCheck: true
                        });
                        kendo.bind($("#PREHEATRelleno"), data);
                    } else {
                        var data = kendo.observable({
                            optionCheck: false
                        });
                        kendo.bind($("#PREHEATRelleno"), data);
                    }
                    if (dataItem.PWHT) {
                        var data = kendo.observable({
                            optionCheck: true
                        });
                        kendo.bind($("#PWHRelleno"), data);
                    }
                    else {
                        var data = kendo.observable({
                            optionCheck: false
                        });
                        kendo.bind($("#PWHRelleno"), data);
                    }
                    if (dataItem.CVN) {
                        var data = kendo.observable({
                            optionCheck: true
                        });
                        kendo.bind($("#CVNRelleno"), data);
                    }
                    else {
                        var data = kendo.observable({
                            optionCheck: false
                        });
                        kendo.bind($("#CVNRelleno"), data);
                    }



                    $("#grupoPRelleno").val(dataItem.GrupoPMaterialBase1Nombre + " - " + dataItem.GrupoPMaterialBase2Nombre);
                    $("#RellenoEspesorRaiz").text(parseFloat(dataItem.EspesorRaiz));
                    $("#RellenoEspesorRelleno").text(parseFloat(dataItem.EspesorRelleno));

                    //var EspesoresRelleno = ObtenerEspesorCorrecto(parseFloat(dataItem.EspesorRaiz) + parseFloat(dataItem.EspesorRelleno), dataItem.PWHT, dataItem.CodigoRelleno, false);
                    var EspesoresRaiz, EspesorMaximoRaiz, EspesorMaximoRelleno;
                    if ($("#PQRRaizNombre").data("kendoComboBox").dataItem($("#PQRRaizNombre").data("kendoComboBox").select()) == undefined) {

                    }
                    else {
                        EspesoresRaiz = ObtenerEspesorCorrecto(parseFloat($("#PQRRaizNombre").data("kendoComboBox").dataItem($("#PQRRaizNombre").data("kendoComboBox").select()).EspesorRelleno) +
                                                                  parseFloat(dataItem.EspesorRelleno),
                                                                  $('#PWHRelleno').is(':checked'), $('#CVNRelleno').is(':checked'), $("#PQRRaizNombre").data("kendoComboBox").dataItem($("#PQRRaizNombre").data("kendoComboBox").select()).CodigoRelleno,
                                                                  $('#QuitarCVN').is(':checked'));
                        $("#EspesorMaximoWPS").text(parseFloat(EspesoresRaiz[0].EspesorMaximo));
                        $("#EspesorMinimoWPS").text(parseFloat(EspesoresRaiz[0].EspesorMinimo));




                        EspesorMaximoRaiz = ObtenerEspesorCorrecto(parseFloat($("#PQRRaizNombre").data("kendoComboBox").dataItem($("#PQRRaizNombre").data("kendoComboBox").select()).EspesorRaiz),
                                                      $('#PWHRelleno').is(':checked'), $('#CVNRelleno').is(':checked'), $("#PQRRellenoNombre").data("kendoComboBox").dataItem($("#PQRRellenoNombre").data("kendoComboBox").select()).CodigoRelleno,
                                                      $('#QuitarCVN').is(':checked'));
                        $("#EspesorRaiz").text(parseFloat(EspesorMaximoRaiz[0].EspesorMaximo));


                        EspesorMaximoRelleno = ObtenerEspesorCorrecto(parseFloat($("#PQRRellenoNombre").data("kendoComboBox").dataItem($("#PQRRellenoNombre").data("kendoComboBox").select()).EspesorRelleno),
                                                      $('#PWHRelleno').is(':checked'), $('#CVNRelleno').is(':checked'), $("#PQRRellenoNombre").data("kendoComboBox").dataItem($("#PQRRellenoNombre").data("kendoComboBox").select()).CodigoRelleno,
                                                      $('#QuitarCVN').is(':checked'));
                        $("#EspesorRelleno").text(parseFloat(EspesorMaximoRelleno[0].EspesorMaximo));

                        if (dataItem.CVN == 1 && $("#PQRRaizNombre").data("kendoComboBox").dataItem($("#PQRRaizNombre").data("kendoComboBox").select()).CVN == 1) {
                            $('#QuitarCVN').prop("disabled", false);
                        }
                        else {
                            $('#QuitarCVN').prop("disabled", true);
                            var data = kendo.observable({
                                optionCheck: false
                            });
                            kendo.bind($("#QuitarCVN"), data);
                        }


                    }
                }
                else {
                    displayNotify("WPSMensajeErrorPQRNoAplicaRelleno", "", "1");
                    $("#PQRRellenoNombre").data("kendoComboBox").value("");
                    $("#grupoPRelleno").val("");
                    $("#RellenoEspesorRaiz").text(0);
                    $("#RellenoEspesorRelleno").text(0);
                    var data = kendo.observable({
                        optionCheck: false
                    });
                    kendo.bind($("#PREHEATRelleno"), data);
                    kendo.bind($("#PWHRelleno"), data);
                    if ($("#PQRRaizNombre").data("kendoComboBox").value() == "") {
                        $("#EspesorMaximoWPS").text("0");
                        $("#EspesorMinimoWPS").text("0");
                    }
                }
            }
            else {
                $("#PQRRellenoNombre").data("kendoComboBox").value("");
                $("#grupoPRelleno").val("");
                $("#RellenoEspesorRaiz").text(0);
                $("#RellenoEspesorRelleno").text(0);
                var data = kendo.observable({
                    optionCheck: false
                });
                kendo.bind($("#PREHEATRelleno"), data);
                kendo.bind($("#PWHRelleno"), data);

                if ($("#PQRRaizNombre").text() == "") {
                    $("#EspesorMaximoWPS").text("0");
                    $("#EspesorMinimoWPS").text("0");
                }
            }

        },
    });
};

function suscribirEventoChangeCVN() {
    $('#QuitarCVN').change(function (e) {
        var Espesores, EspesorMaximoRaiz, EspesorMaximoRelleno;
        if ($("#PQRRellenoNombre").data("kendoComboBox").dataItem($("#PQRRellenoNombre").data("kendoComboBox").select()) != undefined && $("#PQRRaizNombre").data("kendoComboBox").dataItem($("#PQRRaizNombre").data("kendoComboBox").select()) != undefined) {
            Espesores = ObtenerEspesorCorrecto(parseFloat($("#PQRRellenoNombre").data("kendoComboBox").dataItem($("#PQRRellenoNombre").data("kendoComboBox").select()).EspesorRelleno) +
                                                      parseFloat($("#PQRRaizNombre").data("kendoComboBox").dataItem($("#PQRRaizNombre").data("kendoComboBox").select()).EspesorRaiz),
                                                      $('#PWHRelleno').is(':checked'), $('#CVNRelleno').is(':checked'), $("#PQRRellenoNombre").data("kendoComboBox").dataItem($("#PQRRellenoNombre").data("kendoComboBox").select()).CodigoRelleno,
                                                      $('#QuitarCVN').is(':checked'));
            $("#EspesorMaximoWPS").text(parseFloat(Espesores[0].EspesorMaximo));
            $("#EspesorMinimoWPS").text(parseFloat(Espesores[0].EspesorMinimo));

            EspesorMaximoRaiz = ObtenerEspesorCorrecto(parseFloat($("#PQRRaizNombre").data("kendoComboBox").dataItem($("#PQRRaizNombre").data("kendoComboBox").select()).EspesorRaiz),
                                                      $('#PWHRelleno').is(':checked'), $('#CVNRelleno').is(':checked'), $("#PQRRellenoNombre").data("kendoComboBox").dataItem($("#PQRRellenoNombre").data("kendoComboBox").select()).CodigoRelleno,
                                                      $('#QuitarCVN').is(':checked'));
            $("#EspesorRaiz").text(parseFloat(EspesorMaximoRaiz[0].EspesorMaximo));


            EspesorMaximoRelleno = ObtenerEspesorCorrecto(parseFloat($("#PQRRellenoNombre").data("kendoComboBox").dataItem($("#PQRRellenoNombre").data("kendoComboBox").select()).EspesorRelleno),
                                                      $('#PWHRelleno').is(':checked'), $('#CVNRelleno').is(':checked'), $("#PQRRellenoNombre").data("kendoComboBox").dataItem($("#PQRRellenoNombre").data("kendoComboBox").select()).CodigoRelleno,
                                                      $('#QuitarCVN').is(':checked'));
            $("#EspesorRelleno").text(parseFloat(EspesorMaximoRelleno[0].EspesorMaximo));
            

        }
    });
}