using BackEndSAM.Models.Sam3General.Consumible;
using DatabaseManager.Sam3;
using SecurityManager.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BackEndSAM.DataAcces.Sam3General.ConsumibleBD
{
    public class ConsumibleBD
    {
        private static readonly object _mutex = new object();
        private static ConsumibleBD _instance;

        public static ConsumibleBD Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new ConsumibleBD();
                    }
                }
                return _instance;
            }
        }

        public object ObtenerConsumibles(int patioID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<Consumible> ListaConsumible = (from cons in ctx.Sam3_Steelgo_Get_Consumible(null)
                                                        select new Consumible
                                                        {
                                                            Codigo = cons.Codigo,
                                                            ConsumibleID = cons.ConsumibleID
                                                        }).AsParallel().ToList().OrderBy(x => x.Codigo).ToList<Consumible>();
                    ListaConsumible.Insert(0, new Consumible());
                    return ListaConsumible;
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