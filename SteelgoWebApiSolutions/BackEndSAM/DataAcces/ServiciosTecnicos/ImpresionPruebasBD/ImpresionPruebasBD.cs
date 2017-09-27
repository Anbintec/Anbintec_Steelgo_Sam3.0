using BackEndSAM.Models.ServiciosTecnicos.impresionPruebas;
using DatabaseManager.Constantes;
using DatabaseManager.Sam3;
using SecurityManager.Api.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace BackEndSAM.DataAcces.ServiciosTecnicos.ImpresionPruebasBD
{
    public class ImpresionPruebasBD
    {
        private static readonly object _mutex = new object();
        private static ImpresionPruebasBD _instance;

        public static ImpresionPruebasBD Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new ImpresionPruebasBD();
                    }
                }
                return _instance;
            }
        }

        public object ObtenerImpresionPruebas(string mostrar, int RequisicionID, int ProveedorID)
        {
            try
            {
                List<ImpresionPruebasDetalle> listaDetalleImpresionPruebas = new List<ImpresionPruebasDetalle>();
                using (SamContext ctx = new SamContext())
                {

                    List<Sam3_ST_Get_ImpresionPruebasDetalle_Result> result = ctx.Sam3_ST_Get_ImpresionPruebasDetalle(RequisicionID,ProveedorID).ToList();
                    foreach (var item in result)
                    {
                        listaDetalleImpresionPruebas.Add(new ImpresionPruebasDetalle
                        {
                            Requisicion = item.NombreRequisicion,
                            Spool = item.NumeroControl,
                            Junta = item.EtiquetaJunta,
                            Clasificacion = item.Clasificacion,
                            Diametro = item.Diametro,
                            Espesor = item.Diametro,
                            TipoJunta = item.TipoJunta,
                            Reporte = item.Reporte,
                            Seleccionado = false,
                            Cedula = item.Cedula,
                            Cuadrante = item.Cuadrante,
                            ElementoPorClasificacionPNDID = item.ElementoPorClasificacionPNDID,
                            Especificacion = item.Especificacion,
                            JuntaSpoolID = item.JuntaSpoolID,
                            OrdenTrabajoSpoolID = item.OrdenTrabajoSpoolID,
                            Prioridad = item.Prioridad.GetValueOrDefault(),
                            ProyectoID = item.ProyectoID,
                            ReporteRequisicionID = item.ReporteRequisicionID,
                            RequisicionID = item.RequisicionID,
                            SpoolID = item.SpoolID,
                            TipoPruebaID = item.TipoPruebaID.GetValueOrDefault(),
                            Url = item.Url,
                            Version = item.Version == 0 ? "": item.Version.ToString(),
                            Accion = item.ReporteRequisicionID == 0 ? 1 : 2
                        });
                    }
                }
                return listaDetalleImpresionPruebas;
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

        public object ObtenerRequisiciones(int proyectoID, int tipoPruebaID, int proveedorID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {

                    List<Requisicion> listaRequisiciones = new List<Requisicion>();
                    List<Sam3_ST_Get_ListaRequisicionesImpresionPruebas_Result> listaRequisicionesCTX = ctx.Sam3_ST_Get_ListaRequisicionesImpresionPruebas( proyectoID, tipoPruebaID, proveedorID).ToList();
                    listaRequisiciones.Add(new Requisicion());
                    foreach (Sam3_ST_Get_ListaRequisicionesImpresionPruebas_Result item in listaRequisicionesCTX)
                    {
                        listaRequisiciones.Add(new Requisicion
                        {
                            Accion = item.RequisicionID == 0 ? 1 : 2,
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
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(ex.Message);
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;
                return result;
            }
        }

        public object InsertarCaptura(DataTable dtDetalleCaptura, Sam3_Usuario usuario, string lenguaje, int requisicionID,string nombreReporte,string fechaReporte)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    ObjetosSQL _SQL = new ObjetosSQL();
                    string[,] parametro = { { "@Usuario", usuario.UsuarioID.ToString() }, { "@Lenguaje", lenguaje }, { "@RequisicionID", requisicionID.ToString() }, { "@NombreFolio", nombreReporte }, { "@FechaReporte", fechaReporte } };
                    _SQL.Ejecuta(Stords.GUARDARCAPTURAIMPRESIONPRUEBAS, dtDetalleCaptura, "@ImpresionPruebas", parametro);
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

        public object ValidarNombre(string nombre)
        {
            try
            {
                List<string> listaDetalleImpresionPruebas = new List<string>();
                using (SamContext ctx = new SamContext())
                {

                    List<string> result = ctx.Sam3_ST_IP_Get_ValidarNombreReporte(nombre).ToList();
                    foreach (var item in result)
                    {
                        listaDetalleImpresionPruebas.Add(item);
                    }
                }
                return listaDetalleImpresionPruebas;
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