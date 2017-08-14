using BackEndSAM.Models;
using DatabaseManager.Sam3;
using SecurityManager.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.DataAcces
{
    public class MTRBd
    {
        /// <summary>
        /// operaciones sobre la entidad MTR
        /// </summary>
        private static readonly object _mutex = new object();
        private static MTRBd _instance;

        /// <summary>
        /// constructor privado para implementar el patron Singleton
        /// </summary>
        private MTRBd()
        {
        }

        /// <summary>
        /// crea una instancia de la clase
        /// </summary>
        public static MTRBd Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new MTRBd();
                    }
                }
                return _instance;
            }
        }

        public object ObtenerMTR(int ItemCodeID, string Colada, int proyectoID)
        {
            try
            {
                List<ListaCombos> lista = new List<ListaCombos>();

                using (SamContext ctx = new SamContext())
                {
                    int coladaID = (from c in ctx.Sam3_Colada
                                    where c.Activo && c.NumeroColada == Colada
                                    && c.ProyectoID == proyectoID
                                    select c.ColadaID).AsParallel().SingleOrDefault();

                    if (coladaID == null && coladaID <= 0)
                    {
                        throw new Exception("No existe la Colada especificada");
                    }
                    else
                    {
                        lista = (from m in ctx.Sam3_MTR
                                 where m.Activo && m.ItemCodeID == ItemCodeID && m.ColadaID == coladaID
                                 select new ListaCombos
                                 {
                                     id = m.MTRID.ToString(),
                                     value = m.NumeroMTR
                                 }).AsParallel().ToList();
                    }
                }
                return lista;
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