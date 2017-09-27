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
using static BackEndSAM.Models.Calidad.OkDocumental.OkDocumental.OkDocumental;

namespace BackEndSAM.DataAcces.Calidad.OkDocumental.OkDocumental.OkDocumental
{
    public class OkDocumentalBD
    {
        private static readonly object _mutex = new object();
        private static OkDocumentalBD _instance;

        public static OkDocumentalBD Instance
        {
            get
            {
                lock (_mutex)
                {
                    if(_instance == null)
                    {
                        _instance = new OkDocumentalBD();
                    }
                }
                return _instance;
            }
        }

        public object ObtenerNumeroElementosNombreSpool(int ProyectoID, string NombreSpool, int Muestra)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    ObjectResult<int?> elementos = ctx.Sam3_Calidad_OKDOCUMENTAL_GET_NumeroElementos(ProyectoID, NombreSpool, Muestra);
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

        public object ObtenerElementosPorNombreSpool(string Lenguaje, int ProyectoID, string NombreSpool, int Muestra)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<ElementosOK> ListaElementos = new List<ElementosOK>();
                    List<Sam3_Calidad_OKDOCUMENTAL_GET_Elementos_Result> ListaElementosOK = ctx.Sam3_Calidad_OKDOCUMENTAL_GET_Elementos(ProyectoID, NombreSpool, Muestra).ToList();
                    foreach (Sam3_Calidad_OKDOCUMENTAL_GET_Elementos_Result item in ListaElementosOK)
                    {
                        ListaElementos.Add(new ElementosOK
                        {
                            SpoolWorkStatusID = item.SpoolWorkStatusID,
                            NombreSpool = item.NombreSpool,
                            Cuadrante = item.Cuadrante,
                            Prioridad = item.Prioridad.GetValueOrDefault(),
                            ProyectoID = item.ProyectoID,
                            SpoolID = item.SpoolID,
                            OrdenTrabajoSpoolID = item.OrdenTrabajoSpoolID,
                            OkDocumental = item.OkDocumental,
                            ModificadoPorUsuario = false,
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

        public object ObtenerListaElementosPorOrdenTrabajo(string Lenguaje, int ProyectoID, string OrdenTrabajo, int Muestra)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<ElementosOK> ListaElementos = new List<ElementosOK>();
                    List<Sam3_Calidad_OKDOCUMENTAL_GET_ElementosXSpoolID_Result> ListaElementosOK = ctx.Sam3_Calidad_OKDOCUMENTAL_GET_ElementosXSpoolID(ProyectoID, OrdenTrabajo, Muestra).ToList();
                    foreach (Sam3_Calidad_OKDOCUMENTAL_GET_ElementosXSpoolID_Result item in ListaElementosOK)
                    {
                        ListaElementos.Add(new ElementosOK
                        {
                            SpoolWorkStatusID = item.SpoolWorkStatusID,
                            NombreSpool = item.NombreSpool,
                            Cuadrante = item.Cuadrante,
                            Prioridad = item.Prioridad.GetValueOrDefault(),
                            ProyectoID = item.ProyectoID,
                            SpoolID = item.SpoolID,
                            OrdenTrabajoSpoolID = item.OrdenTrabajoSpoolID,
                            OkDocumental = item.OkDocumental,
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
        
        public object GuardaCaptura(DataTable Datos, string Lenguaje, Sam3_Usuario Usuario, int ProyectoID)
        {
            try
            {
                using(SamContext ctx = new SamContext())
                {
                    ObjetosSQL _SQL = new ObjetosSQL();
                    string[,] Parametros = 
                    {
                        {"@UsuarioID", Usuario.UsuarioID.ToString() },
                        {"@Lenguaje", Lenguaje },
                        {"@ProyectoID", ProyectoID.ToString() }
                    };
                    _SQL.Ejecuta(Stords.CALIDAD_OKDOCUMENTAL_GUARDACAPTURA, Datos, "@TTOK", Parametros);
                    TransactionalInformation result = new TransactionalInformation();
                    result.ReturnMessage.Add("OK");
                    result.ReturnCode = 200;
                    result.ReturnStatus = true;
                    result.IsAuthenicated = true;
                    return result;
                }
            }
            catch (Exception e)
            {
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(e.Message);
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;
                return result;
            }
        }

        public object GuardaOKMasivo(DataTable datos, int ProyectoID, string Lenguaje, Sam3_Usuario Usuario)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    ObjetosSQL _SQL = new ObjetosSQL();
                    string[,] Parametros =
                    {
                        { "@ProyectoID", ProyectoID.ToString() },
                        { "@UsuarioID", Usuario.UsuarioID.ToString() },
                        { "@Lenguaje", Lenguaje }
                    };
                    DataTable Respuesta = _SQL.EjecutaDataAdapter(Stords.CALIDAD_OKDOCUMENTAL_GUARDAMASIVO, datos, "@TTOK", Parametros);                    
                    return ToDataTable.table_to_csv(Respuesta);
                }
            }
            catch (Exception e)
            {
                //-----------------Agregar mensaje al Log -----------------------------------------------
                LoggerBd.Instance.EscribirLog(e);
                //-----------------Agregar mensaje al Log -----------------------------------------------
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(e.Message);
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;

                return result;
            }
        }

    }
}