
function AjaxCargarCamposPredeterminados() {

    loadingStart();
    $Soldadura.Soldadura.read({ token: Cookies.get("token"), lenguaje: $("#language").val() }).done(function (data) {
        if (Error(data)) {
            var NewDate = kendo.toString(data.FechaSoldadura, _dictionary.FormatoFecha[$("#language").data("kendoDropDownList").value()]);

            endRangeDate.val(NewDate);


            if (data.TipoCaptura == "Reporte") {
                $('input:radio[name=TipoAgregado]:nth(0)').trigger("click");
                //$('input:radio[name=TipoAgregado]:nth(1)').removeAttr('checked');
                $("#styleReporte").addClass("active");
                $("#styleListado").removeClass("active");
                $('input:radio[name=TipoAgregado]:nth(0)').attr('checked', true).trigger("change");
            }
            else if (data.TipoCaptura == "Lista") {
                //$('input:radio[name=TipoAgregado]:nth(0)').removeAttr('checked');
                $('input:radio[name=TipoAgregado]:nth(1)').trigger("click");
                $("#styleListado").addClass("active");
                $("#styleReporte").removeClass("active");
                $('input:radio[name=TipoAgregado]:nth(1)').attr('checked', true).trigger("change");
            }
            //eventoCambioTipoListado();
        }
        loadingStop();
    });

};

function AjaxObtenerListaTaller() {
    //loadingStart();
    if (Cookies.get("Proyecto") != undefined) {
        $Soldadura.Soldadura.read({ idProyecto: Cookies.get("Proyecto").split('°')[0], token: Cookies.get("token") }).done(function (data) {
            if (Error(data)) {
                $("#inputTaller").data("kendoComboBox").value("");
                $("#inputTaller").data("kendoComboBox").dataSource.data(data);
            }
            //loadingStop();
        });
    }
}


function AjaxCargarCamposPredeterminadosCambiaTipoVista() {

    loadingStart();
    $Soldadura.Soldadura.read({ token: Cookies.get("token"), lenguaje: $("#language").val() }).done(function (data) {
        if (Error(data)) {
            var NewDate = kendo.toString(data.FechaSoldadura, _dictionary.FormatoFecha[$("#language").data("kendoDropDownList").value()]);

            endRangeDate.val(NewDate);

            if (data.Muestra == "Sincaptura") {
                $('input:radio[name=Muestra]:nth(0)').trigger("click");
                //$('input:radio[name=Muestra]:nth(1)').removeAttr('checked');

            }
            else if (data.Muestra == "Todos") {
                //$('input:radio[name=Muestra]:nth(0)').removeAttr('checked');
                $('input:radio[name=Muestra]:nth(1)').trigger("click");

            }

            if (data.Llena == "Todos") {
                $('input:radio[name=LLena]:nth(0)').trigger("click");
                //$('input:radio[name=LLena]:nth(1)').removeAttr('checked');

            }
            else if (data.Llena == "Vacios") {
                //$('input:radio[name=LLena]:nth(0)').removeAttr('checked');
                $('input:radio[name=LLena]:nth(1)').trigger("click");

            }

            //

        }
        loadingStop();
    });
};

function AjaxJunta(spoolID) {
    loadingStart();
    $CapturasRapidas.CapturasRapidas.read({ id: spoolID, sinCaptura: $('input:radio[name=Muestra]:checked').val(), token: Cookies.get("token"), proceso: 2 }).done(function (data) {
        if (Error(data)) {
            $("#Junta").data("kendoComboBox").value("");
            $("#Junta").data("kendoComboBox").dataSource.data(data)
        }
        loadingStop();
    });
};


function AjaxJuntaModoSpool(spoolID) {
    loadingStart();


    $CapturasRapidas.CapturasRapidas.read({ id: spoolID, sinCaptura: $('input:radio[name=Muestra]:checked').val(), token: Cookies.get("token"), proceso: 2 }).done(function (data) {
        if (Error(data)) {
            $("#Junta").data("kendoComboBox").value("");
            $("#Junta").data("kendoComboBox").dataSource.data(data);
        }
        
        AjaxCargarReporteJuntas();
    });
}


function AjaxCargarReporteJuntas() {

    var listadoReporte = ArregloListadoReporte();
    var elementoNoEncontrado = false;

    CapturaJuntas = [];
    CapturaJuntas[0] = { Detalles: "" };
    ListaDetalles = [];

    ListaDetalles = listadoReporte;
    CapturaJuntas[0].Detalles = ListaDetalles;


    if (listadoReporte.length > 0) {
        loadingStart();
        $Soldadura.Soldadura.read({ JsonCaptura: JSON.stringify(listadoReporte[0]), isReporte: true, lenguaje: $("#language").val(), token: Cookies.get("token") }).done(function (data) {
            if (Error(data)) {
                editado = true;
                var elementosModificados = "";
                var elementosNoModificados = "";
                var ds = $("#grid").data("kendoGrid").dataSource;
                var array = JSON.parse(data);

                for (var i = 0; i < array.length; i++) {
                    if (!ExisteJuntaEnSpool(array[i])) {
                        ds.insert(0, array[i]);

                        if (array[i].FechaSoldadura != null) {
                            array[i].FechaSoldadura = new Date(ObtenerDato(array[i].FechaSoldadura, 1), ObtenerDato(array[i].FechaSoldadura, 2), ObtenerDato(array[i].FechaSoldadura, 3));//año, mes, dia
                        }
                        elementoNoEncontrado = true;

                        if (elementosModificados != "")
                            elementosModificados += ", " + array[i].Junta;
                        else
                            elementosModificados = array[i].Junta;
                    }
                    else {
                        for (var j = 0; j < ds._data.length; j++) {
                            if (array[i].SpoolID == ds._data[j].SpoolID && array[i].JuntaID == ds._data[j].JuntaID && ds._data[j].Accion == 3) {
                                ds._data[j].DetalleJunta = array[i].DetalleJunta;
                                ds._data[j].TemplateSoldadoresRaiz = array[i].TemplateSoldadoresRaiz;
                                ds._data[j].TemplateSoldadoresRelleno = array[i].TemplateSoldadoresRelleno;
                                ds._data[j].TipoJuntaID = array[i].TipoJuntaID;
                                ds._data[j].Accion = array[i].Accion;
                                ds._data[j].ProcesoSoldaduraRaizID = array[i].ProcesoSoldaduraRaizID;
                                ds._data[j].ProcesoSoldaduraRellenoID = array[i].ProcesoSoldaduraRellenoID;
                                ds._data[j].ProcesoSoldaduraRaiz = array[i].ProcesoSoldaduraRaiz;
                                ds._data[j].ProcesoSoldaduraRelleno = array[i].ProcesoSoldaduraRelleno;
                                ds._data[j].Proyecto = array[i].Proyecto;
                                ds._data[j].IDProyecto = array[i].IDProyecto;
                                ds._data[j].IdOrdenTrabajo = array[i].IdOrdenTrabajo;
                                ds._data[j].OrdenTrabajo = array[i].OrdenTrabajo;
                                ds._data[j].idVal = array[i].idVal;
                                ds._data[j].idText = array[i].idText;
                                ds._data[j].SpoolID = array[i].SpoolID;
                                ds._data[j].JuntaID = array[i].JuntaID;
                                ds._data[j].Junta = array[i].Junta;
                                ds._data[j].TipoJunta = array[i].TipoJunta;
                                ds._data[j].Cedula = array[i].Cedula;
                                ds._data[j].Diametro = array[i].Diametro;
                                ds._data[j].TallerID = array[i].TallerID;
                                ds._data[j].Taller = array[i].Taller;
                                ds._data[j].TemplateTrabajosAdicionales = array[i].TemplateTrabajosAdicionales;
                                ds._data[j].FechaSoldadura = array[i].FechaSoldadura;
                                ds._data[j].listaTrabajosAdicionalesSoldadura = array[i].listaTrabajosAdicionalesSoldadura;
                                ds._data[j].ListaTaller = array[i].ListaTaller;
                                ds._data[j].ListaWPS = array[i].ListaWPS;
                                ds._data[j].ListaCedulas = array[i].ListaCedulas;

                                if (array[i].FechaSoldadura != null) {
                                    ds._data[j].FechaSoldadura = new Date(ObtenerDato(array[i].FechaSoldadura, 1), ObtenerDato(array[i].FechaSoldadura, 2), ObtenerDato(array[i].FechaSoldadura, 3));//año, mes, dia
                                }

                                if (elementosModificados != "")
                                    elementosModificados += ", " + array[i].Junta;
                                else
                                    elementosModificados = array[i].Junta;

                                var elementgrid = ds._data.splice(j, 1);
                                ds._data.unshift(elementgrid[0]);

                                elementoNoEncontrado = true;
                            }
                            else if (array[i].SpoolID == ds._data[j].SpoolID && array[i].JuntaID == ds._data[j].JuntaID) {
                                //Elementos no agregados
                                if (elementosNoModificados != "")
                                    elementosNoModificados += ", " + array[i].Junta;
                                else
                                    elementosNoModificados = array[i].Junta;
                            }
                        }

                    }
                }
                $("#grid").data("kendoGrid").dataSource.sync();

                loadingStop();
                $("#InputID").data("kendoComboBox").value("");

                if (elementosModificados != "") {
                    displayNotify("", _dictionary.CapturaSoldaduraMsgExiste[$("#language").data("kendoDropDownList").value()] +
                        elementosModificados + _dictionary.CapturaArmadoMsgNuevoEnReporte[$("#language").data("kendoDropDownList").value()], '0');
                }
                if (elementosNoModificados != "") {
                    displayNotify("", _dictionary.CapturaSoldaduraMsgExiste[$("#language").data("kendoDropDownList").value()] +
                        elementosNoModificados + _dictionary.CapturaArmadoMsgExisteReporte[$("#language").data("kendoDropDownList").value()], '1');
                }
            }
        });
    } else {
        if ($('input:radio[name=Muestra]:checked').val() == "Sin Capturar") {
            displayNotify("CapturaSoldaduraNoExistenJuntasSpool", "", "1");
        } else {
            displayNotify("CapturaSoldaduraNoTieneJuntas", "", "1");
        }
    }
    $('#ButtonAgregar').prop("disabled", false);
}



function AjaxObtenerListadoWPS(dataItem) {

    loadingStart();
    var pwht = dataItem.RequierePwht ? 1 : 0;
    $Soldadura.Soldadura.read({ ProyectoID: dataItem.IDProyecto, ProcesoRaizID: dataItem.ProcesoSoldaduraRaizID, ProcesoRellenoID: dataItem.ProcesoSoldaduraRellenoID, Espesor: dataItem.Espesor, RequierePwht: pwht, lenguaje: $("#language").val(), token: Cookies.get("token") }).done(function (data) {
        if (Error(data)) {
            dataItem.ListaWPS = data;
        }
        loadingStop();
    });
}


function AjaxObtenerListadoSoldadores(dataItem, TipoProceso) {

    if (TipoProceso == 0)
        procesoSoldadura = dataItem.ProcesoSoldaduraRellenoID;
    else
        procesoSoldadura = dataItem.ProcesoSoldaduraRaizID;
    loadingStart();
    $Soldadura.Soldadura.read({ TipoProceso: TipoProceso, ProcesoSoldadura: procesoSoldadura, Espesor: dataItem.Espesor, Diametro: dataItem.Diametro, lenguaje: $("#language").val(), token: Cookies.get("token") }).done(function (data) {
        if (Error(data)) {
            if (TipoProceso == 0) {
                listadoSoldadoresParaRelleno = data;
                GridPopupSoldadoresRellenoCapturados(dataItem);

            }
            else {
                listadoSoldadoresParaRaiz = data;
                GridPopupSoldadoresRaizCapturados(dataItem);
            }
        }
        loadingStop();
    });
}

function AjaxObtenerListadoColadas(dataItem, TipoProceso) {

    if (TipoProceso == 0)
        procesoSoldadura = dataItem.ProcesoSoldaduraRellenoID;
    else
        procesoSoldadura = dataItem.ProcesoSoldaduraRaizID;
    loadingStart();
    $Soldadura.Soldadura.read({ ProcesoSoldadura: procesoSoldadura, esRaiz: TipoProceso, WPSID: dataItem.WPSID, FamiliaMaterialID: dataItem.FamiliaMaterialID, lenguaje: $("#language").val(), token: Cookies.get("token") }).done(function (data) {
        if (Error(data)) {
            listadoColada = data;
            AjaxObtenerListadoSoldadores(dataItem, TipoProceso);
        }
        loadingStop();
    });
}



function ObtenerJSonGridSoldadura() {
    try {

        loadingStart();
        $Soldadura.Soldadura.read({ JsonCaptura: JSON.stringify(ArregloListadoCaptura()), isReporte: false, lenguaje: $("#language").val(), token: Cookies.get("token") }).done(function (data) {
            if (Error(data)) {
                editado = true;
                var ds = $("#grid").data("kendoGrid").dataSource;
                var array = JSON.parse(data);
                var elementoNoEncontrado = false;

                if (ExisteJunta(array[0])) {
                    // Proceso validar accion
                    for (var j = 0; j < ds._data.length; j++) {
                        if (array[0].SpoolID == ds._data[j].SpoolID && array[0].JuntaID == ds._data[j].JuntaID && ds._data[j].Accion == 3) { //SPOOL JUNTA ACCION
                            ds._data[j].DetalleJunta = array[0].DetalleJunta;
                            ds._data[j].TemplateSoldadoresRaiz = array[0].TemplateSoldadoresRaiz;
                            ds._data[j].TemplateSoldadoresRelleno = array[0].TemplateSoldadoresRelleno;
                            ds._data[j].TipoJuntaID = array[0].TipoJuntaID;
                            ds._data[j].Accion = array[0].Accion;
                            ds._data[j].ProcesoSoldaduraRaizID = array[0].ProcesoSoldaduraRaizID;
                            ds._data[j].ProcesoSoldaduraRellenoID = array[0].ProcesoSoldaduraRellenoID;
                            ds._data[j].ProcesoSoldaduraRaiz = array[0].ProcesoSoldaduraRaiz;
                            ds._data[j].ProcesoSoldaduraRelleno = array[0].ProcesoSoldaduraRelleno;
                            ds._data[j].Proyecto = array[0].Proyecto;
                            ds._data[j].IDProyecto = array[0].IDProyecto;
                            ds._data[j].IdOrdenTrabajo = array[0].IdOrdenTrabajo;
                            ds._data[j].OrdenTrabajo = array[0].OrdenTrabajo;
                            ds._data[j].idVal = array[0].idVal;
                            ds._data[j].idText = array[0].idText;
                            ds._data[j].SpoolID = array[0].SpoolID;
                            ds._data[j].JuntaID = array[0].JuntaID;
                            ds._data[j].Junta = array[0].Junta;
                            ds._data[j].TipoJunta = array[0].TipoJunta;
                            ds._data[j].Cedula = array[0].Cedula;
                            ds._data[j].Diametro = array[0].Diametro;
                            ds._data[j].TallerID = array[0].TallerID;
                            ds._data[j].Taller = array[0].Taller;
                            ds._data[j].TemplateTrabajosAdicionales = array[0].TemplateTrabajosAdicionales;
                            ds._data[j].FechaSoldadura = array[0].FechaSoldadura;
                            ds._data[j].listaTrabajosAdicionalesSoldadura = array[0].listaTrabajosAdicionalesSoldadura;
                            ds._data[j].ListaTaller = array[0].ListaTaller;
                            ds._data[j].ListaWPS = array[0].ListaWPS;
                            ds._data[j].ListaCedulas = array[0].ListaCedulas;

                            if (array[0].FechaSoldadura != null) {
                                ds._data[j].FechaSoldadura = new Date(ObtenerDato(array[0].FechaSoldadura, 1), ObtenerDato(array[0].FechaSoldadura, 2), ObtenerDato(array[0].FechaSoldadura, 3));//año, mes, dia
                            }

                            var elementgrid = ds._data.splice(j, 1);
                            ds._data.unshift(elementgrid[0]);

                            displayNotify("",
                            _dictionary.CapturaSoldaduraMsgExiste[$("#language").data("kendoDropDownList").value()] +
                            array[0].Junta +
                            _dictionary.CapturaSoldaduraMsgNuevoEnListado[$("#language").data("kendoDropDownList").value()], '0');

                            elementoNoEncontrado = true;
                        }
                    }
                    if (!elementoNoEncontrado)
                        displayNotify("",
                        _dictionary.CapturaSoldaduraMsgExiste[$("#language").data("kendoDropDownList").value()] +
                        array[0].Junta +
                        _dictionary.CapturaSoldaduraMsgExisteListado[$("#language").data("kendoDropDownList").value()], '1');
                }
                else {
                    //Proceso insertar elemento
                    for (var i = 0; i < array.length; i++) {
                        if (array[i].FechaSoldadura != null) {
                            array[i].FechaSoldadura = new Date(ObtenerDato(array[i].FechaSoldadura, 1), ObtenerDato(array[i].FechaSoldadura, 2), ObtenerDato(array[i].FechaSoldadura, 3));//año, mes, dia
                        }
                        $("#Junta").val("");
                    }



                    $("#Junta").data("kendoComboBox").value("");

                    ds.insert(0, array[0]);


                    if (array.length == 0) {
                        displayNotify("",
                            _dictionary.CapturaSoldaduraJuntaCapturada[$("#language").data("kendoDropDownList").value()], '1');
                    }
                    else {
                        displayNotify("",
                            _dictionary.CapturaSoldaduraMsgExiste[$("#language").data("kendoDropDownList").value()] +
                            array[0].Junta +
                            _dictionary.CapturaSoldaduraMsgNuevoEnListado[$("#language").data("kendoDropDownList").value()], '0');
                    }

                    //displayNotify("", 'La junta ' + $('#Junta').data("kendoComboBox").value() + ' ya existe en el listado', '2');
                    $('#ButtonAgregar').prop("disabled", false);
                }
            }
            loadingStop();
        });
        //$('#ButtonAgregar').prop("disabled", false);
    } catch (e) {
        loadingStop();
        displayNotify("Mensajes_error", e.message, '1');
        //$('#ButtonAgregar').prop("disabled", false);
    }
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

function AjaxGuardarCaptura(arregloCaptura, tipoGuardar) {
    loadingStart();
    try {
        var bandera = true, banderaProcesoRaiz = true, banderaProcesoRelleno = true;
        loadingStart();
        Captura = [];
        Captura[0] = { Detalles: "" };
        ListaDetalles = [];

        for (index = 0; index < arregloCaptura.length; index++) {
            $("#grid").data("kendoGrid").dataSource._data[index].RowOk = true;
            ListaDetalles[index] = {
                Accion: "", OrdenTrabajoSpoolID: "", JuntaSpoolID: "",
                TipoJuntaID: "", EtiquetaJunta: "",
                JuntaSoldaduraID: "", TallerID: "",
                ProcesoSoldaduraRaizID: "", ProcesoSoldaduraRellenoID: "", FechaSoldadura: "", ListaSoldaduraRaiz: "",
                ListaSoldaduraRelleno: "", ListaDetalleTrabajoAdicional: "", FechaReporte: "", Estatus: 1
            };

            var labelFecha = String(_dictionary.FormatoFecha[$("#language").data("kendoDropDownList").value()].replace('{', '').replace('}', '').replace("0:", ""));

            ListaDetalles[index].Accion = arregloCaptura[index].Accion;
            ListaDetalles[index].OrdenTrabajoSpoolID = arregloCaptura[index].idVal;
            ListaDetalles[index].JuntaSpoolID = arregloCaptura[index].JuntaID;
            ListaDetalles[index].TipoJuntaID = arregloCaptura[index].TipoJuntaID;
            ListaDetalles[index].EtiquetaJunta = arregloCaptura[index].Junta;
            ListaDetalles[index].JuntaSoldaduraID = arregloCaptura[index].JuntaSoldaduraID;
            ListaDetalles[index].TallerID = arregloCaptura[index].TallerID;
            ListaDetalles[index].WPSID = arregloCaptura[index].WPSID;
            ListaDetalles[index].ProcesoSoldaduraRaizID = arregloCaptura[index].ProcesoSoldaduraRaizID;
            ListaDetalles[index].ProcesoSoldaduraRellenoID = arregloCaptura[index].ProcesoSoldaduraRellenoID;
            ListaDetalles[index].FechaSoldadura =
                kendo.toString(arregloCaptura[index].FechaSoldadura, labelFecha) == null ? "" :
                kendo.toString(arregloCaptura[index].FechaSoldadura, labelFecha).trim();


            if (ListaDetalles[index].TallerID == null || ListaDetalles[index].TallerID == "" || ListaDetalles[index].TallerID == undefined)
                bandera = false;
            if (ListaDetalles[index].WPSID == null || ListaDetalles[index].WPSID == "" || ListaDetalles[index].WPSID == undefined)
                bandera = false;
            if (ListaDetalles[index].ProcesoSoldaduraRaizID == null || ListaDetalles[index].ProcesoSoldaduraRaizID == "" || ListaDetalles[index].ProcesoSoldaduraRaizID == undefined || ListaDetalles[index].ProcesoSoldaduraRaizID == 0)
                banderaProcesoRaiz = false;
            if (ListaDetalles[index].ProcesoSoldaduraRellenoID == null || ListaDetalles[index].ProcesoSoldaduraRellenoID == "" || ListaDetalles[index].ProcesoSoldaduraRellenoID == undefined || ListaDetalles[index].ProcesoSoldaduraRellenoID == 0)
                banderaProcesoRelleno = false;


            ListaTrabajosAdicionalesEditados = [];
            if (arregloCaptura[index].ListaDetalleTrabajoAdicional != undefined) {
                for (j = 0; j < arregloCaptura[index].ListaDetalleTrabajoAdicional.length; j++) {
                    ListaTrabajosAdicionalesEditados[j] = {
                        JuntaSpoolID: "", TallerID: "", Accion: "", JuntaID: "", SoldaduraTrabajoAdicionalID: "",
                        JuntaSoldaduraID: "", TrabajoAdicionalID: "", ObreroID: "", Observacion: ""
                    }

                    if (arregloCaptura[index].ListaDetalleTrabajoAdicional[j].TrabajoAdicionalID == 0 || arregloCaptura[index].ListaDetalleTrabajoAdicional[j].ObreroID == 0)
                        ListaTrabajosAdicionalesEditados[j].Accion = 0;
                    else
                        ListaTrabajosAdicionalesEditados[j].Accion = arregloCaptura[index].ListaDetalleTrabajoAdicional[j].Accion;

                    ListaTrabajosAdicionalesEditados[j].JuntaID = arregloCaptura[index].ListaDetalleTrabajoAdicional[j].JuntaID;
                    ListaTrabajosAdicionalesEditados[j].SoldaduraTrabajoAdicionalID = arregloCaptura[index].ListaDetalleTrabajoAdicional[j].SoldaduraTrabajoAdicionalID;
                    ListaTrabajosAdicionalesEditados[j].JuntaSoldaduraID = arregloCaptura[index].JuntaSoldaduraID;
                    ListaTrabajosAdicionalesEditados[j].JuntaSpoolID = arregloCaptura[index].ListaDetalleTrabajoAdicional[j].JuntaSpoolID;
                    ListaTrabajosAdicionalesEditados[j].TrabajoAdicionalID = arregloCaptura[index].ListaDetalleTrabajoAdicional[j].TrabajoAdicionalID;
                    ListaTrabajosAdicionalesEditados[j].TallerID = arregloCaptura[index].ListaDetalleTrabajoAdicional[j].TallerID
                    ListaTrabajosAdicionalesEditados[j].ObreroID = arregloCaptura[index].ListaDetalleTrabajoAdicional[j].ObreroID;
                    ListaTrabajosAdicionalesEditados[j].Observacion = arregloCaptura[index].ListaDetalleTrabajoAdicional[j].Observacion;
                }
            }


            //-----------------------------------Comparar lista soldadores--------------------------------------------
            //var listaSoldadoresDetalle = arregloCaptura[index].RaizDetalle;
            //var listaSoldadoresNueva = ;
            //var listaSoldadoresInicial = arregloCaptura[index].RaizInicial;
            var listaSoldadoresFinal = arregloCaptura[index].ListaSoldadoresRaizCapturados;

            //if (listaSoldadoresNueva.length == 0) {
            //    for (var j = 0 ; j < listaSoldadoresInicial.length ; j++) {
            //        listaSoldadoresInicial[j].Accion = 3;
            //        listaSoldadoresFinal.push(listaSoldadoresInicial[j]);
            //    }
            //}
            //else {
            //    for (var i = 0; i < listaSoldadoresInicial.length; i++) {
            //        var banderaSoldadores = false;
            //        for (var j = 0 ; j < listaSoldadoresNueva.length ; j++) {
            //            if (listaSoldadoresInicial[i].ObreroID == listaSoldadoresNueva[j].ObreroID) {
            //                listaSoldadoresFinal.push(listaSoldadoresInicial[i]);
            //                banderaSoldadores = true;
            //            }
            //            if ((listaSoldadoresNueva.length - 1) == j && banderaSoldadores == false) {
            //                listaSoldadoresInicial[i].Accion = 3;
            //                listaSoldadoresFinal.push(listaSoldadoresInicial[i]);
            //            }
            //        }
            //    }
            //}

            //if (listaSoldadoresInicial.length == 0)
            //    listaSoldadoresFinal = listaSoldadoresDetalle;
            //else {
            //    for (var i = 0; i < listaSoldadoresNueva.length; i++) {
            //        var banderaSoldadores = false;
            //        var banderaExisteSoldador = false;
            //        for (var j = 0 ; j < listaSoldadoresInicial.length ; j++) {
            //            if (listaSoldadoresNueva[i].ObreroID != listaSoldadoresInicial[j].ObreroID)
            //                banderaSoldadores = true;
            //            else
            //                banderaExisteSoldador = true;
            //            var soldadorDetalle = $.grep(listaSoldadoresDetalle, function (e) {
            //                return e.ObreroID == listaSoldadoresNueva[i].ObreroID;
            //            });
            //            if (soldadorDetalle.length > 0)
            //                if ((listaSoldadoresInicial.length - 1) == j && !banderaExisteSoldador && banderaSoldadores == true && soldadorDetalle[0].Accion == 1)
            //                    listaSoldadoresFinal.push(soldadorDetalle[0]);
            //        }
            //    }
            //}

            ListaSoldadoresEditados = [];

            for (j = 0; j < listaSoldadoresFinal.length; j++) {

                ListaSoldadoresEditados[j] = {
                    Accion: "", JuntaSpoolID: "",
                    JuntaSoldaduraSoldadoID: "", JuntaSoldaduraID: "",
                    EsRaiz: "", ObreroID: "", Comentario: "", ConsumibleID: ""
                };

                ListaSoldadoresEditados[j].Accion = listaSoldadoresFinal[j].Accion == undefined ? 1 : listaSoldadoresFinal[j].Accion;
                ListaSoldadoresEditados[j].JuntaSpoolID = arregloCaptura[index].JuntaID;
                ListaSoldadoresEditados[j].JuntaSoldaduraSoldadoID = listaSoldadoresFinal[j].JuntaSoldaduraSoldadoID == undefined ? 0 : listaSoldadoresFinal[j].JuntaSoldaduraSoldadoID;
                ListaSoldadoresEditados[j].JuntaSoldaduraID = arregloCaptura[index].JuntaSoldaduraID == undefined ? 0 : arregloCaptura[index].JuntaSoldaduraID;
                ListaSoldadoresEditados[j].EsRaiz = 1;
                ListaSoldadoresEditados[j].ObreroID = listaSoldadoresFinal[j].ObreroID;
                ListaSoldadoresEditados[j].Comentario = listaSoldadoresFinal[j].Observaciones == undefined ? "" : listaSoldadoresFinal[j].Observaciones;
                ListaSoldadoresEditados[j].ConsumibleID = listaSoldadoresFinal[j].ColadaID;

            }


            //-----------------------------------Comparar lista soldadores relleno--------------------------------------------
            //var listaRellenoSoldadoresDetalle = arregloCaptura[index].RellenoDetalle;
            //var listaRellenoSoldadoresNueva = arregloCaptura[index].Relleno;
            //var listaRellenoSoldadoresInicial = arregloCaptura[index].RellenoInicial;
            var listaRellenoSoldadoresFinal = arregloCaptura[index].ListaSoldadoresRellenoCapturados;

            //if (listaRellenoSoldadoresNueva.length == 0) {
            //    for (var j = 0 ; j < listaRellenoSoldadoresInicial.length ; j++) {
            //        listaRellenoSoldadoresInicial[j].Accion = 3;
            //        listaRellenoSoldadoresFinal.push(listaRellenoSoldadoresInicial[j]);
            //    }
            //}
            //else {
            //    for (var i = 0; i < listaRellenoSoldadoresInicial.length; i++) {
            //        var banderaSoldadoresRelleno = false;
            //        for (var j = 0 ; j < listaRellenoSoldadoresNueva.length ; j++) {
            //            if (listaRellenoSoldadoresInicial[i].ObreroID == listaRellenoSoldadoresNueva[j].ObreroID) {
            //                listaRellenoSoldadoresFinal.push(listaRellenoSoldadoresInicial[i]);
            //                banderaSoldadoresRelleno = true;
            //            }
            //            if ((listaRellenoSoldadoresNueva.length - 1) == j && banderaSoldadoresRelleno == false) {
            //                listaRellenoSoldadoresInicial[i].Accion = 3;
            //                listaRellenoSoldadoresFinal.push(listaRellenoSoldadoresInicial[i]);
            //            }
            //        }
            //    }
            //}


            //if (listaRellenoSoldadoresInicial.length == 0)
            //    listaRellenoSoldadoresFinal = listaRellenoSoldadoresDetalle;
            //else {
            //    for (var i = 0; i < listaRellenoSoldadoresNueva.length; i++) {
            //        var banderaSoldadoresRelleno = false;
            //        var banderaExisteSoldador = false;
            //        for (var j = 0 ; j < listaRellenoSoldadoresInicial.length ; j++) {
            //            if (listaRellenoSoldadoresNueva[i].ObreroID != listaRellenoSoldadoresInicial[j].ObreroID)
            //                banderaSoldadoresRelleno = true;
            //            else
            //                banderaExisteSoldador = true;

            //            var soldadorRellenoDetalle = $.grep(listaRellenoSoldadoresDetalle, function (e) {
            //                return e.ObreroID == listaRellenoSoldadoresNueva[i].ObreroID;
            //            });

            //            if ((listaRellenoSoldadoresInicial.length - 1) == j && !banderaExisteSoldador && banderaSoldadoresRelleno == true && soldadorRellenoDetalle[0].Accion == 1)
            //                listaRellenoSoldadoresFinal.push(soldadorRellenoDetalle[0]);
            //        }
            //    }
            //}

            ListaRellenoEditados = [];

            for (j = 0; j < listaRellenoSoldadoresFinal.length; j++) {
                ListaRellenoEditados[j] = {
                    Accion: "", JuntaSoldaduraSoldadoID: "",
                    JuntaSoldaduraID: "", EsRaiz: "", ObreroID: "", Comentario: "", ConsumibleID: ""
                };

                ListaRellenoEditados[j].Accion = listaRellenoSoldadoresFinal[j].Accion == undefined ? 1 : listaRellenoSoldadoresFinal[j].Accion;;
                ListaRellenoEditados[j].JuntaSpoolID = arregloCaptura[index].JuntaID;
                ListaRellenoEditados[j].JuntaSoldaduraSoldadoID = listaRellenoSoldadoresFinal[j].JuntaSoldaduraSoldadoID == undefined ? 0 : listaRellenoSoldadoresFinal[j].JuntaSoldaduraSoldadoID;
                ListaRellenoEditados[j].JuntaSoldaduraID = arregloCaptura[index].JuntaSoldaduraID == undefined ? 0 : arregloCaptura[index].JuntaSoldaduraID;
                ListaRellenoEditados[j].EsRaiz = 0;
                ListaRellenoEditados[j].ObreroID = listaRellenoSoldadoresFinal[j].ObreroID;
                ListaRellenoEditados[j].Comentario = listaRellenoSoldadoresFinal[j].Observaciones == undefined ? "" : listaRellenoSoldadoresFinal[j].Observaciones;
                ListaRellenoEditados[j].ConsumibleID = listaRellenoSoldadoresFinal[j].ColadaID;

            }

            ListaDetalles[index].ListaSoldaduraRelleno = ListaRellenoEditados;
            ListaDetalles[index].ListaSoldaduraRaiz = ListaSoldadoresEditados;
            ListaDetalles[index].ListaDetalleTrabajoAdicional = ListaTrabajosAdicionalesEditados;

            if (
                   (
                      ListaDetalles[index].WPSID == "" || ListaDetalles[index].WPSID == 0 ||
                      ListaDetalles[index].TallerID == "" || ListaDetalles[index].TallerID == "0" ||
                      ListaDetalles[index].ProcesoSoldaduraRaizID == "" || ListaDetalles[index].ProcesoSoldaduraRaizID == 0 ||
                      ListaDetalles[index].ProcesoSoldaduraRellenoID == "" || ListaDetalles[index].ProcesoSoldaduraRellenoID == 0 ||
                      ListaDetalles[index].FechaSoldadura == "" || 
                      (ListaDetalles[index].ListaSoldaduraRelleno.length == 0 && arregloCaptura[index].ProcesoSoldaduraRelleno != "N/A") || 
                      (ListaDetalles[index].ListaSoldaduraRaiz.length == 0 && arregloCaptura[index].ProcesoSoldaduraRaiz != "N/A") 
                   ) && (ListaDetalles[index].Accion != 3 && ListaDetalles[index].Accion != 4)
                  ) {
                if ((ListaDetalles[index].WPSID == "" || ListaDetalles[index].WPSID == 0) &&
                      (ListaDetalles[index].TallerID == "" || ListaDetalles[index].TallerID == "0") &&
                      (ListaDetalles[index].ProcesoSoldaduraRaizID == "" || ListaDetalles[index].ProcesoSoldaduraRaizID == 0) &&
                      (ListaDetalles[index].ProcesoSoldaduraRellenoID == "" || ListaDetalles[index].ProcesoSoldaduraRellenoID == 0) &&
                      ListaDetalles[index].FechaSoldadura == "" && ListaDetalles[index].Accion == 2) {
                    ListaDetalles[index].Accion = 4;
                }
                else {

                    if (ListaDetalles[index].WPSID == "" || ListaDetalles[index].WPSID == 0 ||
                        ListaDetalles[index].TallerID == "" || ListaDetalles[index].TallerID == "0" ||
                        ListaDetalles[index].ProcesoSoldaduraRellenoID == "" || ListaDetalles[index].ProcesoSoldaduraRellenoID == 0 ||
                        ListaDetalles[index].ProcesoSoldaduraRaizID == "" || ListaDetalles[index].ProcesoSoldaduraRaizID == 0  ||
                        ListaDetalles[index].FechaSoldadura == "" || 
                        (ListaDetalles[index].ListaSoldaduraRelleno.length == 0 && arregloCaptura[index].ProcesoSoldaduraRelleno != "N/A") ||
                        (ListaDetalles[index].ListaSoldaduraRaiz.length == 0 && arregloCaptura[index].ProcesoSoldaduraRaiz != "N/A")
                        ) {
                        ListaDetalles[index].Estatus = 0;
                        $("#grid").data("kendoGrid").dataSource._data[index].RowOk = false;
                        $("#grid").data("kendoGrid").dataSource.sync();
                        //$('tr[data-uid="' + arregloCaptura[index].uid + '"] ').css("background-color", "#ffcccc");
                    }

                }
            }
                
            else {

            }
        }




        Captura[0].Detalles = ListaDetalles;






        ////        if (bandera) {
        if (!ExistRowEmpty(ListaDetalles)) {
            if (Captura[0].Detalles.length > 0) {
                AjaxEjecutarGuardado(Captura[0], tipoGuardar);
                loadingStart();
            }

        }
        else {
            loadingStop();
            windowTemplate = kendo.template($("#windowTemplate").html());

            ventanaConfirm = $("#ventanaConfirm").kendoWindow({
                iframe: true,
                title: _dictionary.CapturaAvanceIntAcabadoMensajeErrorGuardado[$("#language").data("kendoDropDownList").value()],
                visible: false, //the window will not appear before its .open method is called
                width: "auto",
                height: "auto",
                actions: [],
                modal: true,
                animation: {
                    close: false,
                    open: false
                }
            }).data("kendoWindow");

            ventanaConfirm.content(_dictionary.CapturaAvanceIntAcabadoMensajePreguntaGuardado[$("#language").data("kendoDropDownList").value()] +
                "</br><center><button class='btn btn-blue' id='yesButton'>Si</button><button class='btn btn-blue' id='noButton'> No</button></center>");

            ventanaConfirm.open().center();

            $("#yesButton").click(function () {
                loadingStart();

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


                if (ArregloGuardado.length > 0) {
                    AjaxEjecutarGuardado(Captura[0], tipoGuardar);
                }
                else {
                    loadingStop();
                    displayNotify("AdverteciaExcepcionGuardado", "", '1');
                }

                ventanaConfirm.close();
            });
            $("#noButton").click(function () {
                ventanaConfirm.close();
            });

        }

        loadingStop();
    } catch (e) {
        loadingStop();
        displayNotify("", e.message, '2');
    }

};


function AjaxEjecutarGuardado(Captura, tipoGuardar) {
    $Soldadura.Soldadura.create(Captura, { token: Cookies.get("token"), lenguaje: $("#language").val() }).done(function (data) {
        if (Error(data)) {
            if (data.ReturnMessage.length > 0 && data.ReturnMessage[0] == "Ok") {
                displayNotify("CapturaMensajeGuardadoExitoso", "", '0');
                
                if (tipoGuardar == 1) {
                    editado = false;
                    Limpiar();
                    loadingStop();
                    AjaxCargarCamposPredeterminados();
                }
                else {
                    opcionHabilitarView(true, "FieldSetView");
                    loadingStop();
                    AjaxCambiarAccionAModificacion();
                }
            }
            else {
                loadingStop();
                displayNotify("CapturaMensajeGuardadoErroneo", "", '2');
            }
        }
    });
}


function AjaxCambiarAccionAModificacion() {
    var capturaListado = [];
    capturaListado[0] = { Detalles: "" };
    var listado = ArregloListadoJuntasCapturadas();
    capturaListado[0].Detalles = listado;
    var isReporte = true;

    var listadogrid = $("#grid").data("kendoGrid").dataSource._data;

    $("#grid").data("kendoGrid").dataSource.data([]);

    var differentsJoits = [];
    for (var y = 0; y < listado.length; y++) {
        if (differentsJoits.length == 0) {
            differentsJoits.push(listado[y]);
        }
        else if (differentsJoits[differentsJoits.length - 1].SpoolID != listado[y].SpoolID) {
            differentsJoits.push(listado[y]);
        }
    }

    if ($('input:radio[name=TipoAgregado]:checked').val() == "Listado") {
        isReporte = false;
        differentsJoits = listado;
    }

    for (var x = 0; x < differentsJoits.length; x++) {

        loadingStart();
        $Soldadura.Soldadura.read({ JsonCaptura: JSON.stringify(differentsJoits[x]), isReporte: isReporte, lenguaje: $("#language").val(), token: Cookies.get("token") }).done(function (data) {
            if (Error(data)) {
                var ds = $("#grid").data("kendoGrid").dataSource;
                var array = JSON.parse(data);

                for (var i = 0; i < array.length; i++) {
                    if (array[i].FechaSoldadura != null) {
                        array[i].FechaSoldadura = kendo.toString(array[i].FechaSoldadura, _dictionary.FormatoFecha[$("#language").data("kendoDropDownList").value()]);
                        array[i].FechaSoldadura = new Date(ObtenerDato(array[i].FechaSoldadura, 1), ObtenerDato(array[i].FechaSoldadura, 2), ObtenerDato(array[i].FechaSoldadura, 3));//año, mes, dia
                    }


                    ds.add(array[i]);
                }
            }
            loadingStop();
        });
    }
}