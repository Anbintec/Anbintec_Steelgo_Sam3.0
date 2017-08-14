using BackEndSAM.Models;
using DatabaseManager.Sam3;
using SecurityManager.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.DataAcces
{
    public class FabricantesBd
    {
        private static readonly object _mutex = new object();
         private static FabricantesBd _instance;

        /// <summary>
        /// constructor privado para implementar el patron Singleton
        /// </summary>
         private FabricantesBd()
        {
        }

        /// <summary>
        /// crea una instancia de la clase
        /// </summary>
         public static FabricantesBd Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new FabricantesBd();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Se obtienen todos los fabricantes 
        /// </summary>
        /// <returns>lista de fabricantes</returns>
         public object ObtenerFabricantes()
         {
             try 
             {
                 List<Fabricante> fabricante = new List<Fabricante>();

                 using (SamContext ctx = new SamContext())
                 {
                     fabricante = (from f in ctx.Sam3_Fabricante
                                   where f.Activo
                                   select new Fabricante
                                   {
                                       FabricanteID = f.FabricanteID,
                                       Nombre = f.Nombre
                                   }).AsParallel().ToList();

                     return fabricante;
                 }
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