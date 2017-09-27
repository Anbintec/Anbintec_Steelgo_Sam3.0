using BackEndSAM.Models.Sam3General.FamiliaAcero;
using DatabaseManager.Sam3;
using SecurityManager.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.DataAcces.Sam3General.FamiliaAcero
{
    public class FamiliaAceroBD
    {
        private static readonly object _mutex = new object();
        private static FamiliaAceroBD _instance;

        public static FamiliaAceroBD Instance
        {
            get
            {
                lock (_mutex)
                {
                    if(_instance == null)
                    {
                        _instance = new FamiliaAceroBD();
                    }
                }

                return _instance;
            }
        }

        public object ObtieneListadoFamiliaAcero()
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_Steelgo_Get_Acero_Result> result = ctx.Sam3_Steelgo_Get_Acero().ToList();
                    List<DetalleFamiliaAcero> listaDetalle = new List<DetalleFamiliaAcero>();
                    listaDetalle.Add(new DetalleFamiliaAcero());

                    foreach (Sam3_Steelgo_Get_Acero_Result item in result)
                    {
                        listaDetalle.Add(new DetalleFamiliaAcero {
                            FamiliaAceroID = item.FamiliaAceroID,
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