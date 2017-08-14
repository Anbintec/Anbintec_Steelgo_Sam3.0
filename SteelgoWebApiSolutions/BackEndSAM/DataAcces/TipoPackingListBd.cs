using SecurityManager.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BackEndSAM.Models;
using DatabaseManager.Sam3;

namespace BackEndSAM.DataAcces
{
    public class TipoPackingListBd
    {
          private static readonly object _mutex = new object();
         private static TipoPackingListBd _instance;

        /// <summary>
        /// constructor privado para implementar el patron Singleton
        /// </summary>
         private TipoPackingListBd()
        {
        }

        /// <summary>
        /// crea una instancia de la clase
        /// </summary>
         public static TipoPackingListBd Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new TipoPackingListBd();
                    }
                }
                return _instance;
            }
        }

         /// <summary>
         /// Obtener los tipo packing list (Tipo Material)
         /// </summary>
         /// <returns>Lista con los tipos de PL</returns>
         public object ObtenerTipoPackingList()
         {
             try
             {
                 List<TipoPackingList> PL = new List<TipoPackingList>();

                 using (SamContext ctx = new SamContext())
                 {
                     PL = (from m in ctx.Sam3_TipoMaterial
                           where m.Activo
                           select new TipoPackingList
                           {
                               id = m.TipoMaterialID.ToString(),
                               Nombre = m.Nombre
                           }).AsParallel().ToList();
                 }
                 return PL;

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
        /// <summary>
        /// Obtener los tipo packing list (Tipo Material)
        /// </summary>
        /// <returns>Lista con los tipos de PL</returns>
         public object ObtenerTipoPackingListOpcionTodos(string idioma)
         {
             try
             {
                 List<TipoPackingList> PL = new List<TipoPackingList>();
                 if (idioma == "en-US") {
                     PL.Add(new TipoPackingList { Nombre = "All", id = "0" });
                 }
                 else {
                     PL.Add(new TipoPackingList { Nombre = "Todos", id = "0" });
                 }

                 using (SamContext ctx = new SamContext())
                 {
                     PL.AddRange((from m in ctx.Sam3_TipoMaterial
                                  where m.Activo
                                  select new TipoPackingList
                                  {
                                      id = m.TipoMaterialID.ToString(),
                                      Nombre = m.Nombre
                                  }).AsParallel().ToList());
                 }
                 return PL;

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