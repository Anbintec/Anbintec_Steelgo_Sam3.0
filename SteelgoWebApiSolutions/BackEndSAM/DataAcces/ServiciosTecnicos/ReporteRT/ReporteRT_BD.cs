
using BackEndSAM.Controllers.ServiciosTecnicos.ReporteRT;
using BackEndSAM.Models.ServiciosTecnicos.ReporteRT;
using DatabaseManager.Constantes;
using DatabaseManager.Sam3;
using SecurityManager.Api.Models;
using SecurityManager.TokenHandler;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace BackEndSAM.DataAcces.ServiciosTecnicos.ReporteRT
{
    public class ReporteRT_BD
    {
        private static readonly object _mutex = new Object();
        private static ReporteRT_BD _instance;

        //Constructor
        public static ReporteRT_BD Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new ReporteRT_BD();
                    }
                }
                return _instance;
            }
        }

        public object ObtenerListadoProyectos(int usuarioID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_ST_ReportesRT_Get_ListaProyectos_Result> result = ctx.Sam3_ST_ReportesRT_Get_ListaProyectos(usuarioID).ToList();


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

        public object ObtenerListadoProveedores(int proyectoID,int tipoPruebaID, int patioID)
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
                            Nombre = item.Proveedor
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

        public object ObtenerRequisiciones(int proyectoID, int proveedorID)
        {
            try
            {

                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_ST_ReportesRT_Get_Requisiciones_Result> result = ctx.Sam3_ST_ReportesRT_Get_Requisiciones(proyectoID, proveedorID).ToList();


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

        public object ObtenerRequisicionesDetalle(int proyectoID, int tipoPruebaID, int proveedorID, int requisicionID, int equipoID, int turnoID, string lenguaje)
        {
            try
            {

                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_ST_ReportesRT_Get_Requisiciones_Detalle_Result> result = ctx.Sam3_ST_ReportesRT_Get_Requisiciones_Detalle(proyectoID, tipoPruebaID, proveedorID, requisicionID, equipoID, turnoID, lenguaje).ToList();

                    int? numPlacas = 0;

                    List<Resultados> listaResultados = ObtenerReportesRTResultados(lenguaje);
                    
                    
                    List<DetalleCaptura> listaDetalleJunta = new List<DetalleCaptura>();
                    DetalleCaptura detalle = null;
                    foreach (Sam3_ST_ReportesRT_Get_Requisiciones_Detalle_Result item in result)
                    {
                        numPlacas = 0;

                        List<Defectos> listaDefectos = ObtenerCatalogoDefectos(item.JuntaSpoolID == null ? 0 : 1, lenguaje);//por junta

                        List<DetallePorPlacas> listaDetallePorPlacas = ObtenerDetallePorPlacas(item.OrdenTrabajoID, item.SpoolID.GetValueOrDefault(), item.JuntaSpoolID.GetValueOrDefault(), out numPlacas, lenguaje, listaResultados, listaDefectos);

                        numPlacas = (numPlacas == 0 ? null : numPlacas);



                        detalle = new DetalleCaptura
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
                            ResultadoConciliacion = item.ResultadoConciliacion,
                            RazonNoConciliacion = item.RazonNoConciliacion,
                            ListaDetallePorPlacas = listaDetallePorPlacas,
                            Accion = item.ReporteRTID == null ?1:2,//falta negocio.
                            ListaResultados = listaResultados,
                            ListaDefectos = listaDefectos,
                            TemplateDetalleElemento = lenguaje=="es-MX"? "Ver detalle" :" View detail ",
                            EstatusRequisicion = item.Estatus.GetValueOrDefault(),
                            RequisicionID = item.RequisicionID,
                            EquipoID = item.EquipoID,
                            Equipo = item.Equipo,
                            Turno = item.TurnoLaboral,
                            TurnoLaboralID = item.TurnoLaboralID.GetValueOrDefault(),
                            EsSector = true
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


        public List<DetallePorPlacas> ObtenerDetallePorPlacas(int ordenTrabajoID, int spoolID, int juntaSpoolID, out int? numeroPlacas, string lenguaje, List<Resultados> listaResultados, List<Defectos> listaDefectos)
        {
            List<DetallePorPlacas> listaDetalleResultado = new List<DetallePorPlacas>();
            try
            {

                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_ST_ReportesRT_Get_Resultados_Result> result = ctx.Sam3_ST_ReportesRT_Get_Resultados(ordenTrabajoID, spoolID, juntaSpoolID, lenguaje).ToList();

                    numeroPlacas = result.Count;

                    DetallePorPlacas detalleResultado = null;

                    foreach (Sam3_ST_ReportesRT_Get_Resultados_Result item in result)
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
                            TemplateDetallePorPlaca = lenguaje== "es-MX" ?"Detalle": "Detail",
                            Accion=2,
                            ListaResultados= listaResultados,
                            ListaDefectos= listaDefectos
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

        public List<DetalleResultadosDefectos> ObtenerReportesRTResultadosDetalle(int resultadosDefectoID, int ordenTrabajoID, int spoolID, int juntaSpoolID, List<Defectos> listaDefectos,string lenguaje)
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

        public object ObtenerEquipos(int tipoPruebaID, int proveedorID, string lenguaje)
        {
            try
            {

                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_ST_Get_Equipo_Result> result = ctx.Sam3_ST_Get_Equipo(tipoPruebaID, proveedorID, lenguaje).ToList();


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

        public object ObtenerTurnos(int tipoPruebaID, int proveedorID, int equipoID, string lenguaje)
        {
            try
            {

                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_ST_Get_TurnoLaboral_Result> result = ctx.Sam3_ST_Get_TurnoLaboral(tipoPruebaID, proveedorID, equipoID, lenguaje).ToList();


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

        public object InsertarCapturaReportesRT(DataTable dtDetalleCaptura, DataTable dtDetalleResultados, DataTable dtDetalleDefectos, int usuario, string lenguaje)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    ObjetosSQL _SQL = new ObjetosSQL();
                    string[,] parametro = { { "@UsuarioID", usuario.ToString() }, { "@lenguaje", lenguaje } };
                    _SQL.Ejecuta(Stords.GUARDARCAPTURAREPORTE, dtDetalleCaptura, "@ReporteRT", dtDetalleResultados, "@ReporteRTResultados", dtDetalleDefectos, "@ReporteRTResultadosDefectos", parametro);
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

        /*
        public bool Ejecuta(string Stord, DataTable TablaSube, String NombreTabla, string[,] Parametros = null)
        {
            using (SqlCommand cmd = new SqlCommand(Stord, Conexion()))
            {
                if (Parametros != null)
                    for (int i = Numeros.CERO; i < Parametros.Length / Numeros.DOS; i++)
                        cmd.Parameters.AddWithValue(Parametros[i, Numeros.CERO].ToString(), Parametros[i, Numeros.UNO].ToString());
                cmd.Parameters.Add(new SqlParameter(NombreTabla, TablaSube));
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cmd.CommandTimeout = 0;
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    cmd.Connection.Close();
                    return true;
                }
                catch (Exception e)
                {
                    cmd.Connection.Close();
                    throw new Exception(e.Message);
                }
            }
        }
        */




    }
}