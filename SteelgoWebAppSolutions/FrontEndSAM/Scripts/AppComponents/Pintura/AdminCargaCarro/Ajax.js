function AjaxCargarCamposPredeterminados() {
	loadingStart();
	var TipoMuestraPredeterminadoID = 46;
	$CamposPredeterminados.CamposPredeterminados.read({ token: Cookies.get("token"), lenguaje: $("#language").val(), id: TipoMuestraPredeterminadoID }).done(function (data) {
		if (data == "Escritorio") {
			$("#styleEscritorio").addClass("active");
			$("#stylePatio").removeClass("active");

			$('input:radio[name=TipoVista]:nth(0)').attr('checked', true);
			$('input:radio[name=TipoVista]:nth(1)').attr('checked', false);
			$('input:radio[name=TipoVista]:nth(0)').trigger("change");
			$("#contenedorPrincipalEscritorio").show();
			$("#contenedorSecundarioEscritorio").show();
			$("#contenedorPrincipalPatio").hide();
			$("#contenedorSecundarioPatio").hide();
		}
		else if (data == "Patio") {
			$("#styleEscritorio").removeClass("active");
			$("#stylePatio").addClass("active");

			$('input:radio[name=TipoVista]:nth(0)').attr('checked', false);
			$('input:radio[name=TipoVista]:nth(1)').attr('checked', true);
			$('input:radio[name=TipoVista]:nth(1)').trigger("change");
			$("#contenedorPrincipalEscritorio").hide();
			$("#contenedorSecundarioEscritorio").hide();
			$("#contenedorPrincipalPatio").show();
			$("#contenedorSecundarioPatio").show();
		}
	});

	AjaxCargaMostrarPredeterminado();
	loadingStop();
};

function AjaxCargaMostrarPredeterminado() {
	var TipoMuestraPredeterminadoID = 2048;
	$CamposPredeterminados.CamposPredeterminados.read({ token: Cookies.get("token"), lenguaje: $("#language").val(), id: TipoMuestraPredeterminadoID }).done(function (data) {
		if (data == "sin captura") {
			$('input:radio[name=Muestra]:nth(0)').trigger("click");
		}
		else if (data == "Todos") {
			$('input:radio[name=Muestra]:nth(1)').trigger("click");
		}
	});

	AjaxCargaTipoSpool();
}

function AjaxCargaTipoSpool() {
	var TipoMuestraPredeterminadoID = 34;
	$CamposPredeterminados.CamposPredeterminados.read({ token: Cookies.get("token"), lenguaje: $("#language").val(), id: TipoMuestraPredeterminadoID }).done(function (data) {
		if (data == "Spool") {
			$('input:radio[name=TipoSeleccion]:nth(0)').attr('checked', true).trigger("change");
		}
		else if (data == "Codigo") {
			$('input:radio[name=TipoSeleccion]:nth(1)').attr('checked', true).trigger("change");
		}

	});
}

function AjaxCargarProyecto() {
	$Proyectos.Proyectos.read({ token: Cookies.get("token") }).done(function (data) {
		$("#inputProyecto").data("kendoComboBox").dataSource.data([]);
		var proyectoId = 0;

		if (data.length > 0) {
			$("#inputProyecto").data("kendoComboBox").dataSource.data(data);
			if (data.length == 2) {
				$("#inputProyecto").data("kendoComboBox").value(data[1].ProyectoID);
				$("#inputProyecto").data("kendoComboBox").trigger("change");
			}

		}
	});
}

function AjaxCargarMedioTransporte(ProyectoID, nuevoCarro) {

	$PinturaGeneral.PinturaGeneral.read({ token: Cookies.get("token"), lenguaje: $("#language").val(), proyectoID: ProyectoID }).done(function (data) {
		var medioTranporteId = 0;

		//se agrega la opcion del carro nuevo
		data.splice(1, 0, {
			MedioTransporteID: -1,
			Nombre: _dictionary.PinturaCargaAgregarNuevoCarro[$("#language").data("kendoDropDownList").value()],
			MedioTransporteCargaID: 0,
			CarroCerrado: false
		});

		$("#inputCarro").data("kendoComboBox").dataSource.data(data);
		var carroEcontrado = false;
		if (data.length == 3) {
			$("#inputCarro").data("kendoComboBox").value(data[2].MedioTransporteID);
			$("#inputCarro").data("kendoComboBox").trigger("change");
		}
		else {
			if (nuevoCarro != null) {
				for (var i = 0; i < data.length; i++) {
					if (data[i].Nombre == nuevoCarro) {
						carroEcontrado = true;
						$("#inputCarro").data("kendoComboBox").value(data[i].MedioTransporteID);
						$("#inputCarro").data("kendoComboBox").trigger("change");
						break;
					}
				}
				if (!carroEcontrado) {
					$("#chkCerrar")[0].checked = false;
				}
			}
			else {
				$("#inputCarro").data("kendoComboBox").trigger("change");
				$("#chkCerrar")[0].checked = false;
			}
		}

	});
}

function AjaxGuardarNuevoCarro() {

	try {


		loadingStart();

		var Proyecto = $("#inputProyecto").data("kendoComboBox").dataItem($("#inputProyecto").data("kendoComboBox").select());
		var Captura = { Nombre: $("#InputNombre").val(), UsuarioID: 0, PatioID: Proyecto.PatioID, ProyectoID: Proyecto.ProyectoID };

		if ($("#InputNombre").val() != "" && $("#InputNombre").val() != null) {
			$PinturaGeneral.PinturaGeneral.create(Captura, { token: Cookies.get("token") }).done(function (data) {
				if (data.ReturnMessage.length > 0 && data.ReturnMessage[0] == "Ok") {
					AjaxCargarMedioTransporte(Proyecto.ProyectoID, $("#InputNombre").val());
					displayNotify("PinturaGuardarNuevoCarro", "", '0');
					windowNewCarriage.close();
				}
				else if (data.ReturnMessage.length > 0 && data.ReturnMessage[0] != "Ok") {
					displayNotify("PinturaErrorGuardarNuevoCarro", "", '2');
				}
				loadingStop();
			});
		}
		else {
			displayNotify("PinturaGuardarNuevoCarroNombre", "", '2');
			loadingStop();
		}
	} catch (e) {
		loadingStop();
		displayNotify("Mensajes_error", e.message, '0');

	}
}

function AjaxObtenerDetalleCargaCarro(MedioTransporteID, tipoEscenario, valorBusqueda) {
	loadingStart();
	var SpoolPerteneceProyecto = true;
	if ($("#inputProyecto").data("kendoComboBox").dataItem($("#inputProyecto").data("kendoComboBox").select()).ProyectoID > 0) {
		if ($("#inputCarro").data("kendoComboBox").dataItem($("#inputCarro").data("kendoComboBox").select()).MedioTransporteID > 0) {
			if (valorBusqueda != "" && valorBusqueda.split('~')[1] == "Spool" && $("#inputProyecto").data("kendoComboBox").dataItem($("#inputProyecto").data("kendoComboBox").select()).PrefijoOrdenTrabajo != $("#InputOrdenTrabajo").val()[0]) {
				SpoolPerteneceProyecto = false;
				displayNotify("EmbarqueCargaMsjErrorSpoolAgregarProyectoIncorrecto", "", '1');
			}

			if (SpoolPerteneceProyecto) {

				$CargaCarro.CargaCarro.read({ medioTransporteID: MedioTransporteID, token: Cookies.get("token"), proyectoID: $("#inputProyecto").data("kendoComboBox").value(), lenguaje: $("#language").val(), escenario: tipoEscenario, valorBusqueda: valorBusqueda }).done(function (data) {
					carroActualSeleccionado = $("#inputCarro").data("kendoComboBox").dataItem($("#inputCarro").data("kendoComboBox").select());
					var ds = $("#grid").data("kendoGrid").dataSource;
					var array = data;
					var elementosNoModificados = "";
					var elementosModificados = "";

					var sistemaPinturaID = 0;
					var sistemaPintura;
					if (data.length > 0) {

					    editado = true;
					    if (valorBusqueda == "") {

					        $("#grid").data("kendoGrid").dataSource.data([]);
					        $("#grid").data("kendoGrid").dataSource.data(data);
					        editado = true;
					    }
					    else {

					        var CargaCarroID = 0;
					        //obtenemos el id de la carga
					        CargaCarroID = array.length > 0 ? array[0].MedioTransporteCargaDetalleID : 0;

					        if (ds._data.length > 0) {
					            sistemaPinturaID = ds._data[0].SistemaPinturaID;
					            sistemaPintura = ds._data[0].SistemaPintura
					        }


					        for (var i = 0; i < array.length; i++) {
					            if (!validarInformacion(array[i])) {
					                if (array[i].CarroID == 0) {
					                    if (array[i].PatioID != 7) {
					                        if (sistemaPinturaID == 0) {
					                            if (array[i].SistemaPinturaID == 0 || array[i].SistemaPinturaID == undefined) {
					                                displayNotify("", _dictionary.PinturaCargaCarroSinSpools[$("#language").data("kendoDropDownList").value()], '1');
					                            }
					                            else if (array[i].NoPintable) {
					                                displayNotify("menuSistemaPinturaNoPintable", "", '1');
					                            }
					                            else {
					                                ds._data.unshift(array[i]);
					                                //ds.add();
					                                if (elementosModificados != "")
					                                    elementosModificados += ", " + array[i].NumeroControl;
					                                else
					                                    elementosModificados = array[i].NumeroControl;
					                            }
					                        } else if (sistemaPinturaID == array[i].SistemaPinturaID) {
					                            array[i].MedioTransporteCargaDetalleID = CargaCarroID == 0 ? $("#inputCarro").data("kendoComboBox").dataItem($("#inputCarro").data("kendoComboBox").select()).MedioTransporteCargaID : CargaCarroID;
					                            if (array[i].SistemaPinturaID == 0 || array[i].SistemaPinturaID == undefined) {
					                                displayNotify("", _dictionary.PinturaCargaCarroSinSpools[$("#language").data("kendoDropDownList").value()], '1');
					                            }
					                            else if (array[i].NoPintable) {
					                                displayNotify("menuSistemaPinturaNoPintable", "", '1');
					                            }
					                            else {
					                                ds._data.unshift(array[i]);
					                                //ds.add(array[i]);
					                                if (elementosModificados != "")
					                                    elementosModificados += ", " + array[i].NumeroControl;
					                                else
					                                    elementosModificados = array[i].NumeroControl;
					                            }
					                        }
					                        else {
					                            if (sistemaPintura == array[i].SistemaPintura && sistemaPinturaID != array[i].SistemaPinturaID) {
					                                displayNotify("PinturaSpoolSistemaPinturaNoCoincideVersion", "", '1');
					                            }
					                            else if (array[i].SistemaPinturaID == 0 || array[i].SistemaPinturaID == undefined) {
					                                displayNotify("", _dictionary.PinturaCargaCarroSinSpools[$("#language").data("kendoDropDownList").value()], '1');
					                            }
					                            else {
					                                //alert("NO SE INSERTA SP DIFERENTE" + sistemaPinturaID + " " + array[i].SistemaPinturaID);
					                                displayNotify("PinturaSpoolSistemaPinturaNoCoincide", "", '1');
					                            }
					                        }
					                    }
					                    else {
					                        displayNotify("", _dictionary.PinturaSpoolenEmbarque[$("#language").data("kendoDropDownList").value()] + array[i].Cuadrante, '1');
					                    }
					                }
					                else {
					                    displayNotify("", _dictionary.PinturaSpoolCargadoEnCarro[$("#language").data("kendoDropDownList").value()].replace('?', array[i].MedioTransporte), '1');
					                }
					            }
					            else {
					                if (elementosModificados != "")
					                    elementosNoModificados += ", " + array[i].NumeroControl;
					                else
					                    elementosNoModificados = array[i].NumeroControl;
					            }
					        }

					        if (elementosModificados != "") {
					            displayNotify("", _dictionary.SpoolAgregado[$("#language").data("kendoDropDownList").value()] +
									elementosModificados + _dictionary.CapturaArmadoMsgNuevoEnReporte[$("#language").data("kendoDropDownList").value()], '0');
					            editado = true;

					        }

					        if (elementosNoModificados != "") {
					            displayNotify("", _dictionary.SpoolAgregado[$("#language").data("kendoDropDownList").value()] +
									elementosNoModificados + _dictionary.CapturaArmadoMsgExisteReporte[$("#language").data("kendoDropDownList").value()], '1');
					        }

					        if (valorBusqueda.split("~")[1] == "Spool") {
					            $("#InputID").data("kendoComboBox").value("");
					            $("#InputID").val("")
					        }
					        else {
					            $("#inputCodigo").val("")
					        }

					    }
					    $("#grid").data("kendoGrid").refresh();

					    ImprimirAreaTonelada();
					}
					else {
					    var ds = $("#grid").data("kendoGrid").dataSource;
					    if (data.length > 0)
					        ds.page(1);
					}




					loadingStop();
				});
			}
			else {
				loadingStop();
			}
		}
		else {
			displayNotify("PinturaSeleccionaCarro", "", '1');
			loadingStop();
		}
	}
	else {
		displayNotify("SistemaPinturaMensajeErrorProyecto", "", '1');
		loadingStop();
	}
}

function AjaxObtenerSpoolID() {

	var OrdenTrabajoOrigianl = $("#InputOrdenTrabajo").val();
	$Armado.Armado.read({ ordenTrabajo: $("#InputOrdenTrabajo").val(), tipo: '1', token: Cookies.get("token"), lenguaje: $("#language").val(), proyectoID: $("#inputProyecto").data("kendoComboBox").dataItem($("#inputProyecto").data("kendoComboBox").select()).ProyectoID }).done(function (data) {
		dataSpoolArray = data;
		if (Error(data)) {
			if (data.OrdenTrabajo != "") {
				$("#InputOrdenTrabajo").val(data.OrdenTrabajo);
			}
			else {
				$("#InputOrdenTrabajo").val(OrdenTrabajoOrigianl);
				displayNotify("CapturaArmadoMensajeOrdenTrabajoNoEncontrada", "", '1');
			}

			$("#InputID").data("kendoComboBox").dataSource.data(data.idStatus);
			Cookies.set("LetraProyecto", data.OrdenTrabajo.substring(0, 1), { path: '/' });
			$("#InputID").data("kendoComboBox").enable(true);
			$("#InputID").data("kendoComboBox").input.focus();
		}
	});
}

function ajaxGuardarPatio(arregloCaptura, guardarYNuevo) {
	try {

		if (arregloCaptura.length > 0) {

			var CarroSeleccionado = $("#inputCarro").data("kendoComboBox").dataItem($("#inputCarro").data("kendoComboBox").select());
			var CarroID = CarroSeleccionado.MedioTransporteID;
			var CargaCarroID = CarroSeleccionado.MedioTransporteCargaID;

			loadingStart();
			Captura = [];
			Captura[0] = { Detalles: "" };
			ListaGuardarDetalles = [];

			for (index = 0; index < arregloCaptura.length; index++) {
				ListaGuardarDetalles[index] = { SpoolID: "", MedioTransporteCargaID: 0, Accion: "", CuadranteID: 0 };
				ListaGuardarDetalles[index].Accion = arregloCaptura[index].Accion;
				ListaGuardarDetalles[index].SpoolID = arregloCaptura[index].SpoolID;
				ListaGuardarDetalles[index].CuadranteID = arregloCaptura[index].CuadranteID;
				ListaGuardarDetalles[index].MedioTransporteCargaID = arregloCaptura[index].MedioTransporteCargaDetalleID;
			}

			Captura[0].Detalles = ListaGuardarDetalles;

			var carroCerrado = 0;
			if ($('#chkCerrar').is(':checked') /*&& carroCerrado == "false"*/) {
				carroCerrado = 1;
			}
			$CargaCarro.CargaCarro.create(Captura[0], { token: Cookies.get("token"), lenguaje: $("#language").val(), medioTransporteID: CarroID, medioTransporteCargaID: CargaCarroID, cerrar: carroCerrado }).done(function (data) {

				if (data.ReturnMessage.length > 0 && data.ReturnMessage[0] == "Ok") {
					$("#inputCarro").data("kendoComboBox").dataItem($("#inputCarro").data("kendoComboBox").select()).CarroCerrado = carroCerrado;
					if (!guardarYNuevo) {
						opcionHabilitarView(true, "FieldSetView");
						AjaxObtenerDetalleCargaCarro($("#inputCarro").data("kendoComboBox").select() == -1 ? 0 : $("#inputCarro").data("kendoComboBox").dataItem($("#inputCarro").data("kendoComboBox").select()).MedioTransporteID, $('input:radio[name=TipoVista]:checked').val(), '');
						displayNotify("PinturaCargaGuardar", "", '0');

					} else {
						displayNotify("PinturaCargaGuardar", "", '0');
						Limpiar();
						editado = false;
					}

				} else if (data.ReturnMessage.length > 0 && data.ReturnMessage[0] != "Ok") {
					displayNotify("PinturaGuardarErrorGuardar", "", '2');
				}
				loadingStop();
			});
		}

		else {
			if ($('#chkCerrar').is(':checked')) {
				displayNotify("PinturaCargaCarroSistemaPinturaCerradoSinSpools", "", '1');
			}
			else {
				displayNotify("MensajeAdverteciaExcepcionGuardado", "", '1');
			}
		}
	} catch (e) {
		loadingStop();
		displayNotify("", _dictionary.Mensajes_error[$("#language").data("kendoDropDownList").value()] + e.message, '2');

	}
}

function ajaxGuardarEscritorio(listaSpool, guardarYNuevo) {
	var count = 0;
	var contSave = 0;
	var proyectoID = $("#inputProyecto").data("kendoComboBox").value() != undefined ? $("#inputProyecto").data("kendoComboBox").value() : "";
	var CarroSeleccionado = $("#inputCarro").data("kendoComboBox").dataItem($("#inputCarro").data("kendoComboBox").select());
	var CarroID = CarroSeleccionado == undefined ? 0 : CarroSeleccionado.MedioTransporteID;
	var CargaCarroID = CarroSeleccionado == undefined ? 0 : CarroSeleccionado.MedioTransporteCargaID;
	var carroCerrado = $("#inputCarro").attr("mediotransportecerrado");

	Captura = [];
	Captura[0] = { Detalles: "" };

	ListaGuardarDetalles = [];
	var sistemaPinturaCarro = "";
	var sistemaPinturaCarroPrimerElementoSeleccionado = "";
	if (CarroID != "" && CarroID != "0" && CarroID != "-1") {
		if (listaSpool.length > 0) {
			for (var index = 0; index < listaSpool.length; index++) {
				if (listaSpool[index].Seleccionado) {
					ListaGuardarDetalles[contSave] = { SpoolID: "", MedioTransporteCargaID: 0, Accion: "", CuadranteID: 0, SistemaPinturaID: "" };
					ListaGuardarDetalles[contSave].Accion = listaSpool[index].Accion;
					ListaGuardarDetalles[contSave].SpoolID = listaSpool[index].SpoolID;
					ListaGuardarDetalles[contSave].CuadranteID = listaSpool[index].CuadranteID;
					ListaGuardarDetalles[contSave].MedioTransporteCargaID = listaSpool[index].MedioTransporteCargaDetalleID;
					ListaGuardarDetalles[contSave].SistemaPinturaID = listaSpool[index].SistemaPinturaID;
					if (sistemaPinturaCarroPrimerElementoSeleccionado == "")
						sistemaPinturaCarroPrimerElementoSeleccionado = listaSpool[index].SistemaPintura;
					contSave++;
				}
				if (listaSpool[index].Accion == 2)
					sistemaPinturaCarro = listaSpool[index].SistemaPintura;
			}

			if (sistemaPinturaCarro == "") {
				sistemaPinturaCarro = sistemaPinturaCarroPrimerElementoSeleccionado;
			}


			var carroCerrado = 0;
			if ($('#chkCerrar').is(':checked') /*&& carroCerrado == "false"*/) {
				carroCerrado = 1;
			}

			if (ListaGuardarDetalles.length > 0) {
				if (ServicioPinturaCorrecto(ListaGuardarDetalles)) {
					Captura[0].Detalles = ListaGuardarDetalles;

					loadingStart();
					$CargaCarro.CargaCarro.create(Captura[0], { token: Cookies.get("token"), lenguaje: $("#language").val(), medioTransporteID: CarroID, medioTransporteCargaID: CargaCarroID, cerrar: carroCerrado }).done(function (data) {
						if (data.ReturnMessage.length > 0 && data.ReturnMessage[0] == "Ok") {
							$("#inputCarro").data("kendoComboBox").dataItem($("#inputCarro").data("kendoComboBox").select()).CarroCerrado = carroCerrado;
							if (!guardarYNuevo) {
								opcionHabilitarView(true, "FieldSetView");
								AjaxObtenerDetalleCargaCarro($("#inputCarro").data("kendoComboBox").select() == -1 ? 0 : $("#inputCarro").data("kendoComboBox").dataItem($("#inputCarro").data("kendoComboBox").select()).MedioTransporteID, $('input:radio[name=TipoVista]:checked').val(), '');
								displayNotify("PinturaCargaGuardar", "", '0');

							} else {
								displayNotify("PinturaCargaGuardar", "", '0');
								//$("#btnCancelar").trigger("clik");
								Limpiar();
								editado = false;
							}

						}
						else if (data.ReturnMessage.length > 0 && data.ReturnMessage[0] != "Ok") {
							displayNotify("PinturaGuardarErrorGuardar", "", '2');
							loadingStop();
						}
						else {
							loadingStop();
						}

					});
				}
				else {

					displayNotify("", _dictionary.PinturaCargaBackLogMensajeErrorServicioPintura[$("#language").data("kendoDropDownList").value()] + sistemaPinturaCarro, "1");

				}
			}
			else {
				displayNotify("PinturaCargaBackLogMensajeSeleccionaSpool", "", "1");
			}
		}
		else {
			if ($('#chkCerrar').is(':checked')) {
				displayNotify("PinturaCargaCarroSistemaPinturaCerradoSinSpools", "", '1');
			}
			else {
				displayNotify("MensajeAdverteciaExcepcionGuardado", "", '1');
			}
		}
	}
	else {
		displayNotify("PinturaSeleccionaCarro", "", '1');
	}
}

function AjaxCargarZona(patioID, dataItem) {

	$Zona.Zona.read({ token: Cookies.get("token"), PatioID: patioID }).done(function (data) {
		var ZonaId = 0;
		if (data.length > 0) {
			$("#inputZonaPopup").data("kendoComboBox").dataSource.data(data);
			if (data.length < 3) {//si solo tiene una zona asignada
				for (var i = 0; i < data.length; i++) {
					if (data[i].ZonaID != 0) {
						ZonaId = data[i].ZonaID;
					}
				}
				$("#inputZonaPopup").data("kendoComboBox").value(ZonaId);
				$("#inputZonaPopup").data("kendoComboBox").trigger("change");
			}
			else {// si ya se tiene una zona asignada.
				$("#inputZonaPopup").data("kendoComboBox").value(dataItem.ZonaAnteriorID == 0 && dataItem.CuadranteAnteriorID != 0 ? 1 : dataItem.ZonaAnteriorID);
				$("#inputZonaPopup").data("kendoComboBox").trigger("change");
			}
		}
	});
}

function AjaxCargarCuadrante(zonaID) {
	$Cuadrante.Cuadrante.read({ token: Cookies.get("token"), ZonaID: zonaID }).done(function (data) {
		var CuadranteId = 0;

		if (data.length > 0) {

			if ($("#windowDownload").data("kendoWindow").element.is(":hidden"))
				windowDownload.open().center();

			$("#inputCuadrantePopup").data("kendoComboBox").dataSource.data(data);

			if (data.length < 3 && CuadranteSpoolAnterior == 0) {
				for (var i = 0; i < data.length; i++) {
					if (data[i].CuadranteID != 0) {
						CuadranteId = data[i].CuadranteID;
					}
				}
			}
			else
				CuadranteId = CuadranteSpoolAnterior;

			$("#inputCuadrantePopup").data("kendoComboBox").value(CuadranteId);
			$("#inputCuadrantePopup").data("kendoComboBox").trigger("change");
		}
	});
}

function AjaxDescargarSpool(dataItem, Cuadrante) {
	loadingStart();
	var dataSource = $("#grid").data("kendoGrid").dataSource;
	var elemento = 0;
	$PinturaGeneral.PinturaGeneral.read({ token: Cookies.get("token"), CarroID: $("#inputCarro").data("kendoComboBox").dataItem($("#inputCarro").data("kendoComboBox").select()).MedioTransporteID, SpoolID: dataItem.SpoolID, CuadranteID: Cuadrante.CuadranteID, CuadranteSam2ID: Cuadrante.CuadranteSam2ID, CuadranteAnterior: dataItem.CuadranteID, Pantalla: 1 }).done(function (data) {
		if (data.ReturnMessage.length > 0 && data.ReturnMessage[0] == "OK") {
			var dataSource = $("#grid").data("kendoGrid").dataSource;
			dataSource.remove(dataItem);
			loadingStop();

			if ($('input:radio[name=TipoVista]:checked').val() != "Patio")
			    AjaxObtenerDetalleCargaCarro($("#inputCarro").data("kendoComboBox").select() == -1 ? 0 : $("#inputCarro").data("kendoComboBox").dataItem($("#inputCarro").data("kendoComboBox").select()).MedioTransporteID, $('input:radio[name=TipoVista]:checked').val(), '');
			else
			    ImprimirAreaTonelada();

			displayNotify("EmbarqueCargaMsjDescargaSpoolExito", "", "0");
			
		} else {
			loadingStop();
			displayNotify("EmbarqueCargaMsjDescargaSpoolError", "", "2");
		}

	});
}
