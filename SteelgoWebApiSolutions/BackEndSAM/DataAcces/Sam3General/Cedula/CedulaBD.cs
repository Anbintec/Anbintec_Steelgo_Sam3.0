using BackEndSAM.Models.Sam3General.Cedula;
using DatabaseManager.Sam3;
using SecurityManager.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.DataAcces.Sam3General.Cedula
{
    public class CedulaBD
    {
        private static readonly object _mutex = new object();
        private static CedulaBD _instance;

        public static CedulaBD Instance
        {
            get
            {
                lock (_mutex)
                {
                    if(_instance == null)
                    {
                        _instance = new CedulaBD();
                    }
                }

                return _instance;
            }
        }

        public object ObtieneListadoCedula()
        {
            try
            {
                using(SamContext ctx = new SamContext())
                {
                    List<Sam3_Steelgo_Get_Cedula_Result> result = ctx.Sam3_Steelgo_Get_Cedula().ToList();
                    List<DetalleCedula> listaDetalle = new List<DetalleCedula>();
                    listaDetalle.Add(new DetalleCedula());

                    foreach (Sam3_Steelgo_Get_Cedula_Result item in result)
                    {
                        listaDetalle.Add(new DetalleCedula {
                            CedulaID = item.CedulaID,
                            Nombre = item.Codigo
                        });
                    }

                    return listaDetalle;
                }

            }catch(Exception ex)
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