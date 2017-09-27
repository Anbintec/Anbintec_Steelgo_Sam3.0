using BackEndSAM.Models.Sam3General.TipoJunta;
using DatabaseManager.Sam3;
using SecurityManager.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.DataAcces.Sam3General.TipoJunta
{
    public class TipoJuntaBD
    {
        private static readonly object _mutex = new object();
        private static TipoJuntaBD _instance;

        public static TipoJuntaBD Instance {
            get
            {
                lock (_mutex)
                {
                    if(_instance == null)
                    {
                        _instance = new TipoJuntaBD();
                    }
                }

                return _instance;
            }
        }

        public object ObtieneListadoTipoJunta()
        {
            try
            {
                using(SamContext ctx = new SamContext())
                {
                    List<Sam3_Steelgo_Get_TipoJunta_Result> result = ctx.Sam3_Steelgo_Get_TipoJunta().ToList();
                    List<DetalleTipoJunta> listaDetalle = new List<DetalleTipoJunta>();
                    listaDetalle.Add(new DetalleTipoJunta());

                    foreach (Sam3_Steelgo_Get_TipoJunta_Result item in result)
                    {
                        listaDetalle.Add(new DetalleTipoJunta {
                            TipoJuntaID = item.TipoJuntaID,
                            Nombre = item.Nombre
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