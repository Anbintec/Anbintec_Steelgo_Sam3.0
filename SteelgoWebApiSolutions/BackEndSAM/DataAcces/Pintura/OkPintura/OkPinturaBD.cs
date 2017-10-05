using BackEndSAM.Utilities.ConvertirDataTable;
using DatabaseManager.Constantes;
using DatabaseManager.Sam3;
using SecurityManager.Api.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using static BackEndSAM.Models.Pintura.OK.OkPintura;

namespace BackEndSAM.DataAcces.Pintura.OkPintura
{
    public class OkPinturaBD
    {
        private static readonly object _mutex = new object();
        private static OkPinturaBD _Instance;
        public static OkPinturaBD Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_Instance == null)
                    {
                        _Instance = new OkPinturaBD();
                    }
                }
                return _Instance;
            }
        }
        public object NumControlExisteEnProyecto(int ProyectoID, string NumControl)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    ObjectResult<int?> result = ctx.Sam3_Pintura_CheckNumControl(ProyectoID, NumControl);                    
                    var existe = result.Where(x => x.HasValue).Select(x => x.Value).ToList()[0];
                    return existe;
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

        public object ObtenerNumeroElementosOkPintura(int ProyectoID, string NumControl, int Muestra)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    ObjectResult<int?> elementos = ctx.Sam3_Pintura_GET_NumeroElementosOkPintura(ProyectoID, NumControl, Muestra);
                    var valor = elementos.Where(x => x.HasValue).Select(x => x.Value).ToList()[0];
                    return valor;                    
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

        public object ObtenerListaElementosOkPintura(string Lenguaje, int ProyectoID, string NumControl, int Muestra)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<ElementosOK> ListaElementos = new List<ElementosOK>();
                    List<Sam3_Pintura_GET_ElementosOKPintura_Result> ListaElementosOK = ctx.Sam3_Pintura_GET_ElementosOKPintura(ProyectoID, NumControl, Muestra).ToList();
                    foreach (Sam3_Pintura_GET_ElementosOKPintura_Result item in ListaElementosOK)
                    {
                        ListaElementos.Add(new ElementosOK
                        {
                            SpoolWorkStatusID = item.SpoolWorkStatusID.GetValueOrDefault(),
                            NumeroControl = item.NumeroControl,
                            Cuadrante = item.Cuadrante,
                            Prioridad = item.Prioridad.GetValueOrDefault(),
                            SpoolID = item.SpoolID.GetValueOrDefault(),
                            OrdenTrabajoSpoolID = item.OrdenTrabajoSpoolID.GetValueOrDefault(),                            
                            ProyectoID = item.ProyectoID.GetValueOrDefault(),
                            SHOTID = item.SHOTID.GetValueOrDefault(),
                            PRIMID = item.PRIMID.GetValueOrDefault(),
                            INTERID = item.INTERID.GetValueOrDefault(),
                            ACABID = item.ACABID.GetValueOrDefault(),
                            FechaProcesoSHOT = item.FechaProcesoSHOT,
                            FechaProcesoPRIM = item.FechaProcesoPRIM,
                            FechaProcesoINTER = item.FechaProcesoINTER,
                            FechaProcesoACAB = item.FechaProcesoACAB,
                            OkPintura = item.OkPintura.GetValueOrDefault(),
                            Detalle = Lenguaje == "es-MX" ? "Ver Detalle" : "See Details"
                        });
                    }
                    return ListaElementos;
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


        public List<Detalle> ObtenerDetallePruebas(string Lenguaje, int SpoolID, int ProcesoID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<Detalle> ListaPruebas = new List<Detalle>();
                    List<Sam3_Pintura_GET_DetallePruebas_Result> Lista = ctx.Sam3_Pintura_GET_DetallePruebas(SpoolID, ProcesoID, Lenguaje).ToList();
                    foreach (Sam3_Pintura_GET_DetallePruebas_Result item in Lista)
                    {
                        ListaPruebas.Add(new Detalle
                        {
                            Prueba = item.Prueba,
                            UnidadMedida = item.UnidadMedida,
                            UnidadMinima = item.UnidadMinima,
                            UnidadMaxima = item.UnidadMaxima
                        });
                    }
                    return ListaPruebas;
                }
            }
            catch (Exception ex)
            {
                List<Detalle> result = null;
                return result;
            }
        }
        public object GuardaCaptura(DataTable dtLista, string Lenguaje, Sam3_Usuario Usuario, int ProyectoID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    ObjetosSQL _SQL = new ObjetosSQL();
                    string[,] Parametros =
                    {
                        { "@UsuarioID", Usuario.UsuarioID.ToString() },
                        { "@Lenguaje",  Lenguaje },
                        { "@ProyectoID", ProyectoID.ToString() }                        
                    };
                    _SQL.Ejecuta(Stords.PINTURA_SET_OKPINTURA, dtLista, "@TTOK", Parametros);
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

        public object GuardaCapturaMasiva(DataTable Data, int ProyectoID, string lenguaje, Sam3_Usuario usuario)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    object res = new object();
                    ObjetosSQL _SQL = new ObjetosSQL();
                    string[,] parametro = {
                        { "@ProyectoID", ProyectoID.ToString()},
                        { "@UsuarioID", usuario.UsuarioID.ToString()},
                        { "@Lenguaje", lenguaje }
                    };
                    DataTable OKPND = _SQL.EjecutaDataAdapter(Stords.PINTURA_SET_OKPINTURA_MASIVO, Data, "@TTOK", parametro);
                    return ToDataTable.table_to_csv(OKPND);
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
    }    
}
