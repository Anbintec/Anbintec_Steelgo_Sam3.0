using DatabaseManager.Constantes;
using DatabaseManager.Sam3;
using SecurityManager.Api.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using static BackEndSAM.Models.ServiciosTecnicos.EditarRequisicion.EditarRequisicion;

namespace BackEndSAM.DataAcces.ServiciosTecnicos.EditarRequisicion
{
    public class EditarRequisicionBD
    {
        private static readonly object _mutex = new object();
        private static EditarRequisicionBD _instance;

        public static EditarRequisicionBD Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new EditarRequisicionBD();
                    }
                }
                return _instance;
            }
        }

        public object ObtenerListadoElementos(string lenguaje, int RequisicionID, int TipoPruebaID, int ProyectoID, int Muestra)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<ElementosPorClasificacion> listaElementos = new List<ElementosPorClasificacion>();
                    List<Sam3_ST_Get_ElementosPorPrueba_Result> listaElementosCTX = ctx.Sam3_ST_Get_ElementosPorPrueba(lenguaje, ProyectoID, TipoPruebaID, RequisicionID, Muestra).ToList();
                                                                                        
                    foreach (Sam3_ST_Get_ElementosPorPrueba_Result item in listaElementosCTX)
                    {
                        listaElementos.Add(new ElementosPorClasificacion
                        {
                            Accion = item.RequisicionID == 0 ? 1 : 2,
                            NumeroControl = item.NumeroControl,
                            EtiquetaJunta = item.EtiquetaJunta,
                            TipoJunta = item.TipoJunta,
                            NombreRequisicion = item.NombreRequisicion,
                            Cuadrante = item.Cuadrante,
                            Prioridad = item.Prioridad.GetValueOrDefault(),
                            Clasificacion = item.Clasificacion,
                            Diametro = item.DiametroPlano.GetValueOrDefault(),
                            Espesor = item.Espesor.GetValueOrDefault(),
                            Cedula = item.Cedula,                           
                            ElementoPorClasificacionPNDID = item.ElementoPorClasificacionPNDID,
                            RequisicionID = item.RequisicionID,
                            ProyectoID = item.ProyectoID,
                            SpoolID = item.SpoolID.GetValueOrDefault(),
                            JuntaSpoolID = item.JuntaSpoolID.GetValueOrDefault(),
                            OrdenTrabajoSpoolID = item.OrdenTrabajoSpoolID,
                            TipoPruebaID = item.TipoPruebaID.GetValueOrDefault(),
                            Especificacion = item.Especificacion,
                            Agregar = item.RequisicionID > 0 ? true : false,
                            EstatusCaptura = 0
                            //Disposicion = item.Disposicion,                            
                            //ClasificacionPNDID = item.ClasificacionPNDID,
                            //OrdenTrabajoID = item.OrdenTrabajoID
                        });
                    }

                    return listaElementos;
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

        public object ObtieneElementosRequisicion(int TipoPruebaID, int usuarioID, int RequisicionID, string lenguaje, int Muestra, int ProyectoID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<ElementosPorClasificacion> listaElementos = new List<ElementosPorClasificacion>();
                    List<Sam3_ST_Get_ElementosPorPrueba_Result> listaElementosCTX = ctx.Sam3_ST_Get_ElementosPorPrueba(lenguaje, ProyectoID, TipoPruebaID, RequisicionID, Muestra).ToList();

                    foreach (Sam3_ST_Get_ElementosPorPrueba_Result item in listaElementosCTX)
                    {
                        listaElementos.Add(new ElementosPorClasificacion
                        {
                            NumeroControl = item.NumeroControl,
                            EtiquetaJunta = item.EtiquetaJunta,
                            TipoJunta = item.TipoJunta,
                            NombreRequisicion = item.NombreRequisicion,
                            Cuadrante = item.Cuadrante,
                            Prioridad = item.Prioridad.GetValueOrDefault(),
                            Clasificacion = item.Clasificacion,
                            Diametro = item.DiametroPlano.GetValueOrDefault(),
                            Espesor = item.Espesor.GetValueOrDefault(),
                            Cedula = item.Cedula,
                            Codigo = item.Codigo,

                            ElementoPorClasificacionPNDID = item.ElementoPorClasificacionPNDID,
                            Agregar = item.RequisicionID > 0 ? true : false,
                            RequisicionID = item.RequisicionID,
                            ProyectoID = item.ProyectoID,
                            SpoolID = item.SpoolID.GetValueOrDefault(),
                            JuntaSpoolID = item.JuntaSpoolID.GetValueOrDefault(),
                            OrdenTrabajoSpoolID = item.OrdenTrabajoSpoolID,
                            TipoPruebaID = item.TipoPruebaID.GetValueOrDefault(),
                            Especificacion = item.Especificacion,
                            //Disposicion = item.Disposicion,
                            //ClasificacionPNDID = item.ClasificacionPNDID,
                            //OrdenTrabajoID = item.OrdenTrabajoID
                        });
                    }

                    return listaElementos;
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

        public object ObtieneSpools(int UsuarioID, string IdOrdenTrabajo, int OrdenTrabajoSpoolID, int TipoPruebaID, int ProyectoID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<ElementosPorClasificacion> listaElementos = new List<ElementosPorClasificacion>();
                    List<Sam3_ST_Get_ElementoManual_Result> listaElementosCTX = ctx.Sam3_ST_Get_ElementoManual(TipoPruebaID, IdOrdenTrabajo, OrdenTrabajoSpoolID, ProyectoID, UsuarioID).ToList();

                    foreach (Sam3_ST_Get_ElementoManual_Result item in listaElementosCTX)
                    {
                        listaElementos.Add(new ElementosPorClasificacion
                        {
                            Accion = 1,
                            NumeroControl = item.NumeroControl,
                            EtiquetaJunta = item.EtiquetaJunta,
                            TipoJunta = item.TipoJunta,
                            NombreRequisicion = item.NombreRequisicion,
                            Cuadrante = item.Cuadrante,
                            Prioridad = item.Prioridad.GetValueOrDefault(),
                            Clasificacion = item.Clasificacion,
                            Diametro = item.DiametroPlano.GetValueOrDefault(),
                            Espesor = item.Espesor,
                            Cedula = item.Cedula,
                            ElementoPorClasificacionPNDID = item.ElementoPorClasificacionPNDID,
                            Agregar = item.RequisicionID > 0 ? true : false,
                            RequisicionID = item.RequisicionID,
                            ProyectoID = item.ProyectoID,
                            SpoolID = item.SpoolID,
                            JuntaSpoolID = item.JuntaSpoolID.GetValueOrDefault(),
                            OrdenTrabajoSpoolID = item.OrdenTrabajoSpoolID,
                            TipoPruebaID = item.TipoPruebaID.GetValueOrDefault(),
                            Especificacion = item.Especificacion,
                            EstatusCaptura = 1

                        });
                    }
                    return listaElementos;
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

        public object ObtenerJuntasXSpoolID(Sam3_Usuario usuario, string ordenTrabajo, string id, int sinCaptura)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_ST_Get_JuntaSpool_Result> lista = ctx.Sam3_ST_Get_JuntaSpool(int.Parse(id)).ToList();
                    List<JuntaSpool> Juntas = new List<JuntaSpool>();
                    foreach(Sam3_ST_Get_JuntaSpool_Result item in lista)
                    {
                        Juntas.Add(new JuntaSpool
                        {
                            JuntaSpoolID = item.JuntaSpoolID,
                            Etiqueta = item.Etiqueta
                        });
                    }
                    return Juntas;
                    //return lista.OrderBy(x => int.Parse(x.Etiqueta)).ToList<Sam3_ST_Get_JuntaSpool_Result>();

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

        public object ObtienedetalleJunta(int UsuarioID, string IdOrdenTrabajo, int OrdenTrabajoSpoolID, int TipoPruebaID, int ProyectoID, int JuntaSpoolID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<ElementosPorClasificacion> listaElementos = new List<ElementosPorClasificacion>();
                    List<Sam3_ST_Get_ElementoManualXJunta_Result> listaElementosCTX = ctx.Sam3_ST_Get_ElementoManualXJunta(TipoPruebaID, IdOrdenTrabajo, OrdenTrabajoSpoolID, ProyectoID, UsuarioID, JuntaSpoolID).ToList();

                    foreach (Sam3_ST_Get_ElementoManualXJunta_Result item in listaElementosCTX)
                    {
                        listaElementos.Add(new ElementosPorClasificacion
                        {
                            Accion = 1,
                            NumeroControl = item.NumeroControl,
                            EtiquetaJunta = item.EtiquetaJunta,
                            TipoJunta = item.TipoJunta,
                            NombreRequisicion = item.NombreRequisicion,
                            Cuadrante = item.Cuadrante,
                            Prioridad = item.Prioridad.GetValueOrDefault(),
                            Clasificacion = item.Clasificacion,
                            Diametro = item.DiametroPlano.GetValueOrDefault(),
                            Espesor = item.Espesor,
                            Cedula = item.Cedula,
                            ElementoPorClasificacionPNDID = item.ElementoPorClasificacionPNDID,
                            Agregar = item.RequisicionID > 0 ? true : false,
                            RequisicionID = item.RequisicionID,
                            ProyectoID = item.ProyectoID,
                            SpoolID = item.SpoolID,
                            JuntaSpoolID = item.JuntaSpoolID.GetValueOrDefault(),
                            OrdenTrabajoSpoolID = item.OrdenTrabajoSpoolID,
                            TipoPruebaID = item.TipoPruebaID.GetValueOrDefault(),
                            Especificacion = item.Especificacion,
                            EstatusCaptura = 1

                        });
                    }
                    return listaElementos;
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

        public object InsertarNuevaRequisicion(DataTable dtDetalleRequisicion, int RequisicionID, string NombreRequisicion, int ProyectoID, int TipoPruebaID, string FechaRequisicion, string CodigoAsme, string Observacion, Sam3_Usuario usuario, string lenguaje)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    
                    ObjetosSQL _SQL = new ObjetosSQL();
                    string[,] parametro = {                         
                        { "@RequisicionID", RequisicionID.ToString() },
                        { "@NombreRequisicion", NombreRequisicion },
                        { "@ProyectoID", ProyectoID.ToString() },
                        { "@TipoPruebaID", TipoPruebaID.ToString() },
                        { "@FechaRequisicion", FechaRequisicion },                        
                        { "@Observacion", Observacion == null ? "":Observacion },
                        { "@UsuarioID", usuario.UsuarioID.ToString() },
                        { "@Lenguaje", lenguaje }};

                    int identityResult = _SQL.EjecutaInsertUpdate(Stords.GUARDARNUEVAREQUISICION, dtDetalleRequisicion, "@TTRequisicion", parametro);

                    TransactionalInformation result = new TransactionalInformation();
                    result.ReturnMessage.Add("OK");

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

        public object EliminarRequisicion(int requisicionID, int usuarioID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    ObjetosSQL _SQL = new ObjetosSQL();
                    string[,] parametro = { { "@RequisicionID", requisicionID.ToString() }, 
                        { "@UsuarioID", usuarioID.ToString() }};

                    _SQL.Ejecuta(Stords.ELIMINAREQUISICIONPND, parametro);

                    TransactionalInformation result = new TransactionalInformation();
                    result.ReturnMessage.Add("OK");

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
    }
}