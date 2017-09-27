using BackEndSAM.Models.ServiciosTecnicos.ReporteRT;
using DatabaseManager.Constantes;
using DatabaseManager.Sam3;
using SecurityManager.Api.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace BackEndSAM.DataAcces.ServiciosTecnicos.ReporteRT
{
    public class ReporteRTBD
    {
        private static readonly object _mutex = new object();
        private static ReporteRTBD _instance;

        public static ReporteRTBD Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new ReporteRTBD();
                    }
                }
                return _instance;
            }
        }

        public object ObtenerListadoProyectos(Sam3_Usuario usuario)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<Proyectos> listaProyectos = new List<Proyectos>();
                    List<Sam3_ST_CRRT_Get_ListaProyectos_Result> listaProyectosCTX = ctx.Sam3_ST_CRRT_Get_ListaProyectos(usuario.UsuarioID).ToList();
                    listaProyectos.Add(new Proyectos());
                    foreach (Sam3_ST_CRRT_Get_ListaProyectos_Result item in listaProyectosCTX)
                    {
                        listaProyectos.Add(new Proyectos
                        {
                            ProyectoID = item.ProyectoID,
                            Nombre = item.Nombre,
                            PatioID = item.PatioID,
                            PrefijoOrdenTrabajo = item.PrefijoOrdenTrabajo
                        });
                    }

                    return listaProyectos;
                }
            }
            catch (Exception ex)
            {
                //-----------------Agregar mensaje al Log -----------------------------------------------
                LoggerBd.Instance.EscribirLog(ex);
                //-----------------Agregar mensaje al Log -----------------------------------------------
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(ex.Message);
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;

                return result;
            }
        }

        public object ObtenerListadoProveedores(int proyectoID, int tipoPruebaID, int patioID)
        {
            try
            {

                using (SamContext ctx = new SamContext())
                {

                    List<Proveedor> listaProveedores = new List<Proveedor>();
                    List<Sam3_ST_ReportesRT_Get_Proveedores_Result> result = ctx.Sam3_ST_ReportesRT_Get_Proveedores(proyectoID, tipoPruebaID, patioID).ToList();
                    listaProveedores.Add(new Proveedor());
                    foreach (Sam3_ST_ReportesRT_Get_Proveedores_Result item in result)
                    {
                        listaProveedores.Add(new Proveedor
                        {
                            ProveedorID = item.ProveedorID,
                            Nombre = item.Proveedor,
                            TipoPruebaID = item.TipoPruebaID,
                            TipoPruebaProveedorID = item.TipoPruebaProveedorID
                        });
                    }

                    return listaProveedores;
                }

            }
            catch (Exception ex)
            {
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(ex.Message);
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;

                return result;
            }
        }


        public object ObtenerTipoPruebas(int proyectoID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<TipoPruebaRT> listatiposPruebas = new List<TipoPruebaRT>();
                    List<Sam3_ST_ReportesRT_Get_Tipos_Pruebas_Result> listaTipoPruebasCTX = ctx.Sam3_ST_ReportesRT_Get_Tipos_Pruebas(proyectoID).ToList();
                    listatiposPruebas.Add(new TipoPruebaRT());
                    foreach (Sam3_ST_ReportesRT_Get_Tipos_Pruebas_Result item in listaTipoPruebasCTX)
                    {
                        listatiposPruebas.Add(new TipoPruebaRT
                        {
                            TipoPruebaID = item.TipoPruebaID,
                            Nombre = item.Nombre,
                            RequiereEquipoCaptura = item.RequiereEquipoCaptura.GetValueOrDefault()
                        });
                    }

                    return listatiposPruebas;
                }
            }
            catch (Exception ex)
            {
                //-----------------Agregar mensaje al Log -----------------------------------------------
                LoggerBd.Instance.EscribirLog(ex);
                //-----------------Agregar mensaje al Log -----------------------------------------------
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(ex.Message);
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;

                return result;
            }
        }


        public object ObtenerListadoRequisiciones(Sam3_Usuario usuario, int proyectoID, int proveedorID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {

                    List<Requisicion> listaRequisiciones = new List<Requisicion>();
                    List<Sam3_ST_CRRT_Get_Requisiciones_Result> listaRequisicionesCTX = ctx.Sam3_ST_CRRT_Get_Requisiciones(proyectoID, proveedorID).ToList();
                    listaRequisiciones.Add(new Requisicion());
                    foreach (Sam3_ST_CRRT_Get_Requisiciones_Result item in listaRequisicionesCTX)
                    {
                        listaRequisiciones.Add(new Requisicion
                        {
                            RequisicionID = item.RequisicionID,
                            ProyectoID = item.ProyectoID,
                            TipoPruebaID = item.TipoPruebaID.GetValueOrDefault(),
                            NombreRequisicion = item.NombreRequisicion,
                            FechaRequisicion = item.FechaRequisicion.ToString(),
                            Observacion = item.Observaciones,
                            TurnoLaboralID = item.TurnoLaboralID.GetValueOrDefault(),
                            TurnoLaboral = item.TurnoLaboral
                        });
                    }

                    return listaRequisiciones;
                }
            }
            catch (Exception ex)
            {
                //-----------------Agregar mensaje al Log -----------------------------------------------
                LoggerBd.Instance.EscribirLog(ex);
                //-----------------Agregar mensaje al Log -----------------------------------------------
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(ex.Message);
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;

                return result;
            }
        }

        public object ObtenerListadoEquipos(int tipoPruebaID, int proveedorID, string lenguaje)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {

                    List<Equipo> listaEquipos = new List<Equipo>();
                    List<Sam3_ST_CRRT_Get_Equipo_Result> listaEquiposCTX = ctx.Sam3_ST_CRRT_Get_Equipo(tipoPruebaID, proveedorID, lenguaje).ToList();
                    listaEquipos.Add(new Equipo());
                    foreach (Sam3_ST_CRRT_Get_Equipo_Result item in listaEquiposCTX)
                    {
                        listaEquipos.Add(new Equipo
                        {
                            EquipoID = item.EquipoID,
                            NombreEquipo = item.Equipo,
                            ProveedorEquipoID = item.ProveedorEquipoID.GetValueOrDefault(),
                            TipoPruebaProveedorID = item.TipoPruebaProveedorID
                        });
                    }

                    return listaEquipos;
                }
            }
            catch (Exception ex)
            {
                //-----------------Agregar mensaje al Log -----------------------------------------------
                LoggerBd.Instance.EscribirLog(ex);
                //-----------------Agregar mensaje al Log -----------------------------------------------
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(ex.Message);
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;

                return result;
            }
        }

        public object ObtenerListadoTurnos(int tipoPruebaID, int proveedorID, int equipoID, string lenguaje)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {

                    List<TurnoLaboral> listaTurnosLaborales = new List<TurnoLaboral>();
                    List<Sam3_ST_CRRT_Get_TurnoLaboral_Result> listaTurnosLaboralesCTX = ctx.Sam3_ST_CRRT_Get_TurnoLaboral(tipoPruebaID, proveedorID, equipoID, lenguaje).ToList();
                    listaTurnosLaborales.Add(new TurnoLaboral());
                    foreach (Sam3_ST_CRRT_Get_TurnoLaboral_Result item in listaTurnosLaboralesCTX)
                    {
                        listaTurnosLaborales.Add(new TurnoLaboral
                        {
                            TurnoLaboralID = item.TurnoLaboralID,
                            Turno = item.Turno,
                            Capacidad = item.Capacidad,
                            CapacidadTurnoEquipoID = item.CapacidadTurnoEquipoID,
                            CapacidadTurnoProveedorID = item.CapacidadTurnoProveedorID,
                            TipoPruebaProveedorID = item.TipoPruebaProveedorID
                        });
                    }

                    return listaTurnosLaborales;
                }
            }
            catch (Exception ex)
            {
                //-----------------Agregar mensaje al Log -----------------------------------------------
                LoggerBd.Instance.EscribirLog(ex);
                //-----------------Agregar mensaje al Log -----------------------------------------------
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(ex.Message);
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;

                return result;
            }
        }

        public object ObtenerTurnoLaboralTotal(string lenguaje, int tipoPruebaID)
        {
            try
            {
                List<TurnoLaboral> listaTurnoLaboralTotal = new List<TurnoLaboral>();
                using (SamContext ctx = new SamContext())
                {

                    List<Sam3_ST_CRRT_Get_TurnoLaboralGrid_Result> result = ctx.Sam3_ST_CRRT_Get_TurnoLaboralGrid(lenguaje, tipoPruebaID).ToList();
                    foreach (var item in result)
                    {
                        listaTurnoLaboralTotal.Add(new TurnoLaboral
                        {
                            TurnoLaboralID = item.TurnoLaboralID,
                            Turno = item.Turno,
                            TipoPruebaProveedorID = item.TipoPruebaProveedorID,
                            Capacidad = item.Capacidad,
                            CapacidadTurnoEquipoID = item.CapacidadTurnoEquipoID,
                            CapacidadTurnoProveedorID = item.CapacidadTurnoProveedorID,
                            ElementosAsignados = item.ElementosAsignados,
                            ProveedorEquipoID = item.ProveedorEquipoID
                        });
                    }
                }
                return listaTurnoLaboralTotal;
            }
            catch (Exception ex)
            {
                List<TurnoLaboral> listaTurnoLaboralTotal = new List<TurnoLaboral>();
                return listaTurnoLaboralTotal;
            }
        }

        public object ObtenerDetalleRequisicion(int proyectoID, int tipoPruebaID, int proveedorID, int requisicionID, int equipoID, int turnoID, string lenguaje)
        {
            try
            {

                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_ST_ReportesRT_Get_Requisiciones_Detalle_Result> result = ctx.Sam3_ST_ReportesRT_Get_Requisiciones_Detalle(requisicionID,lenguaje).ToList();

                    int? numPlacas = 0;

                    List<Resultados> listaResultados = ObtenerListadoResultados(lenguaje);


                    List<DetalleCaptura> listaDetalleJunta = new List<DetalleCaptura>();
                    DetalleCaptura detalle = null;
                    foreach (Sam3_ST_ReportesRT_Get_Requisiciones_Detalle_Result item in result)
                    {
                        numPlacas = 0;

                        List<Defectos> listaDefectos = ObtenerListadoDefectos(lenguaje);//por junta
                        List<RangoCuadrante> listaRangoCuadrantes = ObtenerListadoRangoCuadrantes();
                        List<DetallePorPlacas> listaDetallePorPlacas = ObtenerDetallePorPlacas(item.CapturaResultadoID.GetValueOrDefault(), out numPlacas, lenguaje, listaResultados, listaDefectos);

                        numPlacas = (numPlacas == 0 ? null : numPlacas);
                        
                        detalle = new DetalleCaptura
                        {
                            CapturaResultadoID = item.CapturaResultadoID.GetValueOrDefault(),
                            ElementoPorClasificacionPNDID = item.ElementoPorClasificacionPNDID,
                            NumeroControl = item.NumeroControl,
                            NumeroRequisicion = item.NumeroRequisicion,
                            Estatus = item.Estatus.GetValueOrDefault(),
                            Junta = item.Etiqueta,
                            ClasificacionPND = item.ClasificacionPND,
                            TipoPrueba = item.TipoPrueba,
                            Observaciones = item.Observaciones,
                            NumeroPlacas = numPlacas,
                            ResultadoConciliacion = item.ResultadoConciliacion,
                            RazonNoConciliacion = item.RazonNoConciliacion,
                            ListaDetallePorPlacas = listaDetallePorPlacas,
                            Accion = item.CapturaResultadoID  == null ? 1 : 2,//falta negocio.
                            ListaResultados = listaResultados,
                            ListaDefectos = listaDefectos,
                            listaRangoCuadrantes = listaRangoCuadrantes,
                            TemplateDetalleElemento = lenguaje == "es-MX" ? "Ver detalle" : " View detail ",
                            EstatusRequisicion = item.Estatus.GetValueOrDefault(),
                            RequisicionID = item.RequisicionID,
                            EquipoID = item.EquipoID,
                            Equipo = item.Equipo,
                            Turno = item.TurnoLaboral,
                            TurnoLaboralID = item.TurnoLaboralID.GetValueOrDefault(),
                            EsSector = item.Evaluacion.GetValueOrDefault() == 1 ? true : false, // Donde true es sector y false es cuadrante
                            NoPlacas = item.NoPlacas.GetValueOrDefault(),
                            listadoEquipo = (List<Equipo>)ObtenerListadoEquipos(tipoPruebaID,proveedorID,lenguaje),
                            listadoTurno = (List<TurnoLaboral>)ObtenerTurnoLaboralTotal(lenguaje,tipoPruebaID),
                            ProveedorEquipoID = item.ProveedorEquipoID,
                            CapacidadTurnoEquipoID = item.CapacidadTurnoEquipoID,
                            CapacidadTurnoProveedorID = item.CapacidadTurnoProveedorID,
                            Resultado  = item.Resultado,
                            ResultadoID = item.ResultadoID,
                            MinimoPlacasCuadrante = item.MinimoPlacasCuadrante.GetValueOrDefault(),
                            MaximoPlacasCuadrante = item.MaximoPlacasCuadrante.GetValueOrDefault(),
                            MinimoPlacasSector = item.MinimoPlacasSector.GetValueOrDefault(),
                            MaximoPlacasSector = item.MaximoPlacasSector.GetValueOrDefault(),
                            CodigoAsme = "",
                            NombreGrafica = item.NombreGrafica,
                            DefectoID = item.DefectoID,
                            Indicacion = item.Indicacion,
                            RequiereConciliacion = item.RequiereConciliacion,
                            TieneEntregaPlacas = item.TieneEntregaPlacas,
                            RowOk = true
                        };
                        listaDetalleJunta.Add(detalle);

                    }
                    return listaDetalleJunta;
                }

            }
            catch (Exception ex)
            {
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(ex.Message);
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;

                return result;
            }
        }

        public object ObtieneElementosRequisicionParametro(string lenguaje, int requisicionID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    ElementoRequisicion elemento = null;
                    //Sam3_ST_Get_AR_RequisicionParametro_Result result = ctx.Sam3_ST_Get_AR_RequisicionParametro(lenguaje, requisicionID).SingleOrDefault();
                    Sam3_ST_Get_CR_RequisicionParametro_Result result = ctx.Sam3_ST_Get_CR_RequisicionParametro(lenguaje, requisicionID).SingleOrDefault();

                    if (result != null && result.RequisicionID != 0)
                    {
                        elemento = new ElementoRequisicion();

                        List<Proyecto> listaProyecto = new List<Proyecto>();
                        listaProyecto.Add(new Proyecto());
                        listaProyecto.Add(new Proyecto
                        {
                            ProyectoID = result.ProyectoID,
                            Nombre = result.Proyecto,
                            PatioID = result.PatioID,
                            ProveedorID = result.ProveedorID

                        });

                        List<TipoPrueba> listaTipoPrueba = new List<TipoPrueba>();
                        listaTipoPrueba.Add(new TipoPrueba());
                        listaTipoPrueba.Add(new TipoPrueba
                        {
                            TipoPruebaID = result.TipoPruebaID.GetValueOrDefault(),
                            Nombre = result.TipoPrueba
                        });

                        List<Proveedor> ListaProveedor = new List<Proveedor>();
                        ListaProveedor.Add(new Proveedor());
                        ListaProveedor.Add(new Proveedor
                        {
                            ProveedorID = result.ProveedorID,
                            Nombre = result.Proveedor
                        });

                        List<Requisicion2> listaRequisicion = new List<Requisicion2>();
                        listaRequisicion.Add(new Requisicion2());
                        listaRequisicion.Add(new Requisicion2
                        {
                            RequisicionID = result.RequisicionID,
                            ProyectoID = result.ProyectoID,
                            NombreRequisicion = result.NumeroRequisicion,
                            TipoPruebaID = result.TipoPruebaID.GetValueOrDefault(),
                        });


                        elemento.RequisicionID = result.RequisicionID;
                        elemento.ProyectoID = result.ProyectoID;
                        elemento.TipoPruebaID = result.TipoPruebaID.GetValueOrDefault();
                        elemento.listaProyecto = listaProyecto;
                        elemento.listaTipoPrueba = listaTipoPrueba;
                        elemento.listaProveedor = ListaProveedor;
                        elemento.listaRequisicion = listaRequisicion;
                    }
                    return elemento;
                }
            }
            catch (Exception ex)
            {
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(ex.Message);
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;

                return result;
            }
        }

        public object InsertarCapturaResultados(DataTable dtDetalleCaptura, DataTable dtDetalleResultados, DataTable dtDetalleDefectos, int usuario, string lenguaje, int RequisicionID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    ObjetosSQL _SQL = new ObjetosSQL();
                    string[,] parametro = { { "@RequisicionID", RequisicionID.ToString() }, { "@UsuarioID", usuario.ToString() }, { "@lenguaje", lenguaje } };
                    _SQL.Ejecuta(Stords.GUARDARCAPTURAREPORTE, dtDetalleCaptura, "@CapturaResultados", dtDetalleResultados, "@CapturaResultadosPlacas", dtDetalleDefectos, "@CapturaResultadosDefectos", parametro);
                    TransactionalInformation result = new TransactionalInformation();
                    result.ReturnMessage.Add("Ok");
                    result.ReturnCode = 200;
                    result.ReturnStatus = true;
                    result.IsAuthenicated = true;
                    return result;
                }
            }
            catch (Exception ex)
            {
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(ex.Message);
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;

                return result;
            }
        }


        public List<Resultados> ObtenerListadoResultados(string lenguaje)
        {
            List<Resultados> listaResultados = new List<Resultados>();

            try
            {

                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_ST_CRRT_Get_Resultado_Result> result = ctx.Sam3_ST_CRRT_Get_Resultado(lenguaje).ToList();

                    Resultados resultado = null;
                    listaResultados.Add(new Resultados());
                    foreach (Sam3_ST_CRRT_Get_Resultado_Result item in result)
                    {
                        resultado = new Resultados
                        {
                            ResultadosID = item.ResultadosID,
                            Resultado = item.Resultado
                        };

                        listaResultados.Add(resultado);
                    }
                    return listaResultados;
                }

            }
            catch (Exception ex)
            {
                return listaResultados;
            }
        }

        public List<DetallePorPlacas> ObtenerDetallePorPlacas(int CapturaResultadoID, out int? numeroPlacas, string lenguaje, List<Resultados> listaResultados, List<Defectos> listaDefectos)
        {
            List<DetallePorPlacas> listaDetalleResultado = new List<DetallePorPlacas>();
            try
            {

                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_ST_ReportesRT_Get_Resultados_Result> result = ctx.Sam3_ST_ReportesRT_Get_Resultados(CapturaResultadoID, lenguaje).ToList();

                    numeroPlacas = result.Count;

                    DetallePorPlacas detalleResultado = null;

                    foreach (Sam3_ST_ReportesRT_Get_Resultados_Result item in result)
                    {
                        List<DetalleResultadosDefectos> listadoDetalleResultadoDefectos = ObtenerDetalleIndicaciones(item.CapturaResultadoPlacaID, listaDefectos, lenguaje);

                        detalleResultado = new DetallePorPlacas
                        {
                            Ubicacion = item.Ubicacion,
                            ResultadoID = int.Parse(item.ResultadosID.ToString()),
                            Resultado = item.Resultado,
                            ListaDetalleDefectos = listadoDetalleResultadoDefectos,
                            TemplateDetallePorPlaca = lenguaje == "es-MX" ? "Detalle" : "Detail",
                            Accion = 2,
                            ListaResultados = listaResultados,
                            ListaDefectos = listaDefectos,
                            CapturaResultadoID = item.CapturaResultadoID,
                            CapturaResultadoPlacaID = item.CapturaResultadoPlacaID,
                            ElementoPorClasificacionPNDID = item.ElementoPorClasificacionPNDID
                        };
                        listaDetalleResultado.Add(detalleResultado);
                    }
                    return listaDetalleResultado;
                }

            }
            catch (Exception ex)
            {
                numeroPlacas = 0;
                return listaDetalleResultado;
            }
        }

        public List<DetalleResultadosDefectos> ObtenerDetalleIndicaciones(int capturaResultadoPlacaID, List<Defectos> listaDefectos, string lenguaje)
        {
            List<DetalleResultadosDefectos> listaDetalleDefectos = new List<DetalleResultadosDefectos>();
            try
            {

                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_ST_ReportesRT_Get_Resultados_Detalle_Result> result = ctx.Sam3_ST_ReportesRT_Get_Resultados_Detalle(capturaResultadoPlacaID,lenguaje).ToList();
                    
                    foreach (Sam3_ST_ReportesRT_Get_Resultados_Detalle_Result item in result)
                    {
                        
                        listaDetalleDefectos.Add(new DetalleResultadosDefectos
                        {
                            Accion = 2,
                            CapturaResultadoPlacaID = item.CapturaResultadoPlacaID,
                            CapturaResultadoPlacaDefectoID = item.CapturaResultadoPlacaDefectoID,
                            ElementoPorClasificacionPNDID = item.ElementoPorClasificacionPNDID,
                            DefectoID = item.DefectoID,
                            Defecto = item.Defecto,
                            InicioMM = item.InicioMM.GetValueOrDefault(),
                            FinMM = item.FinMM.GetValueOrDefault(),
                            RangoCuadranteID = item.RangoCuadranteID.GetValueOrDefault(),
                            ResultadoID = item.ResultadoID,
                            Resultado = item.Resultado,
                            Ubicacion = item.Ubicacion,
                            Cuadrante = item.Cuadrante
                            
                        });
                    }

                    return listaDetalleDefectos;
                }

            }
            catch (Exception ex)
            {
                return listaDetalleDefectos;
            }
        }

        public List<Defectos> ObtenerListadoDefectos( string lenguaje)
        {
            List<Defectos> listaDefectos = new List<Defectos>();
            try
            {

                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_ST_ReportesRT_Get_CatalogoDefectos_Result> result = ctx.Sam3_ST_ReportesRT_Get_CatalogoDefectos( lenguaje).ToList();

                    Defectos defecto = null;
                    listaDefectos.Add(new Defectos());
                    foreach (Sam3_ST_ReportesRT_Get_CatalogoDefectos_Result item in result)
                    {
                        defecto = new Defectos
                        {
                            DefectoID = item.DefectoID,
                            Defecto = item.Defecto
                        };

                        listaDefectos.Add(defecto);
                    }
                    return listaDefectos;
                }

            }
            catch (Exception ex)
            {
                return listaDefectos;
            }
        }


        public List<RangoCuadrante> ObtenerListadoRangoCuadrantes()
        {
            List<RangoCuadrante> listaRangoCuadrantes = new List<RangoCuadrante>();
            try
            {

                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_ST_ReportesRT_Get_RangoCuadrantes_Result> result = ctx.Sam3_ST_ReportesRT_Get_RangoCuadrantes().ToList();

                    RangoCuadrante elemento = null;
                    listaRangoCuadrantes.Add(new RangoCuadrante());
                    foreach (Sam3_ST_ReportesRT_Get_RangoCuadrantes_Result item in result)
                    {
                        elemento = new RangoCuadrante
                        {
                            RangoCuadranteID = item.RangoCuadranteID,
                            Cuadrante = item.Cuadrante
                        };

                        listaRangoCuadrantes.Add(elemento);
                    }
                    return listaRangoCuadrantes;
                }

            }
            catch (Exception ex)
            {
                return listaRangoCuadrantes;
            }
        }

    }

}