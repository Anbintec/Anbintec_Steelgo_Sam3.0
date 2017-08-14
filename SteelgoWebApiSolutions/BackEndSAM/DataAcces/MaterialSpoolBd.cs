using System;
using System.Collections.Generic;
using System.Linq;
using DatabaseManager.Sam2;
using DatabaseManager.Sam3;
using BackEndSAM.Models;
using SecurityManager.Api.Models;
using System.Web.Script.Serialization;

namespace BackEndSAM.DataAcces
{
    public class MaterialSpoolBd
    {
        private static readonly object _mutex = new object();
        private static MaterialSpoolBd _instance;

        /// <summary>
        /// constructor privado para implementar el patron Singleton
        /// </summary>
        private MaterialSpoolBd()
        {
        }

        /// <summary>
        /// crea una instancia de la clase
        /// </summary>
        public static MaterialSpoolBd Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new MaterialSpoolBd();
                    }
                }
                return _instance;
            }
        }

        public object ObtnerEtiquetasMaterialPorODTS(string odtsm, Sam3_Usuario usuario)
        {
            try
            {
                int odtsID = string.IsNullOrEmpty(odtsm) ? 0 : Convert.ToInt32(odtsm);
                List<ListaCombos> listado = new List<ListaCombos>();
                using (Sam2Context ctx2 = new Sam2Context())
                {

                    listado = (from ms in ctx2.MaterialSpool
                               where ms.SpoolID == (from odts in ctx2.OrdenTrabajoSpool
                                                    where odts.OrdenTrabajoSpoolID == odtsID
                                                    select odts.SpoolID).FirstOrDefault()
                               select new ListaCombos
                               {
                                   id = ms.MaterialSpoolID.ToString(),
                                   value = ms.Etiqueta
                               }).AsParallel().Distinct().OrderBy(x => x.value).ToList();
                }
                return listado;
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