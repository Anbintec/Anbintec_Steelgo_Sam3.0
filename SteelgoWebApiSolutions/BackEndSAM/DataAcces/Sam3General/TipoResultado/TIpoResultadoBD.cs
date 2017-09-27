using DatabaseManager.Sam3;
using SecurityManager.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.DataAcces.Sam3General.TipoResultado
{
    public class TipoResultadoBd
    {
        private static readonly object _mutex = new object();
        private static TipoResultadoBd _instance;

        /// <summary>
        /// constructor privado para implementar el patron Singleton
        /// </summary>
        private TipoResultadoBd()
        {
        }

        /// <summary>
        /// crea una instancia de la clase
        /// </summary>
        public static TipoResultadoBd Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new TipoResultadoBd();
                    }
                }
                return _instance;
            }
        }
        public object ObtenerListadoResultados(string Lenguaje)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_Steelgo_Get_TipoResultado_Result> lista = ctx.Sam3_Steelgo_Get_TipoResultado(Lenguaje).ToList();
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