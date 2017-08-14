using BackEndSAM.Models;
using DatabaseManager.Sam3;
using SecurityManager.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.DataAcces
{
    public class FamiliaBd
    {
         private static readonly object _mutex = new object();
         private static FamiliaBd _instance;

        /// <summary>
        /// constructor privado para implementar el patron Singleton
        /// </summary>
         private FamiliaBd()
        {
        }

        /// <summary>
        /// crea una instancia de la clase
        /// </summary>
         public static FamiliaBd Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new FamiliaBd();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Obtiene un listado para combo con las familias de acero disponibles
        /// </summary>
        /// <returns></returns>
        public object obtenerFamilia()
        {
            try
            {
                List<Familia> familia = new List<Familia>();

                using (SamContext ctx = new SamContext())
                {
                    familia = (from f in ctx.Sam3_FamiliaAcero
                               where f.Activo
                               select new Familia
                               {
                                   FamiliaAceroID = f.FamiliaAceroID.ToString(),
                                   Nombre = f.Nombre
                               }).AsParallel().ToList();
                }
                return familia;
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