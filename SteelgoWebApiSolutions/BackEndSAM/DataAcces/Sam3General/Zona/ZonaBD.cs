using BackEndSAM.Models.Sam3General.Zona;
using DatabaseManager.Sam3;
using SecurityManager.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.DataAcces.Sam3General.Zona
{
    public class ZonaBD
    {
        private static readonly object _mutex = new object();
        private static ZonaBD _instance;

        public static ZonaBD Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new ZonaBD();
                    }
                }
                return _instance;
            }
        }

        public object ObtenerZonaPorUsuario(int UsuarioID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_Steelgo_Get_ZonasUsuario_Result> result = ctx.Sam3_Steelgo_Get_ZonasUsuario(UsuarioID).ToList();
                    List<ZonaObject> listaDetalle = new List<ZonaObject>();
                    listaDetalle.Add(new ZonaObject());
                    foreach (Sam3_Steelgo_Get_ZonasUsuario_Result item in result)
                    {
                        listaDetalle.Add(new ZonaObject
                        {
                            ZonaID = item.ZonaID,
                            Nombre = item.Nombre
                        });
                    }
                    return listaDetalle.OrderBy(x => x.Nombre);

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

        public object ObtenerZonaPorPatio(int PatioID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_Steelgo_Get_ZonaByPatio_Result> result = ctx.Sam3_Steelgo_Get_ZonaByPatio(PatioID).ToList();
                    List<ZonaObject> listaDetalle = new List<ZonaObject>();
                    listaDetalle.Add(new ZonaObject());
                    foreach (Sam3_Steelgo_Get_ZonaByPatio_Result item in result)
                    {
                        listaDetalle.Add(new ZonaObject
                        {
                            ZonaID = item.ZonaID,
                            Nombre = item.Nombre
                        });
                    }
                    return listaDetalle.OrderBy(x => x.Nombre);

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