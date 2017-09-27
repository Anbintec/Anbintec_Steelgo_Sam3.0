using BackEndSAM.Models.ServiciosTecnicos.RequisicionPND;
using DatabaseManager.Constantes;
using DatabaseManager.Sam3;
using SecurityManager.Api.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;

namespace BackEndSAM.DataAcces.ServiciosTecnicos.GenerarRequisicion
{
    public class RequisicionPNDDB
    {
        private static readonly object _mutex = new object();
        private static RequisicionPNDDB _instance;

        public static RequisicionPNDDB Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new RequisicionPNDDB();
                    }
                }
                return _instance;
            }
        }

        public object ObtenerValorFecha(Sam3_Usuario usuario, string lenguaje, int idCampoPredeterminado)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    var oMyString = new ObjectParameter("Retorna", typeof(string));
                    var result = ctx.Sam3_Steelgo_Get_CampoPredeterminado(idCampoPredeterminado, lenguaje, oMyString);
                    var data = oMyString.Value.ToString();

                    return data;
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

        public object ObtenerListadoElementos(string lenguaje, int RequisicionID, int TipoPruebaID, int ProyectoID, int Muestra)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<ElementosPorClasificacion> listaElementos = new List<ElementosPorClasificacion>();
                    if(TipoPruebaID == 0)
                    {
                        return listaElementos;
                    }
                    List<Sam3_ST_Get_ElementosPorPrueba_Result> listaElementosCTX = ctx.Sam3_ST_Get_ElementosPorPrueba(lenguaje, ProyectoID, TipoPruebaID, RequisicionID, Muestra).ToList();
                    
                    foreach(Sam3_ST_Get_ElementosPorPrueba_Result item in listaElementosCTX)
                    {
                        listaElementos.Add(new ElementosPorClasificacion
                        {
                            Accion = item.RequisicionID == 0 ? 1: 2,
                            NumeroControl = item.NumeroControl,
                            EtiquetaJunta = item.EtiquetaJunta,
                            TipoJunta = item.TipoJunta,
                            NombreRequisicion = item.NombreRequisicion,
                            Cuadrante = item.Cuadrante,
                            Prioridad = item.Prioridad.GetValueOrDefault(),
                            Clasificacion = item.Clasificacion,
                            DiametroPlano = item.DiametroPlano.GetValueOrDefault(),
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
                            ClasificacionPNDID = item.ClasificacionPNDID,
                            OrdenTrabajoID = item.OrdenTrabajoID,
                            
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

        public object InsertarNuevaRequisicion(DataTable dtDetalleRequisicion, int RequisicionID, string NombreRequisicion, int ProyectoID, int TipoPruebaID, string FechaRequisicion, string folioCliente, string Observacion, string lenguaje, Sam3_Usuario usuario)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {

                    //ctx.Sam3_Armado_JuntaArmado()
                    ObjetosSQL _SQL = new ObjetosSQL();
                    string[,] parametro = {
                        { "@RequisicionID", RequisicionID.ToString() },
                        { "@NombreRequisicion", NombreRequisicion },
                        { "@FolioCliente", folioCliente },
                        { "@ProyectoID", ProyectoID.ToString() },
                        { "@TipoPruebaID", TipoPruebaID.ToString() },
                        { "@FechaRequisicion",FechaRequisicion},
                        { "@Observacion", Observacion == null ? "" : Observacion },
                        { "@Lenguaje", lenguaje },
                        { "@UsuarioID", usuario.UsuarioID.ToString() }};

                    int identityResult = _SQL.EjecutaInsertUpdate(Stords.GUARDARNUEVAREQUISICION, dtDetalleRequisicion, "@TTRequisicion", parametro);

                    TransactionalInformation result = new TransactionalInformation();
                    result.ReturnMessage.Add("Ok");

                    if (identityResult > 0)
                        result.ReturnMessage.Add(identityResult.ToString());

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

        public object ObtieneSpools(int UsuarioID,string IdOrdenTrabajo,int OrdenTrabajoSpoolID,int TipoPruebaID,int ProyectoID)
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
                            DiametroPlano = item.DiametroPlano.GetValueOrDefault(),
                            Espesor = item.Espesor,
                            Cedula = item.Cedula,

                            ElementoPorClasificacionPNDID = item.ElementoPorClasificacionPNDID,
                            Agregar =  true ,
                            RequisicionID = item.RequisicionID,
                            ProyectoID = item.ProyectoID,
                            SpoolID = item.SpoolID,
                            JuntaSpoolID = item.JuntaSpoolID.GetValueOrDefault(),
                            OrdenTrabajoSpoolID = item.OrdenTrabajoSpoolID,
                            TipoPruebaID = item.TipoPruebaID.GetValueOrDefault(),
                            Especificacion = item.Especificacion,
                            ClasificacionPNDID=item.ClasificacionPNDID,
                            OrdenTrabajoID=item.OrdenTrabajoID,
                            ClasificacionManual = 1,
                           

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
                    Sam3_ST_Get_JuntaSpool_Result emptyItem = new Sam3_ST_Get_JuntaSpool_Result();
                    emptyItem.JuntaSpoolID = 0;
                    emptyItem.Etiqueta = "";

                    List<Sam3_ST_Get_JuntaSpool_Result> lista = ctx.Sam3_ST_Get_JuntaSpool( int.Parse(id)).ToList();
                    lista.OrderBy(x => int.Parse(x.Etiqueta)).ToList<Sam3_ST_Get_JuntaSpool_Result>();
                    lista.Insert(0, emptyItem );

                    return lista;
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

        public object ObtienedetalleJunta(int UsuarioID, string IdOrdenTrabajo, int OrdenTrabajoSpoolID, int TipoPruebaID, int ProyectoID,int JuntaSpoolID)
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
                            DiametroPlano = item.DiametroPlano.GetValueOrDefault(),
                            Espesor = item.Espesor,
                            Cedula = item.Cedula,

                            ElementoPorClasificacionPNDID = item.ElementoPorClasificacionPNDID,
                            Agregar =  true ,
                            RequisicionID = item.RequisicionID,
                            ProyectoID = item.ProyectoID,
                            SpoolID = item.SpoolID,
                            JuntaSpoolID = item.JuntaSpoolID.GetValueOrDefault(),
                            OrdenTrabajoSpoolID = item.OrdenTrabajoSpoolID,
                            TipoPruebaID = item.TipoPruebaID.GetValueOrDefault(),
                            Especificacion = item.Especificacion,
                            ClasificacionPNDID = item.ClasificacionPNDID,
                            OrdenTrabajoID = item.OrdenTrabajoID,
                            ClasificacionManual = 1
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


    }
}