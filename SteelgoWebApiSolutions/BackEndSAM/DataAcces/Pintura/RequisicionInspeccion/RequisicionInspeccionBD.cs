using BackEndSAM.Models.Pintura.RequisicionInspeccion;
using DatabaseManager.Constantes;
using DatabaseManager.Sam3;
using SecurityManager.Api.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;

namespace BackEndSAM.DataAcces.Pintura.RequisicionInspeccion
{
    public class RequisicionInspeccionBD
    {
        private static readonly object _mutex = new object();
        private static RequisicionInspeccionBD _instance;

        public static RequisicionInspeccionBD Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new RequisicionInspeccionBD();
                    }
                }
                return _instance;
            }
        }
        public object ObtenerSpoolID(string OrdenTrabajo, string Lenguaje)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_Pintura_GET_SpoolIDRequisitado_Result> lista = ctx.Sam3_Pintura_GET_SpoolIDRequisitado(OrdenTrabajo, Lenguaje).ToList();
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

        public object ObtenerInfoPorSpool(string NumeroControl)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<InfoGrid> Info = new List<InfoGrid>();
                    List<Sam3_Pintura_GET_RequisicionInspeccionBySpool_Result> result = ctx.Sam3_Pintura_GET_RequisicionInspeccionBySpool(NumeroControl).ToList();
                    foreach (Sam3_Pintura_GET_RequisicionInspeccionBySpool_Result item in result) 
                    {
                        Info.Add(new InfoGrid {
                            Accion = item.Accion,
                            RequisicionInspeccionID = item.RequisicionInspeccionID,
                            SpoolID = item.SpoolID,
                            NumeroControl = item.NumeroControl,
                            SistemaPintura = item.SistemaPintura,
                            Color = item.Color,
                            FechaSolicitud = item.FechaSolicitud,
                            FechaInspeccion = item.FechaInspeccion,
                            Resultado = item.Resultado,
                            ModificadoPorUsuario = false
                        });
                    }
                    return Info;                    
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

        public object ObtenerSpoolsGuardados(DataTable ListaSpool)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    ObjetosSQL _Sql = new ObjetosSQL();                                        
                    DataTable Resultado = _Sql.Tabla("Sam3_Pintura_GET_InformacionRequisicionesGuardadas", ListaSpool, "@TableNumeroControl");                    
                    var Info = Resultado.AsEnumerable()                                
                                .Select(dr =>
                                    new InfoGrid
                                    {
                                        Accion = Convert.ToInt32(dr.Field<int>("Accion")),
                                        RequisicionInspeccionID = Convert.ToInt32(dr.Field<int>("RequisicionInspeccionID")),
                                        SpoolID = Convert.ToInt32(dr.Field<int>("SpoolID")),
                                        NumeroControl = dr.Field<string>("NumeroControl"),
                                        SistemaPintura = dr.Field<string>("SistemaPintura"),
                                        Color = dr.Field<string>("Color"),
                                        FechaSolicitud = dr.Field<string>("FechaSolicitud"),
                                        FechaInspeccion = dr.Field<string>("FechaInspeccion"),
                                        Resultado = dr.Field<string>("Resultado"),
                                        ModificadoPorUsuario = false
                                    }).ToList();                    
                    return Info;
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
        
        public object GuardaCapturaRequisicionInspeccion(DataTable Captura, Sam3_Usuario Usuario)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    ObjetosSQL _SQL = new ObjetosSQL();
                    string[,] Parametros =
                    {
                        { "@UsuarioID", Usuario.UsuarioID.ToString() }                        
                    };
                    _SQL.Ejecuta(Stords.GUARDARCAPTURAREQUISICIONINSPECCIONPINTURA, Captura, "@TT_ReqInsp", Parametros);
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