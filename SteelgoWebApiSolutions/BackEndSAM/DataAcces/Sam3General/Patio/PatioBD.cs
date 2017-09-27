using BackEndSAM.Models.Sam3General.Patios;
using DatabaseManager.Sam3;
using SecurityManager.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BackEndSAM.DataAcces.Sam3General.Patio
{
    public class PatioBD
    {
        private static readonly object _mutex = new object();
        private static PatioBD _instance;

        public static PatioBD Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new PatioBD();
                    }
                }
                return _instance;
            }
        }

        public object ObtenerPatioPorUsuario(int UsuarioID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_Steelgo_Get_PatiosPorUsuario_Result> result = ctx.Sam3_Steelgo_Get_PatiosPorUsuario(UsuarioID).ToList();
                    List<PatioObject> listaDetalle = new List<PatioObject>();
                    listaDetalle.Add(new PatioObject());
                    foreach (Sam3_Steelgo_Get_PatiosPorUsuario_Result item in result)
                    {
                        listaDetalle.Add(new PatioObject
                        {
                            PatioID = item.PatioID,
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