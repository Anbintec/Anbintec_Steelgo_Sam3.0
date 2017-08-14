using BackEndSAM.Models;
using DatabaseManager.Sam3;
using SecurityManager.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.DataAcces
{
    public class ValidarCedulasBd
    {
        private static readonly object _mutex = new object();
        private static ValidarCedulasBd _instance;

        /// <summary>
        /// constructor privado para implementar el patron Singleton
        /// </summary>
        private ValidarCedulasBd()
        {
        }

        /// <summary>
        /// crea una instancia de la clase
        /// </summary>
        public static ValidarCedulasBd Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new ValidarCedulasBd();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Obtiene los espesores en MM o IN disponibles cuando Cedula A B y C son vacias
        /// En el catalogo de Item code steelgo
        /// </summary>
        /// <param name="diametroID"> diametro seleccionado </param>
        /// <returns>lista de espesores</returns>
        public object ValidarEspesoresMMCedulasVacias(int diametroID, bool espesorIN)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<ListaDiametros> espesores = (from cat in ctx.Sam3_CatalogoCedulas
                                                      where cat.Activo && cat.CedulaA == 0 && cat.CedulaB == 0 && cat.CedulaC == 0 && cat.DiametroID == diametroID
                                                      select new ListaDiametros
                                                      {
                                                          id = cat.CatalogoCedulasID,
                                                          value = espesorIN ? cat.EspesorIn : cat.EspesorMM
                                                      }).AsParallel().GroupBy(x=> x.value).Select(x=> x.First()).ToList();

                    List<ListaCombos> lista = (from es in espesores.OrderBy(x => x.value)
                                               select new ListaCombos
                                               {
                                                   id = es.id.ToString(),
                                                   value = es.value.ToString()
                                               }).AsParallel().ToList();
                    return lista;
                }
            }
            catch (Exception ex)
            {
                //-----------------Agregar mensaje al Log -----------------------------------------------
                LoggerBd.Instance.EscribirLog(ex);
                //-----------------Agregar mensaje al Log -----------------------------------------------
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;

                return result;
            }
        }
    }
}