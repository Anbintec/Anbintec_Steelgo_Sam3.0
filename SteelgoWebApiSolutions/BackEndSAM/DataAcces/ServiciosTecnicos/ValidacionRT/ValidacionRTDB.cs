using BackEndSAM.Models.ServiciosTecnicos.ValidacionRT;
using DatabaseManager.Constantes;
using DatabaseManager.Sam3;
using SecurityManager.Api.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace BackEndSAM.DataAcces.ServiciosTecnicos.ValidacionRT
{
    public class ValidacionRTDB
    {
        private static readonly object _mutex = new object();
        private static ValidacionRTDB _instance;

        public static ValidacionRTDB Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new ValidacionRTDB();
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
                    List<Sam3_ST_VR_Get_ListaProyectos_Result> listaProyectosCTX = ctx.Sam3_ST_VR_Get_ListaProyectos(usuario.UsuarioID).ToList();
                    listaProyectos.Add(new Proyectos());
                    foreach (Sam3_ST_VR_Get_ListaProyectos_Result item in listaProyectosCTX)
                    {
                        listaProyectos.Add(new Proyectos
                        {
                            ProyectoID = item.ProyectoID,
                            Nombre = item.Nombre,
                            PatioID = item.PatioID,
                            PrefijoOrdenTrabajo = item.PrefijoOrdenTrabajo,
                            TipoPruebaID = item.TipoPruebaID.GetValueOrDefault(),
                            RequisicionID = item.RequisicionID
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

        public object ObtenerListadoProveedores(int ProyectoID, int TipoPruebaID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<Proveedor> listaProveedores = new List<Proveedor>();
                    //List<Sam3_ST_VR_Get_Proveedores_Result> listaProveedorCTX = ctx.Sam3_ST_VR_Get_Proveedores(ProyectoID, TipoPruebaID).ToList();
                    ////listaProveedores.Add(new Proveedor());
                    //foreach (Sam3_ST_VR_Get_Proveedores_Result item in listaProveedorCTX)
                    //{
                    //    listaProveedores.Add(new Proveedor
                    //    {
                    //        ProveedorID = item.ProveedorID,
                    //        TipoPruebaProveedorID = item.TipoPruebaProveedorID,
                    //        Nombre = item.Proveedor
                    //    });
                    //}
                    return listaProveedores;
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

        public object ObtenerTipoPruebas(int proyectoID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<TiposDePrueba> listatiposPruebas = new List<TiposDePrueba>();
                    List<Sam3_ST_VR_Get_Tipos_Pruebas_Result> listaTipoPruebasCTX = ctx.Sam3_ST_VR_Get_Tipos_Pruebas(proyectoID).ToList();
                    listatiposPruebas.Add(new TiposDePrueba());
                    foreach (Sam3_ST_VR_Get_Tipos_Pruebas_Result item in listaTipoPruebasCTX)
                    {
                        listatiposPruebas.Add(new TiposDePrueba
                        {
                            TipoPruebaID = item.TipoPruebaID,
                            Nombre = item.Nombre
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

        public object ObtenerListadoRequisiciones(int ProyectoID, int ProveedorID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {

                    List<Requisicion> listaRequisiciones = new List<Requisicion>();
                    List<Sam3_ST_VR_Get_Requisiciones_Result> listaRequisicionesCTX = ctx.Sam3_ST_VR_Get_Requisiciones(ProyectoID, ProveedorID).ToList();
                    listaRequisiciones.Add(new Requisicion());
                    foreach (Sam3_ST_VR_Get_Requisiciones_Result item in listaRequisicionesCTX)
                    {
                        listaRequisiciones.Add(new Requisicion
                        {
                            RequisicionID = item.RequisicionID,
                            ProyectoID = item.ProyectoID,
                            TipoPruebaID = item.TipoPruebaID.GetValueOrDefault(),
                            NombreRequisicion = item.NombreRequisicion,
                            FechaRequisicion = item.FechaRequisicion.ToString(),
                            Observacion = item.Observaciones
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

        public object ObtenerEquipos(int tipoPruebaID, int proveedorID, string lenguaje) //EQUIPO
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_ST_Get_Equipo_Result> result = ctx.Sam3_ST_Get_Equipo(tipoPruebaID, proveedorID, lenguaje).ToList();
                    List<Equipo> Equipo = new List<Equipo>();
                    foreach (Sam3_ST_Get_Equipo_Result item in result)
                    {
                        Equipo.Add(new Equipo {
                            EquipoID = item.EquipoID,
                            NombreEquipo = item.Equipo,                                                        
                        });
                    }
                    return Equipo;
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
        public object ObtenerTurnos(int tipoPruebaID, int proveedorID, int equipoID, string lenguaje)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_ST_Get_TurnoLaboral_Result> result = ctx.Sam3_ST_Get_TurnoLaboral(tipoPruebaID, proveedorID, equipoID, lenguaje).ToList();
                    List<TurnoLaboral> Turno = new List<TurnoLaboral>();
                    foreach (Sam3_ST_Get_TurnoLaboral_Result item in result)
                    {
                        Turno.Add(new TurnoLaboral {
                            TurnoLaboralID = item.TurnoLaboralID,
                            Turno = item.Turno
                        });
                    }
                    return Turno;
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



        public object ObtenerRequisicionesDetalle(int proyectoID, int tipoPruebaID, int proveedorID, int requisicionID, int equipoID, int turnoID, string lenguaje)
        {
            try
            {

                using (SamContext ctx = new SamContext())
                {
                    //List<Sam3_ST_ReportesRT_Get_Requisiciones_Detalle_Result> result = ctx.Sam3_ST_ReportesRT_Get_Requisiciones_Detalle(proyectoID, tipoPruebaID, proveedorID, requisicionID, equipoID, turnoID, lenguaje).ToList();
                    
                    List<DetalleCaptura> listaDetalleJunta = new List<DetalleCaptura>();
                    
                
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

        public List<Defectos> ObtenerCatalogoDefectos(int catalogo, string lenguaje)
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

        public List<Resultados> ObtenerReportesRTResultados(string lenguaje)
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


        public List<DetallePorPlacas> ObtenerDetallePorPlacas(int capturaResultadoID, out int? numeroPlacas, string lenguaje, List<Resultados> listaResultados, List<Defectos> listaDefectos)
        {
            List<DetallePorPlacas> listaDetalleResultado = new List<DetallePorPlacas>();
            try
            {

                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_ST_ReportesRT_Get_Resultados_Result> result = ctx.Sam3_ST_ReportesRT_Get_Resultados(capturaResultadoID, lenguaje).ToList();

                    numeroPlacas = result.Count;

                    DetallePorPlacas detalleResultado = null;

                    foreach (Sam3_ST_ReportesRT_Get_Resultados_Result item in result)
                    {
                        List<DetalleResultadosDefectos> listadoDetalleResultadoDefectos = ObtenerReportesRTResultadosDetalle(item.CapturaResultadoPlacaID, listaDefectos, lenguaje);

                        detalleResultado = new DetallePorPlacas
                        {
                            CapturaResultadoID = item.CapturaResultadoID,
                            CapturaResultadoPlacaID = item.CapturaResultadoPlacaID,
                            Ubicacion = item.Ubicacion,
                            ResultadoID = int.Parse(item.ResultadosID.ToString()),
                            Resultado = item.Resultado,
                            ListaDetalleDefectos = listadoDetalleResultadoDefectos,
                            TemplateDetallePorPlaca = (lenguaje == "es-MX" ? "Tienes " : "You have ") + listadoDetalleResultadoDefectos.Count + (lenguaje == "es-MX" ? " Defecto (s)" : " Defects"),
                            Accion = 2,
                            ListaResultados = listaResultados,
                            ListaDefectos = listaDefectos
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

        public List<DetalleResultadosDefectos> ObtenerReportesRTResultadosDetalle(int capturaResultadoID, List<Defectos> listaDefectos, string lenguaje)
        {
            List<DetalleResultadosDefectos> listaDetalleDefectos = new List<DetalleResultadosDefectos>();
            try
            {

                using (SamContext ctx = new SamContext())
                {
                    //List<Sam3_ReportesRT_Get_Resultados_Detalle_Result> result = ctx.Sam3_ReportesRT_Get_Resultados_Detalle(resultadosDefectoID, ordenTrabajoID, spoolID, juntaSpoolID,lenguaje).ToList();
                    DetalleResultadosDefectos detalleDefecto = null;

                    int posicion = 0;
                    //foreach (Sam3_ReportesRT_Get_Resultados_Detalle_Result item in result)
                    //{
                    //    posicion++;
                    //    detalleDefecto = new DetalleResultadosDefectos
                    //    {
                    //        OrdenTrabajoID = item.OrdenTrabajoID,
                    //        SpoolID = item.SpoolID,
                    //        JuntaSpoolID = item.JuntaSpoolID.GetValueOrDefault(),
                    //        DefectoID = item.DefectoID,
                    //        Defecto = item.Defecto,
                    //        InicioMM =item.InicioMM,
                    //        FinMM = item.FinMM,
                    //        Accion = 2,
                    //        Posicion = posicion,//item.Posicion.GetValueOrDefault(),
                    //    };
                    //    listaDetalleDefectos.Add(detalleDefecto);
                    //}

                    return listaDetalleDefectos;
                }

            }
            catch (Exception ex)
            {
                return listaDetalleDefectos;
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
                            Turno = item.Turno
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

        public object ObtenerListadoEquipos(int tipoPruebaID, int proveedorID, string lenguaje)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {

                    List<Equipo> listaEquipos = new List<Equipo>();
                    List<Sam3_ST_CRRT_Get_Equipo_Result> listaEquiposCTX = ctx.Sam3_ST_CRRT_Get_Equipo(tipoPruebaID, proveedorID,lenguaje).ToList();
                    listaEquipos.Add(new Equipo());
                    foreach (Sam3_ST_CRRT_Get_Equipo_Result item in listaEquiposCTX)
                    {
                        listaEquipos.Add(new Equipo
                        {
                            EquipoID = item.EquipoID,
                            NombreEquipo = item.Equipo
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

        public object ObtenerProyectos(int usuarioID)
        {
            try
            {                
                using (SamContext ctx = new SamContext())
                {
                    List<Proyectos> ListaProyecto = new List<Proyectos>();
                    List<Sam3_ST_ReportesRT_Get_ListaProyectos_Result> result = ctx.Sam3_ST_ReportesRT_Get_ListaProyectos(usuarioID).ToList();
                    ListaProyecto.Add(new Proyectos());
                    foreach(Sam3_ST_ReportesRT_Get_ListaProyectos_Result item in result)
                    {
                        ListaProyecto.Add(new Proyectos {
                            ProyectoID = item.ProyectoID,
                            Nombre = item.Nombre,
                            PatioID = item.PatioID,
                            PrefijoOrdenTrabajo = item.PrefijoOrdenTrabajo                            
                        });
                    }

                    return ListaProyecto;
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

        public object ObtenerProveedores(int proyectoID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<Proveedor> listaProveedores = new List<Proveedor>();
                    //List<Sam3_ST_ReporteRT_Get_Proveedores_Result> listaProveedorCTX = ctx.Sam3_ST_CRRT_Get_Proveedores(proyectoID).ToList();
                    //listaProveedores.Add(new Proveedor());
                    //foreach (Sam3_ST_CRRT_Get_Proveedores_Result item in listaProveedorCTX)
                    //{
                    //    listaProveedores.Add(new Proveedor
                    //    {
                    //        ProveedorID = item.ProveedorID,
                    //        Nombre = item.Proveedor
                    //    });
                    //}
                    return listaProveedores;
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

        /*
         public object ObtenerListadoProyectosVR(Sam3_Usuario usuario)
         {
             try
             {
                 using (SamContext ctx = new SamContext())
                 {
                     //List<Proyectos> listaProyectos = new List<Proyectos>();
                     //List<Sam3_ST_VR_Get_ListaProyectos_Result> listaProyectosCTX = ctx.Sam3_ST_VR_Get_ListaProyectos(usuario.UsuarioID).ToList();
                     //listaProyectos.Add(new Proyectos());
                     //foreach (Sam3_ST_VR_Get_ListaProyectos_Result item in listaProyectosCTX)
                     //{
                     //    listaProyectos.Add(new Proyectos
                     //    {
                     //        ProyectoID = item.ProyectoID,
                     //        Nombre = item.Nombre,
                     //        PatioID = item.PatioID,
                     //        PrefijoOrdenTrabajo = item.PrefijoOrdenTrabajo
                     //    });
                     //}

                     return null;
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

         public object ObtenerListadoTiposPruebaVR(String lenguaje)
         {
             try
             {
                 using (SamContext ctx = new SamContext())
                 {
                     //List<TiposDePrueba> listaTiposDePrueba = new List<TiposDePrueba>();
                     //List<Sam3_ST_VR_Get_TiposDePrueba_Result> listaTiposDePruebaCTX = ctx.Sam3_ST_VR_Get_TiposDePrueba(lenguaje).ToList();
                     //listaTiposDePrueba.Add(new TiposDePrueba());
                     //foreach (Sam3_ST_VR_Get_TiposDePrueba_Result item in listaTiposDePruebaCTX)
                     //{
                     //    listaTiposDePrueba.Add(new TiposDePrueba
                     //    {
                     //        TipoPruebaID = item.TipoPruebaID,
                     //        Nombre = item.Nombre,
                     //        Categoria = item.Categoria,
                     //        TipoPruebaPorSpool = item.TipoPruebaPorSpool.GetValueOrDefault(),
                     //        RequiereEquipo = (bool)item.RequiereEquipo
                     //    });
                     //}

                     return null;
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

         public object ObtenerListadoRequisicionesVR(Sam3_Usuario usuario, int proyectoID, int tipoPruebaID, int proveedorID, int estatusID)
         {
             try
             {
                 using (SamContext ctx = new SamContext())
                 {

                     //List<Requisicion> listaRequisiciones = new List<Requisicion>();
                     //List<Sam3_ST_VR_Get_ListaRequisiciones_Result> listaRequisicionesCTX = ctx.Sam3_ST_VR_Get_ListaRequisiciones(usuario.UsuarioID, proyectoID, tipoPruebaID, proveedorID, estatusID).ToList();
                     //listaRequisiciones.Add(new Requisicion());
                     //foreach (Sam3_ST_VR_Get_ListaRequisiciones_Result item in listaRequisicionesCTX)
                     //{
                     //    listaRequisiciones.Add(new Requisicion
                     //    {
                     //        RequisicionID = item.RequisicionID,
                     //        ProyectoID = item.ProyectoID,
                     //        TipoPruebaID = item.TipoPruebaID.GetValueOrDefault(),
                     //        NombreRequisicion = item.NombreRequisicion,
                     //        CodigoAsme = item.CodigoAsme,
                     //        FechaRequisicion = item.FechaRequisicion.ToString(),
                     //        Observacion = item.Observaciones
                     //    });
                     //}

                     return null;
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


         public object ObtenerListadoProveedoresVR(int proyectoID, int tipoPruebaID)
         {
             try
             {
                 using (SamContext ctx = new SamContext())
                 {
                     //List<Proveedor> listaProveedores = new List<Proveedor>();
                     //List<Sam3_ST_VR_Get_Proveedores_Result> listaProveedorCTX = ctx.Sam3_ST_VR_Get_Proveedores(proyectoID, tipoPruebaID).ToList();
                     //listaProveedores.Add(new Proveedor());
                     //foreach (Sam3_ST_VR_Get_Proveedores_Result item in listaProveedorCTX)
                     //{
                     //    listaProveedores.Add(new Proveedor
                     //    {
                     //        ProveedorID = item.ProveedorID,
                     //        TipoPruebaProveedorID = item.TipoPruebaProveedorID,
                     //        Nombre = item.Proveedor
                     //    });
                     //}

                     return null;
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

         public object ObtenerLOGINProveedor(int ProveedorID, int ProyectoID, string NombreProveedor, string Password)
         {
             try
             {
                 using (SamContext ctx = new SamContext())
                 {
                     List<LoginProveedor> listaProveedores = new List<LoginProveedor>();
                     List<Sam3_ST_Get_LOGINProveedores_Result> listaProveedorCTX = ctx.Sam3_ST_Get_LOGINProveedores(ProveedorID, ProyectoID, NombreProveedor, Password).ToList();
                     foreach (Sam3_ST_Get_LOGINProveedores_Result item in listaProveedorCTX)
                     {
                         listaProveedores.Add(new LoginProveedor
                         {
                             ProveedorID = item.ProveedorID,
                             Nombre = item.Nombre
                         });
                     }

                     return listaProveedores;
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

         public object ObtenerListadoElementosVR(int proyectoID, int tipoPruebaID, int proveedorID, int requisicionID, string lenguaje)
         {
             try
             {
                 using (SamContext ctx = new SamContext())
                 {
                     //List<GridElement> listaElementos = new List<GridElement>();
                     //List<Sam3_ST_VR_Get_ListadoElementos_Result> listaElementosCTX = ctx.Sam3_ST_VR_Get_ListadoElementos(proyectoID, tipoPruebaID, proveedorID, requisicionID, lenguaje).ToList();

                     //System.Text.StringBuilder cad = new System.Text.StringBuilder();
                     //cad.Append('[');
                     //int numPlacas = 0;
                     //string detalleTemp = "";
                     //for (int i = 0; i < listaElementosCTX.Count; i++)
                     //{
                     //    detalleTemp = ObtenerReportesRTResultados(listaElementosCTX[i].OrdenTrabajoID, listaElementosCTX[i].SpoolID, listaElementosCTX[i].JuntaSpoolID.GetValueOrDefault(), out numPlacas, lenguaje);
                     //    cad.Append("{\"ReporteRTID\": \""+listaElementosCTX[i].ReporteRTID+"\", \"RequisicionID\":\"" + listaElementosCTX[i].RequisicionID + "\", \"OrdenTrabajoID\":\"" + listaElementosCTX[i].OrdenTrabajoID + "\",\"SpoolID\":\"" + listaElementosCTX[i].SpoolID + "\", \"JuntaSpoolID\":\"" + listaElementosCTX[i].JuntaSpoolID + "\", \"NumeroControl\": \"" + listaElementosCTX[i].NumeroControl + "\", \"Etiqueta\": \"" + listaElementosCTX[i].Etiqueta + "\", \"ClasificacionPND\": \"" + listaElementosCTX[i].ClasificacionPND + "\", \"TipoPrueba\": \"" + listaElementosCTX[i].TipoPrueba + "\", \"Observaciones\": \"" + listaElementosCTX[i].Observaciones + "\", \"CodigoAsme\": \"" + listaElementosCTX[i].CodigoAsme + "\", \"NumeroPlacas\": \"" + numPlacas + "\", \"Tamanomm\": \""+listaElementosCTX[i].Tamano+"\", \"Densidad\": "+listaElementosCTX[i].Densidad+", \"Conciliado\": \""+listaElementosCTX[i].ResultadoConciliacionID+ "\", \"RazonRechazo\": \"" + listaElementosCTX[i].RazonNoConciliacionID+"\", \"InformacionResultados\": [" + detalleTemp + "], \"Accion\": 1, \"Activo\": 1, \"UsuarioModificacion\": 1, \"FechaModificacion\": \"" + DateTime.Now.ToString("yyyy-MM-dd") + "\" }");
                     //    if (i != (listaElementosCTX.Count - 1))
                     //        cad.Append(',');
                     //}
                     //cad.Append(']');

                     //return cad.ToString();

                     //foreach (Sam3_ST_VR_Get_ListadoElementos_Result item in listaElementosCTX)
                     //{
                     //    listaElementos.Add(new GridElement
                     //    {
                     //        NumeroControl = item.NumeroControl,
                     //        Etiqueta = item.Etiqueta,
                     //        NumeroRequisicion = item.NumeroRequisicion,
                     //        ClasificacionPND = item.ClasificacionPND,
                     //        TipoPrueba = item.TipoPrueba,
                     //        Observaciones = item.Observaciones,
                     //        RequisicionID = item.RequisicionID,
                     //        OrdenTrabajoID = item.OrdenTrabajoID,
                     //        SpoolID = item.SpoolID,
                     //        JuntaSpoolID = item.JuntaSpoolID.GetValueOrDefault(),
                     //        ReporteRTID = item.ReporteRTID
                     //    });
                     //}

                     return null;

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

         public string ObtenerReportesRTResultados(int ordenTrabajoID, int spoolID, int juntaSpoolID, out int numeroPlacas,string lenguaje)
         {
             try
             {

                 using (SamContext ctx = new SamContext())
                 {
                     //List<Sam3_ReportesRT_Get_Resultados_Result> result = ctx.Sam3_ReportesRT_Get_Resultados(ordenTrabajoID, spoolID, juntaSpoolID, lenguaje).ToList();

                     //numeroPlacas = result.Count;
                     //System.Text.StringBuilder cad = new System.Text.StringBuilder();
                     ////cad.Append('[');
                     //for (int i = 0; i < result.Count; i++)
                     //{
                     //    cad.Append("{\"ReporteRTResultadosID\": \"" + result[i].ReporteRTResultadosID + "\", \"ReporteRTID\":\"" + result[i].ReporteRTID + "\", \"OrdenTrabajoID\":\"" + result[i].OrdenTrabajoID + "\",\"SpoolID\":\"" + result[i].SpoolID + "\", \"JuntaSpoolID\":\"" + result[i].JuntaSpoolID + "\", \"Ubicacion\": \"" + result[i].Ubicacion + "\", \"Resultado\": \"" + result[i].Resultado + "\", \"DetalleResultados\": [" + ObtenerReportesRTResultadosDetalle(result[i].ReporteRTResultadosID, result[i].OrdenTrabajoID, result[i].SpoolID, result[i].JuntaSpoolID.GetValueOrDefault()) + "] }");
                     //    if (i != (result.Count - 1))
                     //        cad.Append(',');
                     //}
                     //cad.Append(']');
                     numeroPlacas = 0;
                     return null;
                 }

             }
             catch (Exception ex)
             {
                 numeroPlacas = 0;
                 return "";
             }
         }

         public string ObtenerReportesRTResultadosDetalle(int reporteResultadosID, int ordenTrabajoID, int spoolID, int juntaSpoolID)
         {
             try
             {

                 using (SamContext ctx = new SamContext())
                 {
                     //List<Sam3_ReportesRT_Get_Resultados_Detalle_Result> result = ctx.Sam3_ReportesRT_Get_Resultados_Detalle(reporteResultadosID,ordenTrabajoID, spoolID, juntaSpoolID).ToList();

                     //System.Text.StringBuilder cad = new System.Text.StringBuilder();
                     ////cad.Append('[');
                     //for (int i = 0; i < result.Count; i++)
                     //{
                     //    cad.Append("{\"ResultadosDefectoID\": \"" + result[i].ResultadosDefectoID + "\", \"ReporteRTID\":\"" + result[i].ReporteResultadosID + "\", \"OrdenTrabajoID\":\"" + result[i].OrdenTrabajoID + "\",\"SpoolID\":\"" + result[i].SpoolID + "\", \"JuntaSpoolID\":\"" + result[i].JuntaSpoolID + "\", \"DefectoID:\": \"" + result[i].DefectoID + "\", \"InicioMM:\": \"" + result[i].InicioMM + "\", \"FinMM\": \"" + result[i].FinMM + "\" }");
                     //    if (i != (result.Count - 1))
                     //        cad.Append(',');
                     //}
                     ////cad.Append(']');

                     return null;
                 }

             }
             catch (Exception ex)
             {
                 return "";
             }
         }

         public object ObtenerRequisicionesDetalle(int proyectoID, int tipoPruebaID, int proveedorID, int requisicionID, int equipoID, int turnoID, string lenguaje)
         {
             try
             {

                 using (SamContext ctx = new SamContext())
                 {
                     List<Sam3_ST_VR_Get_Requisiciones_Detalle_Result> result = ctx.Sam3_ST_VR_Get_Requisiciones_Detalle(proyectoID, tipoPruebaID, proveedorID, requisicionID, equipoID, turnoID, lenguaje).ToList();

                     int? numPlacas = 0;

                     List<Resultados> listaResultados = ObtenerReportesRTResultados(lenguaje);

                     List<ResultadoConciliacion> listaResultadoConciliacion = ObtenerResultadoConciliacion(lenguaje);
                     List<RazonNoConciliacion> listaRazonNoConciliacion = ObtenerRazonNoConciliacion(lenguaje, tipoPruebaID);

                     List<DetalleCapturaRT> listaDetalleJunta = new List<DetalleCapturaRT>();
                     DetalleCapturaRT detalle = null;
                     foreach (Sam3_ST_VR_Get_Requisiciones_Detalle_Result item in result)
                     {
                         numPlacas = 0;

                         List<Defectos> listaDefectos = ObtenerCatalogoDefectos(item.JuntaSpoolID == null ? 0 : 1, lenguaje);//por junta

                         List<DetallePorPlacas> listaDetallePorPlacas = ObtenerDetallePorPlacas(item.OrdenTrabajoID, item.SpoolID.GetValueOrDefault(), item.JuntaSpoolID.GetValueOrDefault(), out numPlacas, lenguaje, listaResultados, listaDefectos);

                         numPlacas = (numPlacas == 0 ? null : numPlacas);



                         detalle = new DetalleCapturaRT
                         {
                             OrdenTrabajoID = item.OrdenTrabajoID,
                             NumeroControl = item.NumeroControl,
                             SpoolID = item.SpoolID.GetValueOrDefault(),
                             JuntaSpoolID = item.JuntaSpoolID.GetValueOrDefault(),
                             Junta = int.Parse(item.Etiqueta.ToString()),
                             ClasificacionPND = item.ClasificacionPND,
                             TipoPrueba = item.TipoPrueba,
                             Observaciones = item.Observaciones,
                             NumeroPlacas = numPlacas,
                             Tamano = item.Tamano,
                             Densidad = item.Densidad,
                             ResultadoConciliacionID = item.ResultadoConciliacionID,
                             ResultadoConciliacion = item.ResultadoConciliacion,
                             ListaResultadoConciliacion = listaResultadoConciliacion,
                             RazonNoConciliacionID = item.RazonNoConciliacionID,
                             RazonNoConciliacion = item.RazonNoConciliacion,
                             ListaRazonNoConciliacion = listaRazonNoConciliacion,
                             ListaDetallePorPlacas = listaDetallePorPlacas,
                             Accion = item.ReporteRTID == null ? 1 : 2,//falta negocio.
                             ListaResultados = listaResultados,
                             ListaDefectos = listaDefectos,
                             TemplateDetalleElemento = lenguaje == "es-MX" ? "Ver detalle" : " View detail ",
                             EstatusRequisicion = item.Estatus.GetValueOrDefault(),
                             RequisicionID = item.RequisicionID,
                             Comentarios = item.Comentarios,
                             ReporteRTID = item.ReporteRTID
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

         public List<Resultados> ObtenerReportesRTResultados(string lenguaje)
         {
             List<Resultados> listaResultados = new List<Resultados>();

             try
             {

                 using (SamContext ctx = new SamContext())
                 {
                     List<Sam3_ReportesRT_ST_CRRT_Get_Resultado_Result> result = ctx.Sam3_ReportesRT_ST_CRRT_Get_Resultado(lenguaje).ToList();

                     Resultados resultado = null;
                     listaResultados.Add(new Resultados());
                     foreach (Sam3_ReportesRT_ST_CRRT_Get_Resultado_Result item in result)
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


         public List<DetallePorPlacas> ObtenerDetallePorPlacas(int ordenTrabajoID, int spoolID, int juntaSpoolID, out int? numeroPlacas, string lenguaje, List<Resultados> listaResultados, List<Defectos> listaDefectos)
         {
             List<DetallePorPlacas> listaDetalleResultado = new List<DetallePorPlacas>();
             try
             {

                 using (SamContext ctx = new SamContext())
                 {
                     List<Sam3_ReportesRT_Get_Resultados_Result> result = ctx.Sam3_ReportesRT_Get_Resultados(ordenTrabajoID, spoolID, juntaSpoolID, lenguaje).ToList();

                     numeroPlacas = result.Count;

                     DetallePorPlacas detalleResultado = null;

                     foreach (Sam3_ReportesRT_Get_Resultados_Result item in result)
                     {
                         List<DetalleResultadosDefectos> listadoDetalleResultadoDefectos = ObtenerReportesRTResultadosDetalle(item.ResultadosID, item.OrdenTrabajoID, item.SpoolID, item.JuntaSpoolID.GetValueOrDefault(), listaDefectos, lenguaje);

                         detalleResultado = new DetallePorPlacas
                         {
                             OrdenTrabajoID = item.OrdenTrabajoID,
                             SpoolID = item.SpoolID,
                             JuntaSpoolID = item.JuntaSpoolID.GetValueOrDefault(),
                             Ubicacion = item.Ubicacion,
                             ResultadoID = int.Parse(item.ResultadosID.ToString()),
                             Resultado = item.Resultado,
                             ListaDetalleDefectos = listadoDetalleResultadoDefectos,
                             TemplateDetallePorPlaca = (lenguaje == "es-MX" ? "Tienes " : "You have ") + listadoDetalleResultadoDefectos.Count + (lenguaje == "es-MX" ? " Defecto (s)" : " Defects"),
                             Accion = 2,
                             ListaResultados = listaResultados,
                             ListaDefectos = listaDefectos
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

         public List<DetalleResultadosDefectos> ObtenerReportesRTResultadosDetalle(int resultadosDefectoID, int ordenTrabajoID, int spoolID, int juntaSpoolID, List<Defectos> listaDefectos, string lenguaje)
         {
             List<DetalleResultadosDefectos> listaDetalleDefectos = new List<DetalleResultadosDefectos>();
             try
             {

                 using (SamContext ctx = new SamContext())
                 {
                     //List<Sam3_ReportesRT_Get_Resultados_Detalle_Result> result = ctx.Sam3_ReportesRT_Get_Resultados_Detalle(resultadosDefectoID, ordenTrabajoID, spoolID, juntaSpoolID, lenguaje).ToList();
                     DetalleResultadosDefectos detalleDefecto = null;

                     int posicion = 0;
                     //foreach (Sam3_ReportesRT_Get_Resultados_Detalle_Result item in result)
                     //{
                     //    posicion++;
                     //    detalleDefecto = new DetalleResultadosDefectos
                     //    {
                     //        OrdenTrabajoID = item.OrdenTrabajoID,
                     //        SpoolID = item.SpoolID,
                     //        JuntaSpoolID = item.JuntaSpoolID.GetValueOrDefault(),
                     //        DefectoID = item.DefectoID,
                     //        Defecto = item.Defecto,
                     //        InicioMM = item.InicioMM,
                     //        FinMM = item.FinMM,
                     //        Accion = 2,
                     //        Posicion = posicion,//item.Posicion.GetValueOrDefault(),
                     //    };
                     //    listaDetalleDefectos.Add(detalleDefecto);
                     //}

                     return listaDetalleDefectos;
                 }

             }
             catch (Exception ex)
             {
                 return listaDetalleDefectos;
             }
         }

         public List<Defectos> ObtenerCatalogoDefectos(int catalogo, string lenguaje)
         {
             List<Defectos> listaDefectos = new List<Defectos>();
             try
             {

                 using (SamContext ctx = new SamContext())
                 {
                     List<Sam3_ReportesRT_ST_CRRT_Get_CatalogoDefectos_Result> result = ctx.Sam3_ReportesRT_ST_CRRT_Get_CatalogoDefectos(catalogo, lenguaje).ToList();

                     Defectos defecto = null;
                     listaDefectos.Add(new Defectos());
                     foreach (Sam3_ReportesRT_ST_CRRT_Get_CatalogoDefectos_Result item in result)
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

         public List<ResultadoConciliacion> ObtenerResultadoConciliacion(string lenguaje)
         {
             List<ResultadoConciliacion> listaResultados = new List<ResultadoConciliacion>();

             try
             {

                 using (SamContext ctx = new SamContext())
                 {
                     List<Sam3_ST_VR_Get_ListadoResultadoConciliaciones_Result> result = ctx.Sam3_ST_VR_Get_ListadoResultadoConciliaciones(lenguaje).ToList();

                     ResultadoConciliacion resultado = null;
                     listaResultados.Add(new ResultadoConciliacion());
                     foreach (Sam3_ST_VR_Get_ListadoResultadoConciliaciones_Result item in result)
                     {
                         resultado = new ResultadoConciliacion
                         {
                             ResultadoConciliacionID = item.ResultadoConciliacionID,
                             Descripcion = item.Resultado
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

         public List<RazonNoConciliacion> ObtenerRazonNoConciliacion(string lenguaje, int tipoPruebaID)
         {
             List<RazonNoConciliacion> listaResultados = new List<RazonNoConciliacion>();

             try
             {

                 using (SamContext ctx = new SamContext())
                 {
                     List<Sam3_ST_VR_Get_ListadoRazonesNoConciliacion_Result> result = ctx.Sam3_ST_VR_Get_ListadoRazonesNoConciliacion(lenguaje, tipoPruebaID).ToList();

                     RazonNoConciliacion resultado = null;
                     listaResultados.Add(new RazonNoConciliacion());
                     foreach (Sam3_ST_VR_Get_ListadoRazonesNoConciliacion_Result item in result)
                     {
                         resultado = new RazonNoConciliacion
                         {
                             RazonNoConciliacionID = item.RazonNoConciliacionID,
                             Descripcion = item.Descripcion
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

         public object ActualizaCapturaReportesRT(DataTable dtDetalleCaptura, int usuario, string lenguaje)
         {
             try
             {
                 using (SamContext ctx = new SamContext())
                 {
                     ObjetosSQL _SQL = new ObjetosSQL();
                     string[,] parametro = { { "@UsuarioID", usuario.ToString() }, { "@lenguaje", lenguaje } };
                     _SQL.Ejecuta(Stords.GUARDARCAPTURAREPORTEVALIDACION, dtDetalleCaptura, "@ReporteRT", parametro);
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
         }*/
    }
}