using DatabaseManager.Sam3;
using SecurityManager.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BackEndSAM.DataAcces
{
    public class TallerBD
    {
        private static readonly object _mutex = new object();
        private static TallerBD _instance;

        /// <summary>
        /// constructor privado para implementar el patron Singleton
        /// </summary>
        public TallerBD()
        {

        }

        /// <summary>
        /// crea una instancia de la clase
        /// </summary>
        public static TallerBD Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new TallerBD();
                    }
                }
                return _instance;
            }
        }
        /// <summary>
        /// obtiene el listado de talleres por proyecto
        /// </summary>
        /// <param name="idProyecto">Identificador del Proyecto</param>
        /// <returns>Lista de proyectos</returns>
        public object ObtenerTallerXPoryecto(int idProyecto)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_SteelGo_Get_Taller_Result> lista = ctx.Sam3_SteelGo_Get_Taller(idProyecto).ToList();
                    lista.Insert(0, new Sam3_SteelGo_Get_Taller_Result());
                    lista[0].Nombre = "";
                    return lista;
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